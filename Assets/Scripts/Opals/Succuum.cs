using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succuum : OpalScript {

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 4;
        priority = 1;
        myName = "Succuum";
        transform.localScale = new Vector3(3f, 3f, 1)*1.25f;
        animFrames = 3;
        offsetX = 0;
        offsetY = 0.15f;
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
        Attacks[0] = new Attack("Suck Essence", 0, 0, 0, "<Passive>\nEnemy Opals that start their turn surrounding Succuum lose -3 attack and -1 speed, until they start their turn away.");
        Attacks[1] = new Attack("Vaccuum Breath", 3, 4, 0, "Pull target Opal to Succuum. Tiles the target moves over, and under Succuum, become Flood.",0, 3);
        Attacks[2] = new Attack("Empowered Deluge", 8, 1, 0, "For each enemy Opal surrounding Succuum, place a Flood. Tidal: And place Flood on adjacent tiles.",0,3);
        Attacks[2].setTidalD("For each enemy Opal surrounding Succuum, place a Flood, and Flood adjacent tiles. Tidal: Adjacent tiles are not Flooded.");
        Attacks[3] = new Attack("Deep Breathing", 0, 1, 0, "Heal Succuum 4 health. Place Flood on tiles surrounding and under Succuum.",0,3);
        type1 = "Water";
        type2 = "Air";
        og = true;

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Crowded", 1, 5), new Behave("Line-Up", 1, 5), new Behave("Ally", 1, 2),
            new Behave("Safety", 0,1) });
    }

    private void adjustDeluge()
    {
        int nearbyOpals = 0;
        foreach(TileScript t in getSurroundingTiles(false))
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != getTeam())
                nearbyOpals++;
        }
        Attacks[2].setUses(nearbyOpals);
    }

    public override void onStart()
    {
        adjustDeluge();
    }

    public override void onMove(int distanceMoved)
    {
        adjustDeluge();
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if(attackNum == 0) //Suck Essence
        {
            
        }
        else if(attackNum == 1) //Healing Breath
        {
            string direct = "right";
            int dist = (int)getPos().x - (int)target.getPos().x;
            if (dist == 0) {
                direct = "up";
                dist = (int)getPos().z - (int)target.getPos().z;
            }
            if(dist < 0)
            {
                if(direct == "right")
                {
                    direct = "left";
                }
                else if (direct == "up")
                {
                    direct = "down";
                }
                dist = Mathf.Abs(dist);
            }
            for (int i = 0; i < dist + 1; i++)
            {
                if (direct == "right")
                {
                    getBoard().setTile((int)getPos().x - i, (int)getPos().z, "Flood", false);
                }
                else if (direct == "left")
                {
                    getBoard().setTile((int)getPos().x + i, (int)getPos().z, "Flood", false);
                }
                else if (direct == "up")
                {
                    getBoard().setTile((int)getPos().x, (int)getPos().z - i, "Flood", false);
                }
                else if (direct == "down")
                {
                    getBoard().setTile((int)getPos().x, (int)getPos().z + i, "Flood", false);
                }
            }
            if (direct == "right")
            {
                target.nudge(4, true, true);
            }
            else if (direct == "left")
            {
                target.nudge(4, true, false);
            }
            else if (direct == "up")
            {
                target.nudge(4, false, true);
            }
            else if (direct == "down")
            {
                target.nudge(4, false, false);
            }

            boardScript.setTile(this, "Flood", false);
         
            return 0;
        }
        else if(attackNum == 2) //Deep Breathing
        {
            if (getTidal())
            {
                foreach (TileScript t in target.getSurroundingTiles(true))
                {
                    boardScript.setTile(t, "Flood", false);
                }
            }

            boardScript.setTile(target, "Flood", false);
            
            return 0;
        }
        else if (attackNum == 3)
        {
            doHeal(4, false);
            boardScript.setTile(target, "Flood", false);
            foreach (TileScript t in getSurroundingTiles(false))
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
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
        }
        else if (attackNum == 2)
        {
            
            if (getTidal())
            {
                foreach (TileScript t in target.getSurroundingTiles(true))
                {
                    boardScript.setTile(t, "Flood", false);
                }
            }

            boardScript.setTile(target, "Flood", false);
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
        if (attackNum == 2)
        {
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            return false;
        }
        else if (atNum == 1 && !boardScript.myCursor.getFollowUp())
        {
            if(targettingEnemy(target) && notAdjacent(target))
                return true;
        }
        else if (atNum == 2)
        {
            int result = 0;
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != getTeam())
                    result++;
            }
            if (result > 1 && target.currentPlayer != null)
                return true;
            
        }
        else if (atNum == 3 && !boardScript.myCursor.getFollowUp())
        {
            int result = 0;
            foreach (TileScript t in getSurroundingTiles(false))
            {
                if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != getTeam())
                    result++;
            }
            if (result <= 1 && !checkHasLineOfSightNotAdjacent(getMaxRange()))
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 3;
    }
}
