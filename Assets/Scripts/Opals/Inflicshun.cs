using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inflicshun : OpalScript
{
    string currentForm = "000";

    public Sprite burningAttack;
    public Sprite burningHurt;

    public Sprite poisonAttack;
    public Sprite poisonHurt;

    public Sprite liftedAttack;
    public Sprite liftedHurt;

    private GameObject poisonPart;
    private GameObject liftedPart;
    private GameObject burningPart;

    private Sprite poisonDefault;
    private Sprite liftedDefault;
    private Sprite burningDefault;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 1;
        speed = 2;
        priority = 8;
        myName = "Inflicshun";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Unstable", 0, 0, 0, "<Passive>\nInflicshun will change form depending on the status conditions it is inflicted by. It takes half damage from status conditions.");
        Attacks[1] = new Attack("Slash", 1, 1, 6, "Deal close range damage",0,3);
        Attacks[2] = new Attack("Blast", 3, 4, 4, "Deal damage from further away",0,3);
        Attacks[3] = new Attack("Tune", 0, 1, 0, "Gain +1 attack",0,3);
        type1 = "Void";
        type2 = "Void";
        anim = GetComponent<Animator>();
        setUpInflicshun();
    }

    private void setUpInflicshun()
    {
        foreach(Transform g in GetComponentsInChildren<Transform>())
        {
            if(g.name == "Poison")
            {
                poisonPart = g.gameObject;
            }else if (g.name == "Lifted")
            {
                liftedPart = g.gameObject.gameObject;
            }else if (g.name == "Burning")
            {
                burningPart = g.gameObject;
            }
        }

        setVisuals();
    }

    public void doFrame(string frame, bool active)
    {
        if (active)
        {
            burningDefault = burningPart.GetComponent<SpriteRenderer>().sprite;
            burningPart.GetComponent<Animator>().enabled = false;
            poisonDefault = poisonPart.GetComponent<SpriteRenderer>().sprite;
            poisonPart.GetComponent<Animator>().enabled = false;
            liftedDefault = liftedPart.GetComponent<SpriteRenderer>().sprite;
            liftedPart.GetComponent<Animator>().enabled = false;
            if (frame == "attack")
            {
                burningPart.GetComponent<SpriteRenderer>().sprite = burningAttack;
                poisonPart.GetComponent<SpriteRenderer>().sprite = poisonAttack;
                liftedPart.GetComponent<SpriteRenderer>().sprite = liftedAttack;
            }
            else if(frame == "hurt")
            {
                burningPart.GetComponent<SpriteRenderer>().sprite = burningHurt;
                poisonPart.GetComponent<SpriteRenderer>().sprite = poisonHurt;
                liftedPart.GetComponent<SpriteRenderer>().sprite = liftedHurt;
            }
        }
        else
        {
            burningPart.GetComponent<SpriteRenderer>().sprite = burningDefault;
            burningPart.GetComponent<Animator>().enabled = true;
            poisonPart.GetComponent<SpriteRenderer>().sprite = poisonDefault;
            poisonPart.GetComponent<Animator>().enabled = true;
            liftedPart.GetComponent<SpriteRenderer>().sprite = liftedDefault;
            liftedPart.GetComponent<Animator>().enabled = true;
            setVisuals();
        }
    }

    private void setVisuals()
    {
        if (currentForm.Substring(0,1) == "1")
        {
            //burningPart.GetComponent<Animator>().CrossFade("Inflicshun-Burning", 0);
            burningPart.GetComponent<SpriteRenderer>().enabled = true;
            burningPart.GetComponent<Animator>().enabled = true;
        }
        else
        {
            burningPart.GetComponent<SpriteRenderer>().enabled = false;
            burningPart.GetComponent<Animator>().enabled = false;
        }

        if (currentForm.Substring(1, 1) == "1")
        {
            //poisonPart.GetComponent<Animator>().CrossFade("Inflicshun-Poison", 0);
            poisonPart.GetComponent<SpriteRenderer>().enabled = true;
            poisonPart.GetComponent<Animator>().enabled = true;
        }
        else
        {
            poisonPart.GetComponent<SpriteRenderer>().enabled = false;
            poisonPart.GetComponent<Animator>().enabled = false;
        }

        if (currentForm.Substring(2, 1) == "1")
        {
            //liftedPart.GetComponent<Animator>().CrossFade("Inflicshun-Lifted", 0);
            liftedPart.GetComponent<SpriteRenderer>().enabled = true;
            liftedPart.GetComponent<Animator>().enabled = true;
        }
        else
        {
            liftedPart.GetComponent<SpriteRenderer>().enabled = false;
            liftedPart.GetComponent<Animator>().enabled = false;
        }
        restartHighlight();
        anim.Play("Inflicshun", -1, 0);
        poisonPart.GetComponent<Animator>().Play("Inflicshun-Poison", -1, 0);
        burningPart.GetComponent<Animator>().Play("Inflicshun-Burning",-1, 0);
        liftedPart.GetComponent<Animator>().Play("Inflicshun-Lifted",-1, 0);
    }

    private void getForm()
    {
        if (getBurning() && getPoison() && getLifted())
        {
            Attacks[1] = new Attack("Omega Slash", 1, 1, 15, "Give target burn, poison, and lift. Heal by half the damage dealt.",0,3);
            Attacks[2] = new Attack("Omega Blast", 8, 4, 10, "Add damage based on current burn and poison damage. The target is pushed 2 tiles.",0,3);
            Attacks[3] = new Attack("Omega Tune", 0, 1, 0, "Heal 10 health. Gain +3 attack, +3 defense, and +2 speed.",0,3);
            //anim.Play("Inflicshun111", 0);
            currentForm = "111";
        }
        else if(getBurning() && getPoison())
        {
            Attacks[1] = new Attack("Painful Slash", 1, 1, 10, "Give target burn and poison.",0,3);
            Attacks[2] = new Attack("Painful Blast", 5, 4, 8, "Add damage based on current burn and poison damage.",0,3);
            Attacks[3] = new Attack("Painful Tune", 0, 1, 0, "Gain +3 attack, +3 defense.",0,3);
            //anim.Play("Inflicshun110", 0);
            currentForm = "110";
        }
        else if(getPoison() && getLifted())
        {
            Attacks[1] = new Attack("Nauseous Slash", 1, 1, 10, "Give target poison and lift.",0,3);
            Attacks[2] = new Attack("Nauseous Blast", 5, 4, 8, "Add damage based on current poison damage. The target is pushed 2 tiles.",0,3);
            Attacks[3] = new Attack("Nauseous Tune", 0, 1, 0, "Gain +3 defense and +2 speed.",0,3);
            //anim.Play("Inflicshun011", 0);
            currentForm = "011";
        }
        else if(getBurning() && getLifted())
        {
            Attacks[1] = new Attack("Flaming Slash", 1, 1, 10, "Give target burn and lift.",0,3);
            Attacks[2] = new Attack("Flaming Blast", 5, 4, 8, "Add damage based on current burn damage. The target is pushed 2 tiles.",0,3);
            Attacks[3] = new Attack("Flaming Tune", 0, 1, 0, "Gain +3 attack and +2 speed.",0,3);
            //anim.Play("Inflicshun101", 0);
            currentForm = "101";
        }
        else if (getBurning())
        {
            Attacks[1] = new Attack("Searing Slash", 1, 1, 8, "Burn target.",0,3);
            Attacks[2] = new Attack("Searing Blast", 4, 4, 6, "Add damage based on current burn damage.",0,3);
            Attacks[3] = new Attack("Searing Tune", 0, 1, 0, "Gain +2 attack.",0,3);
            //anim.Play("Inflicshun100", 0);
            currentForm = "100";
        }
        else if (getPoison())
        {
            Attacks[1] = new Attack("Sickly Slash", 1, 1, 8, "Poison target.",0,3);
            Attacks[2] = new Attack("Sickly Blast", 4, 4, 6, "Add damage based on current poison damage.",0,3);
            Attacks[3] = new Attack("Sickly Tune", 0, 1, 0, "Gain +2 defense.",0,3);
            //anim.Play("Inflicshun010", 0);
            currentForm = "010";
        }
        else if (getLifted())
        {
            Attacks[1] = new Attack("Floaty Slash", 1, 1, 8, "Lift target.",0,3);
            Attacks[2] = new Attack("Floaty Blast", 4, 4, 6, "The target is pushed 1 tile.",0,3);
            Attacks[3] = new Attack("Floaty Tune", 0, 1, 0, "Gain +1 speed.",0,3);
            //anim.Play("Inflicshun001", 0);
            currentForm = "001"; 
        }
        else
        {
            Attacks[1] = new Attack("Slash", 1, 1, 6, "Deal close range damage",0,3);
            Attacks[2] = new Attack("Blast", 3, 4, 4, "Deal damage from further away",0,3);
            Attacks[3] = new Attack("Tune", 0, 1, 0, "Gain +1 attack",0,3);
            //anim.Play("Inflicshun000", 0);
            currentForm = "000";
        }
        doHighlight("Inflicshun");
        //setVisuals();
    }

    public override void onStart()
    {
        getForm();
        setVisuals();
        showHighlight();
    }

    public override void onMove(int distanceMoved)
    {
        getForm();
        showHighlight();
        //setVisuals();
    }

    public override void onMove(PathScript p)
    {
        getForm();
        setVisuals();
        showHighlight();
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
