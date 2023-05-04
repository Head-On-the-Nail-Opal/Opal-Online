using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amalgum : OpalScript
{
    private Amal amalPrefab;
    private Gum gumPrefab;
    private int currentUse = 0;
    private Amal tempAmal;

    public override void onAwake()
    {
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
                tempAmal = (Amal)spawnOplet(amalPrefab, target, 2);
                currentUse = 1;
            }
            else
            {
                Gum tempGum = (Gum)spawnOplet(gumPrefab, target, 6);
                tempAmal.setTwin(tempGum);
                tempGum.setTwin(tempAmal);
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
