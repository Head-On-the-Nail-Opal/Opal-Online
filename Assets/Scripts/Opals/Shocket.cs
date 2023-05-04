using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shocket : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 4;
        priority = 8;
        myName = "Shocket";
        transform.localScale = new Vector3(3, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Powered Up", 0, 0, 0, "<Passive>\n Whenever Shocket is buffed or debuffed, it gains charge equal to the amount of stat change.");
        Attacks[1] = new Attack("Jolt", 4, 4, 0, "<Free Ability>\n Costs 1 charge. Target loses -1 speed for 1 turn.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Power Trip", 0, 1, 0, "Shocket gains +2 attack and defense for each adjacent buffed Opal",0,3);
        Attacks[3] = new Attack("Short Circuit", 3, 4, 4, "If target is buffed then they lose -3 speed for 1 turn.",0,3);
        type1 = "Electric";
        type2 = "Metal";
    }

    public override void onBuff(TempBuff buff)
    {
        doCharge(Mathf.Abs(buff.getAmount()));
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
            if (getCharge() > 0)
            {
                doCharge(-1);
                target.doTempBuff(2, 1, -1);
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if(t.currentPlayer != null && t.currentPlayer.isBuffed())
                {
                    target.doTempBuff(0, -1, 2);
                    target.doTempBuff(1, -1, 2);
                }

            }
            return 0;
        }
        else if(attackNum == 3)
        {
            if(target.isBuffed())
                target.doTempBuff(2, -1, -3);
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
            return 0;
        }else if(attackNum == 3)
        {
            
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1 && getCharge() < 1)
        {
            return -1;
        }
        if (target.currentPlayer == null)
        {
            return -1;
        }
        return 1;
    }
}
