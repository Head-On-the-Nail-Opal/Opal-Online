using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackLabel : MonoBehaviour
{
    public Text name;
    public Text description;
    public Text baseDamage;
    public Text range;

    public Text health;
    public Text attack;
    public Text defense;
    public Text speed;
    public Text priority;

    public void setAttackLabel(string n, string d, string bD, string r)
    {
        name.text = n;
        description.text = d;
        baseDamage.text = bD;
        range.text = r;
    }

    public void setStatLabel(string h, string a, string d, string s, string p)
    {
        health.text = h;
        attack.text = a;
        defense.text = d;
        speed.text = s;
        priority.text = p;
    }
}
