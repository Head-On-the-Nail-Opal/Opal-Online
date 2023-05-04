using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpalInspector : MonoBehaviour
{
    public AttackLabel attack0;
    public AttackLabel attack1;
    public AttackLabel attack2;
    public AttackLabel attack3;
    public AttackLabel stats;

    public Text charm;
    public Text personality;
    public Text name;
    public Text type;

    public Text nickname;
    public Text nn;
    public InputField iF;

    public GameObject backDrop;
    private OpalScript myOpal;
    private OpalScript displayOpal;
    private bool personalityInfo = false;


    public void setOpal(OpalScript o)
    {
        if(displayOpal != null)
        {
            Destroy(displayOpal.gameObject);
        }
        myOpal = o;
        displayOpal = Instantiate<OpalScript>(o, backDrop.transform);
        displayOpal.transform.localPosition = new Vector3(0, 0, -1);
        attack0.setAttackLabel(o.getAttacks()[0].aname, o.getAttacks()[0].getDesc(), o.getAttacks()[0].getBaseDamage() + "", o.getAttacks()[0].getRange() + "");
        attack1.setAttackLabel(o.getAttacks()[1].aname, o.getAttacks()[1].getDesc(), o.getAttacks()[1].getBaseDamage() + "", o.getAttacks()[1].getRange() + "");
        attack2.setAttackLabel(o.getAttacks()[2].aname, o.getAttacks()[2].getDesc(), o.getAttacks()[2].getBaseDamage() + "", o.getAttacks()[2].getRange() + "");
        attack3.setAttackLabel(o.getAttacks()[3].aname, o.getAttacks()[3].getDesc(), o.getAttacks()[3].getBaseDamage() + "", o.getAttacks()[3].getRange() + "");

        stats.setStatLabel(o.getMaxHealth() + "", o.getAttack() + "", o.getDefense() + "", o.getSpeed() + "", o.getPriority() + "");

        attack0.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        attack1.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        attack2.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        attack3.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
        stats.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);

        name.text = myOpal.getMyName();
        type.text = myOpal.getTypes();

        if(myOpal.getPersonality() != null)
        {
            personality.text = "Personality: " + myOpal.getPersonality();
        }
        else
        {
            personality.text = "Personality: None?";
        }

        if (myOpal.getCharms()[0] != null)
        {
            charm.text = "Charm: " + myOpal.getCharms()[0];
        }
        else
        {
            charm.text = "Charm: None";
        }

        if(myOpal.getNickname() != null)
        {
            iF.text = myOpal.getNickname();
            nickname.text = myOpal.getNickname();
            nn.text = myOpal.getNickname();
        }
        else
        {
            iF.text = myOpal.getNickname();
            nickname.text = "No Nickname";
            nn.text = "No Nickname";
        }
    }

    public void togglePersonality()
    {
        if (personalityInfo)
        {
            personality.text = "Personality: " + myOpal.getPersonality();
            personalityInfo = false;
        }
        else
        {
            personality.text = "Personality: " + myOpal.getPersonalityInfo(myOpal.getPersonality());
            personalityInfo = true;
        }
    }

    public void setNickname()
    {
        myOpal.setNickname(nn.text);
    }
}
