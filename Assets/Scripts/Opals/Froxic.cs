using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froxic : OpalScript
{

    int poisonedTeammates = 0;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 9;
        myName = "Froxic";
        transform.localScale = new Vector3(3f, 3f, 2) * 1f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Contaminate", 0, 0, 0, "<Passive>\nWhen Froxic takes damage, poison all Opals in the same body of water as it.");
        Attacks[1] = new Attack("Secretion", 1, 3, 0, "<Water Rush>\nGive target +3 attack and +4 defense for 1 turn. Also heal them by 4. Has one more use for each teammate currently poisoned.",0,3);
        Attacks[2] = new Attack("Neurotoxin", 1, 3, 0, "<Water Rush>\nIncrease target's poison damage by 3. Tidal: By 5 instead.",0,3);
        Attacks[2].setTidalD("<Water Rush>\nIncrease target's poison damage by 5. Tidal: By 3 instead.");
        Attacks[3] = new Attack("Toxic Shock", 1, 4, 5, "Deal damage. If every living non-plague type Opal in the match is poisoned, deal 20 more damage.",0,3);
        type1 = "Plague";
        type2 = "Water";
    }

    public override void onStart()
    {
        checkPoisonedTeammates();
    }

    public override void onDamage(int dam)
    {
        if(currentTile != null && currentTile.type == "Flood")
        {
           foreach(OpalScript o in getOpalsInSameFlood())
            {
                o.setPoison(true);
                StartCoroutine(playFrame("attack", 3));
            }
        }

        checkPoisonedTeammates();
    }

    private void checkPoisonedTeammates()
    {
        poisonedTeammates = 0;
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (o.getTeam() == getTeam() && !o.getDead() && o.getPoison())
            {
                poisonedTeammates++;
            }
        }
        Attacks[1].setUses(poisonedTeammates);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            target.doTempBuff(0, 1, 3);
            target.doTempBuff(1, 1, 4);
            target.doHeal(4, false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (getTidal())
                target.setPoisonCounter(getPoisonCounter() + 5, true);
            else
                target.setPoisonCounter(getPoisonCounter() + 3, true);
            return 0;
        }else if(attackNum == 3)
        {
            foreach(OpalScript o in boardScript.gameOpals)
            {
                if (!(o.getDead() || o.getPoison() || o.getMainType() == "Plague" || o.getSecondType() == "Plague"))
                {
                    return cA.getBaseDamage() + getAttack();
                }
            }
            return 20 + cA.getBaseDamage() + getAttack();
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
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
        }else if(attackNum == 3)
        {
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (!(o.getDead() || o.getPoison() || o.getMainType() == "Plague" || o.getSecondType() == "Plague"))
                {
                    return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
                }
            }
            return 20 + Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
