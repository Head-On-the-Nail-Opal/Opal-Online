using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luminute : OpalScript
{
    bool canMove = true;
    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 2;
        priority = 0;
        myName = "Luminute";
        //baseSize = new Vector3(0.2f, 0.2f, 1);
        transform.localScale = new Vector3(0.2f, 0.24f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0;
        offsetZ = 0;
        player = pl;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        Attacks[0] = new Attack("Distracted", 0, 0, 0, "Every other turn Luminute will be unable to act");
        Attacks[1] = new Attack("Overgrowth", 3, 1, 0, "Give an Opal +2 attack and defense. Place a growth under them. Has 2 uses.");
        Attacks[1].setUses(2);
        Attacks[2] = new Attack("Pretty Orb", 2, 1, 0, "Targets lose their next turn.", 1);
        Attacks[3] = new Attack("Orb Sprouts", 0, 5, 0, "Overheal a target on any Growth by 15 health, they lose -4 speed for 1 turn.");
        type1 = "Light";
        type2 = "Grass";
    }

    public override void onStart()
    {
        if (!canMove)
        {
            boardScript.getMyCursor().nextTurn();
        }
        canMove = !canMove;
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Balance
        {

            return 0;
        }
        else if (attackNum == 1) //Restore
        {
            boardScript.setTile(target, "Growth", false);
            target.doTempBuff(0, -1, 2);
            target.doTempBuff(1, -1, 2);
            return 0;
        }
        else if (attackNum == 2) //Shift
        {
            target.setSkipTurn(true);
            return 0;
        }else if(attackNum == 3)
        {
            target.doHeal(10, true);
            target.doTempBuff(2, 1, -4);
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
}
