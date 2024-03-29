﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasmcrawler : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 5;
        defense = 4;
        speed = 2;
        priority = 0;
        myName = "Chasmcrawler";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0.15f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Sealed Away", 0, 0, 0, "<Passive>\n Chasmcrawler begins the game with two layers of boulders surrounding it.");
        Attacks[1] = new Attack("Rampage", 3, 4, 4, "Deal damage to up to 5 Opals. Gain +1 attack for each Opal damaged. Cannot break boulders.", 1,3);
        Attacks[2] = new Attack("Stone Monument", 4, 4, 0, "Place a Boulder. It has +10 defense.",0,3);
        Attacks[3] = new Attack("Frustrated Slumber", 0, 1, 0, "Gain +2 attack. Lose -3 speed for 1 turn.",0,3);
        type1 = "Ground";
        type2 = "Dark";
    }

    public override void onPlacement()
    {
        for(int i = -2; i < 3; i++)
        {
            for(int j = -2; j < 3; j++)
            {
                if(Mathf.Abs(i) != 2 || Mathf.Abs(j) != 2)
                    getBoard().placeBoulder((int)getPos().x+i, (int)getPos().z+j, this);
            }
        }
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
            if (target != this && target.getMyName() != "Boulder")
            {
                doTempBuff(0, -1, 1);
                target.takeDamage(5 + getAttack(), true, true);
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }else if (attackNum == 3)
        {
            doTempBuff(0, -1, 2);
            doTempBuff(2, 2, -3);
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            TileScript t = boardScript.setTile(target, "Boulder", false);
            if(t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                t.currentPlayer.doTempBuff(1,-1,10);
            }
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
            return 5 + getAttack() - target.currentPlayer.getDefense();
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
        if (attackNum == 2)
        {
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
