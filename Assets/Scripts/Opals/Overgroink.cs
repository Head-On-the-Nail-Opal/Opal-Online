using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overgroink : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 0;
        myName = "Overgroink";
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
        Attacks[0] = new Attack("Nutritious", 0, 0, 0, "<Passive>\n At the start of its turn, if Overgroink is standing on a Growth, gain +2 attack and defense.");
        Attacks[1] = new Attack("Boink", 2, 1, 6, "Lose all your buffs. If standing on Growth, only lose -2 attack and defense.");
        Attacks[2] = new Attack("Transplant", 0, 1, 0, "Give adjacent Opals standing on Growths the buffs on Overgroink. Remove them from Overgroink. Clear surrounding Growths.");
        Attacks[3] = new Attack("Nurture", 0, 1, 0, "Place Growths under your feet and on adjacent tiles. Gain +1 attack and defense.");
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onStart()
    {
        if(currentTile != null && currentTile.type == "Growth")
        {
            TempBuff test = doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 2);
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
            int attackBuff = getAttack();
            if(currentTile != null && currentTile.type == "Growth")
            {
                doTempBuff(0, -1, -2);
                doTempBuff(1, -1, -2);
            }
            else
            {
                clearAllBuffs();
            }
            return cA.getBaseDamage() + attackBuff;
        }
        else if (attackNum == 2)
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if(t.type == "Growth" && t.getCurrentOpal() != null)
                {
                    foreach (TempBuff b in buffs)
                    {
                        if (b.getTurnlength() != 0)
                        {
                            t.getCurrentOpal().doTempBuff(b.getTargetStat(), b.getTurnlength(), b.getAmount());
                        }
                    }
                }
                if (t.type == "Growth" && t != currentTile)
                {
                    boardScript.setTile(t, "Grass", false);
                }
            }
            return 0;
        }else if(attackNum == 3)
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Growth", false);
            }
            boardScript.setTile(this, "Growth", false);
            doTempBuff(0, -1, 1);
            doTempBuff(1, -1, 1);
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
        }
        else if (attackNum == 2)
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
            
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (target != null && target.currentPlayer != null)
            return 1;
        return -1;
    }
}
