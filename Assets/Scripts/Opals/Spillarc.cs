using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spillarc : OpalScript
{
    private int refreshCount = 0;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 3;
        priority = 3;
        myName = "Spillarc";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = 0;
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
        Attacks[0] = new Attack("Prismatic Soothe", 0, 0, 0, "<Passive>\nWhen Spillarc moves to a Flood tile, adjacent Opals also in Flood gain +2 defense for 1 turn.");
        Attacks[1] = new Attack("Refresh", 1, 3, 0, "<Free Ability>\nHeal a target in Flood 2 health. (May use this twice a turn.)", 0, 3);
        Attacks[1].setFreeAction(true);
        Attacks[2] = new Attack("Encolour", 1, 3, 0, "Give the target +2 attack and +2 defense for 2 turns.", 0,3);
        Attacks[3] = new Attack("Water Down",0,1,0,"Overheal self by 6 health. Place flood at your feet and on adjacent tiles.",0,3);
        type1 = "Water";
        type2 = "Light";
        og = true;
    }

    public override void onMove(int x, int z)
    {
        if(!getDead() && boardScript.tileGrid[x,z].type == "Flood")
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if(t.type == "Flood" && t.currentPlayer != null)
                {
                    t.currentPlayer.doTempBuff(1, 1, 2);
                    StartCoroutine(playFrame("attack", 3));
                }
            }
        }
    }

    public override void onStart()
    {
        refreshCount = 0;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Prismatic Soothe
        {
            
        }
        else if (attackNum == 1) //Encolour
        {
            if (refreshCount < 2)
            {
                target.doHeal(2, false);
                refreshCount++;
            }
        }
        else if (attackNum == 2) //Soak
        {
            target.doTempBuff(0, 2, 2);
            target.doTempBuff(1, 2, 2);
            return 0;
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
        else if (attackNum == 3) //Soak
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

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 && refreshCount >= 2)
        {
            return -1;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
