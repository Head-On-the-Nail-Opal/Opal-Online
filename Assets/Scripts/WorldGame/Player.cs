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
    public PartyScreen ps;
    public AttackHitbox aim;
    private SpriteRenderer aimsr;
    public CatchGame cg;
    private bool canMove = true;
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

    // Use this for initialization
    void Start () {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        grass = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Grass"), this.transform);
        wet = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/World/Particles/Wet"), this.transform);
        grass.gameObject.SetActive(false);
        wet.gameObject.SetActive(false);
        aimsr = aim.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(fullMap != null && !loaded)
        {
            activateTiles((int)transform.position.x, (int)transform.position.y);
            loaded = true;
        }
        anim.speed = 2;
	    if(controller == "keyboard" && canMove)
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                ps.addEssence("Water");
                ps.addEssence("Fire");
                ps.addEssence("Ground");
                ps.addEssence("Air");
                ps.addEssence("Grass");
                ps.addEssence("Plague");
                ps.addEssence("Dark");
                ps.addEssence("Light");
                ps.addEssence("Metal");
                ps.addEssence("Void");
                ps.addEssence("Swarm");
                ps.addEssence("Laser");
                ps.addEssence("Electric");
            }
            int upSpeed = 0;
            int rightSpeed = 0;
            processInputs();
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.speed *= 2;
                running = true;
            }else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                anim.speed /= 2;
                running = false;
            }if (Input.GetKeyDown(KeyCode.E))
            {
                ps.setClickType("default");
                if (!inv)
                {
                    ps.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                    
                    ps.activate(true);
                    inv = true;
                }
                else
                {
                    ps.transform.position = new Vector3(transform.position.x, transform.position.y, -100);
                    ps.activate(false);
                    ps.resetLift();
                    inv = false;
                    if(ps.getCurrentShop() != null)
                    {
                        ps.getCurrentShop().bye();
                        ps.getCurrentShop().summonUI(false, this);
                    }
                    ps.setCurrentShop(null);

                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //StartCoroutine(aim.slash());
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                saveToFile();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                loadFromFile();
            }
            if (!inv)
            {
                if (currentKey != "" && !moving)
                    StartCoroutine(move());
            }
        }	
	}

    private void processInputs()
    {
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
        for(int i = 0; i < steps; i++)
        {
            if (thisStep == "D")
            {
                aim.transform.localRotation = Quaternion.Euler(0, 0, -270);
                anim.CrossFade("RBIdle", 0);
                if (!getPassable[fullMap[(int)transform.position.x+1, (int)transform.position.y].getCode()] && i == 0)
                {
                    break;
                }
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2((1 * speedMod) / steps + transform.position.x, transform.position.y);
            }
            else if (thisStep == "A")
            {
                aim.transform.localRotation = Quaternion.Euler(0, 0, 270);
                anim.CrossFade("LBIdle", 0);
                if (!getPassable[fullMap[(int)transform.position.x-1, (int)transform.position.y].getCode()] && i == 0)
                {
                    break;
                }
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2((-1 * speedMod) / steps + transform.position.x, transform.position.y);
            }
            else if (thisStep == "S")
            {
                anim.CrossFade("FBIdle", 0);
                aim.transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (!getPassable[fullMap[(int)transform.position.x, (int)transform.position.y - 1].getCode()] && i == 0)
                {
                    break;
                }
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2(transform.position.x, (-1 * speedMod) / steps + transform.position.y);
            }
            else if (thisStep == "W")
            {
                anim.CrossFade("BBIdle", 0);
                aim.transform.localRotation = Quaternion.Euler(0, 0, 180);
                if (!getPassable[fullMap[(int)transform.position.x,(int)transform.position.y+1].getCode()] && i == 0)
                {
                    break;
                }
                onMoveToTile(fullMap[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)].getCode());
                transform.position = new Vector2(transform.position.x, (1 * speedMod) / steps + transform.position.y);
            }
            yield return new WaitForSeconds(0.00001f);
        }
        if(!keepMoving)
            currentKey = "";
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        activateTiles((int)transform.position.x, (int)transform.position.y);
        OpalScript collide = oL.checkTile((int)transform.position.x, (int)transform.position.y);
        if(collide != null)
        {
            print(collide.getMyName());
            addOpal(collide.getVariant());
            oL.removeOpal(collide.gameObject.GetComponent<RoamOpal>());
            DestroyImmediate(collide.gameObject);
        }
        moving = false;
        //print(fullMap[(int)transform.position.x, (int)transform.position.y].getCode());
        if (buffer != "")
        {
            currentKey = buffer;
            buffer = "";
            StartCoroutine(move());
        }
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
        cg.Launch(variant, transform.position);
    }

    public void addOpal(string variant)
    {
        canMove = true;
        if (variant != null)
        {
            party.Add(variant);
            ps.addOpal(variant);
        }
    }

    public void openShop(Shop currentShop)
    {
        if(currentShop == null)
        {
            return;
        }
        string use = "";
        if(currentShop.shopName == "OpalVendor")
        {
            use = "sell";
        }else if(currentShop.shopName == "MaterialShop")
        {
            use = "sellItem";
            
        }
        else if (currentShop.shopName == "PortalVendor")
        {
            use = "swap";

        }
        if (currentShop.shopName == "Digi")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        invType = use;
        //print(currentShop.shopName);
        ps.setCurrentShop(currentShop);
        ps.transform.position = new Vector3(transform.position.x-20, transform.position.y, 10);
        ps.activate(true);
        ps.setClickType(use);
        currentShop.summonUI(true, this);
        inv = true;
    }

    public void saveToFile()
    {
        string path = "Assets/Resources/Maps/player.txt";
        using (var stream = new FileStream(path, FileMode.Truncate))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(transform.position.x + "_" + transform.position.y+"\n");
                foreach (PartyPlate p in ps.getParty())
                {
                    if (p.getOpal() != null)
                        writer.Write(p.getOpal().getVariant() + "\n");
                    else
                        writer.Write("_\n");
                }
                writer.Write("partyend\n");
                foreach (PartyPlate p in ps.getContents(0))
                {
                    if(p.getOpal() != null)
                        writer.Write(p.getOpal().getVariant() + "\n");
                    else
                        writer.Write("_\n");
                }
                writer.Write("opalend\n");
                foreach (PartyPlate p in ps.getContents(1))
                {
                    if (p.getItem() != null)
                        writer.Write(p.getItem().getCode() + "\n");
                    else
                        writer.Write("_\n");
                }
                writer.Write("itemend\n");
                writer.Write("" + ps.getGold() + "\n");
                int i = 0;
                foreach(Shop s in AllShops)
                {
                    writer.Write("nextShop\n");
                    writer.Write(s.getSaveData());
                    i++;
                }
                writer.Write("shopsend\n");
            }
        }
        //AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load<TextAsset>("Maps/player");
    }

    public void loadFromFile() //when updating this make sure you don't break people's saves
    {
        string path = "Assets/Resources/Maps/player.txt";
        StreamReader reader = new StreamReader(path);
        string read = reader.ReadLine();
        ps.clear();

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
        while (read != "partyend")
        {
            if (read != "_")
            {
                ps.addToParty(read);
            }
            i++;
            read = reader.ReadLine();
        }
        read = reader.ReadLine();
        while (read != "opalend")
        {
            if(read != "_")
            {
                ps.addOpal(read);
            }
            i++;
            read = reader.ReadLine();
        }
        while (read != "itemend")
        {
            //print(read);
            if (read != "_")
            {
                ps.addItem(read);
            }
            i++;
            read = reader.ReadLine();
        }
        ps.updateGold(int.Parse(reader.ReadLine()));
        read = reader.ReadLine();
        int currentShop = -1;
        while (read != "shopsend")
        {
            if(read == "nextShop")
            {
                currentShop++;
                AllShops[currentShop].upgradeShop(int.Parse(reader.ReadLine()));
            }
            if(read.Substring(0,1) == "O" || read.Substring(0, 1) == "I")
                AllShops[currentShop].processLine(read);
            read = reader.ReadLine();
        }
    }

    public void updateGold(int amount)
    {
        ps.updateGold(amount);
    }

    public void addItem(Item i)
    {
        ps.addItem(i);
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
                    }
                }
            }
        }
    }

    private void onMoveToTile(string code)
    {
        if(code == "G")
        {
            tallGrassCover.SetActive(true);
        }
        else
        {
            tallGrassCover.SetActive(false);
        }
    }

}
