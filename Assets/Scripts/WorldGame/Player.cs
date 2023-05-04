using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
    private string controller = "keyboard";
    private float speedMod = 1f;
    private SpriteRenderer sr;
    private Animator anim;
    private ParticleSystem wet;
    private ParticleSystem grass;
    private List<string> party = new List<string>();
    private List<OpalScript> literalParty = new List<OpalScript>();
    private bool inv = false;
    private string invType = "default";
    // public PartyScreen ps;
    public GameObject actualSprite;
    public AttackHitbox aim;
    private SpriteRenderer aimsr;
    public CatchGame cg;
    private bool canMove = true;
    private bool catchGame = false;
    public List<Shop> AllShops;
    private string currentKey = "";
    private bool moving = false;
    private bool keepMoving = false;
    private bool running = false;
    private string buffer = "";
    private Dictionary<string, bool> getPassable;
    private TileCode[,] fullMap;
    private Vector2 currentGridPos;
    public OpalLogger oL;
    private bool loaded = false;
    private List<TileCode> loadedTiles = new List<TileCode>();
    public GameObject tallGrassCover;
    public GameObject waterCover;
    public GameObject caughtOpals;
    private bool viewing = false;
    private InventoryOpal switchOpal = null;
    private List<InventoryOpal> partyOpals = new List<InventoryOpal>();
    private List<InventoryOpal> inventoryOpals = new List<InventoryOpal>();
    private List<InventoryOpal> inventoryOpals1 = new List<InventoryOpal>();
    private List<InventoryOpal> inventoryOpals2 = new List<InventoryOpal>();
    private List<InventoryOpal> allOpals = new List<InventoryOpal>();
    private OpalScript currentDisplayOpal;
    private string fromTileCode;
    private BuildWorld bw;
    public OpalInspector oI;
    private bool inspecting = false;
    public ItemMenu itemMenu;
    private bool assigningCharms = false;
    private bool inShop = false;
    private OpalScript currentOpal;
    private int currentPage = 0;
    public GlobalScript glob;
    public NPCTracker npcT;
    private int facing = 0;
    private bool naming = false;

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = actualSprite.GetComponent<Animator>();
        grass = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Grass"), this.transform);
        wet = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Wet"), this.transform);
        grass.gameObject.SetActive(false);
        wet.gameObject.SetActive(false);
        aimsr = aim.GetComponent<SpriteRenderer>();
        
        InventoryOpal ioPrefab = Resources.Load<InventoryOpal>("Prefabs/World/InventoryOpal");
        for (int i = 0; i < 49; i++)
        {
            InventoryOpal io = Instantiate<InventoryOpal>(ioPrefab, caughtOpals.transform);
            io.setPlayer(this);
            io.setOpal(null, i);
            io.setPage(0);
            inventoryOpals.Add(io);
            allOpals.Add(io);
        }
        for (int i = 0; i < 49; i++)
        {
            InventoryOpal io = Instantiate<InventoryOpal>(ioPrefab, caughtOpals.transform);
            io.setPlayer(this);
            io.setOpal(null, i);
            inventoryOpals1.Add(io);
            allOpals.Add(io);
            io.setPage(1);
            io.gameObject.SetActive(false);
        }
        for (int i = 0; i < 49; i++)
        {
            InventoryOpal io = Instantiate<InventoryOpal>(ioPrefab, caughtOpals.transform);
            io.setPlayer(this);
            io.setOpal(null, i);
            inventoryOpals2.Add(io);
            allOpals.Add(io);
            io.setPage(2);
            io.gameObject.SetActive(false);
        }
        for (int i = 0; i < 6; i++)
        {
            InventoryOpal io = Instantiate<InventoryOpal>(ioPrefab, caughtOpals.transform);
            io.setPlayer(this);
            io.setPartyOpal(null, i);
            partyOpals.Add(io);
            allOpals.Add(io);
        }
        InventoryOpal trash = Instantiate<InventoryOpal>(ioPrefab, caughtOpals.transform);
        trash.setPlayer(this);
        trash.setTrash();
        loadFromFile();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(fullMap != null && !loaded)
        {
            activateTiles((int)transform.position.x, (int)transform.position.y);
            loaded = true;
        }
        anim.speed = 2;
	    if(controller == "keyboard")
        {
            if (inspecting)
            {
                return;
            }
            int upSpeed = 0;
            int rightSpeed = 0;
            processInputs();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (inShop)
                {
                    itemMenu.transform.position = new Vector3(-1000, -1000, -10);
                    inShop = false;
                }
                if (assigningCharms)
                {
                    //itemMenu.transform.position = new Vector3(-1000, -1000, -10);
                    //assigningCharms = false;
                }
                else if (inspecting)
                {
                   // oI.transform.position = new Vector3(-1000, -1000, -10);
                   // inspecting = false;
                   // currentOpal = null;
                }
                else if (canMove && !catchGame)
                {
                    caughtOpals.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
                    canMove = false;
                }
                else if(!catchGame)
                {
                    caughtOpals.transform.position = new Vector3(-1000, -1000, -10);
                    canMove = true;
                }
                else
                {
                    caughtOpals.transform.position = new Vector3(-1000, -1000, -10);
                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                 //temporary
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkForNPC((int)getFacingTile().x, (int)getFacingTile().y);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                saveToFile();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                resetState();
                loadFromFile();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                resetState();
                saveToFile();
            }
            if (!inv)
            {
                if (currentKey != "" && !moving && canMove)
                    StartCoroutine(move());
            }
        }	
	}

    private void processInputs()
    {
        if (canMove == false)
            return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            keepMoving = false;
            currentKey = "W";
            if (moving)
                buffer = "W";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            keepMoving = false;
            currentKey = "S";
            if (moving)
                buffer = "S";
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            keepMoving = false;
            currentKey = "D";
            if (moving)
                buffer = "D";
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            keepMoving = false;
            currentKey = "A";
            if (moving)
                buffer = "A";
        }
        if(currentKey != "")
        {
            if (Input.GetKey(KeyCode.W) && currentKey == "W" || Input.GetKey(KeyCode.A) && currentKey == "A" || Input.GetKey(KeyCode.S) && currentKey == "S" || Input.GetKey(KeyCode.D) && currentKey == "D")
            {
                keepMoving = true;
            }
            else
            {
                if (Input.GetKey(KeyCode.W))
                {
                    currentKey = "W";
                    keepMoving = true;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    currentKey = "A";
                    keepMoving = true;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    currentKey = "S";
                    keepMoving = true;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    currentKey = "D";
                    keepMoving = true;
                }
                else
                {
                    currentKey = "";
                    keepMoving = false;
                }
            }
        }
    }

    public IEnumerator move()
    {
        moving = true;
        float steps = 10;
        if (running)
            steps = 6;
        string thisStep = currentKey;
        fromTileCode = fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode();
        for (int i = 0; i < steps; i++)
        {
            if (thisStep == "D")
            {
                aim.transform.localRotation = Quaternion.Euler(0, 0, -270);
                
                if (!getPassable[fullMap[(int)transform.position.x+1, (int)transform.position.y].getCode()] && i == 0)
                {
                    break;
                }
                anim.CrossFade("GobbaMovingRight", 0);
                facing = 0;
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2((1 * speedMod) / steps + transform.position.x, transform.position.y);
            }
            else if (thisStep == "A")
            {
                aim.transform.localRotation = Quaternion.Euler(0, 0, 270);
                
                if (!getPassable[fullMap[(int)transform.position.x-1, (int)transform.position.y].getCode()] && i == 0)
                {
                    break;
                }
                anim.CrossFade("GobbaMovingLeft", 0);
                facing = 1;
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2((-1 * speedMod) / steps + transform.position.x, transform.position.y);
            }
            else if (thisStep == "S")
            {
                
                aim.transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (!getPassable[fullMap[(int)transform.position.x, (int)transform.position.y - 1].getCode()] && i == 0)
                {
                    break;
                }
                anim.CrossFade("GobbaMovingFront", 0);
                facing = 2;
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2(transform.position.x, (-1 * speedMod) / steps + transform.position.y);
            }
            else if (thisStep == "W")
            {
                
                aim.transform.localRotation = Quaternion.Euler(0, 0, 180);
                if (!getPassable[fullMap[(int)transform.position.x,(int)transform.position.y+1].getCode()] && i == 0)
                {
                    break;
                }
                anim.CrossFade("GobbaMovingBack", 0);
                facing = 3;
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2(transform.position.x, (1 * speedMod) / steps + transform.position.y);
            }
            if(i == 0)
                activateTiles((int)transform.position.x, (int)transform.position.y);
            yield return new WaitForSeconds(0.00001f);
        }
        if(!keepMoving)
            currentKey = "";
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        OpalScript collide = oL.checkTile((int)transform.position.x, (int)transform.position.y);
        if(collide != null)
        {
            canMove = false;
            catchOpal(collide.getVariant());
            oL.removeOpal(collide.gameObject.GetComponent<RoamOpal>());
            DestroyImmediate(collide.gameObject);
        }
        moving = false;
        //print(fullMap[(int)transform.position.x, (int)transform.position.y].getCode());
        if (buffer != "" && canMove == true)
        {
            currentKey = buffer;
            buffer = "";
            StartCoroutine(move());
        }
        else
        {
            if(thisStep == "W")
            {
                anim.CrossFade("GobbaIdleBack", 0);
                facing = 3;
            }
            else if(thisStep == "A")
            {
                anim.CrossFade("GobbaIdleLeft", 0);
                facing = 1;
            }
            else if (thisStep == "S")
            {
                anim.CrossFade("GobbaIdleFront", 0);
                facing = 2;
            }
            else if (thisStep == "D")
            {
                anim.CrossFade("GobbaIdleRight", 0);
                facing = 0;
            }
        }
    }

    public void setBuildWorld(BuildWorld build)
    {
        bw = build;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TallGrass")
        {
            grass.gameObject.SetActive(true);
        }
        if(collision.tag == "Water")
        {
            wet.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TallGrass")
        {
            grass.gameObject.SetActive(false);
        }
        if (collision.tag == "Water")
        {
            wet.gameObject.SetActive(false);
        }
    }

    public string getInvType()
    {
        return invType;
    }

    public void catchOpal(string variant)
    {
        canMove = false;
        catchGame = true;
        StartCoroutine(cg.doFlash(variant, transform.position));
    }

    public void addOpal(string variant)
    {
        catchGame = false;
        canMove = true;
        if (variant == null)
        {
            return;
        }
        if (variant != null)
        {
            party.Add(variant);
        }
        bool placed = false;
        foreach(InventoryOpal io in inventoryOpals)
        {
            if(io.getOpal() == null)
            {
                io.setOpal(variant);
                io.setRandomPersonality();
                placed = true;
                break;
            }
        }
        if(placed == false)
        {
            foreach (InventoryOpal io in inventoryOpals1)
            {
                if (io.getOpal() == null)
                {
                    io.setOpal(variant);
                    io.setRandomPersonality();
                    placed = true;
                    break;
                }
            }
        }
        if (placed == false)
        {
            foreach (InventoryOpal io in inventoryOpals2)
            {
                if (io.getOpal() == null)
                {
                    io.setOpal(variant);
                    io.setRandomPersonality();
                    placed = true;
                    break;
                }
            }
        }
    }

    public void addOpalDetails(string details, int i)
    {
        catchGame = false;
        canMove = true;
        if (details == null)
        {
            return;
        }
        if (details != null)
        {
            //party.Add(details);
        }
        int index = i;
        int page = 0;
        while(index > 48)
        {
            index -= 49;
            page += 1;
        }
        print("Found a " + details + " on page " + page + " in slot " + index);
        bool placed = false;
        foreach (InventoryOpal io in inventoryOpals)
        {
            if (io.getOpal() == null && io.getIndex() == index && page == 0)
            {
                io.setOpalWithDetails(details);
                placed = true;
                break;
            }
        }
        if (placed == false)
        {
            foreach (InventoryOpal io in inventoryOpals1)
            {
                if (io.getOpal() == null && io.getIndex() == index && page == 1)
                {
                    io.setOpalWithDetails(details);
                    placed = true;
                    break;
                }
            }
        }
        if (placed == false)
        {
            foreach (InventoryOpal io in inventoryOpals2)
            {
                if (io.getOpal() == null && io.getIndex() == index && page == 2)
                {
                    io.setOpalWithDetails(details);
                    placed = true;
                    break;
                }
            }
        }
    }

    public void addWithDetailsToParty(string details)
    {
        foreach(InventoryOpal io in partyOpals)
        {
            if (io.getOpal() == null)
            {
                io.setOpalWithDetails(details);
                break;
            }
        }
    }

    public void switchOpals(InventoryOpal io)
    {
        if (switchOpal == null) {
            if(io.getOpal() == null)
            {
                return;
            }
            switchOpal = io;
            io.setSwitching(true);
        }
        else
        {
            if (io == switchOpal)
            {
                switchOpal.setSwitching(false);
                switchOpal = null;
                return;
            }

            OpalScript temp = io.getOpal();
            io.clearOpal();
            if (io.getParty())
            {
                //io.setPartyOpal(switchOpal.getOpal().getDetails(), io.getIndex());
                io.setOpal(switchOpal.getOpal().saveDetails());
            }
            else
            {
                io.setOpal(switchOpal.getOpal().saveDetails());
            }
            switchOpal.clearOpal();
            if(temp != null && !io.getTrash())
            {
                if (switchOpal.getParty())
                {
                    //switchOpal.setPartyOpal(temp.getDetails(), switchOpal.getIndex());
                    switchOpal.setOpal(temp.saveDetails());
                }
                else
                {
                    switchOpal.setOpal(temp.saveDetails());
                }
            }
            switchOpal.setSwitching(false);
            switchOpal = null;

            /**
            OpalScript ioClone = io.getOpal();
            OpalScript switchOpalClone = switchOpal.getOpal();
            OpalScript temp = io.getOpal();
            int tempIndex = io.getIndex();
            io.clearOpal();
            if (io.getParty())
            {
                io.setPartyOpal(switchOpal.getOpal().getDetails(), io.getIndex());
                //io.setDetails(switchOpalClone);
            }
            else
            {
                io.setOpal(switchOpal.getOpal().getDetails(), io.getIndex());
                //io.setDetails(switchOpalClone);
            }
            switchOpal.clearOpal();
            if (temp != null && !io.getTrash())
            {
                if (switchOpal.getParty())
                {
                    switchOpal.setPartyOpal(temp.getDetails(), switchOpal.getIndex());
                    //switchOpal.setDetails(ioClone);
                }
                else
                {
                    switchOpal.setOpal(temp.getDetails(), switchOpal.getIndex());
                    //switchOpal.setDetails(ioClone);
                }
            }
            switchOpal.setSwitching(false);
            switchOpal = null;*/
        }
    }

    public void inspectOpal(OpalScript o)
    {
        inspecting = true;
        currentOpal = o;
        oI.transform.position = new Vector3(transform.position.x, transform.position.y, -12);
        oI.setOpal(o);
        
    }

    public void displayOpal(OpalScript opal)
    {
        if(currentDisplayOpal != null)
        {
            Destroy(currentDisplayOpal.gameObject);
            currentDisplayOpal = null;
        }
        if (opal == null)
        {
            return;
        }
        currentDisplayOpal = Instantiate<OpalScript>(opal, caughtOpals.transform);
        currentDisplayOpal.transform.localPosition = new Vector3(-5, 2, -1);
        currentDisplayOpal.transform.localScale *= 3;
    }

    public void saveToFile()
    {
        string path = "Assets/StreamingAssets/player.txt";
        
        using (var stream = new FileStream(path, FileMode.Truncate))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(transform.position.x + "_" + transform.position.y+"\n");
                foreach (InventoryOpal io in inventoryOpals)
                {
                    if (io.getOpal() != null)
                        writer.Write(io.getOpal().saveDetails() + "\n");
                    else
                        writer.Write("_\n");
                }
                foreach (InventoryOpal io in inventoryOpals1)
                {
                    if (io.getOpal() != null)
                        writer.Write(io.getOpal().saveDetails() + "\n");
                    else
                        writer.Write("_\n");
                }
                foreach (InventoryOpal io in inventoryOpals2)
                {
                    if (io.getOpal() != null)
                        writer.Write(io.getOpal().saveDetails() + "\n");
                    else
                        writer.Write("_\n");
                }
                writer.Write("opalend\n");
                foreach (InventoryOpal io in partyOpals)
                {
                    if (io.getOpal() != null)
                        writer.Write(io.getOpal().saveDetails() + "\n");
                    else
                        writer.Write("_\n");
                }
                writer.Write("partyend\n");
            }
        }
        //AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Maps/player");
    }

    public void loadFromFile() //when updating this make sure you don't break people's saves
    {
        resetState();
        string path = "Assets/StreamingAssets/player.txt";
        StreamReader reader = new StreamReader(path);
        string read = reader.ReadLine();
        //ps.clear();

        string[] posArray = read.Split('_');
        float x = float.Parse(posArray[0]);
        float y = float.Parse(posArray[1]);
        transform.position = new Vector3(x, y, transform.position.z);
        int i = 0;
        read = reader.ReadLine();
        if (reader.EndOfStream)
        {
            return;
        }
        while (read != "opalend")
        {
            if (read != "_")
            {
                addWithDetails(read, i);
            }
            i++;
            read = reader.ReadLine();
        }
        //read = reader.ReadLine();
        while (read != "partyend")
        {
            if (read != "_" && read != "opalend")
            {
                addWithDetailsToParty(read);
            }
            i++;
            read = reader.ReadLine();
        }
    }

    public void updateGold(int amount)
    {
        //ps.updateGold(amount);
    }

    public void addItem(Item i)
    {
       // ps.addItem(i);
    }

    public void sendMapPosition(Vector2 pos)
    {
        currentGridPos = pos;
        //print(fullMap[(int)pos.x, (int)pos.y].getCode());
    }

    public void sendMap(TileCode[,] cS)
    {
        fullMap = cS;
    }

    public void sendBumpCodes(Dictionary<string,bool> b)
    {
        getPassable = b;
    }

    public void activateTiles(int x, int y)
    {
        foreach(TileCode t in loadedTiles)
        {
            t.gameObject.SetActive(false);
        }
        for(int i = -8; i < 9; i++)
        {
            for (int j = -5; j < 6; j++)
            {
                if (x + i < fullMap.GetLength(0) && y + j < fullMap.GetLength(1) && x+i >= 0 && y+j >= 0)
                {
                    if (fullMap[x + i, y + j] != null)
                    {
                        fullMap[x + i, y + j].gameObject.SetActive(true);
                        loadedTiles.Add(fullMap[x + i, y + j]);
                        bw.checkSpawn(fullMap[x + i, y + j]);
                    }
                }
            }
        }
    }

    private void onMoveToTile(string code)
    {
        if (code == "G")
        {
            tallGrassCover.SetActive(true);
        } else if(code == "W"){
            if(fromTileCode != "W")
            {
                ParticleSystem splash = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Splash"),this.transform);
            }
            waterCover.SetActive(true);
        }
        else
        {
            tallGrassCover.SetActive(false);
            waterCover.SetActive(false);
            if (fromTileCode == "W")
            {
                ParticleSystem splash = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Splash"), this.transform);
            }
        }
    }

    public void doCharms()
    {
        assigningCharms = true;
        itemMenu.setCharms();
        itemMenu.transform.position = new Vector3(transform.position.x, transform.position.y, -20);
    }

    public void doPersonality()
    {
        oI.togglePersonality();
    }

    public void charmClicked(string charm)
    {
        currentOpal.setCharmFromString(charm,false);
        assigningCharms = false;
        itemMenu.transform.position = new Vector3(-1000, -1000, -10);
        oI.setOpal(currentOpal);
    }

    public void generateDescription(string item)
    {
        itemMenu.generateDescription(item);
    }

    public void nextPage(bool next)
    {
        if (next)
        {
            if(currentPage == 0)
            {
                currentPage = 1;
                enableList(true, inventoryOpals1);
                enableList(false, inventoryOpals);
            }
            else if(currentPage == 1)
            {
                currentPage = 2;
                enableList(true, inventoryOpals2);
                enableList(false, inventoryOpals1);
            }
        }
        else
        {
            if (currentPage == 1)
            {
                currentPage = 0;
                enableList(false, inventoryOpals1);
                enableList(true, inventoryOpals);
            }
            else if (currentPage == 2)
            {
                currentPage = 1;
                enableList(true, inventoryOpals1);
                enableList(false, inventoryOpals2);
            }
        }
    }

    public void enableList(bool enable, List<InventoryOpal> io)
    {
        foreach(InventoryOpal o in io)
        {
            o.gameObject.SetActive(enable);
            o.summonOpal();
        }
    }

    private Vector2 getFacingTile() 
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        switch (facing) {
            case 0:
                x += 1;
                break;
            case 1:
                x -= 1;
                break;
            case 2:
                y -= 1;
                break;
            case 3:
                y += 1;
                break;

        }
        return new Vector2(x, y);
    }

    private string checkForNPC(int x, int y)
    {
        string output =  npcT.getNPC(x,y);

        if (output == "BatlGob")
        {
            saveToFile();
            if (checkReadyForBattle())
            {
                List<OpalScript> myTeam = new List<OpalScript>();
                for (int i = 0; i < 4; i++)
                {
                    if (partyOpals[i] != null && partyOpals[i].getOpal() != null)
                        myTeam.Add(partyOpals[i].getOpal());
                }
                glob.setTeams(myTeam, myTeam, null, null);
                glob.setControllers("keyboard", "keyboard", "keyboard", "keyboard");
                glob.setMult(true);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Connecting", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }else if(output == "Shubo")
        {
            saveToFile();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        return output;
    }

    private bool checkReadyForBattle()
    {
        List<OpalScript> opals = new List<OpalScript>();
        foreach(InventoryOpal o in partyOpals)
        {
            foreach(OpalScript op in opals)
            {
                if(o.getOpal() != null && o.getOpal().getMyName() == op.getMyName())
                {
                    print("opal repeat");
                    return false;
                }
                else if ((op.getCharmsNames().Count != 0 && op.getCharmsNames()[0] != "None") && op.getCharms()[0] != null && op.getCharmsNames()[0] != "" && o.getOpal() != null && o.getOpal().getCharms()[0] == op.getCharms()[0])
                {
                    print("charm repeat");
                    return false;
                }
            }
            if(o.getOpal() != null)
                opals.Add(o.getOpal());
        }
        if(opals.Count < 1)
        {
            print("not enough opals");
            return false;
        }
        return true;
    }


    //format OPALNAME/detail1,detail2
    private void addWithDetails(string opalData, int i)
    {
        addOpalDetails(opalData, i);
    }

    private void resetState()
    {
        foreach(InventoryOpal io in inventoryOpals)
        {
            io.clearOpal();
        }
        foreach (InventoryOpal io in inventoryOpals1)
        {
            io.clearOpal();
        }
        foreach (InventoryOpal io in inventoryOpals2)
        {
            io.clearOpal();
        }
        foreach(InventoryOpal io in partyOpals)
        {
            io.clearOpal();
        }
    }

    public void setNaming()
    {
       // naming = true;
    }

    public void setNotNaming()
    {
       // naming = false;
        foreach(InventoryOpal io in allOpals)
        {
            io.updateLabel();
        }
    }

    public void back()
    {
        oI.transform.position = new Vector3(-1000, -1000, -10);
        inspecting = false;
        currentOpal = null;
    }

}
