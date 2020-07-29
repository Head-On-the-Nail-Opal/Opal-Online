using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamePlateScript : MonoBehaviour {
    private Animator myAnimator;


	// Use this for initialization
	void Start () {
        myAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    // 1 is red, 2 is blue
    public void setPlayer(int player)
    {
        if(player == 1)
        {
            myAnimator.CrossFade("RedName",0);
            transform.localScale = new Vector3(1, 1, 0);
        }
        else if(player == 2)
        {
            myAnimator.CrossFade("BlueName",0);
            transform.localScale = new Vector3(1, 1, 0);
        }
        else if(player == 3)
        {
            myAnimator.CrossFade("GreenName", 0);
            transform.localScale = new Vector3(2, 2, 0);
        }
        else if(player == 4)
        {
            myAnimator.CrossFade("OrangeName", 0);
            transform.localScale = new Vector3(2, 2, 0);
        }
    }
}
