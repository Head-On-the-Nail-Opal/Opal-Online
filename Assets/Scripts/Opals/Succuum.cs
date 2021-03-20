using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Succuum : OpalScript {

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 4;
        priority = 1;
        myName = "Succuum";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0.15f;
        offsetZ = 0;
        player = pl;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        Attacks[0] = new Attack("Exhale", 5, 4, 1, "Deal 1 damage and push target back one tile. Use this attack 2 times.");
        Attacks[0].setUses(2);
        Attacks[1] = new Attack("Vaccuum Breath", 4, 4, 0, "Heal a target 8 health and pull them to you. Tiles they move over become flooded.");
        Attacks[2] = new Attack("Deep Breathing", 0, 1, 8, "Gain +3 attack for 1 turn. Place Flood tiles on your surrounding tiles and at your feet. Targets on surrounding tiles gain Lift.");
        Attacks[3] = new Attack("Suck Essence", 1, 3, 5, "<Water Rush> Heal the damage you deal.");
        type1 = "Water";
        type2 = "Air";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if(attackNum == 0) //Suck Essence
        {
            pushAway(1, target);
        }
        else if(attackNum == 1) //Healing Breath
        {
            target.doHeal(8, false);
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
         
            return 0;
        }
        else if(attackNum == 2) //Deep Breathing
        {
            doTempBuff(0, 2, 3);
            foreach(TileScript t in getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Flood", false);
            }
            boardScript.setTile(this, "Flood", false);
            return 0;
        }
        else if (attackNum == 3)
        {
            doHeal(getAttackDamage(attackNum, target.getCurrentTile()), false);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 0)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
