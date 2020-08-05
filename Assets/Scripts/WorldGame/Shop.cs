using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{

    public string shopName;
    public PartyScreen shopInventory;
    private int upgradeCost = 20;
    private int currentTier = 0;
    private SpriteRenderer sr;
    private Animator anim;
    private Canvas chat;
    private Text chatText;

    // Start is called before the first frame update
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        chat = GetComponentInChildren<Canvas>();
        chatText = chat.GetComponentInChildren<Text>();
        chatText.text = "";
    }

    void Start()
    {
        anim.CrossFade(shopName + currentTier, 0);
    }

    public void upgradeShop()
    {
        if(currentTier < 5)
        {
            currentTier++;
            Speak("thanks...");
            anim.CrossFade(shopName + currentTier, 0);
            upgradeCost += 20;
            if(currentTier == 5)
            {
                upgradeCost = 0;
            }
        }
    }

    public void upgradeShop(int num)
    {
            currentTier = num;
            //Speak("thanks...");
            anim.CrossFade(shopName + currentTier, 0);
            upgradeCost += 20;
            if (currentTier == 5)
            {
                upgradeCost = 0;
            }
    }

    public int getUpgradeCost()
    {
        return upgradeCost;
    }

    public void Speak(string line)
    {
        chatText.text = line;
        StopAllCoroutines();
        StartCoroutine(speakHelper());
    }

    public void greet()
    {
        string voice = "..";
        if(shopName == "OpalVendor")
        {
            voice = "please sell me your pets..";
        }else if(shopName == "MaterialShop")
        {
            voice = "i have things..";
        }
        Speak(voice);
    }

    public void bye()
    {
        string voice = "..";
        if (shopName == "OpalVendor")
        {
            voice = "please go catch more beings..";
        }
        else if (shopName == "MaterialShop")
        {
            voice = "leave now dont lose my things..";
        }
        Speak(voice);
    }

    public void offer(int price, bool item)
    {
        string voice = "..";
        if (shopName == "OpalVendor" && price >= 0 && !item)
        {
            voice = "i give you " + price + ".. good price..";
        }
        else if (shopName == "MaterialShop" && price >= 0 && item)
        {
            voice = "yess very shiny, ill take it for " + price + " haha..";
        }
        Speak(voice);
    }

    public void sellItem(Item i)
    {
        string voice = "..";
        int price = getItemPrice(i);
        if (shopName == "MaterialShop" && price >= 0)
        {
            voice = "ah yes,\n a " + i.getItemName() + " for sale at \n"+ price +" value";
        }
        Speak(voice);
    }

    public void purchase()
    {
        string voice = "..";
        if (shopName == "OpalVendor")
        {
            voice = "i do not eat them..";
        }
        else if (shopName == "MaterialShop")
        {
            voice = "ah yes a good decision.."; 
        }
        Speak(voice);
    }

    public int value(int var, Item i)
    {
        if(var != -1)
        {
            int variant = var;
            int skin = variant % 100;
            int particleSystem = (variant / 100) % 100;
            int particleColor = (variant / 10000) % 100;
            int size = (variant / 1000000) % 100;
            int extraCost = 0;
            if(particleSystem > 0)
            {
                extraCost += 10;
            }
            if(size > 0)
            {
                extraCost += 10;
            }
            return (20 * (skin+1) + extraCost) + 5*currentTier;
        }else if(i != null)
        {
            return (int)(getItemPrice(i) * 0.1f * (currentTier + 1));
        }
        return 0;
    }

    public void summonUI(bool summon, Player pl)
    {
        if(shopInventory == null)
        {
            return;
        }
        if (summon)
        {
            shopInventory.transform.position = new Vector3(pl.transform.position.x+20, pl.transform.position.y+3, 10);
            shopInventory.activate(true);
        }
        else
        {
            shopInventory.transform.position = new Vector3(-100,-100,-100);
            shopInventory.activate(false);
        }
    }

    public IEnumerator speakHelper()
    {
        chat.transform.localPosition = new Vector3(2, 2, -10);
        for (int i = 0; i < 15; i++)
        {
            chat.transform.localPosition = new Vector3(chat.transform.localPosition.x + Random.Range(-0.2f,0.2f), chat.transform.localPosition.y + Random.Range(-0.2f, 0.2f), -10);
            yield return new WaitForSeconds(0.08f);
        }
        chatText.text = "";
        chat.transform.localPosition = new Vector3(0, 0, 0);
    }

    public int getItemPrice(Item i)
    {
        return 20;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        string voice = "..";
        if (shopName == "OpalVendor")
        {
            voice = "best shop for selling.. animals..";
        }
        else if (shopName == "MaterialShop")
        {
            voice = "dont touch anything..";
        }
        Speak(voice);
    }

    public void addToInventory(OpalScript o)
    {
        shopInventory.addOpal(o.getVariant());
    }

    public void addToInventory(Item i)
    {
        shopInventory.addItem(i);
    }

    public string getSaveData()
    {
        string output = "";
        output = output + currentTier + "\n";
        if (shopInventory != null)
        {
            foreach (OpalScript o in shopInventory.getAllOpals())
            {
                output = output + "O" + o.getVariant() + "\n";
            }
            foreach (Item i in shopInventory.getAllItems())
            {
                output = output + "I" + i.getCode() + "\n";
            }
        }
        return output;
    }

    public void processLine(string line)
    {
        if(line == null || line.Length < 1)
        {
            return;
        }
        if(line.Substring(0,1) == "O")
        {
            shopInventory.addOpal(line.Substring(1));
        }else if (line.Substring(0, 1) == "I")
        {
            shopInventory.addItem(line.Substring(1));
        }
    }
}
