using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyScreen : MonoBehaviour {
    private List<PartyPlate> plates = new List<PartyPlate>();
    private List<PartyPlate> itemPlates = new List<PartyPlate>();
    private List<PartyPlate> equipmentPlates = new List<PartyPlate>();
    private List<PartyPlate> playerParty = new List<PartyPlate>();
    private List<List<PartyPlate>> allPlates;
    private List<PartyPlate> currentScreen;
    private ParticleSystem coinParticles;
    public PartyPlate template;
    public string use;
    private PartyPlate toSwap;
    private string clickType = "swap";
    private int goldAmount = 0;
    public Text goldDisplay;
    public Player p;
    private Shop currentShop = null;
    public InventoryButton upgradeButton;
    private Item essencePrefab;
    private Item dyePrefab;
    private Item rarityPrefab;
    public PartyScreen materialShop;
    public Shop myShop;
    private int variantLength = 8;
    //public PartyCursor pc;


    private void Awake()
    {
        coinParticles = Resources.Load<ParticleSystem>("Prefabs/World/Particles/SellCoin");
        essencePrefab = Resources.Load<Item>("Prefabs/World/Items/Essence");
        rarityPrefab = Resources.Load<Item>("Prefabs/World/Items/Rarity");
        dyePrefab = Resources.Load<Item>("Prefabs/World/Items/Dye");
        allPlates = new List<List<PartyPlate>>() {plates, itemPlates, equipmentPlates };
        currentScreen = plates;
        if (use == "portal")
        {
            setUpPortalShop();
        }
        if (use == "itemshop")
        {
            setUpItemShop();
        }
    }

    // Use this for initialization
    void Start () {
        if (use == "player")
        {
            setUpPlayer();
            p.loadFromFile();
        }
        if (myShop != null)
        {
            currentShop = myShop;
        }
        //pc.gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -100);
    }

    private void setUpPlayer()
    {
        upgradeButton.goAway(true);
        foreach(PartyPlate p in GetComponentsInChildren<PartyPlate>())
        {
            playerParty.Add(p);
            p.setScreen(this);
            p.setPurpose("opal");
        }
        int i = -2;
        int j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("opal");
            temp.setHome();
            plates.Add(temp);
            i++;
        }
        i = -2;
        j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("item");
            temp.setHome();
            itemPlates.Add(temp);
            i++;
        }
        enableAll(false, itemPlates);
        updateGold(0);
    }

    public List<PartyPlate> getContents(int num)
    {
        if(num == 0)
        {
            return plates;
        }
        else if(num == 1)
        {
            return itemPlates;
        }
        else
        {
            return equipmentPlates;
        }
    }

    private void setUpItemShop()
    {
        int i = -2;
        int j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("item");
            temp.setHome();
            itemPlates.Add(temp);
            i++;
        }
        //enableAll(false, plates);
        clickType = "material";
    }

    private void setUpPortalShop()
    {
        int i = -2;
        int j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("opal");
            temp.setHome();
            plates.Add(temp);
            i++;
        }
        i = -2;
        j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("opal");
            temp.setHome();
            itemPlates.Add(temp);
            i++;
        }
        i = -2;
        j = 2;
        while (true)
        {
            if (i == 3)
            {
                j--;
                i = -2;
                if (j == -3)
                {
                    break;
                }
            }
            PartyPlate temp = Instantiate<PartyPlate>(template, this.transform);
            temp.transform.localPosition = new Vector3(i * 5, j * 5 - 1, -1);
            temp.setScreen(this);
            temp.setPurpose("item");
            temp.setHome();
            equipmentPlates.Add(temp);
            i++;
        }
        enableAll(false, itemPlates);
        enableAll(false, equipmentPlates);
        print("all set up");
    }

    private void enableAll(bool enable, List<PartyPlate> ps)
    {
        foreach(PartyPlate p in ps)
        {
            if (enable)
            {
                p.goHome();
            }
            else
            {
                p.transform.position = new Vector3(-100, -100, -100);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (toSwap != null && toSwap.getOpal() != null)
        {
            toSwap.getOpal().transform.position = new Vector2(mousePos.x, mousePos.y);
        }else if (toSwap != null && toSwap.getItem() != null)
        {
            toSwap.getItem().transform.position = new Vector2(mousePos.x, mousePos.y);
        }

    }

    public void addOpal(string opal)
    {
        string opalN = opal.Substring(0, opal.Length-variantLength);
        string opalV = opal.Substring(opal.Length - variantLength, variantLength);
        //print(opalN + " " + opalV);
        
        OpalScript opalPrefab = Resources.Load<OpalScript>("Prefabs/Opals/" + opalN);
        foreach (List<PartyPlate> ls in allPlates)
        {
            if (myShop != null)
            {
                print(myShop.shopName);
                print(ls.Count);
            }
            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].getPurpose() == "opal" && ls[i].getOpal() == null)
                {
                    //print(opalN);
                    ls[i].setPlate(opalPrefab, 0, 0, opalV);
                    return;
                }
            }
        }
    }

    public void addItem(Item item)
    {
        foreach (List<PartyPlate> ls in allPlates)
        {
            for (int i = 0; i < ls.Count; i++)
            {
                if (ls[i].getPurpose() == "item" && ls[i].getItem() == null)
                {
                    //print(opalN);
                    ls[i].setItem(item);
                    return;
                }
            }
        }
    }

    public void addEssence(string type)
    {
        Item temp = Instantiate<Item>(essencePrefab);
        temp.setUpEssence(type);
        temp.transform.localScale *= 0.3f;
        addItem(temp);
    }

    public void addItem(string code)
    {
        if(code.Substring(0,1) == "r")
        {
            Item temp = Instantiate<Item>(rarityPrefab);
            temp.createFromString(code);
            temp.transform.localScale *= 0.3f;
            addItem(temp);
        }
        else if (code.Substring(0, 1) == "d")
        {
            Item temp = Instantiate<Item>(dyePrefab);
            temp.createFromString(code);
            temp.transform.localScale *= 0.3f;
            addItem(temp);
        }else if(code.Substring(0, 1) == "e")
        {
            Item temp = Instantiate<Item>(essencePrefab);
            temp.setUpEssence(code.Substring(1));
            temp.transform.localScale *= 0.3f;
            addItem(temp);
        }
    }
    public void setCurrentShop(Shop c)
    {
        currentShop = c;
        if (c != null)
        {
            upgradeButton.setText("Upgrade: o" + c.getUpgradeCost());
            upgradeButton.goAway(false);
            c.greet();
        }
        else
        {
            upgradeButton.goAway(true);
        }
    }

    public Shop getCurrentShop()
    {
        return currentShop;
    }

    public void activate(bool act)
    {
        //pc.gameObject.SetActive(act);
    }

    public void getClick(PartyPlate input)
    {
        if(clickType == "sell" && input.getOpal() != null)
        {
            ParticleSystem temp = Instantiate<ParticleSystem>(coinParticles, input.transform);
            temp.transform.localPosition = new Vector3(0, 0, -2);
            
            currentShop.purchase();
            updateGold(currentShop.value(input.getOpal().getVariantNum(), null));
            sendMaterials(input.getOpal());
            input.setOpal(null);
            
        }else if (clickType == "material" && input.getPurpose() == "item" && input.getItem() != null)
        {
            ParticleSystem temp = Instantiate<ParticleSystem>(coinParticles, input.transform);
            temp.transform.localPosition = new Vector3(0, 0, -2);
            currentShop.purchase();
            p.updateGold(-currentShop.getItemPrice(input.getItem()));
            p.addItem(input.getItem());
            input.setItem(null);
        }else if (clickType == "sellItem" && input.getPurpose() == "item" && input.getItem() != null)
        {
            ParticleSystem temp = Instantiate<ParticleSystem>(coinParticles, input.transform);
            temp.transform.localPosition = new Vector3(0, 0, -2);
            currentShop.purchase();
            p.updateGold(currentShop.value(-1, input.getItem()));
            input.setItem(null);
        }else if (clickType == "swap" && (input.getItem() != null || input.getOpal() != null))
        {
            if (myShop != null && myShop.shopName == "PortalVendor")
            {
                if (input.getItem() != null)
                {
                    p.addItem(input.getItem());
                    input.setItem(null);
                }
                else if (input.getOpal() != null)
                {
                    p.addOpal(input.getOpal().getVariant());
                    input.setOpal(null);
                }
            }
            else
            {
                if (input.getItem() != null)
                {
                    currentShop.addToInventory(input.getItem());
                    input.setItem(null);
                }
                else if (input.getOpal() != null)
                {
                    currentShop.addToInventory(input.getOpal());
                    input.setOpal(null);
                }
            }
        }
        else if(toSwap == null && (input.getOpal() != null || input.getItem() != null))
        {
            toSwap = input;
        }
        else
        {
            if (toSwap != null && input.getPurpose() == "opal" && toSwap.getPurpose() == "opal")
            {
                OpalScript temp = toSwap.getOpal();
                toSwap.setOpal(input.getOpal());
                input.setOpal(temp);
                toSwap = null;
            }else if (toSwap != null && input.getPurpose() == "item" && toSwap.getPurpose() == "item")
            {
                Item temp = toSwap.getItem();
                toSwap.setItem(input.getItem());
                input.setItem(temp);
                toSwap = null;
            }
        }
    }

    public void resetLift()
    {
        if(toSwap != null && toSwap.getOpal() != null)
        {
            toSwap.getOpal().transform.localPosition = new Vector3(0, -1, -2);
            toSwap = null;
        }else if(toSwap != null && toSwap.getItem() != null)
        {
            toSwap.getItem().transform.localPosition = new Vector3(0, -1, -2);
            toSwap = null;
        }
    }

    public void setClickType(string set)
    {
        clickType = set;
    }

    public void updateGold(int g)
    {
        goldAmount += g;
        goldDisplay.text = "Gold: " + goldAmount;
    }

    public int upgradeShop()
    {
        
        if(currentShop == null)
        {
            return -1;
        }
        if(goldAmount >= currentShop.getUpgradeCost())
        {
            updateGold(-currentShop.getUpgradeCost());
            currentShop.upgradeShop();
            return currentShop.getUpgradeCost();
        }
        return currentShop.getUpgradeCost();
    }

    public void updateCurrent(string purpose)
    {
        if(purpose == "Opal" && currentScreen != plates)
        {
            enableAll(false, currentScreen);
            enableAll(true, plates);
            currentScreen = plates;
        }else if (purpose == "Items" && currentScreen != itemPlates)
        {
            enableAll(false, currentScreen);
            enableAll(true, itemPlates);
            currentScreen = itemPlates;
        }
        else if (purpose == "Equipment" && currentScreen != equipmentPlates)
        {
            enableAll(false, currentScreen);
            enableAll(true, equipmentPlates);
            currentScreen = equipmentPlates;
        }
    }

    public void clear()
    {
        foreach(PartyPlate p in plates)
        {
            p.setOpal(null);
        }
        foreach (PartyPlate p in itemPlates)
        {
            p.setItem(null);
        }
        updateGold(-goldAmount);
    }

    public int getGold()
    {
        return goldAmount;
    }

    public void sendMaterials(OpalScript o)
    {
        string type1 = o.getMainType();
        string type2 = o.getSecondType();
        int intNum = o.getVariantNum();
        int skin = intNum % 100;
        int particleSystem = (intNum / 100) % 100;
        int particleColor = (intNum / 10000) % 100;
        int size = (intNum / 1000000) % 100;
        if(materialShop != null)
        {
            materialShop.addEssence(type1);
            materialShop.addEssence(type2);
            if(particleSystem != 0)
            {
                materialShop.addItem("r"+particleSystem);
                materialShop.addItem("d" + particleColor);
            }
        }
        else
        {
            print("oof! you're not supposed to call this!");
        }
    }

    public Shop getMyShop()
    {
        return myShop;
    }

    public List<OpalScript> getAllOpals()
    {
        List<OpalScript> output = new List<OpalScript>();
        foreach(PartyPlate p in plates)
        {
            if(p.getPurpose() == "opal" && p.getOpal() != null)
            {
                output.Add(p.getOpal());
            }
        }
        foreach (PartyPlate p in itemPlates)
        {
            if (p.getPurpose() == "opal" && p.getOpal() != null)
            {
                output.Add(p.getOpal());
            }
        }
        foreach (PartyPlate p in equipmentPlates)
        {
            if (p.getPurpose() == "opal" && p.getOpal() != null)
            {
                output.Add(p.getOpal());
            }
        }
        return output;
    }

    public List<Item> getAllItems()
    {
        List<Item> output = new List<Item>();
        foreach (PartyPlate p in plates)
        {
            if (p.getPurpose() == "item" && p.getItem() != null)
            {
                output.Add(p.getItem());
            }
        }
        foreach (PartyPlate p in itemPlates)
        {
            if (p.getPurpose() == "item" && p.getItem() != null)
            {
                output.Add(p.getItem());
            }
        }
        foreach (PartyPlate p in equipmentPlates)
        {
            if (p.getPurpose() == "item" && p.getItem() != null)
            {
                output.Add(p.getItem());
            }
        }
        return output;
    }

    public List<PartyPlate> getParty()
    {
        return playerParty;
    }

    public void addToParty(string opal)
    {
        string opalN = opal.Substring(0, opal.Length - variantLength);
        string opalV = opal.Substring(opal.Length - variantLength, variantLength);

        OpalScript opalPrefab = Resources.Load<OpalScript>("Prefabs/Opals/" + opalN);
        foreach (PartyPlate p in playerParty)
        {
            if (p.getPurpose() == "opal" && p.getOpal() == null)
            {
                //print(opalN);
                p.setPlate(opalPrefab, 0, 0, opalV);
                return;
            }
        }
    }

}
