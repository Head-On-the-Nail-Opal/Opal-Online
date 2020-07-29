using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoviron : OpalScript
{
    private CursorScript cs;
    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 3;
        defense = 2;
        speed = 4;
        priority = 1;
        myName = "Hoviron";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
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
        Attacks[1] = new Attack("Take Off", 0, 1, 0, "Push adjacent Opals away by 2 tiles. They gain lift. Gain +1 Armor for each.", 1);
        Attacks[0] = new Attack("Hovering", 0, 0, 0, "<Passive>\n When taking damage from an adjacent Opal, push the attacker away by three tiles.");
        Attacks[2] = new Attack("Rocket Blast", 3, 4, 4, "Push target 2 tiles away. If they are Lifted then gain +2 attack and +1 speed.");
        Attacks[3] = new Attack("Quick Flight",20,1,0,"Fly to any tile on the map.");
        type1 = "Metal";
        type2 = "Air";
        if (pl != null)
        {
            cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
        }
    }

    public override void onDamage(int dam)
    {
        if (dam > 0 || dam == -1)
        {
            if (cs.getCurrentOpal() != this)
            {
                string direct = "left";
                int dist = (int)getPos().x - (int)cs.getCurrentOpal().getPos().x;
                if (dist == 0)
                {
                    direct = "down";
                    dist = (int)getPos().z - (int)cs.getCurrentOpal().getPos().z;
                }
                if (dist < 0)
                {
                    if (direct == "left")
                    {
                        direct = "right";
                    }
                    else if (direct == "down")
                    {
                        direct = "up";
                    }
                    dist = Mathf.Abs(dist);
                }
                pushAway(3, cs.getCurrentOpal());
            }
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 1) //Thorns
        {
            if(target != this && target != null)
            {
                pushAway(2, target);
                target.setLifted(true);
                addArmor(1);
            }
            return 0;
        }
        else if (attackNum == 0) //Seed Launch
        {
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            pushAway(2, target);
            if (target.getLifted())
            {
                doTempBuff(0, -1, 2);
                doTempBuff(2, -1, 1);
            }
        }
        else if (attackNum == 3) //Seed Launch
        {
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

        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            doMove((int)target.getPos().x, (int)target.getPos().z, 0);
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
        if (attackNum == 3 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 3)
        {
            return 0;
        }
        return -1;
    }
}
