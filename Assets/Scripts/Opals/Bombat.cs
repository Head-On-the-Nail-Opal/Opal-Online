using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombat : OpalScript
{

    private bool noEarly = true;
    private bool catchRepeat = false;
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = 10;
        attack = 8;
        defense = 3;
        speed = 0;
        priority = 0;
        myName = "Bombat";
        transform.localScale = new Vector3(0.3f, 0.3f, 1) * 0.8f;
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
        Attacks[0] = new Attack("Short Fuse", 0, 0, 0, "<Passive>\n At the end of your turn, lose 5 health.");
        Attacks[1] = new Attack("Sulfurious", 0, 0, 0, "<Passive>\n On death, explode dealing damage equal to your attack stat to Opals on surrounding tiles.");
        Attacks[2] = new Attack("Screech", 0, 1, 0, "Gain +5 attack.");
        Attacks[3] = new Attack("Tiny Wings", 0, 1, 0, "Gain +5 health and +2 speed for 1 turn");
        type1 = "Air";
        type2 = "Swarm";
    }

    public override void onEnd()
    {
        if (noEarly)
        {
            noEarly = false;
        }
        else
        {
            takeDamage(5, false, true);
        }
    }


    public override void onDie()
    {
        if (catchRepeat != true)
        {
            catchRepeat = true;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (getPos().x + i > -1 && getPos().x + i < 10 && getPos().z + j > -1 && getPos().z + j < 10)
                    {
                        if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != this)
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.takeDamage(getAttack(), true, true);
                        }
                        boardScript.callParticles("burning", new Vector3((int)getPos().x + i, (int)getPos().y, (int)getPos().z + j));
                    }
                }
            }
            
        }
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            doTempBuff(0, -1, 5);
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            doTempBuff(2, 2, 2);
            doHeal(5, false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

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
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
