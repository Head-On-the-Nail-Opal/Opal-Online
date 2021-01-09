using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquarican : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 2;
        speed = 3;
        priority = 9;
        myName = "Aquarican";
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
        Attacks[0] = new Attack("Water Bowl", 0, 0, 0, "<Passive>\n On the start of its turn, if it's attack is less than 1, Aquarican takes 10 damage.");
        Attacks[1] = new Attack("Refill", 0, 1, 0, "Remove the Flood tile at your feet and gain +6 attack.");
        Attacks[2] = new Attack("Spout Snipe", 6, 4, 5, "Lose -4 attack after using this attack.");
        Attacks[3] = new Attack("Refresh", 1, 3, 0, "<Water Rush>\n Give target Opal +4 attack and +1 speed for 1 turn. Gain +4 attack for 1 turn. Place a Flood at your feet.");
        type1 = "Water";
        type2 = "Air";
    }

    public override void onStart()
    {
        if(getAttack() < 1)
        {
            takeDamage(10, false, true);
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
            if(currentTile.type == "Flood")
            {
                boardScript.setTile(target, "Grass", true);
                doTempBuff( 0, -1, 6);
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            doTempBuff(0, -1, -4);
            return cA.getBaseDamage() + getAttack() + 4;
        }
        else if (attackNum == 3)
        {
            target.doTempBuff(0, 1, 4);
            target.doTempBuff(2, 1, 1);
            doTempBuff(0, 1, 4);
            boardScript.setTile(this, "Flood", false);
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

        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
