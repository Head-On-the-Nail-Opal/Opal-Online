using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heant : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 1;
        defense = 0;
        speed = 3;
        priority = 0;
        myName = "Heant";
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
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Hot Body", 0, 0, 0, "<Passive>\n On death leave a Flame at your location.");
        Attacks[1] = new Attack("Fiery Venom", 1, 1, 0, "Light the target tile on fire. Can target an unoccupied tile.",0,3);
        Attacks[2] = new Attack("Searing Sting", 1, 1, 0, "Raise the damage target Opal takes from their burn by 2.",0,3);
        Attacks[3] = new Attack("Full Venom", 1, 1, 0, "Target takes damage from their burn. Die.",0,3);
        type1 = "Fire";
        type2 = "Swarm";
    }

    public override void onDamage(int dam)
    {
        if (dam - getDefense() > health)
        {
            boardScript.setTile((int)getPos().x, (int)getPos().z, "Fire", false);
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if(target.getBurning())
                target.setBurningDamage(target.getBurningDamage() + 2);
            return 0;
        }else if(attackNum == 3)
        {
            if (target.getBurning())
            {
                target.takeDamage(target.getBurningDamage(), false, true);
                boardScript.callParticles("burning", target.getPos());
                target.setBurningDamage(target.getBurningDamage() + 2);
            }
            takeDamage(getHealth(), false, true);
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
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
            return 0;
        }
        else if (attackNum == 2)
        {
            return 0;
        }else if(attackNum == 3)
        {
            return target.currentPlayer.getBurningDamage();
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 1)
        {
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
