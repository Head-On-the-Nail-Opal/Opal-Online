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

    private List<List<Sprite>> directions = new List<List<Sprite>>();
    private List<Sprite> east = new List<Sprite>();
    private List<Sprite> north = new List<Sprite>();
    private List<Sprite> west = new List<Sprite>();
    private List<Sprite> south = new List<Sprite>();
    private List<Sprite> northeast = new List<Sprite>();
    private List<Sprite> northwest = new List<Sprite>();
    private List<Sprite> southeast = new List<Sprite>();
    private List<Sprite> southwest = new List<Sprite>();


    public void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        myCurs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        redPlate = Resources.Load<GameObject>("Prefabs/RedPlate");
        bluePlate = Resources.Load<GameObject>("Prefabs/BluePlate");
        greenPlate = Resources.Load<GameObject>("Prefabs/GreenPlate");
        orangePlate = Resources.Load<GameObject>("Prefabs/OrangePlate");
        if (changeTexture != null)
            changeSpriteRenderer = changeTexture.GetComponent<SpriteRenderer>();
        if(type == "Boulder")
        {
            impassable = true;
        }
        setupTimer();
        showTimer(false);
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

    public void setCoordinates(int row, int col)
    {
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
        if (type.Equals("Grass"))
        {
            if (Random.Range(0, 4) == 0)
            {
                int natureNum = Random.Range(0, 5);
                if (natureNum == 0)
                {
                    onMe = Resources.Load<Nature>("Prefabs/Nature1");
                }else if (natureNum == 1)
                {
                    onMe = Resources.Load<Nature>("Prefabs/Nature2");
                }
                else if(natureNum == 2)
                {
                    onMe = Resources.Load<Nature>("Prefabs/Nature3");
                }
                else if (natureNum == 3)
                {
                    onMe = Resources.Load<Nature>("Prefabs/Nature4");
                }
                else if (natureNum == 4)
                {
                    onMe = Resources.Load<Nature>("Prefabs/Nature5");
                }
                Nature temp = Instantiate<Nature>(onMe, this.transform);
                int xRand = Random.Range(1, 3);
                int zRand = Random.Range(1, 3);
                temp.transform.localPosition = new Vector3(0.43f, 0.7f, -0.43f);
                if(natureNum == 4)
                    temp.transform.localPosition = new Vector3(0.48f, 0.72f, -0.48f);
                temp.transform.localScale = new Vector3(0.2f, 0.1f, 0);
                if(natureNum == 3)
                {
                    temp.transform.localScale = new Vector3(0.3f, 0.15f, 0);
                    temp.transform.rotation = Quaternion.Euler(40, -45, 0);
                    temp.transform.localPosition = new Vector3(0.44f, 0.70f, -0.44f);
                }
                if(natureNum == 4)
                    temp.transform.localScale = new Vector3(0.25f, 0.125f, 0);
                if (Random.Range(0, 2) == 0)
                {
                    temp.GetComponent<SpriteRenderer>().flipX = true;
                    foreach(SpriteRenderer sr in temp.GetComponentsInChildren<SpriteRenderer>())
                    {
                        sr.flipX = true;
                    }
                    if(natureNum == 0 || natureNum == 1 || natureNum == 2)
                        temp.transform.localPosition = new Vector3(0.5f, 0.72f, -0.5f);
                    if(natureNum == 3)
                    {
                        
                        temp.transform.localPosition = new Vector3(0.44f, 0.70f, -0.44f);
                    }
                }
                onMe = temp;
            }
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
                mr.material.color = new Color(225 / 255f, 225 / 255f, 1);
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
                mr.material.color = new Color(1, 100 / 255f, 100 / 255f);
            }
            foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = new Color(1, 100 / 255f, 100 / 255f);
            }
            //isRed = true;
        }
        else
        {
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.material.color = Color.white;
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = Color.white;
            }
            //isRed = false;
        }
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
        if (displayNum == null)
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

    public void showTimer(bool show)
    {
        if (displayNum == null)
            return;
        displayNum.SetActive(show);
        if(show)
            setTimer(decayTurn);
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
                //currentPlayer.doTempBuff(1, -1, -2, false);
                currentPlayer.onMiasma(false);
                currentPlayer.shrouded = false;
            }
            if (type.Equals("Growth") && currentPlayer != null)
            {
                //currentPlayer.doTempBuff(0, -1, -2, false);
                //currentPlayer.doTempBuff(1, -1, -2, false);
                currentPlayer.onGrowth(false);
                DestroyImmediate(currentEffect);
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
                    //player.doTempBuff(1, -1, 2, false);
                    currentPlayer.onMiasma(true);
                    player.shrouded = true;
                }
            }
            if (type.Equals("Growth"))
            {
                player.setPoison(false);
                if (currentEffect == null)
                {
                    GameObject temp = Instantiate<GameObject>(GrowthEffect, this.transform);
                    temp.transform.position = new Vector3(gridPos.x, -1f, gridPos.y);
                    currentEffect = temp;
                    player.setPoison(false);
                    //player.doTempBuff(0, -1, 2, false);
                    //player.doTempBuff(1, -1, 2, false);
                    currentPlayer.onGrowth(true);
                }
            }
            if(trapEffect != null && currentTrap != "PortalOut")
            {
                doTrap(player);
                clearTrap();
            }
        }
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

    public IEnumerator fall()
    {
        if(currentPlayer != null)
        {
            currentPlayer.setDead();
            StartCoroutine(currentPlayer.shrinker());
            myCurs.checkWin();
        }
        foreach(OpalScript o in boardScript.gameOpals)
        {
            if(o.getCurrentTile() == this)
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
        while(transform.position.y > -100)
        {
            int xRand = Random.Range(-10, 10);
            int zRand = Random.Range(-10, 10);
            transform.position = new Vector3(transform.position.x + xRand*0.002f, transform.position.y - 0.01f, transform.position.z+zRand * 0.002f);
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

    public void determineShape()
    {
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
        print("(X:" + gridPos.x + ",Y:" + gridPos.y + ") = " + shape);
        changeSpriteRenderer.sprite = connectedTileSprites[18];
        connectedTile.changeSprite(shape);
    }

    public void determineShapeALSOOLD()
    {
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
        print("(X:" + gridPos.x + ",Y:" + gridPos.y + ") = " + shape);
        changeSpriteRenderer.sprite = connectedTileSprites[18];

        int dir0 = 0;
        List<Sprite> maybeMe = new List<Sprite>();
        List<Sprite> notMe = new List<Sprite>();
        foreach(List<Sprite> ls in directions)
        {
            foreach(Sprite s in ls)
            {
                if (shape[dir0] == '4' && notMe.Contains(s)) //check its 
                {
                    if (maybeMe.Contains(s))
                        maybeMe.Remove(s);
                } else if (shape[dir0] == '1' && maybeMe.Contains(s))
                {
                    maybeMe.Remove(s);
                    if (!notMe.Contains(s))
                    {
                        notMe.Add(s);
                    }
                }
                else if (shape[dir0] == '4' && !maybeMe.Contains(s) && !notMe.Contains(s))
                {
                    maybeMe.Add(s);
                }
                else if (shape[dir0] == '1' && !notMe.Contains(s))
                {
                    notMe.Add(s);
                    if (maybeMe.Contains(s))
                        maybeMe.Remove(s);
                }
            }
            dir0++;
        }

        foreach(Sprite s in maybeMe)
        {
            if (notMe.Contains(s))
            {
                maybeMe.Remove(s);
            }
        }
        if(maybeMe.Count >= 1)
        {
            changeSpriteRenderer.sprite = maybeMe[maybeMe.Count-1];
        }
        print(maybeMe.Count);
    }

    public void determineShapeOLD() 
    {
        int shape = 0;
        int dec = 1;
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(boardScript.getTileType((int)gridPos.x+i, (int)gridPos.y+j) == "Flood")
                {
                    shape += 4 * dec;
                }
                else
                {
                    shape += 1 * dec;
                }
                dec *= 10;
            }
        }
        if(shape == 111141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[18];
        }else if(shape == 444444444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[6];
        }
        else if (shape == 441441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[0];
        }
        else if (shape == 144144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[2];
        }
        else if (shape == 111441441)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[10];
        }
        else if (shape == 111144144)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[12];
        }
        else if(shape == 144144144)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[7];
        }
        else if (shape == 441441441)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[5];
        }
        else if (shape == 111444444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[11];
        }
        else if (shape == 444444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[1];
        }
        else if (shape == 141141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (shape == 141141444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if (shape == 444141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if (shape == 111141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }else if(shape == 141141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if(shape == 141444141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[23];
        }else if(shape == 111141444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }
        else if (shape == 444141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (shape == 411441411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (shape == 114144114)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }else if(shape == 111441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (shape == 111144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
        else if (shape == 111444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[16];
        }
        else if (shape == 411441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (shape == 114144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
        else if (shape == 141444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[0];
        }
        else if (shape == 441141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (shape == 141441141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[5];
        }
        else if (shape == 111441411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }

        else if(shape == 111411411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[11];
        }
        else if(shape == 111444141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if(shape == 111144114)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }

        else
        {
            print("(X:" + gridPos.x + ",Y:" + gridPos.y + ") = " + shape);
        }
    }

}
