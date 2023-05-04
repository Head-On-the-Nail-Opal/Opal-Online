using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluttorch : OpalScript
{

    int cinderWing = 1;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 2;
        defense = 3;
        speed = 4;
        priority = 5;
        myName = "Fluttorch";
        transform.localScale = new Vector3(3f, 3f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Building Flame", 0, 0, 0, "<Passive>\nAt the start of Fluttorch's turn it gains one more use of Cinder Wing.");
        Attacks[1] = new Attack("Cinder Wing", 3, 4, 0, "<Free Ability>\nFly up to three tiles in a single direction. Place Flames on the tiles you fly over. Fluttorch has [1] uses of this ability.",0,3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Healing Warmth", 2, 4, 0, "Heal a target by 3. Give them +2 attack and +1 speed for 1 turn. If you're standing in Flame, double this effect.",0,3);
        Attacks[3] = new Attack("Dragon's Breath",2,4,0, "Give a target burn and lift. If you're standing in Flame, deal 6 damage and push them back by 2 tiles.",0,3);
        type1 = "Fire";
        type2 = "Air";
        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 3), new Behave("Line-Up-Ally", 1, 5),
            new Behave("Safety", 0,1), new Behave("Hot-Headed", 0, 10) });
    }

    public override void onStart()
    {
        cinderWing++;
        Attacks[1] = new Attack("Cinder Wing", 3, 4, 0, "<Free Ability>\nMove up to four tiles in a single direction. Place flames at your feet as you move. Fluttorch has ["+cinderWing+"] uses of this ability.",0,3);
        Attacks[1].setFreeAction(true);
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (currentTile != null && currentTile.type == "Fire")
            {
                target.doHeal(6, false);
                target.doTempBuff(0, 1, 4);
                target.doTempBuff(2, 1, 2);
                return 0;
            }
            else
            {
                target.doHeal(3, false);
                target.doTempBuff(0, 1, 2);
                target.doTempBuff(2, 1, 1);
            }
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            if (currentTile != null && currentTile.type == "Fire")
            {
                target.setBurning(true);
                target.setLifted(true);
                pushAway(2, target);
                return 6 + getAttack();
            }
            target.setBurning(true);
            target.setLifted(true);
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
            if (cinderWing > 0)
            {
                TileScript start = this.currentTile;
                TileScript end = target;
                teleport((int)target.getPos().x, (int)target.getPos().z, 0);

                foreach(TileScript t in getLineBetween(start, end))
                {
                    boardScript.setTile(t, "Fire", false);
                }

                cinderWing--;
                Attacks[1] = new Attack("Cinder Wing", 3, 4, 0, "<Free Ability>\nMove up to four tiles in a single direction. Place flames at your feet as you move. Fluttorch has [" + cinderWing + "] uses of this ability.",0,3);
                Attacks[1].setFreeAction(true);
                if (cinderWing <= 0)
                    Attacks[1] = new Attack("Cinder Wing", 0, 0, 0, "<Free Ability>\nMove up to four tiles in a single direction. Place flames at your feet as you move. Fluttorch has [" + cinderWing + "] uses of this ability. It cannot use this now.",0,3);
                    Attacks[1].setFreeAction(true);
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
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
        else if (attackNum == 3)
        {
            if (currentTile != null && currentTile.type == "Fire")
            {
                return 6 + getAttack() - target.currentPlayer.getDefense();
            }
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 1)
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
            if(cinderWing > 0 && (useAroundOpal(target, true, 3) || useAroundOpal(target, false, 3)))
                return true;
        }
        else if (atNum == 2)
        {
            if(target.getCurrentOpal() != null && target.getCurrentOpal().getTeam() == getTeam())
            return true;
        }
        else if (atNum == 3)
        {
            if(target.getCurrentOpal() != null && target.getCurrentOpal().getTeam() != getTeam() && currentTile.type == "Fire")
                return true;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 2;
    }
}
