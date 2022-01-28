using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cottonmaw : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 2;
        myName = "Cottonmaw";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
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
        Attacks[0] = new Attack("Absorbent", 0, 0, 0, "<Passive>\n All incoming stat changes are doubled if Cottonmaw is standing on a Growth, then remove the Growth");
        Attacks[1] = new Attack("Triple Snap", 1, 4, 1, "Deal 1 damage 3 times.");
        Attacks[1].setUses(3);
        Attacks[2] = new Attack("Harvest", 0, 1, 0, "If standing on a Growth then heal 4 health, and place Growths on adjacent tiles.");
        Attacks[3] = new Attack("Triple Plant", 2, 1, 0,"Place Growths on 3 different tiles");
        Attacks[3].setUses(3);
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onStart()
    {
        if (currentTile != null && currentTile.type == "Growth")
        {
            //boardScript.setTile(this, "Grass", true);
        }
    }


    public override void onBuff(TempBuff buff)
    {
        if (currentTile != null && currentTile.type == "Growth")
        {
            buffs.Add(buff);
            handleTempBuffs(false);
        }

        if (currentTile != null && currentTile.type == "Growth")
        {
            boardScript.setTile(this, "Grass", false);
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
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (currentTile.type == "Growth")
            {
                doHeal(4, false);
                boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Growth", false);
                boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Growth", false);
                boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Growth", false);
                boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Growth", false);
            }
            return 0;
        }
        else if (attackNum == 3) //Seed Launch
        {
            boardScript.setTile(target, "Growth", true);
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

        }
        else if (attackNum == 2) //Grass Cover
        {

            return 0;
        }
        else if (attackNum == 3) //Seed Launch
        {
            boardScript.setTile(target, "Growth", true);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
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
        if (attackNum == 3)
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
