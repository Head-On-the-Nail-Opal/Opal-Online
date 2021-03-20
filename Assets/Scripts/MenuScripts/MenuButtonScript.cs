using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        }else if(purpose == "personality")
        {
            
        }
        else if (purpose == "items")
        {
            mainCam.transform.position = new Vector3(0, 26, -10);
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
        //print(": "+purpose);
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
            main.startLocalGame();
        }else if(purpose == "setup2")
        {
            main.startMultiplayerGame();
            main.blueController = "keyboard";
        }else if(purpose == "teams")
        {
            main.displayOpal(null, true);
        }
        else if (purpose == "incTeam")
        {
            //print("du hello");
            main.incTeamNum();
        }
        else if (purpose == "decTeam")
        {
            main.decrTeamNum();
        }else if(purpose == "createTeam")
        {
            main.createTeam();
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
            if(purpose == "teams")
            {
                transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 0f, 0f);
            }
            else if (purpose == "personality")
            {
                transform.GetComponent<Text>().color = new Color(1f, 0f, 0f);
                if (Input.GetMouseButtonDown(0))
                    main.setNextPersonality(false);
                else if (Input.GetMouseButtonDown(1))
                    main.setNextPersonality(true);
            }
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
            if (purpose == "teams")
            {
                transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = new Color(0f, 0f, 1f);
            }
            else if(purpose == "personality")
            {
                transform.GetComponent<Text>().color = new Color(1f, 1f, 1f);
            }
        }
    }

}
