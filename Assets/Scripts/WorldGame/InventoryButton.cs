using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    public string purpose;
    private Material blueClr;
    private Material redClr;
    private Material softBlue;
    private Material softRed;
    private PartyScreen ps;
    private bool current = false;
    private MeshRenderer mr;
    public Text myText;

    private void Awake()
    {
        blueClr = Resources.Load<Material>("Materials/Mat_Blue");
        redClr = Resources.Load<Material>("Materials/Mat_DarkRed");
        softRed = Resources.Load<Material>("Materials/Mat_SoftRed");
        softBlue = Resources.Load<Material>("Materials/Mat_SoftBlue");
        ps = GetComponentInParent<PartyScreen>();
        mr = GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (purpose == "Upgrade")
        {
            myText.text = "Upgrade: o" + ps.upgradeShop();
            myText.color = Color.white;
        }
        if (purpose == "Opal")
        {
            setCurrent(true);
        }
        else
        {
            setCurrent(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        if (current)
        {
            mr.material = softRed;
        }
        else
        {
            mr.material = softBlue;
        }
    }

    public void OnMouseExit()
    {
        if (current)
        {
            mr.material = redClr;
        }
        else
        {
            mr.material = blueClr;
        }
    }

    public void setText(string input)
    {
        myText.text = input;
    }

    public void goAway(bool go)
    {
        this.gameObject.SetActive(!go);
    }

    public void OnMouseUp()
    {
        if(purpose == "Upgrade")
        {
            myText.text = "Upgrade: o" + ps.upgradeShop();  
        }
        else if (!current)
        {
            foreach (InventoryButton ib in transform.parent.GetComponentsInChildren<InventoryButton>())
            {
                ib.setCurrent(false);
            }
            setCurrent(true);
        }
    }

    public void setCurrent(bool c)
    {
        current = c;
        if (c)
        {
            mr.material = redClr;
        }
        else
        {
            mr.material = blueClr;
        }
        ps.updateCurrent(purpose);
    }
}
