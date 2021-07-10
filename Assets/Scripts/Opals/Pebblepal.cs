using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pebblepal : OpalScript
{
    private CursorScript cs;
    private bool atSwitch = false;
    private bool noEarly = true;
    private int rA = 5;
    private ParticleSystem myAura;

    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 2;
        defense = 5;
        speed = 0;
        priority = 9;
        myName = "Pebblepal";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.7f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if(pl != null)
        {
            cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
            myAura = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/RocklyAuraBuff"), transform);
            myAura.transform.localScale = transform.localScale;
            //myAura.transform.localRotation = Quaternion.Euler(0, 90, 35);
        }
        offsetX = 0;
        offsetY = -0.3f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Rockly Aura", 0, 0, 0, "<Passive>\nAt the end of Pebblepal's turn, friendly Opals adjacent to Pebblepal gain +5 attack and defense for 1 turn");
        Attacks[1] = new Attack("Friend Boost", 0, 0, 0, "<Passive>\n Each Boulder adjacent to Pebblepal extends the range of its Rockly Aura ");
        Attacks[2] = new Attack("Indecisive", 0, 1, 0, "Rockly Aura deals debuffs to adjacent enemy Opals instead of buffing.");
        Attacks[3] = new Attack("Roll", 1, 1, 0, "Move one tile over.");
        type1 = "Ground";
        type2 = "Air";
    }


    public override void onStart()
    {
        //cs = GameObject.Find("Cursor(Clone)").GetComponent<CursorScript>();
    }

    public override void onEnd()
    {
        int currentBoulderCount = 1;
        foreach(TileScript t in getSurroundingTiles(true))
        {
            if(t.getCurrentOpal() != null && t.getCurrentOpal().getMyName() == "Boulder")
            {
                currentBoulderCount++;
            }
        }
        for(int i = -(currentBoulderCount); i < (currentBoulderCount)+1; i++)
        {
            for(int j = -(currentBoulderCount); j < (currentBoulderCount)+1; j++)
            {
                if (getPos().x + i < 10 && getPos().x + i > -1 && getPos().z + j < 10 && getPos().z + j > -1 && !(i == 0 && j == 0) && ((Mathf.Abs(i) != Mathf.Abs(j) || (Mathf.Abs(i) != Mathf.Abs(currentBoulderCount) && Mathf.Abs(j) != Mathf.Abs(currentBoulderCount)))))
                {
                    if (!atSwitch)
                    {
                        if(boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.getTeam() == getTeam())
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.doTempBuff(0, 1, 5);
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.doTempBuff(1, 1, 5);
                        }
                    }
                    else
                    {
                        if (boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer != null && boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.getTeam() != getTeam())
                        {
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.doTempBuff(0, 1, -8);
                            boardScript.tileGrid[(int)getPos().x + i, (int)getPos().z + j].currentPlayer.doTempBuff(1, 1, -8);
                        }
                    }
                }
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            atSwitch = !atSwitch;
            if (atSwitch)
            {
                Attacks[0] = new Attack("Rockly Aura", 0, 0, 0, "<Passive>\nAt the end of Pebblepal's turn, enemy Opals adjacent to Pebblepal gain -8 attack and defense for 1 turn");
                DestroyImmediate(myAura.gameObject);
                myAura = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/RocklyAuraHarm"), transform);
            }
            else
            {
                Attacks[0] = new Attack("Rockly Aura", 0, 0, 0, "<Passive>\nAt the end of Pebblepal's turn, friendly Opals adjacent to Pebblepal gain +5 attack and defense for 1 turn");
                DestroyImmediate(myAura.gameObject);
                myAura = Instantiate<ParticleSystem>(Resources.Load<ParticleSystem>("Prefabs/ParticleSystems/RocklyAuraBuff"), transform);
            }
            myAura.transform.localScale = transform.localScale;
            //myAura.transform.localRotation = Quaternion.Euler(0, 90, 35);
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
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
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {

            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            doMove((int)target.getPos().x, (int)target.getPos().z, 1);
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
            if (atSwitch)
            {
                return 15 + attack - target.currentPlayer.getDefense();
            }
            return 0;
        }
        else if (attackNum == 2)
        {
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
