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

    public void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        myCurs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        redPlate = Resources.Load<GameObject>("Prefabs/RedPlate");
        bluePlate = Resources.Load<GameObject>("Prefabs/BluePlate");
        greenPlate = Resources.Load<GameObject>("Prefabs/GreenPlate");
        orangePlate = Resources.Load<GameObject>("Prefabs/OrangePlate");
        if(type == "Boulder")
        {
            impassable = true;
        }
    }

    public void setCoordinates(int row, int col)
    {
        gridPos.x = row;
        gridPos.y = col;
        if (type == "Flood")
        {
            transform.position = new Vector3(gridPos.x, -1.1f, gridPos.y);
        }
        else
        {
            transform.position = new Vector3(gridPos.x, -1, gridPos.y);
        }
        if (type.Equals("Grass"))
        {
            if (Random.Range(0, 6) == 0)
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
                temp.transform.localPosition = new Vector3(0, 0.51f, 0);
                if (Random.Range(0, 2) == 0)
                    temp.GetComponent<SpriteRenderer>().flipX = true;
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
                currentPlayer.doBuff(0, -2, 0, false);
                currentPlayer.shrouded = false;
            }
            if (type.Equals("Growth") && currentPlayer != null)
            {
                currentPlayer.doBuff(-2, -2, 0, false);
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
                    player.doBuff(0, 2, 0, false);
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
                    player.doBuff(2, 2, 0, false);
                }
            }
            if(trapEffect != null && currentTrap != "PortalOut")
            {
                doTrap(player);
                clearTrap();
            }
        }
    }

    public void updateTile()
    {
        if (setup)
        {
            if(type != "Grass" && type != "Boulder" && !(type == "Growth" && currentPlayer != null) && !(type == "Flood" && checkNeighbors("Flood")))
            {
                decayTurn--;
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
        return this.transform.position;
    }

    public IEnumerator fall()
    {
        if(currentPlayer != null)
        {
            currentPlayer.setDead();
            StartCoroutine(currentPlayer.shrinker());
            myCurs.checkWin();
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
        if(currentPlayer != null)
        {
            if(currentPlayer.getPos().x != getPos().x || currentPlayer.getPos().z != getPos().z)
            {
                standingOn(null);
                print("Cleared a disassociated ghost");
            }
        }
        else
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
    }

    public void clearTrap()
    {
        currentTrap = null;
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
}
