using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour {
    public GameObject currentScreen;
    public Text currentName;
    public Text currentType;
    public Text currentHealth;
    public Text currentAttack;
    public Text currentDefense;
    public Text currentSpeed;
    public Text currentMovesLeft;

    public GameObject targetScreen;
    public Text targetName;
    public Text targetType;
    public Text targetHealth;
    public Text targetAttack;
    public Text targetDefense;
    public Text targetSpeed;
    public SpriteRenderer icons;

    public Text attack1;
    public Text attack2;
    public Text attack3;


    public Text theyWon;
    public Transform winBarrier;
    public GameObject winScreen;

    public Transform outline;

    public Text roundCounter;
    public Text warningText;

    private OpalScript displayCurrent = null;
    private OpalScript displaySelected = null;

    private StatusIndicator burnIndicator;
    private StatusIndicator poisonIndicator;
    private StatusIndicator chargeIndicator;
    private StatusIndicator liftIndicator;
    private StatusIndicator armorIndicator;

    private StatusIndicator burnIndicator2;
    private StatusIndicator poisonIndicator2;
    private StatusIndicator chargeIndicator2;
    private StatusIndicator liftIndicator2;
    private StatusIndicator armorIndicator2;

    public AttackIndicator attackIndicator;

    public GameObject tileIndicator;
    public Text tileName;
    public Text tileLife;
    public Text tileDescription1;
    public Text tileDescription2;

    public GameObject itemUI;
    public Text itemName;
    public Text itemDesc;
    public ItemDescriptions iD;
    public int currentItem = 0;

    public AttackScreen attackScreen;
    public AttackScreen attackScreen1;
    public AttackScreen attackScreen2;
    public AttackScreen attackScreen3;

    public SpriteRenderer currentOpalDisplay;
    public SpriteRenderer selectedOpalDisplay;

    public GameObject currentAttackToken;
    public GameObject currentDefenseToken;
    public GameObject currentSpeedToken;

    public GameObject selectedAttackToken;
    public GameObject selectedDefenseToken;
    public GameObject selectedSpeedToken;

    public Text currentAttackBuffs;
    public Text currentDefenseBuffs;
    public Text currentSpeedTBuffs;

    public Text selectedAttackBuffs;
    public Text selectedDefenseBuffs;
    public Text selectedSpeedBuffs;

    private BuffMarker bm;

    private List<BuffMarker> currentAttackMarkers = new List<BuffMarker>();
    private List<BuffMarker> currentDefenseMarkers = new List<BuffMarker>();
    private List<BuffMarker> currentSpeedMarkers = new List<BuffMarker>();

    private List<BuffMarker> selectedAttackMarkers = new List<BuffMarker>();
    private List<BuffMarker> selectedDefenseMarkers = new List<BuffMarker>();
    private List<BuffMarker> selectedSpeedMarkers = new List<BuffMarker>();

    private bool tileEnabled = false;
    private bool charmEnabled = false;
    private bool buffEnabled = false;
    private bool selectedEnabled = false;

    private bool buffAnimating = false;

    //private bool doattacking = false;

    // Use this for initialization
    void Start() {

        theyWon.text = "";
        winBarrier.position = new Vector3(-10000, -10000, -10000);

        StartCoroutine(toggleBuffs(false));
        displayAttacks(null, null);

        //current player
        burnIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        burnIndicator.setType("burning");
        burnIndicator.transform.position = new Vector3(6.50f, 4.5f, -7.75f);
        burnIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        burnIndicator.setEnable(false);

        poisonIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        poisonIndicator.setType("poisoned");
        poisonIndicator.transform.position = new Vector3(7f, 4.5f, -7.25f);
        poisonIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        poisonIndicator.setEnable(false);

        liftIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        liftIndicator.setType("lifted");
        liftIndicator.transform.position = new Vector3(7.50f, 4.5f, -6.75f);
        liftIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        liftIndicator.setEnable(false);

        chargeIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        chargeIndicator.setType("charge");
        chargeIndicator.transform.position = new Vector3(8f, 4.5f, -6.25f);
        chargeIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        chargeIndicator.setEnable(false);

        armorIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        armorIndicator.setType("armor");
        armorIndicator.transform.position = new Vector3(8.50f, 4.5f, -5.75f);
        armorIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        armorIndicator.setEnable(false);


        //selected player
        burnIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        burnIndicator2.setType("burning");
        burnIndicator2.transform.position = new Vector3(14.901f, 4.474f, 0.733f);
        burnIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        burnIndicator2.setEnable(false);

        poisonIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        poisonIndicator2.setType("poisoned");
        poisonIndicator2.transform.position = new Vector3(15.376f, 4.474f, 1.2f);
        poisonIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        poisonIndicator2.setEnable(false);

        liftIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        liftIndicator2.setType("lifted");
        liftIndicator2.transform.position = new Vector3(15.852f, 4.474f, 1.684f);
        liftIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        liftIndicator2.setEnable(false);

        chargeIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        chargeIndicator2.setType("charge");
        chargeIndicator2.transform.position = new Vector3(16.335f, 4.474f, 2.167f);
        chargeIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        chargeIndicator2.setEnable(false);

        armorIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        armorIndicator2.setType("armor");
        armorIndicator2.transform.position = new Vector3(16.808f, 4.474f, 2.648f);
        armorIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        armorIndicator2.setEnable(false);

        bm = Resources.Load<BuffMarker>("Prefabs/UIandMenu/BuffMarker");
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void updateCurrent(OpalScript opal, int movesLeft)
    {
        if(displayCurrent != null && displayCurrent.getMyName() != opal.getMyName())
        {
            DestroyImmediate(displayCurrent.gameObject);
            displayCurrent = null;
        }
        if (displayCurrent == null)
        {
            displayCurrent = Instantiate<OpalScript>(opal);
            displayCurrent.setDisplayOpal();
            displayCurrent.doHighlight();
            //displayCurrent.setOpal(null);
            displayCurrent.transform.position = new Vector3(5.228f, 3.6f, -10.078f);
            displayCurrent.transform.localRotation = Quaternion.Euler(40, -45, 0);
            displayCurrent.transform.localScale *= 2;
            displayCurrent.GetComponent<SpriteRenderer>().sortingLayerName = "UI Back";
            //displayCurrent.resetHighlight();
            //displayCurrent.transform.localScale *= 2;
        }
        Attack[] atlist = opal.getAttacks();
        currentName.text = opal.getName();
        currentType.text = opal.getTypes();
        currentHealth.text = ""+opal.getHealth()+"/"+opal.getMaxHealth();
        currentAttack.text = ""+opal.getAttack();
        currentDefense.text = ""+opal.getDefense();
        currentSpeed.text = ""+opal.getSpeed();
        currentMovesLeft.text = "" + movesLeft;

        generateBuffMarkers(true, opal);

        //attack1.text = atlist[0].aname;
        //attack2.text = atlist[1].aname;
        //attack3.text = atlist[2].aname;
        displayScreen(opal.getPlayer(), true);
        if(opal.getBurning() == true)
        {
            burnIndicator.setEnable(true);
            burnIndicator.setAmount(opal.getBurningDamage());
            burnIndicator.setTimer(opal.getBurnTimer());
        }
        else
        {
            burnIndicator.setEnable(false);
        }

        if (opal.getPoison() == true)
        {
            poisonIndicator.setEnable(true);
            poisonIndicator.setAmount(opal.getPoisonCounter());
            poisonIndicator.setTimer(opal.getPoisonTimer());
        }
        else
        {
            poisonIndicator.setEnable(false);
        }

        if (opal.getCharge() > 0)
        {
            chargeIndicator.setEnable(true);
            chargeIndicator.setAmount(opal.getCharge());
        }
        else
        {
            chargeIndicator.setEnable(false);
        }

        if (opal.getArmor() > 0)
        {
            armorIndicator.setEnable(true);
            armorIndicator.setAmount(opal.getArmor());
        }
        else
        {
            armorIndicator.setEnable(false);
        }

        if (opal.getLifted() == true)
        {
            liftIndicator.setEnable(true);
            liftIndicator.setAmount(1);
            liftIndicator.setTimer(opal.getLiftTimer());
        }
        else
        {
            liftIndicator.setEnable(false);
        }
    }

    public void checkBuffs(OpalScript current, OpalScript selected)
    {
        /**
        if(current.getAttack() - current.getAttackB() >= 0)
            currentAttack.text = "+" + (current.getAttack() - current.getAttackB());
        else
            currentAttack.text = ""+(current.getAttack() - current.getAttackB());
        if (current.getDefense() - current.getDefenseB() >= 0)
            currentDefense.text = "+" + (current.getDefense() - current.getDefenseB());
        else
            currentDefense.text = ""+(current.getDefense() - current.getDefenseB());
        if (current.getSpeed() - current.getSpeedB() >= 0)
            currentSpeed.text = "+" + (current.getSpeed() - current.getSpeedB());
        else
            currentSpeed.text = ""+(current.getSpeed() - current.getSpeedB());

        if (selected != null)
        {
            if (selected.getAttack() - selected.getAttackB() >= 0)
                targetAttack.text = "+" + (selected.getAttack() - selected.getAttackB());
            else
                targetAttack.text = "" + (selected.getAttack() - selected.getAttackB());
            if (selected.getDefense() - selected.getDefenseB() >= 0)
                targetDefense.text = "+" + (selected.getDefense() - selected.getDefenseB());
            else
                targetDefense.text = "" + (selected.getDefense() - selected.getDefenseB());
            if (selected.getSpeed() - selected.getSpeedB() >= 0)
                targetSpeed.text = "+" + (selected.getSpeed() - selected.getSpeedB());
            else
                targetSpeed.text = "" + (selected.getSpeed() - selected.getSpeedB());
        }
    */
        if (!buffEnabled)
        {
            StartCoroutine(toggleBuffs(true));
            buffEnabled = true;
        }
    }

    public void disableBuffs(OpalScript current, OpalScript selected)
    {
        /**
        if (current != null)
        {
            currentAttack.text = "" + current.getAttack();
            currentDefense.text = "" + current.getDefense();
            currentSpeed.text = "" + current.getSpeed();
        }
        if (selected != null)
        {
            targetAttack.text = "" + selected.getAttack();
            targetDefense.text = "" + selected.getDefense();
            targetSpeed.text = "" + selected.getSpeed();
        }
    */
        if (buffEnabled)
        {
            StartCoroutine(toggleBuffs(false));
            buffEnabled = false;
        }
    }

    public void updateSelected(OpalScript opal)
    {
        if (opal == null)
        {
            if(displaySelected != null)
            {
                DestroyImmediate(displaySelected.gameObject);
                displaySelected = null;
            }
            selectedEnabled = false;
            targetName.text = "";
            targetType.text = "";
            targetHealth.text = "";
            targetAttack.text = "";
            targetDefense.text = "";
            targetSpeed.text = "";
            burnIndicator2.setEnable(false);
            poisonIndicator2.setEnable(false);
            liftIndicator2.setEnable(false);
            chargeIndicator2.setEnable(false);
            armorIndicator2.setEnable(false);
            selectedOpalDisplay.enabled = false;
            targetScreen.GetComponent<SpriteRenderer>().enabled = false;
            icons.enabled = false;
            selectedAttackToken.GetComponent<SpriteRenderer>().enabled = false;
            selectedDefenseToken.GetComponent<SpriteRenderer>().enabled = false;
            selectedSpeedToken.GetComponent<SpriteRenderer>().enabled = false;

            generateBuffMarkers(false, null);
        }
        else
        {
            selectedEnabled = true;
            selectedOpalDisplay.enabled = true;
            targetScreen.GetComponent<SpriteRenderer>().enabled = true;
            selectedAttackToken.GetComponent<SpriteRenderer>().enabled = true;
            selectedDefenseToken.GetComponent<SpriteRenderer>().enabled = true;
            selectedSpeedToken.GetComponent<SpriteRenderer>().enabled = true;
            icons.enabled = true;
            if (displaySelected != null && displaySelected.getMyName() != opal.getMyName())
            {
                DestroyImmediate(displaySelected.gameObject);
                displaySelected = null;
            }
            if (displaySelected == null)
            {
                displaySelected = Instantiate<OpalScript>(opal);
                displaySelected.setDisplayOpal();
                displaySelected.doHighlight();
                //displaySelected.setOpal(null);
                displaySelected.transform.position = new Vector3(15.69f, 3.6f, 0.61f);
                displaySelected.transform.localRotation = Quaternion.Euler(40, -45, 0);
                displaySelected.transform.localScale *= 2;
                
                displaySelected.GetComponent<SpriteRenderer>().sortingLayerName = "UI Back";
                //displaySelected.resetHighlight();
                //displaySelected.transform.localScale *= 2;
            }
            targetName.text = opal.getName();
            targetType.text = opal.getTypes();
            targetHealth.text = "" + opal.getHealth() + "/" + opal.getMaxHealth();
            targetAttack.text = "" + opal.getAttack();
            targetDefense.text = "" + opal.getDefense();
            targetSpeed.text = "" + opal.getSpeed();

            

            displayScreen(opal.getPlayer(), false);

            generateBuffMarkers(false, opal);
            if (opal.getBurning() == true)
            {
                burnIndicator2.setEnable(true);
                burnIndicator2.setAmount(opal.getBurningDamage());
                burnIndicator2.setTimer(opal.getBurnTimer());
            }
            else
            {
                burnIndicator2.setEnable(false);
            }

            if (opal.getPoison() == true)
            {
                poisonIndicator2.setEnable(true);
                poisonIndicator2.setAmount(opal.getPoisonCounter());
                poisonIndicator2.setTimer(opal.getPoisonTimer());
            }
            else
            {
                poisonIndicator2.setEnable(false);
            }

            if (opal.getCharge() > 0)
            {
                chargeIndicator2.setEnable(true);
                chargeIndicator2.setAmount(opal.getCharge());
            }
            else
            {
                chargeIndicator2.setEnable(false);
            }

            if (opal.getArmor() > 0)
            {
                armorIndicator2.setEnable(true);
                armorIndicator2.setAmount(opal.getArmor());
            }
            else
            {
                armorIndicator2.setEnable(false);
            }

            if (opal.getLifted() == true)
            {
                liftIndicator2.setEnable(true);
                liftIndicator2.setAmount(1);
                liftIndicator2.setTimer(opal.getLiftTimer());
            }
            else
            {
                liftIndicator2.setEnable(false);
            }
        }
    }

    private void displayScreen(string player, bool current)
    {
        if (player == null)
            return;
        if (player.Equals("Red"))
        {
            if (current)
            {
                currentScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.3f, 0.3f);
                currentOpalDisplay.color = new Color(1, 0.3f, 0.3f);

                currentAttackToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);
                currentDefenseToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);
                currentSpeedToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);

                //outline.GetComponent<Renderer>().material.color = Color.red;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.red;
                }
            }
            else
            {

                //outline.GetComponent<Renderer>().material.color = Color.red;
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.3f, 0.3f);
                selectedOpalDisplay.color = new Color(1, 0.3f, 0.3f);

                selectedAttackToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);
                selectedDefenseToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);
                selectedSpeedToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.1f, 0.1f);

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    //t.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
        else if(player == "Blue")
        {
            if (current)
            {
                //outline.GetComponent<Renderer>().material.color = Color.blue;
                currentScreen.GetComponent<SpriteRenderer>().material.color = new Color(0.4f, 0.4f, 1);
                currentOpalDisplay.color = new Color(0.4f, 0.4f, 1);

                currentAttackToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);
                currentDefenseToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);
                currentSpeedToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            else
            {

                //outline.GetComponent<Renderer>().material.color = Color.blue;
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(0.4f, 0.4f, 1);
                selectedOpalDisplay.color = new Color(0.4f, 0.4f, 1);

                selectedAttackToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);
                selectedDefenseToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);
                selectedSpeedToken.GetComponent<SpriteRenderer>().color = new Color(0.1f, 0.1f, 1);

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    //t.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }
        else if (player == "Green")
        {
            if (current)
            {
                currentScreen.GetComponent<SpriteRenderer>().material.color = Color.green;
                currentOpalDisplay.color = Color.green;

                currentAttackToken.GetComponent<SpriteRenderer>().color =  Color.green;
                currentDefenseToken.GetComponent<SpriteRenderer>().color =  Color.green;
                currentSpeedToken.GetComponent<SpriteRenderer>().color = Color.green;

                //outline.GetComponent<Renderer>().material.color = Color.blue;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                targetScreen.GetComponent<SpriteRenderer>().material.color = Color.green;
                selectedOpalDisplay.color = Color.green;
                //outline.GetComponent<Renderer>().material.color = Color.blue;

                selectedAttackToken.GetComponent<SpriteRenderer>().color = Color.green;
                selectedDefenseToken.GetComponent<SpriteRenderer>().color = Color.green;
                selectedSpeedToken.GetComponent<SpriteRenderer>().color = Color.green;

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    //t.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
        else if (player == "Orange")
        {
            if (current)
            {
                currentScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.4f, 0f);
                currentOpalDisplay.color = new Color(1, 0.4f, 0f);
                //outline.GetComponent<Renderer>().material.color = Color.blue;

                currentAttackToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);
                currentDefenseToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);
                currentSpeedToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = new Color(255, 0.5f, 0);
                }
            }
            else
            {
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.4f, 0f);
                selectedOpalDisplay.color = new Color(1, 0.4f, 0f);
                //outline.GetComponent<Renderer>().material.color = Color.blue;

                selectedAttackToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);
                selectedDefenseToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);
                selectedSpeedToken.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0f);

                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    //t.GetComponent<Renderer>().material.color = new Color(255, 0.5f, 0);
                }
            }
        }
    }

    public void updateAttackScreen(OpalScript attacking, int attackNum, TileScript target)
    {
        //updateAttackIndicator(attackNum);

        attackScreen.updateScreen(attacking, attackNum, target, true);
        attackScreen1.updateScreen(null, -1, null, false);
        attackScreen2.updateScreen(null, -1, null, false);
        attackScreen3.updateScreen(null, -1, null, false);
    }

    public void enableTileScreen(bool enable, string type, int tl)
    {
        if (!enable)
        {
            StartCoroutine(toggleTile(false));
            tileEnabled = false;
            
        }
        else
        {
            if (!tileEnabled)
                StartCoroutine(toggleTile(true));
            tileIndicator.GetComponent<SpriteRenderer>().enabled = true;
            tileName.text = type;
            tileLife.text = tl+"";
            if(tl == 100)
            {
                tileLife.text = "-";
            }
            if (type == "Grass")
            {
                tileDescription1.text = "Basic tile";
                tileDescription2.text = "No special effects";
            }
            else if(type == "Fire")
            {
                tileName.text = "Flame";
                tileDescription1.text = "Traversal will cause burning";
                tileDescription2.text = "Attacks made will burn target";
            }
            else if (type == "Miasma")
            {
                tileDescription1.text = "Traversal will cause poisoning";
                tileDescription2.text = "Standing gives a +2 defense bonus";
            }
            else if (type == "Growth")
            {
                tileDescription1.text = "Traversal will cure poisoning";
                tileDescription2.text = "Standing gives a +2 attack and defense";
            }
            else if (type == "Flood")
            {
                tileDescription1.text = "Traversal will cure burning";
                tileDescription2.text = "Increases range of Water Rush";
            }
            else if (type == "Boulder")
            {
                tileDescription1.text = "Impedes movement";
                tileDescription2.text = "Attack the Boulder to destroy it";
            }
        }
    }

    public void doWin(string whoWon)
    {
        winBarrier.localPosition = new Vector3(0, 0, 0);
        if (whoWon == "Tie")
        {
            //theyWon.text = "Oh no a tie! Better play again!!!\n Press ESCAPE or START BUTTON to go the Main Menu";
            theyWon.color = Color.magenta;
            winScreen.GetComponent<SpriteRenderer>().color = Color.magenta;
            return;
        }
        //theyWon.text = "Congrats!\n " + whoWon + " Team won!!!!\n Press ESCAPE or START BUTTON to go the Main Menu";
        if(whoWon == "Red")
        {
            theyWon.color = Color.red;
            winScreen.GetComponent<SpriteRenderer>().color = Color.red;
        }else if(whoWon == "Blue")
        {
            theyWon.color = Color.blue;
            winScreen.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (whoWon == "Green")
        {
            theyWon.color = Color.green;
            winScreen.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (whoWon == "Orange")
        {
            theyWon.color = new Color(1, 0.5f, 0);
            winScreen.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0);
        }
    }

    public void setUpWinOpals(List<OpalScript> winners)
    {
        if(winners.Count > 0)
        {
            winners[0].transform.position = new Vector3(8.5f,7,-4);
            winners[0].transform.rotation = Quaternion.Euler(35, -45, 0);
            winners[0].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            winners[0].setOpal(winners[0].getTeam());
            winners[0].transform.localScale *= 2;
            winners[0].flipOpal(true);
            winners[0].resetHighlight();
        }if(winners.Count > 1)
        {
            winners[1].transform.position = new Vector3(13f, 7, 0.3f);
            winners[1].transform.rotation = Quaternion.Euler(35, -45, 0);
            winners[1].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            winners[1].setOpal(winners[0].getTeam());
            winners[1].transform.localScale *= 2;
            winners[1].flipOpal(false);
            winners[1].resetHighlight();
        }
        if (winners.Count > 2)
        {
            winners[2].transform.position = new Vector3(9.5f, 4.5f, -5);
            winners[2].transform.rotation = Quaternion.Euler(35, -45, 0);
            winners[2].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            winners[2].setOpal(winners[0].getTeam());
            winners[2].transform.localScale *= 2;
            winners[2].flipOpal(true);
            winners[2].resetHighlight();
        }
        if (winners.Count > 3)
        {
            winners[3].transform.position = new Vector3(14, 4.5f, -0.5f);
            winners[3].transform.rotation = Quaternion.Euler(35, -45, 0);
            winners[3].transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            winners[3].setOpal(winners[0].getTeam());
            winners[3].transform.localScale *= 2;
            winners[3].flipOpal(false);
            winners[3].resetHighlight();
        }
    }

    public IEnumerator displayRoundNum(int round)
    {
        roundCounter.transform.localPosition = new Vector3(0, 0, 0);
        roundCounter.text = "R";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Ro";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Rou";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Roun";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Round";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Round ";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Round " + round;
        yield return new WaitForFixedUpdate();
        if(round > 4 && round % 3 == 2)
        {
            warningText.text = "";
            yield return new WaitForFixedUpdate();
            warningText.text = "Im";
            yield return new WaitForFixedUpdate();
            warningText.text = "Immi";
            yield return new WaitForFixedUpdate();
            warningText.text = "Immine";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Ea";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Eart";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Earthq";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Earthqua";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Earthquake";
            yield return new WaitForFixedUpdate();
            yield return new WaitForSeconds(0.5f);
            warningText.text = "Imminent Earthqua";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Earthq";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Eart";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent Ea";
            yield return new WaitForFixedUpdate();
            warningText.text = "Imminent";
            yield return new WaitForFixedUpdate();
            warningText.text = "Immine";
            yield return new WaitForFixedUpdate();
            warningText.text = "Immi";
            yield return new WaitForFixedUpdate();
            warningText.text = "Im";
            yield return new WaitForFixedUpdate();
            warningText.text = "";
            yield return new WaitForFixedUpdate();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        roundCounter.text = "Round " + round;
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Round ";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Round";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Roun";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Rou";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "Ro";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "R";
        yield return new WaitForFixedUpdate();
        roundCounter.text = "";
    }

    public void updateAttackIndicator(int attackNum)
    {
        attackIndicator.displayAttack(attackNum);
    }

    public void displayAttacks(OpalScript current, OpalScript selected)
    {
        if(selected == null && current == null)
        {
            attackScreen.updateScreen(null, -1, null, false);
            attackScreen1.updateScreen(null, -1, null, false);
            attackScreen2.updateScreen(null, -1, null, false);
            attackScreen3.updateScreen(null, -1, null, false);
            updateCharm(null);
        }
        if(selected == null)
        {
            if(current != null)
            {
                attackScreen.updateScreen(current, 0, null, false);
                attackScreen1.updateScreen(current, 1, null, false);
                attackScreen2.updateScreen(current, 2, null, false);
                attackScreen3.updateScreen(current, 3, null, false);
                updateCharm(current);
            }
        }
        else
        {
            attackScreen.updateScreen(selected, 0, null, false);
            attackScreen1.updateScreen(selected, 1, null, false);
            attackScreen2.updateScreen(selected, 2, null, false);
            attackScreen3.updateScreen(selected, 3, null, false);
            updateCharm(selected);
        }
    }

    public void updateCharm(OpalScript o)
    {
        if(o == null)
        {
            
            itemName.text = "";
            itemName.fontSize = 20;
            itemDesc.text = "";
            StartCoroutine(toggleCharm(false));
            charmEnabled = false;
        }else if (o.getCharmsNames().Count == 0 || o.getCharmsNames()[0] == "None")
        {
            itemName.text = "";
            itemName.fontSize = 20;
            itemDesc.text = "";
            StartCoroutine(toggleCharm(false));
            charmEnabled = false;
        }
        else if (o.getCharmsNames().Count > currentItem && !o.checkRevealed(o.getCharmsNames()[currentItem])&& false)
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = true;
            itemName.text = "Hidden Charm";
            itemName.fontSize = 20;
            itemDesc.text = "This charm is still unknown.";
            if (!charmEnabled)
            {
                charmEnabled = true;
                StartCoroutine(toggleCharm(true));
            }
        }
        else
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = true;
            itemName.text = o.getCharmsNames()[currentItem];
            itemName.fontSize = 20;
            itemDesc.text = iD.getDescFromItem(o.getCharmsNames()[currentItem]);
            if (!charmEnabled)
            {
                charmEnabled = true;
                StartCoroutine(toggleCharm(true));
            }
        }
    }

    public void cycleCharm(bool up, OpalScript o) {
        int count = o.getCharms().Count;
        if (up)
        {
            if (currentItem > 0)
                currentItem--;
        }
        else
        {
            if (currentItem < count-1)
                currentItem++;
        }
        updateCharm(o);
    }


    //current is true if updating current opal, false if updating selected opal
    public void generateBuffMarkers(bool current, OpalScript myBuffs)
    {
        if (current)
        {
            foreach(BuffMarker b in currentAttackMarkers)
            {
                Destroy(b.gameObject);
            }
            currentAttackMarkers.Clear();
            foreach (BuffMarker b in currentDefenseMarkers)
            {
                Destroy(b.gameObject);
            }
            currentDefenseMarkers.Clear();
            foreach (BuffMarker b in currentSpeedMarkers)
            {
                Destroy(b.gameObject);
            }
            currentSpeedMarkers.Clear();
        }
        else
        {
            foreach (BuffMarker b in selectedAttackMarkers)
            {
                Destroy(b.gameObject);
            }
            selectedAttackMarkers.Clear();
            foreach (BuffMarker b in selectedDefenseMarkers)
            {
                Destroy(b.gameObject);
            }
            selectedDefenseMarkers.Clear();
            foreach (BuffMarker b in selectedSpeedMarkers)
            {
                Destroy(b.gameObject);
            }
            selectedSpeedMarkers.Clear();
        }

        if (myBuffs == null)
            return;

        List<Vector3> attackValues = new List<Vector3>();
        List<Vector3> defenseValues = new List<Vector3>();
        List<Vector3> speedValues = new List<Vector3>();
        for (int i = 0; i < 101; i++)
        {
            attackValues.Add(new Vector3(0,0,0));
            defenseValues.Add(new Vector3(0, 0, 0));
            speedValues.Add(new Vector3(0, 0, 0));
        }
        foreach (OpalScript.TempBuff t in myBuffs.getBuffs())
        {
            List<Vector3> values;

            if(t.getTargetStat() == 0)
            {
                values = attackValues;
            }else if(t.getTargetStat() == 1)
            {
                values = defenseValues;
            }
            else
            {
                values = speedValues;
            }

            if (t.getTurnlength() == -1)
            {
                values[0] = new Vector3(t.getTargetStat(), t.getAmount() + values[0].y, 0);
            }
            else if (t.getTurnlength() > 0)
            {
                values[t.getTurnlength()] = new Vector3(t.getTargetStat(), t.getAmount() + values[t.getTurnlength()].y, t.getTurnlength());
            }
        }
        List<Vector3> allValues = new List<Vector3>();

        List<Vector3> attackClean = new List<Vector3>();
        List<Vector3> defenseClean = new List<Vector3>();
        List<Vector3> speedClean = new List<Vector3>();
        foreach (Vector3 v in attackValues)
        {
            if (v != new Vector3(0, 0, 0))
            {
                allValues.Add(v);
                attackClean.Add(v);
            }
        }
        foreach (Vector3 v in defenseValues)
        {
            if (v != new Vector3(0, 0, 0))
            {
                allValues.Add(v);
                defenseClean.Add(v);
            }
        }
        foreach (Vector3 v in speedValues)
        {
            if (v != new Vector3(0, 0, 0))
            {
                allValues.Add(v);
                speedClean.Add(v);
            }
        }

        foreach (Vector3 v in allValues)
        {
            if (v != new Vector3(0, 0, 0))
            {

                BuffMarker temp = Instantiate<BuffMarker>(bm);
                temp.adjustAmount("" + v.y);
                temp.adjustLength("" + v.z);
                if (current)
                {
                    if (v.x == 0)
                    {
                        currentAttackMarkers.Add(temp);
                        spawnMarkerAtToken(temp, currentAttackToken, currentAttackMarkers.Count-1, true);

                    }
                    else if (v.x == 1)
                    {
                        currentDefenseMarkers.Add(temp);
                        spawnMarkerAtToken(temp, currentDefenseToken, currentDefenseMarkers.Count-1, true);

                    }
                    else if (v.x == 2)
                    {
                        currentSpeedMarkers.Add(temp);
                        spawnMarkerAtToken(temp, currentSpeedToken, currentSpeedMarkers.Count-1, true);

                    }
                }
                else
                {
                    if (v.x == 0)
                    {
                        selectedAttackMarkers.Add(temp);
                        spawnMarkerAtToken(temp, selectedAttackToken, selectedAttackMarkers.Count-1, false);

                    }
                    else if (v.x == 1)
                    {
                        selectedDefenseMarkers.Add(temp);
                        spawnMarkerAtToken(temp, selectedDefenseToken, selectedDefenseMarkers.Count-1, false);

                    }
                    else if (v.x == 2)
                    {
                        selectedSpeedMarkers.Add(temp);
                        spawnMarkerAtToken(temp, selectedSpeedToken, selectedSpeedMarkers.Count-1, false);

                    }
                }
            }
        }
    }

    private void spawnMarkerAtToken(BuffMarker buff, GameObject token, int placeInList, bool current)
    {
        buff.transform.parent = token.transform;
        buff.transform.localRotation = Quaternion.Euler(0,0,0);
        buff.transform.localScale = new Vector3(1, 1, 1);
        buff.GetComponent<SpriteRenderer>().color = token.GetComponent<SpriteRenderer>().color;
        buff.hide(!buffEnabled, true);
        buff.hide(!buffEnabled, false);
        float dist = 0.14f;
        if(current)
            buff.transform.localPosition = new Vector3(dist + dist*placeInList, 0, 0);
        else
            buff.transform.localPosition = new Vector3(-dist - dist * placeInList, 0, 0);
    }


    private IEnumerator toggleTile(bool enable)
    {
        if (enable)
        {
            tileEnabled = true;
            tileIndicator.transform.localPosition = new Vector3(tileIndicator.transform.localPosition.x, tileIndicator.transform.localPosition.y + 10, tileIndicator.transform.localPosition.z);
            tileDescription1.enabled = false;
            tileDescription2.enabled = false;
            tileLife.enabled = false;
            tileName.enabled = false;
            for(int i = 0; i < 10;i++)
            {
                tileIndicator.transform.localPosition = new Vector3(tileIndicator.transform.localPosition.x, tileIndicator.transform.localPosition.y -1, tileIndicator.transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
            tileDescription1.enabled = true;
            tileDescription2.enabled = true;
            tileLife.enabled = true;
            tileName.enabled = true;
        }
        else
        {
            tileName.text = "";
            tileLife.text = "";
            tileDescription1.text = "";
            tileDescription2.text = "";
            for (int i = 0; i < 10; i++)
            {
                tileIndicator.transform.localPosition = new Vector3(tileIndicator.transform.localPosition.x, tileIndicator.transform.localPosition.y + 1, tileIndicator.transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
            tileIndicator.transform.localPosition = new Vector3(tileIndicator.transform.localPosition.x, tileIndicator.transform.localPosition.y - 10, tileIndicator.transform.localPosition.z);
            tileIndicator.GetComponent<SpriteRenderer>().enabled = false;
            
        }
    }

    private IEnumerator toggleCharm(bool enable)
    {
        if (enable)
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = true;
            itemUI.transform.localPosition = new Vector3(itemUI.transform.localPosition.x, itemUI.transform.localPosition.y + 1, itemUI.transform.localPosition.z);
            itemName.enabled = false;
            itemDesc.enabled = false;
            for (int i = 0; i < 10; i++)
            {
                itemUI.transform.localPosition = new Vector3(itemUI.transform.localPosition.x, itemUI.transform.localPosition.y - 0.1f, itemUI.transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
            itemName.enabled = true;
            itemDesc.enabled = true;
        }
        else
        {
            
            for (int i = 0; i < 10; i++)
            {
                itemUI.transform.localPosition = new Vector3(itemUI.transform.localPosition.x, itemUI.transform.localPosition.y + 0.1f, itemUI.transform.localPosition.z);
                yield return new WaitForFixedUpdate();
            }
            itemUI.transform.localPosition = new Vector3(itemUI.transform.localPosition.x, itemUI.transform.localPosition.y - 1, itemUI.transform.localPosition.z);
            itemUI.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator toggleBuffs(bool enable)
    {
        buffAnimating = true;
        List<GameObject> moveLeft = new List<GameObject>();
        List<GameObject> moveRight = new List<GameObject>();
        if (enable)
        {
            moveRight.Add(currentAttackToken);
            moveRight.Add(currentDefenseToken);
            moveRight.Add(currentSpeedToken);

            moveLeft.Add(selectedAttackToken);
            moveLeft.Add(selectedDefenseToken);
            moveLeft.Add(selectedSpeedToken);

            hideAllMarkers(true, true);
            hideAllMarkers(false, false);


            if (selectedEnabled)
            {
                selectedAttackToken.GetComponent<SpriteRenderer>().enabled = true;
                selectedDefenseToken.GetComponent<SpriteRenderer>().enabled = true;
                selectedSpeedToken.GetComponent<SpriteRenderer>().enabled = true;
            }

            currentAttackToken.GetComponent<SpriteRenderer>().enabled = true;
            currentDefenseToken.GetComponent<SpriteRenderer>().enabled = true;
            currentSpeedToken.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {

            hideAllMarkers(false, true);
            //hideAllMarkers(true, false);
            hideAllMarkers(true, true);


            moveLeft.Add(currentAttackToken);
            moveLeft.Add(currentDefenseToken);
            moveLeft.Add(currentSpeedToken);

            moveRight.Add(selectedAttackToken);
            moveRight.Add(selectedDefenseToken);
            moveRight.Add(selectedSpeedToken);
        }
        
        for(int i = 0; i < 10; i++)
        {
            foreach(GameObject g in moveRight)
            {
                g.transform.localPosition = new Vector3(g.transform.localPosition.x + 1f, g.transform.localPosition.y, g.transform.localPosition.z);
            }
            foreach (GameObject g in moveLeft)
            {
                g.transform.localPosition = new Vector3(g.transform.localPosition.x - 1f, g.transform.localPosition.y, g.transform.localPosition.z);
            }
            yield return new WaitForFixedUpdate();
        }
        if (!enable)
        {

            hideAllMarkers(false, false);
            hideAllMarkers(true, true);
            hideAllMarkers(true, false);

            selectedAttackToken.GetComponent<SpriteRenderer>().enabled = false;
            selectedDefenseToken.GetComponent<SpriteRenderer>().enabled = false;
            selectedSpeedToken.GetComponent<SpriteRenderer>().enabled = false;
            currentAttackToken.GetComponent<SpriteRenderer>().enabled = false;
            currentDefenseToken.GetComponent<SpriteRenderer>().enabled = false;
            currentSpeedToken.GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            hideAllMarkers(false, true);

        }
        buffAnimating = false;
    }

    private void hideAllMarkers(bool hideMe, bool justText)
    {
        if (selectedEnabled)
        {
            foreach (BuffMarker b in selectedAttackMarkers)
            {
                b.hide(hideMe, justText);
            }
            foreach (BuffMarker b in selectedDefenseMarkers)
            {
                b.hide(hideMe, justText);
            }
            foreach (BuffMarker b in selectedSpeedMarkers)
            {
                b.hide(hideMe, justText);
            }
        }


        foreach (BuffMarker b in currentAttackMarkers)
        {
            b.hide(hideMe, justText);
        }
        foreach (BuffMarker b in currentDefenseMarkers)
        {
            b.hide(hideMe, justText);
        }
        foreach (BuffMarker b in currentSpeedMarkers)
        {
            b.hide(hideMe, justText);
        }

    }


}
