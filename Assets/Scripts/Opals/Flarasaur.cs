using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flarasaur : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 0;
        myName = "Flarasaur";
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
        Attacks[0] = new Attack("Ignite", 0, 0, 0, "<Passive>\nOn Flaresaur's death, all Opals cursed by it will have their tiles enflamed.");
        Attacks[1] = new Attack("Oil Spill", 3, 4, 0, "Curse all targets in the radius. They take damage from their burn.", 1);
        Attacks[2] = new Attack("Grave Flame", 0, 1, 0, "Burn all Opals cursed by Flaresaur.");
        Attacks[3] = new Attack("Engulf", 4, 4, 0, "Light the tiles surrounding and under the target on fire.");
        type1 = "Fire";
        type2 = "Spirit";
    }

    public override void onDie()
    {
        foreach(OpalScript o in cursed)
        {
            if (!o.getDead())
            {
                boardScript.setTile(o, "Fire", false);
            }
        }
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
            target.setCursed(this);
            if (target.getBurning())
            {
                target.takeBurnDamage(false);
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            foreach (OpalScript o in cursed)
            {
                if (!o.getBurning())
                {
                    o.setBurning(true);
                }
                else
                {
                    o.setBurning(true);
                }
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            boardScript.setTile(target, "Fire", false);
            foreach (TileScript t in target.getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Fire", false);
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
        if(attackNum == 1)
        {
            return 0;
        }
        return base.checkCanAttack(target, attackNum);
    }
}
