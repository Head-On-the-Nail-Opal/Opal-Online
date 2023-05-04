using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflectron : OpalScript
{
    bool diagonal = false;
    int damageTaken = 0;

    public Sprite tempDamage;


    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 5;
        priority = 2;
        myName = "Reflectron";
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
        Attacks = new Attack[6];
        Attacks[0] = new Attack("Reflect", 0, 0, 0, "<Passive>\n Reflectron will duplicate Fragmatom's laser attacks. Bounced lasers deal double damage.");
        Attacks[1] = new Attack("Form Switch", 0, 1, 0, "Change between Diagonal Form or Cross Form");
        Attacks[2] = new Attack("Empower", 0, 1, 0, "Overheal by 10",0,3);
        Attacks[3] = new Attack("Heal Ray", 1, 6, 0, "Heal all Opals in a line 2 health",0,3);
        Attacks[4] = new Attack("Straight Reflection", 1, 6, 4,"");
        Attacks[5] = new Attack("Diagonal Reflection", 1, 8, 4, "");
        type1 = "Light";
        type2 = "Laser";
        setUpAngle();
    }

    public void setUpAngle()
    {
        diagonal = !diagonal;
        if (!diagonal)
        {
            Attacks[3] = new Attack("Heal Ray", 1, 8, 0, "Heal all Opals in a diagonal line 2 health");
            //anim.CrossFade("ReflectronX", 0);
            //doHighlight("ReflectronX");
            animCutoff = 4;
            changeVisual(getMyVisual(), true);
        }
        else
        {
            Attacks[3] = new Attack("Heal Ray", 1, 6, 0, "Heal all Opals in a line 2 health");
            //anim.CrossFade("ReflectronT", 0);
            //doHighlight("ReflectronT");
            animCutoff = 8;
            changeVisual(getMyVisual(), true);
        }
    }

    public override void toggleMethod()
    {
        diagonal = !diagonal;
        Sprite t = tempDamage;
        tempDamage = attackFrame;
        attackFrame = t;
        hurtFrame = t;
        if (!diagonal)
        {
            Attacks[3] = new Attack("Heal Ray", 1, 8, 0, "Heal all Opals in a diagonal line 2 health");
            // anim.CrossFade("ReflectronX", 0);
            //doHighlight("ReflectronX");
            animCutoff = 8;
            changeVisual(getMyVisual(), true);
        }
        else
        {
            Attacks[3] = new Attack("Heal Ray", 1, 6, 0, "Heal all Opals in a line 2 health");
            //anim.CrossFade("ReflectronT", 0);
            //doHighlight("ReflectronT");
            animCutoff = 4;
            changeVisual(getMyVisual(), true);
        }
    }

    public override void onDamage(int dam)
    {
        damageTaken = dam - getDefense();
        CursorScript c = boardScript.myCursor;
        if (c.getCurrentOpal().getMyName() == "Glintrey")
        {
            if (!diagonal)
            {
                foreach(TileScript t in getSurroundingTiles(true))
                {
                    int i = 0;
                    int j = 0;
                    while (i + t.getPos().x != 10 && j + t.getPos().z != 10 && i + t.getPos().x != -1 && j + t.getPos().z != -1)
                    {
                        c.doAttack((int)t.getPos().x + i, (int)t.getPos().z + j, 4, this);
                        if(t.getPos().x != getPos().x)
                        {
                            if(t.getPos().x > getPos().x)
                            {
                                i++;
                            }
                            else
                            {
                                i--;
                            }
                        }
                        else
                        {
                            if (t.getPos().z > getPos().z)
                            {
                                j++;
                            }
                            else
                            {
                                j--;
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TileScript t in getDiagonalTiles())
                {
                        int i = 0;
                        int j = 0;
                        while (i + t.getPos().x != 10 && j + t.getPos().z != 10 && i + t.getPos().x != -1 && j + t.getPos().z != -1)
                        {
                            c.doAttack((int)t.getPos().x + i, (int)t.getPos().z + j, 5, this);
                            if (t.getPos().x > getPos().x)
                            {
                                i++;
                            }
                            else
                            {
                                i--;
                            }
                            if (t.getPos().z > getPos().z)
                            {
                                j++;
                            }
                            else
                            {
                                j--;
                            }
                        }
                }
            }
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
            toggleMethod();
            return 0;
        }
        else if (attackNum == 2)
        {
            target.doHeal(10, true);
            return 0;
        }
        else if (attackNum == 3)
        {
            target.doHeal(2, false);
            return 0;
        }
        else if(attackNum == 4)
        {
            int temp = damageTaken * 2;
            return temp;
        }
        else if (attackNum == 5)
        {
            int temp = damageTaken * 2;
            return temp;
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
        else if (attackNum == 4)
        {
            
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
        if(attackNum == 3)
        {
            return 1;
        }
        return base.checkCanAttack(target, attackNum);
        
    }
}
