using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravelpack : OpalScript
{
    private Gritwit dungletPrefab;
    private int gravelLevel = 0;

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
        Attacks[1] = new Attack("Winning Path", 3, 1, 0, "Break a Boulder, and add a level of Gravel to your pack. (0)",0,3);
        Attacks[2] = new Attack("Crunchy Bite", 2, 4, 4, "Deal 4 damage for each level of Gravel in your pack. (+0 damage)",0,3);
        Attacks[3] = new Attack("Rock Carapace", 3, 1, 0, "Place a Boulder with defense equal to your total number of Gravel. Reduce your level of Gravel by -1.",0,3);
        type1 = "Ground";
        type2 = "Swarm";
    }


    private void adjustGravelText()
    {
        Attacks[1].setDescription("Break a Boulder, and add a level of Gravel to your pack. ("+gravelLevel+")");
        Attacks[2].setDescription("Deal 4 damage for each level of Gravel in your pack. (+"+(gravelLevel*4)+" damage)");
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
            if(target.getMyName() == "Boulder")
            {
                target.takeDamage(target.getHealth(), false, true);
                gravelLevel++;
                adjustGravelText();
            }
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {

            return cA.getBaseDamage() * gravelLevel + getAttack();
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
            TileScript newBoulder = boardScript.setTile(target, "Boulder", false);
            newBoulder.getCurrentOpal().doTempBuff(1, -1, gravelLevel);
            gravelLevel--;
            adjustGravelText();
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
            if(target.currentPlayer.getMyName() == "Boulder")
            {
                return target.currentPlayer.getHealth();
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            return Attacks[attackNum].getBaseDamage() * gravelLevel + getAttack() - target.currentPlayer.getDefense();
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if(attackNum == 0)
        {
            if (target.currentPlayer == null)
                return 0;
        }else if(attackNum == 1)
        {
            if (target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder")
                return 0;
        }else if(attackNum == 2)
        {
            if (target.currentPlayer != null)
                return 0;
        }
        else if(attackNum == 3)
        {
            if (target.currentPlayer == null || gravelLevel < 1)
                return 0;
        }
        return -1;
    }
}
