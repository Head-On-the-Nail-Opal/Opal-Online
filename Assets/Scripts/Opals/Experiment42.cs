using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment42 : OpalScript
{
    int currentUpgrade = 2;
    //(10, 0, 0, 3, "Experiment42", 0.7f, 0, 0, 0, "Blue", "Metal", "Plague");
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 3;
        priority = 2;
        myName = "Experiment42";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        offsetX = 0;
        offsetY = 0;
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
        Attacks[0] = new Attack("Upgrade", 1, 1, 6, "Gain +2 attack and +2 defense. If this was your previous attack too, gain +3 attack and +3 defense instead.");
        Attacks[1] = new Attack("Optimize", 0, 1, 0, "Gain +2 attack and +3 speed for the next turn, and spawn a miasma under your feet.");
        Attacks[2] = new Attack("Sick Shot", 2, 4, 3, "Deal 3 damage and poison target. If target is already poisoned, gain +4 attack.");
        Attacks[3] = new Attack("Influx",1,1,0,"Remove a poison from a target, or a miasma from underfoot, and gain +1 armor.");
        type1 = "Metal";
        type2 = "Plague";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Upgrade
        {
            doTempBuff(0, -1, currentUpgrade);
            doTempBuff(1, -1, currentUpgrade);
            currentUpgrade = 3;
            return cA.getBaseDamage() + getAttack() -(currentUpgrade);
        }
        else if (attackNum == 1) //Optimize
        {
            doTempBuff(2, 2, 3);
            doTempBuff(0, 2, 2);
            getBoard().setTile((int)getPos().x, (int)getPos().z, "Miasma", false);
            currentUpgrade = 2;
            return cA.getBaseDamage() - getAttack();
        }
        else if (attackNum == 2) //Sick Shot
        {
            if (target.getPoison())
            {
                doTempBuff(0, -1, 4);
                currentUpgrade = 2;
                return cA.getBaseDamage() + getAttack() - 4;
            }
            target.setPoison(true);
        }
        else if (attackNum == 3) //Sick Shot
        {
            if (target.getPoison())
            {
                target.setPoison(false);
                addArmor(1);
            }
            if(currentTile != null && currentTile.type == "Miasma")
            {
                addArmor(1);
                boardScript.setTile(this, "Grass", true);
            }
            currentUpgrade = 2;
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
