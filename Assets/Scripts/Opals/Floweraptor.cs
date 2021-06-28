using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floweraptor : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 4;
        defense = 0;
        speed = 4;
        priority = 8;
        myName = "Floweraptor";
        transform.localScale = new Vector3(3.5f, 3.5f, 1);
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
        Attacks[0] = new Attack("Death's Shriek", 0, 0, 0, "<Passive>\n On Floweraptor's death, all Opals cursed by it lose -10 attack for 1 turn.");
        Attacks[1] = new Attack("Bloom", 0, 1, 0, "Place a Growth at your feet and gain +1 speed.");
        Attacks[2] = new Attack("Drain Nutrients", 0, 1, 0, "Gain +2 attack for each cursed target. They each lose -2 attack.");
        Attacks[3] = new Attack("Stalk", 1, 1, 8, "Curse the target, you may move after attacking.");
        type1 = "Grass";
        type2 = "Spirit";
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
            moveAfter = false;
        }
        else if (attackNum == 3)
        {
            moveAfter = true;
        }
    }

    public override void onDie()
    {
        foreach(OpalScript o in cursed)
        {
            if (!o.getDead())
            {
                o.doTempBuff(0, 1, -10);
            }
        }
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
            boardScript.setTile(target,"Growth", false);
            doTempBuff(2, -1, 1);
            return 0;
        }
        else if (attackNum == 2)
        {
            foreach (OpalScript o in cursed)
            {
                if (!o.getDead())
                {
                    o.doTempBuff(0,-1,-2);
                    doTempBuff(0, -1, 2);
                }
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            if (!cursed.Contains(target))
                cursed.Add(target);
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
        return base.checkCanAttack(target, attackNum);
    }
}
