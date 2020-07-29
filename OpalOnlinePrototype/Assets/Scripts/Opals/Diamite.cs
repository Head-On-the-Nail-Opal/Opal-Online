using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamite : OpalScript
{
    private List<OpalScript> oofed = new List<OpalScript>();
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 2;
        priority = 9;
        myName = "Diamite";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.5f;
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
        Attacks[0] = new Attack("Precious", 0, 0, 0, "<Passive>\nWhen Diamite takes damage, it loses all stat changes, and opponents regain stats drained by Diamite.");
        Attacks[1] = new Attack("Crystallizing Bite", 1, 1, 1, "Target loses -1 speed. Gain +1 speed. May move after using this.");
        Attacks[2] = new Attack("Chip Gem", 0, 1, 0, "Take 1 damage, then gain +2 armor.");
        Attacks[3] = new Attack("Crack", 0, 1, 0, "Lose an armor and gain +3 defense. Then take 1 damage.");
        type1 = "Dark";
        type2 = "Metal";
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            moveAfter = true;
        }
        else if (attackNum == 2)
        {
            moveAfter = false;
        }
    }


    public override void onDamage(int dam)
    {
        clearBuffs();
        foreach(OpalScript o in oofed)
        {
            o.doTempBuff(2, -1, 1);
        }
        oofed.Clear();
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
            target.doTempBuff(2, -1, -1);
            doTempBuff(2, -1, 1);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            takeDamage(1 + getDefense(), true, true);
            addArmor(2);
            return 0;
        }else if(attackNum == 3)
        {
            if(getArmor() > 0)
            {
                addArmor(-1);
                doTempBuff(1, -1, 3);
            }
            takeDamage(1 + getDefense(), true, true);
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
        }
        else if (attackNum == 2)
        {
            return 1;
        }
        else if (attackNum == 3)
        {
            return 1;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
