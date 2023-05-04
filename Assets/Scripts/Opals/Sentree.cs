using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentree : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 2;
        priority = 5;
        myName = "Sentree";
        transform.localScale = new Vector3(3f, 3f, 1)*1.1f;
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
        Attacks[0] = new Attack("Canopy", 0, 1, 0, "All adjacent Opals gain +1 attack and +2 defense, and place growths at their and your feet", 2,3);
        Attacks[1] = new Attack("Nature Call", 0, 5, 0, "Target any growth, place growths on adjacent tiles", 2,3);
        Attacks[2] = new Attack("Stray Limb", 1, 1, 16, "Deal 16 damage. Take 12 damage. Place a growth under the target's feet.",0,3);
        Attacks[3] = new Attack("Deep Roots", 0, 1, 0, "Gain +2 attack and +4 defense. Lose -2 speed for 1 turn.",0,3);
        type1 = "Grass";
        type2 = "Grass";
        og = true;

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Ally", 1, 14), new Behave("Close-Combat", 1, 2),
            new Behave("Safety", 0,1) });

        getSpeciesSynergies().AddRange(new List<Behave>{
            new Behave("AnchorTree", 0, 10) });
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Canopy
        {
            
            if (target.getPos() != getPos())
            {
                target.doTempBuff(0, -1, 1);
                target.doTempBuff(1, -1, 2);
            }
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 1) //Nature Call
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Stray Limb
        {
            takeDamage(6, false, true);
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
        }
        else if (attackNum == 3) //Stray Limb
        {
            doTempBuff(0, -1, 2);
            doTempBuff(1, -1, 4);
            doTempBuff(2, 2, -2);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Canopy
        {

        }
        else if (attackNum == 1) //Nature Call
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Stray Limb
        {
            takeDamage(12, false, true);
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return cA.getBaseDamage() + getAttack() + 2;
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1)
        {
            return 0;
        }
        if(target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            if(useAdjacentToOpal(target, false))
                return true;
        }
        else if (atNum == 1)
        {
            if(target.currentPlayer != null)
                return true;
        }
        else if (atNum == 2)
        {
            if (target.getCurrentOpal() != null && target.getCurrentOpal().getTeam() != getTeam())
                return true;
        }
        else if (atNum == 3)
        {
            if(!useAdjacentToOpal(target, true) && !useAdjacentToOpal(target, false))
                return false;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }
}
