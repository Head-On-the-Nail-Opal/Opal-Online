using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Betary : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 1;
        priority = 8;
        myName = "Betary";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = -0.15f;
        offsetZ = 0;
        player = pl;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Attacks[0] = new Attack("Recharge", 0, 1, 0, "Gain +2 charge and +1 speed.");
        Attacks[1] = new Attack("Zap", 3, 4, 5, "<Aftershock>\nDeal 5 damage. Repeatable until Betary is out of charge. Costs 2 charge.");
        Attacks[2] = new Attack("Draw Power", 0, 1, 0, "<Aftershock>\nGain +2 attack for this turn. Costs 2 charge.");
        Attacks[3] = new Attack("Recycling Energy", 0, 1, 0, "Lose -1 speed and gain +3 charge.");
        type1 = "Electric";
        type2 = "Electric";
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
            attackAgain = true;
        }
        else if (attackNum == 2)
        {
            attackAgain = true;
        }else if (attackNum == 3)
        {
            attackAgain = false;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            doCharge(2);
            boardScript.setChargeDisplay(getCharge());
            doTempBuff(2, -1, 1);
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 1) //
        {
            if(getCharge() > 1)
            {
                doCharge(-2);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if(getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                return cA.getBaseDamage() + getAttack();
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 2) //
        {
            if (getCharge() > 1)
            {
                doCharge(-2);
                boardScript.setChargeDisplay(getCharge());
                doTempBuff(0, 1, 2);
                attackAgain = true;
                bannedAttacks.Add(attackNum);
            }
            return 0;
        }else if(attackNum == 3)
        {
            doTempBuff(2, -1, -1);
            doCharge(3);
            boardScript.setChargeDisplay(getCharge());
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {

        }
        else if (attackNum == 1) //
        {

        }
        else if (attackNum == 2) //
        {

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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
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
