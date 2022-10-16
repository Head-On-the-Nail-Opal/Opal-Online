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
    private List<Enchantment> enchants = new List<Enchantment>();
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
    protected ParticleSystem bubbles;
    private int matchID = -1;
    private int minionCount = 0;
    //private string myCharm;
    private List<Charm> myCharms = new List<Charm>();
    private string myNickname;
    private GameObject myHighlight;
    private Color teamColor;
    private bool flashing = false;
    private Vector3 highlightSpot = new Vector3(0, 0, 0.01f);
    private Coroutine curseFlash;
    private bool displayOpal = false;
    private bool damagedByPoison = false;
    private List<TileScript> checkedFloods = new List<TileScript>();

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
    private int liftCounter = 2;
    private int skin = 0;

    private bool tidal = false;

    private GameObject playerIndicator;
    private bool setTeam = false;
    private GameObject mySpot;

    private bool earlyEnd = false;
    private Spiritch spiritchPrefab;
    private string personality = "Straight-Edge";
    private int mySize = 2;

    private Vector3 startTile;
    private bool takenDamage = false;
    private bool tidalDelay = false;

    private bool flooded = false;
    private bool waddling = false;

    private int succuumTurns = 0;

    private Vector3 coordinates = new Vector3();

    protected List<OpalScript> cursed = new List<OpalScript>();
    protected List<OpalScript> cursedBy = new List<OpalScript>();

    public List<OpalScript> evolvesInto = new List<OpalScript>();
    public List<OpalScript> evolvesFrom = new List<OpalScript>();
    public bool defaultStage = true;

    private List<Behave> myPriorities = new List<Behave>();

    private List<Behave> speciesPriorities = new List<Behave>();
    private List<Behave> speciesAwareness = new List<Behave>();

    public Sprite attackFrame;
    public Sprite hurtFrame;
    public Sprite myBoulders;
    private Sprite defaultFrame;

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
        defaultFrame = GetComponent<SpriteRenderer>().sprite;
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

    public void setDisplayOpal()
    {
        displayOpal = true;
        
        GetComponent<SpriteRenderer>().sprite = defaultFrame;
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            if(sr.name == "Highlight")
            {
                sr.enabled = false;
                return;
            }
        }
    }

    //Start changes

    void Update () {
        if(myName != "Boulder" && !displayOpal && (myHighlight == null || !setTeam) && player != null && playerIndicator != null)
        {
            setTeam = true;
            doHighlight();
            
            //StartCoroutine(highlightFlash());
        }

        if(myHighlight != null && myHighlight.transform.position != highlightSpot)
        {
            myHighlight.transform.localPosition = new Vector3(0,0,0.01f);
            myHighlight.transform.rotation = transform.rotation;
            myHighlight.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            //print("du hello");
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


    public bool getWaddling()
    {
        return waddling;
    }

    public IEnumerator highlightFlash()
    {
        if (myHighlight != null)
        {

            flashing = true;
            SpriteRenderer hsr = myHighlight.GetComponent<SpriteRenderer>();
            hsr.enabled = true;
            while (true)
            {
                if (hsr == null)
                    break;
                for (int i = 0; i < 20; i++)
                {
                    if (hsr == null)
                        break;
                    hsr.color += new Color(0.05f, 0.05f, 0.05f);
                    yield return new WaitForFixedUpdate();
                }
                if (hsr == null)
                    break;
                for (int j = 0; j < 20; j++)
                {
                    if (hsr == null)
                        break;
                    hsr.color -= new Color(0.05f, 0.05f, 0.05f);
                    yield return new WaitForFixedUpdate();
                }
                if (hsr == null)
                    break;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public void doHighlight()
    {
        doHighlight("");
    }

    public void restartHighlight()
    {
        if(myHighlight != null)
            myHighlight.GetComponent<Animator>().Play(getMyName(),-1,0);
    }

    public void doHighlight(string input)
    {
        if(myHighlight != null)
        {
            Destroy(myHighlight);
            myHighlight = null;
        }

        foreach(Transform child in transform)
        {
            if(child.name == "Highlight")
            {
                Destroy(child.gameObject);
            }
        }

        SpriteRenderer highlight = Instantiate<SpriteRenderer>(GetComponent<SpriteRenderer>(), this.transform);
        highlight.gameObject.name = "Highlight";
        Destroy(highlight.gameObject.GetComponent<OpalScript>());
        foreach(Transform child in highlight.transform)
        {
            Destroy(child.gameObject);
        }
        if(highlight.GetComponent<Animator>() != null)
            highlight.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
        if(input != "")
        {
            highlight.GetComponent<Animator>().Play(input);
        }
        highlight.transform.localScale *= 1f;
        highlight.transform.position = new Vector3(0, 0, 0);
        Shader shaderGUItext = Shader.Find("GUI/Text Shader");
        highlight.material.shader = shaderGUItext;
        myHighlight = highlight.gameObject;

        if (player == "Red")
        {
            teamColor = Color.red;
        }
        else if (player == "Blue")
        {
            teamColor = Color.blue;
        }
        else if (player == "Green")
        {

            teamColor = Color.green;
        }
        else if (player == "Orange")
        {

            teamColor = new Color(1, 0.5f, 0);
        }
        highlight.color = teamColor;
        highlight.enabled = false;
        if (GetComponent<SpriteRenderer>().flipX != highlight.flipX)
        {
            highlight.flipX = GetComponent<SpriteRenderer>().flipX;
        }

        if(boardScript.myCursor.getCurrentOpal() == this)
        {
            boardScript.myCursor.flashOpal();
        }
    }

    public void resetHighlight()
    {
        if (myHighlight == null)
            return;
        myHighlight.GetComponent<SpriteRenderer>().color = teamColor;
        myHighlight.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void showHighlight()
    {
        if (myHighlight == null)
            return;
        myHighlight.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void flipOpal(bool dir)
    {
        GetComponent<SpriteRenderer>().flipX = dir;
        if(myHighlight != null)
            //myHighlight.GetComponent<SpriteRenderer>().flipX = dir;
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.flipX = dir;
        }
    }

    public void toggleSkull(bool toggle)
    {
        if (toggle)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private IEnumerator doCurseFlash()
    {
        //print("du hello");
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.white;
        while (true)
        {
            for (int i = 0; i < 50; i++)
            {
                sr.color -= new Color(0.01f, 0.01f, 0.01f);
                yield return new WaitForFixedUpdate();
            }
            for (int j = 0; j < 50; j++)
            {
                sr.color += new Color(0.01f, 0.01f, 0.01f);
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(0.3f);
        }
    }


    public List<OpalScript> getCursed()
    {
        return cursed;
    }

    //Variant documentation - changed from griff's version
    //skin is decided by the last two digits
    //particlesystem by second to last two digits
    //particlecolor by third to last two digits

    public void setVariant(string num)
    {
        List<Color> clrs = new List<Color>(){ Color.red, Color.green, Color.blue, Color.cyan, Color.black, Color.gray, Color.magenta, Color.white, new Color(255/256f, 165/256f, 0), new Color(212/256f, 175/256f, 55/256f), new Color(0,102/256f,0), new Color(1,153/255f,1) };
        List<string> particles = new List<string>(){"", "Prefabs/World/Particles/Rarity-Burn", "Prefabs/World/Particles/Rarity-Drip", "Prefabs/World/Particles/Rarity-Twinkle", "Prefabs/World/Particles/Rarity-Crown", "Prefabs/World/Particles/Rarity-Dusty", "Prefabs/World/Particles/Rarity-Hyperspeed", "Prefabs/World/Particles/Rarity-Portal" };
        variant = myName + num;
        int intNum = int.Parse(num);
        skin = intNum % 100;
        List<float> sizes = new List<float>() { 0.6f, 0.8f, 1, 1.2f, 1.4f };
        //int particleSystem = (intNum / 100) % 100;
        int particleSystem = 0;
        int particleColor = (intNum / 10000) % 100;
        int size = (intNum / 1000000) % 100;
        if (particleSystem != 0)
        {
            string path = particles[particleSystem];
            ParticleSystem myPart = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>(path), this.transform);
            myPart.transform.localPosition = new Vector3(0, 0, -2);
            myPart.transform.localScale *= 0.1f;
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
        if (anim != null && haveCharm("Goreilla Suit")) {
            anim.CrossFade("Goreilla", 0);
        }
        else if(anim != null)
        {
            anim.CrossFade(myName + skin, 0);
        }
        transform.localScale *= sizes[size];
        mySize = size;
        foreach(Transform t in GetComponentInChildren<Transform>())
        {
            t.localScale *= sizes[size];
        }
        //anim.CrossFade(myName + skin, 0);
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
        return getMyName() +'|'+ getCharmsNames()[0] +'|'+ getPersonality();
    }

    public void setFromSave(string data)
    {
        string[] parsed = data.Split('|');
        setCharmFromString(parsed[1], false);
        personality = parsed[2];
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

    public string getPersonalityInfo(string p)
    {
        switch (p)
        {
            case "Proud":
                return "Attack +1, Defense -1";
            case "Reserved":
                return "Defense +1, Attack -1";
            case "Risk-Taker":
                return "Attack +2, Defense -2";
            case "Worried":
                return "Defense +2, Attack -2";
            case "Tactical":
                return "Attack +3, Speed -1";
            case "Cautious":
                return "Defense +3, Speed -1";
            case "Relaxed":
                return "Health +5, Speed -1";
            case "Optimistic":
                return "Health +5, Attack -2";
            case "Pesimistic":
                return "Health +5, Defense -2";
            case "Impatient":
                return "Speed +1, Attack -2, Defense -2";
            case "Straight-Edge":
                return "No stat change";
        }
        return "";
    }

    public void setRandomPersonality()
    {
        int rand = Random.Range(0, 11);
        switch (rand) {
            case 0:
                personality = "Straight-Edge";
                break;
            case 1:
                personality = "Proud";
                break;
            case 2:
                personality = "Reserved";
                break;
            case 3:
                personality = "Risk-Taker";
                break;
            case 4:
                personality = "Worried";
                break;
            case 5:
                personality = "Tactical";
                break;
            case 6:
                personality = "Cautious";
                break;
            case 7:
                personality = "Relaxed";
                break;
            case 8:
                personality = "Optimistic";
                break;
            case 9:
                personality = "Pesimistic";
                break;
            case 10:
                personality = "Impatient";
                break;
        }

    }

    public List<Charm> getCharms()
    {
        if (myCharms.Count == 0)
        {
            Charm empty = new Charm();
            empty.setName("None");
            return new List<Charm>() {empty};
        }
        return myCharms;
    }

    public void setCharm(Charm i)
    {
        myCharms.Add(i);
    }

    public void setCharmFromString(string c, bool revealed)
    {
        Charm charm = new Charm();
        charm.setName(c);
        charm.setRevealed(revealed);
        myCharms.Add(charm);
    }

    public List<string> getCharmsNames()
    {
        List<string> charmNames = new List<string>();
        foreach(Charm c in myCharms)
        {
            charmNames.Add(c.getName());
        }
        return charmNames;
    }

    public bool haveCharm(string s)
    {
        foreach (Charm c in myCharms)
        {
            if(c.getName() == s)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkRevealed(string s)
    {
        int index = 0;
        foreach (Charm c in myCharms)
        {
            if (c.getName() == s)
            {
                return myCharms[index].getRevealed();
            }
            index++;
        }
        return false;
    }

    public void setCharmRevealed(string s, bool r)
    {
        foreach (Charm c in myCharms)
        {
            if (c.getName() == s)
            {
                c.setRevealed(r);
            }
        }
    }

    public void replaceCharm(Charm i)
    {
        myCharms.Clear();
        myCharms.Add(i);
    }

    public void clearCharms()
    {
        myCharms.Clear();
    }

    public void replaceCharmName(string c)
    {
        myCharms.Clear();
        Charm i = new Charm();
        i.setName(c);
        i.setRevealed(false);
        myCharms.Add(i);
    }

    public string getNickname()
    {
        return myNickname;
    }
    
    public void setNickname(string nn)
    {
        myNickname = nn;
    }

    /**public void setCharm(string n)
    {
        myCharm = new Charm();
        myCharm.setName(n);
        print(getMyName() + "'s charm is set to " + myCharm.getName());
    }*/

    public void setDetails(OpalScript o)
    {
        foreach(Charm c in o.getCharms())
        {
            setCharm(c);
        }
        personality = o.getPersonality();
        List<float> sizes = new List<float>() { 0.6f, 0.8f, 1, 1.2f, 1.4f };
        mySize = o.getSize();
        transform.localScale *= sizes[mySize];
        myNickname = o.getNickname();
    }

    public int getSize()
    {
        return mySize;
    }

    public void setSize(int size)
    {
        List<float> sizes = new List<float>() { 0.6f, 0.8f, 1, 1.2f, 1.4f };
        mySize = size;
        transform.localScale *= sizes[mySize];
    }

    public string saveDetails()
    {
        return myName + "/" + myCharms[0] + "," + personality + ","+mySize+","+myNickname;
    }

    public void summonParticle(string name)
    {
        ParticleSystem temp = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/" + name);
        ParticleSystem inst = Instantiate<ParticleSystem>(temp, this.transform);
        inst.transform.localScale = transform.localScale;
        inst.transform.localRotation = Quaternion.Euler(0,90,35);
    }

    public void summonNewParticle(string name)
    {
        ParticleSystem temp = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/" + name);
        ParticleSystem inst = Instantiate<ParticleSystem>(temp);
        inst.transform.position = transform.position;
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

    public List<Behave> getBehaviours()
    {
        return myPriorities;
    }

    public List<Behave> getSpeciesPriorities()
    {
        return speciesPriorities;
    }

    public List<Behave> getSpeciesAwareness()
    {
        return speciesAwareness;
    }

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

    public OpalScript getMyModel()
    {
        if (myName == "Boulder")
            return this;
        OpalScript temp = Resources.Load<OpalScript>("Prefabs/Opals/" + getMyName());
        if (temp != null)
            return temp;
        return Resources.Load<OpalScript>("Prefabs/SubOpals/" + getMyName()); 
    }

    public int doFullMove(List<PathScript> paths, int totalDist)
    {
        int output = 0;
        if(this.myName == "Hopscure")
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

    public List<TileScript> getLineBetween(TileScript start, TileScript end)
    {
        List<TileScript> output = new List<TileScript>();
        if (start.getPos().x == end.getPos().x)
        {
            if (start.getPos().z - end.getPos().z > 0)
            {
                for (int i = 0; i <= Mathf.Abs(start.getPos().z - end.getPos().z); i++)
                {
                    if (boardScript.getTile((int)start.getPos().x, (int)start.getPos().z - i) != null)
                        output.Add(boardScript.getTile((int)start.getPos().x, (int)start.getPos().z - i));
                }
            }
            else
            {
                for (int i = 0; i <= Mathf.Abs(start.getPos().z - end.getPos().z); i++)
                {
                    if (boardScript.getTile((int)start.getPos().x, (int)start.getPos().z + i) != null)
                        output.Add(boardScript.getTile((int)start.getPos().x, (int)start.getPos().z + i));
                }
            }
        }
        else if (start.getPos().z == end.getPos().z)
        {
            if (start.getPos().x - end.getPos().x > 0)
            {
                for (int i = 0; i <= Mathf.Abs(start.getPos().x - end.getPos().x); i++)
                {
                    if (boardScript.getTile((int)start.getPos().x - i, (int)start.getPos().z) != null)
                        output.Add(boardScript.getTile((int)start.getPos().x - i, (int)start.getPos().z));
                }
            }
            else
            {
                for (int i = 0; i <= Mathf.Abs(start.getPos().x - end.getPos().x); i++)
                {
                    if (boardScript.getTile((int)start.getPos().x + i, (int)start.getPos().z) != null)
                        output.Add(boardScript.getTile((int)start.getPos().x + i, (int)start.getPos().z));
                }
            }
        }
        return output;
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
                        flipOpal(true);
                        xVel = 1;
                    }
                    else if (currentTile.getPos().x > targetTile.getPos().x)
                    {
                        flipOpal(false);
                        xVel = -1;
                    }
                    else if (currentTile.getPos().z < targetTile.getPos().z)
                    {
                        flipOpal(true);
                        yVel = 1;
                    }
                    else if (currentTile.getPos().z > targetTile.getPos().z)
                    {
                        flipOpal(false);
                        yVel = -1;
                    }
                    currentTile.setShadow(false);
                    for (int i = 0; i < 10; i++)
                    {
                        waddling = true;
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
                waddling = false;
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
                if (currentTile.findSurroundingMeadowebb())
                {
                    boardScript.refundMovement(tilesTravelled.Count - tile-1);
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
            if (adj && !boardScript.getResetting() && currentTile != null)
            {
                int xVel = 0;
                int yVel = 0;
                if (currentTile.getPos().x < targetTile.getPos().x)
                {
                    flipOpal(true);
                    xVel = 1;
                }
                else if (currentTile.getPos().x > targetTile.getPos().x)
                {
                    flipOpal(false);
                    xVel = -1;
                }
                else if (currentTile.getPos().z < targetTile.getPos().z)
                {
                    flipOpal(true);
                    yVel = 1;
                }
                else if (currentTile.getPos().z > targetTile.getPos().z)
                {
                    flipOpal(false);
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
        //boardScript.clearGhosts(x, y);
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
                boardScript.updateCurrent();
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
                boardScript.myCursor.updateCurrentActually();
            }
            return (int)(Mathf.Abs(lastPos.x - x) + Mathf.Abs(lastPos.z - y));
        }
        return -1;
    }

    public IEnumerator playFrame(string action, int length)
    {
        if (displayOpal)
            GetComponent<SpriteRenderer>().sprite = defaultFrame;
        if (length > 0 && myName != "Boulder" && !displayOpal)
        {
            if(anim == null)
            {
                anim = GetComponent<Animator>();
            }
            defaultFrame = GetComponent<SpriteRenderer>().sprite;
            anim.enabled = false;
            if (GetComponent<Inflicshun>() != null)
                GetComponent<Inflicshun>().doFrame(action, true);
            if (myHighlight != null)
            {
                myHighlight.GetComponent<Animator>().enabled = false;
            }
            if (action == "attack" && attackFrame != null)
            {
                GetComponent<SpriteRenderer>().sprite = attackFrame;
                if (myHighlight != null)
                    myHighlight.GetComponent<SpriteRenderer>().sprite = attackFrame;
            }
            else if (action == "hurt" && hurtFrame != null)
            {
                GetComponent<SpriteRenderer>().sprite = hurtFrame;
                if (myHighlight != null)
                    myHighlight.GetComponent<SpriteRenderer>().sprite = hurtFrame;
            }


            for (int i = 0; i < length * 6; i++)
            {
                yield return new WaitForFixedUpdate();
            }

            if (this != null)
            {
                if (myName != "Experiment42" || (GetComponent<Experiment42>() != null && GetComponent<Experiment42>().isCorpse == false))
                {
                    GetComponent<SpriteRenderer>().sprite = defaultFrame;
                    if (myHighlight != null)
                        myHighlight.GetComponent<SpriteRenderer>().sprite = defaultFrame;

                    anim.enabled = true;
                    if (GetComponent<Inflicshun>() != null)
                        GetComponent<Inflicshun>().doFrame(action, false);
                    if (myHighlight != null)
                    {
                        myHighlight.GetComponent<Animator>().enabled = true;
                    }
                }
            }
        }
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
            onChargeItem(amount);
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

    public int getLiftCount()
    {
        return liftCounter;
    }

    public void setLiftCount(int l)
    {
        liftCounter = l;
    }

    public void setBurning(bool np)
    {
        setBurning(np, true);
    }

    public void setBurning(bool newburn, bool insect)
    {
        if(myName == "Boulder")
        {
            return;
        }
        if(newburn && haveCharm("Insect Husk") && insect)
        {
            setPoison(true, false);
            setCharmRevealed("Insect Husk", true);
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
            }else if(newburn && burning)
            {
                burnTimer = 3;
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
        if(myName == "Boulder")
        {
            return;
        }

        if (newLift && !lifted)
        {
            onStatusCondition(false, false, true);
            currentLift = Instantiate<ParticleSystem>(liftedParticle, this.transform);
            liftTimer = 3;
            liftCounter = 2;
            if(haveCharm("Balloon of Light"))
            {
                doTempBuff(2, -1, 1);
                setCharmRevealed("Balloon of Light", true);
            }else if(haveCharm("Floating Bandages"))
            {
                setPoison(false);
                setBurning(false);
                setCharmRevealed("Floating Bandages", true);
            }
        }
        else if (!newLift && lifted)
        {
            DestroyImmediate(currentLift.gameObject);
            currentLift = null;
            if (type1 != "Air" && type2 != "Air" && getMyName() != "Inflicshun")
                takeDamage(liftCounter, false, true);
            else if(getMyName() != "Inflicshun")
            {
                takeDamage(liftCounter/2, false, true);
            }
            liftCounter = 2;
            if (haveCharm("Balloon of Light"))
            {
                doTempBuff(2, -1, -1);
                setCharmRevealed("Balloon of Light", true);
            }
        }
        else if (newLift && lifted)
        {
            liftTimer = 3;
        }
        lifted = newLift;
        
        
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
        if (myName == "Boulder")
            return;
        if (newpoison && haveCharm("Insect Husk") && insect)
        {
            setBurning(true, false);
            setCharmRevealed("Insect Husk", true);
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
            else if (newpoison && poisoned)
            {
                poisonTimer = 3;
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

    private void triggerBubbled()
    {
        if (getEnchantmentValue("Bubbled") < 1)
            return;
        if (currentTile != null)
        {
            boardScript.setTilesRound(currentTile, getEnchantmentValue("Bubbled"), "Flood");
            incrementEnchantment("Bubbled", -1, 5);
        }
        summonNewParticle("BubbleDamage");
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

    public void doBrutalDebuff()
    {
        List<TempBuff> newBuffs = new List<TempBuff>();

        foreach(TempBuff t in buffs)
        {
            newBuffs.Add(t);
        }

        clearAllBuffs();

        foreach(TempBuff b in newBuffs)
        {
            if(b.getTurnlength() == -1)
                doTempBuff(b.getTargetStat(), 2, b.getAmount());
        }
    }

    public int getTempTempBuff(int stat)
    {
        int output = 0;
        foreach(TempBuff b in buffs)
        {
            if (b.getTargetStat() == stat && b.getTurnlength() > 0)
                output += b.getAmount();
        }
        return output;
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
        if(!flood && flooded)
        {
            flooded = false;
        }
        else if(!flooded && flood)
        {
            currentTile.callParticleEffect("Minisplash");
            flooded = true;
        }
    }

    public IEnumerator shrinker()
    {
        
        onDie();
        onDieItem();
        if(boardScript.getCurrentOpal() == this)
            boardScript.nextTurn();
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
            Vector3 deadTile = getPos();
            if (transform.position.x != -100 && transform.position.y < 1)
            {
                boardScript.tileGrid[(int)getPos().x, (int)getPos().z].currentPlayer = null;
            }
            transform.position = new Vector3(-100, -100, -100);
            coordinates = new Vector3(-100, -100, -100);
            boardScript.clearGhosts((int)deadTile.x, (int)deadTile.z);
            if (deadTile.x > -1 && deadTile.x < 10 && deadTile.z > -1 && deadTile.z < 10)
            {
                foreach (string c in exportCharmToTile())
                {
                    if (c.Split(',')[0] != "None")
                    {
                        print(c);
                        boardScript.tileGrid[(int)deadTile.x, (int)deadTile.z].addCharm(c);
                    }
                }
            }
        }
    }

    public void setCharmFromTile(string tileCharm)
    {
        string[] parsed = tileCharm.Split(',');
        if (parsed.Length <= 1)
            return;
        setCharmFromString(parsed[0], parsed[1]=="True");
        onPickUpItem(parsed[0]);
    }

    public List<string> exportCharmToTile()
    {
        List<string> output = new List<string>();
        foreach (Charm charm in myCharms)
        {
            output.Add(charm.getName()+ ","+charm.getRevealed()+","+getTeam());
        }
        return output;
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

    public virtual void takeDamage(int dam, bool mod, bool effect)
    {
        takeDamage(dam, mod, effect, false);
    }

    public virtual void takeDamageBelowArmor(int dam, bool mod, bool effect)
    {
        takeDamage(dam, mod, effect, true);
    }

    //mod specifies whether defense should modify the damage taken
    public virtual void takeDamage(int dam, bool mod, bool effect, bool belowArmor)
    {
        if (health <= 0)
            return;
        List<OpalScript> cursedByOozwl = new List<OpalScript>();
        foreach (OpalScript o in cursedBy)
        {
            if (o.getMyName() == "Oozwl")
                cursedByOozwl.Add(o);
        }
        if (dam <= 0)
        {
            return;
        }
        if (!belowArmor)
        {
            if (armor > 0 && (!mod || dam - getDefense() > 0))
            {
                addArmor(-1);
                onArmorDamage(-1);
                StartCoroutine(yowch());
                StartCoroutine(playFrame("hurt", 5));
                return;
            }
        }
        if (barriarraySurrounding() != null)
        {
            barriarraySurrounding().takeDamage(dam, mod, effect);
            return;
        }
        if (!boardScript.getResetting())
        {
            StartCoroutine(yowch());
            StartCoroutine(playFrame("hurt", 5));
            if(dam > 5)
            {
                StartCoroutine(boardScript.screenShake(1, 4));
            }
            else if(dam > 15)
            {
                StartCoroutine(boardScript.screenShake(2, 4));
            }
            else
            {
                StartCoroutine(boardScript.screenShake(1, 4));
            }
            
        }
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
            if(haveCharm("Spectre Essence") && !checkRevealed("Spectre Essence"))
            {
                health = 1;
                setCharmRevealed("Spectre Essence", true);
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
            if (cursedByOozwl.Count > 0 && damagedByPoison)
            {
                foreach(OpalScript owl in cursedByOozwl)
                {
                    owl.doHeal(owl.getMaxHealth(), false);
                    owl.doTempBuff(2, -1, 2);
                    StartCoroutine(owl.playFrame("attack", 5));
                }
            }
            if(getMyName() == "Experiment42")
            {
                bool allDead = true;
                foreach(OpalScript o in boardScript.gameOpals)
                {
                    if(o.getTeam() == getTeam() && !o.getDead() && o != this)
                    {
                        allDead = false;
                    }
                }
                if(!allDead)
                    return;
            }
            currentTile = null;
            if(temp != null)
                temp.setImpassable(false);
            dead = true;
            StartCoroutine(shrinker());
        }
        if(!dead)
            triggerBubbled();
        onDamage(dam);
        onDamageItem(dam);
        damagedByPoison = false;
    }

    public IEnumerator doAttackAnim(OpalScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        TileScript myTile = currentTile;
        if(target != null && target.getCurrentTile().getPos().x <= currentTile.getPos().x && target.getCurrentTile().getPos().z <= currentTile.getPos().z)
        {
            flipOpal(false);
            target.flipOpal(true);
        }
        else if(target != null)
        {
            flipOpal(true);
            target.flipOpal(false);
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
        StartCoroutine(playFrame("attack", Attacks[attackNum].getAttackAnim()));
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
            flipOpal(false);
        }
        else if (target != null)
        {
            flipOpal(true);
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
        StartCoroutine(playFrame("attack", Attacks[attackNum].getAttackAnim()));
        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(transform.position.x - dir * speed, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        myTile.setCurrentOpal(this);
    }

    public void doAttack(OpalScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        Projectile tempProj = currentProj;
        tempProj.setUp(getAttacks()[attackNum].getShape(), getMainType());
        adjustProjectile(tempProj, attackNum);
        tempProj.fire(this, target, attackNum);
    }

    public void doAttack(TileScript target, CursorScript cursor, int attackNum, Projectile currentProj)
    {
        Projectile tempProj = currentProj;
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

    public List<OpalScript> getOpalsInSameFlood()
    {
        checkedFloods = new List<TileScript>();
        return getOpalsInFlood(currentTile);
    }

    private List<OpalScript> getOpalsInFlood(TileScript flood)
    {
        List<OpalScript> output = new List<OpalScript>();

        List<OpalScript> recurse = new List<OpalScript>();
        if (!checkedFloods.Contains(flood) && flood.type == "Flood")
        {
            checkedFloods.Add(flood);
            if (flood.currentPlayer != null && flood.currentPlayer != this)
            {
                output.Add(flood.currentPlayer);
            }
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (flood.getPos().x + i < 10 && flood.getPos().x + i > -1 && getPos().z + j < 10 && flood.getPos().z + j > -1 && Mathf.Abs(i) != Mathf.Abs(j))
                    {
                        recurse.AddRange(getOpalsInFlood(boardScript.tileGrid[(int)flood.getPos().x + i, (int)flood.getPos().z + j]));
                    }
                }
            }
        }
        foreach(OpalScript o in recurse)
        {
            if (!output.Contains(o))
            {
                output.Add(o);
                
            }
        }
        return output;
    }

    public bool getTidal()
    {
        return tidal;
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
        //boardScript.updateCurrent();
        boardScript.updateCurrentActually();
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
        //boardScript.updateCurrent();
        boardScript.updateCurrentActually();
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

    public void setCursed(OpalScript curser)
    {
        if (!curser.cursed.Contains(this))
        {
            curser.cursed.Add(this);
            cursedBy.Add(curser);
        }
    }

    public List<OpalScript> getCursedBy()
    {
        return cursedBy;
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
            liftCounter += 2;
            if (liftTimer == 0)
            {
                setLifted(false);
            }
        }
        foreach(Attack a in Attacks)
        {
            if(a!= null)
                a.getCurrentUse(-a.getCurrentUse(0));
        }

        if (tidalDelay)
            manageTidal();
        else
            tidalDelay = true;


        bool foundSuccuum = false;
        foreach(TileScript t in getSurroundingTiles(false))
        {
            if(t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Succuum" && t.getCurrentOpal().getTeam() != getTeam())
            {
                foundSuccuum = true;
                succuumTurns++;
                doTempBuff(0, -1, -3);
                doTempBuff(2, -1, -1);
                StartCoroutine(t.getCurrentOpal().playFrame("attack", 4));
            }
        }
        if(succuumTurns != 0 && !foundSuccuum)
        {
            doTempBuff(0, -1, 3*succuumTurns);
            doTempBuff(2, -1, 1*succuumTurns);
            succuumTurns = 0;
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
        
        bool cursedByMoppet = false;
        foreach(OpalScript o in cursedBy)
        {
            if(o.getMyName() == "Moppet")
            {
                if (GetComponent<Inflicshun>() != null)
                {
                    o.takeDamage(burnCounter/2, false, false);
                    cursedByMoppet = true;
                }
                else
                {
                    o.takeDamage(burnCounter, false, false);
                    cursedByMoppet = true;
                }
            }
        }
        if (!cursedByMoppet)
        {
            onBurnDamage(burnCounter);
            if (GetComponent<Inflicshun>() != null)
            {
                takeDamage(burnCounter / 2, false, false);
            }
            else
            {
                takeDamage(burnCounter, false, false);
            }
        }
        if (haveCharm("Heat-Proof Cloth"))
        {
            burnCounter += 1;
            setCharmRevealed("Heat-Proof Cloth", true);
        }
        else
        {
            burnCounter += 2;
        }
        boardScript.callParticles("burning", transform.position);
    }

    public void takePoisonDamage(bool decay)
    {
        bool cursedByOozwl = false;
        foreach(OpalScript o in cursedBy)
        {
            if (o.getMyName() == "Oozwl")
                cursedByOozwl = true;
        }
        if (decay)
        {
            if(!cursedByOozwl)
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

        bool cursedByMoppet = false;
        foreach (OpalScript o in cursedBy)
        {

            if (o.getMyName() == "Moppet")
            {
                if (GetComponent<Inflicshun>() != null)
                {
                    o.takeDamage(poisonCounter / 2, false, false);
                    o.doTempBuff(0, -1, -1);
                    o.doTempBuff(1, -1, -1);
                    if (cursedByOozwl)
                    {
                        o.doTempBuff(0, -1, -1);
                        o.doTempBuff(1, -1, -1);
                    }
                    cursedByMoppet = true;
                }
                else
                {
                    o.takeDamage(poisonCounter, false, false);
                    o.doTempBuff(0, -1, -1);
                    o.doTempBuff(1, -1, -1);
                    if (cursedByOozwl)
                    {
                        o.doTempBuff(0, -1, -1);
                        o.doTempBuff(1, -1, -1);
                    }
                    cursedByMoppet = true;
                }
            }
        }
        if (!cursedByMoppet)
        {
            if (GetComponent<Inflicshun>() != null)
            {
                takeDamage(poisonCounter / 2, false, false);
                damagedByPoison = true;
                doTempBuff(0, -1, -1);
                doTempBuff(1, -1, -1);
                if (cursedByOozwl)
                {
                    doTempBuff(0, -1, -1);
                    doTempBuff(1, -1, -1);
                }
            }
            else
            {
                damagedByPoison = true;
                takeDamage(poisonCounter, false, false);
                doTempBuff(0, -1, -1);
                doTempBuff(1, -1, -1);
                if (cursedByOozwl)
                {
                    doTempBuff(0, -1, -1);
                    doTempBuff(1, -1, -1);
                }
            }

        }
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

    public virtual void onArmorDamage(int dam)
    {
        return;
    }

    public virtual void onBurnDamage(int dam)
    {
        return;
    }

    public virtual void toggleMethod()
    {
        return;
    }

    public void onChargeItem(int inc)
    {
        if (haveCharm("Frayed Wires"))
        {
            if (inc < 0)
            {
                doTempBuff(0, -1, 0-inc);
                setCharmRevealed("Frayed Wires", true);
            }
        }
    }

    public void onPickUpItem(string pickedUp)
    {
        if(pickedUp == "Cloak of Whispers")
        {
            doTempBuff(2, -1, 1);
        }else if (pickedUp == "Death Wish")
        {
            doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 2);
        }
        else if (pickedUp == "Defense Orb")
        {
            doTempBuff(1, -1, 4);
        }
        else if (pickedUp == "Juniper Necklace")
        {
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o != null && o.getTeam() == getTeam() && (o.getMainType() == "Void" || o.getSecondType() == "Void"))
                {
                    doTempBuff(0, -1, 1);
                    doTempBuff(1, -1, 1);
                    doTempBuff(2, -1, 1);
                }
            }
        }
        else if (pickedUp == "Taunting Necklace")
        {
            doTempBuff(1, -1, 5);
        }
    }

    public void onPlacementItem()
    {
        if(haveCharm("Defense Orb"))
        {
            doTempBuff(1, -1, 4);
            setCharmRevealed("Defense Orb", true);
        }else if(haveCharm("Makeshift Shield"))
        {
            addArmor(1);
            setCharmRevealed("Mskeshift Shield", true);
        }
        else if (haveCharm("Cursed Ring"))
        {
            setPoison(true);
            setCharmRevealed("Cursed Ring", true);
        }
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Cloak of Whispers":
                    doTempBuff(2, -1, 1);
                    setCharmRevealed("Cloak of Whispers", true);
                    break;
                case "Death Wish":
                    doTempBuff(0, -1, -2);
                    doTempBuff(1, -1, 2);
                    setCharmRevealed("Death Wish", true);
                    break;
                case "Taunting Mask":
                    doTempBuff(1, -1, 5);
                    setCharmRevealed("Taunting Mask", true);
                    break;
                case "Juniper Necklace":
                    foreach (OpalScript o in boardScript.gameOpals)
                    {
                        if (o != null && o.getTeam() == getTeam() && (o.getMainType() == "Void" || o.getSecondType() == "Void"))
                        {
                            doTempBuff(0, -1, 1);
                            doTempBuff(1, -1, 1);
                            doTempBuff(2, -1, 1);
                        }
                    }
                    setCharmRevealed("Juniper Necklace", true);
                    break;
            }
        }
    }

    public void onDamageItem(int dam)
    {
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Shock Collar":
                    if (!checkRevealed("Shock Collar") && boardScript.getMyCursor().getCurrentOpal().getTeam() != getTeam())
                    {
                        doCharge(5);
                        setCharmRevealed("Shock Collar", true);
                    }
                    break;
                case "Defense Orb":
                    doTempBuff(1, -1, -1);
                    break;
                case "Jade Figure":
                    if (currentTile != null && currentTile.type == "Growth")
                    {
                        doTempBuff(0, -1, 1);
                        doTempBuff(1, -1, 1);
                        setCharmRevealed("Jade Figure", true);
                    }
                    break;
                case "Broken Doll":
                    if (dam > 0 && dam > getDefense())
                    {
                        if (boardScript.getMyCursor().getCurrentOpal().getTeam() == getTeam())
                        {
                            doTempBuff(2, 1, 2);
                        }
                        setCharmRevealed("Broken Doll", true);
                    }
                    break;
                case "Potion of Gratitude":
                    if (dam > 0)
                    {
                        if (!takenDamage)
                        {
                            doHeal(10, false);
                            setCharmRevealed("Potion of Gratitude", true);
                            takenDamage = true;
                        }
                    }
                    break;
                case "Taunting Mask":
                    if (dam > defense)
                    {
                        boardScript.myCursor.getCurrentOpal().doHeal(2, false);
                        setCharmRevealed("Taunting Mask", true);
                    }
                    break;
                case "Death's Skull":
                    if (dead)
                    {
                        boardScript.myCursor.getCurrentOpal().takeDamage(dam, true, true);
                        setCharmRevealed("Death's Skull", true);
                    }
                    break;
                case "Golden Figure":
                    doHeal(2, false);
                    setCharmRevealed("Golden Figure", true);
                    break;
                case "Garnet Figure":
                    boardScript.setTile(boardScript.myCursor.getCurrentOpal(), "Fire", false);
                    setCharmRevealed("Garnet Figure", true);
                    break;
                case "Suffering Crown":
                    if (boardScript.myCursor.getCurrentOpal().getAttack() > getAttack())
                    {
                        doTempBuff(0, -1, 5);
                        setCharmRevealed("Suffering Crown", true);
                    }
                    return;

            }
        }
    }

    public void onStartItem()
    {
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Electromagnet":
                    foreach (OpalScript o in boardScript.gameOpals)
                    {
                        if (o != null && o.getCharge() > 0)
                        {
                            doCharge(2);
                            setCharmRevealed("Electromagnet", true);
                        }
                    }
                    break;
                case "Lightweight Fluid":
                    doHeal(2, getLifted());
                    setCharmRevealed("Lightweight Fluid", true);
                    break;
                case "Metal Scrap":
                    if (currentTile != null)
                    {
                        startTile = currentTile.getPos();
                    }
                    break;
                case "Azurite Figure":
                    if (currentTile != null)
                    {
                        boardScript.setTile(currentTile, "Flood", false);
                    }
                    setCharmRevealed("Azurite Figure", true);
                    break;
                case "Dripping Candle":
                    foreach (TileScript t in getSurroundingTiles(true))
                    {
                        if (t.getCurrentOpal() != null)
                            t.getCurrentOpal().setBurning(true);
                    }
                    setCharmRevealed("Dripping Candle", true);
                    break;
            }
        }
    }

    public void onEndItem()
    {
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Metal Scrap":
                    if (currentTile != null && currentTile.getPos() == startTile)
                    {
                        addArmor(1);
                        setCharmRevealed("Metal Scrap", true);
                    }
                    break;
                case "Mysterious Leaf":
                    if (currentTile != null && currentTile.type == "Growth")
                    {
                        foreach (TileScript t in getSurroundingTiles(true))
                        {
                            if (t.getCurrentOpal() != null)
                            {
                                t.getCurrentOpal().doTempBuff(0, -1, 1);
                                t.getCurrentOpal().doTempBuff(1, -1, 1);
                            }
                        }
                        setCharmRevealed("Mysterious Leaf", true);
                    }
                    break;
                case "Damp Machine":
                    if (currentTile != null && currentTile.type == "Flood")
                    {
                        doTempBuff(1, -1, 2);
                    }
                    break;
                case "Whetstone":
                    bool rock = false;
                    foreach (TileScript t in getSurroundingTiles(true))
                    {
                        if (t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Boulder")
                        {
                            t.getCurrentOpal().doTempBuff(1, -1, 2);
                            rock = true;
                        }
                    }
                    if (rock)
                    {
                        doTempBuff(1, -1, 2);
                        setCharmRevealed("Whetstone", true);
                    }
                    break;
            }
        }
    }

    public void onArmorItem(int add)
    {
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Comfortable Padding":
                    if (add > 1)
                    {
                        doTempBuff(0, -1, 2);
                        setCharmRevealed("Comfortable Padding", true);
                    }
                    else
                    {
                        doTempBuff(0, -1, -2);
                        setCharmRevealed("Comfortable Padding", true);
                    }
                    break;
            }
        }
    }

    public void onDieItem()
    {
        foreach (string c in getCharmsNames())
        {
            switch (c)
            {
                case "Grieving Shrimp":
                    foreach (OpalScript o in boardScript.gameOpals)
                    {
                        if (o.getTeam() == getTeam())
                        {
                            o.doTempBuff(0, 1, 7);
                            o.doTempBuff(1, 1, 7);
                        }
                    }
                    setCharmRevealed("Grieving Shrimp", true);
                    break;
            }
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

    public void manageTidal()
    {
        tidal = !tidal;

        foreach(Attack a in getAttacks())
        {
            if(a.getTidalD() != "")
            {
                string d = a.getDesc();
                a.setDescription(a.getTidalD());
                a.setTidalD(d);
            }
        }
    }

    public int setEnchantment(string name, int value, int max)
    {
        if(name == "Bubbled")
        {
            if (value > 0)
            {
                if (value > max)
                    value = max;
                if (bubbles == null)
                {
                    bubbles = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/BubblePassive"), transform);
                }
                var main = bubbles.main;
                main.maxParticles = value;
            }
            else
            {
                Destroy(bubbles.gameObject);
                bubbles = null;
            }
        }
        foreach (Enchantment e in enchants)
        {
            if (e.getName() == name)
            {
                e.setValue(value);
                return e.getValue();
            }
        }

        enchants.Add(new Enchantment(name, value, max));
        return value;
    }

    public int getEnchantmentValue(string name)
    {
        foreach (Enchantment e in enchants)
        {
            if(e.getName() == name)
            {
                return e.getValue();
            }
        }
        return -1;
    }

    public int incrementEnchantment(string name, int inc, int max)
    {
        foreach (Enchantment e in enchants)
        {
            if (e.getName() == name)
            {
                return setEnchantment(name, e.getValue()+inc, max);
            }
        }

        return setEnchantment(name, inc, max);
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

    public void clearBuffs(int target)
    {
        foreach (TempBuff t in buffs)
        {
            if(t.getTargetStat() == target)
                t.clearStat();
        }
        handleTempBuffs(false);
    }

    public TempBuff doTempBuff(int ts, int tl, int a, bool effect)
    {
        int mod = 0;
        if (getLifted())
            mod = 1;
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
        TempBuff temp = new TempBuff(ts, tl, a+mod);
        onBuff(temp);
        buffs.Add(temp);
        handleTempBuffs(false);
        if(a < 0 && tl !=0)
        {
            StartCoroutine(playFrame("hurt", 5));
            if (a < -2)
            {
                StartCoroutine(boardScript.screenShake(1, 4));
            }
            else if (a < -4)
            {
                StartCoroutine(boardScript.screenShake(2, 4));
            }
            else
            {
                StartCoroutine(boardScript.screenShake(1, 4));
            }
        }
        return temp;
    }


    public TempBuff doTempBuff(int ts,int tl,int a)
    {
        return doTempBuff(ts, tl, a, true);
    }

    public Color getColorFromType(string type, bool first)
    {
        Color projColor = Color.white;
        if (type == "Grass")
        {
            if (first)
                return new Color(7 / 255f, 200 / 255f, 0);
            return new Color(9 / 255f, 255 / 255f, 0);
        }
        else if (type == "Fire")
        {
            if (first)
                return new Color(219 / 255f, 11 / 255f, 0);
            return new Color(255 / 255f, 136 / 255f, 0);
        }
        else if (type == "Water")
        {
            if (first)
                return new Color(0, 77 / 255f, 219 / 255f);
            return new Color(0, 212 / 255f, 245 / 255f);
        }
        else if (type == "Light")
        {
            if (first)
                return new Color(255 / 255f, 255 / 255f, 255 / 255f);
            return new Color(255 / 255f, 254 / 255f, 240 / 255f);
        }
        else if (type == "Dark")
        {
            if (first)
                return new Color(0, 0, 0);
            return new Color(25 / 255f, 0, 71 / 255f);
        }
        else if (type == "Plague")
        {
            if (first)
                return new Color(78 / 255f, 0, 22 / 255f);
            return new Color(141 / 255f, 0, 222 / 255f);
        }
        else if (type == "Air")
        {
            if (first)
                return new Color(255 / 255f, 251 / 255f, 201 / 255f);
            return new Color(224 / 255f, 255 / 255f, 253 / 255f);
        }
        else if (type == "Ground")
        {
            if (first)
                return new Color(235 / 255f, 121 / 255f, 0);
            return new Color(89 / 255f, 66 / 255f, 42 / 255f);
        }
        else if (type == "Void")
        {
            if (first)
                return new Color(105 / 255f, 105 / 255f, 105 / 255f);
            return new Color(200 / 255f, 200 / 255f, 200 / 255f);
        }
        else if (type == "Metal")
        {
            if (first)
                return new Color(191 / 255f, 182 / 255f, 163 / 255f);
            return new Color(163 / 255f, 191 / 255f, 182 / 255f);
        }
        else if (type == "Electric")
        {
            if (first)
                return new Color(255 / 255f, 255 / 255f, 102 / 255f);
            return new Color(255 / 255f, 255 / 255f, 204 / 255f);
        }
        else if (type == "Laser")
        {
            if (first)
                return new Color(255 / 255f, 0 / 255f, 0 / 255f);
            return new Color(200 / 255f, 0 / 255f, 0 / 255f);
        }
        else if (type == "Swarm")
        {
            if (first)
                return new Color(184 / 255f, 109 / 255f, 39 / 255f);
            return new Color(149 / 255f, 166 / 255f, 36 / 255f);
        }
        else if (type == "Spirit")
        {
            if (first)
                return new Color(157 / 255f, 0 / 255f, 166 / 255f);
            return new Color(98 / 255f, 0 / 255f, 143 / 255f);
        }
        return projColor;
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


    public class Charm
    {
        string name;
        bool revealed = false;

        public void setName(string n)
        {
            name = n;
        }

        public string getName()
        {
            return name;
        }

        public bool getRevealed()
        {
            return revealed;
        }

        public void setRevealed(bool r)
        {
            revealed = r;
        }
    }

    public class Enchantment
    {
        string name;
        int value;
        int maxValue;

        public Enchantment(string n, int v, int m)
        {
            name = n;
            value = v;
            maxValue = m;
        }

        public void setName(string n)
        {
            name = n;
        }

        public string getName()
        {
            return name;
        }

        public int getValue()
        {
            return value;
        }

        public void setValue(int v)
        {
            value = v;
            if (value > maxValue)
                value = maxValue;
            if (value < 0)
                value = 0;
        }

    }


}
