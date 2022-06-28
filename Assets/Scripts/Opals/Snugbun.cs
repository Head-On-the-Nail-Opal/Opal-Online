using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snugbun : OpalScript
{
    private Vector3 panicPos = new Vector3(-100,-100,-100);
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 0;
        speed = 3;
        priority = 2;
        myName = "Snugbun";
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
        offsetY = -0.2f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Group Snuggles", 0, 0, 0, "<Passive>\n When Snugbun takes damage, or is healed, that damage or healing is also dealt to Opals adjacent to Snugbun.");
        Attacks[1] = new Attack("Bunny Hop", 3, 1, 0, "<Free Ability>\n Teleport to the target tile, then take 4 damage.");
        Attacks[2] = new Attack("Escape Plan", 3, 1, 0, "Choose a tile. After the next time you take damage (not on your turn), teleport to that tile.");
        Attacks[3] = new Attack("Heavy Cuddles", 1, 1, 4, "Deal damage, target loses -3 defense before the damage is dealt.");
        Attacks[1].setFreeAction(true);
        type1 = "Dark";
        type2 = "Dark";
    }

    public override void onDamage(int dam)
    {
        if (dam > 0 && !getDead())
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if(t.currentPlayer != null)
                {
                    t.currentPlayer.takeDamage(dam, true, true);
                }
            }

            if (panicPos != new Vector3(-100, -100, -100))
            {
                if (boardScript.tileGrid[(int)panicPos.x, (int)panicPos.z].currentPlayer == null)
                {
                    teleport((int)panicPos.x, (int)panicPos.z, 0);
                    panicPos = new Vector3(-100, -100, -100);
                }
            }
        }
    }

    public override void onHeal(int heal)
    {
        if(heal > 0)
        {
            foreach (TileScript t in getSurroundingTiles(true))
            {
                if (t.currentPlayer != null)
                {
                    t.currentPlayer.doHeal(heal,false);
                }
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
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            target.doTempBuff(1, -1, -3);
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
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
            takeDamage(4, false, true);
            return 0;
        }
        else if (attackNum == 2)
        {
            panicPos = new Vector3(target.getPos().x, 0, target.getPos().z);
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
            return 4;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 3;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 || attackNum == 2)
        {
            if(target.currentPlayer != null)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        return base.checkCanAttack(target, attackNum);
    }
}
