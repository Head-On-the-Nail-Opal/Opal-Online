using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gritwit : OpalScript
{
    int lastAttack = 0;
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 4;
        priority = 3;
        myName = "Gritwit";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
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
        Attacks[0] = new Attack("Crag Advantage", 0, 0, 0, "<Passive>\nGritwit gains +1 defense (for 1 turn) at the start of its turn equal to the amount of Boulders adjacent to it..",0,3);
        Attacks[1] = new Attack("Pave", 1, 1, 0, "If your attack is greater than 0, place a Boulder and lose -1 attack.",0,3);
        Attacks[2] = new Attack("Reclaim", 1, 1, 0, "Target a Boulder. Gain +1 attack, and remove that Boulder.",0,3);
        Attacks[3] = new Attack("Shake It Off", 0, 1, 0, "Gain +1 attack and +1 speed.",0,3);
        type1 = "Ground";
        type2 = "Swarm";
    }

    public override void onStart()
    {
        foreach(TileScript t in boardScript.getSurroundingTiles(currentTile, true))
        {
            if(t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                doTempBuff(1, 1, 1);
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (target.getMyName() == "Boulder")
            {
                target.takeDamage(target.getHealth(), false, false);
                doTempBuff(0, -1, 1);
            }
            return 0;
        }
        else if(attackNum == 3)
        {
            doTempBuff(0, -1, 1);
            doTempBuff(2, -1, 1);
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
            if(getAttack() > 0)
            {
                boardScript.setTile(target, "Boulder", false);
                doTempBuff(0, -1, -1);
            }
            return 0;
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
        if (attackNum == 0)
        {
            return -1;
        }else if(attackNum == 2)
        {
            if (target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder")
                return 0;
        }else if (attackNum == 1)
        {
            if (target.getCurrentOpal() == null && getAttack() > 0)
                return 0;
            return -1;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
