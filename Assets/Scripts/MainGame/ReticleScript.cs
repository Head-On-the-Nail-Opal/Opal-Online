using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleScript : MonoBehaviour {
    public Sprite open;
    public Sprite closed;
    private bool currentlyOpen = true;
    private SpriteRenderer sr;
    private bool toggled = false;


	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setState(bool o)
    {
        if (o)
        {
            sr.sprite = open;
            currentlyOpen = true;
        }
        else
        {
            sr.sprite = closed;
            currentlyOpen = false;
        }
    }

    public bool getOpen()
    {
        return currentlyOpen;
    }

    public void toggleMe(bool t)
    {
        toggled = t;
        if (toggled)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
    }
}
