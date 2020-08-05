using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorj : OpalScript
{
    List<string> tiles = new List<string>();
    override public void setOpal(string pl)
    {
        health = 200;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 0;
        myName = "Gorj";
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
        Attacks[0] = new Attack("Unwieldy Smack", 1, 1, 4, "If the target's defense is higher than this attack's damage, gain that much defense and take 25 damage.");
        Attacks[1] = new Attack("Engorge", 0, 1, 0, "Remove special effects from surrounding tiles and under you. Take 50 damage.");
        Attacks[2] = new Attack("Belch", 4, 4, 0, "Place the tiles that you last destroyed with Engorge. Take 10 damage.");
        Attacks[3] = new Attack("Wake Up", 0, 1, 0, "Take 50 damage and gain +1 speed.");
        type1 = "Void";
        type2 = "Void";
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            if(target.getDefense() > cA.getBaseDamage() + getAttack())
            {
                takeDamage(25, false, true);
                setTempBuff(1, -1, target.getDefense());
            }
        }
        else if (attackNum == 1)
        {
            tiles.Clear();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (target.getPos().x + i < 10 && target.getPos().x + i > -1 && target.getPos().z + j < 10 && target.getPos().z + j > -1)
                    {
                        tiles.Add(boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].type);
                        getBoard().setTile((int)target.getPos().x + i, (int)target.getPos().z + j, "Grass", true);
                    }
                }
            }
            takeDamage(50, false, true);
            return 0;
        }
        else if (attackNum == 2)
        {
            int num = 0;
            Vector2 targetPos = new Vector2((int)target.getPos().x, (int)target.getPos().z);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if(tiles[num] != "Grass")
                        getBoard().setTile((int)targetPos.x + i, (int)targetPos.y + j, tiles[num], false);
                    num++;
                }
            }
            takeDamage(10, false, true);
            return 0;
        }
        else if (attackNum == 3)
        {
            takeDamage(50, false, true);
            doTempBuff(2, -1, 1);
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
            int num = 0;
            Vector2 targetPos = new Vector2((int)target.getPos().x, (int)target.getPos().z);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (tiles[num] != "Grass")
                        getBoard().setTile((int)targetPos.x + i, (int)targetPos.y + j, tiles[num], false);
                    num++;
                }
            }
            takeDamage(10, false, true);
            return 0;
        }
        else if (attackNum == 3)
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
        else if (attackNum == 3)
        {
            return 50;
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
