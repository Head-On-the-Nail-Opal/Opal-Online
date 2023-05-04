using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{
    public ItemSlot itemSlot;
    public ItemDescriptions charms;
    public Player p;
    public Text description;
    private List<ItemSlot> items = new List<ItemSlot>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCharms()
    {
        clear();
        if(charms.itemsCount() == 0)
        {
            charms.setUp();
        }
        int j = 0;
        int k = 0;
        for(int i = 0; i < charms.itemsCount(); i++)
        {

           
            ItemSlot temp = Instantiate<ItemSlot>(itemSlot, transform);
            temp.setPlayer(p);
            float x = -4 + (k*4);
            float y = 4 - j*0.5f;
            temp.transform.localPosition = new Vector3(x, y, -1);
            temp.setText(charms.getItem(i));
            items.Add(temp);
            k++;
            if (k == 3)
            {
                k = 0;
                j++;
            }
            
        }
    }

    public void setPlayGame()
    {
        clear();
    }

    public void clear()
    {
        foreach (ItemSlot i in items)
        {
            Destroy(i.gameObject);
        }
        items.Clear();
    }

    private void addButton(string text)
    {
        int k = items.Count;
        int j = k / 4;
        ItemSlot temp = Instantiate<ItemSlot>(itemSlot, transform);
        temp.setPlayer(p);
        float x = -4 + (k * 4);
        float y = 4 - j * 0.5f;
        temp.transform.localPosition = new Vector3(x, y, -1);
        temp.setText(text);
        items.Add(temp);
        k++;
        if (k == 3)
        {
            k = 0;
            j++;
        }
    }

    public void generateDescription(string item)
    {
        if(charms.getDescFromItem(item) != null)
        {
            description.text = charms.getDescFromItem(item);
        }
        else
        {
            description.text = "Generic item description";
        }
    }
}
