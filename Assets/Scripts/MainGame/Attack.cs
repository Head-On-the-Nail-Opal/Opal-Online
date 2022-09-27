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
    int effectiveRange = 0;
    List<string> tags = new List<string>();
    int attackAnim = 0;

    private List<AbilityProj> proj =  new List<AbilityProj>();
    private ParticleSystem chargeUp;
    private ParticleSystem hitEffect;

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

    public Attack(string n, int r, int s, int bd, string d, int a, int aa)
    {
        aname = n;
        range = r;
        shape = s;
        baseDamage = bd;
        description = d;
        aoe = a;
        attackAnim = aa;
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

    public int getAttackAnim()
    {
        return attackAnim;
    }

    public List<string> getTags()
    {
        return tags;
    }

    public void setRange(int r)
    {
        range = r;
    }

    public void setEffectiveRange(int r)
    {
        effectiveRange = r;
    }

    public int getEffectiveRange()
    {
        return effectiveRange;
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

    public void setOnHit(string particlesystemName, string particleName, Color color, bool beforeDamage, int length)
    {

    }

    public void addProjectile(string particlesystemName, string particleName, float particleMultiplier, Color color, float speed)
    {
        proj.Add(new AbilityProj(particlesystemName, particleName, particleMultiplier, color, speed));
    }

    public void addProjectile(string particlesystemName, string particleName, float particleMultiplier, Color color, Color color2, float speed)
    {
        proj.Add(new AbilityProj(particlesystemName, particleName, particleMultiplier, color, speed));
    }

    //'length' here is independent from the lifetime of the system, just the delay until 
    public void setChargeUp(string particlesystemName, string particleName, Color color, int length)
    {

    }

    public List<AbilityProj> getProj()
    {
        return proj;
    }

    public ParticleSystem getOnHit()
    {
        return hitEffect;
    }

    public ParticleSystem getChargeUp()
    {
        return chargeUp;
    }
}

public class AbilityProj {
    public string shapeName;
    public string particleName;
    public float particleMultiplier;
    public Color clr;
    public Color clr2;
    public float speed;


    public AbilityProj(string s, string p, float pM, Color c, float m){
        shapeName = s;
        particleName = p;
        particleMultiplier = pM;
        clr = c;
        speed = m;
    }

    public AbilityProj(string s, string p, float pM, Color c, Color c2, float m)
    {
        shapeName = s;
        particleName = p;
        particleMultiplier = pM;
        clr = c;
        clr2 = c2;
        speed = m;
    }
}

