using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nekrokrab : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 4;
        speed = 1;
        priority = 0;
        myName = "Nekrokrab";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.9f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Toxic Personality", 0, 0, 0, "<Passive>\n Any Opals in the same pool of Floods as Toxikrab at the start of its turn die.");
        Attacks[1] = new Attack("Free Hugs", 0, 0, 0, "<Passive>\n No one will cuddle with Toxikrab (no in-game effect)");
        Attacks[2] = new Attack("Free Hugs", 0, 0, 0, "<Passive>\n No one will cuddle with Toxikrab (no in-game effect)");
        Attacks[3] = new Attack("Free Hugs", 0, 0, 0, "<Passive>\n No one will cuddle with Toxikrab (no in-game effect)");
        type1 = "Water";
        type2 = "Plague";
    }

    public override void onStart()
    {
        foreach(OpalScript o in getOpalsInSameFlood())
        {
            o.takeDamage(o.getHealth(), false, true);
        }
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
