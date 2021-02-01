using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightLite : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 2;
        priority = 7;
        myName = "KnightLight";
        transform.localScale = new Vector3(3.5f, 3.5f, 1);
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
        Attacks[0] = new Attack("Divine Touch", 0, 0, 0, "<Passive>\nOn the start of its turn, KnightLite gains attack equal to it's current overheal");
        Attacks[2] = new Attack("Blessing", 1, 1, 0, "Overheal a target by your current attack.");
        Attacks[1] = new Attack("Shield of Truth", 0, 1, 0, "<Free Ability>\nLose 5 health and gain an Armor");
        Attacks[1].setFreeAction(true);
        Attacks[3] = new Attack("Sword of Honor", 1, 1, 10, "Lose -1 armor. Overheal self by 6.");
        type1 = "Light";
        type2 = "Metal";
    }

    public override void onStart()
    {
        if(health > maxHealth)
            doTempBuff(0, 1, health-maxHealth);
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
            takeDamageBelowArmor(5, false, true);
            addArmor(1);
            return 0;
        }
        else if (attackNum == 2)
        {
            target.doHeal(getAttack(), true);
            return 0;
        }
        else if (attackNum == 3)
        {
            addArmor(-1);
            doHeal(5, true);
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
            return 0;
        }
        else if (attackNum == 3)
        {
            
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
