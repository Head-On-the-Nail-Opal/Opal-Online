using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gilsplish : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 2;
        priority = 0;
        myName = "Gilsplish";
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
        Attacks[0] = new Attack("Gills", 0, 0, 0, "<Passive>\n At the start of its turn, Gilsplish loses -10 health outside of Flood, or gains +5 health in Flood");
        Attacks[1] = new Attack("Splish", 4, 4, 5, "If the target is in Flood, this attack heals them instead of dealing damage.");
        Attacks[2] = new Attack("Splurt", 5, 4, 4, "If Gilpslish in in Flood, then this attack places Flood under and adjacent to the target.");
        Attacks[3] = new Attack("Flop", 0, 1, 0, "Gain +2 speed for 1 turn. Place Flood under Gilsplish and on adjacent tiles.");
        type1 = "Water";
        type2 = "Water";
    }

    public override void onStart()
    {
        base.onStart();
        if(currentTile != null && currentTile.type == "Flood")
        {
            doHeal(5, false);
        }
        else
        {
            takeDamage(10, false, true);
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
            if(target.getCurrentTile().type == "Flood")
            {
                target.doHeal(cA.getBaseDamage() + getAttack(), false);
                return 0;
            }
        }
        else if (attackNum == 2)
        {
            if(currentTile.type == "Flood")
            {
                boardScript.setTile(target, "Flood", false);
                boardScript.setTile((int)target.getPos().x + 1, (int)target.getPos().z, "Flood", false);
                boardScript.setTile((int)target.getPos().x - 1, (int)target.getPos().z, "Flood", false);
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z + 1, "Flood", false);
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z - 1, "Flood", false);
            }
        }
        else if (attackNum == 3)
        {
            doTempBuff(2, 1, 2);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Flood", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Flood", false);
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
            if(target.type == "Flood")
            {
                return 0;
            }
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
