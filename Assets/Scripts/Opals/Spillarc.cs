using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spillarc : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 2;
        priority = 3;
        myName = "Spillarc";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0.15f;
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
        Attacks[0] = new Attack("Prismatic Soothe", 1, 3, 0, "<Water Rush>\nHeal another Opal 8 health. Place Flood surrounding the target.");
        Attacks[1] = new Attack("Encolour", 1, 3, 0, "<Water Rush>\nBuff another Opal by +3 attack and +2 defense for 1 turn");
        Attacks[2] = new Attack("Soak", 2, 1, 6, "If the target Opal is not on Flood, replace the tile they're standing on with Flood.");
        Attacks[3] = new Attack("Water Down",0,1,0,"Overheal self by 6 health. Place flood at your feet and on adjacent tiles.");
        type1 = "Water";
        type2 = "Light";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Prismatic Soothe
        {
            target.doHeal(8, false);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    getBoard().setTile((int)target.getPos().x + i, (int)target.getPos().z + j, "Flood", false);
                }
            }
            return 0;
        }
        else if (attackNum == 1) //Encolour
        {
                target.doTempBuff(0, 1, 3);
                target.doTempBuff(2, 1, 2);
        }
        else if (attackNum == 2) //Soak
        {

        }
        else if (attackNum == 3) //Encolour
        {
            doHeal(6, true);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Flood", false);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Prismatic Soothe
        {
        }
        else if (attackNum == 1) //Encolour
        {
            return 0;
        }
        else if (attackNum == 2) //Soak
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
            if(target.type == "Flood")
            {
                return Attacks[attackNum].getBaseDamage()  + getAttack() - target.currentPlayer.getDefense();
            }
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
