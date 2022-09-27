using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glintrey : OpalScript
{
    List<OpalScript> buddies = new List<OpalScript>();

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 1;
        priority = 7;
        myName = "Glintrey";
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
        Attacks[0] = new Attack("King's Order", 3, 1, 0, "Spawn a Reflectron, a mirror that will reflect your lasers.");
        Attacks[1] = new Attack("Straight Ray", 1, 6, 3, "Deal damage to all Opals in a line",0,3);
        Attacks[2] = new Attack("Diagonal Ray", 1, 8, 3, "Deal damage to all Opals in a diagonal line",0,3);
        Attacks[3] = new Attack("Shift Order", 0, 1, 0, "<Free Ability>\nAll of your Reflectrons shift their angle");
        Attacks[3].setFreeAction(true);
        type1 = "Laser";
        type2 = "Swarm";
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

        }
        else if (attackNum == 2)
        {
 
        }
        else if (attackNum == 3)
        {
            foreach (OpalScript buddy in buddies)
            {
                if (!buddy.getDead())
                {
                    buddy.toggleMethod();
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
            
            OpalScript buddy = spawnOplet(Resources.Load<OpalScript>("Prefabs/SubOpals/Reflectron"), target);
            if(buddy != null)
                buddies.Add(buddy);
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
        if (attackNum == 0 || attackNum == 1 || attackNum == 2 || attackNum == 3)
        {
            return 0;
        }
        else
            return -1;
    }
}
