using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluttorch : OpalScript
{

    bool cinderWing = false;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 4;
        defense = 1;
        speed = 4;
        priority = 5;
        myName = "Fluttorch";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
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
        Attacks[0] = new Attack("Cinder Wings", 0, 1, 0, "<Free Ability>\nTake 5 damage. Tiles you move over this turn light on fire.");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Light the Way", 2, 4, 0, "Give an Opal +3 speed for 1 turn and Lift. All tiles adjacent and under them light on fire.");
        Attacks[2] = new Attack("Torch Fire", 2, 4, 5, "Give target lift and burning.");
        Attacks[3] = new Attack("Warmth Support",2,4,0, "Heal a target by 6. If the are lifted give them +3 attack. If they are burning they take burn damage.");
        type1 = "Fire";
        type2 = "Air";

    }

    public override void onStart()
    {
        cinderWing = false;
    }

    public override void onMove(PathScript p)
    {
        if (cinderWing)
        {
            boardScript.setTile((int)p.getPos().x, (int)p.getPos().z, "Fire", false);
            currentTile = boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z];
        }
        //boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z].standingOn(null);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            takeDamage(5, false, true);
            cinderWing = true;
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            target.doTempBuff(2, 1, 3);
            target.setLifted(true);
            boardScript.setTile(target, "Fire", false);
            foreach(TileScript t in target.getSurroundingTiles(true))
            {
                boardScript.setTile(t, "Fire", false);
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            target.setBurning(true);
            target.setLifted(true);
        }
        else if (attackNum == 3) //Grass Cover
        {
            target.doHeal(6, false);
            if (target.getLifted())
                target.doTempBuff(0, -1, 3);
            if (target.getBurning())
            {
                target.takeBurnDamage(false);
            }
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
        else if (attackNum == 3) //Grass Cover
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
            if (target.currentPlayer.getBurning())
                return target.currentPlayer.getBurningDamage();
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
