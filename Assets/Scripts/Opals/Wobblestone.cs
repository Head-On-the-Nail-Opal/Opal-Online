using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobblestone : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 2;
        defense = 4;
        speed = 2;
        priority = 1;
        myName = "Wobblestone";
        transform.localScale = new Vector3(0.3f, 0.3f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0.1f;
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
        Attacks[0] = new Attack("Cobbled", 0, 0, 0, "<Passive>\nWhen Wobblestone targets a Boulder with an ability it gains +5 defense and doesn't take damage.");
        Attacks[1] = new Attack("Rolling", 4, 4, 8, "Move Wobblestone adjacent to target. If the target is a Boulder, gain +3 attack.");
        Attacks[2] = new Attack("Meteor", 2, 4, 10, "Place a boulder and deal damage to targets adjacent to placed boulder.");
        Attacks[3] = new Attack("Landslide", 1, 4, 0, "Place boulders in the area of effect. They all have 1 health.", 1);
        type1 = "Ground";
        type2 = "Ground";
    }

    public override void onStart()
    {
        Attacks[1] = new Attack("Rolling", 4, 4, 0, "Move Wobblestone adjacent to target. Can target boulders.");
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Sky Drop
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            if (getPos().x == target.getPos().x)
            {
                if (getPos().z < target.getPos().z)
                {
                    nudge((int)target.getPos().z - (int)getPos().z, false, true);
                }
                else
                {
                    nudge(-(int)target.getPos().z + (int)getPos().z, false, false);
                }
            }
            else
            {
                if (getPos().x < target.getPos().x)
                {
                    nudge((int)target.getPos().x - (int)getPos().x, true, true);
                }
                else
                {
                    nudge(-(int)target.getPos().x + (int)getPos().x, true, false);
                }
            }
            if (target != null && target.getMyName() == "Boulder")
            {
                target.doTempBuff(1, -1, 5);
                target.doTempBuff(0, -1, 3);
            }
            else
            {
                target.takeDamage(8 + getAttack(), true, true);
            }
        }
        else if (attackNum == 2) //Catapult
        {
            return 0;
        }
        else if (attackNum == 3) //Catapult
        {
            return 0;
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
            if(target.type == "Boulder")
            {
                doTempBuff(0, -1, 2);
                if(getPos().x == target.getPos().x)
                {
                    if(getPos().z < target.getPos().z)
                    {
                        nudge((int)target.getPos().z - (int)getPos().z, false, true);
                    }
                    else
                    {
                        nudge(-(int)target.getPos().z + (int)getPos().z, false, false);
                    }
                }
                else
                {
                    if (getPos().x < target.getPos().x)
                    {
                        nudge((int)target.getPos().x - (int)getPos().x, true, true);
                    }
                    else
                    {
                        nudge(-(int)target.getPos().x + (int)getPos().x, true, false);
                    }
                }
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
            }
            return 0;
        }
        else if (attackNum == 2) //Catapult
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if (target.getPos().x + i < 10 && target.getPos().x + i > -1 && target.getPos().z + j < 10 && target.getPos().z + j > -1 && boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].currentPlayer != null)
                        {
                            if (target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder")
                            {
                                boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].currentPlayer.doTempBuff(1, -1, 5);
                            }
                            else
                            {
                                boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].currentPlayer.takeDamage(cA.getBaseDamage() + getAttack(), true, true);
                            }
                        }
                    }
                }
            }
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Boulder", false);
        }
        else if (attackNum == 3) //Catapult
        {
            boardScript.setTile(target, "Boulder", false);
            target.currentPlayer.takeDamage(4, false, false);
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
            return 8 + getAttack() - target.currentPlayer.getDefense();
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

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 && target.type == "Boulder")
        {
            return 0;
        }
        if ((attackNum == 2 || attackNum == 3) && target.currentPlayer == null)
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
