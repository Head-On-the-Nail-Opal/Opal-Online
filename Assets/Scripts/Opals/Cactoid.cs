using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cactoid : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 5;
        myName = "Cactoid";
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
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Enriched Soil", 0, 1, 0, "Gain +2 charge for each adjacent Opal");
        Attacks[1] = new Attack("Propagate", 3, 1, 0, "<Aftershock>\n Costs 1 charge. Place a Growth tile. Repeatable while Cactoid has charge. Doesn't need to target an Opal.");
        Attacks[2] = new Attack("Needles", 0, 5, 12, "Costs 2 charge. Deal 12 damage to an Opal standing next to a Growth tile. They lose -3 speed for their next turn.");
        Attacks[3] = new Attack("Power Spike", 4, 4, 0, "Costs all charge. Deal twice damage of the charge amount.");
        type1 = "Grass";
        type2 = "Electric";
    }

    public override void onStart()
    {
        bannedAttacks.Clear();
        attackAgain = true;
    }

    public override void prepAttack(int attackNum)
    {
        if(attackNum == 0)
        {
            attackAgain = false;
        }
        else if(attackNum == 1)
        {
            attackAgain = true;
        }
        else if (attackNum == 2)
        {
            attackAgain = false;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Enriched Soil
        {
            attackAgain = false;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                        {
                            doCharge(2);
                        }
                    }
                }
            }
            boardScript.setChargeDisplay(getCharge());
        }
        else if (attackNum == 1) //Propagate
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
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 2) //Needles
        {
            if (getCharge() > 1 && target.getCurrentTile().type != "Growth")
            {
                doCharge(-2);
                boardScript.setChargeDisplay(getCharge());
                target.doTempBuff(2, 1, -3);
                attackAgain = false;
                bannedAttacks.Add(attackNum);
                return cA.getBaseDamage() + getAttack();
            }
            return 0;
        }else if(attackNum == 3)
        {
            int dam = getCharge();
            doCharge(getCharge());
            boardScript.setChargeDisplay(getCharge());
            return dam*2 + getAttack();
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Enriched Soil
        {

        }
        else if (attackNum == 1) //Propagate
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
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
                return 0;
            }
            attackAgain = false;
            return 0;
        }
        else if (attackNum == 2) //Needles
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            if(target.type == "Growth")
            {
                return 0;
            }
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }else if(attackNum == 3)
        {
            return getCharge() * 2 + getAttack() - target.currentPlayer.getDefense();
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
