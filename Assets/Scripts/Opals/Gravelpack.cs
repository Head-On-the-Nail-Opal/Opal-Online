using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravelpack : OpalScript
{
    private Gritwit dungletPrefab;
    private int boulderDamage = 0;

    public override void onAwake()
    {
        dungletPrefab = Resources.Load<Gritwit>("Prefabs/SubOpals/Gritwit");
    }

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 1;
        priority = 6;
        myName = "Gravelpack";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.3f;
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
        Attacks[0] = new Attack("Rocky Load", 2, 4, 0, "Summon two Gritwits",0,3);
        Attacks[0].setUses(2);
        Attacks[1] = new Attack("Winning Path", 4, 1, 0, "Give an Opal +2 attack.",0,3);
        Attacks[2] = new Attack("Crunchy Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles (0)",0,3);
        Attacks[3] = new Attack("Rock Carapace", 0, 1, 0, "Surrounding Boulders gain +3 defense.",0,3);
        type1 = "Ground";
        type2 = "Swarm";
    }

    public override void onStart()
    {
        boulderDamage = 0;
        List<TileScript> sur = getSurroundingTiles(false);
        foreach (TileScript t in sur)
        {
            if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                boulderDamage++;
            }
        }
        Attacks[2] = new Attack("Crunchy Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles ("+boulderDamage+")");
    }

    public override void onMove(int distanceMoved)
    {
        boulderDamage = 0;
        List<TileScript> sur = getSurroundingTiles(false);
        foreach (TileScript t in sur)
        {
            if (t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
            {
                boulderDamage++;
            }
        }
        Attacks[2] = new Attack("Crunchy Bite", 3, 4, 6, "Deal 6 damage for each Boulder on surrounding tiles (" + boulderDamage + ")");
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
            target.doTempBuff(0, -1, 2);
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {

            return cA.getBaseDamage() * boulderDamage + getAttack();
        }else if(attackNum == 3)
        {
            List<TileScript> sur = getSurroundingTiles(false);
            foreach(TileScript t in sur)
            {
                if(t.currentPlayer != null && t.currentPlayer.getMyName() == "Boulder")
                {
                    t.currentPlayer.doTempBuff(1, -1, 3);
                }
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            spawnOplet(dungletPrefab, target);
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
        if(attackNum == 3 && target.currentPlayer != null)
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
