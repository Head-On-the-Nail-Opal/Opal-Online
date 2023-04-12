using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirmtongue : OpalScript
{
    bool canUseAgain = true;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 3;
        priority = 8;
        myName = "Squirmtongue";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
        offsetX = 0;
        offsetY = 0f;
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
        Attacks[0] = new Attack("Topsoil", 0, 0, 0, "<Passive>\n When Squirmtongue damages a Boulder, it gains +1 attack.");
        Attacks[1] = new Attack("Wriggling Bite", 1, 1, 1, "<Free Ability>\n Deal 1 damage. If this ability is not targeting a Boulder, then you may not use it again this turn.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Double Dig", 1, 1, 0, "Place a Boulder adjacent to you. It gains defense equal to your current attack.",0,3);
        Attacks[3] = new Attack("Dirty Defense", 1, 4, 0, "Give other targets within the area of effect +1 defense for each point of attack on Squirmtongue. The defense buffs last for 1 turn.",1,3);
        type1 = "Ground";
        type2 = "Ground";
    }

    public override void onStart()
    {
        canUseAgain = true;
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
            if(target.getMyName() != "Boulder")
            {
                canUseAgain = false;
            }
            else
            {
                doTempBuff(0, -1, 1);
            }
        }
        else if (attackNum == 2) 
        {
            return 0;
        }
        else if (attackNum == 3) 
        {
            if(target != this)
            {
                target.doTempBuff(1,1, getAttack());
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

        }
        else if (attackNum == 1) 
        {
            
            return 0;
        }
        else if (attackNum == 2) 
        {
            boardScript.setTile(target, "Boulder", false);
            target.getCurrentOpal().doTempBuff(1, -1, getAttack());
            return 0;
        }
        else if (attackNum == 3) 
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
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 && !canUseAgain || (attackNum == 1 && target.currentPlayer == null))
        {
            return -1;
        }
        if(attackNum == 2 && target.currentPlayer != null)
        {
            return -1;
        }
        return 1;
    }
}
