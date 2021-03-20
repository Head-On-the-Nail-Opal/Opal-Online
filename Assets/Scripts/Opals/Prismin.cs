using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prismin : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 2;
        priority = 2;
        myName = "Prismin";
        //baseSize = new Vector3(0.2f, 0.2f, 1);
        transform.localScale = new Vector3(0.2f, 0.24f, 1) * 0.5f;
        offsetX = 0;
        offsetY = -0.08f;
        offsetZ = 0;
        player = pl;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Attacks[0] = new Attack("Replicate", 0, 1, 0, "Double all buffs or debuffs on Prismin. Prismin takes 15 damage.");
        Attacks[1] = new Attack("Balance", 1, 1, 0, "Switch buffs and debuffs with a friendly Opal.");
        Attacks[2] = new Attack("Restore", 0, 1, 0, "Overheal Prismin by 10 health.");
        Attacks[3] = new Attack("Support", 1, 1, 0, "Give an Opal +1 attack and defense. Overheal them by 5.");
        type1 = "Light";
        type2 = "Light";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {
            List<TempBuff> newBuffs = new List<TempBuff>();
            foreach(TempBuff t in buffs)
            {
                newBuffs.Add(t);
            }
            foreach(TempBuff t in newBuffs)
            {
                if(t.getTurnlength() != 0)
                    doTempBuff(t.getTargetStat(), t.getTurnlength(),t.getAmount());
            }
            return 0;
        }
        else if (attackNum == 1) //Restore
        {
            if(target.getTeam() == getTeam())
            {
                List<TempBuff> myBuffs = new List<TempBuff>();
                List<TempBuff> tBuffs = new List<TempBuff>();
                foreach(TempBuff t in buffs)
                {
                    if (t.getTurnlength() != 0)
                        myBuffs.Add(t);
                }
                foreach(TempBuff t in target.getBuffs())
                {
                    if (t.getTurnlength() != 0)
                        tBuffs.Add(t);
                }
                target.clearAllBuffs();
                clearAllBuffs();
                foreach (TempBuff t in myBuffs)
                {
                    target.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
                }
                foreach(TempBuff t in tBuffs)
                {
                    doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
                }
                takeDamage(15, false, true);
            }
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            doHeal(10, true);
            return 0;
        }
        else if(attackNum == 3)
        {
            target.doTempBuff(0, -1, 1);
            target.doTempBuff(1, -1, 1);
            target.doHeal(5, true);
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
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
