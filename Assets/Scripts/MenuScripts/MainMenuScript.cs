using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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
    private List<OpalTeam> displayTeams = new List<OpalTeam>();
    private List<OpalScript> myOpals = new List<OpalScript>();
    private List<List<OpalScript>> activeTeams = new List<List<OpalScript>>();
    private GlobalScript glob;
    public string currentTeam;
    public string blueController;
    public string redController;
    public string greenController;
    public string orangeController;
    public Text currentText;
    public Text teamText;
    public Text numTeamsText;
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
    private GameObject opalPlate;
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
    public Text currentTeamNumOpals;
    private List<PlateScript> teamEditor = new List<PlateScript>();
    public GameObject teamScreen;
    private PlateScript platePrefab;
    private PlateScript currentTeamPlate = null;
    public OpalDisplay teamOpalDisplay;
    public MenuButtonScript createTeamButton;
    public MenuButtonScript addNewTeamButton;
    private int currentEditorTeam = -1;
    private TextAsset save;
    public Text personalityTracker;
    private int personalityNum = 0;
    private ItemLabel itemLabelPrefab;
    public ItemDescriptions iD;
    public Text description;
    public Text charmLabel;
    public Text visualLabel;
    private int currentVisual = -1;
    private OpalScript viewedOpal = null;
    private List<string> bannedCharms = new List<string>();
    private int waiting = -1;
    public Text chooseTeamsText;
    private List<string> controls = new List<string>();

    public MenuButtonScript toggleAI;

    private List<string> personalities = new List<string>();
    private int controlTracker = 0;


    // Use this for initialization

    void Start() {
        currentController = "";
        mainCam = GameObject.Find("Main Camera");
        allOpals = Resources.LoadAll<OpalScript>("Prefabs/Opals");
        platePrefab = Resources.Load<PlateScript>("Prefabs/OpalPlate");
        teamDisplay = Resources.Load<TeamDisplay>("Prefabs/TeamDisplay");
        itemLabelPrefab = Resources.Load<ItemLabel>("Prefabs/ItemThing");
        opalPlate = Resources.Load<GameObject>("Prefabs/OpalPlate2");
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        mm = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>();
        lPage = GameObject.Find("LastPage");
        nPage = GameObject.Find("NextPage");
        save = Resources.Load<TextAsset>("Assets/save.txt");
        createTeamButton.gameObject.SetActive(false);
        lPage.SetActive(false);
        populateOpalScreen();
        setUpItems();
        setupTeamDisplay();
        startButton.transform.position = new Vector3(-100, -100, -100);
        TargetInfo.transform.position = new Vector3(0.2f, 18, -1);
        mainDisplay.clearInfo();
        menuState = "Main";
        foreach (string s in Input.GetJoystickNames())
        {
            print(s);
        }

        for (int i = 0; i < 4; i++)
        {
            controls.Add("keyboard");
        }

        //TargetInfo.transform.position = new Vector3(-100, -100, -100);
        currentTeam = "blue";
        currentText.text = "Current Player: Blue";
        currentText.color = Color.blue;
        loadPersonalities();
        loadData();
        if(glob.getFinishedGame() == true)
        {
            addNewOpal();
            glob.setFinishedGame(false); 
        }
        setUpMenuBackground();
        //loadTeams();
    }

    // Update is called once per frame
    void Update() {
        int num = 0;
        foreach (string s in Input.GetJoystickNames())
        {
            num++;
        }
        //print(num);
        Vector3 mousePos = Input.mousePosition;
        if (menuState == "Draft" && startDraft)
        {
            if (!choose)
            {
                populateTeams();
            }
        }

        updateTeamsText();
        if(waiting != -1)
        {
            if (waiting == 0)
            {
                if (activeTeams.Count > 0)
                {
                    doMultiplayerSettings();
                    blueTeam = activeTeams[0];
                    blueController = "keyboard";
                    startGame();
                }
            }
            else if (waiting == 1)
            {
                if (activeTeams.Count >= numTeams)
                {
                    glob.setMult(false);


                    string rCont = controls[0];
                    string bCont = controls[1];
                    string gCont = controls[2];
                    string oCont = controls[3];
                    glob.setControllers(rCont, bCont, gCont, oCont);
                    glob.setNumPlayers(activeTeams.Count);
                    switch (numTeams) {
                        case 2:
                            glob.setTeams(activeTeams[0], activeTeams[1], null, null);
                            glob.setOverloads(calculateTypeOverload(activeTeams[0]), calculateTypeOverload(activeTeams[1]), null, null);
                            break;
                        case 3:
                            glob.setTeams(activeTeams[0], activeTeams[1], activeTeams[2], null);
                            glob.setOverloads(calculateTypeOverload(activeTeams[0]), calculateTypeOverload(activeTeams[1]), calculateTypeOverload(activeTeams[2]), null);
                            break;
                        case 4:
                            glob.setTeams(activeTeams[0], activeTeams[1], activeTeams[2], activeTeams[3]);
                            glob.setOverloads(calculateTypeOverload(activeTeams[0]), calculateTypeOverload(activeTeams[1]), calculateTypeOverload(activeTeams[2]), calculateTypeOverload(activeTeams[3]));
                            break;
                    }

                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
                    //if(rCont == "AI" || bCont == "AI" || oCont == "AI" || gCont == "AI")
                    //{
                    //    UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    //}
                }
            }
            else if (waiting == 2)
            {
                if (activeTeams.Count > 0)
                {
                    glob.setMult(false);
                    glob.setControllers("keyboard", "AI", "keyboard", "keyboard");
                    glob.setNumPlayers(2);

                    List<OpalScript> aiTeam = new List<OpalScript>();
                    for(int i = 0; i < activeTeams[0].Count; i++)
                    {
                        aiTeam.Add(getRandomAIOpal());
                    }

                    glob.setTeams(aiTeam, activeTeams[0], null, null);
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            addNewOpal();
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            wipeOpals();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            refillOpals();
        }
        if (Input.GetKeyDown(KeyCode.Plus))
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene("World", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
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
            if (menuState == "Draft")
            {
                if (currentTeam == "blue" && blueController == "joystick 1" || currentTeam == "red" && redController == "joystick 1" || currentTeam == "green" && greenController == "joystick 1" || currentTeam == "orange" && orangeController == "joystick 1")
                    addCurrentOpal();
            }
            if (menuState == "Play")
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
        else if (Input.GetAxis("dpadUp") > 0)
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
                } else if (currentTeam == "green")
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
                else if (currentTeam == "orange")
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

    private void setUpMenuBackground()
    {
        GameObject tempPrefab = Resources.Load<GameObject>("Prefabs/UIandMenu/TeamSelectionBackground");
        LilOpalBox lilPrefab = Resources.Load<LilOpalBox>("Prefabs/UIandMenu/LilOpalBox");
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject temp = Instantiate<GameObject>(tempPrefab);
                temp.transform.position = new Vector3(-32.5f+2.8f*j, 3.61f - i * 1.4f, -0.5f);
                temp.transform.localScale = new Vector3(4.4f, 4.4f, 0);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            LilOpalBox temp = Instantiate<LilOpalBox>(lilPrefab);
            temp.transform.position = new Vector3(-16.875f - i * 1.5f, 4.2f, -0.51f);
            temp.transform.localScale = new Vector3(5f, 5f, 0);
            temp.setOpal(Resources.Load<OpalScript>("Prefabs/Opals/"+getRandomOpalName()));
        }

        for (int i = 0; i < 3; i++)
        {
            LilOpalBox temp = Instantiate<LilOpalBox>(lilPrefab);
            temp.transform.position = new Vector3(-24.6f, 2f - i*2.5f, -0.51f);
            temp.transform.localScale = new Vector3(5f, 5f, 0);
            temp.setOpal(Resources.Load<OpalScript>("Prefabs/Opals/" + getRandomOpalName()));
        }
    }

    public void addCurrentOpal()
    {
        if (selectionDisplay.getCurrentOpal() == null)
        {
            return;
        }
        selectionDisplay.getCurrentOpal().setOpal(null);
        if (selectionDisplay.getCurrentOpal() != null && ((!checkRepeats(blueTeam, selectionDisplay.getCurrentOpal()) && !checkRepeats(redTeam, selectionDisplay.getCurrentOpal())) && !checkRepeats(greenTeam, selectionDisplay.getCurrentOpal()) && !checkRepeats(orangeTeam, selectionDisplay.getCurrentOpal()) || (dupes == 1 && !checkRepeats(getTeam(currentTeam), selectionDisplay.getCurrentOpal()))) && !full)
        {
            if (dupes == 0 && currentTeamPlate == null)
            {
                foreach (PlateScript p in displayOpals)
                {
                    if (p.getOpal() != null && p.getOpal().getMyName() == selectionDisplay.getCurrentOpal().getMyName())
                    {
                        p.setTeam(currentTeam);
                    }
                }
            }
            if (currentTeamPlate != null)
            {
                //print("du hello");
                foreach (PlateScript p in teamEditor)
                {
                    if (p.getOpal() != null && p.getOpal().getMyName() == selectionDisplay.getCurrentOpal().getMyName())
                    {
                        currentTeamPlate = p;
                        mainCam.transform.position = new Vector3(0, 15, -10);
                        return;
                    }
                }
                currentTeamPlate.setPlate(selectionDisplay.getCurrentOpal());
                currentTeamPlate = null;
                mainCam.transform.position = new Vector3(0, 15, -10);
                displayOpal(selectionDisplay.getCurrentOpal(), true);
                return;
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
        if (redTeam.Count + blueTeam.Count + greenTeam.Count + orangeTeam.Count == numOpals * numTeams)
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
        while (!full)
        {
            OpalScript temp = Instantiate<OpalScript>(allOpals[Random.Range(0, allOpals.Length)]);
            temp.setVariant("" + Random.Range(0, 2));
            mainDisplay.setCurrentOpal(temp);
            addCurrentOpal();
        }
    }

    private bool checkRepeats(List<OpalScript> opals, OpalScript check)
    {
        if (dupes == 2)
        {
            return false;
        }
        foreach (OpalScript o in opals)
        {
            if (o.getMyName() == check.getMyName())
            {
                return true;
            }
        }
        return false;
    }

    public void clear()
    {
        menuState = "Draft";
        foreach (TeamDisplay t in displayPlates)
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
        foreach (OpalScript opal in blueTeam)
        {
            if (o.getMyName() == opal.getMyName())
            {
                break;
            }
            i++;
        }
        foreach (PlateScript p in displayOpals)
        {
            if (p.getOpal() != null && o.getMyName() == p.getOpal().getMyName())
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
            foreach (OpalScript o in allOpals)
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
            for (int i = 0; i < searched.Count; i++)
            {
                if (i > displayOpals.Count - 1)
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
        return allOpals[(int)(x + y)];
    }

    public void setPlateActive(bool hmm)
    {
        //ps.setActive(hmm);
    }

    public void setCurrentOplet(int num)
    {
        currentOplet = num;
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
        if (dupes > 2)
        {
            dupes = 0;
        } else if (dupes < 0)
        {
            dupes = 2;
        }
        if (dupes == 0)
        {
            dupeText.text = "Allow Duplicates?\nNo Duplicates";
        }
        else if (dupes == 1)
        {
            dupeText.text = "Allow Duplicates?\nNo Team Duplicates";
        }
        else if (dupes == 2)
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
        if (mainDisplay.getCurrentOpalInstance().getOplets() != null)
        {
            opletBool = true;
            opletButton.transform.position = new Vector3(opletButton.transform.position.x, opletButton.transform.position.y, -1);
            if (currentOplet >= mainDisplay.getCurrentOpalInstance().getOplets().Count)
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
        if (currentTeam == "blue" && blueController == controller)
        {
            if (player == "blue")
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
        } else if (currentTeam == "red")
        {
            if (numTeams > 2)
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
        } else if (currentTeam == "green")
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
        else if (currentTeam == "orange")
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

    public void setUpItems()
    {
        iD.setUp();
        List<string> items = new List<string>();
        for(int i = 0; i < iD.itemsCount(); i++)
        {
            items.Add(iD.getItem(i));
        }
        float height = 30.5f;
        int k = 0;
        int j = 0;
        foreach(string name in items)
        {
            ItemLabel temp = Instantiate<ItemLabel>(itemLabelPrefab);
            temp.setText(name);
            temp.setMain(this);
            temp.transform.position = new Vector3(-7 + j*4,height,-2);
            temp.transform.localScale /= 2;
            height -= 0.5f;
            k++;
            if(k >= 19)
            {
                j++;
                height = 30.5f;
                k = 0;
            }
        }
    }

    public void readDescription(string name)
    {
        description.text = iD.getDescFromItem(name);
    }

    public void loadPersonalities()
    {
        //personality format: name;health,attack,defense,speed,priority
        personalities.Add("Straight-Edge;0,0,0,0,0");
        personalities.Add("Proud;0,1,-1,0,0");
        personalities.Add("Reserved;0,-1,1,0,0");
        personalities.Add("Risk-Taker;0,2,-2,0,0");
        personalities.Add("Worried;0,-2,2,0,0");
        personalities.Add("Tactical;0,3,0,-1,0");
        personalities.Add("Cautious;0,0,3,-1,0");
        personalities.Add("Relaxed;5,0,0,-1,0");
        personalities.Add("Optimistic;5,-2,0,0,0");
        personalities.Add("Pessimistic;5,0,-2,0,0");
        personalities.Add("Impatient;0,-2,-2,1,0");
        //personalities.Add("Jumpy;0,-1,-,1,0");
    }

    public void setCurrentCharmName(string name)
    {
        OpalScript temp = viewedOpal;
        if (bannedCharms.Contains(name))
        {
            return;
        }
        if (temp == null)
            return;
        if (temp.getCharmsNames().Count > 0 && temp.getCharmsNames()[0] != null)
        {
            bannedCharms.Remove(temp.getCharmsNames()[0]);
        }
        bannedCharms.Add(name);
        temp.replaceCharmName(name);
        mainCam.transform.position = new Vector3(0,15,-10);
        //print(temp.getMyName() + "'s charm is set to " + temp.getCharm());
        setCurrentCharm(temp);
    }

    public void setCurrentCharm(OpalScript op)
    {
        if (op.getCharms()[0] == null)
            charmLabel.text = "None";
        else
            charmLabel.text = "" +  op.getCharmsNames()[0];
    }

    public void setTeamDisplays()
    {
        //print("duh hello");
        TeamDisplay teamTemp = Instantiate<TeamDisplay>(teamDisplay);
        teamTemp.setUp("blue", numOpals);
        teamTemp.transform.localPosition = new Vector3(11.7f, 19.4f, -4f);
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
        if (numTeams >= 4)
        {
            TeamDisplay teamTemp4 = Instantiate<TeamDisplay>(teamDisplay);
            teamTemp4.setUp("orange", numOpals);
            teamTemp4.transform.localPosition = new Vector3(-5.4f, 19f, -0.6f);
            displayPlates.Add(teamTemp4);

        }
    }

    public void nextPage(int inc)
    {
        if (currentOpalPage + inc < maxOpalPage && currentOpalPage + inc >= 0)
        {
            nPage.SetActive(true);
            lPage.SetActive(true);
            if (currentOpalPage + inc == 0)
            {
                lPage.SetActive(false);
            } else if (currentOpalPage + inc == maxOpalPage - 1)
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
        if (o != null)
        {
            o.gameObject.SetActive(true);
            selectionDisplay.setCurrentOpal(o);
        }
        
        if (o != null)
        {
            personalityTracker.text = "" + o.getPersonality() + "\n" + getPersonalityStats(o.getPersonality());
            if (o.getCharmsNames().Count == 0 || o.getCharmsNames()[0] == "None")
                charmLabel.text = "None";
            else
                charmLabel.text = "" + o.getCharmsNames()[0];
        }

    }

    public void displayOpal(OpalScript o, bool team)
    {
        if (o != null)
        {
            teamOpalDisplay.setCurrentOpal(o);
            viewedOpal = o;
            if (team)
            {
                visualLabel.text = o.getMyVisual();
            }
        }
    }

    public void incNumTeams()
    {
        numTeams++;
        if(numTeams > 4)
        {
            numTeams = 2;
        }
        numTeamsText.text = numTeams+"";
    }

    public void decNumTeams()
    {
        numTeams--;
        if (numTeams < 2)
        {
            numTeams = 4;
        }
        numTeamsText.text = numTeams + "";
    }

    public void incTeamNum()
    {
        if (currentTeamNumOpals.text != "8")
        {
            currentTeamNumOpals.text = (int.Parse(currentTeamNumOpals.text) + 1) + "";
            PlateScript temp = Instantiate<PlateScript>(platePrefab, teamScreen.transform);
            temp.transform.localPosition = new Vector3((int.Parse(currentTeamNumOpals.text) - 1) * 1.8f - 4, 10.1f, 3);
            teamEditor.Add(temp);
            temp.setTeamPlate();
            createTeamButton.gameObject.SetActive(false);
        }
    }

    public void decrTeamNum()
    {
        if (currentTeamNumOpals.text != "1")
        {
            currentTeamNumOpals.text = (int.Parse(currentTeamNumOpals.text) - 1) + "";
            teamEditor[int.Parse(currentTeamNumOpals.text)].setPlate(null);
            DestroyImmediate(teamEditor[int.Parse(currentTeamNumOpals.text)].gameObject);
            teamEditor.RemoveAt(int.Parse(currentTeamNumOpals.text));
            int currentPopulation = 0;
            foreach (PlateScript pl in teamEditor)
            {
                if (pl.getOpal() != null)
                {
                    currentPopulation++;
                }
            }
            if (currentPopulation == int.Parse(currentTeamNumOpals.text))
            {
                createTeamButton.gameObject.SetActive(true);
            }
        }
    }

    private void setupTeamDisplay()
    {
        if (teamEditor.Count < 1)
        {
            for (int i = 0; i < int.Parse(currentTeamNumOpals.text); i++)
            {
                PlateScript temp = Instantiate<PlateScript>(platePrefab, teamScreen.transform);
                temp.transform.localPosition = new Vector3(i * 1.8f - 4, 10.1f, 3);
                teamEditor.Add(temp);
                temp.setTeamPlate();
            }
        }
    }

    public int getWaiting()
    {
        return waiting;
    }

    public void chooseOneOpal(PlateScript p)
    {
        currentTeamPlate = p;
        mainCam.transform.position = new Vector3(20, 15, -10);
        int currentPopulation = 0;
        foreach (PlateScript pl in teamEditor)
        {
            if (pl.getOpal() != null)
            {
                currentPopulation++;
            }
        }
        if (currentPopulation == int.Parse(currentTeamNumOpals.text) || (currentPopulation == int.Parse(currentTeamNumOpals.text) - 1 && currentTeamPlate.getOpal() == null))
        {
            createTeamButton.gameObject.SetActive(true);
        }
    }

    public void createTeam()
    {
        List<OpalScript> teamOpals = new List<OpalScript>();
        foreach (PlateScript p in teamEditor)
        {
            teamOpals.Add(p.getOpal());
        }
        createTeam(teamOpals);
    }

    public void createTeam(List<OpalScript> o)
    {
        List<OpalScript> teamOpals = o;
        OpalTeam temp = Instantiate<OpalTeam>(Resources.Load<OpalTeam>("Prefabs/OpalTeam"));
        List<OpalScript> copy = new List<OpalScript>();
        foreach(OpalScript opal in teamOpals)
        {
            OpalScript opalCopy = Instantiate<OpalScript>(opal);
            opalCopy.setOpal(null);
            //opalCopy.setPersonality(opal.getPersonality());
            opalCopy.setDetails(opal);
            //print(opalCopy.getCharm());
            copy.Add(opalCopy);
        }
        if (currentEditorTeam == -1)
        {
            bannedCharms.Clear();
            teams.Add(copy);
            temp.setMain(this, teams.Count - 1);

            if (teams.Count == 1000)
                temp.transform.position = new Vector3(-30, 5 - teams.Count * 1.5f, -1);
            else
                temp.transform.position = new Vector3(-30, 5f - teams.Count * 1.4f, -1);

            if (teams.Count == 1000)
                addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5 - (teams.Count + 1) * 1.5f, addNewTeamButton.transform.position.z);
            else
                addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5f - (teams.Count + 1) * 1.4f, addNewTeamButton.transform.position.z);
            temp.displayTeam(copy);
            displayTeams.Add(temp);
        }
        else
        {
            bannedCharms.Clear();
            teams[currentEditorTeam] = copy;
            temp.setMain(this, currentEditorTeam);
            if (currentEditorTeam == 1000)
                temp.transform.position = new Vector3(-30, 5 - (currentEditorTeam + 1) * 1.5f, -1);
            else
                temp.transform.position = new Vector3(-30, 5f - (currentEditorTeam + 1) * 1.4f, -1);

            if (currentEditorTeam == 1000)
                addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5 - (currentEditorTeam + 1) * 1.5f, addNewTeamButton.transform.position.z);
            else
                addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5f - (currentEditorTeam + 1) * 1.4f, addNewTeamButton.transform.position.z);

            temp.displayTeam(copy);
            displayTeams[currentEditorTeam].selfDestruct();
            DestroyImmediate(displayTeams[currentEditorTeam].gameObject);
            displayTeams.RemoveAt(currentEditorTeam);
            displayTeams.Insert(currentEditorTeam, temp);
            currentEditorTeam = -1;
        }
        createTeamButton.gameObject.SetActive(false);
        foreach (PlateScript p in teamEditor)
        {
            p.setPlate(null);
        }
        mainCam.transform.position = new Vector3(-25, 0, -10);
        
        if (teams.Count == 6)
        {
            addNewTeamButton.gameObject.SetActive(false);
        }
        loadTeams();
    }

    public void displayTeam(List<OpalScript> opals, int teamNum)
    {
        if(waiting != -1)
        {
            activeTeams.Add(opals);
            if (toggleAI.getToggle())
            {
                toggleAI.setToggle(false);
                controls[activeTeams.Count-1] = "AI";
            }
            return;
        }
        foreach(OpalScript o in opals)
        {
            if(o == null)
            {
                return;
            }
            if(o.getCharms()[0] != null)
            {
                bannedCharms.Add(o.getCharmsNames()[0]);
            }
        }
        mainCam.transform.position = new Vector3(0, 15, -10);
        while (opals.Count != teamEditor.Count)
        {
            if (opals.Count > teamEditor.Count)
                incTeamNum();
            else if (opals.Count < teamEditor.Count)
                decrTeamNum();
        }
        int i = 0;
        foreach (PlateScript p in teamEditor)
        {
            p.setPlate(opals[i]);
            i++;
        }
        createTeamButton.gameObject.SetActive(true);
        currentEditorTeam = teamNum;
        displayOpal(opals[0], true);
    }
  
    public void deleteTeam(OpalTeam oT, int teamNum)
    {
        teams.RemoveAt(teamNum);
        bannedCharms.Clear();
        loadTeams();
        if (teams.Count == 1000)
            addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5 - (teams.Count + 1) * 1.5f, addNewTeamButton.transform.position.z);
        else
            addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5f - (teams.Count + 1) * 1.4f, addNewTeamButton.transform.position.z);
        if (teams.Count < 6)
        {
            addNewTeamButton.gameObject.SetActive(true);
        }
    }

    private string getPersonalityStats(string personality)
    {
        if(personality == "Straight-Edge")
        {
            return "(No Stat Changes)";
        }
        if(personality == "")
        {
            return "";
        }
        string stats = "";
        foreach(string s in personalities)
        {
            string[] temp = s.Split(';');
            if(temp[0] == personality)
            {
                stats = temp[1];
            }
        }
        if(stats == "")
        {
            return "";
        }
        string[] temp2 = stats.Split(',');
        string pHealth = temp2[0];
        string pAttack = temp2[1];
        string pDefense = temp2[2];
        string pSpeed = temp2[3];
        string pPriority = temp2[4];
        string firsthalf = "";
        string secondhalf = "";
        if (int.Parse(pHealth) > 0)
        {
            firsthalf = "+" + pHealth + " health, ";
        }
        if (int.Parse(pAttack) > 0)
        {
            firsthalf = "+" + pAttack + " attack, ";
        }
        else if (int.Parse(pAttack) < 0)
        {
            secondhalf = pAttack + " attack ";
        }

        if (int.Parse(pDefense) > 0)
        {
            firsthalf = "+" + pDefense + " defense, ";
        }
        else if (int.Parse(pDefense) < 0)
        {
            secondhalf += pDefense + " defense ";
        }

        if (int.Parse(pSpeed) > 0)
        {
            firsthalf = "+" + pSpeed + " speed, ";
        }
        else if (int.Parse(pSpeed) < 0)
        {
            secondhalf = pSpeed + " speed ";
        }

        if (int.Parse(pPriority) > 0)
        {
            firsthalf = "+" + pPriority + " priority, ";
        }
        else if (int.Parse(pPriority) < 0)
        {
            secondhalf = pPriority + " priority ";
        }
        return firsthalf + secondhalf;

    }

    public void setNextPersonality(bool reverse)
    {

        if (!reverse)
        {
            personalityNum++;
            if (personalityNum >= personalities.Count)
            {
                personalityNum = 0;
            }
        }
        else
        {
            personalityNum--;
            if (personalityNum < 0)
            {
                personalityNum = personalities.Count-1;
            }
        }
        string[] temp = personalities[personalityNum].Split(';');
        string pName = temp[0];
        string stats = temp[1];
        personalityTracker.text = "" + pName + "\n" + getPersonalityStats(pName);
        if (viewedOpal == null)
            return;
        viewedOpal.setPersonality(pName);
    }

    public void setNextVisual(bool reverse)
    {
        Texture2D[] tempTextures = Resources.LoadAll<Texture2D>("Spritesheets/Alternates/"+viewedOpal.getMyName());

        List<Texture2D> textures = new List<Texture2D>();
        textures.AddRange(tempTextures);

        if(textures.Count == 0)
        {
            visualLabel.text = "Default";
            return;
        }


        int target = -1;
        for(int i = 0; i < textures.Count; i++)
        {
            if(textures[i].name == "Default")
            {
                target = i;
            }
        }
        if (target != -1)
            textures.RemoveAt(target);

        int numTextures = textures.Count;

        if (!reverse)
        {
            currentVisual++;
            if (currentVisual >= textures.Count)
            {
                currentVisual = -1;
            }
        }
        else
        {
            currentVisual--;
            if (currentVisual <= -2)
            {
                currentVisual = textures.Count - 1;
            }
        }
        if (currentVisual == -1)
        {
            visualLabel.text = "Default";
            teamOpalDisplay.getCurrentOpalInstance().changeVisual("Default", true);
        }
        else
        {
            visualLabel.text = textures[currentVisual].name;
            teamOpalDisplay.getCurrentOpalInstance().changeVisual(textures[currentVisual].name, true);
            viewedOpal.changeVisual(textures[currentVisual].name, true);

        }
    }

    private OpalScript findViewedOpal()
    {

        foreach(OpalScript o in teams[currentEditorTeam])
        {
            if (o.getMyName() == teamOpalDisplay.getCurrentOpal().getMyName())
                return teamOpalDisplay.getCurrentOpal();
        }
        return null;
    }

    public void startLocalGame()
    {
        activeTeams.Clear();
        waiting = 1;
    }

    public void updateTeamsText()
    {
        if (waiting == -1 && chooseTeamsText.text != "Teams")
        {
            chooseTeamsText.text = "Teams";
        }
        else if (waiting == 0 && chooseTeamsText.text != "Choose 1 team!")
        {
            chooseTeamsText.text = "Choose 1 team!";
        }
        else if (waiting == 1 && chooseTeamsText.text != "Choose " + (numTeams - activeTeams.Count) + " teams!")
        {
            chooseTeamsText.text = "Choose " + (numTeams - activeTeams.Count) + " teams!";
        }
        else if (waiting == 2 && chooseTeamsText.text != "Choose 1 team!")
        {
            chooseTeamsText.text = "Choose 1 team!";
        }
    }

    public void loadTeams()
    {
        if(displayTeams.Count > 0)
        {
            foreach(OpalTeam oT in displayTeams)
            {
                oT.selfDestruct();
            }
        }
        displayTeams.Clear();
        int i = 0;
        foreach(List<OpalScript> l in teams)
        {
            List<OpalScript> teamOpals = l;
            OpalTeam temp = Instantiate<OpalTeam>(Resources.Load<OpalTeam>("Prefabs/OpalTeam"));
            temp.setMain(this, i);
            if (i == 1000)
                temp.transform.position = new Vector3(-30, 5 - (i+1) * 1.5f, -1);
            else
                temp.transform.position = new Vector3(-30, 5 - (i+1) * 1.4f, -1);
            temp.displayTeam(teamOpals); //this is broken
            displayTeams.Add(temp);
            i++;
        }
        if (teams.Count == 100)
            addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5 - (teams.Count + 1) * 1.5f, addNewTeamButton.transform.position.z);
        else
            addNewTeamButton.transform.position = new Vector3(addNewTeamButton.transform.position.x, 5 - (teams.Count + 1) * 1.4f, addNewTeamButton.transform.position.z);
        saveData();
    }

    public void startMultiplayerGame()
    {
        activeTeams.Clear();
        waiting = 0;
    }

    public void startLocalAI()
    {
        activeTeams.Clear();
        waiting = 1;
    }

    private void populateOpalScreen()
    {
        /**
        foreach(PlateScript p in displayOpals)
        {
            Destroy(p.gameObject);
        }
        displayOpals.Clear();
        float x = 20;
        float y = 19;
        int i = 0;
        int offset = 0;
        maxOpalPage = Mathf.CeilToInt(allOpals.Length / 25f);
        for (int j = 0 + offset; j < 25 + offset; j++)
        {
            PlateScript tempP = Instantiate<PlateScript>(platePrefab);
            tempP.setPlate(allOpals[j], x, y);
            x += 1.6f;
            i++;
            if (i == 5)
            {
                x = 20;
                y -= 1.6f;
                i = 0;
            }
            displayOpals.Add(tempP);
        }*/

        foreach (PlateScript p in displayOpals)
        {
            p.clearPlate();
            Destroy(p.gameObject);
        }
        displayOpals.Clear();
        float x = 20;
        float y = 19;
        int i = 0;
        int offset = 0;
        maxOpalPage = Mathf.CeilToInt(myOpals.Count / 25f);
        for (int j = 0 + offset; j < 25 + offset; j++)
        {
            PlateScript tempP = Instantiate<PlateScript>(platePrefab);
            if(j < myOpals.Count)
                tempP.setPlate(myOpals[j], x, y);
            else
                tempP.setPlate(null, x, y);
            x += 1.6f;
            i++;
            if (i == 5)
            {
                x = 20;
                y -= 1.6f;
                i = 0;
            }
            displayOpals.Add(tempP);
        }
    }

    public OpalScript pickNewOpal(bool rand)
    {
        if (myOpals.Count == allOpals.Length)
            return null;
        List<OpalScript> newOpals = new List<OpalScript>();
        foreach(OpalScript o in allOpals)
        {
            if (!myOpals.Contains(o))
            {
                newOpals.Add(o);
            }
        }
        return newOpals[Random.Range(0, newOpals.Count)];
    }

    public void addNewOpal()
    {
        OpalScript add = pickNewOpal(true);
        if (add == null)
            return;
        myOpals.Add(add);
        populateOpalScreen();
        saveData();
    }

    public void wipeOpals()
    {
        myOpals.Clear();
        populateOpalScreen();
        saveData();
    }

    public void refillOpals()
    {
        wipeOpals();
        foreach(OpalScript o in allOpals)
        {
            myOpals.Add(o);
        }
        populateOpalScreen();
        saveData();
    }

    public void saveData()
    {
        string path = "/StreamingAssets/save.txt";
        using (var stream = new FileStream(Application.dataPath + path, FileMode.Truncate))
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write("Please don't change this, it will be encrypted eventually.\n");
                foreach(List<OpalScript> lO in teams)
                {
                    foreach(OpalScript o in lO)
                    {
                        o.setOpal(null);
                        writer.Write(o.saveOpal() + ",");
                        
                    }
                    writer.Write("\n");
                }
                writer.Write("endTeams\n");
                foreach (OpalScript o in myOpals)
                {
                    //print("du hello");
                    o.setOpal(null);
                    writer.Write(o.getMyName() + ",");
                }
                writer.Write("\n");
                writer.Write("endOpals\n");
            }
        }
        //AssetDatabase.ImportAsset(path);
        //TextAsset asset = Resources.Load<TextAsset>("save");
    }

    public void loadData()
    {
        string path = "/StreamingAssets/save.txt";
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(Application.dataPath + path);
        string reid = reader.ReadLine();
        reid = reader.ReadLine();
        while (reid != "endTeams")
        {
            string[] opalNames = reid.Split(',');
            List<OpalScript> opals = new List<OpalScript>();
            foreach(string name in opalNames)
            {
                if (name != "\n" && name != "")
                {
                    string[] parsed = name.Split('|');
                    OpalScript temp = Instantiate(Resources.Load<OpalScript>("Prefabs/Opals/" + parsed[0]));
                    if (temp != null)
                    {
                        temp.setFromSave(name);
                        opals.Add(temp);
                    }
                }
            }
            teams.Add(opals);
            reid = reader.ReadLine();
        }
        while(reid != "endOpals")
        {
            string[] opalNames = reid.Split(',');
            foreach (string name in opalNames)
            {
                if (name != "\n" && name != "" && name != "endTeams")
                {
                    //print("name: |"+name+"|");
                    myOpals.Add(Resources.Load<OpalScript>("Prefabs/Opals/" + name));
                }
            }

            reid = reader.ReadLine();
        }
        if(myOpals.Count == 0)
        {
            myOpals.Add(pickNewOpal(false));
            myOpals.Add(pickNewOpal(false));
            myOpals.Add(pickNewOpal(false));
            myOpals.Add(pickNewOpal(false));
            myOpals.Add(pickNewOpal(false));
            myOpals.Add(pickNewOpal(false));
        }
        reader.Close();
        loadTeams();
        populateOpalScreen();
    }

    public OpalScript getRandomAIOpal()
    {
        return Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/Ambush"));
    }

    private string getRandomOpalName()
    {
        return allOpals[Random.Range(0,allOpals.Length)].getMyName();
    }

    public List<string> calculateTypeOverload(List<OpalScript> opals)
    {
        if (opals == null)
            return null;
        Dictionary<string, int> typeFrequency = new Dictionary<string, int>();
        List<string> allTypes = new List<string>();

        foreach (OpalScript o in opals)
        {
            if (typeFrequency.ContainsKey(o.getMainType()))
            {
                typeFrequency[o.getMainType()] += 2;
            }
            else
            {
                typeFrequency.Add(o.getMainType(), 2);
            }

            if (typeFrequency.ContainsKey(o.getSecondType()))
            {
                if (o.getMainType() != o.getSecondType())
                {
                    typeFrequency[o.getSecondType()] += 1;
                }
            }
            else
            {
                if(o.getMainType() != o.getSecondType())
                {
                    typeFrequency.Add(o.getSecondType(), 1);
                }
            }

            if (!allTypes.Contains(o.getMainType()))
            {
                allTypes.Add(o.getMainType());
            }
            if (!allTypes.Contains(o.getSecondType()))
            {
                allTypes.Add(o.getSecondType());
            }
        }
        List<string> overloads = new List<string>();
        foreach(string s in allTypes)
        {
            if(typeFrequency[s] > 5)
            {
                if(!overloads.Contains(s))
                    overloads.Add(s);
            }
        }
        return overloads;
    }
}
