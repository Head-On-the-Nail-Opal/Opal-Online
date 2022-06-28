using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gilsplish : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 0;
        myName = "Gilsplish";
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
        Attacks[0] = new Attack("Gills", 0, 0, 0, "<Passive>\n At the start of its turn, Gilsplish loses -10 health outside of Flood, or gains +5 health in Flood");
        Attacks[1] = new Attack("Splish", 1, 3, 5, "<Water Rush> Deal damage, if Gilsplish is at full health, deal damage twice.");
        Attacks[2] = new Attack("Splurt", 3, 4, 6, "Deal damage to the target. Tiles under and adjacent to them are Flooded. Tidal: This has +2 range, and you heal the damage it deals.");
        Attacks[2].setTidalD("Deal damage to the target. Tiles under and adjacent to them are Flooded. You heal the damage it deals. Tidal: This has -2 range, and you don't heal");
        Attacks[3] = new Attack("Flop", 0, 1, 0, "Gain +2 speed for 1 turn. Place Flood under Gilsplish and on adjacent tiles.");
        type1 = "Water";
        type2 = "Water";
    }

    public override void onStart()
    {
        base.onStart();
        if(currentTile != null && currentTile.type == "Flood")
        {
            doHeal(5, false);
        }
        else
        {
            takeDamage(10, false, true);
        }
        if (getTidal())
        {
            Attacks[2].setRange(5);
        }
        else
        {
            Attacks[2].setRange(3);
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
            if(getHealth() == getMaxHealth())
                target.takeDamage(cA.getBaseDamage() + getAttack(), true, true);
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Flood", false);
            foreach (TileScript t in target.getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Flood", false);
            }
            if (getTidal())
            {
                if(target.getArmor() == 0)
                    doHeal(cA.getBaseDamage() + getAttack() - target.getDefense(), false);
            }
        }
        else if (attackNum == 3)
        {
            doTempBuff(2, 2, 2);
            boardScript.setTile(this, "Flood", false);
            foreach(TileScript t in getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Flood", false);
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
            if(getHealth() == getMaxHealth())
                return (Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense())*2;
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
}
