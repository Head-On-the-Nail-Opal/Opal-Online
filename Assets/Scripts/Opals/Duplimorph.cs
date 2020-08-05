using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplimorph : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 1;
        speed = 2;
        priority = 0;
        myName = "Duplimorph";
        transform.localScale = transform.localScale;
        offsetX = 0;
        offsetY = -0.1f;
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
        Attacks[0] = new Attack("Duplicate", 1, 0, 0, "Lose 7 health and create a Duplimorph clone");
        Attacks[1] = new Attack("Insight", 0, 1, 0, "Gain +2 attack and +2 defense and heal by 2. Can heal over max health.");
        Attacks[2] = new Attack("Spectral Lunge", 1, 1, 0, "Deal 0 damage. Ignores defense. Die.");
        Attacks[3] = new Attack("Bolster", 1, 1, 0, "<Free Ability>\n Take 5 damage. Target gains +2 attack and defense.");
        Attacks[3].setFreeAction(true);
        type1 = "Void";
        type2 = "Swarm";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 2);
            doHeal(2, true);
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            target.takeDamage(cA.getBaseDamage() + getAttack() + target.getDefense(), true, true);
            takeDamage(getHealth(), false, true);
            return 0;
        }
        else if (attackNum == 3)
        {
            takeDamage(5, false, true);
            target.doTempBuff(0, -1, 2);
            target.doTempBuff(1, -1, 2);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            takeDamage(7, false, true);
            if (health > 0)
            {
                Duplimorph opalOne = Instantiate<Duplimorph>(this);
                opalOne.setOpal(player); // Red designates player 1, Blue designates player 2
                opalOne.setPos((int)target.getPos().x, (int)target.getPos().z);
                getBoard().gameOpals.Add(opalOne);
                getBoard().addToUnsorted(opalOne);
                if (player == "Red")
                {
                    getBoard().p2Opals.Add(opalOne);
                }
                else if (player == "Green")
                {
                    getBoard().p3Opals.Add(opalOne);
                }
                else if (player == "Orange")
                {
                    getBoard().p4Opals.Add(opalOne);
                }
                else
                {
                    getBoard().p1Opals.Add(opalOne);
                }
                opalOne.setHealth(this.health);
                List<TempBuff> temp = getBuffs();
                foreach (TempBuff t in temp)
                {
                    opalOne.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
                }
                target.standingOn(opalOne);
                opalOne.skipfirstTurn = true;
            }
        }
        else if (attackNum == 1) //Insight
        {
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            return 0;
        }else if(attackNum == 3)
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
            return 7;
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0)
        {
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
