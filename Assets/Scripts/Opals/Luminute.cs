using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luminute : OpalScript
{
    bool didMove;
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 2;
        priority = 0;
        myName = "Luminute";
        //baseSize = new Vector3(0.2f, 0.2f, 1);
        transform.localScale = new Vector3(0.2f, 0.24f, 1) * 0.8f;
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
        Attacks[0] = new Attack("Group Aura", 6, 4, 6, "Each adjacent Opal with lower speed than Luminite adds 6 to this ability's damage.");
        Attacks[1] = new Attack("Nap", 1, 1, 0, "If Luminute didn't move this turn, overheal target and Luminute 8 health. Luminute loses 2 speed for 1 turn.");
        Attacks[2] = new Attack("Distracting Orb", 0, 1, 0, "Adjacent Opals are healed 10 health, they take -2 speed for 1 turn.", 1);
        Attacks[3] = new Attack("Poppy Breath", 0, 5, 0, "Overheal a target on any Growth by 10 health, they lose -4 speed for 1 turn.");
        type1 = "Light";
        type2 = "Grass";
    }

    public override void onStart()
    {
        didMove = false;
    }

    public override void onMove(int distanceMoved)
    {
        didMove = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {
            int addon = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                        {
                            if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.getSpeed() < getSpeed())
                                addon += 6;
                        }
                    }
                }
            }
            return cA.getBaseDamage() + getAttack() + addon;
        }
        else if (attackNum == 1) //Restore
        {
            if (didMove == false)
            {
                target.doHeal(8, true);
                doHeal(8, true);
                doTempBuff(2, 2, -2);
            }
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            if (target.getPos() != getPos())
            {
                target.doHeal(10, false);
                target.doTempBuff(2, 1, -2);
            }
            return 0;
        }else if(attackNum == 3)
        {
            target.doHeal(10, true);
            target.doTempBuff(2, 1, -4);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 0)
        {
            int addon = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                        {
                            if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.getSpeed() < getSpeed())
                                addon += 6;
                        }
                    }
                }
            }
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + addon;
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
}
