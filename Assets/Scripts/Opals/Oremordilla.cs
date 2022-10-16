using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oremordilla : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 4;
        myName = "Oremordilla";
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
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Hidey Hole", 0, 1, 0, "Place Boulders on adjacent tiles",0,3);
        Attacks[1] = new Attack("Balled Thrust", 1, 1, 4, "Take 1 damage. Deal more damage for each Armor on Oremordilla.",0,3);
        Attacks[2] = new Attack("Roll Up", 0, 1, 0, "Gain +1 Defense and +1 Armor",0,3);
        Attacks[3] = new Attack("Armor Plating", 0, 1, 0, "Gain +2 defense for each Armor, and lose all Armor.",0,3);
        type1 = "Ground";
        type2 = "Metal";
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
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Boulder", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Boulder", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Boulder", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Boulder", false);
            return 0;
        }
        else if (attackNum == 1)
        {
            takeDamage(1 + getDefense(), true, true);
            return cA.getBaseDamage() + getAttack() + getArmor();
        }
        else if (attackNum == 2)
        {
            addArmor(1);
            doTempBuff(1, -1, 1);
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(1, -1, getArmor()*2);
            addArmor(-getArmor());
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
            if(getArmor() != 0)
                return Attacks[attackNum].getBaseDamage() + getAttack() + getArmor()-1;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
