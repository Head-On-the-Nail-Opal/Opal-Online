using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tortquoise : OpalScript
{
    TileScript thisTurn = null;
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 5;
        speed = 2;
        priority = 0;
        myName = "Tortquoise";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
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
        Attacks[0] = new Attack("Hidey Shell", 0, 0, 0, "<Passive>\n Gain +10 defense for a turn, at the end of your turn, if you did not move this turn.");
        Attacks[1] = new Attack("Rocky Retreat", 1, 1, 5, "Move 1 tile in the opposite direction and place three Boulders between you and target. They have +5 defense.");
        Attacks[2] = new Attack("Throw Stones", 3, 1, 0, "Place 2 Boulders within range.");
        Attacks[2].setUses(2);
        Attacks[3] = new Attack("Crush", 1, 1, 10, "Place Boulders on tiles without Opals in area of attack.", 1);
        type1 = "Ground";
        type2 = "Ground";
    }

    public override void onStart()
    {
        thisTurn = currentTile;
    }

    public override void onEnd()
    {
        if(currentTile == thisTurn)
        {
            doTempBuff(1, 1, 10);
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
            TileScript cT = currentTile;
            string direct = "right";
            int dist = (int)getPos().x - (int)target.getPos().x;
            if (dist == 0)
            {
                direct = "up";
                dist = (int)getPos().z - (int)target.getPos().z;
            }
            if (dist < 0)
            {
                if (direct == "right")
                {
                    direct = "left";
                }
                else if (direct == "up")
                {
                    direct = "down";
                }
                dist = Mathf.Abs(dist);
            }
            TileScript temp0 = null;
            TileScript temp1 = null;
            if (direct == "right")
            {
                nudge(1, true, true);
                temp0 = boardScript.setTile((int)cT.getPos().x, (int)cT.getPos().z+1, "Boulder", false);
                temp1 = boardScript.setTile((int)cT.getPos().x, (int)cT.getPos().z - 1, "Boulder", false);
            }
            else if (direct == "left")
            {
                nudge(1, true, false);
                temp0 = boardScript.setTile((int)cT.getPos().x, (int)cT.getPos().z + 1, "Boulder", false);
                temp1 = boardScript.setTile((int)cT.getPos().x, (int)cT.getPos().z - 1, "Boulder", false);
            }
            else if (direct == "up")
            {
                nudge(1, false, true);
                temp0 = boardScript.setTile((int)cT.getPos().x+1, (int)cT.getPos().z, "Boulder", false);
                temp1 = boardScript.setTile((int)cT.getPos().x-1, (int)cT.getPos().z, "Boulder", false);
            }
            else if (direct == "down")
            {
                nudge(1, false, false);
                temp0  = boardScript.setTile((int)cT.getPos().x+1, (int)cT.getPos().z, "Boulder", false);
                temp1 = boardScript.setTile((int)cT.getPos().x-1, (int)cT.getPos().z, "Boulder", false);
         
            }
            if (temp0 != null && temp0.currentPlayer.getMyName() == "Boulder")
            {
                temp0.currentPlayer.doTempBuff(1, 1, 5);
            }
            if (temp1 != null && temp1.currentPlayer.getMyName() == "Boulder")
            {
                temp1.currentPlayer.doTempBuff(1, 1, 5);
            }
            boardScript.setTile(cT, "Boulder", false);
            if(cT.currentPlayer.getMyName() == "Boulder")
            {
                cT.currentPlayer.doTempBuff(1, 1, 5);
            }
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
           if(target == this)
            {
                return 0;
            }
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
            boardScript.setTile(target, "Boulder", false);
            return 0;
        }
        else if (attackNum == 3)
        {
            boardScript.setTile(target, "Boulder", false);
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
            return 0;
        }
        else if (attackNum == 3)
        {
           
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 3 || attackNum == 2)
        {
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
