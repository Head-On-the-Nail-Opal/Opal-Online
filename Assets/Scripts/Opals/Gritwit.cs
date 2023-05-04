using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gritwit : OpalScript
{
    int lastAttack = 0;
    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 4;
        priority = 3;
        myName = "Gritwit";
        transform.localScale = new Vector3(3f, 3f, 1) * 1.2f;
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
        Attacks[0] = new Attack("Drop Off", 1, 0, 0, "Lose -1 attack and place a boulder. If your attack is less than one, do nothing instead.",0,3);
        Attacks[1] = new Attack("Reclaim", 1, 1, 0, "Break a boulder and gain +1 attack.",0,3);
        Attacks[2] = new Attack("Gather", 0, 1, 0, "If next to Excremite then gain +3 attack",0,3);
        Attacks[3] = new Attack("Assist", 1, 1, 0, "Give target +1 attack and +3 defense. Lose -1 attack.",0,3);
        type1 = "Ground";
        type2 = "Swarm";
    }

    public override void onDamage(int dam)
    {
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {

        }
        else if (attackNum == 1) //Seed Launch
        {
            if (target.getMyName() == "Boulder")
            {
                target.takeDamage(target.getHealth(), false, false);
                doTempBuff(0, -1, 1);
            }
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            if((boardScript.tileGrid[(int)getPos().x + 1, (int)getPos().z].currentPlayer != null && boardScript.tileGrid[(int)getPos().x+1,(int)getPos().z].currentPlayer.getMyName() == "Excremite")
                || (boardScript.tileGrid[(int)getPos().x - 1, (int)getPos().z].currentPlayer != null && boardScript.tileGrid[(int)getPos().x - 1, (int)getPos().z].currentPlayer.getMyName() == "Excremite") ||
                (boardScript.tileGrid[(int)getPos().x, (int)getPos().z + 1].currentPlayer != null && boardScript.tileGrid[(int)getPos().x, (int)getPos().z + 1].currentPlayer.getMyName() == "Excremite") ||
                (boardScript.tileGrid[(int)getPos().x, (int)getPos().z - 1].currentPlayer != null && boardScript.tileGrid[(int)getPos().x, (int)getPos().z - 1].currentPlayer.getMyName() == "Excremite"))
            {
                doTempBuff(0, -1, 3);
            }
            return 0;
        }else if(attackNum == 3)
        {
            target.doTempBuff(0, -1, 1);
            target.doTempBuff(1, -1, 3);
            doTempBuff(0, -1, -1);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            if (getAttack() > 0)
            {
                doTempBuff(0, -1, -1);
                int tempx = (int)target.getPos().x;
                int tempz = (int)target.getPos().z;
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Boulder", false);
            }
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
        if (attackNum == 0)
        {
            if (getAttack() > 0)
                return 0;
            else
                return -1;
        }else if(attackNum == 2)
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if (t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Excremite")
                    return 0;
            }
            return -1;
        }else if (attackNum == 1)
        {
            if (target.getCurrentOpal() != null && target.getCurrentOpal().getMyName() == "Boulder")
                return 0;
            return -1;
        }
        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }
}
