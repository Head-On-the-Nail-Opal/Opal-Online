using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mistery : OpalScript
{
    private int mistUses = 0;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 4;
        priority = 1;
        myName = "Mistery";
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
        Attacks[0] = new Attack("Blanket Fog", 0, 0, 0, "<Passive>\nWhen Mistery dies, all Opals cursed by it lose -4 speed for 1 turn.");
        Attacks[1] = new Attack("Misty Misery", 2, 1, 0, "<Free Ability>\nTwice per turn, place a mist trap, which deal 14 damage to Opals cursed by Mistery",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Condensed Grip", 3, 4, 6, "Curse the target Opal",0,3);
        Attacks[3] = new Attack("Two Tone Screech", 0, 1, 0, "All cursed enemy Opals lose -2 defense. All cursed ally Opals gain +4 defense.",0,3);
        type1 = "Spirit";
        type2 = "Dark";
    }

    public override void onStart()
    {
        mistUses = 0;
    }

    public override void onDie()
    {
        foreach(OpalScript o in cursed)
        {
            doTempBuff(2, 1, -4);
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
            return 0;
        }
        else if (attackNum == 2)
        {
            target.setCursed(this);
        }
        else if (attackNum == 3)
        {
            foreach (OpalScript o in cursed)
            {
                if(o.getTeam() != this.getTeam())
                    doTempBuff(1, -1, -2);
                else
                    doTempBuff(1, -1, 4);
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
            if (mistUses < 2)
            {
                target.setTrap("Mist");
                mistUses++;
            }
            return 0;
        }
        else if (attackNum == 2)
        {
           
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
            if (target.currentPlayer == null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        return base.checkCanAttack(target, attackNum);
    }
}
