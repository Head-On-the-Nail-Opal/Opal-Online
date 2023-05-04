using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampfireMain : MonoBehaviour
{
    private List<OpalScript> allMyOpals = new List<OpalScript>();

    private List<OpalScript> opalParty = new List<OpalScript>();

    private List<OpalScript> pickOpals = new List<OpalScript>();

    private int levelNum = 1;

    private int partyLimit = 4;
    private bool pickMeSwitch = false;

    private Text partyNum;
    private Text opalName;
    private Text opalType;
    private Text opalStats;
    private Text opalAbilties;
    private Text levelText;

    public Canvas allText;

    private GlobalScript gS;


    void Awake()
    {
        foreach (Text t in allText.GetComponentsInChildren<Text>())
        {
            switch (t.gameObject.name) {
                case "PartyText":
                    partyNum = t;
                    break;
                case "NameText":
                    opalName = t;
                    break;
                case "TypeText":
                    opalType = t;
                    break;
                case "StatsText":
                    opalStats = t;
                    break;
                case "AbilityText":
                    opalAbilties = t;
                    break;
                case "LevelText":
                    levelText = t;
                    break;
            }
        }

        gS = FindObjectOfType<GlobalScript>();

        foreach(OpalScript o in gS.getCampfireOpals())
        {
            addOpal(o);
        }

        if(allMyOpals.Count == 0)
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);

        levelNum = gS.getCampfireLevel();
        levelText.text = levelNum+"";

        if (gS.getWon())
        {
            for(int i = 0; i < 3; i++)
            {
                addPickOpal(getRandomOpal());
            }
        }


        setOpalLimit();
        displayOpalInfo(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
            addOpal(getRandomOpal());

        if (Input.GetKeyDown(KeyCode.Return))
            addPickOpal(getRandomOpal());

        if (Input.GetKeyDown(KeyCode.Backspace))
            goToLevel();
        
    }

    public void goToLevel()
    {
        if (opalParty.Count <= partyLimit && opalParty.Count > 0)
        {
            gS.getCampfireOpals().Clear();
            gS.getPickedCampfireOpals().Clear();

            foreach (OpalScript o in opalParty)
            {
                allMyOpals.Remove(o);
                gS.getPickedCampfireOpals().Add(o);
            }
            foreach(OpalScript o in allMyOpals)
            {
                gS.getCampfireOpals().Add(o);
            }
            setUpLevel();
        }
    }

    public void addOpal(OpalScript o)
    {
        OpalScript temp = Instantiate<OpalScript>(Resources.Load<OpalScript>("Prefabs/Opals/"+o.getMyName()));
        temp.setOpal(null);
        temp.transform.localPosition = new Vector3(Random.Range(-5, 6), 2 + Random.Range(-3, 3), 0);
        temp.transform.localEulerAngles = new Vector3(0, 0, 0);
        temp.transform.localScale *= 1.5f;
        temp.gameObject.AddComponent<CampfireOpal>();
        temp.GetComponent<CampfireOpal>().setMain(this);
        temp.gameObject.AddComponent<BoxCollider2D>();
        temp.GetComponent<BoxCollider2D>().size = new Vector2(0.2f,0.2f);
        allMyOpals.Add(temp);
    }

    public void addPickOpal(OpalScript o)
    {
        OpalScript temp = Instantiate<OpalScript>(o);
        temp.setOpal(null);
        temp.transform.localPosition = new Vector3(pickOpals.Count*1.25f-3, -3, 0);
        temp.transform.localEulerAngles = new Vector3(0, 0, 0);
        temp.transform.localScale *= 1.5f;
        temp.gameObject.AddComponent<CampfireOpal>();
        temp.GetComponent<CampfireOpal>().setMain(this);
        temp.GetComponent<CampfireOpal>().setPickMe(true);
        temp.gameObject.AddComponent<BoxCollider2D>();
        temp.GetComponent<BoxCollider2D>().size = new Vector2(0.2f, 0.2f);
        pickOpals.Add(temp);
    }

    public void pickOpal(OpalScript o)
    {
        allMyOpals.Add(o);
        pickOpals.Remove(o);
        foreach (OpalScript opal in pickOpals)
        {
            Destroy(opal.gameObject);
        }
        pickOpals.Clear();
        pickMeSwitch = false;
        o.GetComponent<CampfireOpal>().setPickMe(false);
    }

    public OpalScript getRandomOpal()
    {
        List<OpalScript> allOpals = new List<OpalScript>();
        foreach(OpalScript o in Resources.LoadAll<OpalScript>("Prefabs/Opals"))
        {
            allOpals.Add(o);
        }

        return allOpals[(int)Random.Range(0, allOpals.Count)];
    }

    public void toggleHidePicks(bool tog, OpalScript picking)
    {
        if (pickMeSwitch == tog)
            return;
        pickMeSwitch = tog;
        if (tog)
        {
            foreach(OpalScript o in pickOpals)
            {
                if(o != picking)
                {
                    o.transform.localScale /= 2;
                    o.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                    o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y-0.5f, o.transform.position.z);
                }
            }
        }
        else
        {
            foreach (OpalScript o in pickOpals)
            {
                if (o != picking)
                {
                    o.transform.localScale *= 2;
                    o.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                    o.transform.position = new Vector3(o.transform.position.x, o.transform.position.y + 0.5f, o.transform.position.z);
                }
            }
        }
    }



    public void displayOpalInfo(OpalScript o)
    {
        if (o != null)
        {
            opalName.text = o.getMyName();
            opalType.text = o.getTypes();
            opalStats.text = "Health: " + o.getMaxHealth() + "\nAttack: " + o.getAttack()+ "\nDefense: " + o.getDefense()+ "\nSpeed: " + o.getSpeed() +"." + o.getPriority();

            string abilityBody = "";
            foreach (Attack a in o.getAttacks())
            {
                abilityBody += "<b>"+a.aname + "</b>\n<i>Range:</i> " + a.getRange() + " <i>Base Damage:</i> " + a.getBaseDamage() + "\n" + a.getDesc() + "\n\n";
            }
            opalAbilties.text = abilityBody;
        }
        else
        {
            opalName.text = "";
            opalType.text = "";
            opalStats.text = "";
            opalAbilties.text = "";
        }
    }

    public List<OpalScript> getParty()
    {
        return opalParty;
    }

    public void updatePartyCount()
    {
        partyNum.text = opalParty.Count + "/" + partyLimit + " chosen";
        if (opalParty.Count > partyLimit)
            partyNum.color = new Color(1,0,0);
        if (opalParty.Count < partyLimit)
            partyNum.color = new Color(0.3f, 0.3f, 1);
        if (opalParty.Count == partyLimit)
            partyNum.color = new Color(0, 1, 0);
    }

    private void setUpLevel()
    {
        List<List<string>> possibleEnemies = new List<List<string>>();
        switch (levelNum) {
            case 1:
                possibleEnemies.Add(new List<string> {getOpalName("Offense")});
                break;
            case 2:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense") });
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Support") });
                break;
            case 3:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support") });
                break;
            case 4:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support") });
                break;
            case 5:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support") });
                break;
            case 6:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support"), getOpalName("Offense") });
                break;
            case 7:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support"), getOpalName("Offense") });
                break;
            case 8:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support"), getOpalName("Offense") });
                break;
            case 9:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support"), getOpalName("Offense"), getOpalName("Offense") });
                break;
            case 10:
                possibleEnemies.Add(new List<string> { getOpalName("Offense"), getOpalName("Offense"), getOpalName("Support"), getOpalName("Offense"), getOpalName("Offense") });
                break;
        }

        if(possibleEnemies.Count > 0)
        {
            int randChoice = Random.Range(0, possibleEnemies.Count);
            List<OpalScript> finalPick = new List<OpalScript>();
            foreach (string s in possibleEnemies[randChoice])
            {
                finalPick.Add(Resources.Load<OpalScript>("Prefabs/Opals/"+s));
            }
            gS.setTeams(opalParty, finalPick, null, null);
            gS.setControllers("keyboard", "AI", "AI", "AI");
            gS.setNumPlayers(2);

            UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    private void setOpalLimit()
    {
        if (levelNum < 2)
        {
            partyLimit = 1;
        } else if(levelNum < 5){ 
             partyLimit = 2;
        }else if (levelNum < 12)
        {
            partyLimit = 3;
        }
        else if (levelNum < 21)
        {
            partyLimit = 4;
        }
        updatePartyCount();
    }

    private string getOpalName(string type)
    {
        List<string> pickOne = new List<string>();
        if(type == "Offense")
        {
            pickOne.Add("Ambush");
            pickOne.Add("Bubbacle");
            pickOne.Add("Hearthhog");
            pickOne.Add("Betary");
            pickOne.Add("Duplimorph");
            pickOne.Add("Gorj");
            pickOne.Add("Beamrider");
            pickOne.Add("Experiment42");
            pickOne.Add("Swoopitch");
        }
        else if(type == "Support")
        {
            pickOne.Add("Fluttorch");
            pickOne.Add("Glummer");
            pickOne.Add("Spillarc");
            pickOne.Add("Glorm");
            pickOne.Add("Sentree");
            pickOne.Add("Rekindle");
        }
        return pickOne[Random.Range(0, pickOne.Count)];
    }
}
