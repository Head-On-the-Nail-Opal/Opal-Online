using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abysmeel : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 4;
        defense = 2;
        speed = 2;
        priority = 8;
        myName = "Abysmeel";
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
        Attacks[0] = new Attack("Crevice Creeper", 0, 0, 0, "<Passive>\n While in Flood, at the start of your turn gain +2 defense and +1 speed for each adjacent Boulder. It lasts for 2 turns.");
        Attacks[1] = new Attack("Sharpen Teeth", 1, 3, 0, "<Water Rush>\n If the target is a Boulder, gain +1 attack. Use this twice.");
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Toothy Maw", 2, 1, 6, "If the target has less than half health, gain +1 attack.");
        Attacks[3] = new Attack("Sea Pillar",3, 1, 0, "Place two Boulders. If you place them in Flood, they will not sink. If you place them out of Flood, adjacent tiles turn to Flood.");
        Attacks[3].setUses(2);
        type1 = "Water";
        type2 = "Ground";
    }

    public override void onStart()
    {
        if(currentTile != null && currentTile.type == "Flood")
        {
            foreach (TileScript t in getSurroundingTiles(true))
            {
                if(t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
                {
                    doTempBuff(1, 2, 2);
                    doTempBuff(2, 2, 1);
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
            if(target.getMyName() == "Boulder")
            {
                doTempBuff(0, -1, 1);
            }
        }
        else if (attackNum == 2)
        {
            if(target.getHealth() < target.getMaxHealth() / 2)
            {
                doTempBuff(0, -1, 1);
            }
        }
        else if (attackNum == 3)
        {
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
            bool notInFlood = false;
            if(target.type != "Flood")
            {
                notInFlood = true;
            }
            int x = (int)target.getPos().x;
            int y = (int)target.getPos().z;
            boardScript.setTile(target, "Boulder", false);
            if(boardScript.tileGrid[x,y].currentPlayer == null)
            {
                boardScript.setTile(boardScript.tileGrid[x, y], "Boulder", false);
            }else if (notInFlood)
            {
                foreach(TileScript t in boardScript.tileGrid[x, y].currentPlayer.getSurroundingTiles(true))
                {
                    boardScript.setTile(t, "Flood", false);
                }
            }
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
            if(target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder")
                return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 2;
        }
        else if (attackNum == 2)
        {
            if (target.currentPlayer != null && target.currentPlayer.getHealth() < target.currentPlayer.getMaxHealth() / 2)
            {
                return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 2;
            }
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 || attackNum == 2)
        {
            if (target.currentPlayer != null)
                return 1;
        }else if(attackNum == 3)
        {
            if (target.currentPlayer == null)
                return 1;
        }
        return -1;
    }
}
