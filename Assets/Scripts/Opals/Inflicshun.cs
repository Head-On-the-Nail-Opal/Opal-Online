using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inflicshun : OpalScript
{
    string currentForm = "000";

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 1;
        speed = 2;
        priority = 8;
        myName = "Inflicshun";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Unstable", 0, 0, 0, "<Passive>\nInflicshun will change form depending on the status conditions it is inflicted by.");
        Attacks[1] = new Attack("Slash", 1, 1, 6, "Deal close range damage");
        Attacks[2] = new Attack("Blast", 3, 4, 4, "Deal damage from further away");
        Attacks[3] = new Attack("Tune", 0, 1, 0, "Gain +1 attack");
        type1 = "Void";
        type2 = "Void";
        anim = GetComponent<Animator>();
        GetComponent<Animator>().CrossFade("Inflicshun000",0);
    }

    private void getForm()
    {
        if (getBurning() && getPoison() && getLifted())
        {
            Attacks[1] = new Attack("Omega Slash", 1, 1, 15, "Give target burn, poison, and lift. Heal by half the damage dealt.");
            Attacks[2] = new Attack("Omega Blast", 8, 4, 10, "Add damage based on current burn and poison damage. The target is pushed 2 tiles.");
            Attacks[3] = new Attack("Omega Tune", 0, 1, 0, "Heal 10 health. Gain +3 attack, +3 defense, and +2 speed.");
            anim.CrossFade("Inflicshun111", 0);
            currentForm = "111";
        }
        else if(getBurning() && getPoison())
        {
            Attacks[1] = new Attack("Painful Slash", 1, 1, 10, "Give target burn and poison.");
            Attacks[2] = new Attack("Painful Blast", 5, 4, 8, "Add damage based on current burn and poison damage.");
            Attacks[3] = new Attack("Painful Tune", 0, 1, 0, "Gain +3 attack, +3 defense.");
            anim.CrossFade("Inflicshun110", 0);
            currentForm = "110";
        }
        else if(getPoison() && getLifted())
        {
            Attacks[1] = new Attack("Nauseous Slash", 1, 1, 10, "Give target poison and lift.");
            Attacks[2] = new Attack("Nauseous Blast", 5, 4, 8, "Add damage based on current poison damage. The target is pushed 2 tiles.");
            Attacks[3] = new Attack("Nauseous Tune", 0, 1, 0, "Gain +3 defense and +2 speed.");
            anim.CrossFade("Inflicshun011", 0);
            currentForm = "011";
        }
        else if(getBurning() && getLifted())
        {
            Attacks[1] = new Attack("Flaming Slash", 1, 1, 10, "Give target burn and lift.");
            Attacks[2] = new Attack("Flaming Blast", 5, 4, 8, "Add damage based on current burn damage. The target is pushed 2 tiles.");
            Attacks[3] = new Attack("Flaming Tune", 0, 1, 0, "Gain +3 attack and +2 speed.");
            anim.CrossFade("Inflicshun101", 0);
            currentForm = "101";
        }
        else if (getBurning())
        {
            Attacks[1] = new Attack("Searing Slash", 1, 1, 8, "Burn target.");
            Attacks[2] = new Attack("Searing Blast", 4, 4, 6, "Add damage based on current burn damage.");
            Attacks[3] = new Attack("Searing Tune", 0, 1, 0, "Gain +2 attack.");
            anim.CrossFade("Inflicshun100", 0);
            currentForm = "100";
        }
        else if (getPoison())
        {
            Attacks[1] = new Attack("Sickly Slash", 1, 1, 8, "Poison target.");
            Attacks[2] = new Attack("Sickly Blast", 4, 4, 6, "Add damage based on current poison damage.");
            Attacks[3] = new Attack("Sickly Tune", 0, 1, 0, "Gain +2 defense.");
            anim.CrossFade("Inflicshun010", 0);
            currentForm = "010";
        }
        else if (getLifted())
        {
            Attacks[1] = new Attack("Floaty Slash", 1, 1, 8, "Lift target.");
            Attacks[2] = new Attack("Floaty Blast", 4, 4, 6, "The target is pushed 1 tile.");
            Attacks[3] = new Attack("Floaty Tune", 0, 1, 0, "Gain +1 speed.");
            anim.CrossFade("Inflicshun001", 0);
            currentForm = "001"; 
        }
        else
        {
            Attacks[1] = new Attack("Slash", 1, 1, 6, "Deal close range damage");
            Attacks[2] = new Attack("Blast", 3, 4, 4, "Deal damage from further away");
            Attacks[3] = new Attack("Tune", 0, 1, 0, "Gain +1 attack");
            anim.CrossFade("Inflicshun000", 0);
            currentForm = "000";
        }
    }

    public override void onStart()
    {
        getForm();
    }

    public override void onMove(int distanceMoved)
    {
        getForm();
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            
        }
        else if (attackNum == 1)
        {
            if (currentForm.Substring(0, 1) == "1")
            {
                target.setBurning(true);
            }
            if (currentForm.Substring(1, 1) == "1")
            {
                target.setPoison(true);
            }
            if (currentForm.Substring(2, 1) == "1")
            {
                target.setLifted(true);
            }
            if (currentForm == "111")
            {
                doHeal((cA.getBaseDamage() + getAttack()) / 2, false);
            }
        }
        else if (attackNum == 2)
        {
            if (currentForm.Substring(0, 1) == "1")
            {
                target.takeBurnDamage(false);
            }
            if (currentForm.Substring(1, 1) == "1")
            {
                target.takePoisonDamage(false);
            }
            if (currentForm.Substring(2, 1) == "1")
            {
                if (currentForm == "001")
                {
                    pushAway(1, target);
                }
                else
                {
                    pushAway(2, target);
                }
            }
            if (currentForm == "111")
            {

            }
        }
        else if (attackNum == 3)
        {
            if(currentForm == "111")
            {
                doHeal(10, false);
                doTempBuff(0, -1, 3);
                doTempBuff(1, -1, 3);
                doTempBuff(2, -1, 2);
            }
            else if(currentForm == "110")
            {
                doTempBuff(1, -1, 3);
                doTempBuff(0, -1, 3);
            }
            else if(currentForm == "101")
            {
                doTempBuff(0, -1, 3);
                doTempBuff(2, -1, 2);
            }
            else if(currentForm == "011")
            {
                doTempBuff(1, -1, 3);
                doTempBuff(2, -1, 2);
            }
            else if(currentForm == "001")
            {
                doTempBuff(2, -1, 1);
            }
            else if(currentForm == "010")
            {
                doTempBuff(1, -1, 1);
            }
            else if(currentForm == "100")
            {
                doTempBuff(0, -1, 1);
            }
            else
            {
                doTempBuff(0, -1, 1);
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

        }
        else if (attackNum == 2)
        {

        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        return base.checkCanAttack(target, attackNum);
    }
}
