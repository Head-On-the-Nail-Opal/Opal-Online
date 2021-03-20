using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hopscure : OpalScript
{
    int fu = 0;
    TileScript temp;
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 2;
        defense = 1;
        speed = 4;
        priority = 3;
        myName = "Hopscure";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.4f;
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
        Attacks[0] = new Attack("Blink", 0, 0, 0, "<Passive>\nHopscure teleports directly to it's destination when moving.");
        Attacks[1] = new Attack("Warp Hop", 3, 1, 0, "<Free Ability>\nSelect a tile to create a portal, then select a tile to become the destination. Lose -2 defense for 1 turn. May move after using this.");
        Attacks[1].setFreeAction(true);
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Thump", 1, 1, 6, "Target is knocked back 2 tiles. May move after attacking.");
        Attacks[3] = new Attack("Lifted Bite", 1, 1, 4, "If target has Lift then they take double damage. Otherwise they gain Lift.");
        type1 = "Dark";
        type2 = "Air";
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            moveAfter = true;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
    }

    public override void onStart()
    {
        fu = 0;
    }

    public override void onFollowUp(int attacking)
    {
        //fu = 1;
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
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
                target.nudge(2, true, false);
            }
            else if (direct == "left")
            {
                target.nudge(2, true, true);
            }
            else if (direct == "up")
            {
                target.nudge(2, false, false);
            }
            else if (direct == "down")
            {
                target.nudge(2, false, true);
            }
        }
        else if (attackNum == 3) //Seed Launch
        {
            if (target.getLifted())
            {
                return (cA.getBaseDamage() + getAttack()) * 2;
            }
            else
            {
                target.setLifted(true);
            }
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
            if(fu == 1)
            {
                //getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
                getBoard().protSetTrap((int)target.getPos().x, (int)target.getPos().z, "PortalOut");
                temp.setLink(getBoard().tileGrid[(int)target.getPos().x, (int)target.getPos().z]);
                fu = 0;
                doTempBuff(1, 1, -2);
                return 0;
            }
            //getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            getBoard().protSetTrap((int)target.getPos().x, (int)target.getPos().z, "PortalIn");
            temp = getBoard().tileGrid[(int)target.getPos().x, (int)target.getPos().z];
            fu = 1;
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
            return 0;
        }
        else if (attackNum == 2)
        {

        }
        else if (attackNum == 3)
        {
            if (target.currentPlayer.getLifted())
            {
                return (Attacks[attackNum].getBaseDamage() + getAttack()) * 2;
            }
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 1)
        {
            return 0;
        }
        return -1;
    }
}
