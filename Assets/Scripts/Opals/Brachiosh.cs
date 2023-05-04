using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brachiosh : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 1;
        priority = 2;
        myName = "Brachiosh";
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
        Attacks[0] = new Attack("Brackish", 0, 0, 0, "<Passive>\nOn Brachiosh's death, all Opals cursed by Brachiosh that are standing in flood take 10 damage,");
        Attacks[1] = new Attack("Brinestorm", 5, 1, 0, "Place a flood tile and curse the Opal standing on the target tile, then repeat on another tile.",0,3);
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Salt Bath", 0, 1, 0, "Cursed Opals currently standing in flood gain +3 to attack and defense.",0,3);
        Attacks[3] = new Attack("Shower", 0, 1, 0, "Surrounding tiles become Flood.",0,3);
        type1 = "Water";
        type2 = "Spirit";
    }

    public override void onDie()
    {
        foreach(OpalScript o in cursed)
        {
            if(o.getDead() != true && o.getCurrentTile() != null && o.getCurrentTile().type == "Flood")
            {
                o.takeDamage(10,false,true);
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
            boardScript.setTile(target, "Flood", false);
            target.setCursed(this);
            return 0;
        }
        else if (attackNum == 2)
        {
            foreach (OpalScript o in cursed)
            {
                if (!o.getDead() && o.getCurrentTile() != null && o.getCurrentTile().type == "Flood")
                {
                    o.doTempBuff(0, -1, 3);
                    o.doTempBuff(1, -1, 3);
                }
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            foreach(TileScript t in getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Flood", false);
            }
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
            boardScript.setTile(target, "Flood", false);
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
        if(attackNum == 1)
        {
            return 0;
        }
        return base.checkCanAttack(target, attackNum);
    }
}
