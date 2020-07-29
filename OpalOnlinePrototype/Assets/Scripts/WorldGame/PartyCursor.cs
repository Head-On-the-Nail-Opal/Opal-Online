using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyCursor : MonoBehaviour {
    private Camera mainCam;
    private float myscale = 2.5f;

	// Use this for initialization
	void Start () {
		mainCam = Camera.main;
        transform.position = new Vector3(-10, 9, -2);
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
        Vector3 newPos = new Vector3(worldPos.x + 2.5f, worldPos.y, -2);
        if ((newPos.x) % myscale * 2 != 0)
        {
            float newX = myscale * 2 * (int)((worldPos.x - myscale*2) / (myscale * 2)) + myscale * 2;
            float newY = myscale * 2 * (int)(worldPos.y  / (myscale * 2)) + myscale * 1.6f;
            if (!(newX > 100 || newX < -100 || newY < -100 || newY > 100))
            {
                transform.position = new Vector3(newX, newY, -2);
            }
            else
            {
                //transform.position = new Vector3(-100, -100, -100);
            }
        }
    }
}
