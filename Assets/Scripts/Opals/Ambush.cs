using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : OpalScript { 
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 4;
        priority = 9;
        myName = "Ambush";
        transform.localScale = new Vector3(0.2f, 0.2f, 1)* 0.9f;
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
        Attacks[0] = new Attack("Relocation", 0, 5, 0, "<Free Ability>\n Teleport from a Growth tile to any Growth tile.");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Seed Launch", 4, 1, 0, "Place a Growth at the feet of your target and at your feet.");
        Attacks[2] = new Attack("Thorned Swipe", 1, 1, 8, "May move after using this attack.");
        Attacks[3] = new Attack("Dimishing Wrap", 0, 1, 0, "Gain +1 attack for each adjacent Opal. Adjacent Opals lose -1 defense. May move after using this attack.", 1);
        type1 = "Grass";
        type2 = "Dark";
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = true;
        }
        else if (attackNum == 1)
        {
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
        else if (attackNum == 3)
        {
            moveAfter = true;
        }
    }

    public override void preFire(int attackNum, TileScript target)
    {
        base.preFire(attackNum, target);
        if(attackNum == 0)
        {
            if(target.type == "Growth" && target.currentPlayer == null)
            {
                summonParticle("Vanish");
            }
        }
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
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {

        }
        else if (attackNum == 3) //Grass Cover
        {
            if(target != this)
            {
                doTempBuff(0, -1, 1);
                target.doTempBuff(1, -1, -1);
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            if(currentTile.type == "Growth") //there is some issue with setting the currentTile type
            {
                if(target.type == "Growth")
                {
                    doMove((int)target.getPos().x, (int)target.getPos().z, 0);
                }
            }
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
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
            
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 || attackNum == 1)
        {
            if(attackNum == 0 && target.currentPlayer != null)
            {
                return -1;
            }
            return 0;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
