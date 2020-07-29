using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overgroink : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 0;
        myName = "Overgroink";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Ecosystem", 0, 0, 0, "<Passive>\n At the start of its turn, all surrounding Growth tiles buff Overgroink as if it were standing on them.");
        Attacks[1] = new Attack("Boink", 2, 1, 4, "Place a Growth at your feet. Heal the damage dealt.");
        Attacks[2] = new Attack("Nurture", 0, 5, 0, "Give an Opal standing on a Growth +2 attack and +2 defense. You may use this twice.");
        Attacks[2].setUses(2);
        Attacks[3] = new Attack("Transplant", 0, 5, 0, "<Free Ability>\n Heal an Opal standing on a Growth by 2 health. Lose 2 health.");
        Attacks[3].setFreeAction(true);
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onStart()
    {
        int tempFire = checkTiles();
        doTempBuff(0, 1, tempFire * 2);
        doTempBuff(1, 1, tempFire * 2);
    }

    private int checkTiles()
    {
        int num = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type == "Growth")
                {
                    if(!(i == 0 && j == 0))
                        num++;
                }
            }
        }
        return num;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            doHeal(getAttack() + cA.getBaseDamage(), false);
            boardScript.setTile(this, "Growth", false);
        }
        else if (attackNum == 2)
        {
            target.doTempBuff(0, -1, 2);
            target.doTempBuff(1, -1, 2);
            return 0;
        }else if(attackNum == 3)
        {
            target.doHeal(2, false);
            takeDamage(2, false, true);
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
            return 0;
        }
        else if (attackNum == 1)
        {

        }
        else if (attackNum == 2)
        {
            
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
