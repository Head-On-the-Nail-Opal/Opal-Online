﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glimmerpillar : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 0;
        priority = 9;
        myName = "Glimmerpillar";
        transform.localScale = new Vector3(3f, 3f, 1)*0.8f;
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
        Attacks[0] = new Attack("Pillar", 0, 0, 0, "<Passive>\n At the start of its turn, surrounding Enemy Opals lose -4 defense for 1 turn.");
        Attacks[1] = new Attack("Lamplight", 2, 1, 0, "Give a target +2 attack and +2 defense for 1 turn",0,3);
        Attacks[2] = new Attack("Soothe", 0, 1, 0, "Surrounding allied Opals are healed by +2 health. Surrounding enemy opals lose -2 attack.",0,3);
        Attacks[3] = new Attack("Intensify", 1, 1, 0, "Give a target +2 attack and +2 defense. Die.",0,3) ;
        type1 = "Swarm";
        type2 = "Light";
        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Safety", 0,1)});
        getSpeciesSynergies().AddRange(new List<Behave> { new Behave("Glimmering-Aura", 0, 2) });
        getSpeciesAwareness().AddRange(new List<Behave> { new Behave("Glimmering-Aura", 0, 2) });
    }

    public override void onStart()
    {
        foreach (TileScript t in getSurroundingTiles(false))
        {
            if (t.currentPlayer != null)
            {
                if (t.currentPlayer.getTeam() != getTeam())
                {
                    t.currentPlayer.doTempBuff(1, 1, -4);
                    StartCoroutine(playFrame("attack", 5));
                }
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
            target.doTempBuff(0, 1, 2);
            target.doTempBuff(1, 1, 2);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if(t.currentPlayer != null)
                {
                    if(t.currentPlayer.getTeam() == getTeam())
                    {
                        t.currentPlayer.doHeal(2, false);
                    }
                    else
                    {
                        t.currentPlayer.doTempBuff(0, -1, -2);
                    }
                }
            }
            return 0;
        }
        else if(attackNum == 3)
        {
            target.doTempBuff(0, -1, 2);
            target.doTempBuff(1, -1, 2);
            takeDamage(health, false, true);

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
            
        }
        else if (attackNum == 2) //Grass Cover
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
            return 0;
        }
        else if (attackNum == 3)
        {
            takeDamage(5, false, true);
            boardScript.setTile(target, "Grass", true);

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
        if (attackNum == 3 || attackNum == 2)
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
            return false;
        }
        else if (atNum == 1)
        {
            if (targettingAlly(target) && target.currentPlayer.getMyName() != "Glimmerpillar")
                return true;
        }
        else if (atNum == 2)
        {
            int result = 0;
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if (t.getCurrentOpal() != null)
                    result++;
            }
            if(result > 3)
                return true;
        }
        else if (atNum == 3)
        {
            if (targettingAlly(target) && target.currentPlayer.getMyName() != "Glimmerpillar")
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }
}
