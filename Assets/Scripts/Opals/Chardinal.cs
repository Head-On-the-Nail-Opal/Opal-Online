using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chardinal : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 4;
        priority = 0;
        myName = "Chardinal";
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
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Flaming Wreck", 1, 1, 5, "Place Flames adjacent to Chardinal, then push the target by 3 tiles.");
        Attacks[1] = new Attack("Searing Flap", 2, 4, 0, "Push a target by 2 tiles. Burn them. If they are Lifted, burning damage is doubled.");
        Attacks[2] = new Attack("Hot Air", 2, 4, 0, "Targets gain Lift, and the tiles they stand on turn to Flame", 1);
        Attacks[3] = new Attack("Fire Feet", 3, 4, 0, "Target gains +4 speed for 1 turn. The tile they stand on turns to Flame.");
        type1 = "Fire";
        type2 = "Air";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile((int)getPos().x + 1, (int)getPos().z, "Fire", false);
            boardScript.setTile((int)getPos().x - 1, (int)getPos().z, "Fire", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z + 1, "Fire", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z - 1, "Fire", false);
            pushAway(3, target);
        }
        else if (attackNum == 1)
        {
            pushAway(2, target);
            target.setBurning(true);
            if (target.getLifted())
            {
                setBurningDamage(4);
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            target.setLifted(true);
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
            return 0;
        }else if(attackNum == 3)
        {
            target.doTempBuff(2, 1, 4);
            boardScript.setTile(target, "Fire", false);
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
        }
        else if (attackNum == 2)
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
}
