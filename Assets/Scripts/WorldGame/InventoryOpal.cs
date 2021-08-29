using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpal : MonoBehaviour
{
    private OpalScript myOpal;
    public GameObject myBackground;
    private bool mouseOver = false;
    private Player player;
    private bool followMouse = false;
    private bool party = false;
    private int index;
    private bool trash = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseOver)
        {
            if (myOpal != null)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    player.inspectOpal(myOpal);
                }
            }
        }
        if (followMouse)
        {
            if (myOpal != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                myOpal.transform.position = new Vector3(mousePos.x, mousePos.y, -13);
            }
        }
    }

    public void setPlayer(Player p)
    {
        player = p;
    }

    public void setTrash()
    {
        trash = true;
        myBackground.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
        transform.localPosition = new Vector3(4, -2.5f, -1);
    }

    public bool getTrash()
    {
        return trash;
    }

    public void setOpal(string opal, int i)
    {
        if (trash)
        {
            setOpal(opal);
            return;
        }
        index = i;
        float x = -3 + index % 7;
        float y = 3.5f - (index / 7);
        transform.localPosition = new Vector3(x, y, -1);
        setOpal(opal);
    }

    public void setPartyOpal(string opal, int i)
    {
        party = true;
        index = i;
        float x = -5;
        float y = 3f - index;
        transform.localPosition = new Vector3(x, y, -1);
        setOpal(opal);
    }

    public bool getParty()
    {
        return party;
    }

    public void setOpal(string opal)
    {
        if (opal == null)
        {
            clearOpal();
            return;
        }
        int variantLength = 8;
        string opalN = opal.Substring(0, opal.Length - variantLength);
        string opalV = opal.Substring(opal.Length - variantLength, variantLength);
        OpalScript opalPrefab = Resources.Load<OpalScript>("Prefabs/Opals/" + opalN);
        if (myOpal != null)
        {
            Destroy(myOpal.gameObject);
        }
        myOpal = Instantiate<OpalScript>(opalPrefab, transform);
        myOpal.setOpal(null);
        //myOpal.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        myOpal.setVariant(opalV);
        myOpal.GetComponent<Animator>().enabled = true;
        myOpal.transform.rotation = Quaternion.Euler(0, 0, 0);
        //transform.localPosition = new Vector3(0, 0, 0);
        myOpal.transform.localScale *= 0.75f;
        myOpal.transform.localPosition = new Vector3(0, 0, -1);
    }

    public void setDetails(OpalScript o)
    {
        myOpal.setDetails(o);
    }

    public void setRandomPersonality()
    {
        myOpal.setRandomPersonality();
    }

    public void clearOpal()
    {
        if (myOpal != null)
        {
            Destroy(myOpal.gameObject);
            myOpal = null;
        }
    }

    public void setSwitching(bool s)
    {
        if (s)
        {
            followMouse = true;
        }
        else
        {
            followMouse = false;
            if(myOpal != null)
                myOpal.transform.localPosition = new Vector3(0, 0, -1);
        }
    }

    public OpalScript getOpal()
    {
        return myOpal;
    }

    public int getIndex()
    {
        return index;
    }

    public void OnMouseEnter()
    {
        if (!mouseOver)
        {
            mouseOver = true;
            myBackground.GetComponent<MeshRenderer>().material.color *= 2;
            //player.displayOpal(myOpal);
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            player.switchOpals(this);
        }
        if (Input.GetMouseButton(1))
        {
            //player.inspectOpal(this.myOpal);
        }
    }

    public void OnMouseExit()
    {
        if (mouseOver)
        {
            mouseOver = false;
            myBackground.GetComponent<MeshRenderer>().material.color /= 2;
        }
    }

    public void showMe(bool show)
    {
        if (myOpal != null)
            myOpal.gameObject.SetActive(show);
    }
}
