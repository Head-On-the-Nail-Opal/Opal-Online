using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour {
    public static GlobalScript Instance;

    private List<OpalScript> redTeam = new List<OpalScript>();
    private List<OpalScript> blueTeam = new List<OpalScript>();
    private List<OpalScript> greenTeam = new List<OpalScript>();
    private List<OpalScript> orangeTeam = new List<OpalScript>();


    private string blueController;
    private string redController;
    private string greenController;
    private string orangeController;

    private string username;

    private bool multiplayer = false;
    private int numPlayers = -1;
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
            temp.setPersonality(o.getPersonality());
            redTeam.Add(temp);
        }
        foreach (OpalScript o in bT)
        {
            OpalScript temp = Instantiate(o);
            temp.setPersonality(o.getPersonality());
            blueTeam.Add(temp);
        }
        foreach (OpalScript o in gT)
        {
            OpalScript temp = Instantiate(o);
            temp.setPersonality(o.getPersonality());
            greenTeam.Add(temp);
        }
        foreach (OpalScript o in oT)
        {
            OpalScript temp = Instantiate(o);
            temp.setPersonality(o.getPersonality());
            orangeTeam.Add(temp);
        }
        //redTeam = rT;
        //blueTeam = bT;
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


    public void setControllers(string blue, string red, string green, string orange)
    {
        blueController = blue;
        redController = red;
        greenController = green;
        orangeController = orange;
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

    // Update is called once per frame
    void Update () {
		
	}
}
