using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystallite : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 3;
        priority = 0;
        myName = "Crystallite";
        transform.localScale = new Vector3(0.2f,0.2f,1) * 0.9f;
        offsetX = 0;
        offsetY = 0.1f;
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
        Attacks[0] = new Attack("Cleansing Flame", 0, 4, 0, "Burn all adjacent targets. Remove all of their stat changes.", 2);
        Attacks[1] = new Attack("World Burn", 0, 4, 0, "Replace all special tiles on adjacent tiles and under Crystallite with Flame tiles. Ignores tile priority.", 2);
        Attacks[2] = new Attack("Healing Heat", 0, 4, 0, "Heal all targets standing on Flames on adjacent tiles by 12 health. If they are not standing on Flame then set it to Flame", 2);
        Attacks[3] = new Attack("Fired Up", 0, 1, 0, "Gain +5 defense and +3 speed for 1 turn.");
        type1 = "Fire";
        type2 = "Light";
        og = true;
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Cleansing Flame
        {
            if (target == this)
            {
                return 0;
            }
            target.setBurning(true);
            target.clearBuffs();
            return 0;
        }
        else if (attackNum == 1) //World Burn
        {
            if (target.getCurrentTile().type != "Grass")
            {
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", true);
            }
            return 0;
        }
        else if (attackNum == 2) //Healing Heat
        {
            if(target == this)
            {
                return 0;
            }
            if(target.getCurrentTile().type == "Fire")
            {
                target.doHeal(12, false);
            }
            else
            {
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(1, 2, 5);
            doTempBuff(2, 2, 3);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Cleansing Flame
        {

        }
        else if (attackNum == 1) //World Burn
        {
            if (target.type != "Grass")
            {
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", true);
            }
        }
        else if (attackNum == 2) //Healing Heat
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
        if (attackNum == 1)
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
