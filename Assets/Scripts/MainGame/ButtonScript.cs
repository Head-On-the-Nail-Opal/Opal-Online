using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    public Sprite unpressed;
    public Sprite hovering;
    public Sprite pressed;

    public int buttonNum;

    private SpriteRenderer sr;
    // Use this for initialization
    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        sr.sprite = unpressed;
        if (Input.GetKey(KeyCode.Alpha1) && buttonNum == 0)
        {
            sr.sprite = pressed;
        }
        else if (Input.GetKey(KeyCode.Alpha2) && buttonNum == 1)
        {
            sr.sprite = pressed;
        }
        else if (Input.GetKey(KeyCode.Alpha3) && buttonNum == 2)
        {
            sr.sprite = pressed;
        }
    }
}
