using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour {
    private string controller = "keyboard";
    private Rigidbody2D rigidbody2D;
    private float speedMod = 15;
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

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
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
           if (Input.GetKey(KeyCode.W))
           {
                upSpeed = 1;
                anim.CrossFade("BBIdle", 0);
                aim.transform.localRotation = Quaternion.Euler(0,0,180);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                upSpeed = -1;
                anim.CrossFade("FBIdle", 0);
                aim.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                rightSpeed = 1;
                //sr.flipX = true;
                aim.transform.localRotation = Quaternion.Euler(0, 0, 90);
                if (upSpeed == 0)
                    anim.CrossFade("RBIdle", 0);
                else
                    aim.transform.localRotation = Quaternion.Euler(0, 0, 90 + upSpeed * 45);

            }
            else if (Input.GetKey(KeyCode.A))
            {
                rightSpeed = -1;
                //sr.flipX = false;
                aim.transform.localRotation = Quaternion.Euler(0, 0, 270);
                if (upSpeed == 0)
                    anim.CrossFade("LBIdle", 0);
                else
                    aim.transform.localRotation = Quaternion.Euler(0, 0, -90 - upSpeed * 45);

            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rightSpeed *= 2;
                upSpeed *= 2;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                anim.speed *= 2;
            }else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                anim.speed /= 2;
            }if (Input.GetKeyDown(KeyCode.E))
            {
                ps.setClickType("default");
                if (!inv)
                {
                    ps.transform.position = new Vector3(transform.position.x, transform.position.y, 10);
                    
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
                StartCoroutine(aim.slash());
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
                rigidbody2D.velocity = new Vector2(rightSpeed * speedMod, upSpeed * speedMod);
            }
            else
                rigidbody2D.velocity = new Vector2(0 * speedMod, 0 * speedMod);
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
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        cg.Launch(variant, transform.position);
    }

    public void addOpal(string variant)
    {
        canMove = true;
        rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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

}
