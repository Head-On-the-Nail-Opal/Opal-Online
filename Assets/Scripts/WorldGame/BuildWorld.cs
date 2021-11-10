using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BuildWorld : MonoBehaviour
{
    private Dictionary<string, TileCode> getTileByCode;
    private Dictionary<string, bool> getPassable;
    private Dictionary<string, bool> getEncounter;
    private List<TileCode> allTiles = new List<TileCode>();
    private List<OpalScript> opalPrefabs = new List<OpalScript>();
    private List<List<TileCode>> patches = new List<List<TileCode>>();
    private List<OpalHolder> patchInfo = new List<OpalHolder>();
    public List<string> opalSpawns;
    public List<List<OpalHolder>> newSpawns = new List<List<OpalHolder>>();
    private float tilesize = 1f;
    public Player pl;
    private Vector3 lastPosition;
    private TileCode[,] checkStanding;
    private Vector2 mapSize;
    private TileCode currentTile;
    private TileCode lastTile;
    public OpalLogger oL;

    private void Awake()
    {
        pl.setBuildWorld(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (OpalScript o in Resources.LoadAll<OpalScript>("Prefabs/Opals/"))
        {
            opalPrefabs.Add(o);
        }
        foreach (string s in opalSpawns)
        {
            if (s != null)
            {
                string[] temps = s.Split(' ');
                OpalHolder o = new OpalHolder();
                //print(s);
                o.set(temps[0], temps[1], temps[2]);
                patchInfo.Add(o);
            }
        }
        getTileByCode = new Dictionary<string, TileCode>() {
            { "_", null },
            { "F", Resources.Load<TileCode>("Prefabs/WorldTiles/ForestB") },
            { "f", Resources.Load<TileCode>("Prefabs/WorldTiles/ForestA") },
            { "g", Resources.Load<TileCode>("Prefabs/WorldTiles/Grass")},
            { "G", Resources.Load<TileCode>("Prefabs/WorldTiles/TallGrass")},
            { "B", Resources.Load<TileCode>("Prefabs/WorldTiles/Bushes")},
            { "W", Resources.Load<TileCode>("Prefabs/WorldTiles/Water")},
            { "S", Resources.Load<TileCode>("Prefabs/WorldTiles/StoneWall") },
            { "C", Resources.Load<TileCode>("Prefabs/WorldTiles/RockyFloor")},
            { "r", Resources.Load<TileCode>("Prefabs/WorldTiles/FlowerGrassA")},
            { "R", Resources.Load<TileCode>("Prefabs/WorldTiles/FlowerGrassB")},
            { "N", Resources.Load<TileCode>("Prefabs/WorldTiles/NPCTile")},
            {"O", Resources.Load<TileCode>("Prefabs/WorldTiles/Ore")} };
        getPassable = new Dictionary<string, bool>() {
            { "_", true },
            { "F", false },
            { "f", false},
            { "g", true},
            { "G", true},
            { "B", false},
            { "W", true},
            { "S", false },
            { "C", true},
            { "R", true},
            { "r", true},
            { "N", false},
            {"O", false} };
        getEncounter = new Dictionary<string, bool>() {
            { "_", false },
            { "F", false },
            { "f", false},
            { "g", false},
            { "G", true},
            { "B", false},
            { "W", true},
            { "S", false },
            { "C", true},
            { "R", true},
            { "r", false},
            { "N", false},
            {"O", false } };
        pl.sendBumpCodes(getPassable);
        build();
        //createWalls();
        createEncounterAreas();
    }

    
    // Update is called once per frame
    void Update()
    {
        if(pl.transform.position != lastPosition)
        {
            lastPosition = pl.transform.position;
            getCurrentTile();
            if (currentTile != lastTile)
            {
                //checkSpawn();
                lastTile = currentTile;
            }
        }
    }

    private void build()
    {
        string path = "Assets/StreamingAssets/map.txt";
        string map = "";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        bool getSize = true;
        string temp = reader.ReadLine();
        while (temp != "endmap")
        {
            if (getSize)
            {
                string[] dims = temp.Split('-');
                mapSize = new Vector2(int.Parse(dims[0]), int.Parse(dims[1]));
                getSize = false;
            }
            else
            {
                map += temp + "-";
            }
            temp = reader.ReadLine();
        }
        while (!reader.EndOfStream)
        {
            temp = reader.ReadLine();
            string[] temps = temp.Split(' ');
            int index = 0;
            for (int num = 0; num < temps.Length; num++)
            {
                if (num == 0)
                {
                    index = int.Parse(temps[num]);
                    newSpawns.Add(new List<OpalHolder>());
                }
                else
                {
                    OpalHolder tempHolder = new OpalHolder();
                    tempHolder.set(temps[num].Split('-')[0], index+"",temps[num].Split('-')[1]);
                    newSpawns[index].Add(tempHolder);
                }
            }
        }
        print("Map size is " + mapSize.x + ", " + mapSize.y);
        checkStanding = new TileCode[(int)mapSize.x, (int)mapSize.y];
        reader.Close();
        string[] splitter = map.Split('-');
        List<string[]> makeMap = new List<string[]>();
        foreach (string s in splitter)
        {
            string[] splitter2 = s.Split(' ');
            makeMap.Add(splitter2);
        }
        int i = (int)mapSize.y-1;
        foreach (string[] ss in makeMap)
        {
            int j = 0;
            foreach (string s in ss)
            {
                //print("|"+s+"|");
                if (s != "" && s != " ")
                {

                    if (s == "___")
                    {
                        j++;
                    }
                    else
                    {
                        TileCode tc = Instantiate<TileCode>(getTileByCode[s.Substring(0,1)]);
                        tc.setSecondaryCode(s.Substring(1, 2));
                        tc.transform.position = new Vector3(j*tilesize, i*tilesize, 19);
                        tc.transform.localScale *= 1/8.3f;
                        allTiles.Add(tc);
                        checkStanding[j,i] = tc;
                        tc.gameObject.SetActive(false);
                        j++;
                    }
                }
            }
            i--;
        }
        pl.sendMap(checkStanding);
    }

    private void createWalls()
    {
        int index = 0;
        foreach(TileCode tc in allTiles)
        {
            if (!getPassable[tc.getCode()])
            {
                if (index != 0 && !getPassable[allTiles[index - 1].getCode()] && (allTiles[index - 1].GetComponent<BoxCollider2D>() != null || allTiles[index - 1].getBorrowing() != null) && allTiles[index - 1].transform.position.y == allTiles[index].transform.position.y)
                {
                    BoxCollider2D bc;
                    if (allTiles[index - 1].GetComponent<BoxCollider2D>() != null)
                    {
                        bc = allTiles[index - 1].GetComponent<BoxCollider2D>();
                    }
                    else
                    {
                        bc = allTiles[index - 1].getBorrowing();
                    }
                    bc.size = new Vector2(bc.size.x + tilesize, bc.size.y);
                    bc.offset = new Vector2(bc.offset.x + tilesize/2, bc.offset.y);
                    allTiles[index].setBorrowing(bc);
                }
                else
                {
                    tc.gameObject.AddComponent<BoxCollider2D>();
                }
            }
            index++;
        }
    }

    private void createEncounterAreas()
    {
        int encounterArea = 0;
        for(int i = 0; i < checkStanding.GetLength(0); i++)
        {
            for (int j = 0; j < checkStanding.GetLength(1); j++)
            {
                //print("uuuhhh: " + i + "  " + j);
                if (checkStanding[i,j] != null && getEncounter[checkStanding[i, j].baseCode] && checkStanding[i, j].getAssigned() == -1)
                {
                    patches.Add(new List<TileCode>());
                    infect(i, j, encounterArea);
                    encounterArea++;
                }
            }
        }
        print("Number of encounter areas:  " + encounterArea);
    }

    private void infect(int x, int y, int eA)
    {
        checkStanding[x, y].setAssigned(eA);
        patches[eA].Add(checkStanding[x, y]);
        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //print("du hello: " + x + i + "  " + y + j);
                if (x + i < 0 || x + i > checkStanding.GetLength(0) - 1 || y + j < 0 || y + j > checkStanding.GetLength(1) - 1 || checkStanding[x + i, y + j] == null)
                {

                }
                else if(getEncounter[checkStanding[x+i, y+j].baseCode] && checkStanding[x + i, y + j].getAssigned() == -1 && checkStanding[x + i, y + j].baseCode == checkStanding[x, y].baseCode)
                {
                    infect(x + i, y + j, eA);
                }
            }
        }
    }

    private void getCurrentTile()
    {
        //need to offset these values based on the offset of the map
        int xpos = (int)(((int)(pl.transform.position.x) / tilesize) + 0.5f);
        int ypos = (int)(((int)(pl.transform.position.y) / tilesize) - 1.5f);
        currentTile = checkStanding[Mathf.RoundToInt(pl.transform.position.x),Mathf.RoundToInt(pl.transform.position.y)];
        //pl.sendMapPosition(new Vector2(xpos, (int)(mapSize.y - ypos)));
        //print(currentTile.getCode());
        //print(xpos + " and "+ ypos);
    }

    public void checkSpawn(TileCode current)
    {
        if (current.getSecondary() != "__" && currentTile != null && currentTile.getSecondary() != "__")
        {
            if((current.getPos().x < currentTile.getPos().x + 4 && current.getPos().x > currentTile.getPos().x - 4) && (current.getPos().y < currentTile.getPos().y + 4 && current.getPos().y > currentTile.getPos().y - 4))
            {
                if (current.getSecondary() == currentTile.getSecondary())
                {
                    if (Random.Range(1, 100) == 4)
                    {
                        doSpawnOpal(int.Parse(current.getSecondary()), current);
                    }
                }
            }
            //else if(current.getSecondary() != currentTile.getSecondary())
            //{
            //    if (Random.Range(1, 200) == 4)
             //   {
             //       doSpawnOpal(int.Parse(current.getSecondary()), current);
             //   }
            //}
        }
    }

    private void doSpawnOpal(int index, TileCode tc)
    {
        int i = Random.Range(0, 100);
        OpalScript opal = null;
        foreach(OpalHolder o in newSpawns[index])
        {
            if(i < o.getFrequency())
            {
                opal = o.getOpal();
                break;
            }
            else
            {
                i -= o.getFrequency();
            }
        }
        if (opal == null)
        {
            print("This opal doesn't exist");
        }
        else
        {
            opal = Instantiate<OpalScript>(opal, this.transform);
            opal.setOpal(null);
            opal.transform.localPosition = new Vector3(tc.getPos().x, tc.getPos().y, 18);

            opal.transform.rotation = Quaternion.Euler(0, 0, 0);
            opal.transform.localScale *= 1;
            //Destroy(opal.GetComponent<Rigidbody2D>());
            opal.gameObject.AddComponent<RoamOpal>();
            opal.GetComponent<RoamOpal>().setMap(checkStanding, getPassable);
            opal.GetComponent<RoamOpal>().addOpalLogger(oL);
            //opal.GetComponent<RoamOpal>().setGridPos(calculateGridPos(opal));
        }
    }

    private Vector2 calculateGridPos(OpalScript o)
    {
        Vector2 output = new Vector2();
        int xpos = (int)(((int)(o.transform.position.x) / tilesize));
        int ypos = (int)(((int)(o.transform.position.y) / tilesize));
        currentTile = checkStanding[xpos, (int)(mapSize.y - ypos)];
        output = new Vector2(xpos, (int)(mapSize.y - ypos));
        return output;
    }
}

public class OpalHolder
{
    OpalScript opal;
    int region;
    int frequency;

    public void set(string o, string r, string f)
    {

        opal = Resources.Load<OpalScript>("Prefabs/Opals/"+o);
        region = int.Parse(r);
        frequency = int.Parse(f);
    }

    public OpalScript getOpal()
    {
        return opal;
    }

    public int getRegion()
    {
        return region;
    }

    public int getFrequency()
    {
        return frequency;
    }
}