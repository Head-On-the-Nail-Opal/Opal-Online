using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechalodon : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 2;
        priority = 2;
        myName = "Mechalodon";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.3f;
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
        Attacks[0] = new Attack("Fish Outta", 0, 0, 0, "<Passive>\nWhen Mechalodon ends its turn outside of Flood, it loses all stat changes and armor. ");
        Attacks[1] = new Attack("Submerge", 0, 1, 0, "Gain +1 armor, +2 attack and +1 speed. Tidal: +3 attack and +2 speed",0,3);
        Attacks[1].setTidalD("Gain +1 armor, +3 attack and +2 speed. Tidal: +2 attack and +1 speed");
        Attacks[2] = new Attack("Gnash", 1, 3, 4, "<Water Rush>\nDeal damage, add two damage for each point of Armor you have.",0,6);
        Attacks[3] = new Attack("Teary Flop", 0, 1, 0,"Place a Flood under Mechalodon and on adjacent tiles. Then gain +2 attack.",0,3);
        type1 = "Water";
        type2 = "Metal";
        og = true;
    }

    public override void onEnd()
    {
        if(boardScript.tileGrid[(int)getPos().x, (int)getPos().z] != null && boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type != "Flood")
        {
            clearBuffs();
            addArmor(-getArmor());
        }
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Ravage
        {

            return cA.getBaseDamage() + getAttack() + (int)(Mathf.Abs(getPos().x - target.getPos().x) + Mathf.Abs(getPos().z - target.getPos().z))-2;
        }
        else if (attackNum == 1) //Submerge
        {

            addArmor(1);
            doTempBuff(0, -1, 2);
            doTempBuff(2, -1, 1);
            if (getTidal())
            {
                doTempBuff(0, -1, 1);
                doTempBuff(2, -1, 1);
            }
            return 0;
        }
        else if (attackNum == 2) //Gnash
        {
            return cA.getBaseDamage() + getAttack() + 2*getArmor();
        }
        else if (attackNum == 3) //Gnash
        {
            boardScript.setTile(this, "Flood", false);
            foreach(TileScript t in getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Flood", false);
            }
            doTempBuff(0, -1, 2);
            return 0;
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
        else if (attackNum == 3) //Gnash
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + 2*getArmor();
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
