using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froxic : OpalScript
{
    CursorScript cs;

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 4;
        defense = 1;
        speed = 2;
        priority = 9;
        myName = "Froxic";
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
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Secretion", 0, 0, 0, "<Passive>\n If Froxic is standing in Flood, attackers are poisoned. This poison deals damage equal to damage dealt to Froxic.");
        Attacks[1] = new Attack("Contaminate", 1, 3, 6, "<Water Rush>\n Poison the target.");
        Attacks[2] = new Attack("Neurotoxin", 1, 3, 0, "<Water Rush>\n Target takes damage from its poison. Heal the damage dealt.");
        Attacks[3] = new Attack("Damp", 0, 1, 0, "Place Flood at your feet and on surrounding tiles.");
        type1 = "Plague";
        type2 = "Water";
        if(pl != null)
        {
            cs = FindObjectOfType<CursorScript>();
        }
    }

    public override void onDamage(int dam)
    {
        if(currentTile != null && currentTile.type == "Flood")
        {
            OpalScript attacker = cs.getCurrentOpal();
            if(attacker != this && dam - getDefense() > 0)
            {
                attacker.setPoison(true);
                attacker.setPoisonCounter(dam - getDefense(), true);
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
            target.setPoison(true);
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (target.getPoison())
            {
                target.takeDamage(target.getPoisonCounter(), false, true);
                doHeal(target.getPoisonCounter(), false);
            }
            return 0;
        }else if(attackNum == 3)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    getBoard().setTile((int)target.getPos().x + i, (int)target.getPos().z + j, "Flood", false);
                }
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
            return 0;
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 2)
        {
            if(target.currentPlayer != null)
            {
                if (target.currentPlayer.getPoison())
                {
                    return target.currentPlayer.getPoisonCounter();
                }
            }
            return 0;
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
