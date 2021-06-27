using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MainBuilder : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    private List<TileCode> tiles = new List<TileCode>();
    private Camera me;
    private int currentTile = 0;
    private TileCode tileInst;
    private List<TileCode> allTiles = new List<TileCode>();
    private Dictionary<string, TileCode> getTileByCode;
    public TextAsset mapFile;
    private Vector2 dim = new Vector2();
    private List<List<string>> opalSpawns = new List<List<string>>();
    private int currentSpawn = 0;
    private Vector3 lastPos;
    private int lastAction = -1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        TileCode[] t = Resources.LoadAll<TileCode>("Prefabs/WorldTiles");

        //opalSpawns.Add(new List<string>() {"Ambush-33", "Succuum-33", "Slungus-33" });
        //opalSpawns.Add(new List<string>() { "Mechalodon-25", "Strikel-25", "Hearthhog-50" });
        me = Camera.main;
        foreach(TileCode temp in t)
        {
            tiles.Add(temp);
        }
        tileInst = Instantiate<TileCode>(tiles[currentTile]);
        tileInst.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
        tileInst.transform.localScale = new Vector3(6.4f, 6.4f, 1.001f);
        if (tileInst.baseCode == "!")
        {
            tileInst.transform.localScale = new Vector3(0.5f, 0.5f, 1.001f);
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
            {"O", Resources.Load<TileCode>("Prefabs/WorldTiles/Ore") } };
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = doMousePos();
        moveMe();
        Vector3 roundedPos = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
        tileInst.transform.position = roundedPos;
        if (Input.GetMouseButton(0) && (roundedPos != lastPos || lastAction != 0))
        {
            lastPos = roundedPos;
            lastAction = 0;
            int index = -1;
            for (int i = 0; i < allTiles.Count; i++)
            {
                if (allTiles[i] != null && allTiles[i].getPos() == tileInst.getPos())
                {
                    index = i;
                    break;
                }
            }
            if (tileInst.baseCode != "!")
            {
                if (index != -1)
                {
                    TileCode timp = allTiles[index];
                    allTiles[index] = null;
                    DestroyImmediate(timp.gameObject);
                }
                TileCode temp = Instantiate<TileCode>(tileInst);
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                temp.transform.position = new Vector3((int)tileInst.transform.position.x, (int)tileInst.transform.position.y, 1);
                allTiles.Add(temp);
            }
            else
            {
                if (index != -1)
                {
                    TileCode timp = allTiles[index];
                    addSpawnCode(timp, index);
                }
            }

        }
        if (Input.GetMouseButton(1) && (roundedPos != lastPos || lastAction != 1))
        {
            lastPos = roundedPos;
            lastAction = 1;
            int index = -1;
            for(int i = 0; i < allTiles.Count; i++)
            {
                if(allTiles[i] != null && allTiles[i].getPos() == tileInst.getPos())
                {
                    index = i;
                    break;
                }
            }
            if (tileInst.baseCode == "!")
            {
                if (index != -1)
                {
                    TileCode timp = allTiles[index];
                    addSpawnCode(timp, index);
                    proliferate(timp, index);
                }
            }
            else
            {
                if (index != -1)
                {
                    TileCode temp = allTiles[index];
                    allTiles[index] = null;
                    DestroyImmediate(temp.gameObject);
                }
            }
        }
        manageTile();
        if (Input.GetKeyUp(KeyCode.RightShift))
        {
            cleanAllTiles(true);
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            cleanAllTiles(false);
            printMap();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            loadFromMap();
        }
        //print(tileInst.transform.position);
    }

    private void moveMe()
    {
        int upSpeed = 0;
        int rightSpeed = 0;
        int backSpeed = 0;
        if (Input.GetKey(KeyCode.W))
        {
            upSpeed = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            upSpeed = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rightSpeed = 1;

        }
        else if (Input.GetKey(KeyCode.A))
        {
            rightSpeed = -1;

        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rightSpeed *= 2;
            upSpeed *= 2;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Camera.main.orthographicSize++;

        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            if(Camera.main.orthographicSize > 1)
                Camera.main.orthographicSize--;
        }
        rigidbody2D.velocity = new Vector2(rightSpeed * 16, upSpeed * 16);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private Vector3 doMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x, mousePos.y, -transform.position.z + 1);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos = new Vector3(mousePos.x, mousePos.y, 1);
        return mousePos;
    }

    private void manageTile()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTile++;
            if (currentTile >= tiles.Count)
            {
                currentTile = 0;
            }
            DestroyImmediate(tileInst.gameObject);
            tileInst = Instantiate<TileCode>(tiles[currentTile]);
            tileInst.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
            tileInst.transform.localScale = new Vector3(6.4f, 6.4f, 1.001f);
            if (tileInst.baseCode == "!")
            {
                tileInst.transform.localScale = new Vector3(0.5f, 0.5f, 1.001f);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentTile--;
            if (currentTile < 0)
            {
                currentTile = tiles.Count - 1;
            }
            DestroyImmediate(tileInst.gameObject);
            tileInst = Instantiate<TileCode>(tiles[currentTile]);
            tileInst.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.8f);
            tileInst.transform.localScale = new Vector3(6.4f, 6.4f, 1.001f);
            if(tileInst.baseCode == "!")
            {
                tileInst.transform.localScale = new Vector3(0.5f, 0.5f, 1.001f);
            }

        }
    }

    private void cleanAllTiles(bool text)
    {
        if(text)
            print("cleaning tile list");
        List<TileCode> temp = new List<TileCode>();
        int ghosts = 0;
        foreach(TileCode t in allTiles)
        {
            if(t != null)
            {
                temp.Add(t);
            }
            else
            {
                ghosts++;
            }
        }
        allTiles = temp;
        if (text)
            print("cleaned " + ghosts + " ghosts");
    }

    private void printMap()
    {
        //print("Printing Map");
        int highestX = 0;
        int lowestX = 0;
        int highestY = 0;
        int lowestY = 0;

        foreach(TileCode t in allTiles)
        { 
            Vector2 tempPos = t.getPos();
            if (tempPos.x > highestX)
            {
                highestX = (int)tempPos.x;
            }
            else if (tempPos.x < lowestX)
            {
                lowestX = (int)tempPos.x;
            }

            if (tempPos.y > highestY)
            {
                highestY = (int)tempPos.y;
            }
            else if (tempPos.y < lowestY)
            {
                lowestY = (int)tempPos.y;
            }
        }

        highestX = Mathf.Abs(lowestX) + highestX+1;
        highestY = Mathf.Abs(lowestY) + highestY+1;
        //print("Map is " + highestX + " wide by " + highestY + " tall.");
        dim = new Vector2(highestX, highestY);
        string[,] map = new string[highestX,highestY];
        foreach(TileCode t in allTiles)
        {
            map[(int)(t.getPos().x + Mathf.Abs(lowestX)), (int)(t.getPos().y+ Mathf.Abs(lowestY))] = t.getCode() + t.getSecondary();
        }

        //print("Finished mapping, filling in blanks...");

        for(int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(1); j++)
            {
                if(map[i,j] == null)
                {
                    map[i, j] = "___";
                }
            }
        }

        //print("Finished map, printing it: ");
        for (int i = map.GetLength(1)-1; i >= 0; i--)
        {
            string row = " ";
            for (int j = 0; j < map.GetLength(0); j++)
            {
                row += map[j, i] + " ";
            }
            //print(row);
        }

        writeMap(map);
    }


    private void writeMap(string[,] map)
    {
        string path = "Assets/Resources/Maps/map.txt";
        using (var stream = new FileStream(path, FileMode.Truncate))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(dim.x + "-" + dim.y + "\n");
                for (int i = map.GetLength(1) - 1; i >= 0; i--)
                {
                    string row = " ";
                    for (int j = 0; j < map.GetLength(0); j++)
                    {
                        row += map[j, i] + " ";
                    }
                    //print(row);
                    writer.Write(row + "\n");
                }
                writer.Write("endmap\n");
                int num = 0;
                foreach(List<string> l in opalSpawns)
                {
                    string row = "" + num;
                    num++;
                    foreach(string s in l)
                    {
                        row = row + " " + s;
                    }
                    writer.Write(row + "\n");
                }
            }
        }
        //AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Maps/map");
    }

    private void loadFromMap()
    {
        cleanAllTiles(false);
        for(int num = 0; num < allTiles.Count; num++)
        {
            DestroyImmediate(allTiles[num].gameObject);
            allTiles[num] = null;
        }
        cleanAllTiles(false);
        string path = "Assets/Resources/Maps/map.txt";
        string map = "";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        string reid = reader.ReadLine();
        reid = reader.ReadLine();
        while (reid != "endmap")
        {
            map += reid + "-";
            reid = reader.ReadLine();
        }
        opalSpawns.Clear();
        currentSpawn = 0;
        while (!reader.EndOfStream)
        {
            reid = reader.ReadLine();
            string[] temps = reid.Split(' ');
            int index = 0;
            for(int num = 0; num < temps.Length; num++)
            {
                if(num == 0)
                {
                    index = int.Parse(temps[num]);
                    opalSpawns.Add(new List<string>());
                    currentSpawn++;
                }
                else
                {
                    opalSpawns[index].Add(temps[num]);
                }
            }
        }
        reader.Close();
        string[] splitter = map.Split('-');
        List<string[]> makeMap = new List<string[]>();
        foreach (string s in splitter)
        {
            string[] splitter2 = s.Split(' ');
            makeMap.Add(splitter2);
        }
        int i = 0;
        foreach(string[] ss in makeMap)
        {
            int j = 0;
            foreach (string s in ss)
            {
                //print("|"+s+"|");
                if (s != "" && s != " " )
                {
                    if (s == "___")
                    {
                        j++;
                    }
                    else
                    {
                        TileCode tc = Instantiate<TileCode>(getTileByCode[s.Substring(0,1)]);
                        tc.setSecondaryCode(s.Substring(1,2));
                        if (s.Substring(1, 2) != "__")
                        {
                            tc.setSpawning(true);
                        }
                        tc.transform.position = new Vector3(j, s.Length - i, 1);
                        tc.transform.localScale = new Vector3(6.4f, 6.4f, 1.001f);
                        allTiles.Add(tc);
                        j++;
                    }
                }
            }
            i++;
        }

    }

    private void addSpawnCode(TileCode tile, int index)
    {
        if(tile.getSecondary() != "__")
        {
            tile.setSecondaryCode("__");
            tile.setSpawning(false);
            return;
        }
        int mySpawn = checkNeighbors(index);
        if(mySpawn == -1)
        {
            mySpawn = currentSpawn;
            opalSpawns.Add(new List<string>());
            currentSpawn++;
        }
        print(mySpawn);
        tile.setSecondaryCode(mySpawn+"");
        tile.setSpawning(true);
    }

    private void addSpawnCode(TileCode tile, int index, int spawn)
    {
        if (tile.getSecondary() == "__")
        {
            int mySpawn = spawn;
            print(mySpawn);
            tile.setSecondaryCode(mySpawn + "");
            tile.setSpawning(true);
            proliferate(tile, index);
        }
    }

    private int checkNeighbors(int index)
    {
        //check left and right
        if(index != 0)
        {
            if(allTiles[index - 1] != null && allTiles[index - 1].getSecondary() != "__")
            {
                return int.Parse(allTiles[index - 1].getSecondary());
            }
        }
        if(index != allTiles.Count - 1)
        {
            if (allTiles[index + 1] != null && allTiles[index + 1].getSecondary() != "__")
            {
                return int.Parse(allTiles[index + 1].getSecondary());
            }
        }

        //check up and down
        float targetx = allTiles[index].getPos().x;
        float targety = allTiles[index].getPos().y;
        foreach (TileCode t in allTiles)
        {
            if(t != null && t.getPos().x == targetx)
            {
                if(t.getPos().y == targety+1 || t.getPos().y == targety - 1)
                {
                    if (t.getSecondary() != "__")
                    {
                        return int.Parse(t.getSecondary());
                    }
                }
            }
        }

        return -1;
    }

    private void proliferate(TileCode tile, int index)
    {
        if (index != 0)
        {
            if (allTiles[index - 1] != null && allTiles[index - 1].getCode() == tile.getCode())
            {
                addSpawnCode(allTiles[index - 1], index - 1, int.Parse(tile.getSecondary()));
            }
        }
        if (index != allTiles.Count - 1)
        {
            if (allTiles[index + 1] != null && allTiles[index + 1].getCode() == tile.getCode())
            {
                addSpawnCode(allTiles[index + 1], index + 1, int.Parse(tile.getSecondary()));
            }
        }

        //check up and down
        float targetx = allTiles[index].getPos().x;
        float targety = allTiles[index].getPos().y;
        int indy = 0;
        foreach (TileCode t in allTiles)
        {
            if (t != null && t.getPos().x == targetx)
            {
                if (t.getPos().y == targety + 1 || t.getPos().y == targety - 1)
                {
                    if (t.getCode() == tile.getCode())
                    {
                        addSpawnCode(t, indy, int.Parse(tile.getSecondary()));
                    }
                }
            }
            indy++;
        }
    }
}
