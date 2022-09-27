using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charayde : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 3;
        defense = 2;
        speed = 3;
        priority = 3;
        myName = "Charayde";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Rain Wings", 0, 0, 0, "<Passive>\n Place Flood tiles down wherever you move.");
        Attacks[1] = new Attack("Merciless Ray", 3, 4, 5, "Deal damage at range. Target loses all temporary buffs. Their current permanent buffs now last 2 turns. Charayde takes 5 damage from this ability.",0,3);
        Attacks[2] = new Attack("Spinal Blade", 1, 3, 8, "<Water Rush>\nDeal damage, if the target is unbuffed then also heal 4 health.",0,3);
        Attacks[3] = new Attack("Glide", 0, 1, 0, "Gain +1 speed and place Flood on adjacent tiles. Tidal: Gain +2 speed instead.",0,3);
        Attacks[3].setTidalD("Gain +3 speed and place Flood on adjacent tiles. Tidal: Gain +1 speed instead.");
        type1 = "Water";
        type2 = "Dark";
    }

    public override void onMove(int x, int y)
    {
        boardScript.setTile(x, y, "Flood", false);
        currentTile = boardScript.tileGrid[x, y];
        StartCoroutine(playFrame("attack", 3));
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
            target.doBrutalDebuff();
            takeDamage(5, false, true);
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (!target.isBuffed())
                doHeal(4, false);
        }else if(attackNum == 3)
        {
            if (getTidal())
                doTempBuff(2, -1, 2);
            else
                doTempBuff(2, -1, 1);
            foreach(TileScript t in getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Flood", false);
            }
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + target.currentPlayer.getTempTempBuff(1);
        }
        else if (attackNum == 2)
        {

        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
