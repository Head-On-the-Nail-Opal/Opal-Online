using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamDisplay : MonoBehaviour {
    GameObject panel;
    int currentOpals = 0;
    List<OpalScript> opals = new List<OpalScript>();
	// Use this for initialization
	void Start () {
		
	}

    public void setUp(string color, int numOpals)
    {
        if (color == "blue")
            panel = Resources.Load<GameObject>("Prefabs/BluePlate");
        else if (color == "red")
            panel = Resources.Load<GameObject>("Prefabs/RedPlate");
        else if (color == "green")
            panel = Resources.Load<GameObject>("Prefabs/GreenPlate");
        else if (color == "orange")
            panel = Resources.Load<GameObject>("Prefabs/OrangePlate");
        for(int i = 0; i < numOpals; i++)
        {
            GameObject temp = Instantiate<GameObject>(panel, this.transform);
            temp.transform.localScale = new Vector3(3, 3f, 1);
            if(color == "green" || color == "orange")
            {
                temp.transform.localScale = new Vector3(0.1923077f * 0.6f, 0.2307692f * 0.5f, 1);
            }
            temp.transform.localPosition = new Vector3(0, -i);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addOpal(OpalScript o)
    {
        OpalScript temp = Instantiate<OpalScript>(o, this.transform);
        opals.Add(temp);
        temp.setOpal(null);
        temp.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        temp.transform.Rotate(new Vector3(0, 45, 0));
        temp.transform.localPosition = new Vector3(0, -currentOpals, -0.1f);
        currentOpals++;
    }

    public void clear()
    {
        currentOpals = 0;
        foreach(OpalScript o in opals)
        {
            DestroyImmediate(o.gameObject);
        }
        opals.Clear();
    }
}
