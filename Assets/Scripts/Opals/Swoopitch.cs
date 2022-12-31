using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swoopitch : OpalScript
{
    int fu = 0;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 4;
        defense = 2;
        speed = 3;
        priority = 2;
        myName = "Swoopitch";
        transform.localScale = new Vector3(3f, 3f, 1)*1.1f;
        offsetX = 0;
        offsetY = 0;
        offsetZ = 0;
        player = pl;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Attacks[0] = new Attack("Favorable Terrain", 0, 0, 0, "<Passive>\nGain +1 speed (for 1 turn) for each Boulder you start your turn surrounded by.");
        Attacks[1] = new Attack("Catapult", 4, 1, 0, "Break an adjacent Boulder. Deal damage to an Opal within 4 tiles based on that Boulder's defense. Place Boulders adjacent to them with the same defense.",0,3);
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Rock Stack", 3, 1, 0, "Place a Boulder. It gains +5 defense for each Boulder adjacent to it on placement.",0,3);
        Attacks[3] = new Attack("Heavy Flight", 5, 1, 0, "Fly to a tile. Leave a boulder where you left with +5 defense.",0,3);
        type1 = "Air";
        type2 = "Ground";
        og = true;

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 1), new Behave("Boulder-Buddy", 1, 6),
            new Behave("Safety", 0,1) });
    }

    public override void onStart()
    {
        fu = -1;
        Attacks[1] = new Attack("Catapult", 1, 1, 0, "Break an adjacent Boulder. Deal damage to an Opal within 6 tiles based on that Boulder's defense. Surround them by Boulders with the same defense.",0,3);
        Attacks[1].setUses(2);
        foreach(TileScript t in getSurroundingTiles(false))
        {
            if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                doTempBuff(2, 1, 1);
            }
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Sky Drop
        {
        }
        else if (attackNum == 1) //Momentum
        {
            if(fu == -1)
            {
                if(target.getMyName() == "Boulder")
                {
                    if(target.getDefense() > -1)
                        fu = target.getDefense();
                    target.takeDamage(target.getHealth(), false, false);
                    Attacks[1] = new Attack("Catapult", 6, 1, 0, "Break an adjacent Boulder. Deal damage to an Opal within 6 tiles based on that Boulder's defense. Place Boulders adjacent to them with the same defense.",0,3);
                   // Attacks[1].setUses(2);
                }
            }
            else
            {
                target.takeDamage(fu + getAttack(), true, true);
                List<TileScript> tiles = target.getSurroundingTiles(true);
                foreach(TileScript t in tiles)
                {
                    TileScript temp = boardScript.setTile(t, "Boulder", false);
                    if(t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
                    {
                        t.currentPlayer.doTempBuff(1, -1, fu);
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Catapult
        {

        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Sky Drop
        {
        }
        else if (attackNum == 1) //Momentum
        {
            if(fu != -1)
            {
                boardScript.setTile(target, "Boulder", false);
                OpalScript temp = target.currentPlayer;
                if(temp != null)
                    temp.doTempBuff(1, -1, fu);
            }
            return 0;
        }
        else if (attackNum == 2) //Catapult
        {
            TileScript targ = boardScript.setTile(target, "Boulder", false);
            if(targ.getCurrentOpal() != null && targ.getCurrentOpal().getMyName() == "Boulder")
            {
                foreach(TileScript t in targ.getCurrentOpal().getSurroundingTiles(true))
                {
                    if(t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Boulder")
                    {
                        targ.currentPlayer.doTempBuff(1, -1, 5);
                    }
                }
            }
            return 0;
        }else if(attackNum == 3)
        {
            TileScript startingTile = currentTile;
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
            boardScript.setTile(startingTile, "Boulder", false);
            if (startingTile.getCurrentOpal() != null && startingTile.getCurrentOpal().getMyName() == "Boulder")
            {
                startingTile.currentPlayer.doTempBuff(1, -1, 5);
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
        if (attackNum == 1 && (fu == -1 && target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder"))
        {
            return 0;
        } else if (attackNum == 1 && (fu == -1 && (target.currentPlayer == null || target.currentPlayer.getMyName() != "Boulder")))
        {
            return -1;
        } else if (attackNum == 1 && (fu != -1 && target.currentPlayer != null)) {
            return 0;
        }
        if((attackNum == 2 || attackNum == 3) && target.currentPlayer != null)
        {
            return -1;
        }
        return 0;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            return false;
        }
        else if (atNum == 1)
        {
            if(fu == -1 && numNearbyOpals(currentTile, Attacks[1].getRange(), true) > 0)
            {
                return true;
            }
            else
            {
                if (targettingEnemy(target))
                {
                    return true;
                }
            }
        }
        else if (atNum == 2 && !boardScript.myCursor.getFollowUp())
        {
            if (useAdjacentToOpal(target, true) || useAdjacentToOpal(target, false))
                return true;
        }
        else if (atNum == 3 && !boardScript.myCursor.getFollowUp())
        {
            if(useAdjacentToOpal(currentTile, true) && !boardScript.myCursor.tileIsFalling((int)target.getPos().x, (int)target.getPos().z))
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 0;
    }
}
