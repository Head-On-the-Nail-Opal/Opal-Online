using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearthhog : OpalScript {
    private bool singleFire = false;
    int currentFire = 0;
    TempBuff cFireBuff;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 3;
        priority = 5;
        myName = "Hearthhog";
        transform.localScale = new Vector3(3f, 3f, 1)*1.25f;
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
        Attacks[0] = new Attack("Incendiary", 0, 0, 0, "<Passive>\nGain a temporary +1 attack for each Flame tile underfoot and on surrounding tiles.");
        Attacks[1] = new Attack("Inferno", 3, 4, 6, "Deal damage and light target tile on Flame. If you are standing on Flame also affect tiles adjacent to target. Doesn't need to target an Opal.",0,3);
        Attacks[1].addProjectile("Default","Flame",12, Color.white, 1);
        Attacks[2] = new Attack("Ignite", 0, 1, 0, "Ignite the tile at your feet. If you are standing on Flame then light all adjacent tiles.",0,3);
        Attacks[3] = new Attack("Flame Shield", 0, 1, 0, "Gain +1 defense for each point of attack you have, for 1 turn.");
        type1 = "Fire";
        type2 = "Fire";

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Cautious", 1, 3), new Behave("Line-Up", 1, 5),
            new Behave("Safety", 0,1), new Behave("Hot-Headed", 0, 10) });
    }

    public override void onStart()
    {
        singleFire = false;
        int tempFire = checkTiles();
        if(tempFire != currentFire)
        {
            if(cFireBuff != null)
            {
                cFireBuff.clearStat();
            }
            cFireBuff = doTempBuff(0, -1, tempFire);
            currentFire = tempFire;
            if (boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire")
            {
                Attacks[1].setAOE(1);
            }
            else
            {
                Attacks[1].setAOE(0);
            }
        }
    }

    public override void onMove(int d)
    {
        int tempFire = checkTiles();
        if (tempFire != currentFire)
        {
            if (cFireBuff != null)
            {
                cFireBuff.clearStat();
            }
            cFireBuff = doTempBuff(0, -1, tempFire);
            currentFire = tempFire;
        }
        if (boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire")
        {
            Attacks[1].setAOE(1);
        }
        else
        {
            Attacks[1].setAOE(0);
        }
    }

    private int checkTiles()
    {
        int num = 0;
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].type == "Fire")
                {
                    num++;
                }
            }
        }
        return num;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 1) //Inferno
        {
            if(target == this)
            {
                return 0;
            }
            if (singleFire == false)
            {
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
                target.takeDamage(cA.getBaseDamage() + getAttack(), true, true);
            }
            if (boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type != "Fire")
            {
                singleFire = true;
            }
            return 0;
        }
        else if (attackNum == 2) //Ignite
        {
            bool onfire = boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire";
             for (int i = -1; i < 2; i++)
             {
                for (int j = -1; j < 2; j++)
                {
                    if((onfire && Mathf.Abs(i) != Mathf.Abs(j)) || (i == 0 && j == 0))
                    {
                        boardScript.setTile((int)getPos().x + i, (int)getPos().z + j, "Fire", false);
                    }
                }
            }
            return 0;
        }
        else if (attackNum == 0) //Incendiary
        {
            return 0;
        }else if(attackNum == 3)
        {
            doTempBuff(1, 2, getAttack());
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 1) //Inferno
        {
            if (singleFire == false)
            {
                boardScript.setTile((int)target.getPos().x, (int)target.getPos().z, "Fire", false);
            }
            if (boardScript.tileGrid[(int)getPos().x, (int)getPos().z].type != "Fire")
            {
                singleFire = true;
            }
            return 0;
        }
        else if (attackNum == 2) //Ignite
        {
            
        }
        else if (attackNum == 0) //Incendiary
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
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

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (atNum == 0)
        {
            return false;
        }
        else if (atNum == 1)
        {
            if((useAdjacentToOpal(target, true) && currentTile.type == "Fire") || (target.currentPlayer != null && target.currentPlayer.getTeam() != getTeam()))
            {
                return true;
            }
        }
        else if (atNum == 2)
        {
            int total = 0;
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if (t.type == "Fire")
                    total++;
            }
            return total < 3;
        }
        else if (atNum == 3)
        {
            return false;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 3;
    }
}
