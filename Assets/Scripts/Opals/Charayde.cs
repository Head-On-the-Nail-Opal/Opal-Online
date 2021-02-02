using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charayde : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 2;
        speed = 3;
        priority = 3;
        myName = "Charayde";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Deluge", 0, 0, 0, "<Passive>\n Place flood tiles down wherever you move.");
        Attacks[1] = new Attack("Sap Ray", 4, 4, 4, "Target loses 2 speed for 1 turn, Charayde gains +2 speed for 1 turn.");
        Attacks[2] = new Attack("Spinal Blade", 1, 3, 10, "<Water Rush>\n May move after attacking");
        Attacks[3] = new Attack("Sacrifice", 0, 1, 0, "Gain +1 speed, take 5 damage. Place Flood under your feet and on adjacent tiles.");
        type1 = "Water";
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
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
    }

    public override void onMove(PathScript p)
    {
        boardScript.setTile((int)p.getPos().x, (int)p.getPos().z, "Flood", false);
        currentTile = boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z];
        //boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z].standingOn(null);
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
            doTempBuff(2, 2, 2);
            target.doTempBuff(2, 1, -2);
        }
        else if (attackNum == 2) //Grass Cover
        {
        }else if(attackNum == 3)
        {
            doTempBuff(2, -1, 1);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Flood", false);
            takeDamage(5, false, true);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 2)
        {
        }else if(attackNum == 3)
        {
            return 5;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
