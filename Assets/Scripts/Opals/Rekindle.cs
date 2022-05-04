using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rekindle : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 2;
        priority = 0;
        myName = "Rekindle";
        transform.localScale = new Vector3(0.2f,0.2f,1) * 0.9f;
        offsetX = 0;
        offsetY = 0.1f;
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
        Attacks[0] = new Attack("Sole Focus", 0, 0, 0, "<Passive>\nRekindle can only curse one Opal at a time, when a second Opal is cursed, the previous Opal loses its curse.");
        Attacks[1] = new Attack("Resurrect", 1, 1, 0, "If the Opal cursed by Rekindle is now dead, respawn it on the target tile. It is no longer cursed.");
        Attacks[2] = new Attack("Fiery Presence", 0, 1, 0, "Surround the Opal cursed by Rekindle in Flames. They gain +4 attack for 1 turn.");
        Attacks[3] = new Attack("Memorize", 1, 1, 0, "Curse the target Opal. Place Flames under Rekindle and the target. Rekindle loses -2 speed for 2 turns.");
        type1 = "Fire";
        type2 = "Spirit";
        og = true;
    }

    public override void onStart()
    {
        if((cursed.Count > 0 && cursed[0].getDead()))
        {
            Attacks[1] = new Attack("Resurrect", 1, 1, 0, "If the Opal cursed by Rekindle is now dead, respawn it on the target tile. It is no longer cursed. Currently will resurrect "+ cursed[0].getMyName()+".");
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Cleansing Flame
        {
            return 0;
        }
        else if (attackNum == 1) //World Burn
        {
            return 0;
        }
        else if (attackNum == 2) //Healing Heat
        {
            if (cursed.Count < 1)
                return 0;
            OpalScript resurrect = cursed[0];
            foreach (TileScript t in resurrect.getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Fire", false);
            }
            resurrect.doTempBuff(0, 1, 4);
            return 0;
        }
        else if (attackNum == 3)
        {
            foreach(OpalScript o in cursed)
            {
                o.getCursedBy().Remove(this);
            }
            cursed.Clear();

            target.setCursed(this);

            boardScript.setTile(target,"Fire", false);
            boardScript.setTile(this, "Fire", false);

            doTempBuff(2, 3, -2);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Cleansing Flame
        {

        }
        else if (attackNum == 1) //World Burn
        {
            if (cursed.Count < 1)
            {
                return 0;
            }
            OpalScript resurrect = cursed[0];
            resurrect.setOpal(resurrect.getTeam());
            resurrect.setNotDead();
            resurrect.setPos((int)target.getPos().x, (int)target.getPos().z);
            resurrect.doMove((int)target.getPos().x, (int)target.getPos().z, 0);

            foreach (OpalScript o in cursed)
            {
                o.getCursedBy().Remove(this);
            }
            cursed.Clear();

            Attacks[1] = new Attack("Resurrect", 1, 1, 0, "If the Opal cursed by Rekindle is now dead, respawn it on the target tile. It is no longer cursed.");
        }
        else if (attackNum == 2) //Healing Heat
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
        if(attackNum == 1 && (cursed.Count <= 0 || !cursed[0].getDead()))
        {
            return -1;
        }
        else if(attackNum == 1 && target.currentPlayer == null)
        {
            return 1;
        }
        if (target.currentPlayer != null && attackNum != 1)
        {
            return 0;
        }
        return -1;
    }
}
