using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class OpalScript : MonoBehaviour {
    protected int speed;
    protected string myName;
    protected int maxHealth;
    protected int health;
    protected int attack;
    protected int tempattack;
    protected int defense;
    protected int priority;
    protected int tempdefense;
    protected int tempspeed;
    protected float offsetX;
    protected float offsetY;
    protected float offsetZ;
    protected string player;
    protected bool dead = false;
    protected int jumpCount;
    protected string type1;
    protected string type2;
    public Attack[] Attacks = new Attack[4];
    protected List<TempBuff> buffs = new List<TempBuff>();
    protected bool burning = false;
    protected bool poisoned = false;
    protected bool lifted = false;
    protected bool skipfirstTurn = false;
    protected bool myTurn = false;
    protected List<int> bannedAttacks = new List<int>();
    protected ParticleSystem burningParticle;
    protected ParticleSystem poisonedParticle;
    protected ParticleSystem liftedParticle;
    private ParticleSystem currentBurn;
    private ParticleSystem currentPoison;
    private ParticleSystem currentLift;
    private ParticleSystem armorParticle;
    protected bool moveAfter = false;
    protected DamageResultScript damRes;
    protected int swarmLimit = 4;
    protected int burnTimer = 0;
    protected int poisonTimer = 0;
    protected int liftTimer = 0;
    protected int armor = 0;
    public bool display = false;
    protected List<ParticleSystem> armors = new List<ParticleSystem>();
    private int matchID = -1;
    private int minionCount = 0;
    private string myCharm;
    private bool charmRevealed = false;


    public bool shrouded = false;
    protected GroundScript boardScript;
    protected TileScript lastTile;
    protected TileScript currentTile;
    protected Animator anim;
    protected int charge;
    protected bool attackAgain = false;
    protected bool og = false;
    protected string variant = "0";

    private int burnCounter = 2;
    public int poisonCounter = 4;
    private int skin = 0;

    private GameObject playerIndicator;
    private bool setTeam = false;
    private GameObject mySpot;

    private bool earlyEnd = false;
    private Spiritch spiritchPrefab;
    private string personality = "Straight-Edge";

    private Vector3 startTile;
    private bool takenDamage = false;

    private Vector3 coordinates = new Vector3();

    private void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        transform.position = new Vector3(5, 0.5f, 5);
        anim = GetComponent<Animator>();
        burningParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassiveBurn");
        poisonedParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassivePoison");
        liftedParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassiveLift");
        armorParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/Armor");
        damRes = Resources.Load<DamageResultScript>("Prefabs/AttackResult");
        playerIndicator = Resources.Load<GameObject>("Prefabs/TeamLabel");
        spiritchPrefab = Resources.Load<Spiritch>("Prefabs/Opals/Spiritch");
        playerIndicator = Resources.Load<GameObject>("Prefabs/TeamLabel");
        GameObject iDBody = GameObject.Find("ItemDescriptions");
        ItemDescriptions iD = iDBody.GetComponent<ItemDescriptions>();
        iD.setUp();
        //if(boardScript != null && !boardScript.getMult())
            //myCharm = iD.getRandomCharmName();
        onAwake();
    }

    void Start () {
        //transform.position = new Vector3(5,0.5f,5);
        
    }

    public void setOpal(int h, int a, int d, int s, string n, float scale, float ofX, float ofY, float ofZ, string p, string t1, string t2)
    {
        health = h;
        maxHealth = h;
        attack = a;
        defense = d;
        speed = s;
        myName = n;
        transform.localScale = transform.localScale * scale;
        offsetX = ofX;
        offsetY = ofY;
        offsetZ = ofZ;
        player = p;
        type1 = t1;
        type2 = t2;
    }

    virtual public void setOpal(string p)
    {
        health = 0;
        maxHealth = 0;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 0;
        myName = "";
        transform.localScale = transform.localScale;
        offsetX = 0;
        offsetY = 0;
        offsetZ = 0;
        player = p;
        type1 = "";
        type2 = "";
    }

    //Start changes

    void Update () {
        if((mySpot == null || !setTeam) && player != null && playerIndicator != null)
        {
            GameObject spot = Instantiate<GameObject>(playerIndicator);
            spot.transform.localPosition = new Vector3(0,-1.1f ,0);
            spot.transform.localScale = new Vector3(1f / transform.localScale.x/3, 0.0001f / transform.localScale.y, 1/transform.localScale.x/3);
            spot.transform.SetParent(this.transform, false);
            setTeam = true;
            Material temp = new Material(Shader.Find("Standard"));
            if(player == "Red")
            {
                temp.color = Color.red;
            }else if(player == "Blue")
            {
                temp.color = Color.blue;
            }
            else if(player == "Green")
            {
                temp.color = Color.green;
            }
            else if(player == "Orange")
            {
                temp.color = new Color(1, 0.5f, 0);
            }
            foreach(MeshRenderer m in spot.GetComponentsInChildren<MeshRenderer>())
            {
                m.material = temp;
            }
            mySpot = spot;
            showSpot(false);
            StartCoroutine(spinSpot());
        }
        if(transform.position.x < -99 || display)
        {
            return;
        }
        if (player != null)
        {
            //currentTile = boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)];
            if (currentTile != lastTile)
            {
                //currentTile.standingOn(this);
                lastTile = currentTile;
            }
        }
	}

    

    //Variant documentation - changed from griff's version
    //skin is decided by the last two digits
    //particlesystem by second to last two digits
    //particlecolor by third to last two digits

    public void setVariant(string num)
    {
        List<Color> clrs = new List<Color>(){ Color.red, Color.green, Color.blue, Color.cyan, Color.black, Color.gray, Color.magenta, Color.white, new Color(255/256f, 165/256f, 0), new Color(212/256f, 175/256f, 55/256f), new Color(0,102/256f,0), new Color(1,153/255f,1) };
        List<string> particles = new List<string>(){"", "Prefabs/World/Particles/Rarity-Burn", "Prefabs/World/Particles/Rarity-Drip", "Prefabs/World/Particles/Rarity-Twinkle", "Prefabs/World/Particles/Rarity-Crown", "Prefabs/World/Particles/Rarity-Dusty", "Prefabs/World/Particles/Rarity-Hyperspeed", "Prefabs/World/Particles/Rarity-Portal" };
        List<float> sizes = new List<float>() {1f, 0.6f, 0.8f, 1.2f, 1.4f };
        variant = myName + num;
        int intNum = int.Parse(num);
        skin = intNum % 100;
        int particleSystem = (intNum / 100) % 100;
        int particleColor = (intNum / 10000) % 100;
        int size = (intNum / 1000000) % 100;
        if (particleSystem != 0)
        {
            string path = particles[particleSystem];
            ParticleSystem myPart = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>(path), this.transform);
            myPart.transform.localPosition = new Vector3(0, 0, -2);
            myPart.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            if(particleSystem == 6 || particleSystem == 7)
            {
                myPart.transform.localPosition = new Vector3(0, 0, 0.1f);
            }
            var main = myPart.main;
            main.startColor = new ParticleSystem.MinMaxGradient(clrs[particleColor]);
        }
        //print(num + ", Skin: " + skin + " Particle: " + particleSystem + " Color: " + particleColor);
        if (og && skin != 0)
        {
            transform.localScale *= 2;
        }
        if (anim != null && myCharm == "Goreilla Suit") {
            anim.CrossFade("Goreilla", 0);
        }
        else if(anim != null)
        {
            anim.CrossFade(myName + skin, 0);
        }
        transform.localScale *= sizes[size];
        foreach(Transform t in GetComponentInChildren<Transform>())
        {
            t.localScale *= sizes[size];
        }
        anim.CrossFade(myName + skin, 0);
    }

    public string convertToString(OpalScript o)
    {
        return o.getMyName();
    }

    public OpalScript convertFromString(string s)
    {
        OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/" + s));
        return temp;
    }

    public string saveOpal()
    {
        return getMyName() +'|'+ getCharm() +'|'+ getPersonality();
    }

    public void setFromSave(string data)
    {
        string[] parsed = data.Split('|');
        myCharm = parsed[1];
        personality = parsed[2];
    }

    public bool getCharmRevealed()
    {
        return charmRevealed;
    }

    public void proccessPersonality(string p)
    {
        //print("I am " + p);
        switch (p) {
            case "Proud":
                attack += 1;
                defense -= 1;
                break;
            case "Reserved":
                attack -= 1;
                defense += 1;
                break;
            case "Risk-Taker":
                attack += 2;
                defense -= 2;
                break;
            case "Worried":
                attack -= 2;
                defense += 2;
                break;
            case "Tactical":
                attack += 3;
                speed -= 1;
                break;
            case "Cautious":
                defense += 3;
                speed -= 1;
                break;
            case "Relaxed":
                maxHealth += 5;
                health += 5;
                speed -= 1;
                break;
            case "Optimistic":
                maxHealth += 5;
                health += 5;
                attack -= 2;
                break;
            case "Pesimistic":
                maxHealth += 5;
                health += 5;
                defense -= 2;
                break;
            case "Impatient":
                speed += 1;
                attack -= 2;
                defense -= 2;
                break;
        }
    }

    public string getCharm()
    {
        return myCharm;
    }

    public void setCharm(string i)
    {
        //print("Set charm to " + i.getName());
        myCharm = i;
    }
    
    /**public void setCharm(string n)
    {
        myCharm = new Charm();
        myCharm.setName(n);
        print(getMyName() + "'s charm is set to " + myCharm.getName());
    }*/

    public void setDetails(OpalScript o)
    {
        myCharm = o.getCharm();
        personality = o.getPersonality();
        //proccessPersonality(personality);
    }

    public void summonParticle(string name)
    {
        ParticleSystem temp = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/" + name);
        ParticleSystem inst = Instantiate<ParticleSystem>(temp, this.transform);
        inst.transform.localScale = transform.localScale;
        inst.transform.localRotation = Quaternion.Euler(0,90,35);
    }

    public virtual void preFire(int attackNum, TileScript target)
    {
        return;
    }

    public virtual void onAwake()
    {
        return;
    }


    //format: name, currentTeam, currentHealth, burning, burnAmount, burnTimer, poisoned, poisonAmount, poisonTimer, lifted, liftTimer, armor, charge, positionX, positionY, buffs..., end
    //
    public string generateString()
    {
        string output = "";
        output += myName + ",";
        output += getTeam() + ",";
        output += getHealth() + ",";
        output += burning + ",";
        output += burnCounter + ",";
        output += burnTimer + ",";
        output += poisoned + ",";
        output += poisonCounter + ",";
        output += poisonTimer + ",";
        output += lifted + ",";
        output += liftTimer + ",";
        output += armor + ",";
        output += charge + ",";
        output += (int)getPos().x + ",";
        output += (int)getPos().z + ",";
        //format targetStat-turnLength-Amount,
        foreach (TempBuff t in buffs)
        {
            if(t.getTurnlength() == -1 || t.getTurnlength() > 0)
            {
                output += t.getTargetStat() + ".";
                output += t.getTurnlength() + ".";
                output += t.getAmount() + ",";
            }
        }
        output += "end";
        return output;
    }

    //don't want to recreate them if we don't have to, would be better if we can just alter pre-existing stats
    public void setFromString(string input)
    {
        string[] parsed = input.Split(',');
        if (player == null)
        {
            setOpal(parsed[1]);
        } 
            player = parsed[1];

            setHealth(int.Parse(parsed[2]));
            if(health == 0)
            {
                TileScript temp = currentTile;
                if (currentTile != null)
                    temp.standingOn(null);
                currentTile = null;
                if (temp != null)
                    temp.setImpassable(false);
                dead = true;
                StartCoroutine(shrinker());
            }

            if(parsed[3] == "true")
                burning = true;
            else
                burning = false;
            burnCounter = int.Parse(parsed[4]);
            burnTimer = int.Parse(parsed[5]);

            if (parsed[6] == "true")
                poisoned = true;
            else
                poisoned = false;
            poisonCounter = int.Parse(parsed[7]);
            poisonTimer = int.Parse(parsed[8]);

            if (parsed[9] == "true")
                lifted = true;
            else
                lifted = false;
            liftTimer = int.Parse(parsed[10]);

            armor = int.Parse(parsed[11]);
            charge = int.Parse(parsed[12]);

            setPos(int.Parse(parsed[13]), int.Parse(parsed[14]));
            currentTile = boardScript.tileGrid[int.Parse(parsed[13]), int.Parse(parsed[14])];
            currentTile.standingOn(this);

            clearBuffs();
            int i = 15;
            while(parsed[i] != "end")
            {
                string[] inceptionParsed = parsed[i].Split('.');
                doTempBuff(int.Parse(inceptionParsed[0]), int.Parse(inceptionParsed[1]), int.Parse(inceptionParsed[2]));
                i++;
            }
    }

    public void setID(int id)
    {
        matchID = id;
    }

    public int getID()
    {
        return matchID;
    }

    public void showSpot(bool show)
    {
        if(mySpot == null || (myTurn && !show))
        {
            return;
        }
        if (show)
        {
            foreach (MeshRenderer m in mySpot.GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = true;
            }
        }
        else
        {
            foreach (MeshRenderer m in mySpot.GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = false;
            }
        }
    }

    public int getVariantNum()
    {
        return int.Parse(variant.Substring(myName.Length));
    }

    public bool getOG()
    {
        return og;
    }

    //end changes

    public string getVariant()
    {
        return variant;
    }

    public void setPersonality(string per)
    {
        personality = per;
    }

    public string getPersonality()
    {
        return personality;
    }

    public void addArmor(int add)
    {
        onArmorItem(add);
        armor += add;
        if(armor < 0)
        {
            armor = 0;
        }
        foreach (ParticleSystem a in armors)
        {
            DestroyImmediate(a.gameObject);
        }
        armors.Clear();
        for (int i = 0; i < armor; i++)
        {
            ParticleSystem p = Instantiate<ParticleSystem>(armorParticle, this.transform);
            armors.Add(p);
        }
    }

    public int getArmor()
    {
        return armor;
    }

    public void setPos(int x, int y)
    {
        transform.position = new Vector3(x + offsetX, 0.5f + offsetY, y + offsetZ);
        coordinates = new Vector3(x, 0.5f + offsetY, y);
        if (mySpot != null)
        {
            showSpot(true);
            mySpot.transform.position = new Vector3(x, 0.01f, y); //fix me
            showSpot(false);
        }
    }

    public Vector3 getPos()
    {
        return coordinates;
        //return new Vector3(transform.position.x - offsetX, transform.position.y - offsetY, transform.position.z - offsetZ);
    }

    public void setMyTurn(bool what)
    {
        myTurn = what;
    }

    public int doFullMove(List<PathScript> paths, int totalDist)
    {
        int output = 0;
        if(this.myName == "Hopscure" || getLifted())
        {
            output += doMove((int)paths[paths.Count - 1].getPos().x, (int)paths[paths.Count - 1].getPos().z, totalDist);
            onMove(paths[paths.Count -1]);
            return paths.Count-1;
        }
        StartCoroutine(moveAnimate((int)paths[0].getPos().x,(int)paths[0].getPos().z,totalDist, paths));
        for (int i = 0; i < paths.Count; i++)
        {
            if (getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != null && getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != "PortalOut")
            {
                
                //output += doMove((int)paths[i].getPos().x, (int)paths[i].getPos().z, totalDist);
                if (getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != "PortalIn" && getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != null)
                {
                    //print(getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap());
                    getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].standingOn(this);
                }
                //onMove(paths[i]);
                return i;
            }
            //output += doMove((int)paths[i].getPos().x, (int)paths[i].getPos().z, totalDist);
            //onMove(paths[i]);
        }
        return paths.Count-1;
    }

    public void pushAway(int d, OpalScript target)
    {
        string direct = "right";
        int dist = (int)getPos().x - (int)target.getPos().x;
        if (dist == 0)
        {
            direct = "up";
            dist = (int)getPos().z - (int)target.getPos().z;
        }
        if (dist < 0)
        {
            if (direct == "right")
            {
                direct = "left";
            }
            else if (direct == "up")
            {
                direct = "down";
            }
            dist = Mathf.Abs(dist);
        }
        if (direct == "right")
        {
            target.nudge(d, true, false);
        }
        else if (direct == "left")
        {
            target.nudge(d, true, true);
        }
        else if (direct == "up")
        {
            target.nudge(d, false, false);
        }
        else if (direct == "down")
        {
            target.nudge(d, false, true);
        }
    }

    public int doMove(int x, int y, int totalDist)
    {
        StartCoroutine(moveAnimate(x,y,totalDist, null));
        return 1;
    }

    public IEnumerator moveAnimate(int x, int y, int totalDist, List<PathScript> paths)
    {
        boardScript.getMyCursor().setAnimating(true);
        bool adj = false;
        int tile = 0;
        List<Vector2> tilesTravelled = new List<Vector2>();
        if (paths != null)
        {
            foreach (PathScript p in paths)
            {
                tilesTravelled.Add(new Vector2(p.getPos().x, p.getPos().z));
            }
            foreach (Vector2 v in tilesTravelled)
            {
                //print(j+" out of "+paths.Count);
                TileScript targetTile = new TileScript();
                x = (int)v.x;
                y = (int)v.y;
                foreach (TileScript t in getSurroundingTiles(true))
                {
                    if (t == boardScript.tileGrid[x, y])
                    {
                        adj = true;
                        targetTile = boardScript.tileGrid[x, y];
                    }
                }
                if (adj && !boardScript.getResetting())
                {
                    int xVel = 0;
                    int yVel = 0;
                    if (currentTile.getPos().x < targetTile.getPos().x)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        xVel = 1;
                    }
                    else if (currentTile.getPos().x > targetTile.getPos().x)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        xVel = -1;
                    }
                    else if (currentTile.getPos().z < targetTile.getPos().z)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        yVel = 1;
                    }
                    else if (currentTile.getPos().z > targetTile.getPos().z)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        yVel = -1;
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        float waddle = transform.position.y * 1f;
                        if (i == 2)
                        {
                            waddle += 0.1f;
                        }
                        else if (i == 6)
                        {
                            waddle -= 0.1f;
                        }
                        transform.position = new Vector3(transform.position.x + xVel * 0.1f, waddle, transform.position.z + yVel * 0.1f);
                        yield return new WaitForFixedUpdate();
                    }
                }
                bool trap = false;
                if(boardScript.tileGrid[x,y].getTrap() != null)
                {
                    trap = true;
                }
                teleport(x, y, totalDist);
                if (trap && tile != 0)
                {
                    if (currentTile != null)
                        currentTile.setCurrentOpal(this);
                    break;
                }
                tile++;
            }
        }
        else
        {
            TileScript targetTile = new TileScript();
            foreach (TileScript t in getSurroundingTiles(true))
            {
                if (t == boardScript.tileGrid[x, y])
                {
                    adj = true;
                    targetTile = boardScript.tileGrid[x, y];
                }
            }
            if (adj && !boardScript.getResetting())
            {
                int xVel = 0;
                int yVel = 0;
                if (currentTile.getPos().x < targetTile.getPos().x)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    xVel = 1;
                }
                else if (currentTile.getPos().x > targetTile.getPos().x)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    xVel = -1;
                }
                else if (currentTile.getPos().z < targetTile.getPos().z)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    yVel = 1;
                }
                else if (currentTile.getPos().z > targetTile.getPos().z)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    yVel = -1;
                }
                for (int i = 0; i < 10; i++)
                {
                    float waddle = transform.position.y * 1f;
                    if (i == 2)
                    {
                        waddle += 0.1f;
                    }
                    else if(i == 6)
                    {
                        waddle -= 0.1f;
                    }
                    transform.position = new Vector3(transform.position.x + xVel * 0.1f, waddle, transform.position.z + yVel * 0.1f);
                    yield return new WaitForFixedUpdate();
                }
            }
            teleport(x, y, totalDist);
        }
        boardScript.getMyCursor().setAnimating(false);
    }

    public IEnumerator nudgeAnim(int dist, bool xorz, bool sign) //true for x, false for z
    {
        int flip = 1;
        if (!sign)
        {
            flip *= -1;
        }
        for (int i = 0; i< dist; i++)
        {
            int xVel = 0;
            int zVel = 0;
            if (xorz)
            {
                xVel = flip;
            }
            else
            {
                zVel = flip;
            }
            //if(boardScript.getResetting())
            while (boardScript.getMyCursor().getAnimating())
            {
                yield return new WaitForFixedUpdate();
            }
            if (getPos().x + xVel > -1 && getPos().x + xVel < 10 && getPos().z + zVel > -1 && getPos().z + zVel < 10 && !boardScript.tileGrid[(int)getPos().x + xVel, (int)getPos().z + zVel].getImpassable() && boardScript.tileGrid[(int)getPos().x + xVel, (int)getPos().z + zVel].currentPlayer == null)
                doMove((int)getPos().x + xVel, (int)getPos().z + zVel, 1);
            yield return new WaitForFixedUpdate();
        }
    }

    public int teleport(int x, int y, int totalDist)
    {
        if (x > -1 && x < 10 && y > -1 && y < 10 && !boardScript.tileGrid[x, y].getImpassable() && boardScript.tileGrid[x, y].currentPlayer == null)
        {
            Vector3 lastPos = getPos();
            if (currentTile != null)
                currentTile.standingOn(null);
            transform.position = new Vector3(x + offsetX, lastPos.y, y + offsetZ);
            coordinates = new Vector3(x, lastPos.y, y);
            if (myTurn)
                boardScript.spotlight.transform.position = new Vector3(x, 2, y);
            currentTile = boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)];
            if (currentTile != lastTile)
            {
                if (lastTile != null)
                    lastTile.standingOn(null);
                currentTile.standingOn(this);
                if (this.getDead())
                {
                    return -1;
                }
                currentTile.setImpassable(true);
                if (lastTile != null)
                    lastTile.setImpassable(false);
                lastTile = currentTile;
                onMove(totalDist);
                onMove(x, y);
            }
            return (int)(Mathf.Abs(lastPos.x - x) + Mathf.Abs(lastPos.z - y));
        }
        return -1;
    }

    public virtual void onMove(int distanceMoved)
    {
        return;
    }

    public virtual void onMove(int x, int z)
    {
        return;
    }

    public virtual void onDamage(int dam)
    {
        return;
    }

    public virtual void onPlacement()
    {
        return;
    }

    public string getName()
    {
        return myName;
    }
    public int getHealth()
    {
        return health;
    }
    public void setHealth(int a)
    {
        health = a;
    }
    public int getAttack()
    {
        return attack + tempattack; ;
    }
    public void setAttack(int a)
    {
        attack = a;
    }
    public int getDefense()
    {
        return defense + tempdefense;
    }
    public void setDefense(int a)
    {
        defense = a;
    }
    public int getSpeed()
    {
        return speed + tempspeed;
    }
    public void setSpeed(int a)
    {
        speed = a;
    }
    public string getPlayer()
    {
        return player;
    }
    public int getRange(int attackNum)
    {
        return Attacks[attackNum-1].getRange();
    }
    public void setDead()
    {
        dead = true;
    }

    public void setNotDead()
    {
        dead = false;
    }

    public bool getDead()
    {
        return dead;
    }
    public Attack[] getAttacks()
    {
        return Attacks;
    }
    public bool getSkipTurn()
    {
        return skipfirstTurn;
    }
    public int getMaxHealth()
    {
        return maxHealth;
    }
    public float getYOffset()
    {
        return offsetY;
    }
    public string getMyName()
    {
        return myName;
    }

    public void setSkipTurn(bool skip)
    {
        skipfirstTurn = skip;
    }
    public string getTypes()
    {
        if(type1 == type2)
        {
            return type1;
        }
        else
        {
            return type1 + "/" + type2;
        }
    }
    public List<int> getBanned()
    {
        return bannedAttacks;
    }

    public void doCharge(int amount)
    {
        if (type1 == "Electric" || type2 == "Electric")
        {
            charge += amount;
            boardScript.callParticles("charge", transform.position);
        }
    }

    public void updateTile()
    {
        currentTile = boardScript.tileGrid[(int)getPos().x, (int)getPos().z];
    }

    public int getCharge()
    {
        return charge;
    }

    public int getPoisonCounter()
    {
        return poisonCounter;
    }

    public string getTeam()
    {
        return player;
    }

    public TileScript getCurrentTile()
    {
        return currentTile;
    }

    public void setCurrentTile(TileScript ct)
    {
        currentTile = ct;
    }

    public bool getBurning()
    {
        return burning;
    }

    public int getBurnTimer()
    {
        return burnTimer;
    }

    public int getPoisonTimer()
    {
        return poisonTimer;
    }

    public int getLiftTimer()
    {
        return liftTimer;
    }

    public void setBurning(bool np)
    {
        setBurning(np, true);
    }

    public void setBurning(bool newburn, bool insect)
    {
        if(newburn && myCharm == "Insect Husk" && insect)
        {
            setPoison(true, false);
            charmRevealed = true;
            return;
        }
        if (type1 != "Fire" && type2 != "Fire")
        {
            if(newburn && !burning && !(currentTile != null && currentTile.type == "Flood"))
            {
                onStatusCondition(false, true, false);
                currentBurn = Instantiate<ParticleSystem>(burningParticle, this.transform);
                burnTimer = 3;
            }else if(!newburn && burning)
            {
                if(currentBurn != null)
                    DestroyImmediate(currentBurn.gameObject);
                currentBurn = null;
                burnCounter = 2;
            }
            burning = newburn;
        }

    }

    public bool getLifted()
    {
        return lifted;
    }

    public void setLifted(bool newLift)
    {
        if (type1 != "Air" && type2 != "Air")
        {
            if (newLift && !lifted)
            {
                onStatusCondition(false, false, true);
                currentLift = Instantiate<ParticleSystem>(liftedParticle, this.transform);
                liftTimer = 3;
                if(myCharm == "Balloon of Light")
                {
                    doTempBuff(2, -1, 1);
                    charmRevealed = true;
                }else if(myCharm == "Floating Bandages")
                {
                    setPoison(false);
                    setBurning(false);
                    charmRevealed = true;
                }
            }
            else if (!newLift && lifted)
            {
                DestroyImmediate(currentLift.gameObject);
                currentLift = null;
                if (myCharm == "Balloon of Light")
                {
                    doTempBuff(2, -1, -1);
                    charmRevealed = true;
                }
            }
            lifted = newLift;
        }
        
    }

    public string getDetails()
    {
        return myName+variant;
    }

    public bool getPoison()
    {
        return poisoned;
    }

    public void setPoison(bool np)
    {
        setPoison(np, true);
    }

    public void setPoison(bool newpoison, bool insect)
    {
        if (newpoison && myCharm == "Insect Husk" && insect)
        {
            setBurning(true, false);
            charmRevealed = true;
            return;
        }
        if ((type1 != "Plague" && type2 != "Plague"))
        {
            if (newpoison && !poisoned && currentTile != null && !(currentTile.type == "Growth"))
            {
                onStatusCondition(true, false, false);
                currentPoison = Instantiate<ParticleSystem>(poisonedParticle, this.transform);
                poisonTimer = 3;
                poisonCounter = 4;
            }
            else if (!newpoison && poisoned)
            {
                if(currentPoison != null)
                    DestroyImmediate(currentPoison.gameObject);
                currentPoison = null;
            }
            poisoned = newpoison;
        }
        
    }

    public void setPoisonTimer(int time, bool over)
    {
        if (over)
        {
            poisonTimer = time;
        }
        else
        {
            if(time > poisonTimer)
            {
                poisonTimer = time;
            }
        }
    }

    public void setPoisonCounter(int num, bool reset)
    {
        poisonCounter = num;
        if (reset)
            poisonTimer = 3;
    }

    public Vector3 reduce(int amount)
    {
        int attackLoss = 0;
        int defenseLoss = 0;
        int speedLoss = 0;
        if(getAttack() > getAttackB())
        {
            int difference = getAttack() - getAttackB();
            if(difference > amount)
            {
                doTempBuff(0, -1, -amount);
                attackLoss = amount;
            }
            else
            {
                doTempBuff(0, -1, -difference);
                attackLoss = difference;
            }
        }
        if(getDefense() > getDefenseB())
        {
            int difference = getDefense() - getDefenseB();
            if (difference > amount)
            {
                doTempBuff(1, -1, -amount);
                defenseLoss = amount;
            }
            else
            {
                doTempBuff(1, -1, -difference);
                defenseLoss = difference;
            }
        }
        if(getSpeed() > getSpeedB())
        {
            int difference = getSpeed() - getSpeedB();
            if (difference > amount)
            {
                doTempBuff(2, -1, -amount);
                speedLoss = amount;
            }
            else
            {
                doTempBuff(2, -1, -difference);
                speedLoss = difference;
            }
        }
        return new Vector3(attackLoss, defenseLoss, speedLoss);
    }

    public void doTempBuffFromReduce(Vector3 input)
    {
        if(input.x > 0)
        {
            doTempBuff(0, -1, (int)input.x);
        }
        if (input.y > 0)
        {
            doTempBuff(1, -1, (int)input.y);
        }
        if (input.z > 0)
        {
            doTempBuff(2, -1, (int)input.z);
        }
    }

    public bool isBuffed()
    {
        if(getAttack() > getAttackB())
        {
            return true;
        }
        if (getDefense() > getDefenseB())
        {
            return true;
        }
        if (getSpeed() > getSpeedB())
        {
            return true;
        }
        return false;
    }

    public bool getAttackAgain()
    {
        return attackAgain;
    }

    public int getAttackB()
    {
        return attack;
    }
    public int getDefenseB()
    {
        return defense;
    }
    public int getSpeedB()
    {
        return speed;
    }

    public bool getMoveAfter()
    {
        return moveAfter;
    }

    public string getMainType()
    {
        return type1;
    }

    public string getSecondType()
    {
        return type2;
    }

    public GroundScript getBoard()
    {
        return boardScript;
    }

    public List<TempBuff> getBuffs()
    {
        return buffs;
    }

    public int getPriority()
    {
        return priority;
    }

    virtual public void toggleFlood(bool flood)
    {
        if(flood)
        {
            //transform.position = new Vector3(getPos().x, transform.position.y - 0.3f, getPos().z);
            //if (mySpot != null)
                //mySpot.transform.localPosition = new Vector3(0, 0f - offsetY, 0);
        }
        else
        {
            //transform.position = new Vector3(getPos().x, getPos().y + getYOffset(), getPos().z);
            //if(mySpot != null)
                //mySpot.transform.localPosition = new Vector3(0, -1.0f - offsetY, 0);
        }
    }

    public IEnumerator shrinker()
    {
        
        onDie();
        onDieItem();
        float shrink = 1f;
        for (int i = 0; i < 20; i++)
        {
            if (boardScript.getGameWon())
                break;
            transform.localScale = transform.localScale * shrink;
            shrink -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        if (!boardScript.getGameWon())
        {
            healStatusEffects();
            dead = true;
            if (transform.position.x != -100 && transform.position.y < 1)
                boardScript.tileGrid[(int)getPos().x, (int)getPos().z].currentPlayer = null;
            transform.position = new Vector3(-100, -100, -100);
            coordinates = new Vector3(-100, -100, -100);
        }
    }

    public IEnumerator spinSpot()
    {
        mySpot.transform.Rotate(45, 0, 0);
        while (mySpot != null)
        {
            mySpot.transform.Rotate(0, 5, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator yowch()
    {
        int dir = 1;
        if (!GetComponent<SpriteRenderer>().flipX)
        {
            dir = -1;
        }
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null)
        {
            sr.color = new Color(1, 1, 1);
        }

        for (int i = 0; i < 3; i++)
        {
            transform.RotateAround(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Vector3.up, -15 * dir);
            yield return new WaitForFixedUpdate();
        }
        
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 9; i++)
        {
            transform.RotateAround(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Vector3.up, 5 * dir);
            yield return new WaitForFixedUpdate();
        }
        if (sr != null)
        {
            sr.color = new Color(1, 1, 1);
        }
    }



    public IEnumerator moveDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator dancer()
    {
        int i = 0;
        string mode = "dance";
        while (true)
        {
            if(mode == "dance")
            {
                int random = Random.Range(1, 6);
                if(random == 1)
                {
                    mode = "jump";
                    i = 0;
                }else if( random > 4)
                {
                    mode = "turn";
                }
            }
            if(mode == "turn")
            {
                if (GetComponent<SpriteRenderer>().flipX == true)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                mode = "dance";
            }
            else if(mode == "jump")
            {
                if(i > 20)
                {
                    mode = "dance";
                    i = 0;
                }
                else if(i < 10)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                }
                else if(i > 10)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
                }
                i++;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public virtual void prepAttack(int attackNum)
    {
        
    }

    private OpalScript barriarraySurrounding()
    {
        OpalScript target = null;
        foreach(TileScript t in getSurroundingTiles(false))
        {
            if(t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Barriarray")
            {
                return t.getCurrentOpal();
            }
        }
        return target;
    }

    //mod specifies whether defense should modify the damage taken
    public virtual void takeDamage(int dam, bool mod, bool effect)
    {
        if(dam <= 0)
        {
            return;
        }
        if(armor > 0 && (!mod || dam - getDefense() > 0))
        {
            addArmor(-1);
            onDamage(-1);
            return;
        }
        if (barriarraySurrounding() != null)
        {
            barriarraySurrounding().takeDamage(dam, mod, effect);
            return;
        }
        if(!boardScript.getResetting())
            StartCoroutine(yowch());
        if(!mod)
        {
            this.health -= dam;
            DamageResultScript temp;
            temp = Instantiate<DamageResultScript>(damRes);
            temp.setUp(-dam, this);
            if (effect)
            {
                boardScript.callParticles("damage", transform.position);
            }
        }
        else if (dam - getDefense() > 0 && dam > 0)
        {
            this.health = this.health - (dam - getDefense());
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
            temp.setUp(-(dam - getDefense()), this);
            if (effect)
                boardScript.callParticles("damage", transform.position);
        }
        if(this.health <= 0)
        {
            if(getCharm() == "Spectre Essence" && !charmRevealed)
            {
                health = 1;
                charmRevealed = true;
                return;
            }
            TileScript temp = currentTile;
            if (currentTile != null)
                temp.standingOn(null);
            onDeathTile(temp);
            if (boardScript.getMyCursor().getCurrentOpal() != null && boardScript.getMyCursor().getCurrentOpal().getMyName() == "Numbskull" && boardScript.getMyCursor().getCurrentOpal().getTeam() != getTeam() && name != "Boulder")
            {
                boardScript.getMyCursor().getCurrentOpal().spawnOplet(spiritchPrefab, boardScript.tileGrid[(int)getPos().x, (int)getPos().z]);
            }
            currentTile = null;
            if(temp != null)
                temp.setImpassable(false);
            dead = true;
            StartCoroutine(shrinker());
        }
        onDamage(dam);
        onDamageItem(dam);
    }

    public void takeDamageBelowArmor(int dam, bool mod, bool effect)
    {
        if (dam <= 0)
        {
            return;
        }
        if (barriarraySurrounding() != null)
        {
            barriarraySurrounding().takeDamage(dam, mod, effect);
            return;
        }
        if (!mod)
        {
            this.health -= dam;
            DamageResultScript temp;
            temp = Instantiate<DamageResultScript>(damRes);
            temp.setUp(-dam, this);
            if (effect)
            {
                boardScript.callParticles("damage", transform.position);
            }
        }
        else if (dam - getDefense() > 0 && dam > 0)
        {
            this.health = this.health - (dam - getDefense());
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
            temp.setUp(-(dam - getDefense()), this);
            if (effect)
                boardScript.callParticles("damage", transform.position);
        }
        if (this.health <= 0)
        {
            TileScript temp = currentTile;
            if (currentTile != null)
                temp.standingOn(null);
            onDeathTile(temp);
            if (boardScript.getMyCursor().getCurrentOpal() != null && boardScript.getMyCursor().getCurrentOpal().getMyName() == "Numbskull" && boardScript.getMyCursor().getCurrentOpal().getTeam() != getTeam())
            {
                boardScript.getMyCursor().getCurrentOpal().spawnOplet(spiritchPrefab, boardScript.tileGrid[(int)getPos().x, (int)getPos().z]);
            }
            currentTile = null;
            if (temp != null)
                temp.setImpassable(false);
            dead = true;
            StartCoroutine(shrinker());
        }
        onDamage(dam);
    }

    public IEnumerator doAttackAnim(OpalScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        TileScript myTile = currentTile;
        if(target != null && target.getCurrentTile().getPos().x <= currentTile.getPos().x && target.getCurrentTile().getPos().z <= currentTile.getPos().z)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            target.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(target != null)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            target.GetComponent<SpriteRenderer>().flipX = false;
        }
        int dir = 1;
        float speed = 0.1f;
        if (!GetComponent<SpriteRenderer>().flipX)
        {
            dir = -1;
        }
        for(int i = 0; i < 5; i++)
        {
            transform.position = new Vector3(transform.position.x-dir*speed,transform.position.y,transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 10; i++)
        {
            transform.position = new Vector3(transform.position.x + dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        doAttack(target, cursor, attackNum, currentProj);
        for (int i = 0; i < 5; i++)
        {
            transform.position = new Vector3(transform.position.x - dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        myTile.setCurrentOpal(this);
    }

    public IEnumerator doAttackAnim(TileScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        TileScript myTile = currentTile;
        if (target != null && target.getPos().x <= currentTile.getPos().x && target.getPos().z <= currentTile.getPos().z)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (target != null)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        int dir = 1;
        float speed = 0.1f;
        if (!GetComponent<SpriteRenderer>().flipX)
        {
            dir = -1;
        }
        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(transform.position.x - dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 6; i++)
        {
            transform.position = new Vector3(transform.position.x + dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        doAttack(target, cursor, attackNum, currentProj);
        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(transform.position.x - dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        myTile.setCurrentOpal(this);
    }

    public void doAttack(OpalScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        Projectile tempProj = Instantiate(currentProj);
        tempProj.setUp(getAttacks()[attackNum].getShape(), getMainType());
        adjustProjectile(tempProj, attackNum);
        tempProj.fire(this, target, attackNum);
    }

    public void doAttack(TileScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        Projectile tempProj = Instantiate(currentProj);
        tempProj.setUp(getAttacks()[attackNum].getShape(), getMainType());
        adjustProjectile(tempProj, attackNum);
        tempProj.fire(this, target, attackNum);
    }

    public OpalScript spawnOplet(OpalScript oplet, TileScript target)
    {
        minionCount = 0;
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (o.GetType() == oplet.GetType() && o.getDead() != true && o.getTeam() == getTeam())
            {
                //print("du hello");
                minionCount++;
            }
        }
        if (minionCount < 4)
        {
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
            temp.setUp(minionCount + 1, swarmLimit, this);
            OpalScript opalTwo = Instantiate<OpalScript>(oplet);
            opalTwo.setOpal(player); 
            opalTwo.setPos((int)target.getPos().x, (int)target.getPos().z);
            opalTwo.setID(boardScript.gameOpals.Count+1);
            getBoard().gameOpals.Add(opalTwo);
            getBoard().addToUnsorted(opalTwo);
            if (player == "Red")
            {
                getBoard().p2Opals.Add(opalTwo);
            }
            else if (player == "Green")
            {
                getBoard().p3Opals.Add(opalTwo);
            }
            else if (player == "Orange")
            {
                getBoard().p4Opals.Add(opalTwo);
            }
            else
            {
                getBoard().p1Opals.Add(opalTwo);
            }
            target.standingOn(opalTwo);
            opalTwo.currentTile = target;
            opalTwo.setSkipTurn(true);
            return opalTwo;
        }
        return null;
    }

    public virtual void onDeathTile(TileScript t)
    {

    }

    public void healStatusEffects()
    {
        setBurning(false);
        setPoison(false);
        setLifted(false);
    }

    public virtual void onStatusCondition(bool p, bool b, bool l)
    {
        return;
    }

    public void nudge(int dist, bool xorz, bool sign) //true for x, false for z
    {
        StartCoroutine(nudgeAnim(dist, xorz, sign));
    }

    public void onMiasma(bool on)
    {
        if (on)
        {
            defense += 2;
        }
        else
        {
            defense -= 2;
        }
    }

    public void onGrowth(bool on)
    {
        if (on)
        {
            attack += 2;
            defense += 2;
        }
        else
        {
            attack -= 2;
            defense -= 2;
        }
    }

    abstract public int getAttackEffect(int attackNum, OpalScript target);

    virtual public int getAttackEffect(int attackNum, TileScript target)
    {
        return 0;
    }

    virtual public int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public void doBuff(int atk, int def, int spd, bool effect)
    {
        if (effect)
        {
            boardScript.callParticles("buff", transform.position);
        }
        if(attack + atk >= 0)
            attack = attack + atk;
        if (defense + def >= 0)
            defense = defense + def;
        if (speed + spd >= 0)
            speed = speed + spd;
    }

    public void doHeal(int heal, bool overheal)
    {
        if(myCharm == "Jasper Figure")
        {
            overheal = true;
            charmRevealed = true;
        }
        if (health != maxHealth || overheal)
        {
            onHeal(heal);
            boardScript.callParticles("health", transform.position);
            if (health + heal >= maxHealth)
            {
                if (overheal)
                {
                    health += heal;
                    DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
                    temp.setUp(heal, this);
                }else if(health > maxHealth)
                {
                    return;
                }
                else
                {
                    DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
                    temp.setUp(maxHealth - health, this);
                    health = maxHealth;
                }
            }
            else
            {
                DamageResultScript temp = Instantiate<DamageResultScript>(damRes);
                temp.setUp(heal, this);
                health += heal;
            }
        }
    }


    public int getBurningDamage()
    {
        return burnCounter;
    }

    public void setBurningDamage(int bc)
    {
        burnCounter = bc;
    }

    public virtual List<OpalScript> getOplets()
    {
        return null;
    }

    public void StartOfTurn()
    {
        if(currentTile == null)
        {
            return;
        }
        if(currentTile.currentPlayer == null)
        {
            currentTile.setCurrentOpal(this);
        }
        moveAfter = false;
        if (burning)
        {
            if (currentTile.type == "Flood")
            {
                setBurning(false);
            } 
            else
            {
                takeBurnDamage(true);
            }
        }
        if (poisoned)
        {
            if (currentTile != null && currentTile.type == "Growth")
            {
                setPoison(false);
            }
            else
            {
                takePoisonDamage(true);
            }
        }
        if (lifted)
        {
            liftTimer--;
            if (liftTimer == -1)
            {
                setLifted(false);
            }
        }
        foreach(Attack a in Attacks)
        {
            if(a!= null)
                a.getCurrentUse(-a.getCurrentUse(0));
        }
        onStart();
        onStartItem();
    }

    public void takeBurnDamage(bool decay)
    {
        if (decay)
        {
            burnTimer--;
            if(currentTile != null && currentTile.type == "Fire")
            {
                burnTimer = 3;
            }
            if(burnTimer == -1)
            {
                setBurning(false);
                return;
            }
        }
        takeDamage(burnCounter, false, false);
        if (myCharm == "Heat-Proof Cloth")
        {
            burnCounter += 1;
            charmRevealed = true;
        }
        else
        {
            burnCounter += 2;
        }
        boardScript.callParticles("burning", transform.position);
    }

    public void takePoisonDamage(bool decay)
    {
        if (decay)
        {
            poisonTimer--;
            if (currentTile != null && currentTile.type == "Miasma")
            {
                poisonTimer = 3;
            }
            if (poisonTimer == -1)
            {
                setPoison(false);
                return;
            }
        }
        takeDamage(poisonCounter, false, false);
        doTempBuff(0, -1, -1);
        doTempBuff(1, -1, -1);
        boardScript.callParticles("poison", transform.position);
    }

    public virtual void onStart()
    {
        return;
    }

    public virtual void onEnd()
    {
        return;
    }

    public virtual void onDie()
    {
        return;
    }

    public virtual void onMove(PathScript p)
    {
        return;
    }

    public virtual void adjustProjectile(Projectile p, int attackNum)
    {
        return;
    }

    public virtual void onFollowUp(int attacking)
    {
        return;
    }

    public virtual void onBuff(TempBuff buff)
    {
        return;
    }

    public virtual void onHeal(int amount)
    {
        return;
    }

    public virtual void toggleMethod()
    {
        return;
    }

    public void onPlacementItem()
    {
        if(myCharm == "Defense Orb")
        {
            doTempBuff(1, -1, 4);
            charmRevealed = true;
        }else if(myCharm == "Makeshift Shield")
        {
            addArmor(1);
            charmRevealed = true;
        }
        else if (myCharm == "Cursed Ring")
        {
            setPoison(true);
            charmRevealed = true;
        }
        switch (myCharm)
        {
            case "Cloak of Whispers":
                doTempBuff(2, -1, 1);
                charmRevealed = true;
                break;
            case "Death Wish":
                doTempBuff(0, -1, -2);
                doTempBuff(1, -1, 2);
                charmRevealed = true;
                break;
            case "Taunting Mask":
                doTempBuff(1, -1, 5);
                charmRevealed = true;
                break;
            case "Juniper Necklace":
                foreach(OpalScript o in boardScript.gameOpals)
                {
                    if(o != null && o.getTeam() == getTeam() && (o.getMainType() == "Void" || o.getSecondType() == "Void"))
                    {
                        doTempBuff(0, -1, 1);
                        doTempBuff(1, -1, 1);
                        doTempBuff(2, -1, 1);
                    }
                }
                charmRevealed = true;
                break;
        }
    }

    public void onDamageItem(int dam)
    {
        switch (myCharm) {
            case "Defense Orb":
                doTempBuff(1, -1, -1);
                break;
            case "Jade Figure":
                if(currentTile != null && currentTile.type == "Growth")
                {
                    doTempBuff(0, -1, 1);
                    doTempBuff(1, -1, 1);
                    charmRevealed = true;
                }
                break;
            case "Broken Doll":
                if(dam > 0)
                {
                    if(boardScript.getMyCursor().getCurrentOpal().getTeam() == getTeam())
                    {
                        doTempBuff(2, 1, 2);
                    }
                    charmRevealed = true;
                }
                break;
            case "Potion of Gratitude":
                if(dam > 0)
                {
                    if (!takenDamage)
                    {
                        doHeal(10, false);
                        charmRevealed = true;
                        takenDamage = true;
                    }
                }
                break;
            case "Taunting Mask":
                if(dam > defense)
                {
                    boardScript.myCursor.getCurrentOpal().doHeal(2, false);
                    charmRevealed = true;
                }
                break;
            case "Death's Skull":
                if(dead)
                {
                    boardScript.myCursor.getCurrentOpal().takeDamage(dam, true, true);
                    charmRevealed = true;
                }
                break;
            case "Golden Figure":
                doHeal(2, false);
                charmRevealed = true;
                break;
            case "Garnet Figure":
                boardScript.setTile(boardScript.myCursor.getCurrentOpal(), "Fire", false);
                charmRevealed = true;
                break;
            case "Suffering Crown":
                if(boardScript.myCursor.getCurrentOpal().getAttack() > getAttack())
                {
                    doTempBuff(0, -1, 5);
                    charmRevealed = true;
                }
                return;
            
        }
    }

    public void onStartItem()
    {
        switch (myCharm) {
            case "Lightweight Fluid":
                doHeal(2, getLifted());
                charmRevealed = true;
                break;
            case "Metal Scrap":
                if (currentTile != null)
                {
                    startTile = currentTile.getPos();
                }
                break;
            case "Azurite Figure":
                if(currentTile != null)
                {
                    boardScript.setTile(currentTile, "Flood", false);
                }
                charmRevealed = true;
                break;
            case "Dripping Candle":
                foreach(TileScript t in getSurroundingTiles(true))
                {
                    if (t.getCurrentOpal() != null)
                        t.getCurrentOpal().setBurning(true);
                }
                charmRevealed = true;
                break;
        }
    }

    public void onEndItem()
    {
        switch (myCharm)
        {
            case "Metal Scrap":
                if (currentTile != null && currentTile.getPos() == startTile)
                {
                    addArmor(1);
                    charmRevealed = true;
                }
                break;
            case "Mysterious Leaf":
                if(currentTile != null && currentTile.type == "Growth")
                {
                    foreach(TileScript t in getSurroundingTiles(true))
                    {
                        if(t.getCurrentOpal() != null)
                        {
                            t.getCurrentOpal().doTempBuff(0, -1, 1);
                            t.getCurrentOpal().doTempBuff(1, -1, 1);
                        }
                    }
                    charmRevealed = true;
                }
                break;
            case "Damp Machine":
                if(currentTile != null && currentTile.type == "Flood")
                {
                    doTempBuff(1, -1, 2);
                }
                break;
            case "Whetstone":
                bool rock = false;
                foreach(TileScript t in getSurroundingTiles(true))
                {
                    if(t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Boulder")
                    {
                        t.getCurrentOpal().doTempBuff(1,-1,2);
                        rock = true;
                    }
                }
                if (rock)
                {
                    doTempBuff(1, -1, 2);
                    charmRevealed = true;
                }
                break;
        }
    }

    public void onArmorItem(int add)
    {
        switch (myCharm)
        {
            case "Comfortable Padding":
                if(add > 1)
                {
                    doTempBuff(0, -1, 2);
                    charmRevealed = true;
                }
                else
                {
                    doTempBuff(0, -1, -2);
                    charmRevealed = true;
                }
                break;
        }
    }

    public void onDieItem()
    {
        switch (myCharm)
        {
            case "Grieving Shrimp":
                foreach(OpalScript o in boardScript.gameOpals)
                {
                    if(o.getTeam() == getTeam())
                    {
                        o.doTempBuff(0, 1, 7);
                        o.doTempBuff(1, 1, 7);
                    }
                }
                charmRevealed = true;
                break;
        }
    }

    public virtual int checkCanAttack(TileScript target, int attackNum)
    {
        if (target != null && target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }

    public void handleTempBuffs(bool agebuff)
    {
        tempattack = 0;
        tempdefense = 0;
        tempspeed = 0;
        foreach(TempBuff t in buffs)
        {
            if (agebuff)
                t.reduceTurn();
            if (t.getTurnlength() > 0 || t.getTurnlength() == -1)
            {
                int choose = t.getTargetStat();
                if (choose == 0)
                {
                    tempattack += t.getAmount();
                }
                else if (choose == 1)
                {
                    tempdefense += t.getAmount();
                }
                else if (choose == 2)
                {
                    tempspeed += t.getAmount();
                }
            }
            //probably want to clean out this list every once in a while
        }
    }

    public List<TileScript> getSurroundingTiles(bool adj)
    {
        List<TileScript> output = new List<TileScript>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && !(i == 0 && j == 0) && (!adj || (Mathf.Abs(i) != Mathf.Abs(j))))
                {
                        //print((int)getPos().x + i + ", " + ((int)getPos().z + j));
                        output.Add(boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j]);
                }
            }
        }
        return output;
    }

    public List<TileScript> getDiagonalTiles()
    {
        List<TileScript> output = new List<TileScript>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && !(i == 0 && j == 0) && ((Mathf.Abs(i) == Mathf.Abs(j))))
                {
                    output.Add(boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j]);
                }
            }
        }
        return output;
    }

    public void setTempBuff(int targetStat, int turnLength, int targetNum)
    {
        int i = 1;
        if (targetStat == 0)
        {
            if(getAttack() > targetNum)
            {
                i *= -1;
                while(getAttack() > targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
            else
            {
                while (getAttack() < targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
        }
        else if (targetStat == 1)
        {
            if (getDefense() > targetNum)
            {
                i *= -1;
                while (getDefense() > targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
            else
            {
                while (getDefense() < targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
        }
        else if (targetStat == 2)
        {
            if (getSpeed() > targetNum)
            {
                i *= -1;
                while (getSpeed() > targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
            else
            {
                while (getSpeed() < targetNum)
                {
                    doTempBuff(targetStat, turnLength, i);
                }
            }
        }
    }

    public int getTempBuff(int type)
    {
        int output = 0;
        foreach(TempBuff t in buffs)
        {
            if(t.getTargetStat() == type && (t.getTurnlength() > 0 || t.getTurnlength() == -1))
            {
                output += t.getAmount();
            }
        }
        return output;
    }

    public void clearBuffs()
    {
        foreach(TempBuff t in buffs)
        {
            t.clearStat();
        }
        handleTempBuffs(false);
    }

    public void clearAllBuffs()
    {
        buffs = new List<TempBuff>();
        handleTempBuffs(false);
    }

    public TempBuff doTempBuff(int ts, int tl, int a, bool effect)
    {
        if (a > 0)
        {
            if(effect)
                boardScript.callParticles("buff", transform.position);
        }
        else if (a < 0)
        {
            if(effect)
                boardScript.callParticles("debuff", transform.position);
        }
        TempBuff temp = new TempBuff(ts, tl, a);
        onBuff(temp);
        buffs.Add(temp);
        handleTempBuffs(false);
        return temp;
    }

    public TempBuff doTempBuff(int ts,int tl,int a)
    {
        return doTempBuff(ts, tl, a, true);
    }

    public class TempBuff
    {
        //0 is attack, 1 is defense, 2 is speed
        int targetstat;
        //how many turns does this last?
        //if -1 then lasts forever
        int turnlength;
        int amount;

        public TempBuff(int ts, int tl, int a)
        {
            targetstat = ts;
            turnlength = tl;
            amount = a;
        }

        public int getAmount()
        {
            return amount;
        }

        public int getTurnlength()
        {
            return turnlength;
        }

        public int getTargetStat()
        {
            return targetstat;
        }

        public void reduceTurn()
        {
            if (turnlength > 0)
            {
                turnlength--;
            }
        }

        public void clearStat()
        {
            turnlength = 0;
        }
    } 
}
