using UnityEngine;

public class Glorm : OpalScript
{
    int increment = 1;
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 6;
        myName = "Glorm";
        transform.localScale = new Vector3(3f, 3f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Glowweb", 3, 4, 0, "Poison a target and give it +4 attack and +4 defense for a turn. Overheal self by 4.",0,3);
        Attacks[1] = new Attack("Gooey Spit", 4, 4, 0, "Deal damage based on amount of current overheal. Remove all overheal.",0,3);
        Attacks[2] = new Attack("Gluey Spit", 3, 4, 0, "Overheal a target by your current overheal. Lose health for each point the target overheals.",0,3);
        Attacks[3] = new Attack("Self Care", 0, 1, 0, "Overheal yourself by 4. Gain +1 defense.",0,3);
        type1 = "Light";
        type2 = "Plague";
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            target.setPoison(true);
            target.doTempBuff(0, 1, 4);
            target.doTempBuff(1, 1, 4);
            doHeal(4, true);
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            if (this.getHealth() > this.getMaxHealth())
            {
                int temp = getHealth() - getMaxHealth();
                health = maxHealth;
                return temp + getAttack();
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (this.getHealth() > this.getMaxHealth())
            {
                int currentTargetOverheal = target.getHealth() - target.getMaxHealth();
                target.doHeal(getHealth() - getMaxHealth(), true);
                if(target.getHealth() > target.getMaxHealth())
                {
                    if(currentTargetOverheal > 0)
                        takeDamage(target.getHealth() - target.getMaxHealth() - currentTargetOverheal, false, true);
                    takeDamage(target.getHealth()-target.getMaxHealth(), false, true);
                }
            }
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            doHeal(4, true);
            doTempBuff(1, -1, 1);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
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
            if(this.getHealth() > this.getMaxHealth())
            {
                return this.getHealth() - getMaxHealth() - target.currentPlayer.getDefense();
            }
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


}
