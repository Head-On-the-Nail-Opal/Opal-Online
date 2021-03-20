using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grimmline : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
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
        Attacks[0] = new Attack("Power Sap", 3, 4, 0, "<Free Ability>\nCosts 2 charge. Reduce the target by 2 and gain the stat loss. Steal up to 1 charge.");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Thieving Swipe", 2, 4, 2, "<Free Ability>\nCosts 1 charge. Steal up to 2 charge and then deal damage.");
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Plot Demise", 0, 1, 0, "Gain +1 charge, +1 attack, and +1 speed.");
        Attacks[3] = new Attack("Incoming Doom", 2, 4, 100, "Costs 10 charge. Deal 100 damage.");
        type1 = "Electric";
        type2 = "Dark";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            if (getCharge() > 1)
            {
                doCharge(-2);
                doTempBuffFromReduce(target.reduce(2));
                if(target.getCharge() > 0)
                {
                    target.doCharge(-1);
                    doCharge(1);
                }
            }
            return 0;
        }
        else if (attackNum == 1)
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                if (target.getCharge() > 0)
                {
                    if (target.getCharge() == 1)
                    {
                        target.doCharge(-1);
                        doCharge(1);
                    }
                    else
                    {
                        target.doCharge(-2);
                        doCharge(2);
                    }
                }
                return cA.getBaseDamage() + getAttack();
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            doCharge(1);
            doTempBuff(0, -1, 1);
            doTempBuff(2, -1, 1);
            return 0;
        }
        else if (attackNum == 3)
        {
            if (getCharge() > 9)
            {
                doCharge(-10);
                return cA.getBaseDamage() + getAttack();
            }
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

        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 && getCharge() < 2)
        {
            return -1;
        }
        if (attackNum == 1 && getCharge() < 1)
        {
            return -1;
        }
        if (attackNum == 3 && getCharge() < 10)
        {
            return -1;
        }
        if (target.currentPlayer == null)
        {
            return -1;
        }
        return 1;
    }
}
