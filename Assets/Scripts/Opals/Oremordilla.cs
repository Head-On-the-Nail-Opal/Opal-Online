using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oremordilla : OpalScript
{
    protected bool movingLeft = true;
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 4;
        myName = "Oremordilla";
        transform.localScale = new Vector3(3f, 3f, 1) * 0.9f;
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
        Attacks[1] = new Attack("Plan Ahead", 0, 1, 0, "Adjust Tumble to turn right instead of left.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[0] = new Attack("Tumble", 5, 4, 7, "Roll into a target to damage them. After hitting a target, turn to your left and repeat this ability.",0,3);
        Attacks[0].setInstant(true);
        Attacks[2] = new Attack("Natural Deposits", 5, 1, 0, "Place 3 Boulders.",0,3);
        Attacks[2].setUses(3);
        Attacks[3] = new Attack("Armor Plating", 0, 1, 0, "Gain +2 defense and 1 Armor.",0,3);
        type1 = "Ground";
        type2 = "Metal";
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
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Boulder", false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 1)
        {
            movingLeft = !movingLeft;
            string str = "right";
            string str1 = "left";
            if (movingLeft)
            {
                str = "left";
                str1 = "right";
            }
            Attacks[0].setDescription("Roll into a target to damage them. After hitting a target, turn to your "+str+" and repeat this ability");
            Attacks[1].setDescription("Adjust Tumble to turn "+str1+" instead of " + str);
            return 0;
        }
        else if (attackNum == 0)
        {
            string direct = "right";
            int dist = (int)getPos().x - (int)target.getPos().x;
            if (dist == 0)
            {
                direct = "up";
                dist = (int)getPos().z - (int)target.getPos().z;
            }
            if (dist < 0)
            {
                if (direct == "right")
                {
                    direct = "left";
                }
                else if (direct == "up")
                {
                    direct = "down";
                }
                dist = Mathf.Abs(dist);
            }
            if (direct == "right")
            {
                oremordillaIndex = 0;
                if (!movingLeft)
                    oremordillaIndex = 1;
                nudge(10, true, false);
            }
            else if (direct == "left")
            {
                oremordillaIndex = 0;
                if (!movingLeft)
                    oremordillaIndex = 1;
                nudge(10, true, true);
            }
            else if (direct == "up")
            {
                oremordillaIndex = 1;
                if (!movingLeft)
                    oremordillaIndex = 0;
                nudge(10, false, false);
            }
            else if (direct == "down")
            {
                oremordillaIndex = 1;
                if (!movingLeft)
                    oremordillaIndex = 0;
                nudge(10, false, true);
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(1, -1, 2);
            addArmor(1);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 0)
        {
            
        }
        else if (attackNum == 2)
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
}
