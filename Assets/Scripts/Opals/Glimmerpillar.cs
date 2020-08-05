using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glimmerpillar : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 10;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 0;
        priority = 9;
        myName = "Glimmerpillar";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.6f;
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
        Attacks[0] = new Attack("Pillar", 0, 0, 0, "<Passive>\n At the start of its turn, clear the tile under Glimmerpillar of any effects.");
        Attacks[1] = new Attack("Lamplight", 4, 1, 0, "Give target +2 attack and +2 defense for 1 turn.");
        Attacks[2] = new Attack("Soothe", 0, 1, 0, "Adjacent tiles effects are cleared. Adjacent Opals are cured of status effects.", 1);
        Attacks[3] = new Attack("Intensify", 4, 1, 0, "<Free Ability>\n Clear a tile effect. Take 5 damage.");
        type1 = "Swarm";
        type2 = "Light";
    }

    public override void onStart()
    {
        boardScript.setTile((int)getPos().x, (int)getPos().z, "Grass", true);
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            target.doTempBuff(0, 1, 2);
            target.doTempBuff(1, 1, 2);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if (target != this)
            {
                target.healStatusEffects();
            }
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
            return 0;
        }
        else if(attackNum == 3)
        {
            takeDamage(5, false, true);
            boardScript.setTile(target, "Grass", true);
            
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
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Grass", true);
            return 0;
        }
        else if (attackNum == 3)
        {
            takeDamage(5, false, true);
            boardScript.setTile(target, "Grass", true);

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
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 3 || attackNum == 2)
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
