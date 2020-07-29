using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechalodon : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 2;
        priority = 2;
        myName = "Mechalodon";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        offsetX = 0;
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Fish Outta", 0, 0, 0, "<Passive>\nLose all armor and stat changes when moving outside of Flood");
        Attacks[1] = new Attack("Submerge", 0, 1, 0, "Gain +1 armor, +2 attack and +1 speed.");
        Attacks[2] = new Attack("Gnash", 1, 3, 4, "<Water Rush>\nDeal 4 damage to an Opal. Add damage for each point of Armor you have.");
        Attacks[3] = new Attack("Teary Flop", 1, 1, 0,"Place a Flood on target and under self.");
        type1 = "Water";
        type2 = "Metal";
        og = true;
    }

    public override void onMove(PathScript p)
    {
        if(boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z] != null && boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z].type != "Flood")
        {
            clearBuffs();
            addArmor(-getArmor());
        }
    }

    public override void onMove(int distanceMoved)
    {
        if (boardScript.tileGrid[(int)getPos().x, (int)getPos().z] != null && boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type != "Flood")
        {
            clearBuffs();
            addArmor(-getArmor());
        }
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Ravage
        {

            return cA.getBaseDamage() + getAttack() + (int)(Mathf.Abs(getPos().x - target.getPos().x) + Mathf.Abs(getPos().z - target.getPos().z))-2;
        }
        else if (attackNum == 1) //Submerge
        {
            if (currentTile.type == "Flood")
            {
                addArmor(1);
                doTempBuff(0, -1, 2);
                doTempBuff(2, -1, 1);
            }
            return 0;
        }
        else if (attackNum == 2) //Gnash
        {
            return cA.getBaseDamage() + getAttack() + getArmor();
        }
        else if (attackNum == 3) //Gnash
        {
            boardScript.setTile(target, "Flood", false);
            boardScript.setTile(this, "Flood", false);
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

        }
        else if (attackNum == 2) //
        {

        }
        else if (attackNum == 3) //Gnash
        {
            boardScript.setTile(target, "Flood", false);
            boardScript.setTile(this, "Flood", false);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + getArmor();
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
