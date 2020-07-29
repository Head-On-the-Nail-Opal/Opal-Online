using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amalgum : OpalScript
{
    private Amal amalPrefab;
    private Gum gumPrefab;
    private int currentUse = 0;
    private Amal tempAmal;

    private void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        transform.position = new Vector3(5, 0.5f, 5);
        anim = GetComponent<Animator>();
        damRes = Resources.Load<DamageResultScript>("Prefabs/AttackResult");
        burningParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassiveBurn");
        poisonedParticle = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/PassivePoison");
        amalPrefab = Resources.Load<Amal>("Prefabs/SubOpals/Amal");
        gumPrefab = Resources.Load<Gum>("Prefabs/SubOpals/Gum");
    }

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 9;
        myName = "Amalgum";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = -0.05f;
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
        Attacks[0] = new Attack("Hatch", 1, 0, 0, "Hatch two void type Opals, Amal and Gum");
        Attacks[0].setUses(2);
        Attacks[1] = new Attack("Dormant", 0, 0, 0, "Does nothing");
        Attacks[2] = new Attack("Dormant", 0, 0, 0, "Does nothing");
        Attacks[3] = new Attack("Dormant", 0, 0, 0, "Does nothing");
        type1 = "Void";
        type2 = "Void";
        og = true;
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
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
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
            if (currentUse == 0)
            {
                Amal opalOne = Instantiate<Amal>(amalPrefab);
                tempAmal = opalOne;
                opalOne.setOpal(player); // Red designates player 1, Blue designates player 2
                opalOne.setPos((int)target.getPos().x, (int)target.getPos().z);
                getBoard().gameOpals.Add(opalOne);
                getBoard().addToUnsorted(opalOne);
                if (player == "Red")
                {
                    getBoard().p2Opals.Add(opalOne);
                }
                else if (player == "Green")
                {
                    getBoard().p3Opals.Add(opalOne);
                }
                else if (player == "Orange")
                {
                    getBoard().p4Opals.Add(opalOne);
                }
                else
                {
                    getBoard().p1Opals.Add(opalOne);
                }
                opalOne.setSkipTurn(true);
                target.standingOn(opalOne);
                currentUse = 1;
            }
            else
            {
                Gum opalTwo = Instantiate<Gum>(gumPrefab);
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
                //getBoard().sortOpals(getBoard().gameOpals);
                target.standingOn(opalTwo);
                tempAmal.setTwin(opalTwo);
                opalTwo.setTwin(tempAmal);
                takeDamage(getHealth(), false, false);
            }
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            return 0;
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
        temp.Add(amalPrefab);
        temp.Add(gumPrefab);
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
        }
        else if (attackNum == 3)
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
