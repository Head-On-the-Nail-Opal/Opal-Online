using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groth : OpalScript
{
    int meditationTurn = 0;
    Vector3 meditationSpot;

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 1;
        priority = 4;
        myName = "Groth";
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
        Attacks[0] = new Attack("Meditation", 0, 0, 0, "<Passive>\n While Groth remains in the same place it will grow larger amounts of growths around it.");
        Attacks[1] = new Attack("Morale Boost", 0, 5, 0, "Give an Opal standing on Growth +5 attack and defense for 1 turn.");
        Attacks[2] = new Attack("Resilience", 0, 1, 0, "Gain +1 defense for each turn spent meditating");
        Attacks[3] = new Attack("Confident Strike", 2, 1, 0, "Deals more damage the longer Groth has been meditating.");
        type1 = "Grass";
        type2 = "Grass";
    }

    public override void onStart()
    {
        if(currentTile.getPos() == meditationSpot)
        {
            meditationTurn++;
            boardScript.setTiles(currentTile, meditationTurn+1, "Growth");
        }
        else
        {
            meditationSpot = currentTile.getPos();
            meditationTurn = 0;
            boardScript.setTiles(currentTile, meditationTurn + 1, "Growth");
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
            target.doTempBuff(0, 1, 5);
            target.doTempBuff(1, 1, 5);
            return 0;
        }
        else if (attackNum == 2)
        {
            doTempBuff(1, -1, meditationTurn);
        }
        else if (attackNum == 3)
        {
            return meditationTurn * 3 + getAttack();
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
            return meditationTurn * 3 + getAttack() - target.currentPlayer.getDefense();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
