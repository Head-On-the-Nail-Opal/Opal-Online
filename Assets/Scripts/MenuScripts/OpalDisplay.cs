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
    public Text priority;
    private OpalScript currentOpal = null;
    private OpalScript currentOpalInstance;


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
            currentOpalInstance.setVariant(o.getVariant());
            currentOpalInstance.transform.position = new Vector3(OpalSpot.transform.position.x, OpalSpot.transform.position.y, -3);
            currentOpalInstance.transform.localScale *= 2;
            currentOpalInstance.transform.Rotate(new Vector3(0, 45, 0));
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
            desc1.text = currentOpalInstance.getAttacks()[0].getDesc() + "\n(" + currentOpalInstance.getAttacks()[0].getBaseDamage() + " damage)";
            desc2.text = currentOpalInstance.getAttacks()[1].getDesc() + "\n(" + currentOpalInstance.getAttacks()[1].getBaseDamage() + " damage)";
            desc3.text = currentOpalInstance.getAttacks()[2].getDesc() + "\n(" + currentOpalInstance.getAttacks()[2].getBaseDamage() + " damage)";
            desc4.text = currentOpalInstance.getAttacks()[3].getDesc() + "\n(" + currentOpalInstance.getAttacks()[3].getBaseDamage() + " damage)";
            priority.text = currentOpalInstance.getPriority() + "";
        }
        if (o == null)
        {
            //TargetInfo.transform.position = new Vector3(-100, -100, -100);
            clearInfo();
        }
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
            currentOpalInstance.transform.localScale *= 2;
            currentOpalInstance.transform.Rotate(new Vector3(0, 45, 0));
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
