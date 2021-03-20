using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barriarray : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 2;
        priority = 4;
        myName = "Barriarray";
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
        Attacks[0] = new Attack("Group Cover", 0, 0, 0, "<Passive>\n When any Opal surrounding Barriarray takes damage, Barriarray takes that damage instead.");
        Attacks[1] = new Attack("Shielding", 1, 1, 0, "Heal an Opal 5 health. If they are at full health give them +1 Armor.");
        Attacks[2] = new Attack("Morale Recovery", 0, 1, 0, "For each surrounding Opal at full health, gain 4 health.");
        Attacks[3] = new Attack("Defensive Burst", 0, 1, 0, "Gain +30 defense for 1 turn, and -15 defense for 3 turns.");
        type1 = "Light";
        type2 = "Metal";
    }

    public override void onStart()
    {
        int temp = 0;
        foreach(TileScript t in getSurroundingTiles(false))
        {
            if(t.getCurrentOpal() != null && t.getCurrentOpal().getHealth() >= t.getCurrentOpal().getMaxHealth())
            {
                temp++;
            }
        }
        Attacks[2] = new Attack("Morale Recovery", 0, 1, 0, "For each surrounding Opal at full health, gain 4 health. Currently ("+temp*4+") health");
    }

    public override void onMove(int distanceMoved)
    {
        int temp = 0;
        foreach (TileScript t in getSurroundingTiles(false))
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getHealth() >= t.getCurrentOpal().getMaxHealth())
            {
                temp++;
            }
        }
        Attacks[2] = new Attack("Morale Recovery", 0, 1, 0, "For each surrounding Opal at full health, gain 4 health. Currently (" + temp*4 + ") health");
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
            if(target.getHealth() >= target.getMaxHealth())
            {
                target.addArmor(1);
            }
            else {
                target.doHeal(5, false);
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            int i = 0;
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if(t.getCurrentOpal() != null && t.getCurrentOpal().getHealth() >= t.getCurrentOpal().getMaxHealth())
                {
                    i++;
                }
            }
            doHeal(4 * i, false);
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(1, 1, 30);
            doTempBuff(1, 3, -15);
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
        return base.checkCanAttack(target, attackNum);
    }
}
