using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oozwl : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 4;
        priority = 7;
        myName = "Oozwl";
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
        Attacks[0] = new Attack("Horrific Aura", 0, 0, 0, "<Passive>\n Opals cursed by Oozwl take double stat debuffs from poison and do not heal poison over time.");
        Attacks[1] = new Attack("Soulsap", 0, 0, 0, "<Passive>\n When an Opal cursed by Oozwl dies from poison, fully heal Oowzl and gain +2 speed.");
        Attacks[2] = new Attack("Venomous Haunt", 2, 4, 0, "Poison and curse the target.",0,3);
        Attacks[3] = new Attack("Weakening Memory", 0, 1, 0, "All enemies cursed by Oozwl lose -2 attack and defense for one turn. If they are poisoned, it lasts forever.",0,3);
        type1 = "Plague";
        type2 = "Spirit";
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
            return 0;
        }
        else if (attackNum == 2)
        {
            target.setPoison(true);
            target.setCursed(this);
            return 0;
        }
        else if (attackNum == 3)
        {
            foreach(OpalScript o in cursed)
            {
                if(getTeam() != o.getTeam())
                {
                    if (o.getPoison())
                    {
                        o.doTempBuff(0, -1, -2);
                        o.doTempBuff(1, -1, -2);
                    }
                    else
                    {
                        o.doTempBuff(0, 1, -2);
                        o.doTempBuff(1, 1, -2);
                    }
                }
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
