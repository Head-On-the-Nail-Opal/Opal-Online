using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject essencePrefab;
    private string itemType = "";
    private string code = "";
    private Dictionary<string, Color> findColor;
    private List<Color> particleColors;
    private List<Color> representativeRarity;
    // Start is called before the first frame update
    private void Awake()
    {
        essencePrefab = Resources.Load<GameObject>("Prefabs/World/Essence");
        findColor = new Dictionary<string, Color>() {
            { "Light", Color.white },
            { "Grass", Color.green },
            { "Water", Color.blue },
            { "Fire", new Color(255/255f, 165/255f, 0/255f)},
            { "Air", new Color(255 / 255f, 251 / 255f, 201 / 255f)},
            { "Ground", new Color(153/255f, 102/255f, 51/255f)},
            { "Electric", new Color(255 / 255f, 255 / 255f, 102 / 255f)},
            { "Metal", new Color(191 / 255f, 182 / 255f, 163 / 255f) },
            { "Plague", new Color(78 / 255f, 0, 22 / 255f) },
            { "Dark", new Color(25 / 255f, 25 / 255f, 25 / 255f) },
            {"Laser", Color.red},
            {"Void", Color.grey},
            {"Swarm", new Color(204/255f, 1, 102/255f)} };
        representativeRarity = new List<Color>() {Color.red, Color.blue, Color.white, Color.gray, Color.yellow, Color.cyan, Color.magenta };
        particleColors = new List<Color>() { Color.red, Color.green, Color.blue, Color.cyan, Color.black, Color.gray, Color.magenta, Color.white, new Color(255 / 256f, 165 / 256f, 0), new Color(212 / 256f, 175 / 256f, 55 / 256f), new Color(0, 102 / 256f, 0), new Color(1, 153 / 255f, 1) };
   
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUpEssence(string type)
    {
        GetComponent<SpriteRenderer>().color = findColor[type];
        itemType = type + " Essence";
        code = "e" + type;
    }

    public void setUpDye(int index)
    {
        GetComponent<SpriteRenderer>().color = particleColors[index];
        itemType = "Dye " + index;
        code = "d" + index;
    }

    public void setUpRarity(int index)
    {
        GetComponent<SpriteRenderer>().color = representativeRarity[index-1];
        switch (index)
        {
            case 1:
                itemType = "Rarity: Flames";
                break;
            case 2:
                itemType = "Rarity: Drips";
                break;
            case 3:
                itemType = "Rarity: Twinkle";
                break;
            case 4:
                itemType = "Rarity: Crown";
                break;
            case 5:
                itemType = "Rarity: Dusty";
                break;
            case 6:
                itemType = "Rarity: Hyperspeed";
                break;
            case 7:
                itemType = "Rarity: Portal";
                break;
        }
        code = "r" + index; 
    }

    public void setItemName(string name)
    {
        itemType = name;
    }

    public string getItemName()
    {
        return itemType;
    }

    public void setCode(string c)
    {
        code = c;
    }

    public void createFromString(string c)
    {
        string baseCode = c.Substring(0, 1);
        if(baseCode == "e")
        {
            setUpEssence(baseCode.Substring(1));
        }else if(baseCode == "d")
        {
            setUpDye(int.Parse(c.Substring(1)));
        }else if(baseCode == "r")
        {
            setUpRarity(int.Parse(c.Substring(1)));
        }
    }

    public string getCode()
    {
        return code;
    }
}
