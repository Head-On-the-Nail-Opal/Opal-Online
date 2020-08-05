using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meadowebb : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 1;
        speed = 4;
        priority = 9;
        myName = "Meadowebb";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.2f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Full Field", 0, 0, 0, "<Passive>\nAt the start of your turn gain +1 attack and defense for each ally standing on Growth.");
        Attacks[1] = new Attack("Seeded", 1, 5, 0, "Place a Growth tile adjacent to any Growth tile. May use this three times.");
        Attacks[1].setUses(3);
        Attacks[2] = new Attack("Green Bite", 1, 1, 4, "Deal double damage to an Opal standing on Growth.");
        Attacks[3] = new Attack("Dewy Munch", 0, 1, 0, "For each adjacent Opal gain +2 health.", 1);
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onStart()
    {
        List<OpalScript> myTeam = new List<OpalScript>();
        if(player == "Red")
        {
            myTeam = boardScript.p1Opals;
        }else if (player == "Blue")
        {
            myTeam = boardScript.p2Opals;
        }
        else if (player == "Green")
        {
            myTeam = boardScript.p3Opals;
        }
        else if (player == "Orange")
        {
            myTeam = boardScript.p4Opals;
        }
        foreach (OpalScript o in myTeam)
        {
            if(o.getCurrentTile() != null && o.getCurrentTile().type == "Growth" && o != this)
            {
                doTempBuff(0, -1, 1);
                doTempBuff(1, -1, 1);
            }
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (target.getCurrentTile().type == "Growth")
            {
                return (Attacks[attackNum].getBaseDamage() + getAttack()) * 2;
            }
        }else if (attackNum == 3)
        {
            if(target != this)
            {
                doHeal(2, false);
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
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
            if (target.type == "Growth")
            {
                return (Attacks[attackNum].getBaseDamage() + getAttack())*2 - target.currentPlayer.getDefense();
            }
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
