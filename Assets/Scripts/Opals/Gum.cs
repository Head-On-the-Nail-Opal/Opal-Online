using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gum : OpalScript {
    private Amal myAmal;
    private ParticleSystem damage;
    public bool stopThat = false;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 2;
        defense = 0;
        speed = 2;
        priority = 7;
        myName = "Gum";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = -0.1f;
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
        Attacks[0] = new Attack("Linked", 0, 0, 0, "<Passive>\n Whenever this opal takes damage, its twin will take it instead. If one twin dies so does the other.");
        Attacks[1] = new Attack("Rift Gaze", 2, 4, 4, "Target loses 4 speed next turn.",0,3);
        Attacks[2] = new Attack("Twin Resolute", 0, 1, 0, "Buff Amal by +4 defense",0,3);
        Attacks[3] = new Attack("Trade Link", 0, 1, 0, "<Free Ability>\n Amal loses 5 health. Gum gains 5 health.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Void";
        type2 = "Void";
        damage = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/LinkDamage");
        og = true;
    }


    public void takeTwinDamage(int dam, bool mod, bool effect)
    {
        if (!mod)
        {
            this.health -= dam;
            if (effect)
            {
                ParticleSystem temp = Instantiate<ParticleSystem>(damage);
                temp.transform.position = transform.position;
            }
        }
        else if (dam - getDefense() > 0)
        {
            this.health = this.health - (dam - getDefense());
            if (effect)
            {
                ParticleSystem temp = Instantiate<ParticleSystem>(damage);
                temp.transform.position = transform.position;
            }
        }
        if (this.health <= 0)
        {
            if (!myAmal.stopThat)
            {
                stopThat = true;
                myAmal.takeTwinDamage(myAmal.getHealth(), false, true);
            }
            StartCoroutine(base.shrinker());
        }
    }

    public override void takeDamage(int dam, bool mod, bool effect)
    {
        myAmal.takeTwinDamage(dam, mod, effect);
    }

    public void setTwin(Amal guy)
    {
        myAmal = guy;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Rift Gaze
        {
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            target.doTempBuff(2, 1, -4);
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            myAmal.doTempBuff(1, -1, 4);
            return 0;
        }
        else if (attackNum == 3) //Spectral Lunge
        {
            takeDamage(5, false, true);
            doHeal(5, false);
            return 0;
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
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
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
}
