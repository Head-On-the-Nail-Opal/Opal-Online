using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyPlate : MonoBehaviour {
    private OpalScript myOpal = null;
    private Item myItem = null;
    private OpalScript opalOne;
    private PartyScreen myScreen;
    private string purpose;
    private Vector3 home;
    private Item essencePrefab;
	// Use this for initialization
	void Start () {
        essencePrefab = Resources.Load<Item>("Prefabs/World/Items/Essence");
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material.color = new Color(0, 0.5f, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        //if(transform.po)
	}

    public void setPlate(OpalScript opal, float x, float y, string variant)
    {
        if(opalOne != null)
        {
            Destroy(opalOne.gameObject);
        }
        myOpal = opal;
        opalOne = Instantiate<OpalScript>(opal, this.transform);
        opalOne.setOpal(null);
        opalOne.transform.Rotate(new Vector3(0, 45, 0));
        opalOne.transform.localPosition = new Vector3(0,-1,-2);
        opalOne.transform.localScale *= 2f;
        opalOne.setVariant(variant);
    }

    public void setOpal(OpalScript opal)
    {
        if (opalOne != null)
        {
            Destroy(opalOne.gameObject);
        }
        if(opal == null)
        {
            opalOne = null;
            return;
        }
        myOpal = opal;
        opalOne = Instantiate<OpalScript>(opal, this.transform);
        opalOne.setOpal(null);
        opalOne.GetComponent<Animator>().enabled = true;
        opalOne.transform.localPosition = new Vector3(0, -1, -2);
        opalOne.transform.localScale *= 2f;
        opalOne.setVariant(opal.getVariant().Substring(opal.getMyName().Length));
    }

    public void setItem(Item item)
    {
        if (myItem != null)
        {
            Destroy(myItem.gameObject);
        }
        if (item == null)
        {
            return;
        }
        myItem = Instantiate<Item>(item, transform);
        myItem.setItemName(item.getItemName());
        myItem.setCode(item.getCode());
        myItem.transform.parent = transform;
        if(myItem.GetComponent<Animator>() != null )
            myItem.GetComponent<Animator>().enabled = true;
        myItem.transform.localPosition = new Vector3(0, -1, -2);
        //DestroyImmediate(item.gameObject);
    }

    public Item getItem()
    {
        return myItem;
    }

    public void setScreen(PartyScreen ps)
    {
        myScreen = ps;
    }

    public void setDetails(string variant)
    {
        myOpal.setVariant(variant);
    }

    public OpalScript getOpal()
    {
        return opalOne;
    }

    public void OnMouseEnter()
    {
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material.color = new Color(0, 1, 0);
        }
        if(myScreen.getCurrentShop() != null && (opalOne != null))
        {
            myScreen.getCurrentShop().offer(myScreen.getCurrentShop().value(opalOne.getVariantNum(), myItem), false);
        }else if (myScreen.getCurrentShop() != null && myItem != null && myScreen.getMyShop() == null)
        {
            myScreen.getCurrentShop().offer(myScreen.getCurrentShop().value(-1, myItem), true);
        }
        else if(myScreen.getCurrentShop() != null && myItem != null && myScreen.getMyShop() != null)
        {
            myScreen.getCurrentShop().sellItem(myItem);
        }
    }

    public void OnMouseExit()
    {
        foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
        {
            m.material.color = new Color(0, 0.5f, 0);
        }
    }

    public void OnMouseDown()
    {
        myScreen.getClick(this);
    }

    public void setPurpose(string p)
    {
        purpose = p;
    }

    public string getPurpose()
    {
        return purpose;
    }

    public void goHome()
    {
        transform.localPosition = home;
    }

    public void setHome()
    {
        home = transform.localPosition;
    }
}
