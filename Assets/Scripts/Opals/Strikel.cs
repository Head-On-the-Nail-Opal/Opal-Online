using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strikel : OpalScript
{
    bool lostArmor = false;
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 4;
        defense = 1;
        speed = 4;
        priority = 6;
        myName = "Strikel";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Sharpen", 0, 1, 0, "<Free Ability>\nTake 2 damage. (Ignores Armor)");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Furious Blades", 1, 1, 0, "Deal damage, dealing more the lower health Strikel has.",0,3);
        Attacks[2] = new Attack("Synthesize", 0, 1, 0, "Gain +3 attack. If you lost armor since your last turn, gain +6 instead.",0,3);
        Attacks[3] = new Attack("Efficiency Boost", 0, 1, 0, "Take 5 damage and then gain +1 armor. If it has armor when Strikel uses this, gain +4 attack and +4 defense for 1 turn.",0,3);
        type1 = "Metal";
        type2 = "Metal";
    }

    public override void onEnd()
    {
        lostArmor = false;
    }

    public override void onArmorDamage(int dam)
    {
        lostArmor = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            takeDamageBelowArmor(2, false, true);
            return 0;
        }
        else if (attackNum == 1) //
        {
            return (maxHealth - getHealth()) + getAttack();
        }
        else if (attackNum == 2) //
        {
            if (lostArmor)
            {
                doTempBuff(0, -1, 6);
            }
            else
            {
                doTempBuff(0, -1, 3);
            }
            return 0;
        }
        else if (attackNum == 3) //
        {
            if(getArmor() > 0)
            {
                doTempBuff(0, 2, 4);
                doTempBuff(1, 2, 4);
            }
            takeDamage(5, false, true);
            addArmor(1);
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
        if(attackNum == 0)
        {
            return 0;
        }else if(attackNum == 1)
        {
            return (maxHealth - getHealth()) + getAttack() - target.currentPlayer.getDefense();
        }
        else if(attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            return 5;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

}
