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
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Propogate", 4, 1, 0, "<Free Ability>\n Costs 1 charge. Place a Growth.",0,3);
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Needles", 0, 1, 6, "<Free Ability>\n Costs 2 charge. Surrounding Opals take 6 damage.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Power Spike", 0, 5, 0, "Give an Opal standing on a Growth +2 attack and +2 charge.",0,3);
        Attacks[3] = new Attack("Enriched Soil", 0, 1, 0, "Gain +2 charge if Cactoid is standing on Growth. Otherwise gain +1.",0,3);
        type1 = "Grass";
        type2 = "Electric";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Enriched Soil
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                boardScript.setTile(target, "Growth", false);
            }
            return 0;
        }
        else if (attackNum == 1) //Propagate
        {
            if (getCharge() > 1)
            {
                doCharge(-2);
                foreach (TileScript t in getSurroundingTiles(false))
                {
                    if (t.currentPlayer != null)
                    {
                        t.currentPlayer.takeDamage(6 + getAttack(), true, true);
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Needles
        {
            target.doCharge(2);
            target.doTempBuff(0, -1, 2);
            return 0;
        }else if(attackNum == 3)
        {
            if (currentTile != null && currentTile.type == "Growth")
            {
                doCharge(2);
            }
            else
                doCharge(1);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Enriched Soil
        {
            if (getCharge() > 0)
            {
                doCharge(-1);
                boardScript.setTile(target, "Growth", false);
            }
            return 0;
        }
        else if (attackNum == 1) //Propagate
        {
            
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

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 && getCharge() < 1)
        {
            return -1;
        }
        else if(attackNum == 0)
        {
            return 1;
        }
        if (attackNum == 1 && getCharge() < 2)
        {
            return -1;
        }
        if (target.currentPlayer == null)
        {
            return -1;
        }
        return 1;
    }
}
