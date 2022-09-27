using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gorj : OpalScript
{
    List<OpalScript> victims = new List<OpalScript>();
    OpalScript victim = null;
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 2;
        priority = 0;
        myName = "Gorj";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Soft Belly", 0, 0, 0, "<Passive>\nWhen Gorj takes damage while it is engorged it will spit it's victim out.");
        Attacks[1] = new Attack("Consume", 1, 1, 0, "Consume an Opal. Gorj's speed is set to 2",0,3);
        Attacks[2] = new Attack("Belly Laugh", 0, 1, 0, "Gain +3 defense. If engorged, deal 5 damage to the victim",0,3);
        Attacks[3] = new Attack("Mulch Munch", 0, 1, 0, "Eat the ground beneath Gorj, each tile type affecting Gorj and it's victim differently.",0,3);
        type1 = "Void";
        type2 = "Void";
    }


    public override void onDamage(int dam)
    {
        if(victims.Count != 0)
        {
            List<TileScript> temps = new List<TileScript>();
            foreach (OpalScript o in victims)
            {
                //print(o.name);
                if(o.getHealth() >= 0) {
                    foreach (TileScript t in getSurroundingTiles(false))
                    {
                       // print(t.type);
                        if (!t.getImpassable() && t.currentPlayer == null && !temps.Contains(t))
                        {
                            temps.Add(t);
                            o.setNotDead();
                            o.setPos((int)t.getPos().x, (int)t.getPos().z);
                            o.doMove((int)t.getPos().x, (int)t.getPos().z, 0);
                            break;
                        }
                    }
                    transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
                    anim.CrossFade("Gorj", 0);
                    doHighlight("Gorj");
                }
            }
            victims.Clear();
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            return 0;
        }
        else if (attackNum == 1)
        {
            victims.Add(target);
            target.setDead();
            //if(target.getCurrentTile() != null)
            target.getCurrentTile().standingOn(null);
            target.setPos(-100,-100);
            setTempBuff(2, -1, 2);
            transform.localScale *= 1.1f;
            anim.CrossFade("EnGorj", 0);
            doHighlight("EnGorj");
            return 0;
        }
        else if (attackNum == 2)
        {
            List<OpalScript> deadOpals = new List<OpalScript>();
            doTempBuff(1, -1, 3);
            if(victims.Count != 0)
            {
                foreach (OpalScript o in victims)
                {
                    o.takeDamage(5, false, false);
                    if(o.getHealth() <= 0)
                    {
                        transform.localScale /= 1.3f;
                        deadOpals.Add(o);
                    }
                }
            }
            foreach(OpalScript o in deadOpals)
            {
                victims.Remove(o);
            }
            deadOpals.Clear();
            return 0;
        }
        else if (attackNum == 3)
        {
            if (currentTile.type == "Fire")
            {
                //takeBurnDamage(false);
                doTempBuff(1, -1, -2);
                foreach (OpalScript o in victims)
                {
                    o.takeDamage(10, false, false);
                }
            }
            else if (currentTile.type == "Growth")
            {
                foreach (OpalScript o in victims)
                {
                    o.doTempBuff(0, -1, 6);
                    o.doTempBuff(1, -1, 6);
                }
                doTempBuff(0, -1, 2);
                doTempBuff(1, -1, 2);
            }
            else if (currentTile.type == "Miasma")
            {
                foreach (OpalScript o in victims)
                {
                    o.doTempBuff(0, -1, -4);
                    o.doTempBuff(1, -1, -4);
                }
                doTempBuff(0, -1, -1);
                doTempBuff(1, -1, -1);
            }
            else if (currentTile.type == "Flood")
            {
                foreach (OpalScript o in victims)
                {
                    o.doHeal(5, false);
                }
                doHeal(5, false);
            }
            boardScript.setTile(currentTile, "Grass", true);
            healStatusEffects();
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
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
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
