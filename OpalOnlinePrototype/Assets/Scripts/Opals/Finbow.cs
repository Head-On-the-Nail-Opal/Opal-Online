using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finbow : OpalScript
{
    bool first;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 2;
        priority = 2;
        myName = "Finbow";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Rainbow Beam", 1, 6, 0, "Place flood down on all tiles in a line. Lose -4 speed for 2 turns.");
        Attacks[1] = new Attack("Nullifying Strike", 1, 3, 6, "<Water Rush>\nTarget loses 2 attack and 2 defense for 2 turns.");
        Attacks[2] = new Attack("Rev", 0, 1, 0, "Gain +3 speed for 3 turns.");
        Attacks[3] = new Attack("Splash Paddle", 1, 3, 0, "Move to any tile in your current pool of Floods. Lose -5 defense.");
        type1 = "Water";
        type2 = "Laser";
    }

    public override void adjustProjectile(Projectile p, int attackNum)
    {
        if(attackNum == 0)
        {
            p.doRainbow();
        }
        return;
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            first = true;
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            moveAfter = false;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            if (first)
            {
                doTempBuff(2, 3, -2);
                first = false;
            }
            boardScript.setTile((int)target.getCurrentTile().getPos().x, (int)target.getCurrentTile().getPos().z, "Flood", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z, "Flood", false);
            //doTempBuff(2, 2, -2);
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            doTempBuff(0, 2, -2);
            doTempBuff(1, 2, -2);
            return cA.getBaseDamage() + getAttack() - 2;
        }
        else if (attackNum == 2) //Grass Cover
        {
            doTempBuff(2, 4, 3);
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            if (first)
            {
                doTempBuff(2, 3, -2);
                first = false;
            }
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Flood", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z, "Flood", false);
            //doTempBuff(2, 2, -2);
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {

        }
        else if (attackNum == 2) //Grass Cover
        {

            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
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
        if (attackNum == 0)
        {
            return 0;
        }
        if(attackNum == 3 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 3)
        {
            return 0;
        }
        return -1;
    }
}
