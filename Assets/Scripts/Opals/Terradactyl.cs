using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terradactyl : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 4;
        defense = 2;
        speed = 2;
        priority = 2;
        myName = "Terradactyl";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
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
        Attacks[0] = new Attack("Sky Drop", 2, 1, 3, "Place boulders in a 2 tile radius. Opals in radius are damaged instead.", 2);
        Attacks[1] = new Attack("Momentum", 1, 1, 0, "Place a boulder on an adjacent tile and gain +2 attack and +2 speed for 1 turn.");
        Attacks[2] = new Attack("Catapult", 15, 1, 0, "Place boulders on all adjacent tiles and move anywhere on the map.");
        Attacks[3] = new Attack("Fragmented Stone", 2, 4, 0, "Destroy a target Boulder and gain +1 speed.");
        type1 = "Air";
        type2 = "Ground";
        og = true;
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Sky Drop
        {
            if(target == this)
            {
                return 0;
            }
        }
        else if (attackNum == 1) //Momentum
        {
            return 0;
        }
        else if (attackNum == 2) //Catapult
        {

        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Sky Drop
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Boulder", false);
        }
        else if (attackNum == 1) //Momentum
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Boulder", false);
            doTempBuff(0, 2, 2);
            doTempBuff(2, 2, 2);
            return 0;
        }
        else if (attackNum == 2) //Catapult
        {
            Vector2 lp = new Vector2(getPos().x, getPos().z);
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (Mathf.Abs(i) != Mathf.Abs(j))
                    {
                        getBoard().setTile((int)lp.x + i, (int)lp.y + j, "Boulder", false);
                    }
                }
            }
            return 0;
        }else if(attackNum == 3)
        {
            if(target.type == "Boulder")
            {
                boardScript.setTile(target, "Grass", true);
                doTempBuff(2, -1, 1);
            }
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
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
        if((attackNum == 3 || attackNum == 1) && target.currentPlayer != null)
        {
            return -1;
        }
        return 0;
    }
}
