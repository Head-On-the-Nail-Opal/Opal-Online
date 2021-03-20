using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbskull : OpalScript
{

    private Spiritch dimstingPrefab;

    public override void onAwake()
    {
        dimstingPrefab = Resources.Load<Spiritch>("Prefabs/Opals/Spiritch");
    }

    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 3;
        priority = 7;
        myName = "Numbskull";
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
        offsetY = 0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Haunting", 0, 0, 0, "<Passive>\n Enemy opals killed by Numbskull leave a Spiritch in their place.");
        Attacks[1] = new Attack("Gambit", 0, 1, 10, "Deal damage in an area of effect. Die after using this attack.",1);
        Attacks[2] = new Attack("Shade Grasp", 1, 1, 6, "You may move after using this ability.");
        Attacks[3] = new Attack("Fateful Reward", 0, 1, 0, "Take 3 damage. Gain +3 attack.");
        type1 = "Dark";
        type2 = "Swarm";
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = false;
        }
        else if (attackNum == 1)
        {
            moveAfter = true;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {
            takeDamageBelowArmor(health, false, true);
        }
        else if (attackNum == 2) //Grass Cover
        {
            moveAfter = true;
        }else if(attackNum == 3)
        {
            takeDamage(3, false, true);
            doTempBuff(0, -1, 3);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {

        }
        else if (attackNum == 2) //Grass Cover
        {

            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    void setIt(OpalScript os)
    {
        currentTile.standingOn(os);
    }

    public override int getAttackDamage(int attackNum, TileScript target)
    {
        if (target.currentPlayer == null)
            return 0;
        if (attackNum == 0)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 1)
        {

        }
        else if (attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }else if(attackNum == 3)
        {
            return 3;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }


}
