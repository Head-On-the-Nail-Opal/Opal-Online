using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aughtment : OpalScript
{
    private int harmLevel = 0;
    private int warmLevel = 0;
    private ParticleSystem superExplosion;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 2;
        priority = 7;
        myName = "Aughtment";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
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
        Attacks[0] = new Attack("Tinker", 0, 0, 0, "<Passive>\n When Aughtment uses one ability, the other is improved. <Warning> Do not upgrade past level 9...");
        Attacks[1] = new Attack("Harm Arm", 1, 1, 1, "Deal 1 damage.");
        Attacks[2] = new Attack("Warm Arm", 0, 1, 0, "Gain +1 defense");
        Attacks[3] = new Attack("Two Arms!", 0, 1, 0, "Upgrade both of your arm abilities.");
        type1 = "Void";
        type2 = "Metal";
        if(pl != null)
        {
            superExplosion = Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/SuperExplosion");
        }
    }

    public override void onEnd()
    {
        if(harmLevel > 9 || warmLevel > 9)
        {
            StartCoroutine(bigExplode());
        }
        switch (harmLevel)
        {
            case 0:
                Attacks[1] = new Attack("Harm Arm [0]", 1, 1, 1, "Deal 1 damage.");
                break;
            case 1:
                Attacks[1] = new Attack("Harm Arm [1]", 1, 1, 2, "Deal 2 damage.");
                break;
            case 2:
                Attacks[1] = new Attack("Harm Arm [2]", 2, 4, 3, "Deal 3 damage.");
                break;
            case 3:
                Attacks[1] = new Attack("Harm Arm [3]", 2, 1, 4, "Deal 4 damage. Ignores line of sight.");
                break;
            case 4:
                Attacks[1] = new Attack("Harm Arm [4]", 3, 4, 6, "Deal 6 damage.");
                break;
            case 5:
                Attacks[1] = new Attack("Harm Arm [5]", 3, 1, 8, "Deal 8 damage. Ignores line of sight.");
                break;
            case 6:
                Attacks[1] = new Attack("Harm Arm [6]", 4, 4, 10, "Deal 10 damage. Hits Opals adjacent to target.", 1);
                break;
            case 7:
                Attacks[1] = new Attack("Harm Arm [7]", 4, 1, 12, "Deal 12 damage. Ignores line of sight. Hits Opals adjacent to target.", 1);
                break;
            case 8:
                Attacks[1] = new Attack("Harm Arm [8]", 5, 1, 14, "Deal 14 damage. Ignores line of sight. Hits Opals adjacent to target.", 1);
                break;
            case 9:
                Attacks[1] = new Attack("Harm Arm [9]", 1, 6, 4444444, "Deal 4444444 damage to all Opals in a line. Die.");
                break;
        }
        switch (warmLevel)
        {
            case 0:
                Attacks[2] = new Attack("Warm Arm [0]", 0, 1, 0, "Gain +1 defense");
                break;
            case 1:
                Attacks[2] = new Attack("Warm Arm [1]", 0, 1, 0, "Gain +1 attack and +1 defense");
                break;
            case 2:
                Attacks[2] = new Attack("Warm Arm [2]", 1, 1, 0, "Give target +1 attack and +1 defense");
                break;
            case 3:
                Attacks[2] = new Attack("Warm Arm [3]", 2, 1, 0, "Give target +1 attack and +1 defense. Cure their status conditions.");
                break;
            case 4:
                Attacks[2] = new Attack("Warm Arm [4]", 2, 1, 0, "Give target +2 attack and +2 defense. Cure their status conditions.");
                break;
            case 5:
                Attacks[2] = new Attack("Warm Arm [5]", 3, 1, 0, "Give target +2 attack and +2 defense. Cure their status conditions.");
                break;
            case 6:
                Attacks[2] = new Attack("Warm Arm [6]", 3, 1, 0, "Give target +2 attack and +2 defense. Cure their status conditions. Heal them by 6.");
                break;
            case 7:
                Attacks[2] = new Attack("Warm Arm [7]", 4, 1, 0, "Give target +3 attack and +3 defense. Cure their status conditions. Heal them by 6.");
                break;
            case 8:
                Attacks[2] = new Attack("Warm Arm [8]", 4, 1, 0, "Give target +3 attack and +3 defense. Cure their status conditions. Overheal them by 8.");
                break;
            case 9:
                Attacks[2] = new Attack("Warm Arm [9]", 1, 1, 0, "Give target +44 attack and +44 defense. Overheal them by 44. Die.");
                break;
        }

    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            warmLevel++;
            
        }
        else if (attackNum == 2)
        {
            harmLevel++;
            summonParticle("Upgrade");
            switch (warmLevel)
            {
                case 0:
                    doTempBuff(1, -1, 1);
                    break;
                case 1:
                    doTempBuff(0, -1, 1);
                    doTempBuff(1, -1, 1);
                    break;
                case 2:
                    target.doTempBuff(0, -1, 1);
                    target.doTempBuff(1, -1, 1);
                    break;
                case 3:
                    target.doTempBuff(0, -1, 1);
                    target.doTempBuff(1, -1, 1);
                    target.healStatusEffects();
                    break;
                case 4:
                    target.doTempBuff(0, -1, 2);
                    target.doTempBuff(1, -1, 2);
                    target.healStatusEffects();
                    break;
                case 5:

                    target.doTempBuff(0, -1, 2);
                    target.doTempBuff(1, -1, 2);
                    target.healStatusEffects();
                    break;
                case 6:
                    target.doTempBuff(0, -1, 2);
                    target.doTempBuff(1, -1, 2);
                    target.healStatusEffects();
                    target.doHeal(6, false);
                    break;
                case 7:
                    target.doTempBuff(0, -1, 3);
                    target.doTempBuff(1, -1, 3);
                    target.healStatusEffects();
                    target.doHeal(6, false);
                    break;
                case 8:
                    target.doTempBuff(0, -1, 3);
                    target.doTempBuff(1, -1, 3);
                    target.healStatusEffects();
                    target.doHeal(8, false);
                    break;
                case 9:
                    target.doTempBuff(0, -1, 44);
                    target.doTempBuff(1, -1, 44);
                    target.healStatusEffects();
                    target.doHeal(44, false);
                    //takeDamage(getHealth(), false, true);
                    break;
            }
            return 0;
        }
        else if (attackNum == 1)
        {
            warmLevel++;
            summonParticle("Upgrade");
            if (harmLevel == 9)
            {
                //takeDamage(getHealth(), false, true);
            }
        }else if(attackNum == 3)
        {
            warmLevel++;
            harmLevel++;
            summonParticle("Upgrade");
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
        }
        else if (attackNum == 2)
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
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public IEnumerator bigExplode()
    {
        ParticleSystem tempParticle = Instantiate<ParticleSystem>(superExplosion);
        tempParticle.transform.position = getPos();
        takeDamage(getHealth(), false, true);
        yield return new WaitForSeconds(0.1f);
        for(int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                ParticleSystem tempParticle1 = Instantiate<ParticleSystem>(superExplosion);
                tempParticle1.transform.position = new Vector3(getPos().x + i, getPos().y, getPos().z+j);
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null)
                {
                   if(!(i == 0 && j == 0))
                    {
                        boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.takeDamage(444, false, true);
                    }
                }
            }
        }
    }
}
