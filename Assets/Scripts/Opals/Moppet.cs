using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moppet : OpalScript
{
    private int damageLastTurn = 0;

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 2;
        priority = 4;
        myName = "Moppet";
        transform.localScale = new Vector3(2.5f, 2.5f, 1);
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
        Attacks[0] = new Attack("Sufferer's Bond", 0, 0, 0, "<Passive>\nMoppet takes burn damage and poison damage instead of Opals cursed by it.");
        Attacks[1] = new Attack("Chain of Torment", 2, 1, 0, "Overheal an Opal by 4 and curse it.");
        Attacks[2] = new Attack("Pain Share", 0, 1, 0, "Enemy Opals cursed by Moppet take the damage Moppet took since its last turn.");
        Attacks[3] = new Attack("Masochism", 0, 1, 0, "Heal Moppet by 8 health. If Moppet is at full health, heal ally Opals cursed by Moppet instead.");
        type1 = "Spirit";
        type2 = "Light";
    }

    public override void onDamage(int dam)
    {
        damageLastTurn += dam;
    }

    public override void onEnd()
    {
        damageLastTurn = 0;
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
            target.doHeal(4, true);
            target.setCursed(this);
            return 0;
        }
        else if (attackNum == 2)
        {
            foreach(OpalScript o in cursed)
            {
                if(o.getTeam() != getTeam())
                {
                    o.takeDamage(damageLastTurn, true, true);
                }
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            if(health == maxHealth)
            {
                foreach(OpalScript o in cursed)
                {
                    if(o.getTeam() == getTeam())
                    {
                        o.doHeal(6, false);
                    }
                }
            }
            else
            {
                doHeal(6, false);
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
            return damageLastTurn;
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1)
        {
            if(target.currentPlayer != null)
            {
                return 1;
            }
            return 0;
        }
        return 1;
    }
}
