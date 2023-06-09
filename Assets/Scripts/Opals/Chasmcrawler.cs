using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasmcrawler : OpalScript
{
    private bool rampaging = true;
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 4;
        defense = 4;
        speed = 2;
        priority = 0;
        myName = "Chasmcrawler";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Sealed Away", 0, 0, 0, "<Passive>\nChasmcrawler begins the game with two layers of boulders surrounding it.");
        Attacks[1] = new Attack("Rampage", 1, 1, 1, "<Free Ability>\nTeleport to the opposite side of the target, leaving a Boulder behind where you started. If the opposite Tile is full already, this ability ends for this turn.", 0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Cave In", 1, 1, 0, "Place Boulders on the Tiles adjacent to the target.",0,3);
        Attacks[3] = new Attack("Frustrated Slumber", 0, 1, 0, "Gain +2 attack. Lose -3 speed for 1 turn.",0,3);
        type1 = "Ground";
        type2 = "Dark";
    }

    public override void onPlacement()
    {
        for(int i = -2; i < 3; i++)
        {
            for(int j = -2; j < 3; j++)
            {
                if(Mathf.Abs(i) != 2 || Mathf.Abs(j) != 2)
                    getBoard().placeBoulder((int)getPos().x+i, (int)getPos().z+j, this);
            }
        }
    }

    public override void onStart()
    {
        rampaging = true;
    }

    private TileScript getOppositeTile(OpalScript target)
    {
        int targetX = (int)getPos().x;
        int targetY = (int)getPos().z;
        if(getPos().x < target.getPos().x)
        {
            targetX = (int)target.getPos().x + 1;
        }else if(getPos().x > target.getPos().x)
        {
            targetX = (int)target.getPos().x - 1;
        }
        else if (getPos().z < target.getPos().z)
        {
            targetY = (int)target.getPos().z + 1;
        }
        else if (getPos().z > target.getPos().z)
        {
            targetY = (int)target.getPos().z - 1;
        }
        return checkTile(targetX, targetY);
    }

    private TileScript checkTile(int x, int y)
    {
        if(x > -1 && x < 10 && y > -1 && y < 10 && !boardScript.tileGrid[x, y].getImpassable())
        {
            return boardScript.tileGrid[x, y];
        }
        else
        {
            return null;
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
            if (rampaging)
            {
                TileScript pastTile = currentTile;
                TileScript nextTile = getOppositeTile(target);
                if (nextTile != null)
                {
                    doMove((int)nextTile.getPos().x, (int)nextTile.getPos().z, 0);
                    boardScript.setTile(pastTile, "Boulder", false);
                }
                else
                {
                    rampaging = false;
                }
            }
        }
        else if (attackNum == 2) //Grass Cover
        {
            foreach(TileScript t in target.getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Boulder", false);
            }
            return 0;
        }else if (attackNum == 3)
        {
            doTempBuff(0, -1, 2);
            doTempBuff(2, 2, -3);
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
        if(attackNum == 1 && !rampaging)
        {
            return -1;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
