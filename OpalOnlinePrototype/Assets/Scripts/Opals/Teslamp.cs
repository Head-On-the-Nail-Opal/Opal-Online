using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teslamp : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 2;
        priority = 6;
        myName = "Teslamp";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0f;
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
        Attacks[0] = new Attack("Grounded", 0, 1, 0, "<Free Ability>\n Clear the tile at your feet and gain +2 charge. If there is no tile effect, this has no effect.");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Shine", 4, 1, 0, "<Aftershock>\nClear a tile effect. Repeatable until Teslamp is out of charge. Targets heal all status effects. Costs 1 charge.");
        Attacks[2] = new Attack("Spotlight", 2, 1, 0, "<Aftershock>\nGive a target +1 attack and +1 defense. Repeatable until Teslamp is out of charge. Costs 2 charge.");
        Attacks[3] = new Attack("Burnout", 0, 1, 0, "Clear all status effects from Teslamp. Gain +1 charge for each.");
        type1 = "Light";
        type2 = "Electric";
        og = true;
    }

    public override void onStart()
    {
        bannedAttacks.Clear();
        attackAgain = true;
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            attackAgain = false;
        }
        else if (attackNum == 1)
        {
            attackAgain = true;
        }
        else if (attackNum == 2)
        {
            attackAgain = true;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            if (boardScript.tileGrid[(int)target.getPos().x, (int)target.getPos().z].type != "Grass")
            {
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
                doCharge(2);
                boardScript.setChargeDisplay(getCharge());
            }
            return 0;
        }
        else if (attackNum == 1) //
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if (getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                target.healStatusEffects();
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 2) //
        {
            if (getCharge() > 1)
            {
                doCharge(-2);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if (getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                target.doTempBuff(0, -1, 1);
                target.doTempBuff(1, -1, 1);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 3)
        {
            int i = 0;
            if (getBurning())
            {
                i++;
            }
            if (getPoison())
            {
                i++;
            }
            if (getLifted())
            {
                i++;
            }
            healStatusEffects();
            doCharge(i);
            boardScript.setChargeDisplay(getCharge());
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
            if (getCharge() > 0)
            {
                doCharge(-1);
                boardScript.setChargeDisplay(getCharge());
                attackAgain = true;
                if (getCharge() <= 0)
                {
                    bannedAttacks.Add(attackNum);
                }
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
                return 0;
            }
            attackAgain = false;
            return 0;
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

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1)
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
