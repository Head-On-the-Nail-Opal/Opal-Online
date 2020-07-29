using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wingnition : OpalScript
{
    private Bombat bombatPrefab;
    private int boulderDamage = 0;

    private void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        transform.position = new Vector3(5, 0.5f, 5);
        anim = GetComponent<Animator>();
        bombatPrefab = Resources.Load<Bombat>("Prefabs/SubOpals/Bombat");
        burningParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassiveBurn");
        poisonedParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassivePoison");
        damRes = Resources.Load<DamageResultScript>("Prefabs/AttackResult");
    }

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 3;
        priority = 7;
        myName = "Wingnition";
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
        Attacks[0] = new Attack("Call Pup", 1, 0, 0, "Summon an exploding Bombat");
        Attacks[1] = new Attack("Wing Flap", 0, 1, 0, "Push adjacent Opals away until they hit something. They gain +3 attack for 2 turns.");
        Attacks[2] = new Attack("Sonar", 2, 1, 0, "Heal a target 5 health. If they're Swarm type then also give them +3 attack for 2 turns.");
        Attacks[3] = new Attack("Swiften", 2, 1, 0, "Give a target +2 speed for 2 turns. They gain Lift.");
        type1 = "Air";
        type2 = "Swarm";
    }

    public override void onStart()
    {
    }

    public override void onMove(int distanceMoved)
    {
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
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (!(i == 0 && j == 0) && (i == 0 || j == 0))
                    {
                        if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.doTempBuff(0, 2, 3);
                            if (i == 0 && j == -1)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(10, false, false);
                            if (i == 0 && j == 1)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(10, false, true);
                            if (i == 1 && j == 0)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(10, true, true);
                            if (i == -1 && j == 0)
                                boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.nudge(10, true, false);
                        }
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {

            target.doHeal(5, false);
            if (target.getMainType() == "Swarm" || target.getSecondType() == "Swarm")
            {
                target.doTempBuff(0, 2, 3);
            }
            return 0;
        }else if(attackNum == 3)
        {
            target.doTempBuff(2, 2, 2);
            target.setLifted(true);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            int minionCount = 0;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Bombat" && o.getDead() == false)
                    minionCount++;
            }
            if (minionCount < 4)
            {
                DamageResultScript temp = Instantiate<DamageResultScript>(damRes, this.transform);
                temp.setUp(minionCount+1, swarmLimit);
                Bombat opalTwo = Instantiate<Bombat>(bombatPrefab);
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
        temp.Add(bombatPrefab);
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
            return 0;
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
        if (target.currentPlayer != null && attackNum != 0)
        {
            return 0;
        }
        return -1;
    }
}
