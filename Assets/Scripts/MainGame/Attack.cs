using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack{

    public string aname;
    int range;
    int shape = 1;
    int baseDamage;
    string description;
    int aoe = 0;
    int uses = 1;
    int currentUse = 0;
    bool freeAction = false;
    string tidalDescription = "";

    /**
     * Official description of attack keywords
     * Passive - Effect is active constantly throughout the game
     * Water Rush - Range is increased in water, to the entirety of the water 
     * Aftershock - Opal may attack again but may not repeat any previously used moves
     * Incremental - Specific numbers in the attack description increase by 1 each time the attack is used
     * */
    public Attack(string n, int r, int s, int bd, string d)
    {
        aname = n;
        range = r;
        shape = s;
        baseDamage = bd;
        description = d;
    }

    public Attack(string n, int r, int s, int bd, string d, int a)
    {
        aname = n;
        range = r;
        shape = s;
        baseDamage = bd;
        description = d;
        aoe = a;
    }

    public int getRange()
    {
        return range;
    }
    public int getShape()
    {
        return shape;
    }

    public void setShape(int i)
    {
        shape = i;
    }

    public void setRange(int r)
    {
        range = r;
    }

    public int getBaseDamage()
    {
        return baseDamage;
    }

    public string getDesc()
    {
        return description;
    }

    public int getAOE()
    {
        return aoe;
    }

    public void setAOE(int AOE)
    {
        aoe = AOE;
    }

    public void setDescription(string thing)
    {
        description = thing;
    }

    public void setUses(int u)
    {
        uses = u;
    }

    public int getUses()
    {
        return uses;
    }

    public bool getFreeAction()
    {
        return freeAction;
    }

    public void setFreeAction(bool f)
    {
        freeAction = f;
    }

    public int getCurrentUse(int mod)
    {
        currentUse += mod;
        return currentUse;
    }

    public void setTidalD(string d)
    {
        tidalDescription = d;
    }

    public string getTidalD()
    {
        return tidalDescription;
    }
}
