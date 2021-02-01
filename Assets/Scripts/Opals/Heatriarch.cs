using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heatriarch : OpalScript
{
    private Heant heantPrefab;

    public override void onAwake()
    {
        heantPrefab = Resources.Load<Heant>("Prefabs/SubOpals/Heant");
    }

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 1;
        priority = 1;
        myName = "Heatriarch";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 1.5f;
        offsetX = 0;
        offsetY = 0.2f;
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
        Attacks[0] = new Attack("Hive Mother", 0, 1, 0, "Hatch a Heant on each adjacent Flame tile.", 1);
        Attacks[1] = new Attack("Flame Nest", 0, 1, 0, "Light adjacent tiles on fire. Adjacent Opals gain +3 speed for their next turn.", 1);
        Attacks[2] = new Attack("Agitating Bite", 2, 4, 0, "Target takes damage from their burn.");
        Attacks[3] = new Attack("Flaming Spit", 4, 4, 0, "Burn all opals in radius.", 1);
        type1 = "Fire";
        type2 = "Swarm";
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
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
                target.doTempBuff(2, 1, 3);

            }
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            if (target.getBurning())
            {
                target.takeDamage(target.getBurningDamage(), false, true);
                boardScript.callParticles("burning", target.getPos());
                target.setBurningDamage(target.getBurningDamage() + 2);
            }
            return 0;
        }
        else if (attackNum == 3) //Spectral Lunge
        {
            target.setBurning(true);
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
            foreach (OpalScript o in boardScript.gameOpals)
            {
                if (o.getMyName() == "Heant" && o.getDead() == false)
                    minionCount++;
            }
            if (minionCount < 4)
            {
                if (target.type == "Fire")
                {
                    spawnOplet(heantPrefab, target);
                }
            }
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override List<OpalScript> getOplets()
    {
        List<OpalScript> temp = new List<OpalScript>();
        temp.Add(heantPrefab);
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
            if(target.currentPlayer.getBurning() == false)
            {
                return 0;
            }
            return target.currentPlayer.getBurningDamage();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 1 || attackNum == 0)
        {
            return 0;
        }
        else if(target.getCurrentOpal() != null)
        {
            return 0;
        }
        return -1;
    }
}
