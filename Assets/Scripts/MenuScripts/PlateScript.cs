using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour {
    private OpalScript myOpal;
    private OpalScript specificOpal;
    private MainMenuScript main;
    private GameObject mainCam;
    private bool disable = false;
    private List<string> opalsPicked = new List<string>();
    private List<string> teamPicked = new List<string>();
    GameObject panel;
    GameObject background;
    private Color defaultBackground = new Color(0f, 0.5f, 0.1f);
    private bool team = false;
    // Use this for initialization
    void Start () {
        mainCam = GameObject.Find("Main Camera");
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        setColor(defaultBackground);
    }

    public void setPlate(OpalScript opal, float x, float y)
    {
        myOpal = opal;
        OpalScript opalOne = Instantiate<OpalScript>(opal);
        specificOpal = opalOne;
        opalOne.setOpal(null);
        //opalOne.setVariant("1");
        transform.position = new Vector3(x, y, -1);
        opalOne.transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, -2.5f);
        opalOne.transform.localScale *= 1.2f;
        opalOne.transform.Rotate(new Vector3(0,45,0));
    }

    public void setPlate(OpalScript opal)
    {
        if(specificOpal != null)
            DestroyImmediate(specificOpal.gameObject);
        if(opal == null)
        {
            myOpal = null;
            return;
        }
        myOpal = opal;
        OpalScript opalOne = Instantiate<OpalScript>(opal);
        
        opalOne.setOpal(null);
        //opalOne.setVariant("1");
        opalOne.transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, -2.5f);
        opalOne.transform.localScale *= 1.2f;
        opalOne.transform.Rotate(new Vector3(0, 45, 0));
        specificOpal = opalOne;
        if (panel != null)
            DestroyImmediate(background);
        int i = 0;
        foreach(string s in opalsPicked)
        {
            if(myOpal.getMyName() == s)
            {
                setTeamBack(teamPicked[i]);
            }
            i++;
        }
    }

    public void clearPlate()
    {
        if (specificOpal != null)
            DestroyImmediate(specificOpal.gameObject);
        myOpal = null;
        if (panel != null)
            DestroyImmediate(background);
    }

    public void disableMouse() { disable = true; }

    public void setColor(Color clr)
    {
        foreach(Transform g in this.GetComponentInChildren<Transform>())
        {
            if(g.name == "Backdrop")
                g.GetComponent<MeshRenderer>().material.color =  clr;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public OpalScript getOpal()
    {
        return myOpal;
    }

    public void setTeam(string color)
    {
        opalsPicked.Add(myOpal.getMyName());
        teamPicked.Add(color);
        if (color == "blue")
        {
            panel = Resources.Load<GameObject>("Prefabs/BluePlate");
            panel.transform.localScale = new Vector3(6, 6);
        }
        else if (color == "red")
        {
            panel = Resources.Load<GameObject>("Prefabs/RedPlate");
            panel.transform.localScale = new Vector3(6, 6);
        }
        else if (color == "green")
        {
            panel = Resources.Load<GameObject>("Prefabs/GreenPlate");
            panel.transform.localScale = new Vector3(0.23f, 0.23f);
            //panel.transform.localScale = new Vector3(6, 6);
        }
        else if (color == "orange")
        {
            panel = Resources.Load<GameObject>("Prefabs/OrangePlate");
            panel.transform.localScale = new Vector3(0.23f, 0.23f);
            //panel.transform.localScale = new Vector3(6, 6);

        }
        else if (color == "reset")
        {
            DestroyImmediate(background);
            opalsPicked.Clear();
            teamPicked.Clear();
            return;
        }
        background = Instantiate<GameObject>(panel, this.transform);
        background.transform.localPosition = new Vector3(0, -1, -1.4f);
    }

    public void setTeamBack(string color)
    {
        if (color == "blue")
        {
            panel = Resources.Load<GameObject>("Prefabs/BluePlate");
            panel.transform.localScale = new Vector3(6, 6);
        }
        else if (color == "red")
        {
            panel = Resources.Load<GameObject>("Prefabs/RedPlate");
            panel.transform.localScale = new Vector3(6, 6);
        }
        else if (color == "green")
        {
            panel = Resources.Load<GameObject>("Prefabs/GreenPlate");
            panel.transform.localScale = new Vector3(0.23f, 0.23f);
        }
        else if (color == "orange")
        {
            panel = Resources.Load<GameObject>("Prefabs/OrangePlate");
            panel.transform.localScale = new Vector3(0.23f, 0.23f);

        }
        else if (color == "reset")
        {
            DestroyImmediate(background);
            opalsPicked.Clear();
            teamPicked.Clear();
            return;
        }
        background = Instantiate<GameObject>(panel, this.transform);
        background.transform.localPosition = new Vector3(0, -1, -1.4f);
    }

    private void OnMouseUp()
    {
        if (!disable)
        {
            if (team)
            {
                main.chooseOneOpal(this);
            }
            else
            {
                main.addCurrentOpal();
            }
        }
    }

    private void OnMouseEnter()
    {
        setColor(new Color(0,0,0.5f));
        main.displayOpal(myOpal);
        if (team)
        {
            main.displayOpal(myOpal, true);
        }
    }

    private void OnMouseExit()
    {
        setColor(defaultBackground);
    }

    public void setTeamPlate()
    {
        team = true;
    }
}
