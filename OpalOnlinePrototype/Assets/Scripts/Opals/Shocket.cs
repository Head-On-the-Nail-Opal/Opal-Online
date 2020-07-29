using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocket : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 4;
        priority = 8;
        myName = "Shocket";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Powered Up", 0, 0, 0, "<Passive>\n Whenever Shocket is buffed or debuffed, it gains charge equal to the amount of stat change.");
        Attacks[1] = new Attack("Jolt", 4, 1, 0, "<Aftershock>\nTarget loses -1 speed for 1 turn. Repeatable until Shocket is out of charge. Costs 1 charge.");
        Attacks[2] = new Attack("Short Circuit", 2, 4, 4, "If target is buffed then deal double damage, and Shocket gains +1 speed.");
        Attacks[3] = new Attack("Power Trip", 3, 4, 0, "If target is buffed then Shocket gains +3 attack for 1 turn.");
        type1 = "Electric";
        type2 = "Metal";
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
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            attackAgain = true;
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            attackAgain = false;
            moveAfter = false;
        }
    }

    public override void onBuff(TempBuff buff)
    {
        doCharge(Mathf.Abs(buff.getAmount()));
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if (getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                target.doTempBuff(2, 1, -1);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            foreach(TempBuff b in target.getBuffs())
            {
                if(b.getTurnlength() != 0 && b.getAmount() > 1)
                {
                    doTempBuff(2, -1, 1);
                    return cA.getBaseDamage() + getAttack() * 2;
                }
            }
        }else if(attackNum == 3)
        {
            foreach (TempBuff b in target.getBuffs())
            {
                if (b.getTurnlength() != 0 && b.getAmount() > 1)
                {
                    doTempBuff(3, -1, 1);
                    return 0;
                }
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {

        }
        else if (attackNum == 2) //Grass Cover
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
            foreach (TempBuff b in target.currentPlayer.getBuffs())
            {
                if (b.getTurnlength() != 0 && b.getAmount() > 1)
                {
                    return Attacks[attackNum].getBaseDamage() + getAttack() * 2;
                }
            }
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
