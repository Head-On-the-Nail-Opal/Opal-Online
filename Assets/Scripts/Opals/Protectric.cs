using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protectric : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 3;
        priority = 8;
        myName = "Protectric";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Conductor", 0, 0, 0, "<Passive>\n At the start of your turn, convert each Armor into 4 charge");
        Attacks[1] = new Attack("Charged Disc", 4, 4, 0, "<Free Ability>\n Costs 2 charge. Gain +1 attack and then deal damage.");
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Voltage Shield", 0, 1, 0, "<Free Ability>\n Costs 3 charge. Gain +1 Armor.");
        Attacks[2].setFreeAction(true);
        Attacks[3] = new Attack("Reserve Power", 0, 1, 0, "Gain +1 Armor.");
        type1 = "Metal";
        type2 = "Electric";
    }


    public override void onStart()
    {
        doCharge(getArmor()*4);
        addArmor(-getArmor());
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
            if (getCharge() > 1)
            {
                doCharge(-2);
                doTempBuff(0, -1, 1);
                return cA.getBaseDamage() + getAttack();
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if(getCharge() > 2)
            {
                doCharge(-3);
                addArmor(1);
            }
            return 0;
        }else if(attackNum == 3)
        {
            addArmor(1);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() + 1 - target.currentPlayer.getDefense();
            //return getArmor() - target.currentPlayer.getDefense();
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
        if (attackNum == 1 && getCharge() < 2)
        {
            return -1;
        }
        if (attackNum == 2 && getCharge() < 3)
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
