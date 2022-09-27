using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterflight : OpalScript
{

    private int useNum = 0;
    private bool xorz;
    private bool sign;
    private OpalScript pushOpal;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 5;
        priority = 3;
        myName = "Butterflight";
        transform.localScale = new Vector3(3f, 3f, 1) * 0.8f;
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
        Attacks[0] = new Attack("Pollinate", 1, 1, 0, "<Free Ability>\n Lose -2 defense and heal an Opal 4 health. Cannot use if below -5 defense.",0,3);
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Breeze", 3, 1, 0, "Select a target Opal. Then choose a direction to push them 4 tiles.",0,3);
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Wind Shield", 0, 1, 0, "Gain +3 defense and Lift.",0,3);
        Attacks[3] = new Attack("Sprint",1,1,0, "Buff a target's speed by your defense for 1 turn. They gain Lift.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Air";
        type2 = "Light";
        og = true;
    }

    public override void onStart()
    {
        useNum = 0;
        Attacks[1].setRange(3);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Pollinate
        {
            target.doHeal(4, false);
            return 0;
        }
        else if (attackNum == 1) //Whirlwind
        {
           if(useNum == 0)
            {
                useNum = 1;
                Attacks[1].setRange(1);
                pushOpal = target;
            }
            else
            {
                if (target.getPos().x == getPos().x)
                {
                    xorz = false;
                }
                else
                {
                    xorz = true;
                }

                if (xorz)
                {
                    if (target.getPos().x > getPos().x)
                    {
                        sign = true;
                    }
                    else
                    {
                        sign = false;
                    }
                }
                else
                {
                    if (target.getPos().z > getPos().z)
                    {
                        sign = true;
                    }
                    else
                    {
                        sign = false;
                    }
                }
                pushOpal.nudge(4, xorz,sign);
                useNum = 0;
            }
            return 0;
        }
        else if (attackNum == 2) //Refreshing Breeze
        {
            doTempBuff(1, -1, 3);
            setLifted(true);
            return 0;
        }else if(attackNum == 3)
        {
            target.doTempBuff(2, 1, getDefense());
            target.setLifted(true);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {

        }
        else if (attackNum == 1) //
        {
            if(useNum == 1)
            {
                if(target.getPos().x == getPos().x)
                {
                    xorz = false;
                }
                else
                {
                    xorz = true;
                }

                if (xorz)
                {
                    if (target.getPos().x > getPos().x)
                    {
                        sign = true;
                    }
                    else
                    {
                        sign = false;
                    }
                }
                else
                {
                    if (target.getPos().z > getPos().z)
                    {
                        sign = true;
                    }
                    else
                    {
                        sign = false;
                    }
                }
                pushOpal.nudge(4, xorz, sign);
                useNum = 0;
            }
        }
        else if (attackNum == 2) //
        {

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
        if (attackNum == 0) {
            if (getDefense() > -5)
                return 0;
            else
                return -1;
        }
        if (attackNum == 1)
            return 0;
        else if(target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
