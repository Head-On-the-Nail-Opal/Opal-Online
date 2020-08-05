using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateSelector : MonoBehaviour {
    bool active = false;
    Camera mainCam;
    private MainMenuScript main;
    private int controllerRest = 0;
    private float myscale = 0.8f;
    private int maxCam = 15;
    private int minCam = 11;
    // Use this for initialization
    void Start () {
        mainCam = Camera.main;
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        transform.localScale = new Vector3(0.8f,0.8f, 1);
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (main.currentController == "Keyboard")
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
                Vector3 newPos = new Vector3(worldPos.x, worldPos.y, -2);
                if ((newPos.x - 15) % myscale*2 != 0)
                {
                    float newX = myscale * 2 * (int)(worldPos.x / (myscale * 2)) + myscale*0.75f;
                    float newY = myscale * 2 * (int)((worldPos.y + myscale/2) / (myscale * 2)) + myscale;
                    if (!(newX > 29 || newX < 14 || newY < 9 || newY > 21))
                    {
                        transform.position = new Vector3(newX, newY, -2);
                        if (mousePos.y < 150)
                        {
                            if (mainCam.transform.position.y > minCam)
                                mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y - 2 * myscale, -10);
                        }
                        else if (mousePos.y > 900)
                        {
                            if(mainCam.transform.position.y < maxCam)
                                mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 2 * myscale, -10);
                        }
                    }
                    else
                    {
                        //transform.position = new Vector3(-100, -100, -100);
                    }
                }
            }
        }else if(main.currentController == "Joystick1" || main.currentController == "Joystick2" || main.currentController == "Joystick3" || main.currentController == "Joystick4")
        {
            string twoSense = "";
            if(main.currentController == "Joystick2")
            {
                twoSense = " 2";
            }else if(main.currentController == "Joystick3")
            {
                twoSense = " 3";
            }
            else if (main.currentController == "Joystick4")
            {
                twoSense = " 4";
            }
            float newX = transform.position.x;
            float newY = transform.position.y;
            if (Input.GetAxis("LSUp" + twoSense) > 0)
            {
                if (controllerRest == 0)
                {
                    newY -= 2 * myscale;
                    controllerRest = 10;
                }
            }
            else if (Input.GetAxis("LSUp" + twoSense) < 0)
            {
                if (controllerRest == 0)
                {
                    newY += 2 * myscale;
                    controllerRest = 10;
                }
            }
            else if (Input.GetAxis("LSRight" + twoSense) > 0)
            {
                if (controllerRest == 0)
                {
                    newX += 2 * myscale;
                    controllerRest = 10;
                }
            }
            else if (Input.GetAxis("LSRight" + twoSense) < 0)
            {
                if (controllerRest == 0)
                {
                    newX -= 2 * myscale;
                    controllerRest = 10;
                }
            }
            if (!(newX > 29 || newX < 14 || newY < 9 || newY > 21))
            {
                transform.position = new Vector3(newX, newY, -2);
                if(newY < 11)
                {
                    if(mainCam.transform.position.y > 13)
                        mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y-2*myscale, -10);
                }else if(newY > 19)
                {
                    if (mainCam.transform.position.y < 15)
                        mainCam.transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y + 2 * myscale, -10);
                }
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            if(Input.GetButtonDown("button 0" + twoSense) && main.menuState == "Pick")
            {
                main.menuState = "Draft";
                main.mainDisplay.setCurrentOpal(main.findOpal((newX-14)/(2*myscale)-6, Mathf.Abs((newY-21)/(2*myscale))*9));
                main.setOpaloids();
                mainCam.transform.position = new Vector3(0, 15, -10);
            }
            if (controllerRest > 0)
            {
                controllerRest--;
            }
        }
	}

    public void setActive(bool hmm)
    {
        active = hmm;
    }

}
