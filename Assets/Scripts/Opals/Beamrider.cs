using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beamrider : OpalScript
{
    int myAttack = 0;

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 4;
        defense = 1;
        speed = 2;
        priority = 3;
        myName = "Beamrider";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 1f;
        offsetX = 0;
        offsetY = 0.15f;
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
        Attacks[0] = new Attack("Energy Blast", 1, 6, 0, "Deal 0 damage to all targets in a line. Ignores line of sight.");
        Attacks[1] = new Attack("Warming Up", 0, 1, 0, "Gain +3 attack.");
        Attacks[2] = new Attack("Reboot", 0, 1, 0, "Gain +5 attack and +2 speed for 1 turn.");
        Attacks[3] = new Attack("Big Red Button", 1, 6, 0, "Deal 0 damage to all targets in a line. Ignores line of sight. Double bonuses from attack, then remove all stat bonuses");
        type1 = "Laser";
        type2 = "Laser";
    }

    public override void onStart()
    {
        myAttack = getAttack();
    }

    public override void onMove(int distanceMoved)
    {
        myAttack = getAttack();
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            return cA.getBaseDamage() + getAttack();
        }
        else if (attackNum == 1) //
        {
            doTempBuff(0, -1, 3);
            return 0;
        }
        else if (attackNum == 2) //
        {
            doTempBuff(0, 2, 5);
            doTempBuff(2, 2, 2);
            return 0;
        }
        else if (attackNum == 3) //
        {
            int atk = getAttack();
            clearAllBuffs();
            return cA.getBaseDamage() + myAttack*2;
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }else if (attackNum == 3)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack()*2 - target.currentPlayer.getDefense();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 || attackNum == 3)
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
