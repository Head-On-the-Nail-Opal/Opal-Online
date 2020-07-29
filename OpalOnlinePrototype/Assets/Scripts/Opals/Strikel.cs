using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strikel : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 2;
        priority = 6;
        myName = "Strikel";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        offsetX = 0;
        offsetY = 0.1f;
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
        Attacks[0] = new Attack("Sharpen", 0, 0, 0, "<Passive>\nWhen Strikel takes damage it gains +1 attack");
        Attacks[1] = new Attack("Overclock", 0, 1, 0, "Take 4 damage, gain +1 speed, then take 2 damage.");
        Attacks[2] = new Attack("Calculated Strike", 1, 1, 6, "If the target survives this attack, then gain +1 defense.");
        Attacks[3] = new Attack("Shattering Slash", 1, 1, 10, "Take 4 damage, then take 4 damage, then deal damage.");
        type1 = "Metal";
        type2 = "Metal";
    }

    

    public override void onDamage(int dam)
    {
        doTempBuff(0, -1, 1);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            return 0;
        }
        else if (attackNum == 1) //
        {
            takeDamage(4, false, true);
            doTempBuff(2, -1, 1);
            takeDamage(2, false, true);
            return 0;
        }
        else if (attackNum == 2) //
        {
            if(target.getHealth() + target.getDefense() > cA.getBaseDamage() + getAttack())
            {
                doTempBuff(1, -1, 1);
            }
        }
        else if (attackNum == 3) //
        {
            takeDamage(4, false, true);
            takeDamage(4, false, true);
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
        if(attackNum == 0)
        {
            return 0;
        }else if(attackNum == 1)
        {
            return 6;
        }else if(attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 3)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 2;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

}
