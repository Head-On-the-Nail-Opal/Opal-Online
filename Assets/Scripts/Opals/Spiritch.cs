using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiritch : OpalScript
{
    private Numbskull dimstingPrefab;


    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 9;
        myName = "Spiritch";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
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
        dimstingPrefab = Resources.Load<Numbskull>("Prefabs/SubOpals/Numbskull");
        Attacks[0] = new Attack("Restless", 0, 0, 0, "<Passive>\n On death, spawn a Numbskull to replace Spiritch.");
        Attacks[1] = new Attack("Unholy", 0, 1, 0, "Adjacent Opals gain +2 attack. If they are Swarm types they gain +1 speed as well.", 1);
        Attacks[2] = new Attack("Undead Legion", 1, 0, 0, "Spawn a Numbskull. Take 5 damage.");
        Attacks[3] = new Attack("Control Fate", 0, 1, 0, "<Free Ability>\n Take 10 damage.");
        Attacks[3].setFreeAction(true);
        type1 = "Dark";
        type2 = "Swarm";
    }

    public override void onDie()
    {
        int minionCount = 0;
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (o.getMyName() == "Numbskull" && o.getDead() == false)
                minionCount++;
        }
        if (minionCount < 4)
        {
            DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
            temp.setUp(minionCount + 1, swarmLimit);
            Numbskull opalTwo = Instantiate<Numbskull>(dimstingPrefab);
            opalTwo.setOpal(player); // Red designates player 1, Blue designates player 2
            opalTwo.setPos((int)getPos().x, (int)getPos().z);
            getBoard().gameOpals.Add(opalTwo);
            getBoard().addToUnsorted(opalTwo);
            if (player == "Red")
            {
                getBoard().p2Opals.Add(opalTwo);
            }
            else if (player == "Green")
            {
                getBoard().p3Opals.Add(opalTwo);
            }
            else if (player == "Orange")
            {
                getBoard().p4Opals.Add(opalTwo);
            }
            else
            {
                getBoard().p1Opals.Add(opalTwo);
            }
            opalTwo.setSkipTurn(true);
            //getBoard().sortOpals(getBoard().gameOpals);
            transform.position = new Vector3(-100, -100, -100);
            //currentTile.standingOn(null);
            currentTile.standingOn(opalTwo);
            setIt(opalTwo);
            setDead();
        }
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            if (target.getPos() != getPos())
            {
                target.doTempBuff(0, -1, 2);
                if(target.getMainType() == "Swarm" || target.getSecondType() == "Swarm")
                {
                    target.doTempBuff(0, -1, 1);
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            return 0;
        }else if(attackNum == 3)
        {
            takeDamage(10 + getDefense(), true, true);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            int minionCount = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Numbskull" && o.getDead() == false)
                    minionCount++;
            }
            if (minionCount < 4)
            {
                DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                temp.setUp(minionCount + 1, swarmLimit);
                Numbskull opalTwo = Instantiate<Numbskull>(dimstingPrefab);
                opalTwo.setOpal(player); // Red designates player 1, Blue designates player 2
                opalTwo.setPos((int)target.getPos().x, (int)target.getPos().z);
                getBoard().gameOpals.Add(opalTwo);
                getBoard().addToUnsorted(opalTwo);
                if (player == "Red")
                {
                    getBoard().p2Opals.Add(opalTwo);
                }
                else if (player == "Green")
                {
                    getBoard().p3Opals.Add(opalTwo);
                }
                else if (player == "Orange")
                {
                    getBoard().p4Opals.Add(opalTwo);
                }
                else
                {
                    getBoard().p1Opals.Add(opalTwo);
                }
                opalTwo.setSkipTurn(true);
                target.standingOn(opalTwo);
                takeDamage(5, false, true);
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    void setIt(OpalScript os)
    {
        //currentTile.standingOn(os);
    }

    public override List<OpalScript> getOplets()
    {
        List<OpalScript> temp = new List<OpalScript>();
        temp.Add(dimstingPrefab);
        return temp;
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
            return 5;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 2)
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
