using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shineode : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 5;
        speed = 2;
        priority = 4;
        myName = "Shineode";
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
        Attacks[0] = new Attack("Shatter Geode", 3, 1, 0, "Destroy a boulder. Deal damage based on its defense to adjacent Opals.");
        Attacks[1] = new Attack("Rock Drop", 4, 1, 0, "Place 3 boulders. They share your defense value.");
        Attacks[1].setUses(3);
        Attacks[2] = new Attack("Mineral Health", 2, 1, 0, "Heal a target by 5. If they're next to a boulder overheal.");
        Attacks[3] = new Attack("Rocky Fortitude", 0, 1, 0, "Surrounding Opals and boulders, and Shineode, gain +2 defense.");
        type1 = "Ground";
        type2 = "Light";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            if(target.getMyName() == "Boulder")
            {
                int dam = target.getDefense();
                foreach(TileScript t in target.getSurroundingTiles(true))
                {
                    if(t.currentPlayer != null && t.currentPlayer.getMyName() != "Boulder")
                    {
                        t.currentPlayer.takeDamage(dam, true, true);
                    }
                }
                target.takeDamage(target.getHealth(), false, false);
            }
            return 0;
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            bool nextTo = false;
            foreach (TileScript t in target.getSurroundingTiles(true))
            {
                if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
                {
                    nextTo = true;
                }
            }
            target.doHeal(5, nextTo);
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(1, -1, 2);
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if (t.currentPlayer != null)
                    t.currentPlayer.doTempBuff(1, -1, 2);
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
            TileScript t = boardScript.setTile(target, "Boulder", false);
            if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                t.currentPlayer.doTempBuff(1, -1, getDefense());
            }
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
            if(target.getCurrentOpal() != null && target.getCurrentOpal().getMyName() == "Boulder")
            {
                return target.getCurrentOpal().getDefense();
            }
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
        if(attackNum == 0 && target.getCurrentOpal() != null && target.getCurrentOpal().getMyName() == "Boulder")
        {
            return 1;
        }else if(attackNum == 1 && target.getCurrentOpal() == null)
        {
            return 1;
        }else if(attackNum == 2 || attackNum == 3 && target.getCurrentOpal() != null)
        {
            return 1;
        }
        return -1;
    }
}
