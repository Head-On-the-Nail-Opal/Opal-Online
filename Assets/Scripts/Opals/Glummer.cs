﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glummer : OpalScript
{

    private Glimmerpillar glimmerpillarPrefab;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 3;
        priority = 3;
        myName = "Glummer";
        transform.localScale = new Vector3(3f, 3f, 1);
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Beacon", 1, 4, 0, "Spawn a Glimmerpillar.",0,3);
        Attacks[1] = new Attack("Refraction", 1, 1, 0, "Give target +2 attack and +2 defense for each Glimmerpillar alive, for 1 turn.",0,3);
        Attacks[2] = new Attack("Magnified Heal", 1, 1, 0, "Heal target 2 health for each Glimmerpillar alive. If there are 4 or more, overheal.",0,3);
        Attacks[3] = new Attack("Larva Love",0,1,0,"Heal all Glimmerpillars by 10 health and heal their status effects.",0,3);
        type1 = "Swarm";
        type2 = "Light";
        glimmerpillarPrefab = Resources.Load<Glimmerpillar>("Prefabs/SubOpals/Glimmerpillar");
        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 3), new Behave("Ally", 1, 10),
            new Behave("Safety", 0,1)});
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
            int minionCount = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Glimmerpillar" && o.getDead() == false && o.getTeam() == getTeam())
                    minionCount++;
            }
            target.doTempBuff(0, 1, 2*minionCount);
            target.doTempBuff(1, 1, 2 * minionCount);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            int minionCount = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Glimmerpillar" && o.getDead() == false)
                    minionCount++;
            }
            target.doHeal(2*minionCount, (minionCount == 4));
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            int minionCount = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Glimmerpillar" && o.getDead() == false)
                {
                    minionCount++;
                    o.doHeal(10, false);
                    o.healStatusEffects();
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
            spawnOplet(glimmerpillarPrefab, target);
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {

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
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 0)
        {
            return 0;
        }
        return -1;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            if(minionCount < 4)
                return true;
        }
        else if (atNum == 1)
        {
            if(targettingAlly(target) && minionCount > 0 && target.currentPlayer.getMyName() != "Glimmerpillar")
                return true;
        }
        else if (atNum == 2)
        {
            if (targettingAlly(target) && minionCount > 0 && target.currentPlayer.getHealth() < target.currentPlayer.getMaxHealth() && target.currentPlayer.getMyName() != "Glimmerpillar")
                return true;
        }
        else if (atNum == 3)
        {
            return false;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }
}
