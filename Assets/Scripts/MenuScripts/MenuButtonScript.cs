using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonScript : MonoBehaviour {
    public Vector3 target;
    public string purpose;
    //private Material unpressed;
    //private Material pressed;
    //private Material hover;
    public Sprite unpressedS;
    public Sprite pressedS;
    public string setMenuState;
    private GameObject mainCam;
    private MainMenuScript main;
    private SpriteRenderer sR;
    //private bool hovering = false;


	// Use this for initialization
	void Start () {
        mainCam = GameObject.Find("Main Camera");
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        sR = GetComponent<SpriteRenderer>();
        
    }
	
	// Update is called once per frame
	void Update () {
     
    }

    private void OnMouseDown()
    {
        if (purpose == "iteam")
        {
            main.updateTeam(1);
        }
        else if (purpose == "dteam")
        {
            main.updateTeam(-1);
        }
        else if (purpose == "iopal")
        {
            main.updateNumOpals(1);
        }
        else if (purpose == "dopal")
        {
            main.updateNumOpals(-1);
        }
        else if (purpose == "cnext" || purpose == "cprev")
        {
            main.cycleChoose();
        }
        else if (purpose == "anext")
        {
            main.cycleDuplicate(1);
        }
        else if (purpose == "aprev")
        {
            main.cycleDuplicate(-1);
        }
        else if (purpose == "nextPage")
        {
            main.nextPage(1);
        }
        else if (purpose == "lastPage")
        {
            main.nextPage(-1);
        }
        else if (purpose == "teams")
        {
            mainCam.transform.position = target;
        }
        //rend.material = pressed;
        if (pressedS != null)
        {
            sR.sprite = pressedS;
        }
        else
        {
            sR.color = new Color(0.1f, 0.1f, 0.1f);
        }
    }

    private void OnMouseUp()
    {
        if (purpose == "")
        {
            mainCam.transform.position = target;
            main.menuState = setMenuState;
        }
        else if (purpose == "quit")
        {
            Application.Quit();
        }else if (purpose == "setup")
        {
            mainCam.transform.position = target;
            main.menuState = setMenuState;
            main.setTeamDisplays();
        }else if(purpose == "setup2")
        {
            mainCam.transform.position = target;
            main.doMultiplayerSettings();
            main.menuState = setMenuState;
            main.setTeamDisplays();
            main.blueController = "keyboard";
        }
        if (!main.checkController(main.currentTeam, "keyboard"))
        {
            return;
        }
        if(purpose == "add")
        {
                main.addCurrentOpal();
        }else if(purpose == "clear")
        {
                main.clear();
        }else if (purpose == "start")
        {
            main.startGame();
        }
        else if(purpose == "opletcycle")
        {
            mainCam.transform.position = target;
            main.setCurrentOplet(main.getCurrentOplet()+1);
            main.setOpaloids();
        }
        else if (purpose == "undo")
        {
            main.clear();
            mainCam.transform.position = target;
            main.menuState = setMenuState;
        }else if(purpose == "main")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        //rend.material = unpressed;
        if (unpressedS != null)
        {
            sR.sprite = unpressedS;
        }
        main.menuState = setMenuState;
    }

    private void OnMouseOver()
    {
        //rend.material = hover;
        if (pressedS != null)
        {
            sR.sprite = pressedS;
        }
        else
        {
            sR.color = new Color(1f, 0.2f, 1f);
        }
    }

    private void OnMouseExit()
    {
        //rend.material = unpressed;
        if(unpressedS != null)
        {
            sR.sprite = unpressedS;
        }
        else
        {
            sR.color = new Color(1f, 1f, 1f);
        }
    }

}
