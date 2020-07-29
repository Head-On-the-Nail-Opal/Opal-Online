using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluttorch : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 4;
        defense = 1;
        speed = 4;
        priority = 5;
        myName = "Fluttorch";
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
        offsetY = 0f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Torched", 0, 0, 0, "<Passive>\n Place flame tiles under your feet as you move.");
        Attacks[1] = new Attack("Emberflap", 1, 4, 4, "Push target back 1 tile. Set the tile they started on and the tile they landed on on fire.");
        Attacks[2] = new Attack("Heatburst", 0, 1, 0, "Gain +1 speed for 1 turn for each surrounding Flame.");
        Attacks[3] = new Attack("Extinguishing Slash",0,1,0,"Put out adjacent flames. Adjacent Opals that were standing on flames take 12 damage.",1);
        type1 = "Fire";
        type2 = "Air";

    }

    public override void onMove(PathScript p)
    {
        boardScript.setTile((int)p.getPos().x, (int)p.getPos().z, "Fire", false);
        currentTile = boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z];
        //boardScript.tileGrid[(int)p.getPos().x, (int)p.getPos().z].standingOn(null);
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
            Vector3 temp = target.getPos();
            pushAway(1, target);
            boardScript.setTile((int)temp.x, (int)temp.z, "Fire", false);
            
            boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
        }
        else if (attackNum == 2) //Grass Cover
        {
            int num = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type == "Fire")
                    {
                        num++;
                    }
                }
            }
            doTempBuff(2, 2, num);
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            if(target.getCurrentTile().type == "Fire")
            {
                boardScript.setTile(target, "Grass", true);
                if (target == this)
                {
                    return 0;
                }
                return 12 + getAttack();
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
        else if (attackNum == 3) //Grass Cover
        {
            if(target.type == "Fire")
                boardScript.setTile(target, "Grass", true);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 2)
        {
            return 0;
        }
        else if (attackNum == 3)
        {
            if(target.type == "Fire")
            {
                return 12 + getAttack() - target.currentPlayer.getDefense();
            }
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 3)
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
