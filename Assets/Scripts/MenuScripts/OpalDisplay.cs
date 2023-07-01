using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpalDisplay : MonoBehaviour {
    public Transform OpalSpot;
    public Text Name;
    public Text Health;
    public Text Attack;
    public Text Defense;
    public Text Speed;
    public Text Type;
    public Text attack1;
    public Text attack2;
    public Text attack3;
    public Text attack4;
    public Text desc1;
    public Text desc2;
    public Text desc3;
    public Text desc4;

    public Text dam1;
    public Text dam2;
    public Text dam3;
    public Text dam4;

    public Text range1;
    public Text range2;
    public Text range3;
    public Text range4;

    public Text keyword1;
    public Text keyword2;
    public Text keyword3;
    public Text keyword4;

    public Text pattern1;
    public Text pattern2;
    public Text pattern3;
    public Text pattern4;

    public Text priority;
    private OpalScript currentOpal = null;
    private OpalScript currentOpalInstance;
    public bool supersizeMe = false;

    private string[] shapes = new string[] { "", "Mortar", "Unimplemented", "Water Rush", "Line of Sight", "Target Growths", "Laser Beam", "All Opals", "Diagonal Laser" };


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCurrentOpal(OpalScript o)
    {
        currentOpal = o;
        if (currentOpalInstance != null)
        {
            DestroyImmediate(currentOpalInstance.gameObject);
        }
        if (currentOpal != null)
        {
            currentOpalInstance = Instantiate<OpalScript>(o);
            currentOpalInstance.setOpal(null);
            currentOpal.setPersonality(o.getPersonality());
            currentOpalInstance.setVariant(o.getVariant());
            currentOpalInstance.transform.position = new Vector3(OpalSpot.transform.position.x, OpalSpot.transform.position.y, -3);
            currentOpalInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            currentOpalInstance.transform.localScale *= 6;
            currentOpalInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
            //TargetInfo.transform.position = new Vector3(0, 18, -1);
            Name.text = currentOpalInstance.getMyName() + "";
            Health.text = currentOpalInstance.getHealth() + "";
            Attack.text = currentOpalInstance.getAttack() + "";
            Defense.text = currentOpalInstance.getDefense() + "";
            Speed.text = currentOpalInstance.getSpeed() + ".";
            Type.text = currentOpalInstance.getTypes() + "";
            attack1.text = currentOpalInstance.getAttacks()[0].aname + "";
            attack2.text = currentOpalInstance.getAttacks()[1].aname + "";
            attack3.text = currentOpalInstance.getAttacks()[2].aname + "";
            attack4.text = currentOpalInstance.getAttacks()[3].aname + "";
            desc1.text = currentOpalInstance.getAttacks()[0].getDesc();
            desc2.text = currentOpalInstance.getAttacks()[1].getDesc();
            desc3.text = currentOpalInstance.getAttacks()[2].getDesc();
            desc4.text = currentOpalInstance.getAttacks()[3].getDesc();
            if(dam1 != null)
            {
                dam1.text = currentOpalInstance.getAttacks()[0].getBaseDamage() + "";
                dam2.text = currentOpalInstance.getAttacks()[1].getBaseDamage() + "";
                dam3.text = currentOpalInstance.getAttacks()[2].getBaseDamage() + "";
                dam4.text = currentOpalInstance.getAttacks()[3].getBaseDamage() + "";
            }
            if (range1 != null)
            {
                range1.text = currentOpalInstance.getAttacks()[0].getRange() + "";
                range2.text = currentOpalInstance.getAttacks()[1].getRange() + "";
                range3.text = currentOpalInstance.getAttacks()[2].getRange() + "";
                range4.text = currentOpalInstance.getAttacks()[3].getRange() + "";
            }
            if (keyword1 != null)
            {
                keyword1.text = extractKeyword(currentOpalInstance.getAttacks()[0].getDesc());
                desc1.text = extractDescription(currentOpalInstance.getAttacks()[0].getDesc());

                keyword2.text = extractKeyword(currentOpalInstance.getAttacks()[1].getDesc());
                desc2.text = extractDescription(currentOpalInstance.getAttacks()[1].getDesc());

                keyword3.text = extractKeyword(currentOpalInstance.getAttacks()[2].getDesc());
                desc3.text = extractDescription(currentOpalInstance.getAttacks()[2].getDesc());

                keyword4.text = extractKeyword(currentOpalInstance.getAttacks()[3].getDesc());
                desc4.text = extractDescription(currentOpalInstance.getAttacks()[3].getDesc());
            }

            if(pattern1 != null)
            {
                pattern1.text = shapes[currentOpalInstance.getAttacks()[0].getShape()];
                pattern2.text = shapes[currentOpalInstance.getAttacks()[1].getShape()];
                pattern3.text = shapes[currentOpalInstance.getAttacks()[2].getShape()];
                pattern4.text = shapes[currentOpalInstance.getAttacks()[3].getShape()];
            }
            priority.text = currentOpalInstance.getPriority() + "";
        }
        if (o == null)
        {
            //TargetInfo.transform.position = new Vector3(-100, -100, -100);
            clearInfo();
        }
    }

    public string extractKeyword(string input)
    {
        string output = "";
        string[] desc = input.Split('\n');
        if (desc.Length == 2)
        {
            output = "" + desc[0].Substring(0, desc[0].Length - 1).Substring(1);
        }

        return output;
    }

    public string extractDescription(string input)
    {
        string output = "";
        string[] desc = input.Split('\n');
        if (desc.Length == 2)
        {
            output = "" + desc[1];
        }
        else
            output = "" + desc[0];
        return output;
    }


    public void setCurrentOpalByName(string on)
    {
        OpalScript o = Resources.Load<OpalScript>("Prefabs/SubOpals/" + on );
        currentOpal = o;
        if (currentOpalInstance != null)
        {
            DestroyImmediate(currentOpalInstance.gameObject);
        }
        if (currentOpal != null)
        {
            currentOpalInstance = Instantiate<OpalScript>(o);
            currentOpalInstance.setOpal(null);
            currentOpalInstance.transform.position = new Vector3(-4 + 1.7f, 29, -2);
            currentOpalInstance.transform.localScale *= 6;
            currentOpalInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
            //TargetInfo.transform.position = new Vector3(0, 18, -1);
            Name.text = currentOpalInstance.getMyName() + "";
            Health.text = currentOpalInstance.getHealth() + "";
            Attack.text = currentOpalInstance.getAttack() + "";
            Defense.text = currentOpalInstance.getDefense() + "";
            Speed.text = currentOpalInstance.getSpeed() + "";
            Type.text = currentOpalInstance.getTypes() + "";
            attack1.text = currentOpalInstance.getAttacks()[0].aname + "";
            attack2.text = currentOpalInstance.getAttacks()[1].aname + "";
            attack3.text = currentOpalInstance.getAttacks()[2].aname + "";
            attack4.text = currentOpalInstance.getAttacks()[3].aname + "";
            desc1.text = currentOpalInstance.getAttacks()[0].getDesc() + "\n(" + currentOpalInstance.getAttacks()[0].getBaseDamage() + " damage)";
            desc2.text = currentOpalInstance.getAttacks()[1].getDesc() + "\n(" + currentOpalInstance.getAttacks()[1].getBaseDamage() + " damage)";
            desc3.text = currentOpalInstance.getAttacks()[2].getDesc() + "\n(" + currentOpalInstance.getAttacks()[2].getBaseDamage() + " damage)";
            desc4.text = currentOpalInstance.getAttacks()[3].getDesc() + "\n(" + currentOpalInstance.getAttacks()[3].getBaseDamage() + " damage)";
            //priority.text = currentOpalInstance.getPriority() + "";
        }
        if (o == null)
        {
            //TargetInfo.transform.position = new Vector3(-100, -100, -100);
            clearInfo();
        }
    }


    public void clearInfo()
    {
        Name.text = "";
        Health.text = "";
        Attack.text = "";
        Defense.text = "";
        Speed.text = "";
        Type.text = "";
        attack1.text = "";
        attack2.text = "Select an Opal";
        attack3.text = "";
        attack4.text = "";
        desc1.text = "";
        desc2.text = "";
        desc3.text = "";
        desc4.text = "";
        priority.text = "";
    }

    public OpalScript getCurrentOpal()
    {
        return currentOpal;
    }

    public OpalScript getCurrentOpalInstance()
    {
        return currentOpalInstance;
    }
}
