using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceraura : OpalScript
{
    bool alreadyTrigger = false;
    int myAttack = 0;

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 2;
        priority = 7;
        myName = "Sorceraura";
        //baseSize = new Vector3(0.2f, 0.2f, 1);
        transform.localScale = new Vector3(0.2f, 0.24f, 1) * 1f;
        offsetX = 0;
        offsetY = 0.2f;
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
        Attacks[0] = new Attack("Energetic Blast", 4, 4, 0, "Heal Opals in an area by your attack. If your attack is negative, deal damage instead.", 1);
        Attacks[1] = new Attack("Trick", 1, 1, 0, "Gain +3 attack. Target loses -3 attack.");
        Attacks[2] = new Attack("Sleight of Hand", 1, 1, 0, "Invert your attack stat. Buff target by your new attack stat.");
        Attacks[3] = new Attack("Turnaround", 2, 1, 0, "Damage an Opal by their attack stat.");
        type1 = "Light";
        type2 = "Dark";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {
            if (target == this)
                return 0;
            if(getAttack() < 0)
            {
                return getAttack();
            }
            else
            {
                target.doHeal(getAttack(), false);
            }
            return 0;
        }
        else if (attackNum == 1) //Restore
        {
            doTempBuff(0, -1, 3);
            target.doTempBuff(0, -1, -3);
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            List<TempBuff> newBuffs = new List<TempBuff>();
            foreach (TempBuff t in buffs)
            {
                if(t.getTurnlength() != 0)
                    newBuffs.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount() * -1));
            }
            clearAllBuffs();
            foreach(TempBuff t in newBuffs)
            {
                doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
            }
            target.doTempBuff(0, -1, getAttack());
            return 0;
        }
        else if (attackNum == 3) //Shift
        {
            return target.getAttack();
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 0)
        {
            if (getAttack() < 0)
                return Attacks[attackNum].getBaseDamage() - getAttack() - target.currentPlayer.getDefense();
            else
            {
                return 0;
            }
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }else if (attackNum == 3)
        {
            if(target.currentPlayer != null)
            {
                return target.currentPlayer.getAttack() - target.currentPlayer.getDefense();
            }
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
