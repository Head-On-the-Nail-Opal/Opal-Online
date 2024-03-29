﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbacle : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 3;
        priority = 6;
        myName = "Bubbacle";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[1] = new Attack("Bubble Blast", 4, 4, 3, "Deal damage at range. Target gets 2 levels of Bubbled. When Bubbled Opals take damage, the bubbles pop and spill water. The more bubbles, the more water.",0,3);
        Attacks[1].addProjectile("Default", "Bubble", 10, Color.white,0.9f);
        Attacks[2] = new Attack("Pressure Wash", 3, 4, 4, "Deal damage at range. Before dealing damage, add Bubbacle’s defense buffs to its attack stat, and then clear its attack and defense buffs.",0,3);
        Attacks[3] = new Attack("Pop Frequency", 0, 1, 0, "All Bubbled Opals take damage based on their bubble level. Bubbacle gains +1 defense for each for 2 turns.",0,3);
        type1 = "Water";
        type2 = "Water";
        og = true;

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 3), new Behave("Line-Up", 1, 5),
            new Behave("Safety", 0,1) });
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

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            return false;
        }
        else if (atNum == 1)
        {
            if (target.currentPlayer != null && target.currentPlayer.getTeam() != getTeam())
            {
                return true;
            }
        }
        else if (atNum == 2)
        {
            if (target.currentPlayer != null && target.currentPlayer.getTeam() != getTeam() && getDefense() > 8)
            {
                return true;
            }
        }
        else if (atNum == 3)
        {
            int result = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getEnchantmentValue("Bubbled") > 0)
                    result += 1;
            }
            if (result > 2)
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 4;
    }
}
