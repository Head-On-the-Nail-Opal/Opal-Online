using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoviron : OpalScript
{
    private CursorScript cs;

    private bool usedBlastOff = false;
    private int blastOffAttack = 0;
    private int blastOffSpeed = 0;
    private bool blastOffDamage = false;
    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 3;
        defense = 3;
        speed = 3;
        priority = 1;
        myName = "Hoviron";
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
        Attacks[0] = new Attack("Blast Off", 20, 1, 0, "<Free Ability>\nFly to any tile on the map. May use once per turn.");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Rocket Shot", 1, 1, 4, "Push target back two tiles. When you use Blast Off in the future, gain +2 attack for 1 turn.");
        Attacks[2] = new Attack("Firepower", 0, 1, 0, "Gain +2 attack and +2 defense. Next time you use Blast Off, push Opals adjacent to your landing back two tiles and deal 2 damage to them.");
        Attacks[3] = new Attack("Evasive Rockets", 0, 1, 0, "Gain +1 armor. When you use Blast Off in the future gain +1 speed for 2 turns.");
        type1 = "Metal";
        type2 = "Air";
        if (pl != null)
        {
            cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        }
    }

    public override void onStart()
    {
        usedBlastOff = false;
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
            pushAway(2, target);
            blastOffAttack += 2;
        }
        else if (attackNum == 2) //Grass Cover
        {
            doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 2);
            blastOffDamage = true;
            return 0;
        }
        else if (attackNum == 3) //Seed Launch
        {
            addArmor(1);
            blastOffSpeed += 1;
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            if (usedBlastOff == false)
            {
                doMove((int)target.getPos().x, (int)target.getPos().z, 0);
                if(blastOffAttack > 0)
                {
                    doTempBuff(0, 1, 2);
                }
                if(blastOffSpeed > 0)
                {
                    doTempBuff(2, 2, 1);
                }
                if (blastOffDamage)
                {
                    foreach(TileScript t in getSurroundingTiles(true))
                    {
                        if(t.currentPlayer != null)
                        {
                            pushAway(2, t.currentPlayer);
                            t.currentPlayer.takeDamage(2 + getAttack(), true, true);
                        }
                    }
                    blastOffDamage = false;
                }
            }
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
            if (blastOffDamage)
                return 2 + getAttack() + blastOffAttack;
            return 0;
        }
        else if (attackNum == 1)
        {
            
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
        if(attackNum == 0 && target.currentPlayer == null && usedBlastOff == false)
        {
            return 0;
        }
        if(attackNum == 1 && target.currentPlayer != null)
        {
            return 0;
        }
        if(attackNum == 2 || attackNum == 3)
        {
            return 0;
        }
        return -1;
    }
}
