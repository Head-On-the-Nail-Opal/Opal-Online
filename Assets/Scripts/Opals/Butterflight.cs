using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterflight : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 5;
        priority = 3;
        myName = "Butterflight";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
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
        Attacks[0] = new Attack("Pollinate", 1, 1, 0, "Buff a target's attack and defense by your attack stat, and heal them by your attack stat. Lose the amount of health you healed.");
        Attacks[1] = new Attack("Whirlwind", 0, 1, 0, "Gain +1 attack and 2 health for each adjacent Opal. Push them each three tiles away.");
        Attacks[2] = new Attack("Breeze", 1, 1, 0, "Buff a target's speed by your attack stat for 1 turn, and cure any status effects on yourself and them.");
        Attacks[3] = new Attack("Dwindle",1,1,0,"<Free Ability>\n Push a target away from you by 1 tile.");
        Attacks[3].setFreeAction(true);
        type1 = "Air";
        type2 = "Light";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Pollinate
        {
            target.doTempBuff(0, -1, getAttack());
            target.doTempBuff(1, -1, getAttack());
            int findHeal = target.getHealth();
            target.doHeal(getAttack(), false);
            findHeal = target.getHealth() - findHeal;
            takeDamage(findHeal, false, true);
            return 0;
        }
        else if (attackNum == 1) //Whirlwind
        {
            for(int i = -1; i < 2; i++)
            {
                for(int j = -1; j < 2; j++)
                {
                    if(!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if(getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                        {
                            doTempBuff(0, -1, 1);
                            doHeal(2, false);
                            if(i == 0 && j == -1)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(3, false, false);
                            if (i == 0 && j == 1)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(3, false, true);
                            if (i == 1 && j == 0)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(3, true, true);
                            if (i == -1 && j == 0)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(3, true, false);
                        }
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Refreshing Breeze
        {
            target.doTempBuff(2, 1, getAttack());
            target.healStatusEffects();
            healStatusEffects();
            return 0;
        }else if(attackNum == 3)
        {
            pushAway(1, target);
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
}
