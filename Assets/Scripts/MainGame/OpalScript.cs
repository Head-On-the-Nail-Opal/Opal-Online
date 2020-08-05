﻿using System.Collections;
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
    }

    void Start () {
        //transform.position = new Vector3(5,0.5f,5);
        playerIndicator = Resources.Load<GameObject>("Prefabs/TeamLabel");
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
        if(!setTeam && player != null && playerIndicator != null)
        {
            GameObject spot = Instantiate<GameObject>(playerIndicator);
            spot.transform.position = new Vector3(getPos().x,0.001f ,getPos().y);
            spot.transform.localScale = new Vector3(1f / transform.localScale.x, 0.0001f / transform.localScale.y, 1/transform.localScale.z);
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
            spot.GetComponent<MeshRenderer>().material = temp;
            mySpot = spot;
            showSpot(false);
        }
        if(transform.position.x < -99 || display)
        {
            return;
        }
        if (player != null)
        {
            currentTile = boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)];
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
        if (anim != null)
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

    public void showSpot(bool show)
    {
        if(mySpot == null)
        {
            return;
        }
        if (show)
        {
            mySpot.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            mySpot.GetComponent<MeshRenderer>().enabled = false;
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

    public void addArmor(int add)
    {
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
        if (mySpot != null)
        {
            showSpot(true);
            mySpot.transform.position = new Vector3(x, 0.001f, y); //fix me
            showSpot(false);
        }
    }

    public Vector3 getPos()
    {
        return new Vector3(transform.position.x - offsetX, transform.position.y - offsetY, transform.position.z - offsetZ);
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
            return output;
        }
        for(int i = 0; i < paths.Count; i++)
        {
            if (getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != null && getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != "PortalOut")
            {
                
                output += doMove((int)paths[i].getPos().x, (int)paths[i].getPos().z, totalDist);
                if (getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != "PortalIn" && getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap() != null)
                {
                    //print(getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].getTrap());
                    getBoard().tileGrid[(int)paths[i].getPos().x, (int)paths[i].getPos().z].standingOn(this);
                }
                onMove(paths[i]);
                return i;
            }
            output += doMove((int)paths[i].getPos().x, (int)paths[i].getPos().z, totalDist);
            onMove(paths[i]);
        }
        return output;
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
        if (x > -1 && x < 10 && y > -1 && y < 10 && !boardScript.tileGrid[x, y].getImpassable() && boardScript.tileGrid[x, y].currentPlayer == null)
        {
            Vector3 lastPos = getPos();
            if(currentTile != null)
                currentTile.standingOn(null);
            transform.position = new Vector3(x + offsetX, lastPos.y + offsetY, y + offsetZ);
            if(myTurn)
                boardScript.spotlight.transform.position = new Vector3(x, 2, y);
            currentTile = boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)];
            if (currentTile != lastTile)
            {
                if(lastTile != null)
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
            }
            return (int)(Mathf.Abs(lastPos.x - x) + Mathf.Abs(lastPos.z - y));
        }
        return -1;
    }

    public virtual void onMove(int distanceMoved)
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

    public void setBurning(bool newburn)
    {
        if (type1 != "Fire" && type2 != "Fire")
        {
            if(newburn && !burning && !(currentTile != null && currentTile.type == "Flood"))
            {
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
                currentLift = Instantiate<ParticleSystem>(liftedParticle, this.transform);
                liftTimer = 3;
            }
            else if (!newLift && lifted)
            {
                DestroyImmediate(currentLift.gameObject);
                currentLift = null;
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

    public void setPoison(bool newpoison)
    {
        if ((type1 != "Plague" && type2 != "Plague"))
        {
            if (newpoison && !poisoned && currentTile != null && !(currentTile.type == "Growth"))
            {
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
            transform.position = new Vector3(getPos().x, 0.2f + offsetY, getPos().z);
        }
        else
        {
            transform.position = new Vector3(getPos().x, 0.5f + offsetY, getPos().z);
        }
    }

    public IEnumerator shrinker()
    {
        onDie();
        float shrink = 1f;
        for (int i = 0; i < 20; i++)
        {
            transform.localScale = transform.localScale * shrink;
            shrink -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        healStatusEffects();
        dead = true;
        if(transform.position.x != -100)
            boardScript.tileGrid[(int)getPos().x, (int)getPos().z].currentPlayer = null;
        transform.position = new Vector3(-100, -100, -100);
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
        if(!mod)
        {
            this.health -= dam;
            DamageResultScript temp;
            temp = Instantiate<DamageResultScript>(damRes, this.transform);
            temp.setUp(-dam);
            if(effect)
               boardScript.callParticles("damage", transform.position);
        }
        else if (dam - getDefense() > 0 && dam > 0)
        {
            this.health = this.health - (dam - getDefense());
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
            temp.setUp(-(dam - getDefense()));
            if (effect)
                boardScript.callParticles("damage", transform.position);
        }
        if(this.health <= 0)
        {
            TileScript temp = currentTile;
            if(currentTile != null)
                temp.standingOn(null);
            currentTile = null;
            if(temp != null)
                temp.setImpassable(false);
            dead = true;
            StartCoroutine(shrinker());
        }
        onDamage(dam);
    }

    public void spawnOplet(OpalScript oplet, TileScript target)
    {
        int minionCount = 0;
        oplet.setOpal(null);
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (o.getMyName() == oplet.getMyName() && o.getDead() == false)
                minionCount++;
        }
        if (minionCount < 4)
        {
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
            temp.setUp(minionCount + 1, swarmLimit);
            OpalScript opalTwo = Instantiate<OpalScript>(oplet);
            opalTwo.setOpal(player); // Red designates player 1, Blue designates player 2
            opalTwo.setPos((int)target.getPos().x, (int)target.getPos().z);
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
            opalTwo.setSkipTurn(true);
        }
    }

    public void healStatusEffects()
    {
        setBurning(false);
        setPoison(false);
        setLifted(false);
    }

    public void nudge(int dist, bool xorz, bool sign) //true for x, false for z
    {
        if (dist <= 0)
        {
            return;
        }
        int flip = 1;
        if (!sign)
        {
            flip *= -1;
        }
        int checkMoved = 0;
        if (xorz)
        {
            checkMoved = doMove((int)getPos().x + flip, (int)getPos().z, 1);
            if (lifted)
            {
                checkMoved = doMove((int)getPos().x + flip, (int)getPos().z, 1);
            }
        }
        else
        { 
            checkMoved = doMove((int)getPos().x, (int)getPos().z + flip, 1);
            if (lifted)
            {
                checkMoved = doMove((int)getPos().x , (int)getPos().z + flip, 1);
            }
        }
        if(checkMoved == -1)
        {
            return;
        }
        nudge(dist-1, xorz, sign);
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
        if (health != maxHealth || overheal)
        {
            boardScript.callParticles("health", transform.position);
            if (health + heal >= maxHealth)
            {
                if (overheal)
                {
                    health += heal;
                    DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                    temp.setUp(heal);
                }else if(health > maxHealth)
                {
                    return;
                }
                else
                {
                    DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                    temp.setUp(maxHealth - health);
                    health = maxHealth;
                }
            }
            else
            {
                DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                temp.setUp(heal);
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
    }

    public void takeBurnDamage(bool decay)
    {
        if (decay)
        {
            burnTimer--;
            if(currentTile.type == "Fire")
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
        burnCounter += 2;
        boardScript.callParticles("burning", transform.position);
    }

    public void takePoisonDamage(bool decay)
    {
        if (decay)
        {
            poisonTimer--;
            if (currentTile.type == "Miasma")
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

    public TempBuff doTempBuff(int ts,int tl,int a)
    {
        if (a > 0)
        {
            boardScript.callParticles("buff", transform.position);
        }
        else if (a < 0)
        {
            boardScript.callParticles("debuff", transform.position);
        }
        TempBuff temp = new TempBuff(ts, tl, a);
        onBuff(temp);
        buffs.Add(temp);
        handleTempBuffs(false);
        return temp;
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