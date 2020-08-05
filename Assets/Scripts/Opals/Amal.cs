using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amal : OpalScript {
    private Gum myGum;
    private ParticleSystem damage;
    public bool stopThat = false;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 1;
        priority = 3;
        myName = "Amal";
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
        Attacks[0] = new Attack("Linked", 0, 0, 0, "<Passive>\n Whenever this opal takes damage, its twin will take it instead. If one twin dies so does the other.");
        Attacks[1] = new Attack("Scythe", 2, 4, 4, "Deal 4 damage, add your defense instead of your attack.");
        Attacks[2] = new Attack("Twin Intrepidity", 0, 1, 0, "Buff Gum by +4 attack");
        Attacks[3] = new Attack("Trade Link", 0, 1, 0, "<Free Ability>\n Gum loses 5 health. Amal gains 5 health.");
        Attacks[3].setFreeAction(true);
        type1 = "Void";
        type2 = "Void";
        damage = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/LinkDamage");
        og = true;
    }

    public void setTwin(Gum guy)
    {
        myGum = guy;
    }

    public void takeTwinDamage(int dam, bool mod, bool effect)
    {
        if (!mod)
        {
            this.health -= dam;
            ParticleSystem temp = Instantiate<ParticleSystem>(damage);
            temp.transform.position = transform.position;
        }
        else if (dam - getDefense() > 0)
        {
            this.health = this.health - (dam - getDefense());
            ParticleSystem temp = Instantiate<ParticleSystem>(damage);
            temp.transform.position = transform.position;
        }
        if (this.health <= 0)
        {
            if (!myGum.stopThat)
            {
                stopThat = true;
                myGum.takeTwinDamage(myGum.getHealth(), false, true);
            }
            StartCoroutine(base.shrinker());
        }
    }

    public override void takeDamage(int dam, bool mod, bool effect)
    {
        myGum.takeTwinDamage(dam, mod, effect);
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
            return cA.getBaseDamage() + getDefense();
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            myGum.doTempBuff(0, -1, 4);
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
            return Attacks[attackNum].getBaseDamage() + getDefense() - target.currentPlayer.getDefense();
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
