using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbacle : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 3;
        priority = 6;
        myName = "Bubbacle";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        offsetX = 0;
        offsetY = -0.1f;
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
        Attacks[0] = new Attack("Frail", 0, 0, 0, "<Passive>\n When Bubbacle takes damage, place water tiles at its feet and on adjacent tiles. Useful!");
        Attacks[1] = new Attack("Bubble Blast", 4, 4, 6, "Deal 1 extra damage for every 2 health points you're missing. Place flood tiles to your target.");
        Attacks[2] = new Attack("Imbibe", 0, 1, 0, "If standing in Flood, heal 6 health. If not, take 1 damage.");
        Attacks[3] = new Attack("Big Bubble", 2, 4, 6, "Take 1 damage. Place Flood tiles adjacent and under target.");
        type1 = "Water";
        type2 = "Water";
        og = true;
    }

    public override void onDamage(int dam)
    {
        if (dam > 0)
        {
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Flood", false);
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Frail
        {
            return 0;
        }
        else if (attackNum == 1) //Bubbleblast
        {
            string direct = "right";
            int dist = (int)getPos().x - (int)target.getPos().x;
            if (dist == 0)
            {
                direct = "up";
                dist = (int)getPos().z - (int)target.getPos().z;
            }
            if (dist < 0)
            {
                if (direct == "right")
                {
                    direct = "left";
                }
                else if (direct == "up")
                {
                    direct = "down";
                }
                dist = Mathf.Abs(dist);
            }
            for (int i = 0; i <= dist; i++)
            {
                if (direct == "right")
                {
                    getBoard().setTile((int)getPos().x - i, (int)getPos().z, "Flood", false);
                }
                else if (direct == "left")
                {
                    getBoard().setTile((int)getPos().x + i, (int)getPos().z, "Flood", false);
                }
                else if (direct == "up")
                {
                    getBoard().setTile((int)getPos().x, (int)getPos().z - i, "Flood", false);
                }
                else if (direct == "down")
                {
                    getBoard().setTile((int)getPos().x, (int)getPos().z + i, "Flood", false);
                }
            }
            return cA.getBaseDamage() + getAttack() + ((getMaxHealth() - getHealth())/2);
        }
        else if (attackNum == 2) //Imbibe
        {
            if(boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Flood")
            {
                doHeal(6, false);
            }
            else
            {
                takeDamage(1, false, true);
            }
            return 0;
        }else if(attackNum == 3)
        {
            takeDamage(1, false, true);
            boardScript.setTile(target, "Flood", false);
            boardScript.setTile((int)target.getPos().x + 1, (int)target.getPos().z, "Flood", false);
            boardScript.setTile((int)target.getPos().x - 1, (int)target.getPos().z, "Flood", false);
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z + 1, "Flood", false);
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z - 1, "Flood", false);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 2)
        {
            if(target.type != "Flood")
            {
                return 1;
            }
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
