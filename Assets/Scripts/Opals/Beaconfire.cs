using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaconfire : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 2;
        priority = 0;
        myName = "Beaconfire";
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
        Attacks[0] = new Attack("Blaze Rage", 0, 0, 0, "At the end of your turn, adjacent Opals standing on flame gain +4 attack");
        Attacks[1] = new Attack("World Burn", 0, 1, 0, "Light fire on all tiles surrounding Beaconfire.");
        Attacks[2] = new Attack("Healing Heat", 1, 4, 0, "Heal all Opals in an area by 4. If they stand on flame then overheal.",1);
        Attacks[3] = new Attack("Fired Up", 1, 1, 0, "Give an Opal +2 attack and gain +1 speed.");
        type1 = "Fire";
        type2 = "Light";
        og = true;
    }

    public override void onEnd()
    {
        foreach (TileScript t in getSurroundingTiles(true))
        {
            if(t.type == "Fire" && t.currentPlayer != null)
            {
                t.currentPlayer.doTempBuff(0, -1, 4);
            }
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
            foreach (TileScript t in getSurroundingTiles(false))
            {
                boardScript.setTile(t, "Fire", false);
            }
            return 0;
        }
        else if (attackNum == 2) //Healing Heat
        {
            if(target.getCurrentTile() != null && target.getCurrentTile().type == "Fire")
            {
                doHeal(4, true);
            }
            else
            {
                doHeal(4, false);
            }
            return 0;
        }
        else if (attackNum == 3)
        {
            doTempBuff(2, -1, 1);
           target.doTempBuff(0, -1, 2);
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
            if (target.type != "Grass")
            {
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", true);
            }
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
        if (attackNum == 1)
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
