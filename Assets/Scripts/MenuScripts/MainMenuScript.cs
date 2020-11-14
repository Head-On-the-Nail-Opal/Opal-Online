using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {
    private OpalScript[] allOpals;
    private List<OpalScript> redTeam = new List<OpalScript>();
    private List<OpalScript> blueTeam = new List<OpalScript>();
    private List<OpalScript> greenTeam = new List<OpalScript>();
    private List<OpalScript> orangeTeam = new List<OpalScript>();
    private List<PlateScript> displayOpals = new List<PlateScript>();
    private List<List<OpalScript>> teams = new List<List<OpalScript>>();
    private GlobalScript glob;
    public string currentTeam;
    public string blueController;
    public string redController;
    public string greenController;
    public string orangeController;
    public Text currentText;
    public Text teamText;
    public Text chooseText;
    public Text opalCount;
    public Text dupeText;
    public MenuButtonScript startButton;
    public string currentController;
    private GameObject mainCam;
    public string menuState;
    public GameObject TargetInfo;
    public PlateSelector ps;
    public Text controllerSelect;
    public Transform opletButton;
    private bool full = false;
    private int currentOplet = 0;
    public OpalDisplay mainDisplay;
    public OpalDisplay opletDisplay;
    public OpalDisplay selectionDisplay;
    private TeamDisplay teamDisplay;
    private bool opletBool = false;
    private int numTeams = 2;
    private int numOpals = 4;
    private bool choose = true; 
    public List<TeamDisplay> displayPlates = new List<TeamDisplay>();
    private bool startDraft = true;
    private int dupes = 0;
    private MultiplayerManager mm;
    private bool mult = false;
    private int currentOpalPage = 0;
    private int maxOpalPage = 0;
    public InputField opalSearch;
    private GameObject nPage;
    private GameObject lPage;


    // Use this for initialization
    void Start () {
        currentController = "";
        mainCam = GameObject.Find("Main Camera");
        allOpals = Resources.LoadAll<OpalScript>("Prefabs/Opals");
        PlateScript platePrefab = Resources.Load<PlateScript>("Prefabs/OpalPlate");
        teamDisplay = Resources.Load<TeamDisplay>("Prefabs/TeamDisplay");
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        mm = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        lPage = GameObject.Find("LastPage");
        nPage = GameObject.Find("NextPage");
        lPage.SetActive(false);
        float x = 20;
        float y = 19;
        int i = 0;
        int offset = 0;
        maxOpalPage = Mathf.CeilToInt(allOpals.Length/25f);
        for(int j = 0 + offset; j < 25+offset; j++)
        {
            PlateScript tempP = Instantiate<PlateScript>(platePrefab);
            tempP.setPlate(allOpals[j], x, y);
            x += 1.6f;
            i++;
            if(i == 5)
            {
                x = 20;
                y -= 1.6f;
                i = 0;
            }
            displayOpals.Add(tempP);
        }
        startButton.transform.position = new Vector3(-100, -100, -100);
        TargetInfo.transform.position = new Vector3(0.2f, 18, -1);
        mainDisplay.clearInfo();
        menuState = "Main";
        foreach (string s in Input.GetJoystickNames())
        {
            print(s);
        }

        //TargetInfo.transform.position = new Vector3(-100, -100, -100);
        currentTeam = "blue";
        currentText.text = "Current Player: Blue";
        currentText.color = Color.blue;
    }
	
	// Update is called once per frame
	void Update () {
        int num = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            num++;
        }
        //print(num);
        Vector3 mousePos = Input.mousePosition;
        if(menuState == "Draft" && startDraft)
        {
            if (!choose)
            {
                populateTeams();
            }
        }

        if (Input.GetButtonDown("button 0"))
        {
            //print("A 1");
            if (menuState == "Controls")
            {
                if (currentTeam == "blue")
                {
                    if (numTeams > 1)
                    {
                        currentTeam = "red";
                        blueController = "joystick 1";
                        controllerSelect.text = "Red Player\nPress A or SPACE";
                    }
                    else
                    {
                        blueController = "joystick 1";
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                }
                else if (currentTeam == "red")
                {
                    redController = "joystick 1";
                    if (numTeams == 2)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "green";
                        controllerSelect.text = "Green Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "green")
                {
                    greenController = "joystick 1";
                    if (numTeams == 3)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "orange";
                        controllerSelect.text = "Orange Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "orange")
                {
                    orangeController = "joystick 1";
                    currentTeam = "blue";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    menuState = "Draft";

                }
            }
            if (menuState == "Main")
            {
                menuState = "GameChanger";
                mainCam.transform.position = new Vector3(-25, 0, -10);
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
            if(menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1" || currentTeam == "green" && greenController == "joystick 1" || currentTeam == "orange" && orangeController == "joystick 1")
                    addCurrentOpal();
            }
            if(menuState == "Play")
            {
                startGame();
                currentTeam = "blue";
                controllerSelect.text = "Blue Player\nPress A or SPACE";
            }
        }
        if (Input.GetButtonDown("button 1"))
        {
            //print("B");
            if (menuState == "Main")
            {
                Application.Quit();
            }
            if (menuState == "Draft")
            {
                //if (currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1")
                   // clear();
            }
        }
        else if (Input.GetAxis("dpadRight") > 0)
        {
            //print("dpadRight");
            if (menuState == "Draft")
            {
                if ((currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1" || currentTeam == "green" && greenController == "joystick 1" || currentTeam == "orange" && orangeController == "joystick 1"))
                {
                    menuState = "Pick";
                    currentController = "Joystick1";
                    mainCam.transform.position = new Vector3(20, 15, -10);
                    setPlateActive(false);
                }
            }
        }
        else if (Input.GetAxis("dpadRight") < 0)
        {
            //print("dpadLeft");
            if (menuState == "Pick")
            {
                if ((currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1" || currentTeam == "green" && greenController == "joystick 1" || currentTeam == "orange" && orangeController == "joystick 1"))
                {
                    currentController = "Joystick1";
                    menuState = "Draft";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    setPlateActive(true);
                }
            }
        }
        else if(Input.GetAxis("dpadUp") > 0)
        {
            if (menuState == "Draft" && opletBool)
            {
                if ((currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1" || currentTeam == "green" && greenController == "joystick 1" || currentTeam == "orange" && orangeController == "joystick 1"))
                {
                    menuState = "Oplet";
                    setOpaloids();
                    mainCam.transform.position = new Vector3(0, 26, -10);
                }
            }
        }
        else if (Input.GetAxis("dpadUp") < 0)
        {

        }


        if (Input.GetButtonDown("button 0 2"))
        {
            print("A 2");
            if (menuState == "Controls")
            {
                if (currentTeam == "blue")
                {
                    if (numTeams > 1)
                    {
                        currentTeam = "red";
                        blueController = "joystick 1";
                        controllerSelect.text = "Red Player\nPress A or SPACE";
                    }
                    else
                    {
                        blueController = "joystick 1";
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                }
                else if (currentTeam == "red")
                {
                    redController = "joystick 2";
                    if (numTeams == 2)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "green";
                        controllerSelect.text = "Green Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "green")
                {
                    greenController = "joystick 2";
                    if (numTeams == 3)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "orange";
                        controllerSelect.text = "Orange Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "orange")
                {
                    orangeController = "joystick 2";
                    currentTeam = "blue";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    menuState = "Draft";

                }
            }
            if (menuState == "Main")
            {
                menuState = "GameChanger";
                mainCam.transform.position = new Vector3(-25, 0, -10);
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2" || currentTeam == "green" && greenController == "joystick 2" || currentTeam == "orange" && orangeController == "joystick 2")
                    addCurrentOpal();
            }
            if (menuState == "Play")
            {
                startGame();
            }
        }
        if (Input.GetButtonDown("button 1 2"))
        {
            //print("B");
            if (menuState == "Main")
            {
                Application.Quit();
            }
            if (menuState == "Draft")
            {
               // if (currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2")
                  //  clear();
            }
        }
        else if (Input.GetAxis("dpadRight 2") > 0)
        {
            //print("dpadRight");
            if (menuState == "Draft")
            {
                if ((currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2" || currentTeam == "green" && greenController == "joystick 2" || currentTeam == "orange" && orangeController == "joystick 2"))
                {
                    menuState = "Pick";
                    currentController = "Joystick2";
                    mainCam.transform.position = new Vector3(20, 15, -10);
                    setPlateActive(false);
                }
            }
        }
        else if (Input.GetAxis("dpadRight 2") < 0)
        {
            //print("dpadLeft");
            if (menuState == "Pick")
            {
                if ((currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2" || currentTeam == "green" && greenController == "joystick 2" || currentTeam == "orange" && orangeController == "joystick 2"))
                {
                    currentController = "Joystick2";
                    menuState = "Draft";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    setPlateActive(true);
                }
            }
        }
        else if (Input.GetButtonDown("ControllerStart") || Input.GetButtonDown("ControllerStart 2"))
        {
            clear();
            menuState = "Main";
            mainCam.transform.position = new Vector3(0, 0, -10);
        }
        else if (Input.GetAxis("dpadUp 2") > 0)
        {
            if (menuState == "Draft" && opletBool)
            {
                if ((currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2" || currentTeam == "green" && greenController == "joystick 2" || currentTeam == "orange" && orangeController == "joystick 2"))
                {
                    menuState = "Oplet";
                    setOpaloids();
                    mainCam.transform.position = new Vector3(0, 26, -10);
                }
            }
        }
        else if (Input.GetAxis("dpadUp 2") < 0)
        {
        }


        if (Input.GetButtonDown("button 0 3"))
        {
            print("A 3");
            if (menuState == "Controls")
            {
                if (currentTeam == "blue")
                {
                    if (numTeams > 1)
                    {
                        currentTeam = "red";
                        blueController = "joystick 1";
                        controllerSelect.text = "Red Player\nPress A or SPACE";
                    }
                    else
                    {
                        blueController = "joystick 1";
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                }
                else if (currentTeam == "red")
                {
                    redController = "joystick 3";
                    if (numTeams == 2)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "green";
                        controllerSelect.text = "Green Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "green")
                {
                    greenController = "joystick 3";
                    if (numTeams == 3)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "orange";
                        controllerSelect.text = "Orange Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "orange")
                {
                    orangeController = "joystick 3";
                    currentTeam = "blue";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    menuState = "Draft";

                }
            }
            if (menuState == "Main")
            {
                menuState = "GameChanger";
                mainCam.transform.position = new Vector3(-25, 0, -10);
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 3" || currentTeam == "red" && redController == "joystick 3" || currentTeam == "green" && greenController == "joystick 3" || currentTeam == "orange" && orangeController == "joystick 3")
                    addCurrentOpal();
            }
            if (menuState == "Play")
            {
                startGame();
            }
        }
        if (Input.GetButtonDown("button 1 3"))
        {
            //print("B");
            if (menuState == "Main")
            {
                Application.Quit();
            }
            if (menuState == "Draft")
            {
                // if (currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2")
                //  clear();
            }
        }
        else if (Input.GetAxis("dpadRight 3") > 0)
        {
            //print("dpadRight");
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 3" || currentTeam == "red" && redController == "joystick 3" || currentTeam == "green" && greenController == "joystick 3" || currentTeam == "orange" && orangeController == "joystick 3")
                {
                    menuState = "Pick";
                    currentController = "Joystick3";
                    mainCam.transform.position = new Vector3(20, 15, -10);
                    setPlateActive(false);
                }
            }
        }
        else if (Input.GetAxis("dpadRight 3") < 0)
        {
            //print("dpadLeft");
            if (menuState == "Pick")
            {
                if ((currentTeam == "blue" && blueController == "joystick 3" || currentTeam == "red" && redController == "joystick 3" || currentTeam == "green" && greenController == "joystick 3" || currentTeam == "orange" && orangeController == "joystick 3"))
                {
                    currentController = "Joystick3";
                    menuState = "Draft";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    setPlateActive(true);
                }
            }
        }
        else if (Input.GetButtonDown("ControllerStart 3"))
        {
            clear();
            menuState = "Main";
            mainCam.transform.position = new Vector3(0, 0, -10);
        }
        else if (Input.GetAxis("dpadUp 3") > 0)
        {

        }

        if (Input.GetButtonDown("button 0 4"))
        {
            print("A 4");
            if (menuState == "Controls")
            {
                if (currentTeam == "blue")
                {
                    if (numTeams > 1)
                    {
                        currentTeam = "red";
                        blueController = "joystick 1";
                        controllerSelect.text = "Red Player\nPress A or SPACE";
                    }
                    else
                    {
                        blueController = "joystick 1";
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                }
                else if (currentTeam == "red")
                {
                    redController = "joystick 4";
                    if (numTeams == 2)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "green";
                        controllerSelect.text = "Green Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "green")
                {
                    greenController = "joystick 4";
                    if (numTeams == 3)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "orange";
                        controllerSelect.text = "Orange Player\nPress A or SPACE";
                    }
                }
                else if (currentTeam == "orange")
                {
                    orangeController = "joystick 4";
                    currentTeam = "blue";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    menuState = "Draft";

                }
            }
            if (menuState == "Main")
            {
                menuState = "GameChanger";
                mainCam.transform.position = new Vector3(-25, 0, -10);
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 4" || currentTeam == "red" && redController == "joystick 4" || currentTeam == "green" && greenController == "joystick 4" || currentTeam == "orange" && orangeController == "joystick 4")
                    addCurrentOpal();
            }
            if (menuState == "Play")
            {
                startGame();
            }
        }
        if (Input.GetButtonDown("button 1 4"))
        {
            //print("B");
            if (menuState == "Main")
            {
                //Application.Quit();
            }
            if (menuState == "Draft")
            {
                // if (currentTeam == "blue" && blueController == "joystick 2" || currentTeam == "red" && redController == "joystick 2")
                //  clear();
            }
        }
        else if (Input.GetAxis("dpadRight 4") > 0) //what
        {
            //print("dpadRight");
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 4" || currentTeam == "red" && redController == "joystick 4" || currentTeam == "green" && greenController == "joystick 4" || currentTeam == "orange" && orangeController == "joystick 4")
                {
                    menuState = "Pick";
                    currentController = "Joystick4";
                    mainCam.transform.position = new Vector3(20, 15, -10);
                    setPlateActive(false);
                }
            }
        }
        else if (Input.GetAxis("dpadRight 4") < 0) //what
        {
            //print("dpadLeft");
            if (menuState == "Pick")
            {
                if ((currentTeam == "blue" && blueController == "joystick 4" || currentTeam == "red" && redController == "joystick 4" || currentTeam == "green" && greenController == "joystick 4" || currentTeam == "orange" && orangeController == "joystick 4"))
                {
                    currentController = "Joystick4";
                    menuState = "Draft";
                    mainCam.transform.position = new Vector3(0, 15, -10);
                    setPlateActive(true);
                }
            }
        }
        else if (Input.GetButtonDown("ControllerStart 4"))
        {
            clear();
            menuState = "Main";
            mainCam.transform.position = new Vector3(0, 0, -10);
        }
        else if (Input.GetAxis("dpadUp 4") > 0)
        {
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (menuState == "Controls")
            {
                if (currentTeam == "blue")
                {
                    blueController = "keyboard";
                    if (numTeams > 1)
                    {
                        currentTeam = "red";
                        
                        controllerSelect.text = "Red Player\nPress A or SPACE";
                    }
                    else
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(20, 15, -10);
                        menuState = "Draft";
                    }
                }
                else if (currentTeam == "red")
                {
                    redController = "keyboard";
                    if (numTeams == 2)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(20, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "green";
                        controllerSelect.text = "Green Player\nPress A or SPACE";
                    }
                }else if(currentTeam == "green")
                {
                    greenController = "keyboard";
                    if (numTeams == 3)
                    {
                        currentTeam = "blue";
                        mainCam.transform.position = new Vector3(20, 15, -10);
                        menuState = "Draft";
                    }
                    else
                    {
                        currentTeam = "orange";
                        controllerSelect.text = "Orange Player\nPress A or SPACE";
                    }
                }
                else if(currentTeam == "orange")
                {
                    orangeController = "keyboard";
                    currentTeam = "blue";
                    mainCam.transform.position = new Vector3(20, 15, -10);
                    menuState = "Draft";
                    
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            goBack();
        }
    }

    public void addCurrentOpal()
    {
        if(selectionDisplay.getCurrentOpal() == null)
        {
            return;
        }
        selectionDisplay.getCurrentOpal().setOpal(null);
        if(selectionDisplay.getCurrentOpal() != null && ((!checkRepeats(blueTeam, selectionDisplay.getCurrentOpal()) && !checkRepeats(redTeam, selectionDisplay.getCurrentOpal())) && !checkRepeats(greenTeam, selectionDisplay.getCurrentOpal()) && !checkRepeats(orangeTeam, selectionDisplay.getCurrentOpal()) || (dupes == 1 && !checkRepeats(getTeam(currentTeam), selectionDisplay.getCurrentOpal()))) && !full)
        {
            if (dupes == 0)
            {
                foreach (PlateScript p in displayOpals)
                {
                    if (p.getOpal() != null && p.getOpal().getMyName() == selectionDisplay.getCurrentOpal().getMyName())
                    {
                        p.setTeam(currentTeam);
                    }
                }
            }
            if (currentTeam == "blue")
            {
                displayPlates[0].addOpal(selectionDisplay.getCurrentOpal());
                blueTeam.Add(selectionDisplay.getCurrentOpal());
                updateCurrentTeam();
            }
            else if (currentTeam == "red")
            {
                displayPlates[1].addOpal(selectionDisplay.getCurrentOpal());
                redTeam.Add(selectionDisplay.getCurrentOpal());
                updateCurrentTeam();
            }
            else if (currentTeam == "green")
            {
                displayPlates[2].addOpal(selectionDisplay.getCurrentOpal());
                greenTeam.Add(selectionDisplay.getCurrentOpal());
                updateCurrentTeam();
            }
            else if (currentTeam == "orange")
            {
                displayPlates[3].addOpal(selectionDisplay.getCurrentOpal());
                orangeTeam.Add(selectionDisplay.getCurrentOpal());
                updateCurrentTeam();
            }
        }
        selectionDisplay.setCurrentOpal(null);
        if (redTeam.Count + blueTeam.Count + greenTeam.Count + orangeTeam.Count == numOpals*numTeams)
        {
            full = true;
            startButton.transform.position = new Vector3(24, 17f, -3);
            menuState = "Play";
        }
    }

    private List<OpalScript> getTeam(string teamName)
    {
        if (teamName == "blue")
            return blueTeam;
        if (teamName == "red")
            return redTeam;
        if (teamName == "green")
            return greenTeam;
        if (teamName == "orange")
            return orangeTeam;
        return null;
    }

    private void populateTeams()
    {
        while(!full)
        {
            OpalScript temp = Instantiate<OpalScript>(allOpals[Random.Range(0, allOpals.Length)]);
            temp.setVariant(""+Random.Range(0, 2));
            mainDisplay.setCurrentOpal(temp);
            addCurrentOpal();
        }
    }

    private bool checkRepeats(List<OpalScript> opals, OpalScript check)
    {
        if(dupes == 2)
        {
            return false;
        }
        foreach(OpalScript o in opals)
        {
            if(o.getMyName() == check.getMyName())
            {
                return true;
            }
        }
        return false;
    }

    public void clear()
    {
        menuState = "Draft";
        foreach(TeamDisplay t in displayPlates)
        {
            t.clear();
        }
        blueTeam.Clear();
        redTeam.Clear();
        orangeTeam.Clear();
        greenTeam.Clear();
        currentTeam = "blue";
        currentText.text = "Current Player: Blue";
        currentText.color = Color.blue;
        full = false;
        startButton.transform.position = new Vector3(-100, -100, -100);
        mainDisplay.setCurrentOpal(null);
        foreach (PlateScript p in displayOpals)
        {
            p.setTeam("reset");
        }
        //DestroyImmediate(currentOpalInstance.gameObject);
        //currentOpalInstance = null;
    }

    public void clearSingleOpal(OpalScript o)
    {
        int i = 0;
        foreach(OpalScript opal in blueTeam)
        {
            if(o.getMyName() == opal.getMyName())
            {
                break;
            }
            i++;
        }
        foreach (PlateScript p in displayOpals)
        {
            if(p.getOpal() != null && o.getMyName() == p.getOpal().getMyName())
                p.setTeam("reset");
        }
        blueTeam.RemoveAt(i);
        full = false;
        startButton.transform.position = new Vector3(-100, -100, -100);
    }

    public void searchForOpal(string text)
    {
        if (opalSearch.text != "")
        {
            nPage.SetActive(false);
            lPage.SetActive(false);
            List<OpalScript> searched = new List<OpalScript>();
            //print(opalSearch.text);
            foreach(OpalScript o in allOpals)
            {
                o.setOpal(null);
                if (o.getMyName() != null && o.getMyName().ToLower().Contains(opalSearch.text.ToLower()))
                {
                    searched.Add(o);
                }
            }

            foreach (PlateScript p in displayOpals)
            {
                p.clearPlate();
            }
            for(int i = 0; i < searched.Count; i++)
            {
                if(i > displayOpals.Count - 1)
                {
                    break;
                }
                displayOpals[i].setPlate(searched[i]);
            }
        }
        else
        {
            int i = 0;
            foreach (PlateScript p in displayOpals)
            {
                if (i + currentOpalPage * 25 >= allOpals.Length)
                {
                    p.clearPlate();
                }
                else
                {
                    p.setPlate(allOpals[i + currentOpalPage * 25]);
                }
                i++;
            }
            nPage.SetActive(true);
            lPage.SetActive(true);
            if (currentOpalPage == 0)
            {
                lPage.SetActive(false);
            }
            else if (currentOpalPage == maxOpalPage - 1)
            {
                nPage.SetActive(false);
            }
        }
    }

    public void startGame()
    {
        glob.setTeams(redTeam, blueTeam, greenTeam, orangeTeam);
        glob.setControllers(blueController, redController, greenController, orangeController);
        //doLoad();
        if (mult)
        {
            glob.setMult(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Connecting", UnityEngine.SceneManagement.LoadSceneMode.Single); //temporary
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        //UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void doLoad()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void goBack()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("World", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public OpalScript findOpal(float x, float y)
    {
        if (allOpals.Length < x + y)
            return null;
        return allOpals[(int)(x+y)];
    }

    public void setPlateActive(bool hmm)
    {
        //ps.setActive(hmm);
    }

    public void setCurrentOplet(int num)
    {
        currentOplet= num;
    }

    public void cycleChoose()
    {
        choose = !choose;
        if (choose)
        {
            chooseText.text = "Choose Type:\nDraft";
        }
        else
        {
            chooseText.text = "Choose Type:\nRandom";
        }
    }

    public void cycleDuplicate(int add)
    {
        dupes += add;
        if(dupes > 2)
        {
            dupes = 0;
        }else if(dupes < 0)
        {
            dupes = 2;
        }
        if (dupes == 0)
        {
            dupeText.text = "Allow Duplicates?\nNo Duplicates";
        }
        else if(dupes == 1)
        {
            dupeText.text = "Allow Duplicates?\nNo Team Duplicates";
        }
        else if(dupes == 2)
        {
            dupeText.text = "Allow Duplicates?\nNo Limits";
        }
    }

    public int getCurrentOplet()
    {
        return currentOplet;
    }

    public void setOpaloids()
    {
        return;
        if(mainDisplay.getCurrentOpalInstance().getOplets() != null)
        {
            opletBool = true;
            opletButton.transform.position = new Vector3(opletButton.transform.position.x, opletButton.transform.position.y, -1);
            if(currentOplet >= mainDisplay.getCurrentOpalInstance().getOplets().Count)
            {
                currentOplet = 0;
            }
            //mainDisplay.getCurrentOpalInstance().setOpal(null)
            OpalScript temp = Instantiate<OpalScript>(mainDisplay.getCurrentOpalInstance().getOplets()[currentOplet]);
            temp.setOpal(null);
            //print(temp.getMyName());
            opletDisplay.setCurrentOpalByName(temp.getMyName());
        }
        else
        {
            opletBool = false;
            opletButton.transform.position = new Vector3(opletButton.transform.position.x, opletButton.transform.position.y, 1);
        }
    }

    public bool checkController(string player, string controller)
    {
        if(currentTeam == "blue" && blueController == controller)
        {
            if(player == "blue")
            {
                return true;
            }
            return false;
        }
        if (currentTeam == "red" && redController == controller)
        {
            if (player == "red")
            {
                return true;
            }
            return false;
        }
        if (currentTeam == "green" && greenController == controller)
        {
            if (player == "green")
            {
                return true;
            }
            return false;
        }
        if (currentTeam == "orange" && orangeController == controller)
        {
            if (player == "orange")
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void updateCurrentTeam()
    {
        if (currentTeam == "blue")
        {
            if (numTeams > 1)
            {
                currentTeam = "red";
                currentText.text = "Current Player: Red";
                currentText.color = Color.red;
            }
        }else if(currentTeam == "red")
        {
            if(numTeams > 2)
            {
                currentTeam = "green";
                currentText.text = "Current Player: Green";
                currentText.color = Color.green;
            }
            else
            {
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
        }else if(currentTeam == "green")
        {
            if (numTeams > 3)
            {
                currentTeam = "orange";
                currentText.text = "Current Player: Orange";
                currentText.color = new Color(1, 0.5f, 0);
            }
            else
            {
                currentTeam = "blue";
                currentText.text = "Current Player: Blue";
                currentText.color = Color.blue;
            }
        }
        else if(currentTeam == "orange")
        {
            currentTeam = "blue";
            currentText.text = "Current Player: Blue";
            currentText.color = Color.blue;
        }
    }

    public void updateTeam(int num)
    {
        numTeams += num;
        if (numTeams > 4)
            numTeams = 4;
        else if (numTeams < 2)
            numTeams = 2;
        teamText.text = "Teams: " + numTeams;
    }

    public void updateNumOpals(int num)
    {
        numOpals += num;
        if (numOpals > 8)
            numOpals = 8;
        else if (numOpals < 1)
            numOpals = 1;
        opalCount.text = "Opals per Team:\n" + numOpals;
    }

    public void doMultiplayerSettings()
    {
        numTeams = 1;
        numOpals = 4;
        mult = true;
    }

    public void setTeamDisplays()
    {
        //print("duh hello");
        TeamDisplay teamTemp = Instantiate<TeamDisplay>(teamDisplay);
        teamTemp.setUp("blue", numOpals);
        teamTemp.transform.localPosition = new Vector3(11.7f,19.4f, -4f);
        displayPlates.Add(teamTemp);

        if (numTeams >= 2)
        {
            TeamDisplay teamTemp2 = Instantiate<TeamDisplay>(teamDisplay);
            teamTemp2.setUp("red", numOpals);
            teamTemp2.transform.position = new Vector3(-7.4f, 19f, -0.6f);
            displayPlates.Add(teamTemp2);
        }


        if (numTeams >= 3)
        {
            TeamDisplay teamTemp3 = Instantiate<TeamDisplay>(teamDisplay);
            teamTemp3.setUp("green", numOpals);
            teamTemp3.transform.localPosition = new Vector3(-6.4f, 19f, -0.6f);
            displayPlates.Add(teamTemp3);


        }
        if(numTeams >= 4)
        {
            TeamDisplay teamTemp4 = Instantiate<TeamDisplay>(teamDisplay);
            teamTemp4.setUp("orange", numOpals);
            teamTemp4.transform.localPosition = new Vector3(-5.4f, 19f, -0.6f);
            displayPlates.Add(teamTemp4);

        }
    }

    public void nextPage(int inc)
    {
        if(currentOpalPage + inc < maxOpalPage && currentOpalPage + inc >= 0)
        {
            nPage.SetActive(true);
            lPage.SetActive(true);
            if (currentOpalPage + inc == 0)
            {
                lPage.SetActive(false);
            }else if (currentOpalPage + inc == maxOpalPage-1)
            {
                nPage.SetActive(false);
            }
            int i = 0;
            currentOpalPage += inc;
            foreach (PlateScript p in displayOpals)
            {
                if (i + currentOpalPage * 25 >= allOpals.Length)
                {
                    p.clearPlate();
                }
                else
                {
                    p.setPlate(allOpals[i + currentOpalPage * 25]);
                }
                i++;
            }
        }
    }

    public void displayOpal(OpalScript o)
    {
        selectionDisplay.setCurrentOpal(o);
    }
}
