using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infernal : OpalScript
{

    Vector3 startingTile;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 4;
        defense = 0;
        speed = 3;
        priority = 6;
        myName = "Infernal";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
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
        Attacks[0] = new Attack("Cloven Hoof", 0, 0, 0, "<Passive>\n If Jetjinn starts its turn on a flame, double its speed.");
        Attacks[1] = new Attack("Flame Spear", 1, 1, 8, "Set target's tile on fire. May move after attacking.",0,3);
        Attacks[2] = new Attack("Hellish Portal", 1, 1, 2, "Teleport target to the tile you started this turn on.",0,3);
        Attacks[3] = new Attack("Palm Reading", 1, 1, 0, "Target loses attack stat equal to the amount of burn damage they are about to take.",0,3);
        type1 = "Fire";
        type2 = "Dark";
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
            moveAfter = false;
        }
    }

    public override void onStart()
    {
        if(currentTile != null && currentTile.type == "Fire")
        {
            doTempBuff(2, 1, getSpeed());
        }
        startingTile = getPos();
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
            boardScript.setTile(target, "Fire", false);
        }
        else if (attackNum == 2) //Grass Cover
        {
            if(boardScript.tileGrid[(int)startingTile.x, (int)startingTile.z].currentPlayer == null && !boardScript.tileGrid[(int)startingTile.x, (int)startingTile.z].getFallen())
                target.doMove((int)startingTile.x, (int)startingTile.z, 0);
        }else if(attackNum == 3)
        {
            if(target.getBurning())
                target.doTempBuff(0, -1, -(target.getBurningDamage()));
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

        }
        else if (attackNum == 2)
        {

        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
