﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Betary : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 1;
        priority = 8;
        myName = "Betary";
        transform.localScale = new Vector3(3f, 3f, 1);
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
        Attacks[0] = new Attack("Zap", 3, 4, 5, "<Free Ability>\nCosts 2 charge. Deal 5 damage.",0,3);
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Draw Power", 3, 4, 5, "<Free Ability>\n Costs 1 charge. If target is buffed then they are Reduced by 1 and Betary gains the stat loss.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Recharge", 0, 1, 0, "Costs 1 charge. Gain +3 charge and +1 speed.",0,3);
        Attacks[3] = new Attack("Jump", 0, 1, 0, "Gain +1 charge.",0,3);
        type1 = "Electric";
        type2 = "Electric";

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 3), new Behave("Line-Up", 1, 5),
            new Behave("Safety", 0,1) });
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            if(getCharge() > 1)
            {
                doCharge(-2);
                return cA.getBaseDamage() + getAttack();
            }
            return 0;
        }
        else if (attackNum == 1) //
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                doTempBuffFromReduce(target.reduce(1));
            }
            return 0;
        }
        else if (attackNum == 2) //
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                doCharge(3);
                doTempBuff(2, -1, 1);
            }
            return 0;
        }else if(attackNum == 3)
        {
            doCharge(1);
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

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 0 && getCharge() < 2)
        {
            return -1;
        }
        if (attackNum == 1 && getCharge() < 1)
        {
            return -1;
        }
        if (attackNum == 2 && getCharge() < 1)
        {
            return -1;
        }
        if(target.currentPlayer == null)
        {
            return -1;
        }
        return 1;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            if (getCharge() >= 2 && targettingEnemy(target))
                return true;
        }
        else if (atNum == 1)
        {
            if (getCharge() >= 1 && targettingEnemy(target) && target.currentPlayer.isBuffed())
                return true;
        }
        else if (atNum == 2)
        {
            if(getCharge() >= 1)
                return true;
        }
        else if (atNum == 3)
        {
            if(getCharge() < 1)
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 3;
    }
}
