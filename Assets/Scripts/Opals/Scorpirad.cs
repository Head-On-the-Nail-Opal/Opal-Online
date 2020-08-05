using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorpirad : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 4;
        defense = 2;
        speed = 3;
        priority = 8;
        myName = "Scorpirad";
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
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Contaminated", 0, 0, 0, "<Passive>\nOpals adjacent to Scorpirad at the end of it's turn are poisoned for 1 turn.");
        Attacks[1] = new Attack("Radsap", 2, 4, 0, "If the target has an attack lower than 0, gain attack opposite to theirs.");
        Attacks[2] = new Attack("Radiation Sickness", 2, 4, 3, "Target is poisoned. Opals adjacent to the target take damage from the target's poison.");
        Attacks[3] = new Attack("Hazard Sting", 1, 4, 2, "Target takes damage from their poison.");
        type1 = "Plague";
        type2 = "Plague";
    }

    public override void onEnd()
    {
        base.onEnd();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (Mathf.Abs(i)!= Mathf.Abs(j) && getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1)
                {
                    OpalScript target = boardScript.tileGrid[(int)(getPos().x + i), (int)(getPos().z + j)].currentPlayer;
                    if (target != null)
                    {
                        target.setPoison(true);
                        target.setPoisonTimer(1, false);
                    }
                }
            }
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            if(target.getAttack() > 0)
            {
                doTempBuff(0, -1, -target.getAttack());
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            target.setPoison(true);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (Mathf.Abs(i) != Mathf.Abs(j) && target.getPos().x + i < 10 && target.getPos().x + i > -1 && target.getPos().z + j < 10 && target.getPos().z + j > -1)
                    {
                        OpalScript t = boardScript.tileGrid[(int)(target.getPos().x + i), (int)(target.getPos().z + j)].currentPlayer;
                        if (t != null && t != this)
                        {
                            t.takeDamage(target.getPoisonCounter(), false, true);
                            t.doTempBuff(0, -1, -1);
                            t.doTempBuff(1, -1, -1);
                        }
                    }
                }
            }
        }
        else if (attackNum == 3)
        {
            target.takePoisonDamage(false);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
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
            if (target.currentPlayer.getPoison())
            {
                return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + target.currentPlayer.getPoisonCounter();
            }
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
