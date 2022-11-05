using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigator : OpalScript
{
    private int bouldersLeft = 2;
    private bool usedAbility = false;
    Flarasaur flarasaurPrefab;
    Brachiosh brachioshPrefab;
    Floweraptor floweraptorPrefab;
    private bool discovering = false;
    private List<OpalScript> summoned = new List<OpalScript>();


    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 3;
        speed = 2;
        priority = 2;
        myName = "Investigator";
        transform.localScale = new Vector3(3f, 3f, 1)*1.2f;
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
        Attacks[0] = new Attack("Archeologist", 0, 0, 0, "<Passive>\nAfter Investigator breaks 2 boulders, they may discover a fossil Opal to spawn on an adjacent tile.");
        Attacks[1] = new Attack("Pick Arm", 1, 1, 10, "If the target is a Boulder this hits Opals adjacent to the target. It does not hit Investigator.",0,3);
        Attacks[2] = new Attack("Discovery", 1, 1, 0, "Place a Boulder",0,3);
        Attacks[3] = new Attack("Excavate", 1, 1, 0, "<Free Ability>\nBreak a boulder adjacent to Investigator, once per turn.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Swarm";
        type2 = "Ground";
        flarasaurPrefab = Resources.Load<Flarasaur>("Prefabs/SubOpals/Flarasaur");
        brachioshPrefab = Resources.Load<Brachiosh>("Prefabs/SubOpals/Brachiosh");
        floweraptorPrefab = Resources.Load<Floweraptor>("Prefabs/SubOpals/Floweraptor");
    }

    public override void onPlacement()
    {
        Attacks[0] = new Attack("Archeologist", 0, 0, 0, "<Passive>\nAfter Investigator breaks 2 boulders, they may discover a fossil Opal to spawn on an adjacent tile. (" + (2 - bouldersLeft) + "/" + 2 + ")",0,3);
    }

    public override void onStart()
    {
        usedAbility = false;
    }

    private void switchAbilties()
    {
        if (discovering)
        {
            ParticleSystem temp = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/Discovery");
            ParticleSystem inst = Instantiate<ParticleSystem>(temp, this.transform);
            //inst.transform.localScale = transform.localScale;
            inst.transform.localRotation = Quaternion.Euler(0, 0, 0);
            Attacks[0] = new Attack("Archeologist", 0, 0, 0, "<Passive>\nChoose a Fossil to summon. You may only choose each one once.");
            Attacks[1] = new Attack("Overgrown Fossil", 1, 1, 0, "<Free Ability>\nSummon a Floweraptor",0,3);
            Attacks[2] = new Attack("Oily Fossil", 1, 1, 0, "<Free Ability>\nSummon a Flarasaur",0,3);
            Attacks[3] = new Attack("Salt Fossil", 1, 1, 0, "<Free Ability>\nSummon a Brachiosh",0,3);
            Attacks[1].setFreeAction(true);
            Attacks[2].setFreeAction(true);
            Attacks[3].setFreeAction(true);
            bouldersLeft = 2;
        }
        else
        {
            Attacks[0] = new Attack("Archeologist", 0, 0, 0, "<Passive>\nAfter Investigator breaks 2 boulders, they may discover a fossil Opal to spawn on an adjacent tile. (" + (2 - bouldersLeft) + "/" + 2 + ")",0,3);
            Attacks[1] = new Attack("Pick Arm", 1, 1, 10, "If the target is a Boulder this hits Opals adjacent to the target",0,3);
            Attacks[2] = new Attack("Discovery", 1, 1, 0, "Place a Boulder",0,3);
            Attacks[3] = new Attack("Excavate", 1, 1, 0, "<Free Ability>\nBreak a boulder adjacent to Investigator, once per turn.",0,3);
            Attacks[3].setFreeAction(true);
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
            if (!discovering)
            {
                if(target.getMyName() == "Boulder")
                {
                    foreach(TileScript t in target.getSurroundingTiles(true))
                    {
                        if(t.currentPlayer != null && t.currentPlayer != this)
                        {
                            t.currentPlayer.takeDamage(10 + getAttack(), true, true);
                        }
                    }
                }
            }
            else
            {
                return 0;
            }
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            if (!discovering)
            {
                if(!usedAbility && target.getMyName() == "Boulder")
                {
                    target.takeDamageBelowArmor(target.getHealth(), false, true);
                    usedAbility = true;
                    bouldersLeft--;
                    Attacks[0] = new Attack("Archeologist", 0, 0, 0, "<Passive>\nAfter Investigator breaks 2 boulders, they may discover a fossil Opal to spawn on an adjacent tile. (" + (2 - bouldersLeft) + "/" + 2 + ")",0,3);
                    if (bouldersLeft < 1)
                    {
                        discovering = true;
                        switchAbilties();
                    }
                }
            }
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
            if (discovering && !summoned.Contains(floweraptorPrefab))
            {
                summoned.Add(floweraptorPrefab);
                spawnOplet(floweraptorPrefab,target,8);
                discovering = false;
                switchAbilties();
            }
            return 0;
        }
        else if (attackNum == 2)
        {
            if (!discovering)
            {
                boardScript.setTile(target, "Boulder", false);
                //bouldersLeft--;
                if (bouldersLeft < 1)
                {
                    discovering = true;
                    switchAbilties();
                }
                return 0;
            }
            else if(!summoned.Contains(flarasaurPrefab))
            {
                summoned.Add(flarasaurPrefab);
                spawnOplet(flarasaurPrefab, target, 4);
                discovering = false;
                switchAbilties();
            }
        }
        else if (attackNum == 3)
        {
            if (discovering && !summoned.Contains(brachioshPrefab))
            {
                summoned.Add(brachioshPrefab);
                spawnOplet(brachioshPrefab, target,12);
                discovering = false;
                switchAbilties();
            }
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
            if (discovering)
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
        if (discovering)
        {
            if(target.currentPlayer == null)
            {
                return 0;
            }
        }
        else
        {
            if(attackNum == 2 && target.currentPlayer == null)
            {
                return 0;
            }
            if(attackNum == 3 && target.currentPlayer != null && target.currentPlayer.getMyName() == "Boulder" && usedAbility == false)
            {
                return 0;
            }
        }
        return base.checkCanAttack(target, attackNum);
    }
}
