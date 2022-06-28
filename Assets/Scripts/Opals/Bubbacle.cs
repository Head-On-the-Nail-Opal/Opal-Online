using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbacle : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 3;
        priority = 6;
        myName = "Bubbacle";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
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
        Attacks[0] = new Attack("Frail", 0, 0, 0, "<Passive>\n When Bubbacle takes damage, place water tiles at its feet and on adjacent tiles.");
        Attacks[1] = new Attack("Bubble Blast", 4, 4, 3, "Deal damage at range. Target gets 2 levels of Bubbled. When Bubbled Opals take damage, they spread water around them at a range equal to the amount of Bubbled levels they have (up to five). This reduces their Bubbled level by 1.");
        Attacks[2] = new Attack("Pressure Wash", 3, 4, 4, "Deal damage at range. Before dealing damage, add Bubbacle’s defense buffs to its attack stat, and then clear its attack and defense buffs.");
        Attacks[3] = new Attack("Pop Frequency", 0, 1, 0, "All Bubbled Opals take damage based on their bubble level. Bubbacle gains +1 defense for each for 2 turns.");
        type1 = "Water";
        type2 = "Water";
        og = true;
    }

    public override void onDamage(int dam)
    {
        if (dam > 0)
        {
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Flood", false);
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Frail
        {
            return 0;
        }
        else if (attackNum == 1) //Bubbleblast
        {
            target.incrementEnchantment("Bubbled", 2, 5);
            return cA.getBaseDamage() + getAttack();
        }
        else if (attackNum == 2) //Imbibe
        {
            int addMe = getAttack() + getTempBuff(1);

            clearBuffs(0);
            clearBuffs(1);

            return cA.getBaseDamage()+addMe;
        }
        else if(attackNum == 3)
        {
            foreach(OpalScript o in boardScript.gameOpals)
            {
                if(o.getEnchantmentValue("Bubbled") > 0)
                {
                    o.takeDamage(o.getEnchantmentValue("Bubbled"), true, true);
                    doTempBuff(1, 3, 1);
                }
            }
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
        else if(attackNum == 1)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if(attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() + getTempBuff(1) - target.currentPlayer.getDefense();
        }
        else if(attackNum == 3)
        {
            if (target.currentPlayer.getEnchantmentValue("Bubbled") < 1)
                return 0;
            return target.currentPlayer.getEnchantmentValue("Bubbled");
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
