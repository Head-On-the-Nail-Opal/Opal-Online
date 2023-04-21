using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {
    Vector2 gridPos;
    public Material[] mats = new Material[2];
    public Material grass1;
    public Material grass2;
    public string type;
    public GameObject GrowthEffect;
    public List<Sprite> connectedTileSprites;
    public GameObject changeTexture;
    private GameObject displayNum;
    private Sprite displayThree;
    private Sprite displayTwo;
    private Sprite displayOne;
    private ConnectedTile connectedTile;
    private Color myNormalColor;

    public OpalScript currentPlayer = null;
    public GameObject currentEffect = null;
    private GameObject trapEffect = null;
    private string currentTrap = null;
    private bool starting = true;
    private string team = "";
    private bool wet = false;
    private ParticleSystem wetPart;
    private bool setup = false;
    private GroundScript boardScript;
    private CursorScript myCurs;
    private bool fallen = false;
    private Nature onMe;
    private GameObject redPlate;
    private GameObject bluePlate;
    private GameObject greenPlate;
    private GameObject orangePlate;
    private GameObject myPlate;
    private int decayTurn;
    private bool impassable = false;
    private TileScript link;
    private SpriteRenderer changeSpriteRenderer;
    private bool highlight = false;
    private bool isRed = false;
    private List<string> currentCharms = new List<string>();
    private ParticleSystem charmParticle;
    private ParticleSystem currentCharmParticle;

    private List<List<Sprite>> directions = new List<List<Sprite>>();
    private List<Sprite> east = new List<Sprite>();
    private List<Sprite> north = new List<Sprite>();
    private List<Sprite> west = new List<Sprite>();
    private List<Sprite> south = new List<Sprite>();
    private List<Sprite> northeast = new List<Sprite>();
    private List<Sprite> northwest = new List<Sprite>();
    private List<Sprite> southeast = new List<Sprite>();
    private List<Sprite> southwest = new List<Sprite>();

    private List<SpriteRenderer> parts = new List<SpriteRenderer>();
    private SpriteRenderer decor;
    private Sprite decor1;
    private Sprite decor2;
    private Sprite trampled;
    int anim = 0;
    int animLength = 40;

    private bool showMyTimer = false;

    private Coroutine falling = null;
    private bool fallFlag = true;

    private GameObject myShadow;
    private bool shady = false;

    private Wildlife myWildlife = null;
    private string alternateTexture = "";

    public void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        myCurs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        redPlate = Resources.Load<GameObject>("Prefabs/RedPlate");
        bluePlate = Resources.Load<GameObject>("Prefabs/BluePlate");
        greenPlate = Resources.Load<GameObject>("Prefabs/GreenPlate");
        orangePlate = Resources.Load<GameObject>("Prefabs/OrangePlate");
        charmParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/Charm");
        myNormalColor = GetComponent<MeshRenderer>().material.color;

        setUpShadow();

        if (changeTexture != null)
            changeSpriteRenderer = changeTexture.GetComponent<SpriteRenderer>();
        if(type == "Boulder")
        {
            impassable = true;
        }
        setupTimer();
        showTimer(false);

        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            if (s.gameObject.name == "NW" || s.gameObject.name == "NE" || s.gameObject.name == "SW" || s.gameObject.name == "SE")
            {
                parts.Add(s);
            }
            if(s.gameObject.name == "Decor")
            {
                decor = s;
            }
        }

        connectedTile = transform.GetChild(0).transform.GetComponentInChildren<ConnectedTile>();
        if(connectedTileSprites.Count > 0)
        {
            directions.Add(northwest); 
            directions.Add(north);
            directions.Add(northeast);
            directions.Add(west);
            directions.Add(new List<Sprite>());
            directions.Add(east);
            directions.Add(southwest);
            directions.Add(south);
            directions.Add(southeast);
            int i = 0;
            foreach(Sprite s in connectedTileSprites)
            {
                switch (i)
                {
                    case 0:
                        east.Add(s);
                        southeast.Add(s);
                        south.Add(s);
                        break;
                    case 1:
                        east.Add(s);
                        southeast.Add(s);
                        south.Add(s);
                        southwest.Add(s);
                        west.Add(s);
                        break;
                    case 2:
                        south.Add(s);
                        southwest.Add(s);
                        west.Add(s);
                        break;
                    case 3:
                        south.Add(s);
                        break;
                    case 4:
                        north.Add(s);
                        west.Add(s);
                        break;
                    case 5:
                        north.Add(s);
                        east.Add(s);
                        south.Add(s);
                        southeast.Add(s);
                        northeast.Add(s);
                        break;
                    case 6:
                        north.Add(s);
                        east.Add(s);
                        south.Add(s);
                        west.Add(s);
                        southeast.Add(s);
                        southwest.Add(s);
                        northeast.Add(s);
                        northwest.Add(s);
                        break;
                    case 7:
                        north.Add(s);
                        west.Add(s);
                        south.Add(s);
                        southwest.Add(s);
                        southwest.Add(s);
                        break;
                    case 8:
                        north.Add(s);
                        south.Add(s);
                        break;
                    case 9:
                        north.Add(s);
                        east.Add(s);
                        break;
                    case 10:
                        north.Add(s);
                        northeast.Add(s);
                        east.Add(s);
                        break;
                    case 11:
                        north.Add(s);
                        northeast.Add(s);
                        east.Add(s);
                        west.Add(s);
                        northwest.Add(s);
                        break;
                    case 12:
                        north.Add(s);
                        west.Add(s);
                        northwest.Add(s);
                        break;
                    case 13:
                        north.Add(s);
                        break;
                    case 14:
                        east.Add(s);
                        south.Add(s);
                        break;
                    case 15:
                        east.Add(s);
                        break;
                    case 16:
                        east.Add(s);
                        west.Add(s);
                        break;
                    case 17:
                        west.Add(s);
                        break;
                    case 19:
                        west.Add(s);
                        south.Add(s);
                        break;
                    case 20:
                        north.Add(s);
                        west.Add(s);
                        south.Add(s);
                        east.Add(s);
                        northeast.Add(s);
                        northwest.Add(s);
                        southwest.Add(s);
                        break;
                    case 21:
                        north.Add(s);
                        west.Add(s);
                        south.Add(s);
                        east.Add(s);
                        northeast.Add(s);
                        northwest.Add(s);
                        break;
                    case 22:
                        north.Add(s);
                        west.Add(s);
                        south.Add(s);
                        east.Add(s);
                        northeast.Add(s);
                        break;
                    case 23:
                        north.Add(s);
                        west.Add(s);
                        south.Add(s);
                        east.Add(s);
                        break;
                }

                i++;
            }
        }
    }

    private void setUpShadow()
    {
        if (myShadow == null)
        {
            myShadow = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Shadow"), transform);
            myShadow.transform.localPosition = new Vector3(0, 0.51f, 0);
            myShadow.transform.localScale *= 3;
            
        }
        myShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.3f);
        if(currentPlayer == null)
        {
            myShadow.GetComponent<SpriteRenderer>().enabled = false;
        }
        else if(currentPlayer.getMyName() == "Boulder")
        {
            //print("du hello");
            //myShadow.transform.localScale *= 2;
        }
    }

    public void setShadow(bool doShade)
    {
        myShadow.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.3f);
        myShadow.GetComponent<SpriteRenderer>().enabled = doShade;
        shady = doShade;

    }

    public void setCoordinates(int row, int col)
    {
        if(row == -100 && col == -100)
        {
            if(myWildlife != null)
                myWildlife.adjustState("run");

            foreach (TileScript t in boardScript.getSurroundingTiles(this, true))
            {
                t.scareMyWildlife();
            }
        }
        gridPos.x = row;
        gridPos.y = col;
        if (type == "Flood")
        {
            transform.position = new Vector3(gridPos.x, -1f, gridPos.y);
        }
        else
        {
            transform.position = new Vector3(gridPos.x, -1f, gridPos.y);
        }
        if(type != "Grass" && type != "Boulder")
        {
            decayTurn = 3;
        }
        else
        {
            decayTurn = 100;
        }
        if(type == "Spore")
        {
            decayTurn = 1;
        }
    }

    public void setupTimer()
    {
        displayNum = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/TileTimer"), transform);
        displayNum.transform.localPosition = new Vector3(0.5f, 0.75f, -0.5f);
        displayNum.transform.rotation = Quaternion.Euler(0,-45,0);
        displayNum.transform.localScale = new Vector3(3, 1.5f, 3);
    }

    public void setRed(bool red, bool blue)
    {
        if (blue)
        {
            //print("du hello");
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                //mr.material.color = new Color(225 / 255f, 225 / 255f, 1);
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(225 / 255f, 225 / 255f, 1);
            }
            //isRed = true;
        }
        else if (red)
        {
            foreach(MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()){
                //mr.material.color = new Color(1, 200 / 255f, 200 / 255f);
            }
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(1, 200 / 255f, 200 / 255f);
            }
            //isRed = true;
        }
        else
        {
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
               // mr.material.color = myNormalColor;
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = Color.white;
            }
            //isRed = false;
        }
        setUpShadow();
    }

    public void highlightTwinPortal(bool toggle)
    {
            if (link == null)
                return;
            doHighlight(toggle);
            link.doHighlight(toggle);
    }

    private void doHighlight(bool toggle)
    {
        if (toggle && !highlight)
        {
            trapEffect.transform.localScale *= 1.5f;
            highlight = true;
        }
        else if(!toggle && highlight)
        {
            trapEffect.transform.localScale /= 1.5f;
            highlight = false;
        }
    }

    public void setTimer(int num)
    {
        if (displayNum == null || !showMyTimer)
            return;
        if(num == 3)
        {
            foreach(SpriteRenderer s in displayNum.GetComponentsInChildren<SpriteRenderer>())
            {
                if(s.name == "3")
                {
                    s.enabled = true;
                }
                else
                {
                    s.enabled = false;
                }
            }
        }else if(num == 2)
        {
            foreach (SpriteRenderer s in displayNum.GetComponentsInChildren<SpriteRenderer>())
            {
                if (s.name == "2")
                {
                    s.enabled = true;
                }
                else
                {
                    s.enabled = false;
                }
            }
        }
        else if(num == 1)
        {
            foreach (SpriteRenderer s in displayNum.GetComponentsInChildren<SpriteRenderer>())
            {
                if (s.name == "1")
                {
                    s.enabled = true;
                }
                else
                {
                    s.enabled = false;
                }
            }
        }
        else
        {
            showTimer(false);
        }
    }

    public int getDecayTurn()
    {
        return decayTurn;
    }

    public void reduceDecay(int incr)
    {
        decayTurn--;
        setTimer(decayTurn);
        if (decayTurn < 1)
        {
            if (decayTurn <= 0 && type != "Grass")
            {
                boardScript.setTile((int)getPos().x, (int)getPos().z, "Grass", true);
            }
        }
    }

    public void resetDecay(int i)
    {
        decayTurn += i;
        if (decayTurn > 3)
            decayTurn = 3;
        setTimer(decayTurn);
    }

    public void showTimer(bool show)
    {
        if (displayNum == null)
            return;
        showMyTimer = show;
        foreach (SpriteRenderer sr in displayNum.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = show;
        }
        if (show)
        {
            setTimer(decayTurn);
        }
    }


    //format: type, fallen, decayTurn, currentTrap, linkX, linkZ, end
    public string generateString()
    {
        string output = "";
        output += type + ",";
        output += getFallen() + ",";
        output += decayTurn + ",";
        output += currentTrap + ",";
        if (link != null)
        {
            output += link.getPos().x + ",";
            output += link.getPos().z + ",";
        }
        output += "end";
        return output;
    }

    public void setFromString(string input)
    {
        string[] parsed = input.Split(',');
        if (fallen)
        {
            impassable = true;
        }
        if (parsed[1] == "true")
        {
            print("du hello");
            fallen = true;
            impassable = true;
        }
        else
            fallen = false;
        decayTurn = int.Parse(parsed[2]);
        
        if(parsed[4] != "end")
        {
            link = boardScript.tileGrid[int.Parse(parsed[4]), int.Parse(parsed[5])];
        }

        if(type != parsed[0])
        {
            int tempX = (int)getPos().x;
            int tempZ = (int)getPos().z;
            boardScript.setTile(this, parsed[0], true);
            if(parsed[3] != "")
                boardScript.tileGrid[tempX, tempZ].setTrap(parsed[3]);
        }
    }

    public void toggleStarting()
    {
        if (starting)
        {
            starting = false;
            if (gridPos.x < 2 && gridPos.y >= 2 && gridPos.y <= 7)
            {
                GameObject temp = Instantiate<GameObject>(redPlate, this.transform);
                temp.transform.rotation = Quaternion.Euler(90, 0, 0);
                temp.transform.localPosition = new Vector3(0,0.501f,0);
                temp.transform.localScale = new Vector3(3f, 3f, 1);
                myPlate = temp;
                team = "Red";
                if (onMe != null)
                {
                    onMe.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if(gridPos.x > 7 && gridPos.y >= 2 && gridPos.y <= 7)
            {
                GameObject temp = Instantiate<GameObject>(bluePlate, this.transform);
                temp.transform.rotation = Quaternion.Euler(90, 0, 0);
                temp.transform.localPosition = new Vector3(0, 0.501f, 0);
                temp.transform.localScale = new Vector3(3f, 3f, 1);
                myPlate = temp;
                team = "Blue";
                if (onMe != null)
                {
                    onMe.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if(gridPos.y < 2 && gridPos.x >= 2 && gridPos.x <= 7 && boardScript.numTeams > 2)
            {
                GameObject temp = Instantiate<GameObject>(greenPlate, this.transform);
                temp.transform.rotation = Quaternion.Euler(90, 0, 0);
                temp.transform.localPosition = new Vector3(0, 0.501f, 0);
                temp.transform.localScale = new Vector3(3/26f, 3/26f, 1);
                myPlate = temp;
                team = "Green";
                if (onMe != null)
                {
                    onMe.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
            else if(gridPos.y > 7 && gridPos.x >= 2 && gridPos.x <= 7 && boardScript.numTeams == 4)
            {
                GameObject temp = Instantiate<GameObject>(orangePlate, this.transform);
                temp.transform.rotation = Quaternion.Euler(90, 0, 0);
                temp.transform.localPosition = new Vector3(0, 0.501f, 0);
                temp.transform.localScale = new Vector3(3/26f, 3/26f, 1);
                myPlate = temp;
                team = "Orange";
                if (onMe != null)
                {
                    onMe.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
        else
        {
            if(onMe != null)
            {
                onMe.GetComponent<SpriteRenderer>().enabled = true;
            }
            if(myPlate != null)
            {
                DestroyImmediate(myPlate);
            }
            GetComponentInChildren<SpriteRenderer>().color = Color.white;
        }
    }

    public string getTeam()
    {
        return team;
    }

    public void printName()
    {
        print("Tile[" + gridPos.x + ", "+ gridPos.y + "]");
    }

    public void setWet(bool what)
    {
        if(type == "Flood")
        {
            return;
        }
        if (what == true && wet == false)
        {
            ParticleSystem temp = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/Wet");
            wetPart = Instantiate<ParticleSystem>(temp);
            wetPart.transform.position = new Vector3(getPos().x, 0, getPos().z);
        }else if(what == false && wet == true)
        {
            DestroyImmediate(wetPart);
            wetPart = null;
        }
        wet = what;
    }

    public bool getWet()
    {
        return wet;
    }

	public void standingOn(OpalScript player)
    {
        if (player == null)
        {
            if (type != "Boulder")
                impassable = false;
            if (type.Equals("Miasma") && currentPlayer != null)
            {
                currentPlayer.onMiasma(false);
                currentPlayer.shrouded = false;
            }
            if (type.Equals("Growth") && currentPlayer != null)
            {
                currentPlayer.onGrowth(false);
                DestroyImmediate(currentEffect);
            }
            if (currentPlayer != null)
            {
                myShadow.GetComponent<SpriteRenderer>().enabled = false;
                myShadow.transform.localScale /= currentPlayer.transform.localScale.x/2.5f;
            }
        }
        currentPlayer = player;
        if (player != null)
        {
            if(!fallen)
                impassable = true;
            if (onMe != null && !(player.getMainType() == "Grass" || player.getSecondType() == "Grass"))
            {
                onMe.setTrampled();
            }
            player.toggleFlood(type == "Flood");
            if(type == "Flood")
            {
                player.setBurning(false);
            }
            if (type.Equals("Fire"))
            {
                player.setBurning(true);
            }
            if (type.Equals("Miasma"))
            {
                if (!player.shrouded)
                {
                    player.setPoison(true);
                    currentPlayer.onMiasma(true);
                    player.shrouded = true;
                }
            }
            if (type.Equals("Growth"))
            {
                player.setPoison(false);
                if (currentEffect == null)
                {
                    currentPlayer.onGrowth(true);
                }
            }
            if(trapEffect != null && currentTrap != "PortalOut")
            {
                doTrap(player);
                clearTrap();
            }

            
            myShadow.GetComponent<SpriteRenderer>().enabled = true;
            myShadow.transform.localScale *= player.transform.localScale.x/2.5f;

            if(myWildlife != null)
                myWildlife.adjustState("run");

            foreach(TileScript t in boardScript.getSurroundingTiles(this, true))
            {
                t.scareMyWildlife();
            }

            if (currentCharms.Count > 0)
            {
                List<string> removedCharms = new List<string>();
                foreach (string c in getCharms())
                {
                    string[] parsed = c.Split(',');
                    if (parsed.Length > 1 && parsed[2] == player.getTeam())
                    {
                        player.setCharmFromTile(c);
                        removedCharms.Add(c);
                    }
                }
                removeCharms(removedCharms);
            }

            switch (type) {
                case "Grass":
                    if(player.getMainType() != "Grass" && player.getSecondType() != "Grass")
                    {
                        decor.sprite = trampled;
                    }
                    break;
                case "Growth":
                    if (player.getMainType() != "Grass" && player.getSecondType() != "Grass")
                    {
                        decor.sprite = trampled;
                    }
                    break;
                case "Flood":
                    if (player.getMainType() != "Water" && player.getSecondType() != "Water")
                    {
                        //decor.sprite = trampled;
                    }
                    break;
                case "Miasma":
                    if (player.getMainType() != "Plague" && player.getSecondType() != "Plague")
                    {
                        decor.sprite = trampled;
                    }
                    break;
                case "Fire":
                    if (player.getMainType() != "Fire" && player.getSecondType() != "Fire")
                    {
                        decor.sprite = trampled;
                    }
                    break;

            }

        }
    }

    public bool findSurroundingMeadowebb()
    {
        bool m = false;
        if (currentPlayer == null)
            return false;
        foreach(TileScript t in currentPlayer.getSurroundingTiles(false))
        {
            if(t != null && t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Meadowebb" && t.type == "Growth")
            {
                OpalScript meadowebb = t.getCurrentOpal();
                boardScript.fireProjectile(meadowebb, currentPlayer, 1);
                m = true;
            }
        }
        return m;
    }

    public void setCurrentOpal(OpalScript o)
    {
        currentPlayer = o;
    }

    public void updateTile()
    {
        if (setup)
        {
            if(type != "Grass" && type != "Boulder" && !(type == "Growth" && currentPlayer != null) && !(type == "Flood" && checkNeighbors("Flood")))
            {
                decayTurn--;
                setTimer(decayTurn);
            }
            if(decayTurn <= 0 && type != "Grass")
            {
                boardScript.setTile((int)getPos().x, (int)getPos().z, "Grass", true);
            }
        }
    }

    public void setForUpdate()
    {
        setup = true;
    }

    public Vector3 getPos()
    {
        if(this != null && transform != null)
         return transform.position;
        return new Vector3(0,0,0);
    }

    public void doFall()
    {
        falling = StartCoroutine(fall());
    }

    public IEnumerator fall()
    {
        fallFlag = true;
        if(myWildlife!= null)
            myWildlife.adjustState("run");
        if (currentPlayer != null)
        {
            currentPlayer.setDead();
            StartCoroutine(currentPlayer.shrinker());
            myCurs.checkWin();
        }
        foreach(OpalScript o in boardScript.gameOpals)
        {
            if(o.getCurrentTile() == this && !o.getDead())
            {
                o.setDead();
                StartCoroutine(o.shrinker());
                myCurs.checkWin();
            }
        }
        if (onMe != null)
        {
            onMe.setTrampled();
        }
        fallen = true;
        impassable = true;
        while(transform.position.y > -100 && fallFlag)
        {
            int xRand = Random.Range(-10, 10);
            int zRand = Random.Range(-10, 10);
            transform.position = new Vector3(transform.position.x + xRand*0.002f, transform.position.y - 0.01f, transform.position.z+zRand * 0.002f);
            if (!fallFlag)
                break;
            yield return new WaitForFixedUpdate();
        }
    }

    public void fixWeirdness()
    {
        if(currentPlayer != null && currentPlayer.getMyName() != "Boulder")
        {
            if(currentPlayer.getPos().x != getPos().x || currentPlayer.getPos().z != getPos().z)
            {
                standingOn(null);
                print("Cleared a disassociated ghost");
            }
        }
        else if(currentPlayer == null)
        {
            if(impassable && type != "Boulder" && !fallen)
            {
                impassable = false;
                print("Cleared an impass ghost");
            }
        }
    }

    public bool getFallen()
    {
        return fallen;
    }

    public bool checkNeighbors(string targetType)
    {
        if (fallen)
            return false;
        bool check = true;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                {
                    if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1)
                    {
                        if(boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type != targetType)
                        {
                            check = false;
                        }
                    }
                }
            }
        }
        return check;
    }

    public void setTrap(string trapType)
    {
        if (currentTrap != null)
            return;
        currentTrap = trapType;
        if(trapType == "Thorns")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/Thorns");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0,0.5f,0);
        }else if(trapType == "Sunflower" || trapType == "Honeysuckle" || trapType == "Rose")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/Flower");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else if (trapType == "PortalIn")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/PortalIn");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else if (trapType == "PortalOut")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/PortalOut");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else if (trapType == "Tomb")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/Tomb");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
        else if (trapType == "Mist")
        {
            GameObject temp = Resources.Load<GameObject>("Prefabs/ParticleSystems/Traps/Mist");
            trapEffect = Instantiate<GameObject>(temp, this.transform);
            trapEffect.transform.localPosition = new Vector3(0, 0.5f, 0);
        }
    }

    public void clearTrap()
    {
        currentTrap = null;
        if(trapEffect != null)
            DestroyImmediate(trapEffect.gameObject);
        trapEffect = null;
    }

    public void doTrap(OpalScript target)
    {
        switch (currentTrap)
        {
            case "Thorns":
                target.takeDamage(16, true, true);
                target.doTempBuff(2, 2, -3);
                DestroyImmediate(trapEffect);
                break;
            case "Sunflower":
                target.doTempBuff(1, -1, 3);
                target.doTempBuff(0, -1, 3);
                DestroyImmediate(trapEffect);
                break;
            case "Honeysuckle":
                target.doHeal(8, false);
                DestroyImmediate(trapEffect);
                break;
            case "Rose":
                target.takeDamage(16, true, true);
                DestroyImmediate(trapEffect);
                break;
            case "PortalIn":
                int temp = target.doMove((int)link.getPos().x, (int)link.getPos().z, 0);
                if(temp > -1)
                {
                    currentPlayer = null;
                }
                //DestroyImmediate(trapEffect);
                link.clearTrap();
                link = null;
                break;
            case "Tomb":
                boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Boulder", false);
                boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Boulder", false);
                boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Boulder", false);
                boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Boulder", false);
                break;
            case "Mist":
                foreach(OpalScript o in target.getCursedBy())
                {
                    if(o.getMyName() == "Mistery")
                    {
                        target.takeDamage(14,true, true);
                        DestroyImmediate(trapEffect);
                        break;
                    }
                }
                break;
        }
    }

    public void addCharm(string c)
    {
        currentCharms.Add(c);
        if (currentCharmParticle == null)
            currentCharmParticle = Instantiate<ParticleSystem>(charmParticle, transform);
    }

    public void removeCharms()
    {
        currentCharms.Clear();
        if (currentCharmParticle != null)
        {
            Destroy(currentCharmParticle.gameObject);
            currentCharmParticle.Clear();
        }
    }


    public void removeCharms(List<string> removeUs)
    {
        foreach(string c in removeUs)
        {
            currentCharms.Remove(c);
        }
        if(currentCharms.Count == 0)
        {
            removeCharms();
        }
    }

    public List<string> getCharms()
    {
        if (currentCharms.Count == 0)
            return new List<string>() { "None" };
        return currentCharms;
    }

    public string getTrap()
    {
        return currentTrap;
    }

    public bool getImpassable()
    {
        return impassable;
    }

    public void setImpassable(bool imp)
    {
        impassable = imp;
    }

    public void setLink(TileScript l)
    {
        link = l;
    }

    public int getLife()
    {
        return decayTurn;
    }

    public OpalScript getCurrentOpal()
    {
        if(currentPlayer == null)
        {
            foreach(OpalScript o in boardScript.gameOpals)
            {
                if(o.getPos().x == getPos().x && o.getPos().z == getPos().z)
                {
                    standingOn(o);
                }
            }
        }
        else
        {
            if (currentPlayer.getPos().x != getPos().x || currentPlayer.getPos().z != getPos().z)
            {
                standingOn(null);
            }
        }
        return currentPlayer;
    }

    public List<TileScript> getSurroundingTiles(bool adj)
    {
        
        List<TileScript> output = new List<TileScript>();
        if (this == null)
            return output;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                //print(getPos().x + i + " " + getPos().z + j);
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && !(i == 0 && j == 0) && (!adj || (Mathf.Abs(i) != Mathf.Abs(j))))
                {
                    output.Add(boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j]);
                }
            }
        }
        return output;
    }

    public void determineShape()
    {
        return;
        if (type != "Flood")
            return;
        string shape = "";
        int dec = 1;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (boardScript.getTileType((int)gridPos.x + i, (int)gridPos.y + j) == "Flood")
                {
                    shape += 4;
                }
                else
                {
                    shape += 1;
                }
                dec *= 10;
            }
        }
        //print("(X:" + gridPos.x + ",Y:" + gridPos.y + ") = " + shape);
        //changeSpriteRenderer.sprite = connectedTileSprites[18];
        //connectedTile.changeSprite(shape);
    }


    public void updateConnection()
    {
        if (this == null)
            return;
        string setUp = getSurroundingTiles();

        List<int> shapes = new List<int>();

        shapes.Add(calcShape(int.Parse(setUp.Substring(0, 1)), int.Parse(setUp.Substring(1, 1)), int.Parse(setUp.Substring(3, 1)))); //NW

        shapes.Add(calcShape(int.Parse(setUp.Substring(2, 1)), int.Parse(setUp.Substring(1, 1)), int.Parse(setUp.Substring(5, 1)))); //NE

        shapes.Add(calcShape(int.Parse(setUp.Substring(6, 1)), int.Parse(setUp.Substring(7, 1)), int.Parse(setUp.Substring(3, 1)))); //SW

        shapes.Add(calcShape(int.Parse(setUp.Substring(8, 1)), int.Parse(setUp.Substring(7, 1)), int.Parse(setUp.Substring(5, 1)))); //SE


        if (true)
        {
            string endTag = "";
            if (type == "Grass")
            {
                if (transform.position.x % 2 == 0 && transform.position.z % 2 != 0 || transform.position.x % 2 != 0 && transform.position.z % 2 == 0)
                    endTag = "D";

            }
            for (int i = 0; i < 4; i++)
            {
                Rect r = new Rect(shapes[i] * 16, 8, 8, 8);

                if (i == 1)
                {
                    r = new Rect(shapes[i] * 16 + 8, 8, 8, 8);
                }
                else if (i == 2)
                {
                    r = new Rect(shapes[i] * 16, 0, 8, 8);
                }
                else if (i == 3)
                {
                    r = new Rect(shapes[i] * 16 + 8, 0, 8, 8);
                }

                string textureName = type + endTag;
                if (alternateTexture != "")
                    textureName = alternateTexture;
                Sprite tempS = Sprite.Create(Resources.Load<Texture2D>("SpriteSheets/Board4P/"+ textureName), r, new Vector2(r.width / 16f, r.height / 16f));
                parts[i].sprite = tempS;
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public void setTexture(string alt)
    {
        alternateTexture = alt;
        
    }

    public string getTexture()
    {
        return alternateTexture;
    }

    private int calcShape(int num0, int num1, int num2)
    {
        if (num0 == 0 && num1 == 0 && num2 == 0)
        {
            return 0;
        }

        if (num0 == 1 && num1 == 1 && num2 == 1)
        {
            return 4;
        }

        if (num1 == 0 && num2 == 1)
        {
            return 1;
        }
        if (num1 == 1 && num2 == 0)
        {
            return 2;
        }
        if (num1 == 1 && num2 == 1)
        {
            return 3;
        }

        return 0;
    }

    private string getSurroundingTiles()
    {
        string output = "";

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (this == null)
                    return output;
                TileScript t = boardScript.getTile((int)transform.position.x + j, (int)transform.position.z - i);
                if (t == this)
                    output += 'X';
                else if(t == null)
                    output += '0';
                else if (t.type == type && alternateTexture == t.getTexture())
                    output += '1';
                else
                    output += '0';
            }
        }

        return output;
    }

    public void setRandomDecor()
    {
        switch (type) {
            case "Grass":
                if (Random.Range(0, 4) != 0)
                    return;
                break;
            case "Growth":
                break;
            case "Flood":

                break;
            case "Miasma":
                callParticleEffect("MiasmaGroundEffect");
                break;
            case "Fire":
                callParticleEffect("BurningGroundEffect");
                if (Random.Range(0, 2) != 0)
                    return;
                break;

        }

        if (alternateTexture != "")
            return;

        Texture2D textures = Resources.Load<Texture2D>("SpriteSheets/Board4P/Decor-" + type);
        if (textures == null)
            return;
        int rand = Random.Range(0, (textures.width / 16)/3);

        Rect r = new Rect(rand * 16*3, 0, 16, 16);

        Sprite tempS = Sprite.Create(Resources.Load<Texture2D>("SpriteSheets/Board4P/Decor-" + type), r, new Vector2(r.width / 32, r.height / 16));
        decor.sprite = tempS;
        decor1 = tempS;

        r = new Rect(rand * 16 * 3 + 16, 0, 16, 16);
        decor2 = Sprite.Create(Resources.Load<Texture2D>("SpriteSheets/Board4P/Decor-" + type), r, new Vector2(r.width / 32, r.height / 16));

        r = new Rect(rand * 16 * 3 + 32, 0, 16, 16);
        trampled = Sprite.Create(Resources.Load<Texture2D>("SpriteSheets/Board4P/Decor-" + type), r, new Vector2(r.width / 32, r.height / 16));

        float randX = Random.Range(-3, 4) * 0.01f;
        decor.transform.localPosition = new Vector3(0.1f + randX, 0.8f, -0.1f + randX);
        if (Random.Range(0, 2) == 1 && type != "Flood")
            decor.flipX = true;
    }

    public void callParticleEffect(string name)
    {
        ParticleSystem ps = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/" + name), transform);
    }

    public void FixedUpdate()
    {
        if(currentPlayer == null && shady)
        {
            setShadow(false);
        }
        if(anim == animLength)
        {
            anim = 0;
            if(decor.sprite != trampled)
            {
                if(decor.sprite == decor1)
                {
                    decor.sprite = decor2;
                }
                else
                {
                    decor.sprite = decor1;
                }
            }
        }

        anim++;
    }

    public string saveState()
    {
        string output = "";

        output += currentTrap + "|";
        output += decayTurn + "|";
        output += fallen + "|";

        if (link != null) {
            output += link.getPos().x + "|";
            output += link.getPos().z + "|";
        }
        else
        {
            output += "none|none|";
        }
        
        foreach(string charm in currentCharms)
        {
            output += charm + "&";
        }

        return output;
    }

    public void loadState(string input)
    {
        string[] data = input.Split('|');

        if(data[0] != "")
            setTrap(data[0]);

        decayTurn = (int.Parse(data[1]));
        setTimer(decayTurn);

        if(data[2] == "True")
        {
            if(!fallen)
                doFall();
        }
        else
        {
            fallFlag = false;
            fallen = false;
            impassable = false;
            transform.position = new Vector3(gridPos.x, -1, gridPos.y);
        }

        if(data[3] != "none")
        {
            link = boardScript.tileGrid[int.Parse(data[3]), int.Parse(data[4])];
        }

        if (data[5] != "")
        {
            string[] cs = data[5].Split('&');
            foreach (string c in cs)
            {
                if (c != "" && c != "None")
                {
                    addCharm(c);
                }
            }
        }
    }

    public void spawnWildlife(string wName)
    {
        if (starting)
            return;
        myWildlife = Instantiate<Wildlife>(Resources.Load<Wildlife>("Prefabs/Wildlife/"+ wName));
        float xRand = Random.Range(-20, 20) * 0.01f;
        //float yRand = Random.Range(-30, 30) * 0.01f;
        myWildlife.transform.position = new Vector3(transform.position.x+xRand, transform.position.y+1.35f, transform.position.z+xRand);
        myWildlife.transform.eulerAngles = new Vector3(40, -45, 0);
        myWildlife.transform.localScale *= 0.75f;
    }

    public void scareMyWildlife()
    {
        if (myWildlife != null)
            myWildlife.adjustState("scared");
    }

}
