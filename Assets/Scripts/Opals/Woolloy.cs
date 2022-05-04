using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woolloy : OpalScript
{
    int numFlames = 0;
    int numMiasma = 0;
    int numGrowth = 0;
    int numFlood = 0;

    bool brushed = false;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 3;
        speed = 2;
        priority = 2;
        myName = "Woolloy";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.1f;
        offsetX = 0;
        offsetY = 0f;
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

        /**
         * "Flame: When you take damage from burn, gain attack equal to the damage. " +
            "Miasma: When Woolloy loses stats, it gains +2 defense. " +
            "Growth: Gain +1 attack and +1 defense at the start of your turn. " +
            "Flood: Gain +1 speed for 1 turn when you start your turn on a Flood tile."
         * */

        Attacks[0] = new Attack("Scrap Alloy", 0, 0, 0, "<Passive>\nThe first tile cleared by Clean Rub per turn provides a variety of passive effects.");
        Attacks[1] = new Attack("Heavy Brush", 0, 1, 0, "<Free Ability>\n Clear the tile under Woolloy.");
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Steel Scrub", 0, 1, 0, "Heal any status conditions on Woolloy and adjacent Opals. Heal each 4 health.");
        Attacks[3] = new Attack("Polish", 1, 1, 0, "Give the target a copy of the buffs on Woolloy, they last for 1 turn.");
        type1 = "Metal";
        type2 = "Light";
    }

    private void updateAlloy()
    {
        string description = "<Passive>\nThe first tile cleared by Clean Rub per turn provides a variety of passive effects.";
        if (numFlames > 0)
            description += " Flames: " + numFlames;
        if (numMiasma > 0)
            description += " Miasma: " + numMiasma;
        if (numGrowth > 0)
            description += " Growth: " + numGrowth;
        if (numFlood > 0)
            description += " Flood: " + numFlood;
        Attacks[0] = new Attack("Scrap Alloy", 0, 0, 0, description);
    }

    private void updateBrush()
    {
        string description = "<Free Ability>\n Clear the tile under Woolloy. (Once per turn)";
        if(!brushed && currentTile != null)
        {
            if (currentTile.type == "Fire")
                description += " Flame: When Woolloy takes damage from burn, it gains +1 attack.";
            else if(currentTile.type == "Miasma")
            {
                description += " Miasma: When Woolloy loses stats, it gains +2 defense.";
            }
            else if (currentTile.type == "Growth")
            {
                description += " Growth: Gain +1 attack and +1 defense at the start of your turn.";
            }
            else if (currentTile.type == "Flood")
            {
                description += " Flood: Gain +1 speed for 1 turn when you start your turn on a Flood tile.";
            }
        }
        Attacks[1] = new Attack("Heavy Brush", 0, 1, 0, description);
        Attacks[1].setFreeAction(true);
    }

    public override void onStart()
    {
        updateAlloy();
        updateBrush();

        brushed = false;

        if(currentTile != null)
        {
            if(currentTile.type == "Growth" && numGrowth > 0)
            {
                doTempBuff(0,-1,numGrowth);
                doTempBuff(1, -1, numGrowth);
            }
            else if(currentTile.type == "Flood" && numFlood > 0)
            {
                doTempBuff(2, 1, numFlood);
            }
        }
    }

    public override void onBuff(TempBuff buff)
    {
        if(buff.getAmount() < 0 && numMiasma > 0)
        {
            doTempBuff(1, -1, 2 * numMiasma);
        }
    }

    public override void onBurnDamage(int dam)
    {
        if(numFlames > 0)
            doTempBuff(0, -1, numFlames);
    }

    public override void onMove(int distanceMoved)
    {
        updateBrush();
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Stored Energy
        {
            return 0;
        }
        else if (attackNum == 1) //Discharge
        {
            

            if (!brushed && currentTile != null)
            {
                ParticleSystem aura = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/WoolloyAura"), transform);

                ParticleSystem.MainModule settings = aura.main;

                if (currentTile.type == "Fire")
                {
                    numFlames++;
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.red);

                }
                else if (currentTile.type == "Miasma")
                {
                    numMiasma++;
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.magenta);
                }
                else if (currentTile.type == "Growth")
                {
                    numGrowth++;
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.green);
                }
                else if (currentTile.type == "Flood")
                {
                    numFlood++;
                    settings.startColor = new ParticleSystem.MinMaxGradient(Color.blue);
                }
            }
            boardScript.setTile(target, "Grass", true);
            updateAlloy();
            brushed = true;
            return 0;
        }
        else if (attackNum == 2) //Static
        {
            doHeal(4, false);
            healStatusEffects();

            foreach(TileScript t in getSurroundingTiles(true))
            {
                if(t.getCurrentOpal() != null)
                {
                    t.getCurrentOpal().healStatusEffects();
                    t.getCurrentOpal().doHeal(4, false);
                }
            }
            return 0;
        }else if(attackNum == 3)
        {
            foreach(TempBuff b in buffs)
            {
                target.doTempBuff(b.getTargetStat(), 1, b.getAmount());
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
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 && currentTile != null && currentTile.type == "Grass")
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
