using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour {
    static public GroundScript me;
    static public Transform GameBoard;
    public TileScript[,] tileGrid = new TileScript[10, 10];
    public DummyScript[,] dummies = new DummyScript[10, 10];
    public List<PathScript> paths = new List<PathScript>();
    public List<List<Vector3>> pathofpaths = new List<List<Vector3>>();
    public List<PathScript> generatedPaths = new List<PathScript>();
    public int switchCam = 1;

    //private List<string> wildlife = new List<string>{"ButterflightSwarm", "Pebble", "Conspicuous Bush", "Diamond Coin", "Bombats"};
    private List<string> wildlife = new List<string> { "ButterflightSwarm", "Pebble", "Conspicuous Bush"};

    public List<OpalScript> gameOpals = new List<OpalScript>();
    private List<OpalScript> nonSortedGameOpals = new List<OpalScript>();
    public List<int> alreadyMoved = new List<int>();
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
    private GameObject opalPlate2;
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
    private int isBarriarray = -1;

    private Canvas myCanvas;

    private OpalScript boulder;
    private OpalScript boulder2;

    private bool gameWon = false;
    private bool resetting = false;

    public Vocabulary vocab;

    private GroundScript AIBoard;
    private bool secondary = false;
    private bool madeBoard = false;

    private string gameState;
    private string gameState1;

    private List<string> saveStates = new List<string>();
    private List<OpalScript> allBoulders = new List<OpalScript>();

    private bool noUpdate = false;

    private void Awake()
    {
        for(int i = 0; i < 20; i++)
        {
            saveStates.Add("");
        }
        me = this;
        GameObject board = new GameObject("Gameboard");
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        mm = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        myCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        blueTeamPriority = 0;
        redTeamPriority = 0;

        GameBoard = board.transform;
        spotlight.transform.position = new Vector3(0, -10, 0);

        boulderTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/BoulderTile");
        sporeTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/SporeTile");
        chargeLightning = Resources.Load<GameObject>("Prefabs/LightningCharge");
        chargeEffect = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/ChargeEffect");
        boulder = Resources.Load<OpalScript>("Prefabs/Boulder0");
        boulder2 = Resources.Load<OpalScript>("Prefabs/Boulder1");

        redPlate = Resources.Load<GameObject>("Prefabs/RedPlate");
        bluePlate = Resources.Load<GameObject>("Prefabs/BluePlate");
        greenPlate = Resources.Load<GameObject>("Prefabs/GreenPlate");
        orangePlate = Resources.Load<GameObject>("Prefabs/OrangePlate");

        opalPlate2 = Resources.Load<GameObject>("Prefabs/OpalPlate2");

        debuffEffect = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/DebuffEffect");

        growthTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/GrowthTile");
        fireTilePrefab = Resources.Load<TileScript>("Prefabs/Tiles/FireTile");

        vocab.setIt();

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
                sendMe += o.GetType().ToString() +o.saveOpal() + ",";
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
            redController = glob.getRedController();
            blueController = glob.getRedController();
            greenController = glob.getRedController();
            orangeController = glob.getRedController();
        }
    }

    public void setSecondary(bool s)
    {
        secondary = s;
    }

    public void setNoUpdate(bool n)
    {
        noUpdate = n;
    }

    public GlobalScript getGlob()
    {
        return glob;
    }

    public bool includeAI()
    {
        if (redController == "AI" || blueController == "AI" || orangeController == "AI" || greenController == "AI")
            return true;
        return false;
    }

    public void Update()
    {
        if(!setUp && glob.getMult() && mm.getTeamOne() != "" && mm.getTeamTwo() != "" && (numTeams < 3 || mm.getTeamThree() != "") && (numTeams < 4 || mm.getTeamFour() != ""))
        {
           
            print(mm.getTeamOne()+ " vs " + mm.getTeamTwo());
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
                    OpalScript temp = convertOpalFromString(s);
                    temp.setPos(-100, -100);
                    tempB.Add(temp);
                }
            }

            foreach (string s in team2Names)
            {
                if (s != "")
                {
                    //print(s);
                    OpalScript temp = convertOpalFromString(s);
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
                        OpalScript temp = convertOpalFromString(s);
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
                        OpalScript temp = convertOpalFromString(s);
                        temp.setPos(-100, -100);
                        tempO.Add(temp);
                    }
                }
            }
            
            setTheRestUp();
        }
    }

    public OpalScript convertOpalFromString(string data)
    {
        string[] parsed = data.Split('|');
        //Debug.LogError("du hello:" +data);
        OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + parsed[0]));
        temp.setPersonality(parsed[2]);
        temp.setCharmFromString(parsed[1],false);
        return temp;
    }

    public int getOnlineTeam()
    {
        return onlinePlayerNum;
    }

    public void setGameWon(bool input)
    {
        gameWon = input;
    }

    public bool getGameWon()
    {
        return gameWon;
    }

    public void setResetting(bool input)
    {
        resetting = input;
    }

    public bool getResetting()
    {
        return resetting;
    }

    private void setTheRestUp()
    {
        myCursor = Instantiate<CursorScript>(cursorPrefab);
        int p = 0;
        int idCount = 0;
        foreach (OpalScript o in tempB)
        {
            //print(o.GetType());
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.setOpal("Blue");
            temp.setDetails(o);
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.transform.rotation = Quaternion.Euler(40,-45,0);
            //temp.setPersonality(o.getPersonality());
            temp.setPos(-100, -100);
            p2Opals.Add(temp);
            blueTeamPriority += temp.getSpeed() * 10;
            blueTeamPriority += temp.getPriority();
            temp.setID(idCount);
            idCount++;
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempR)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = true;
            temp.setOpal("Red");
            temp.setDetails(o);
            temp.transform.rotation = Quaternion.Euler(40, -45, 0);
            //temp.setPersonality(o.getPersonality());
            temp.setPos(-100, -100);
            p1Opals.Add(temp);
            redTeamPriority += temp.getSpeed() * 10;
            redTeamPriority += temp.getPriority();
            temp.setID(idCount);
            idCount++;
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempG)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.transform.rotation = Quaternion.Euler(40, -45, 0);
            temp.setOpal("Green");
            temp.setDetails(o);
            // temp.setPersonality(o.getPersonality());
            temp.setPos(-100, -100);
            p3Opals.Add(temp);
            greenTeamPriority += temp.getSpeed() * 10;
            greenTeamPriority += temp.getPriority();
            temp.setID(idCount);
            idCount++;
            p += 3;
        }
        p = 0;
        foreach (OpalScript o in tempO)
        {
            OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + o.GetType()));
            temp.GetComponent<SpriteRenderer>().flipX = false;
            temp.transform.rotation = Quaternion.Euler(40, -45, 0);
            temp.setOpal("Orange");
            temp.setDetails(o);
            // temp.setPersonality(o.getPersonality());
            temp.setPos(-100, -100);
            p4Opals.Add(temp);
            orangeTeamPriority += temp.getSpeed() * 10;
            orangeTeamPriority += temp.getPriority();
            temp.setID(idCount);
            idCount++;
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
                TileScript temp = Instantiate<TileScript>(tilePrefab);
                temp.transform.SetParent(GameBoard);
                temp.setCoordinates(i , j);
                if (i < 10 && i >= 0 && j < 10 && j >= 0)
                {
                    tileGrid[i, j] = temp;
                    temp.toggleStarting();
                }
                
            }
        }
        foreach(TileScript t in tileGrid)
        {
            t.updateConnection();
            t.setRandomDecor();
            if(Random.Range(0,15) == 4)
            {
                t.spawnWildlife(wildlife[Random.Range(0,wildlife.Count)]);
            }
        }
        foreach(OpalScript o in gameOpals)
        {
            if(glob.getOverload(o.getTeam()).Contains(o.getMainType()) || glob.getOverload(o.getTeam()).Contains(o.getSecondType()))
            {
                o.setOverloaded(true);
            }
        }
        setUpSurroundings();
        sortOpals(gameOpals);
        nonSortedGameOpals.AddRange(gameOpals);
        switchCam = 1;
    }

    public CursorScript getMyCursor()
    {
        return myCursor;
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
    

    public void setUpGlob()
    {
        glob.setFinishedGame(true); 
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
                    result = t.getID() - o.getID();
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

    public TileScript getTile(int x,int z)
    {
        if (x < 0 || x > 9 || z < 0 || z > 9 || tileGrid[x, z].getFallen())
        {
            return null;
        }
        return tileGrid[x, z];
    }

    public TileScript setTile(OpalScript o, string type, bool over)
    {
        return setTile((int)o.getPos().x, (int)o.getPos().z, type, over);
    }

    public TileScript setTile(TileScript t, string type, bool over)
    {
        return setTile((int)t.getPos().x, (int)t.getPos().z, type, over);
    }

    public TileScript setTile(int x, int y, string type, bool over)
    {
        if(x < 0 || x > 9 || y < 0 || y > 9 || tileGrid[x,y].getFallen())
        {
            return null;
        }
        if(tileGrid[x,y].getDecayTurn() != 1 && tileGrid[x, y].type != "Grass" && tileGrid[x, y].type != type && type != "Grass")
        {
            tileGrid[x, y].reduceDecay(1);
            ParticleSystem temp = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/TilePuff"), tileGrid[x,y].transform);
            return tileGrid[x,y];
        }
        if (tileGrid[x, y].type != "Grass" && tileGrid[x, y].type == type)
        {
            tileGrid[x, y].resetDecay(1);
            return tileGrid[x, y];
        }

            TileScript replaced = tileGrid[x, y];
        bool wasFlood = false;
        if(replaced.type == "Flood")
        {
            wasFlood = true;
        }
        List<string> tileCharm = replaced.getCharms();
        OpalScript standing = replaced.getCurrentOpal();
        if(type == replaced.type)
        {
            return null;
        }
        if (type == "Boulder")
        {
            if (replaced.currentPlayer != null)
            {
                return null;
            }
        }
        TileScript tempTile = null;
        if (type.Equals("Grass"))
        {
            replaced.standingOn(null);
            replaced.setCoordinates(-100, -100);
            if ((int)(x + y) % 2 == 0)
                tempTile = Instantiate<TileScript>(tilePrefab);
            else
                tempTile = Instantiate<TileScript>(tilePrefab);
            tempTile.transform.SetParent(GameBoard);
            tempTile.setCoordinates(x, y);
            if(replaced.type != "Grass")
                tempTile.setTexture(getTextureName(replaced.type));
            tileGrid[x, y] = tempTile;
            tempTile.standingOn(standing);
            if (standing != null)
                standing.setCurrentTile(tempTile);
        }
        else if (type.Equals("Fire")) {
            if (!(replaced.getCurrentOpal() != null && replaced.getCurrentOpal().getMyName() == "Boulder") || over)
            {
                if(replaced.getWet() == true)
                {
                    doWet((int)replaced.getPos().x, (int)replaced.getPos().z, false);
                    return null;
                }
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.type = "Fire";
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
                if(standing != null)
                    standing.setCurrentTile(tempTile);
            }
        }
        else if (type.Equals("Miasma"))
        {
            if ( !(replaced.getCurrentOpal() != null && replaced.getCurrentOpal().getMyName() == "Boulder") || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.type = "Miasma";
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
                if (standing != null)
                    standing.setCurrentTile(tempTile);
            }
        }
        else if (type.Equals("Growth"))
        {
            if (!(replaced.getCurrentOpal() != null && replaced.getCurrentOpal().getMyName() == "Boulder") || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.type = "Growth";
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
                
                if (standing != null)
                    standing.setCurrentTile(tempTile);
            }
        }
        else if (type.Equals("Flood"))
        {
            if (!(replaced.getCurrentOpal() != null && replaced.getCurrentOpal().getMyName() == "Boulder") || over)
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.type = "Flood";
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
                if (standing != null)
                    standing.setCurrentTile(tempTile);
                
            }
            else if(replaced.getCurrentOpal() != null && replaced.getCurrentOpal().getMyName() == "Boulder")
            {
                standing.takeDamage(standing.getHealth(), false, false);
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
                if (standing != null)
                    standing.setCurrentTile(tempTile);
            }
        }
        else if (type.Equals("Boulder"))
        {
            if(replaced.currentPlayer != null)
            {
                return null;
            }
            if (replaced.type != "Flood" || over)
            {
                placeBoulder(tileGrid[x, y], myCursor.getCurrentOpal());
                
            }
            else if (replaced.type == "Flood")
            {
                replaced.standingOn(null);
                replaced.setCoordinates(-100, -100);
                tempTile = Instantiate<TileScript>(tilePrefab);
                tempTile.transform.SetParent(GameBoard);
                tempTile.setCoordinates(x, y);
                tileGrid[x, y] = tempTile;
                tempTile.standingOn(standing);
            }
        }
        if(tileGrid[x,y].type == "Flood" || wasFlood)
        {
            
            List<TileScript> output = new List<TileScript>();
            TileScript temp = tileGrid[x, y];
            temp.determineShape();
            bool adj = false;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (temp.getPos().x + i < 10 && temp.getPos().x + i > -1 && temp.getPos().z + j < 10 && temp.getPos().z + j > -1 && !(i == 0 && j == 0) && (!adj || (Mathf.Abs(i) != Mathf.Abs(j))))
                    {
                        output.Add(tileGrid[(int)temp.getPos().x + i, (int)temp.getPos().z + j]);
                        if(tileGrid[(int)temp.getPos().x + i, (int)temp.getPos().z + j].type == "Flood")
                            tileGrid[(int)temp.getPos().x + i, (int)temp.getPos().z + j].determineShape();
                    }
                }
            }
        }

        if (!noUpdate)
        {
            if (tempTile != null)
            {
                tempTile.updateConnection();
                tempTile.setRandomDecor();
                foreach (TileScript t in tempTile.getSurroundingTiles(false))
                {
                    t.updateConnection();
                }
            }
        }

        foreach (string c in tileCharm)
        {
            if(c != "" && c != "None" && tileGrid[x,y] != replaced)
                tileGrid[x, y].addCharm(c);
        }
        return tileGrid[x, y];
    }

    private string getTextureName(string tile)
    {
        string output = "";
        switch (tile)
        {
            case "Fire":
                output = "ScorchedGrass";
                break;
            case "Flood":
                output = "Puddled";
                break;
            case "Growth":
                output = "DeadGrass";
                break;
            case "Miasma":
                output = "PoisonScar";
                break;
        }
        return output;
    }

    public bool isValidTile(int x, int y)
    {
        if (x < 10 && x > -1 && y < 10 && y > -1)
        {
            return true;
        }
        return false;
    }

    public void setTiles(TileScript start, int dist, string type)
    {
        setTile(start, type, false);
        for(int i = -dist+1; i < dist; i++)
        {
            for(int j = -dist+1; j < dist; j++)
            {
                if (isValidTile(i + (int)start.getPos().x, j + (int)start.getPos().z))
                {
                    setTile(tileGrid[i + (int)start.getPos().x, j + (int)start.getPos().z], type, false);
                }
            }
        }
    }

    public void setTilesRound(TileScript start, int dist, string type)
    {
        TileScript tileCatch = setTile(start, type, false);
        if (start.getPos().x == -100)
            start = tileCatch;
        int x = (int)start.getPos().x;
        int y = (int)start.getPos().z;
        for (int i = -dist + 1; i < dist; i++)
        {
            for (int j = -dist + 1; j < dist; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) < dist)
                {
                    if (isValidTile(i + x, j + y))
                    {
                        setTile(tileGrid[i + x, j + y], type, false);
                    }
                }
            }
        }
    }

    public void fireProjectile(OpalScript from, OpalScript to, int attackNum)
    {
        myCursor.doAttack((int)to.getPos().x, (int)to.getPos().z, attackNum, from);
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

    public List<TileScript> getSurroundingTiles(TileScript tile, bool adj)
    {
        List<TileScript> output = new List<TileScript>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (tile.getPos().x + i < 10 && tile.getPos().x + i > -1 && tile.getPos().z + j < 10 && tile.getPos().z + j > -1 && !(i == 0 && j == 0) && (!adj || (Mathf.Abs(i) != Mathf.Abs(j))))
                {
                    //print((int)getPos().x + i + ", " + ((int)getPos().z + j));
                    output.Add(tileGrid[(int)tile.getPos().x + i, (int)tile.getPos().z + j]);
                }
            }
        }
        return output;
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

    public string setCurrentController(string controller, string player)
    {
        if (player == "Red")
        {
            redController = controller;
        }
        else if (player == "Blue")
        {
            blueController = controller;
        }
        else if (player == "Green")
        {
            greenController = controller;
        }
        else if (player == "Orange")
        {
            orangeController = controller;
        }
        return null;
    }

    public void updateTurnOrder(int currentTurn, List<int> aM)
    {
        if (aM != null)
        {
            alreadyMoved = aM;
        } 
        float i = 0;
        foreach(OpalScript o in opalTurns)
        {
            DestroyImmediate(o.gameObject);
        }
        opalTurns.Clear();
        sortOpals(gameOpals);
        if(myCursor.getCurrentOpal() != null)
        {
            int num = 0;
            foreach(OpalScript o in gameOpals)
            {
                if(o.getID() == myCursor.getCurrentOpal().getID())
                {
                    break;
                }
                num++;
            }
            gameOpals.RemoveAt(num);
            gameOpals.Insert(0,myCursor.getCurrentOpal());
        }
        int indent = 0;
        foreach (OpalScript o in gameOpals)
        {
            if (i / 150f > 5)
                break;
            if (gameWon)
                return;
            if (!o.getDead() && !alreadyMoved.Contains(o.getID())) 
            {
                OpalScript temp = Instantiate<OpalScript>(o.getMyModel(), myCanvas.transform.Find("TurnOrder").transform);
                temp.setOpal(o.getTeam());
                temp.setDisplayOpal();
                opalTurns.Add(temp);
                temp.healStatusEffects();
                temp.doHighlight();
                if (indent == 0)
                {
                    temp.transform.localPosition = new Vector3(862.5f, 442.5f - i * 0.96f, 0);
                    indent++;
                }
                else
                {
                    temp.transform.localPosition = new Vector3(878, 283 - (i-150f) * 0.96f, 0);
                }
                temp.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                temp.transform.localScale = o.transform.localScale * 170f;
                //temp.transform.localScale = new Vector3(100*temp.transform.localScale.x, 100 * temp.transform.localScale.y, 1);
                GameObject pl = Instantiate<GameObject>(opalPlate2, temp.transform);
                pl.transform.position = new Vector3(pl.transform.position.x, pl.transform.position.y - 0.00001f, pl.transform.position.z);
                if (o.getTeam() == "Red")
                {
                    pl.GetComponent<SpriteRenderer>().color = new Color(1, 0.2f, 0.2f);

                }
                else if(o.getTeam() == "Blue")
                {
                    pl.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1); 
                }
                else if (o.getTeam() == "Green")
                {
                    pl.GetComponent<SpriteRenderer>().color = new Color(0.2f, 1, 0.2f); 
                }
                else
                {
                    pl.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);
                }
                pl.transform.localScale /= temp.transform.lossyScale.x;
                pl.transform.localScale *= 5f;
                pl.AddComponent<TurnHighlighter>();
                pl.AddComponent<BoxCollider>();
                pl.GetComponent<TurnHighlighter>().setUp(this, o.getID());
                i += 150f;
            }
        }
    }

    public List<int> getAlreadyMoved()
    {
        return alreadyMoved;
    }

    public void updateTurnOrder(int currentTurn)
    {
        updateTurnOrder(currentTurn, null);
    }

    public void highlightOpal(int id, bool highlight)
    {
        foreach(OpalScript o in gameOpals)
        {
            if(o.getID() == id)
            {
                o.showSpot(highlight);
                return;
            }
        }
    }

    public void updateCurrent(int id)
    {

        myCursor.updateCurrent(id);
    }

    public void updateCurrent()
    {
        myCursor.updateCurrent();
    }

    public void updateCurrentActually()
    {
        myCursor.updateCurrentActually();
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

    public bool checkForBarriarray()
    {
        if (isBarriarray == -1)
        { 
            foreach(OpalScript o in gameOpals)
            {
                if (o.getMyName() == "Barriarray")
                {
                    isBarriarray = 1;
                    return true;
                }
                else
                {
                    isBarriarray = 0;
                    return false;
                }
            }
        }
        else if (isBarriarray == 1)
        {
            return true;
        }
        return false;
    }

    public void placeBoulder(int x, int y, OpalScript placed)
    {
        if (x < 0 || x > 9 || y < 0 || y > 9)
        {
            return;
        }
        placeBoulder(tileGrid[x, y], placed);
    }

    public void placeBoulder(TileScript target, OpalScript placed)
    {
        if (target.currentPlayer != null || target.getFallen())
            return;
        if(target.type == "Flood")
        {
            setTile(target, "Grass", true);
            return;
        }
        OpalScript b = boulder;
        if (Random.Range(0,2)==0)
        {
            b = boulder2;
        }
        OpalScript opalTwo = Instantiate<OpalScript>(b);
        OpalScript placer = myCursor.getCurrentOpal();
        if(placed != null)
        {
            placer = placed;
        }
        opalTwo.setOpal(placer.getTeam());
        opalTwo.setPos((int)target.getPos().x, (int)target.getPos().z);
        if (placer.myBoulders != null)
        {
            opalTwo.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            opalTwo.GetComponent<SpriteRenderer>().enabled = true;
            opalTwo.GetComponent<SpriteRenderer>().sprite = placed.myBoulders;
            opalTwo.transform.localScale = new Vector3(3, 3, 1);
        }
        //opalTwo.transform.localPosition = new Vector3(opalTwo.transform.localPosition.x + 0.3f, opalTwo.transform.localPosition.y - 0.1f, opalTwo.transform.localPosition.z - 0.3f);
        opalTwo.setCurrentTile(target);
        target.standingOn(opalTwo);
        allBoulders.Add(opalTwo);
    }

    public void clearGhosts(int x, int y)
    {
        if (x > -1 && x < 10 && y >-1 && y < 10)
        myCursor.clearGhosts(tileGrid[x,y]);
    }

    public void refundMovement(int i)
    {
        myCursor.setDistance(i);
    }

    public void nextTurn()
    {
        myCursor.nextTurn();
    }

    public OpalScript getCurrentOpal()
    {
        return myCursor.getCurrentOpal();
    }

    public string getTileType(int x, int z)
    {
        if(x >= 0 && x <= 9 && z >= 0 && z <= 9)
        {
            return tileGrid[x, z].type;
        }
        return null;
    }

    public int getNextID()
    {
        int i = 0;
        foreach(OpalScript o in gameOpals)
        {
            if(o.getID() != -1)
            {
                i++;
            }
        }
        return i;
    }

    public OpalScript getOpalByID(int id)
    {
        foreach(OpalScript o in gameOpals)
        {
            if(o.getID() == id)
            {
                return o;
            }
        }
        return null;
    }

    public OpalScript getBoulderByID(int id)
    {
        foreach (OpalScript o in allBoulders)
        {
            if (o.getID() == id)
            {
                return o;
            }
        }
        return null;
    }

    public string isVocabWord(string word)
    {
        string strippedWord = "";
        foreach (char c in word) {
            if(c != '.' && c != ',')
            {
                strippedWord += c;
            }
        }
        strippedWord = strippedWord.ToLower();
        return vocab.getDefinition(strippedWord);
    }

    public void setRed(int x, int z, bool red)
    {
        //print(myCursor.getMoving());
        if(x < 10 && x > -1 && z < 10 && z > -1)
            tileGrid[x, z].setRed(red, myCursor.getMoving());
    }

    public IEnumerator screenShake(int intensity, int length)
    {
        Camera target = Camera.main;
        target.transform.localPosition = new Vector3(6, 0, -7);
        intensity = 6;

        bool up = false;
        for (int i = 0; i < length/2 * 2; i++)
        {
            if (up) {
                target.transform.position += Vector3.up * intensity * Time.deltaTime;
                target.transform.position += Vector3.right * intensity * Time.deltaTime;
            }
            else {
                target.transform.position -= Vector3.up * intensity * Time.deltaTime;
                target.transform.position -= Vector3.right * intensity * Time.deltaTime;
            }

            if(i % 2 == 0)
            {
                up = !up;
            }
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
        }
    }

    private void setUpSurroundings()
    {

    }

    public void saveGame(int input)
    {
        string state = "";

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                state += tileGrid[i, j].type+"&";
            }
        }
        state += "^";

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                state += tileGrid[i, j].saveState() + "\n";
            }
        }

        state += "^";

        for(int i = 0; i < nonSortedGameOpals.Count; i++)
        {
            state += nonSortedGameOpals[i].saveStateSB() + "\n";
        }
        state += "^";

        state += myCursor.saveGameState();

        state += "^";

        foreach (OpalScript o in nonSortedGameOpals)
        {
            state += o.getID() + "&";
        }

        state += "^";

        string allBouldersState = "^";

        for (int i = 0; i < allBoulders.Count; i++)
        {
            state += allBoulders[i].saveStateSB() + "\n";
            allBouldersState += allBoulders[i].getID() + '&';
        }

        state += allBouldersState;

        saveStates[input] = state;
    }

    public void loadGame(int input)
    {
        string[] data;
        data = saveStates[input].Split('^');
        string[] tileLayout = data[0].Split('&');
        string[] tileStates = data[1].Split('\n');
        string[] opalStates = data[2].Split('\n');
        string[] allOpals = data[4].Split('&');
        string[] boulderStates = data[5].Split('\n');
        string[] allBouldersState = data[6].Split('&');


        List<OpalScript> leftOut = new List<OpalScript>();
        leftOut.AddRange(nonSortedGameOpals);
        for(int i = 0; i < allOpals.Length; i++)
        {
            if(allOpals[i] != "")
                leftOut.Remove(getOpalByID(int.Parse(allOpals[i])));
        }

        List<OpalScript> bouldersLeftOut = new List<OpalScript>();
        bouldersLeftOut.AddRange(allBoulders);
        for (int i = 0; i < allBouldersState.Length; i++)
        {
            if (allBouldersState[i] != "")
                bouldersLeftOut.Remove(getBoulderByID(int.Parse(allBouldersState[i])));
        }


        if (leftOut.Count > 0)
        {
            foreach(OpalScript o in leftOut)
            {
                nonSortedGameOpals.Remove(o);
                gameOpals.Remove(o);
                if(o.getCurrentTile() != null)
                    o.getCurrentTile().currentPlayer = null;
                Destroy(o.gameObject);
            }
        }

        if (bouldersLeftOut.Count > 0)
        {
            foreach (OpalScript o in bouldersLeftOut)
            {
                allBoulders.Remove(o);
                if (o.getCurrentTile() != null)
                    o.getCurrentTile().currentPlayer = null;
                Destroy(o.gameObject);
            }
        }

        int k = 0;
        k = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                setTile(tileGrid[i, j], tileLayout[k], true);
                k++;
            }
        }


        k = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tileGrid[i, j].loadState(tileStates[k]);
                k++;
            }
        }


        for (int i = 0; i < nonSortedGameOpals.Count; i++)
        {
            nonSortedGameOpals[i].loadState(opalStates[i]);
        }
        for (int i = 0; i < allBoulders.Count; i++)
        {
            if (boulderStates.Length == i)
                break;
            if(boulderStates[i] != "")
                allBoulders[i].loadState(boulderStates[i]);
        }


        foreach (TileScript t in tileGrid)
        {
            t.updateConnection();
        }


        myCursor.loadGameState(data[3]);

    }
}
