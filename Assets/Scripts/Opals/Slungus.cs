using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slungus : OpalScript
{
    int inc = 1;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 2;
        priority = 4;
        myName = "Slungus";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.5f;
        offsetX = 0;
        offsetY = -0.15f;
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
        Attacks[0] = new Attack("Toxic", 0, 0, 0, "<Passive>\n When Slungus is buffed, it is debuffed instead.");
        Attacks[1] = new Attack("Spore Spit", 2, 1, 4,"Target is poisoned. Poison deals damage based on Slungus's negative attack stat.");
        Attacks[2] = new Attack("Mush Rush", 2, 1, 0, "Buff an Opal's attack and defense by the inverse of your current stats.");
        Attacks[3] = new Attack("Lifeshroom", 2, 1, 0, "Buff an Opal's attack and defense by 1. Lose -1 attack and -1 defense. Place Growths under both Opals.");
        type1 = "Plague";
        type2 = "Grass";
    }

 

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            
            return 0;
        }
        else if (attackNum == 1) //
        {
            target.setPoison(true);
            if(getAttack() < 0)
                target.poisonCounter = -1 * getAttack();
           
        }
        else if (attackNum == 2) //
        {
            target.doTempBuff(0, -1, -getTempBuff(0));
            target.doTempBuff(1, -1, -getTempBuff(1));
            target.doTempBuff(2, -1, -getTempBuff(2));
            return 0;
        }
        else if(attackNum == 3)
        {
            target.doTempBuff(0, -1, 1);
            target.doTempBuff(1, -1, 1);
            doTempBuff(0, -1, -1);
            doTempBuff(1, -1, -1);
            boardScript.setTile(target, "Growth", false);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {

        }
        else if (attackNum == 1) //
        {

        }
        else if (attackNum == 2) //
        {

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
        if(target.getCurrentOpal() != null)
        {
            return 0;
        }
        return -1;
    }

    public override void onBuff(TempBuff buff)
    {
        if(buff.getAmount() > 0)
            doTempBuff(buff.getTargetStat(), buff.getTurnlength(), buff.getAmount() * -2);
    }
}
