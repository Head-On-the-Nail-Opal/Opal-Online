using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {
    public static GlobalScript Instance;

    private List<OpalScript> redTeam = new List<OpalScript>();
    private List<OpalScript> blueTeam = new List<OpalScript>();
    private List<OpalScript> greenTeam = new List<OpalScript>();
    private List<OpalScript> orangeTeam = new List<OpalScript>();

    private List<string> redOverloads = new List<string>();
    private List<string> blueOverloads = new List<string>();
    private List<string> greenOverloads = new List<string>();
    private List<string> orangeOverloads = new List<string>();


    private string blueController;
    private string redController;
    private string greenController;
    private string orangeController;

    private string username;

    private bool multiplayer = false;
    private int numPlayers = -1;

    private bool finishedGame = false;
    // Use this for initialization
    void Awake () {
		if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }else if(Instance != this)
        {
            Destroy(gameObject);
        }
	}

    public void setTeams(List<OpalScript> rT, List<OpalScript> bT, List<OpalScript> gT, List<OpalScript> oT)
    {
        redTeam.Clear();
        blueTeam.Clear();
        greenTeam.Clear();
        orangeTeam.Clear();
        foreach(OpalScript o in rT)
        {
            OpalScript temp = Instantiate(o);
            temp.setDetails(o);
            redTeam.Add(temp);
        }
        foreach (OpalScript o in bT)
        {
            OpalScript temp = Instantiate(o);
            temp.setDetails(o);
            blueTeam.Add(temp);
        }
        if (gT != null)
        {
            foreach (OpalScript o in gT)
            {
                OpalScript temp = Instantiate(o);
                temp.setDetails(o);
                greenTeam.Add(temp);
            }
        }
        if (oT != null)
        {
            foreach (OpalScript o in oT)
            {
                OpalScript temp = Instantiate(o);
                temp.setDetails(o);
                orangeTeam.Add(temp);
            }
        }
        //redTeam = rT;
        //blueTeam = bT;
    }

    public void setOverloads(List<string> rOverloads, List<string> bOverloads, List<string> gOverloads, List<string> oOverloads)
    {
        redOverloads = rOverloads;
        blueOverloads = bOverloads;
        greenOverloads = gOverloads;
        orangeOverloads = oOverloads;
    }

    public void setNumPlayers(int np)
    {
        //print("wow!");
        numPlayers = np;
    }

    public int getNumPlayers()
    {
        return numPlayers;
    }


    public void setControllers(string red, string blue, string green, string orange)
    {
        blueController = blue;
        redController = red;
        greenController = green;
        orangeController = orange;
    }

    public List<string> getOverload(string team)
    {
        switch (team)
        {
            case "Red":
                return redOverloads;
            case "Blue":
                return blueOverloads;
            case "Green":
                return greenOverloads;
            case "Orange":
                return orangeOverloads;
        }
        return null;
    }

    public List<OpalScript> getBlueTeam()
    {
        return blueTeam;
    }

    public List<OpalScript> getRedTeam()
    {
        return redTeam;
    }

    public List<OpalScript> getGreenTeam()
    {
        return greenTeam;
    }

    public List<OpalScript> getOrangeTeam()
    {
        return orangeTeam;
    }

    public string getBlueController()
    {
        return blueController;
    }

    public string getRedController()
    {
        return redController;
    }

    public string getGreenController()
    {
        return greenController;
    }

    public string getOrangeController()
    {
        return orangeController;
    }

    public bool getMult()
    {
        return multiplayer;
    }

    public void setMult(bool m)
    {
        multiplayer = m;
    }

    public string getUsername()
    {
        return username;
    }

    public void setUsername(string name)
    {
        username = name;
    }

    public void setFinishedGame(bool input)
    {
        finishedGame = input;
    }

    public bool getFinishedGame()
    {
        return finishedGame;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
