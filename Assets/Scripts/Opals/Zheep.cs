using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zheep : OpalScript
{
    private CursorScript cs;
    private int currentStored = 0;
    private int currentMove = 0;
    private ParticleSystem chrg;

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 3;
        priority = 8;
        myName = "Zheep";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 1.1f;
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

        if(pl != null)
        {
            cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        }
        Attacks[0] = new Attack("Stored Charge", 0, 0, 0, "<Passive>\n When Zheep takes damage the attacker is dealt damage equal to Zheep's charge. Zheep loses a charge.");
        Attacks[1] = new Attack("Discharge", 3, 4, 1, "Costs 2 charge. Gain +1 attack for the amount of damage dealt by Stored Charge last turn (0). Then deal 1 damage.");
        Attacks[2] = new Attack("Static", 0, 1, 0, "Gain +1 charge for the distance moved this turn.");
        Attacks[3] = new Attack("Steel Wool", 0, 1, 0, "Costs 3 charge. Gain +1 armor.");
        type1 = "Electric";
        type2 = "Metal";
        chrg = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/ChargedUp");
    }

    public override void onStart()
    {
        cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        currentMove = 0;
        Attacks[1] = new Attack("Discharge", 3, 4, 1, "Costs 2 charge. Gain +1 attack for the amount of damage dealt by Stored Charge last turn ("+ currentStored + "). Then deal 1 damage.");
        Attacks[2] = new Attack("Static", 0, 1, 0, "Gain +1 charge for the distance moved this turn.");
    }

    public override void onMove(PathScript p)
    {

    }

    public override void onEnd()
    {
        currentStored = 0;
    }


    public override void onDamage(int dam)
    {
        if (getCharge() > 0 && dam > getDefense())
        {
            if (cs.getCurrentOpal() != this)
            {
                cs.getCurrentOpal().takeDamage(getCharge(), false, true);
                currentStored += getCharge();
            }
            Instantiate<ParticleSystem>(chrg, this.transform);
            doCharge(-1);
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Stored Energy
        {
            return 0;
        }
        else if (attackNum == 1) //Discharge
        {
            if (getCharge() > 1)
            {
                doCharge(-2);
                boardScript.setChargeDisplay(getCharge());
                doTempBuff(0, -1, currentStored);
                return cA.getBaseDamage() + getAttack();
            }
            return 0;
        }
        else if (attackNum == 2) //Static
        {
            doCharge(getSpeed() - cs.getDistance());
            boardScript.setChargeDisplay(getCharge());

            return 0;
        }else if(attackNum == 3)
        {
            if(getCharge() > 2)
            {
                doCharge(-3);
                boardScript.setChargeDisplay(getCharge());
                addArmor(1);
            }
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() + currentStored;
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
