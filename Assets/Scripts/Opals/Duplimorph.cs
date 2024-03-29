﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duplimorph : OpalScript
{
    private Duplimorph duplimorphPrefab;

    public override void onAwake()
    {
        duplimorphPrefab = Resources.Load<Duplimorph>("Prefabs/Opals/Duplimorph");

    }
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 1;
        speed = 2;
        priority = 0;
        myName = "Duplimorph";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = 0;
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
        Attacks[0] = new Attack("Duplicate", 1, 0, 0, "Lose 7 health and create a Duplimorph clone",0,3);
        Attacks[1] = new Attack("Insight", 0, 1, 0, "Gain +2 attack and +2 defense and overheal by 2.",0,3);
        Attacks[2] = new Attack("Spectral Lunge", 1, 1, 0, "Deal 0 damage. Ignores defense. Die.",0,3);
        Attacks[3] = new Attack("Bolster", 1, 1, 0, "<Free Ability>\n Take 5 damage. Target gains +2 attack and defense.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Void";
        type2 = "Swarm";

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Weasely", 1, 4),
            new Behave("Safety", 0, 1) });
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
                OpalScript opalOne = spawnOplet(duplimorphPrefab, target, 0);

                if (opalOne != null) { 
                    opalOne.setHealth(this.health);
                    opalOne.setDetails(this);
                    List<TempBuff> temp = getBuffs();
                    foreach (TempBuff t in temp)
                    {
                        opalOne.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
                    }
                }
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

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            if (health > 7 && canSpawnOplet() && !boardScript.myCursor.tileIsFalling((int)target.getPos().x, (int)target.getPos().z)) //needs to check better
                return true;
        }
        else if (atNum == 1 && !useAdjacentToOpal(target, true))
        {
            return true;
        }
        else if (atNum == 2)
        {
            if (health <= maxHealth / 2 && target.getCurrentOpal().getTeam() != getTeam() && !isTeamEmptyDuplimorph(false))
                return true;
        }
        else if (atNum == 3)
        {
            if(health > maxHealth/2 && target.getCurrentOpal().getTeam() == getTeam())
            {
                return true;
            }
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }

    private bool canSpawnOplet()
    {
        foreach(OpalScript o in boardScript.gameOpals)
        {
            if (o.getTeam() == getTeam() && o.getMyName() == "Duplimorph")
                if (o.getMinionCount() == 4)
                    return false;
        }
        return true;
    }

    public bool isTeamEmptyDuplimorph(bool enemy)
    {
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (!o.getDead() && o != this)
            {
                if (enemy)
                {
                    if (o.getTeam() != getTeam() && o.getMyName() == "Duplimorph")
                    {
                        return false;
                    }
                }
                else
                {
                    if (o.getTeam() == getTeam() && o.getMyName() == "Duplimorph")
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
