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

    public Text attack1;
    public Text attack2;
    public Text attack3;


    public Text theyWon;
    public Transform winBarrier;

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

    public AttackScreen attackScreen;
    public AttackScreen attackScreen1;
    public AttackScreen attackScreen2;
    public AttackScreen attackScreen3;

    //private bool doattacking = false;

    // Use this for initialization
    void Start() {

        theyWon.text = "";
        winBarrier.position = new Vector3(-10000, -10000, -10000);

        //current player
        burnIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        burnIndicator.setType("burning");
        burnIndicator.transform.position = new Vector3(10.55f, 2.5f, -6.25f);
        burnIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        burnIndicator.setEnable(false);

        poisonIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        poisonIndicator.setType("poisoned");
        poisonIndicator.transform.position = new Vector3(10.27f, 2.96f, -5.97f);
        poisonIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        poisonIndicator.setEnable(false);

        liftIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        liftIndicator.setType("lifted");
        liftIndicator.transform.position = new Vector3(10f, 3.41f, -5.7f);
        liftIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        liftIndicator.setEnable(false);

        chargeIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        chargeIndicator.setType("charge");
        chargeIndicator.transform.position = new Vector3(9.67f, 3.16f, -6.25f);
        chargeIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        chargeIndicator.setEnable(false);

        armorIndicator = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        armorIndicator.setType("armor");
        armorIndicator.transform.position = new Vector3(9.43f, 3.64f, -6.01f);
        armorIndicator.transform.localRotation = Quaternion.Euler(40, -45, 0);
        armorIndicator.setEnable(false);


        //selected player
        burnIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        burnIndicator2.setType("burning");
        burnIndicator2.transform.position = new Vector3(15.33f, 2.5f, -1.47f);
        burnIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        burnIndicator2.setEnable(false);

        poisonIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        poisonIndicator2.setType("poisoned");
        poisonIndicator2.transform.position = new Vector3(15.04f, 2.96f, -1.2f);
        poisonIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        poisonIndicator2.setEnable(false);

        liftIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        liftIndicator2.setType("lifted");
        liftIndicator2.transform.position = new Vector3(14.77f, 3.41f, -0.93f);
        liftIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        liftIndicator2.setEnable(false);

        chargeIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        chargeIndicator2.setType("charge");
        chargeIndicator2.transform.position = new Vector3(15.63f, 2.54f, 0.45f);
        chargeIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        chargeIndicator2.setEnable(false);

        armorIndicator2 = Instantiate(Resources.Load<StatusIndicator>("Prefabs/StatusEffect"));
        armorIndicator2.setType("armor");
        armorIndicator2.transform.position = new Vector3(15.44f, 3.28f, 0.66f);
        armorIndicator2.transform.localRotation = Quaternion.Euler(40, -45, 0);
        armorIndicator2.setEnable(false);
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
            displayCurrent.setOpal(null);
            displayCurrent.transform.position = new Vector3(5.55f, 3.42f, -10.05f);
            displayCurrent.transform.localRotation = Quaternion.Euler(40, -45, 0);
            displayCurrent.showSpot(false);
            //displayCurrent.transform.localScale *= 2;
        }
        Attack[] atlist = opal.getAttacks();
        currentName.text = opal.getName();
        currentType.text = opal.getTypes();
        currentHealth.text = ""+opal.getHealth()+"/"+opal.getMaxHealth();
        currentAttack.text = ""+opal.getAttack();
        currentDefense.text = ""+opal.getDefense();
        currentSpeed.text = ""+opal.getSpeed();
        currentMovesLeft.text = ""+movesLeft;
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
    }

    public void disableBuffs(OpalScript current, OpalScript selected)
    {
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
        }
        else
        {
            if (displaySelected != null && displaySelected.getMyName() != opal.getMyName())
            {
                DestroyImmediate(displaySelected.gameObject);
                displaySelected = null;
            }
            if (displaySelected == null)
            {
                displaySelected = Instantiate<OpalScript>(opal);
                displaySelected.setOpal(null);
                displaySelected.transform.position = new Vector3(16.46f, 3.46f, 0.83f);
                displaySelected.transform.localRotation = Quaternion.Euler(40, -45, 0);
                displaySelected.showSpot(false);
                //displaySelected.transform.localScale *= 2;
            }
            targetName.text = opal.getName();
            targetType.text = opal.getTypes();
            targetHealth.text = "" + opal.getHealth() + "/" + opal.getMaxHealth();
            targetAttack.text = "" + opal.getAttack();
            targetDefense.text = "" + opal.getDefense();
            targetSpeed.text = "" + opal.getSpeed();
            displayScreen(opal.getPlayer(), false);
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
        if (player.Equals("Red"))
        {
            if (current)
            {
                currentScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.3f, 0.3f);
                //outline.GetComponent<Renderer>().material.color = Color.red;
                foreach(Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.red;
                }
            }
            else
            {

                //outline.GetComponent<Renderer>().material.color = Color.red;
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.3f, 0.3f);
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
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
            else
            {

                //outline.GetComponent<Renderer>().material.color = Color.blue;
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(0.4f, 0.4f, 1);
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
                //outline.GetComponent<Renderer>().material.color = Color.blue;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                targetScreen.GetComponent<SpriteRenderer>().material.color = Color.green;
                //outline.GetComponent<Renderer>().material.color = Color.blue;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
        else if (player == "Orange")
        {
            if (current)
            {
                currentScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.4f, 0f);
                //outline.GetComponent<Renderer>().material.color = Color.blue;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = new Color(255, 0.5f, 0);
                }
            }
            else
            {
                targetScreen.GetComponent<SpriteRenderer>().material.color = new Color(1, 0.4f, 0f);
                //outline.GetComponent<Renderer>().material.color = Color.blue;
                foreach (Transform t in outline.GetComponentInChildren<Transform>())
                {
                    t.GetComponent<Renderer>().material.color = new Color(255, 0.5f, 0);
                }
            }
        }
    }

    public void updateAttackScreen(OpalScript attacking, int attackNum, TileScript target)
    {
        updateAttackIndicator(attackNum);
        attackScreen.updateScreen(attacking, attackNum, target);
    }

    public void enableTileScreen(bool enable, string type, int tl)
    {
        if (!enable)
        {
            tileIndicator.GetComponent<SpriteRenderer>().enabled = false;
            tileName.text = "";
            tileLife.text = "";
            tileDescription1.text = "";
            tileDescription2.text = "";
        }
        else
        {
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
        if(whoWon == "Tie")
        {
            theyWon.text = "Oh no a tie! Better play again!!!\n Press ESCAPE or START BUTTON to go the Main Menu";
            theyWon.color = Color.green;
            winBarrier.localPosition = new Vector3(0, 0, 0);
            return;
        }
        theyWon.text = "Congrats!\n " + whoWon + " Team won!!!!\n Press ESCAPE or START BUTTON to go the Main Menu";
        if(whoWon == "Red")
        {
            theyWon.color = Color.red;
        }else if(whoWon == "Blue")
        {
            theyWon.color = Color.blue;
        }
        else if (whoWon == "Green")
        {
            theyWon.color = Color.green;
        }
        else if (whoWon == "Orange")
        {
            theyWon.color = new Color(1, 0.5f, 0);
        }
        winBarrier.localPosition = new Vector3(0,0,0);
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
            attackScreen.updateScreen(null, -1, null);
            attackScreen1.updateScreen(null, -1, null);
            attackScreen2.updateScreen(null, -1, null);
            attackScreen3.updateScreen(null, -1, null);
            updateCharm(null);
        }
        if(selected == null)
        {
            if(current != null)
            {
                attackScreen.updateScreen(current, 0, null);
                attackScreen1.updateScreen(current, 1, null);
                attackScreen2.updateScreen(current, 2, null);
                attackScreen3.updateScreen(current, 3, null);
                updateCharm(current);
            }
        }
        else
        {
            attackScreen.updateScreen(selected, 0, null);
            attackScreen1.updateScreen(selected, 1, null);
            attackScreen2.updateScreen(selected, 2, null);
            attackScreen3.updateScreen(selected, 3, null);
            updateCharm(selected);
        }
    }

    public void updateCharm(OpalScript o)
    {
        if(o == null)
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = false;
            itemName.text = "";
            itemName.fontSize = 20;
            itemDesc.text = "";
        }else if (o.getCharm() == "")
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = false;
            itemName.text = "No Charm";
            itemName.fontSize = 20;
            itemDesc.text = "This opal carries no charm.";
        }else if (!o.getCharmRevealed())
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = true;
            itemName.text = "Hidden Charm";
            itemName.fontSize = 20;
            itemDesc.text = "This charm is still unknown.";
        }
        else
        {
            itemUI.GetComponent<SpriteRenderer>().enabled = true;
            itemName.text = o.getCharm();
            itemName.fontSize = 20;
            itemDesc.text = iD.getDescFromItem(o.getCharm());
        }
    }
}
