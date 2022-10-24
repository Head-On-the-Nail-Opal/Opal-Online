using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prismin : OpalScript
{

    private int restore = 1;
    private int relay = 0;

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 2;
        priority = 2;
        myName = "Prismin";
        //baseSize = new Vector3(0.2f, 0.2f, 1);
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = 0f;
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
        Attacks[0] = new Attack("Restore", 1, 1, 0, "Fully heal the targeted Opal. Prismin has [1] use of this ability.",0,5);
        Attacks[1] = new Attack("Regeneration", 0, 1, 0, "All surrounding Opals gain +4 attack and +4 defense for 1 turn. Heal them each by 3 health.",0,3);
        Attacks[2] = new Attack("Relay", 1, 1, 0, "Give an Opal +3 attack, then give an Opal +3 defense.",0,3);
        Attacks[2].setUses(2);
        Attacks[3] = new Attack("Recover", 0, 1, 0, "Lose -10 health. Gain another use of Restore.",0,3);
        type1 = "Light";
        type2 = "Light";
        og = true;
    }

    public override void onStart()
    {
        Attacks[0] = new Attack("Restore", 1, 1, 0, "Fully heal the targeted Opal. Prismin has ["+restore+"] use of this ability.",0,5);
        relay = 0;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {
            if(restore > 0)
            {
                target.doHeal(target.getMaxHealth() - target.getHealth(), false);
                restore--;
                Attacks[0] = new Attack("Restore", 1, 1, 0, "Fully heal the targeted Opal. Prismin has [" + restore + "] use of this ability.",0,5);
            }
            return 0;
        }
        else if (attackNum == 1) //Restore
        {
            foreach(TileScript t in getSurroundingTiles(false))
            {
                if(t.getCurrentOpal() != null)
                {
                    t.getCurrentOpal().doHeal(3, false);
                    t.getCurrentOpal().doTempBuff(0,1,4);
                    t.getCurrentOpal().doTempBuff(1,1,4);
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            if(relay == 0)
            {
                target.doTempBuff(0, -1, 3);
                relay = 1;
            }
            else
            {
                target.doTempBuff(1, -1, 3);
                relay = 0;
            }
            return 0;
        }
        else if(attackNum == 3)
        {
            takeDamage(10, false, true);
            restore++;
            Attacks[0] = new Attack("Restore", 1, 1, 0, "Fully heal the targeted Opal. Prismin has ["+restore+"] use of this ability.",0,5);
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
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 && target.currentPlayer != null && restore <= 0)
        {
            return -1;
        }
        else if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
