using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalSelector : MonoBehaviour
{
    private PalSelector palPrefab;
    private List<Pal> palPrefabs = new List<Pal>();

    private List<PalSelector> palBorders = new List<PalSelector>();

    private Pal myPal = null;

    private PalSelector myParent;

    public bool doStuff = false;
    private bool displaying = false;
    private MainMenuScript main;

    // Start is called before the first frame update
    void Awake()
    {
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        if (!doStuff)
            return;
        palPrefab = Resources.Load<PalSelector>("Prefabs/PalPlate");

        palPrefabs.Add(null);
        foreach(Pal p in Resources.LoadAll<Pal>("Prefabs/Pals"))
        {
            palPrefabs.Add(p);
        }

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPal(Pal p)
    {
        if(myPal != null)
        {
            Destroy(myPal.gameObject);
            myPal = null;
            if (myParent == null)
                main.setCurrentPal(null);
        }
        if(p != null)
        {
            myPal = Instantiate<Pal>(p, transform);
            myPal.transform.localPosition = new Vector3(-0.015f, 0.015f, -1);
            myPal.transform.localScale *= 0.8f;
            if (myParent == null)
                main.setCurrentPal(p);
        }
    }

    private void displayPals(bool show)
    {
        //setPal(null);
        displaying = show;
        foreach(PalSelector ps in palBorders)
        {
            ps.setPal(null);
            Destroy(ps.gameObject);
        }
        palBorders.Clear();
        if (show)
        {
            int i = 0;
            foreach (Pal p in palPrefabs)
            {
                PalSelector temp = Instantiate<PalSelector>(palPrefab, transform);
                temp.doStuff = true;
                temp.setParent(this);
                temp.setPal(p);
                temp.transform.localPosition = new Vector3(i*0.145f, -0.145f, 0);
                temp.transform.localScale *= 0.9f;
                palBorders.Add(temp);
                i++;
            }
        }
    }

    public void setParent(PalSelector ps)
    {
        myParent = ps;
    }

    public PalSelector getParent()
    {
        return myParent;
    }

    public void OnMouseUp()
    {
        if (!doStuff)
            return;
        if (getParent() != null)
        {
            getParent().setPal(myPal);
            getParent().displayPals(false);
        }
        else
        {
            displayPals(!displaying);
        }
    }

    public void OnMouseEnter()
    {
        if (!doStuff)
            return;
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.4f);
    }

    public void OnMouseExit()
    {
        if (!doStuff)
            return;
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
    }


}
