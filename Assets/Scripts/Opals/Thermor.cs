using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermor : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 1;
        defense = 4;
        speed = 2;
        priority = 1;
        myName = "Thermor";
        transform.localScale = new Vector3(3f, 3f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Protected", 0, 0, 0, "<Passive>\n At the start of your turn, gain +1 Armor.");
        Attacks[1] = new Attack("Thermal Goo", 2, 4, 0, "Burn a target. Increase burn damage for each Armor you have.",0,3);
        Attacks[2] = new Attack("Hot Iron", 2, 4, 2, "Gain +4 attack before dealing damage. Thermor is hit by this attack as well.",0,3);
        Attacks[3] = new Attack("Cooling", 0, 1, 0, "Gain +2 defense and lose -2 attack. Take damage based on your attack stat.",0,3);
        type1 = "Metal";
        type2 = "Fire";
    }

    public override void onStart()
    {
        addArmor(1);
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
            target.setBurning(true);
            target.setBurningDamage(target.getBurningDamage() + getArmor());
        }
        else if (attackNum == 2) //Grass Cover
        {
            doTempBuff(0, -1, 4);
            takeDamage(getAttack() + cA.getBaseDamage(), true, true);
        }else if(attackNum == 3)
        {
            doTempBuff(1, -1, 2);
            doTempBuff(0, -1, -2);
            return getAttack();
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 4;
        }else if(attackNum == 3)
        {
            return getAttack() - 2;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
