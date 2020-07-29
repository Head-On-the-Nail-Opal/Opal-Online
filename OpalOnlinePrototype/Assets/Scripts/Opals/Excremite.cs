using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Excremite : OpalScript
{
    private Dunglet dungletPrefab;
    private int boulderDamage = 0;

    private void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        transform.position = new Vector3(5, 0.5f, 5);
        anim = GetComponent<Animator>();
        damRes = Resources.Load<DamageResultScript>("Prefabs/AttackResult");
        burningParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassiveBurn");
        poisonedParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassivePoison");
        dungletPrefab = Resources.Load<Dunglet>("Prefabs/SubOpals/Dunglet");
    }

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 1;
        priority = 6;
        myName = "Excremite";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 1.1f;
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
        Attacks[0] = new Attack("Crappy Load", 2, 4, 0, "Summon a Dunglet");
        Attacks[1] = new Attack("Brown Coat", 0, 1, 0, "Adjacent Opals gain +3 defense for two turns. Swarm types gain +2 attack for two turns as well.", 1);
        Attacks[2] = new Attack("Smelly Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles (0)");
        Attacks[3] = new Attack("Rock Carapace", 2, 1, 0, "Break a Boulder and gain +3 defense.");
        type1 = "Ground";
        type2 = "Swarm";
    }

    public override void onStart()
    {
        boulderDamage = 0;
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if (getPos().x + i > -1 && getPos().x + i < 10 && getPos().z + j > -1 && getPos().z + j < 10)
                {
                    if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type == "Boulder")
                    {
                        boulderDamage++;
                    }
                }
            }
        }
        Attacks[2] = new Attack("Smelly Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles ("+boulderDamage+")");
    }

    public override void onMove(int distanceMoved)
    {
        boulderDamage = 0;
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (getPos().x + i > -1 && getPos().x + i < 10 && getPos().z + j > -1 && getPos().z + j < 10)
                {
                    if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type == "Boulder")
                    {
                        boulderDamage++;
                    }
                }
            }
        }
        Attacks[2] = new Attack("Smelly Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles (" + boulderDamage + ")");
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
                target.doTempBuff(1, 1, 3);
                if (target.getMainType() == "Swarm" || target.getSecondType() == "Swarm")
                {
                    target.doTempBuff(0, -1, 2);
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {

            return cA.getBaseDamage() * boulderDamage + getAttack();
        }else if(attackNum == 3)
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            int minionCount = 0;
            foreach(OpalScript o in boardScript.gameOpals)
            {
                if(o.getMyName() == "Dunglet" && o.getDead() == false)
                    minionCount++;
            }
            if (minionCount < 4) {
                DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                temp.setUp(minionCount + 1, swarmLimit);
                Dunglet opalTwo = Instantiate<Dunglet>(dungletPrefab);
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
                target.standingOn(opalTwo);
                opalTwo.setSkipTurn(true);
            }
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
          
        }else if(attackNum == 3)
        {
            if(target.type == "Boulder")
            {
                boardScript.setTile(target, "Grass", true);
                doTempBuff(1, -1, 3);
            }
        }
        return cA.getBaseDamage() + getAttack();
    }

    void setIt(OpalScript os)
    {
        currentTile.standingOn(os);
    }

    public override List<OpalScript> getOplets()
    {
        List<OpalScript> temp = new List<OpalScript>();
        temp.Add(dungletPrefab);
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
            return Attacks[attackNum].getBaseDamage() * boulderDamage + getAttack() - target.currentPlayer.getDefense();
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 && target.currentPlayer == null)
        {
            return 0;
        }
        if(attackNum == 3 && target.type == "Boulder")
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum!= 0 && attackNum != 3)
        {
            return 0;
        }
        return -1;
    }
}
