using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snugbun : OpalScript
{
    private bool masochist = false;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 0;
        speed = 3;
        priority = 2;
        myName = "Snugbun";
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
        offsetY = -0.2f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Group Snuggles", 0, 0, 0, "<Passive>\n Whenever Snugbun takes damage or is healed, it gains +1 attack for each Opal surrounding Snugbun.");
        Attacks[1] = new Attack("Heavy Cuddles", 0, 1, 0, "Snugbun will gain attack equal to the damage it takes on the next attack that damages it.");
        Attacks[2] = new Attack("Friendly Feint", 1, 1, 5, "Deal 1 extra damage for each Opal surrounding Snugbun. May move after attacking.");
        Attacks[3] = new Attack("Bunny Hop", 3, 1, 0, "<Free Ability>\n Teleport up to 3 tiles away. Take 5 damage.");
        Attacks[3].setFreeAction(true);
        type1 = "Dark";
        type2 = "Dark";
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
        else if (attackNum == 3)
        {
            moveAfter = true;
        }
    }

    public override void onDamage(int dam)
    {
        List<TileScript> friends = getSurroundingTiles(false);
        int num = 0;
        foreach (TileScript t in friends)
        {
            if (t.currentPlayer != null && t.currentPlayer.getMyName() != "Boulder")
            {
                num++;
            }
        }
        doTempBuff(0, -1, num);
        if (masochist)
        {
            doTempBuff(0, -1, dam);
            masochist = false;
        }
    }

    public override void onHeal(int heal)
    {
        List<TileScript> friends = getSurroundingTiles(false);
        int num = 0;
        foreach (TileScript t in friends)
        {
            if (t.currentPlayer != null && t.currentPlayer.getMyName() != "Boulder")
            {
                num++;
            }
        }
        doTempBuff(0, -1, num);
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
            masochist = true;
            return 0;
        }
        else if (attackNum == 2)
        {
            List<TileScript> friends = getSurroundingTiles(false);
            int num = 0;
            foreach(TileScript t in friends)
            {
                if(t.currentPlayer != null)
                {
                    num++;
                }
            }
            return cA.getBaseDamage() + getAttack() + num;
        }
        else if (attackNum == 3)
        {
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
            return 0;
        }
        else if (attackNum == 3)
        {
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
            takeDamage(5+getDefense(), true, true);
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
            List<TileScript> friends = getSurroundingTiles(false);
            int num = 0;
            foreach (TileScript t in friends)
            {
                if (t.currentPlayer != null)
                {
                    num++;
                }
            }
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + num;
        }
        else if (attackNum == 3)
        {
            return 5;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 2)
        {
            if (target.currentPlayer != null)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        if (attackNum == 3)
        {
            if(target.currentPlayer != null)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        return base.checkCanAttack(target, attackNum);
    }
}
