using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcoco : OpalScript
{
    private int eruptionRange = 1;
    private int eruptionDamage = 5;
    private int currentCycle = 0;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 3;
        speed = 1;
        priority = 6;
        myName = "Volcoco";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Eruption", 1, 1, 5, "Deal 5 damage. This attack gains +1 range for each surrounding growth, and +2 damage for each surrounding fire.",0,3);
        Attacks[1] = new Attack("Ash Deposit", 0, 1, 0, "Surrounding Opals standing on fire take burn damage, surrounding opals standing on growth gain +2 attack and +2 defense");
        Attacks[2] = new Attack("Natural Cycle", 2, 1, 0, "Place a growth tile and then a fire tile. Then repeat.");
        Attacks[2].setUses(4);
        Attacks[3] = new Attack("Heatleaf", 0, 5, 0, "Opal standing on a growth takes damage from their burn.",0,3);
        type1 = "Fire";
        type2 = "Grass";
    }

    public override void onStart()
    {
        currentCycle = 0;
        int extraDamage = 0;
        int extraRange = 0;
        foreach (TileScript t in getSurroundingTiles(false))
        {
            if (t.type == "Growth")
            {
                extraRange++;
            }
            else if (t.type == "Fire")
            {
                extraDamage++;
            }
        }
        Attacks[0] = new Attack("Eruption", 1 + extraRange, 1, 5 + extraDamage*2, "This attack gains +1 range for each surrounding growth, and +2 damage for each surrounding fire.",0,3);
    }

    public override void onMove(int distanceMoved)
    {
        int extraDamage = 0;
        int extraRange = 0;
        foreach (TileScript t in getSurroundingTiles(false))
        {
            if (t.type == "Growth")
            {
                extraRange++;
            }
            else if (t.type == "Fire")
            {
                extraDamage++;
            }
        }
        Attacks[0] = new Attack("Eruption", 1+extraRange, 1, 5+extraDamage*2, "This attack gains +1 range for each surrounding growth, and +2 damage for each surrounding fire.",0,3);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            
        }
        else if (attackNum == 1)
        {
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if(t.getCurrentOpal() != null)
                {
                    if(t.type == "Growth")
                    {
                        t.getCurrentOpal().doTempBuff(0, -1, 2);
                        t.getCurrentOpal().doTempBuff(1, -1, 2);
                    }
                    else if(t.type == "Fire")
                    {
                        t.getCurrentOpal().takeBurnDamage(false);
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            if (currentCycle == 0)
            {
                boardScript.setTile(target, "Growth", false);
                currentCycle++;
            }
            else
            {
                boardScript.setTile(target, "Fire", false);
                currentCycle = 0;
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            target.takeBurnDamage(false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
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
            if (currentCycle == 0)
            {
                boardScript.setTile(target, "Growth", false);
                currentCycle++;
            }
            else
            {
                boardScript.setTile(target, "Fire", false);
                currentCycle = 0;
            }
            
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Fire", false);
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

        }
        else if (attackNum == 1)
        {
            if (target.type == "Fire" && target.getCurrentOpal() != null && target.getCurrentOpal().getBurning())
                return target.getCurrentOpal().getBurningDamage();
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            if (target.type == "Growth" && target.getCurrentOpal() != null && target.getCurrentOpal().getBurning())
                return target.getCurrentOpal().getBurningDamage();
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 2)
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
