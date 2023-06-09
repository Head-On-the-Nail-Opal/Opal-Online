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
        transform.localScale = new Vector3(3f, 3f, 1);
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
        Attacks[0] = new Attack("Crystal Healing", 3, 1, 0, "Destroy a Boulder, overheal Opals adjacent to Shineode by that Boulder's defense.",0,3);
        Attacks[1] = new Attack("Flash Shatter", 2, 1, 8, "Destroy a Boulder, and deal damage to all adjacent Opals. If a Boulder is destroyed by this ability, it also deals damage to all adjacent Opals.",0,3);
        Attacks[2] = new Attack("Geode Drop", 5, 1, 0, "Place two Boulders, they share your defense.",0,3);
        Attacks[2].setUses(2);
        Attacks[3] = new Attack("Rocky Fortitude", 1, 1, 0, "Gain +2 defense. Give an adjacent Opal +2 defense.",0,3);
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
                int heal = target.getDefense();
                foreach(TileScript t in getSurroundingTiles(true))
                {
                    if(t.currentPlayer != null && t.currentPlayer.getMyName() != "Boulder")
                    {
                        t.currentPlayer.doHeal(heal, true);
                    }
                }
                target.takeDamage(target.getHealth(), false, false);
            }
            return 0;
        }
        else if (attackNum == 1)
        {
            if (target.getMyName() == "Boulder")
            {
                StartCoroutine(shatter(cA.getBaseDamage() + getAttack(), target));
                target.summonNewParticle("Shatter");
                target.takeDamage(cA.getBaseDamage() + getAttack(), true, false);
            }
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
            target.doTempBuff(1, -1, 2);
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
            TileScript t = boardScript.setTile(target, "Boulder", false);
            if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                t.currentPlayer.doTempBuff(1, -1, getDefense());
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    private IEnumerator shatter(int damage, OpalScript target)
    {
        boardScript.myCursor.setAnimating(true);
        if (target != null && !target.getDead())
        {
            foreach(TileScript t in target.getSurroundingTiles(true))
            {
                if(t.getCurrentOpal() != null)
                {
                    if(t.getCurrentOpal().getMyName() == "Boulder")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                       
                        StartCoroutine(shatter(damage, t.getCurrentOpal()));
                        if (t.getCurrentOpal() != null)
                        {
                            t.getCurrentOpal().summonNewParticle("Shatter");
                            t.getCurrentOpal().takeDamage(damage, true, false);
                        }
                    }
                    else
                    {
                        t.getCurrentOpal().summonNewParticle("Shatter");
                        t.getCurrentOpal().takeDamage(damage, true, true);
                    }
                }
            }
        }
        boardScript.myCursor.setAnimating(false);
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
        }else if(attackNum == 1 && target.getCurrentOpal() != null && target.getCurrentOpal().getMyName() == "Boulder")
        {
            return 1;
        }else if(attackNum == 3 && target.getCurrentOpal() != null)
        {
            return 1;
        }else if (attackNum == 2 && target.getCurrentOpal() == null)
        {
            return 1;
        }
        return -1;
    }
}
