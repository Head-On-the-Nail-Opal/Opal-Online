using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiritch : OpalScript
{
    private Numbskull dimstingPrefab;


    public override void onAwake()
    {
        dimstingPrefab = Resources.Load<Numbskull>("Prefabs/SubOpals/Numbskull");
    }

    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 9;
        myName = "Spiritch";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Restless", 0, 0, 0, "<Passive>\n On death, spawn a Numbskull to replace Spiritch.");
        Attacks[1] = new Attack("Unholy", 0, 1, 0, "Adjacent Opals gain +2 attack. If they are Swarm types they gain +1 speed as well.", 1,3);
        Attacks[2] = new Attack("Skull Roll", 1, 0, 0, "Spawn a Numbskull. Take 5 damage.",0,3);
        Attacks[3] = new Attack("Control Fate", 0, 1, 0, "<Free Ability>\n Take 10 damage.");
        Attacks[3].setFreeAction(true);
        type1 = "Spirit";
        type2 = "Swarm";
    }

    public override void onDeathTile(TileScript tile)
    {
        spawnOplet(dimstingPrefab, tile);
        
        transform.position = new Vector3(-100, -100, -100);
        //currentTile.standingOn(null);
        setDead();
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
            spawnOplet(dimstingPrefab, target);
            takeDamage(5, false, true);
            
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
