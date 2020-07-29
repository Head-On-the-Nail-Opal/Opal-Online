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
        Attacks[0] = new Attack("Sync", 2, 4, 0, "Switch buffs and debuffs with a target Opal");
        Attacks[1] = new Attack("Restore", 6, 4, 0, "Remove status effects from and heal a target 4 health, and gain +1 attack and +1 defense");
        Attacks[2] = new Attack("Shift", 3, 4, 0, "Double all buffs and debuffs on Prismin. Switch them to target. They last 1 turn.");
        Attacks[3] = new Attack("Enforce", 0, 1, 0, "Gain +2 attack and +2 defense. Overheal by 4.");
        type1 = "Light";
        type2 = "Light";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {
            List<TempBuff> temp = new List<TempBuff>();
            List<TempBuff> temp2 = new List<TempBuff>();

            foreach (TempBuff t in target.getBuffs())
            {
                temp.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount()));
            }
            foreach (TempBuff t in getBuffs())
            {
                temp2.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount()));
            }
            target.clearBuffs();
            clearBuffs();
            foreach (TempBuff t in temp)
            {
                doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
            }
            foreach (TempBuff t in temp2)
            {
                target.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
            }
            return 0;
        }
        else if (attackNum == 1) //Restore
        {
            target.healStatusEffects();
            target.doHeal(4, false);
            doTempBuff(0, -1, 1);
            doTempBuff(1, -1, 1);
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            List<TempBuff> temp = new List<TempBuff>();
            foreach (TempBuff t in getBuffs())
            {
                temp.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount()));
            }
            foreach (TempBuff t in getBuffs())
            {
                temp.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount()));
            }
            foreach (TempBuff t in temp)
            {
                target.doTempBuff(t.getTargetStat(), 1, t.getAmount());
            }
            clearBuffs();
            //transform.localScale = baseSize * minSize;
            return 0;
        }
        else if(attackNum == 3)
        {
            doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 2);
            doHeal(4, true);
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
