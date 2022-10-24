using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Verminfection : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 1;
        defense = 4;
        speed = 3;
        priority = 8;
        myName = "Verminfection";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Infectious", 0, 0, 0, "<Passive>\nWhen Verminfection takes damage, it's poison is spread to surrounding Opals.");
        Attacks[1] = new Attack("Bad Turn", 0, 1, 0, "Heal 8 health. Poison yourself. If already poisoned then raise damage taken when poisoned by 4.",0,3);
        Attacks[2] = new Attack("Tainted Bite", 1, 1, 5, "Target gains all stat changes from Verminfection. Verminfection loses all stat changes.",0,3);
        Attacks[3] = new Attack("Cough", 0, 1, 0, "<Free Ability> Take damage from your poison.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Dark";
        type2 = "Dark";
    }

    public override void onStart()
    {
    }

    public override void onMove(int d)
    {

    }

    public override void onDamage(int dam)
    {
        if(dead == false && dam > 0 && poisoned)
        {
            StartCoroutine(playFrame("attack", 5));
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (getPos().x + i > -1 && getPos().x + i < 10 && getPos().z + j > -1 && getPos().z + j < 10)
                    {
                        if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != this)
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.setPoison(true);
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.setPoisonCounter(poisonCounter, true);
                        }
                        boardScript.callParticles("poison", new Vector3((int)getPos().x + i, (int)getPos().y, (int)getPos().z + j));
                    }
                }
            }
        }
    }

    public override void onDie()
    {
        base.onDie();
        if (poisoned)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (getPos().x + i > -1 && getPos().x + i < 10 && getPos().z + j > -1 && getPos().z + j < 10)
                    {
                        if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != this)
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.setPoison(true);
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.setPoisonCounter(poisonCounter, true);
                        }
                        boardScript.callParticles("poison", new Vector3((int)getPos().x + i, (int)getPos().y, (int)getPos().z + j));
                    }
                }
            }
        }
    }



    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Inferno
        {
            return 0;
        }
        else if (attackNum == 1) //Ignite
        {
            doHeal(8, false);
            if(poisoned == true)
            {
                setPoisonCounter(poisonCounter+4, true);
            }
            setPoison(true);
            return 0;
        }
        else if (attackNum == 2) //Incendiary
        {
            List<TempBuff> temp2 = new List<TempBuff>();

            foreach (TempBuff t in getBuffs())
            {
                temp2.Add(new TempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount()));
            }
            clearBuffs();
            foreach (TempBuff t in temp2)
            {
                target.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
            }
        }
        else if (attackNum == 3) //Incendiary
        {
            if(getPoison())
                takeDamage(getPoisonCounter(), false, false);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Inferno
        {
            return 0;
        }
        else if (attackNum == 1) //Ignite
        {
            return 0;
        }
        else if (attackNum == 2) //Incendiary
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense() - getTempBuff(0) - getTempBuff(1);
        }
        else if (attackNum == 3)
        {
            return getPoisonCounter();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
