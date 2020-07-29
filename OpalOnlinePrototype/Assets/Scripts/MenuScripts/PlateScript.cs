using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateScript : MonoBehaviour {
    private OpalScript myOpal;
    private MainMenuScript main;
    private GameObject mainCam;
    private bool disable = false;
    GameObject panel;
    GameObject background;
    // Use this for initialization
    void Start () {
        mainCam = GameObject.Find("Main Camera");
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
    }

    public void setPlate(OpalScript opal, float x, float y)
    {
        myOpal = opal;
        OpalScript opalOne = Instantiate<OpalScript>(opal);
        opalOne.setOpal(null);
        //opalOne.setVariant("1");
        transform.position = new Vector3(x, y, -1);
        opalOne.transform.position = new Vector3(transform.position.x, transform.position.y - 0.8f, -2.5f);
        opalOne.transform.localScale *= 1.2f;
        opalOne.transform.Rotate(new Vector3(0,45,0));
    }

    public void disableMouse() { disable = true; }

    public void setColor(string clr)
    {
        foreach(Transform t in this.GetComponentsInParent<Transform>())
        {
            
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
        if (color == "blue")
            panel = Resources.Load<GameObject>("Prefabs/BluePlate");
        else if (color == "red")
            panel = Resources.Load<GameObject>("Prefabs/RedPlate");
        else if (color == "green")
            panel = Resources.Load<GameObject>("Prefabs/GreenPlate");
        else if (color == "orange")
            panel = Resources.Load<GameObject>("Prefabs/OrangePlate");
        else if(color == "reset")
        {
            DestroyImmediate(background);
            return;
        }
        background = Instantiate<GameObject>(panel, this.transform);
        background.transform.localPosition = new Vector3(0, -1, -1.4f);
    }

    private void OnMouseUp()
    {
        if (!disable)
        {
            main.menuState = "Draft";
            main.mainDisplay.setCurrentOpal(myOpal);
            main.setOpaloids();
            mainCam.transform.position = new Vector3(mainCam.transform.position.x - 20, 15, -10);
        }
    }
}
