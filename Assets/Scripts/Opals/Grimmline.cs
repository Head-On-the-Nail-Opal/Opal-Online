using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimmline : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 1;
        maxHealth = health;
        attack = 5;
        defense = 2;
        speed = 3;
        priority = 0;
        myName = "Grimmline";
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
        Attacks[0] = new Attack("Feedback", 0, 0, 0, "<Passive>\nGrimmline takes 100 damage from targeting allied Opals.");
        Attacks[1] = new Attack("Lightning Rod", 0, 7, 0, "Target any Opal. Overheal by their Attack stat.");
        Attacks[2] = new Attack("Diminishing Strike", 0, 7, 0, "<Aftershock>\n Target any Opal. Reduce its attack and defense by 2. Costs 3 charge.");
        Attacks[3] = new Attack("Power Sap", 3, 4, 3, "Gain charge for the amount of the target's attack buffs.");
        type1 = "Electric";
        type2 = "Dark";
    }

    public override void onStart()
    {
        bannedAttacks.Clear();
        attackAgain = true;
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            attackAgain = false;
        }
        else if (attackNum == 1)
        {
            attackAgain = false;
        }
        else if (attackNum == 2)
        {
            attackAgain = true;
        }
        else if (attackNum == 3)
        {
            attackAgain = false;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if(target.getPlayer() == getPlayer())
        {
            takeDamage(100, false, true);
        }
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            doHeal(target.getAttack(), true);
            return 0;
        }
        else if (attackNum == 2)
        {
            if (getCharge() > 2)
            {
                doCharge(-3);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if (getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                target.doTempBuff(0, -1, -2);
                target.doTempBuff(1, -1, -2);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 3)
        {
            int i = target.getTempBuff(0);

            doCharge(i);
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
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
