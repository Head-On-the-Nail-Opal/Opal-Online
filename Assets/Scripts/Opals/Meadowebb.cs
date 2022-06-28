using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meadowebb : OpalScript
{

    private int healAmount = 0;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 9;
        myName = "Meadowebb";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.2f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Grassy Lair", 0, 0, 0, "<Passive>\nWhile Meadowebb is on Growth, if an Opal enters a surrounding tile, Meadowebb uses Munch on them.");
        Attacks[1] = new Attack("Munch", 1, 1, 6, "Deal damage and place a growth tile at the feet of the target. If they're already on Growth, deal damage again.");
        Attacks[2] = new Attack("Striking Stance", 0, 1, 0, "Gain +4 attack, +4 defense and -1 speed for 1 turn. Place a Growth underneath Meadowebb.");
        Attacks[3] = new Attack("Gather Dew", 0, 1, 0, "Consume all Growths surrounding Meadowebb, then after the next time you take damage heal 2 health for each. ("+0+")");
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onDamage(int dam)
    {
        if(healAmount > 0)
        {
            doHeal(healAmount, false);
            healAmount = 0;
            Attacks[3] = new Attack("Gather Dew", 0, 1, 0, "Consume all Growths surrounding Meadowebb, then after the next time you take damage heal 2 health for each. (" + 0 + ")");
        }
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
            if(target.getCurrentTile() != null && target.getCurrentTile().type == "Growth")
            {
                target.takeDamage(cA.getBaseDamage() + getAttack(), true, true);
            }
            else
            {
                boardScript.setTile(target, "Growth", false);
            }
            return cA.getBaseDamage() + getAttack();
        }
        else if (attackNum == 2) //Grass Cover
        {
            doTempBuff(0, 2, 4);
            doTempBuff(1, 2, 4);
            doTempBuff(2, 2, -1);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }else if (attackNum == 3)
        {
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if(t.type == "Growth")
                {
                    healAmount += 2;
                    boardScript.setTile(t, "Grass", true);
                }
            }
            Attacks[3] = new Attack("Gather Dew", 0, 1, 0, "Consume all Growths surrounding Meadowebb, then after the next time you take damage heal 2 health for each. (" + healAmount + ")");
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
            return 0;
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
            int output = Attacks[attackNum].getBaseDamage() + getAttack(); 
            if (target != null && target.type == "Growth")
            {
                output += Attacks[attackNum].getBaseDamage() + getAttack();
            }
            return output;
        }
        else if (attackNum == 2)
        {
            return 0;
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
