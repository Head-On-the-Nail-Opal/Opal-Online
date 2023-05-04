using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puffsqueak : OpalScript
{
    int fu = 0;
    OpalScript selectedOpal = null;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 4;
        priority = 7;
        myName = "Puffsqueak";
        transform.localScale = new Vector3(3f, 3f, 1);
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
        Attacks[0] = new Attack("Carrying Wind", 4, 1, 0, "Place three Growths. Target Opals gain +1 speed for 1 turn.",0,3);
        Attacks[0].setUses(3);
        Attacks[1] = new Attack("Escape Plant", 1, 1, 0, "Teleport target Opal to any Growth tile.",0,3);
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Seed Blast", 3, 1, 8, "Target gains Lift, place a Growth under their feet",0,3);
        Attacks[3] = new Attack("Puffed Up", 0, 5, 0,"Target an Opal on a Growth. They gain +3 attack and +3 defense for 1 turn, and heal 4 health.",0,3);
        type1 = "Grass";
        type2 = "Air";
    }

    public override void onStart()
    {
        fu = 0;
        Attacks[1].setShape(1);
        Attacks[1].setRange(1);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            target.doTempBuff(2, 1, 1);
            return 0;
        }
        else if (attackNum == 1)
        {

            if(fu == 0)
            {
                selectedOpal = target;
                Attacks[1].setShape(5);
                Attacks[1].setRange(0);
                fu = 1;
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            target.setLifted(true);
        }
        else if (attackNum == 3)
        {
            target.doTempBuff(0, 1, 2);
            target.doTempBuff(1, 1, 2);
            target.doHeal(4, false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 1)
        {
            if (fu == 1 && target.type == "Growth")
            {
                selectedOpal.doMove((int)target.getPos().x, (int)target.getPos().z, 0);
            }
            return 0;
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
            return 0;
        }
        else if (attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() - 2;
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1 || attackNum == 0)
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
