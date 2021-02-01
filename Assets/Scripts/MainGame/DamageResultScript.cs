using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageResultScript : MonoBehaviour {
    int i;
    public Text body;
	// Use this for initialization

    public void setUp(int amount, OpalScript myBoy)
    {
        transform.position = myBoy.transform.position;
        transform.rotation = myBoy.transform.rotation;
        transform.localScale /= 5;
        if (amount > 0)
        {
            body.text = "+" + amount;
            body.color = new Color(0, 1, 0);
        }else if(amount < 0)
        {
            body.text = "" + amount;
        }
        else
        {
            body.text = " ";
            body.color = new Color(0, 0, 0);
        }
    }

    public void setUp(int num, int denom, OpalScript myBoy)
    {
        transform.position = myBoy.transform.position;
        transform.rotation = myBoy.transform.rotation;
        transform.localScale /= 5;
        body.text = num+"/"+denom;
        body.color = Color.green;
    }

    void Awake () {
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(i < 50)
        {
            i++;
            transform.position = new Vector3(transform.position.x, transform.position.y+0.01f, transform.position.z);
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
	}
}
