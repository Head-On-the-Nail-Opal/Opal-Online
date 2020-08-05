using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
    static public GroundScript me;
    static public Transform GameBoard;
    public TileScript[,] tileGrid = new TileScript[10,10];
    public DummyScript[,] dummies = new DummyScript[10, 10];
    public List<PathScript> paths = new List<PathScript>();
    public List<List<Vector3>> pathofpaths = new List<List<Vector3>>();
    public List<PathScript> generatedPaths = new List<PathScript>();
    public int switchCam = 1;

    public List<OpalScript> gameOpals = new List<OpalScript>();
    private List<OpalScript> nonSortedGameOpals = new List<OpalScript>();
    public List<OpalScript> p1Opals = new List<OpalScript>(); 
    public List<OpalScript> p2Opals = new List<OpalScript>();
    public List<OpalScript> p3Opals = new List<OpalScript>();
    public List<OpalScript> p4Opals = new List<OpalScript>();
    public int numTeams;

    private List<OpalScript> opalTurns = new List<OpalScript>();

    private GlobalScript glob;

    private string redController;
    private string blueController;
    private string greenController;
    private string orangeController;

    public Camera MainCam;
    public Camera Orthographic;
    public TileScript tilePrefab;
    public TileScript tilePrefab2;
    public TileScript fireTilePrefab;
    public TileScript miasmaTilePrefab;
    public TileScript growthTilePrefab;
    public TileScript floodTilePrefab;
    protected TileScript boulderTilePrefab;
    protected TileScript sporeTilePrefab;
    //public OpalScript opalPrefab;
    public CursorScript cursorPrefab;
    public Light spotlight;
    public ParticleSystem healthEffect;
    public ParticleSystem damageEffect;
    public ParticleSystem buffEffect;
    public ParticleSystem burningEffect;
    public ParticleSystem poisonEffect;
    public PathScript pathPrefab;
    private ParticleSystem debuffEffect;

    private GameObject chargeLightning;
    private ParticleSystem chargeEffect;
    private List<GameObject> charges = new List<GameObject>();
    private GameObject bluePlate;
    private GameObject redPlate;
    private GameObject greenPlate;
    private GameObject orangePlate;
    public CursorScript myCursor;

    private int blueTeamPriority;
    private int redTeamPriority;
    private int greenTeamPriority;
    private int orangeTeamPriority;

    List<OpalScript> tempB = new List<OpalScript>();
    List<OpalScript> tempR = new List<OpalScript>();
    List<OpalScript> tempG = new List<OpalScript>();
    List<OpalScript> tempO = new List<OpalScript>();

    bool setUp = false;
    string myTeam = "";

    //0 is blue, 1 is red
    int onlinePlayerNum = 0;

    public GameObject menu;
    private bool tog = true;


    public int tu = 0;

    private MultiplayerManager mm;

    private string bothTeams;


    private void Awake()
    {
        me = this;
        GameObject board = new GameObject("Gameboard");
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        mm = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        blueTeamPriority = 0;
        redTeamPriority = 0;

        GameBoard = board.transform;
        spotlight.transform.position = new Vector3(0, -10, 0);

        boulderTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/BoulderTile");
        sporeTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/SporeTile");
        chargeLightning = Resources.Load<GameObject>("Prefabs/LightningCharge");
        chargeEffect = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/ChargeEffect");

        redPlate = Resources.Load<GameObject>("Prefabs/RedPlate");
        bluePlate = Resources.Load<GameObject>("Prefabs/BluePlate");
        greenPlate = Resources.Load<GameObject>("Prefabs/GreenPlate");
        orangePlate = Resources.Load<GameObject>("Prefabs/OrangePlate");

        debuffEffect = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/DebuffEffect");

        tempB = glob.getBlueTeam();
        tempR = glob.getRedTeam();
        tempG = glob.getGreenTeam();
        tempO = glob.getOrangeTeam();
        redController = glob.getRedController();
        blueController = glob.getBlueController();
        greenController = glob.getGreenController();
        orangeController = glob.getOrangeController();
        if (!glob.getMult())
        {
            setTheRestUp();
            
        }
        else
        {
            numTeams = glob.getNumPlayers();
            print(numTeams);
            //TODO, send team data to Multiplayer Manager
            //format: myTeam,opalName,opalname,opalname,opalname,
            string sendMe = "";
            foreach (OpalScript o in tempB)
            {
                sendMe += o.GetType() + ",";
            }
            myTeam = sendMe;
            print(myTeam);
            mm.sendMultiplayerTeam(sendMe);

            //print(mm.getTeamOne() + " vs " + mm.getTeamTwo());



            //build teams from team data
            //data need to be in format: 1,opalname,opalname,opalname,oplaname,2,opalname,opalname,opalname,oplaname,3

            //string bothTeams = "";


            //int i = 1;
            tempB.Clear();
            tempR.Clear();
            tempG.Clear();
            tempO.Clear();
            redController = glob.getBlueController();
            blueController = glob.getBlueController();
            greenController = glob.getBlueController();
            orangeController = glob.getBlueController();
        }
    }

    public void Update()
    {
        if(!setUp && glob.getMult() && mm.getTeamOne() != "" && mm.getTeamTwo() != "" && (numTeams < 3 || mm.getTeamThree() != "") && (numTeams < 4 || mm.getTeamFour() != ""))
        {
            print(mm.getTeamOne() + " vs " + mm.getTeamTwo());
            setUp = true;
            
            string[] team1Names = mm.getTeamOne().Split(',');
            string[] team2Names = mm.getTeamTwo().Split(',');
            string[] team3Names = new string[1];
            string[] team4Names = new string[1];
            if (numTeams > 2)
            {
                team3Names= mm.getTeamThree().Split(',');
                if (numTeams == 4)
                {
                    team4Names = mm.getTeamFour().Split(',');
                }
            }
            if(mm.getTeamTwo() == myTeam)
            {
                onlinePlayerNum = 1;
            }else if (numTeams > 2 && mm.getTeamThree() == myTeam)
            {
                //print("i'm the green player");
                onlinePlayerNum = 2;
            }
            else if (numTeams > 3 && mm.getTeamFour() == myTeam)
            {
                onlinePlayerNum = 3;
            }
            foreach (string s in team1Names)
            {
                if (s != "")
                {
                    //print(s);
                    OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + s));
                    temp.setPos(-100, -100);
                    tempB.Add(temp);
                }
            }

            foreach (string s in team2Names)
            {
                if (s != "")
                {
                    //print(s);
                    OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + s));
                    temp.setPos(-100, -100);
                    tempR.Add(temp);
                }
            }

            if(numTeams > 2)
            {
                foreach (string s in team3Names)
                {
                    if (s != "")
                    {
                        //print(s);
                        OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + s));
                        temp.setPos(-100, -100);
                        tempG.Add(temp);
                    }
                }
            }

            if(numTeams > 3)
            {
                foreach (string s in team4Names)
                {
                    if (s != "")
                    {
                        //print(s);
                        OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + s));
                        temp.setPos(-100, -100);
                        tempO.Add(temp);
                    }
                }
            }
            
            setTheRestUp();
        }
    }

    public int getOnlineTeam()
    {
        return onlinePlayerNum;
    }

    private void setTheRestUp()
    {
        myCursor = Instantiate<CursorScript>(cursorPrefab);
        int p = 0;
        foreach (OpalScript o in tempB)
        {
            print(o.GetType());
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.setOpal("Blue");
            temp.setPos(-100, -100);
            p2Opals.Add(temp);
            blueTeamPriority += temp.getSpeed() * 10;
            blueTeamPriority += temp.getPriority();
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempR)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = true;
            temp.setOpal("Red");
            temp.setPos(-100, -100);
            p1Opals.Add(temp);
            redTeamPriority += temp.getSpeed() * 10;
            redTeamPriority += temp.getPriority();
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempG)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.setOpal("Green");
            temp.setPos(-100, -100);
            p3Opals.Add(temp);
            greenTeamPriority += temp.getSpeed() * 10;
            greenTeamPriority += temp.getPriority();
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempO)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.setOpal("Orange");
            temp.setPos(-100, -100);
            p4Opals.Add(temp);
            orangeTeamPriority += temp.getSpeed() * 10;
            orangeTeamPriority += temp.getPriority();
            p += 3;
        }
        if (!getMult()) { 
            if (p3Opals.Count == 0)
            {
                numTeams = 2;
            }
            else if (p4Opals.Count == 0)
            {
                numTeams = 3;
            }
            else
            {
                numTeams = 4;
            }
        }

        if (blueTeamPriority >= redTeamPriority)
        {
            gameOpals.AddRange(p1Opals);
            gameOpals.AddRange(p2Opals);
            gameOpals.AddRange(p3Opals);
            gameOpals.AddRange(p4Opals);
        }
        else if (blueTeamPriority < redTeamPriority)
        {
            gameOpals.AddRange(p2Opals);
            gameOpals.AddRange(p1Opals);
            gameOpals.AddRange(p3Opals);
            gameOpals.AddRange(p4Opals);
        }


        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                TileScript temp;
                if ((int)(i + j) % 2 == 0)
                    temp = Instantiate<TileScript>(tilePrefab);
                else
                    temp = Instantiate<TileScript>(tilePrefab2);
                temp.transform.SetParent(GameBoard);
                temp.setCoordinates(i , j);
                if (i != 10 && j != 10)
                {
                    tileGrid[i, j] = temp;
                    temp.toggleStarting();
                }
            }
        }
        sortOpals(gameOpals);
        nonSortedGameOpals.AddRange(gameOpals);
        switchCam = 1;
    }

    public void addToUnsorted(OpalScript o)
    {
        nonSortedGameOpals.Add(o);
    }

    public string generateString()
    {
        string output = "";
        output += nonSortedGameOpals.IndexOf(myCursor.getCurrentOpal()) + "|";
        foreach(TileScript t in tileGrid)
        {
            output += t.generateString() + "|";
        }

        foreach(OpalScript o in nonSortedGameOpals)
        {
            output += o.generateString() + "|";
        }
        output += "end";
        return output;
    }

    public void updateFromString(string input)
    {
        if(generateString() == input)
        {
            print("detected no desync");
            return;
        }

        string[] parsed = input.Split('|');
        if(nonSortedGameOpals.IndexOf(myCursor.getCurrentOpal()) != int.Parse(parsed[0]))
        {
            myCursor.setCurrentOpal(nonSortedGameOpals[int.Parse(parsed[0])]);
        }

        int i = 1;
        foreach(TileScript t in tileGrid)
        {
            t.setFromString(parsed[i]);
            i++;
        }

        foreach (OpalScript o in nonSortedGameOpals)
        {
            print("Opal: " + parsed[i]);
            if (parsed[i] == "end")
            {
                print("i will add a new opal here");
            }
            else
            {
                if (parsed[i].Split(',')[0] != o.getMyName())
                    print("oof ouch my bones");
                o.setFromString(parsed[i]);
                i++;
            }
        }
    }
    

    public PathScript getPath(int x, int z)
    {
        foreach(PathScript p in paths)
        {
            if(p.getPos().x == x && p.getPos().z == z)
            {
                return p;
            }
        }
        return null;
    }

    public int getCurrentCam()
    {
        return switchCam;
    }

    public MultiplayerManager getMM()
    {
        return mm;
    }

    public bool getMult()
    {
        return glob.getMult();
    }

    public void setBothTeams(string set)
    {
        bothTeams = set;
    }

    public void sortOpals(List<OpalScript> ls)
    {
        ls.Sort(compareOpals);
    }

    private int compareOpals(OpalScript o, OpalScript t)
    {
        int result = t.getSpeed() - o.getSpeed();
        if(result == 0)
        {
            result = t.getPriority() - o.getPriority();
            if(result == 0)
            {
                if(o.getTeam() != t.getTeam())
                {
                    if (o.getTeam() == "Red")
                    {
                        if(t.getTeam() == "Blue")
                            result = blueTeamPriority - redTeamPriority;
                        else if(t.getTeam() == "Green")
                            result = greenTeamPriority - redTeamPriority;
                        else if (t.getTeam() == "Orange")
                            result = orangeTeamPriority - redTeamPriority;
                    }
                    else if(o.getTeam() == "Blue")
                    {
                        if (t.getTeam() == "Red")
                            result = redTeamPriority - blueTeamPriority;
                        else if (t.getTeam() == "Green")
                            result = greenTeamPriority - blueTeamPriority;
                        else if (t.getTeam() == "Orange")
                            result = orangeTeamPriority - blueTeamPriority;
                    }
                    else if(o.getTeam() == "Green")
                    {
                        if (t.getTeam() == "Red")
                            result = redTeamPriority - greenTeamPriority;
                        else if (t.getTeam() == "Blue")
                            result = blueTeamPriority - greenTeamPriority;
                        else if (t.getTeam() == "Orange")
                            result = orangeTeamPriority - greenTeamPriority;
                    }
                    else
                    {
                        if (t.getTeam() == "Red")
                            result = redTeamPriority - orangeTeamPriority;
                        else if (t.getTeam() == "Blue")
                            result = blueTeamPriority - orangeTeamPriority;
                        else if (t.getTeam() == "Green")
                            result = greenTeamPriority - orangeTeamPriority;
                    }
                }
                if(result == 0)
                {
                    List<string> temp = new List<string>();
                    temp.Add(t.getMyName());
                    temp.Add(o.getMyName());
                    temp.Sort();
                    result = temp.IndexOf(t.getMyName());
                    if (result == 0)
                        result = 1;
                    else if (result == 1)
                        result = -1;
                }
            }
        }
        return result;
    }

    public void callParticles(string type, Vector3 pos)
    {
        ParticleSystem temp;
        if (type.Equals("buff"))
        {
            temp = Instantiate<ParticleSystem>(buffEffect);
            temp.transform.position = pos;
        }else if (type.Equals("health"))
        {
            temp = Instantiate<ParticleSystem>(healthEffect);
            temp.transform.position = pos;
        }
        else if (type.Equals("damage"))
        {
            temp = Instantiate<ParticleSystem>(damageEffect);
            temp.transform.position = pos;
        }
        else if (type.Equals("burning"))
        {
            temp = Instantiate<ParticleSystem>(burningEffect);
            temp.transform.position = pos;
        }
        else if (type.Equals("poison"))
        {
            temp = Instantiate<ParticleSystem>(poisonEffect);
            temp.transform.position = pos;
        }else if (type.Equals("debuff"))
        {
            temp = Instantiate<ParticleSystem>(debuffEffect);
            temp.transform.position = pos;
        }
        else if (type.Equals("charge"))
        {
            temp = Instantiate<ParticleSystem>(chargeEffect);
            temp.transform.position = pos;
        }
        else
        {
            temp = Instantiate<ParticleSystem>(chargeEffect);
            temp.transform.position = pos;
        }
        if (pos.x > -1 && pos.x > 9 && pos.z > -1 && pos.z > 9)
        {
            OpalScript target = tileGrid[(int)temp.transform.position.x, (int)temp.transform.position.z].currentPlayer;
            if (target != null)
            {
                temp.transform.SetParent(target.transform, true);
            }
        }
        temp.transform.rotation.SetEulerAngles(temp.transform.rotation.x, temp.transform.rotation.y, 0);
        temp.transform.localScale = new Vector3(1, 1, 1);
    }

    public void recallParticles()
    {
    }

    public void toggleMenu()
    {
        tog = !tog;
        if (tog)
        {
            menu.transform.position = new Vector3(11, 6.3f, -1.2f);
            Cursor.visible = true;
        }
        else
        {
            menu.transform.position = new Vector3(-1000,-1000,-1000);
            Cursor.visible = false;
        }
    }

    public void setTile(OpalScript o, string type, bool over)
    {
        setTile((int)o.getPos().x, (int)o.getPos().z, type, over);
    }

    public void setTile(TileScript t, string type, bool over)
    {
        setTile((int)t.getPos().x, (int)t.getPos().z, type, over);
    }

    public void setTile(int x, int y, string type, bool over)
    {
        if(x < 0 || x > 9 || y < 0 || y > 9 || tileGrid[x,y].getFallen())
        {
            return;
        }
        TileScript replaced = tileGrid[x, y];
        OpalScript standing = replaced.currentPlayer;
        if(type == replaced.type)
        {
            return;
        }
        if (type == "Boulder")
        {
            if (replaced.currentPlayer != null)
            {
                return;
            }
        }
        
        /**if (replaced.type == "Miasma" && type != "Miasma" && replaced.currentPlayer != null)
        {
            //replaced.currentPlayer.doBuff(0, -2, 0, false);
        }
        else if (replaced.type == "Growth" && type != "Growth" && replaced.currentPlayer != null)
        {
            DestroyImmediate(replaced.currentEffect);
            replaced.currentPlayer.doBuff(-2, -2, 0, false);
        }*/
        if (type.Equals("Grass"))
        {
            replaced.standingOn(null);
            replaced.setCoordinates(-100, -100);
            TileScript tempTile;
            if ((int)(x + y) % 2 == 0)
                tempTile = Instantiate<TileScript>(tilePrefab);
            else
                tempTile = Instantiate<TileScript>(tilePrefab2);
            tempTile.transform.SetParent(GameBoard);
            tempTile.setCoordinates(x, y);
            tileGrid[x, y] = tempTile;
            tempTile.standingOn(standing);
        }
        else if (type.Equals("Fire")) {
            if (replaced.type != "Miasma" && replaced.type != "Flood" && replaced.type != "Boulder" || over)
            {
                if(replaced.getWet() == true)
                {
                    doWet((int)replaced.getPos().x, (int)replaced.getPos().z, false);
                    return;
                }
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(fireTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        else if (type.Equals("Miasma"))
        {
            if (replaced.type != "Growth" && replaced.type != "Fire" && replaced.type != "Flood" || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(miasmaTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        else if (type.Equals("Growth"))
        {
            if (replaced.type != "Fire" && replaced.type != "Growth" && replaced.type != "Boulder" || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(growthTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        else if (type.Equals("Flood"))
        {
            if (replaced.type != "Boulder" && replaced.type != "Spore" && replaced.type != "Growth" || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(floodTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }else if(replaced.type == "Boulder")
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        else if (type.Equals("Boulder"))
        {
            if(replaced.currentPlayer != null)
            {
                return;
            }
            if (replaced.type != "Flood" || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(boulderTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
            else if (replaced.type == "Flood")
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        else if (type == "Spore")
        {
            if (replaced.type != "Fire" && replaced.type != "Growth" && replaced.type != "Boulder" && replaced.type != "Flood" || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                TileScript tempTile = Instantiate<TileScript>(sporeTilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
    }

    public void protSetTrap(float x, float y, string traptype)
    {
        TileScript target = tileGrid[(int)x, (int)y];
        if(target != null && target.currentPlayer == null)
        {
            target.setTrap(traptype);
        }
    }

    public void diplayPath(bool func)
    {
        if (func)
        {
            foreach(List<Vector3> l in pathofpaths)
            {
                foreach(Vector3 v in l)
                {
                    PathScript temp = Instantiate<PathScript>(pathPrefab);
                    temp.setCoordinates((int)v.x, (int)v.z);
                    generatedPaths.Add(temp);
                }
            }
        }
        else
        {
            foreach (PathScript p in generatedPaths)
            {
                DestroyImmediate(p.gameObject);
            }
            generatedPaths.Clear();
            pathofpaths.Clear();
        }
    }

    public string getCurrentController(string player)
    {
        if (player == "Red")
        {
            return redController;
        }
        else if (player == "Blue")
        {
            return blueController;
        }else if(player == "Green")
        {
            return greenController;
        }else if(player == "Orange")
        {
            return orangeController;
        }
        return null;
    }

    public void updateTurnOrder(int currentTurn)
    {
        float i = 0;
        if (currentTurn == 0)
        {
            if(opalTurns.Count > 0 && opalTurns[opalTurns.Count - 1] != null)
                DestroyImmediate(opalTurns[opalTurns.Count-1].gameObject);
            opalTurns.Clear();
            foreach (OpalScript o in gameOpals)
            {
                if (!o.getDead())
                {
                    OpalScript temp = Instantiate<OpalScript>(o);
                    opalTurns.Add(temp);
                    temp.healStatusEffects();
                    temp.transform.position = new Vector3(14, 10 - i, 8);
                    if(o.getTeam() == "Red")
                    {
                        GameObject pl = Instantiate<GameObject>(redPlate, temp.transform);
                        pl.transform.localScale = new Vector3(pl.transform.localScale.x / temp.transform.localScale.x, pl.transform.localScale.y / temp.transform.localScale.y, pl.transform.localScale.z / temp.transform.localScale.z);
                        pl.transform.position = new Vector3(pl.transform.position.x, pl.transform.position.y - 0.00001f, pl.transform.position.z);
                    }
                    else if(o.getTeam() == "Blue")
                    {
                        GameObject pl = Instantiate<GameObject>(bluePlate, temp.transform);
                        pl.transform.localScale = new Vector3(pl.transform.localScale.x / temp.transform.localScale.x, pl.transform.localScale.y / temp.transform.localScale.y, pl.transform.localScale.z / temp.transform.localScale.z);
                        pl.transform.position = new Vector3(pl.transform.position.x, pl.transform.position.y - 0.00001f, pl.transform.position.z);
                    }
                    else if (o.getTeam() == "Green")
                    {
                        GameObject pl = Instantiate<GameObject>(greenPlate, temp.transform);
                        pl.transform.localScale = new Vector3(pl.transform.localScale.x / temp.transform.localScale.x, pl.transform.localScale.y / temp.transform.localScale.y, pl.transform.localScale.z / temp.transform.localScale.z);
                        pl.transform.position = new Vector3(pl.transform.position.x, pl.transform.position.y - 0.00001f, pl.transform.position.z);
                    }
                    else if (o.getTeam() == "Orange")
                    {
                        GameObject pl = Instantiate<GameObject>(orangePlate, temp.transform);
                        pl.transform.localScale = new Vector3(pl.transform.localScale.x / temp.transform.localScale.x, pl.transform.localScale.y / temp.transform.localScale.y, pl.transform.localScale.z / temp.transform.localScale.z);
                        pl.transform.position = new Vector3(pl.transform.position.x, pl.transform.position.y - 0.00001f, pl.transform.position.z);
                    }
                    i += 1.8f;
                }
                else
                {
                    OpalScript temp = Instantiate<OpalScript>(o);
                    temp.setDead();
                    
                    opalTurns.Add(temp);
                    temp.transform.position = new Vector3(-100, -100, -100);
                }
            }
        } else
        {
            if (opalTurns.Count > 0 && opalTurns[opalTurns.Count - 1] != null && opalTurns[currentTurn - 1] != null && opalTurns[currentTurn - 1].gameObject)
                DestroyImmediate(opalTurns[currentTurn - 1].gameObject);
            foreach(OpalScript o in opalTurns)
            {
                if (o != null && !o.getDead())
                {
                    o.transform.position = new Vector3(14, 10 - i, 8);
                    o.healStatusEffects();
                    i += 1.8f;
                }
            }
        }
    }

    public void doWet(int x, int y, bool wet)
    {
        if (x < 0 || x > 9 || y < 0 || y > 9)
        {
            return;
        }
        TileScript wetted = tileGrid[x, y];
        if(wet && wetted.type == "Fire")
        {
            setTile(x, y, "Grass", true);
        }
        else
        {
            wetted.setWet(wet);
        }
    }

    public void setChargeDisplay(int num)
    {
        foreach(GameObject g in charges)
        {
            DestroyImmediate(g);
        }
        charges.Clear();
        for(int i = 0; i < num; i++)
        {
            GameObject tempCharge = Instantiate<GameObject>(chargeLightning);
            tempCharge.transform.position = new Vector3(6.5f + 0.1f * i, 4.5f, -7.8f + 0.1f * i);
            tempCharge.transform.eulerAngles = new Vector3(35, -45, 0);
            tempCharge.transform.localScale = new Vector3(0.1111f, 0.1111f, 0.11111f);
            charges.Add(tempCharge);
        } //6.5,-7.8 ---> 7.501, -6.799
    }
}
