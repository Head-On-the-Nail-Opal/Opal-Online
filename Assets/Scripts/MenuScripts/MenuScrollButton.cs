using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScrollButton : MonoBehaviour {
    public string dir;
    public int distance;
    public string setMenuState;
    public int downTime;
    private GameObject mainCam;
    private MainMenuScript main;
    // Use this for initialization
    void Start () {
        mainCam = GameObject.Find("Main Camera");
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        downTime = 50;
    }
	
	// Update is called once per frame
	void Update () {
        if (main.menuState == "Draft" && downTime < 50)
        {
            downTime++;
        }
    }

    private void OnMouseOver()
    {
        if(main.menuState == "Draft" && downTime < 50)
        {
            downTime++;
            return;
        }
        if (!main.checkController(main.currentTeam, "keyboard"))
        {
            return;
        }
        if (dir == "right")
        {
            mainCam.transform.position = new Vector3(mainCam.transform.position.x + distance, 15, -10);
            main.setPlateActive(true);
            main.menuState = setMenuState;
            main.currentController = "Keyboard";
            downTime = 0;

        }
        else if (dir == "left")
        {
            main.setPlateActive(false);
            mainCam.transform.position = new Vector3(mainCam.transform.position.x - distance, 15, -10);
            main.menuState = setMenuState;
            main.currentController = "Keyboard";
        }
    }
}
