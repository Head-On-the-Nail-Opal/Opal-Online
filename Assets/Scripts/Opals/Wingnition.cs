using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wingnition : OpalScript
{
    private Bombat bombatPrefab;
    private int boulderDamage = 0;
    private bool bombatted = false;
    private Vector2 origPos = new Vector2();

    public override void onAwake()
    {
        bombatPrefab = Resources.Load<Bombat>("Prefabs/SubOpals/Bombat");

    }

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 0;
        defense = 2;
        speed = 4;
        priority = 7;
        myName = "Wingnition";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 1.1f;
        offsetX = 0;
        offsetY = 0.1f;
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
        Attacks[0] = new Attack("Call Bombat", 1, 0, 0, "<Free Ability>\nSummon an exploding Bombat once per turn");
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Coddle", 1, 1, 0, "Give target +3 speed for 1 turn. Heal them 5 health and they gain lift.");
        Attacks[2] = new Attack("Retract", 0, 1, 0, "Fly back to the tile you started your turn on.");
        Attacks[3] = new Attack("Unrelenting Flap", 1, 1, 0, "Push the target 5 tiles, give them +4 attack, and give them lift.");
        type1 = "Air";
        type2 = "Swarm";
    }

    public override void onStart()
    {
        if (!dead)
        {
            bombatted = false;
            origPos = new Vector2(currentTile.getPos().x, currentTile.getPos().z);
        }
    }

    public override void onMove(int distanceMoved)
    {
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            target.doTempBuff(2, 1, 3);
            target.doHeal(1, false);
            target.setLifted(true);
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {
            doMove((int)origPos.x, (int)origPos.y,0);
            return 0;
        }else if(attackNum == 3)
        {
            string direct = "right";
            int dist = (int)getPos().x - (int)target.getPos().x;
            if (dist == 0)
            {
                direct = "up";
                dist = (int)getPos().z - (int)target.getPos().z;
            }
            if (dist < 0)
            {
                if (direct == "right")
                {
                    direct = "left";
                }
                else if (direct == "up")
                {
                    direct = "down";
                }
                dist = Mathf.Abs(dist);
            }
            if (direct == "right")
            {
                target.nudge(5, true, false);
            }
            else if (direct == "left")
            {
                target.nudge(5, true, true);
            }
            else if (direct == "up")
            {
                target.nudge(5, false, false);
            }
            else if (direct == "down")
            {
                target.nudge(5, false, true);
            }
            target.doTempBuff(0, -1, 4);
            target.doHeal(5, false);
            target.setLifted(true);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Duplicate
        {
            spawnOplet(bombatPrefab, target);
            bombatted = true;
            return 0;
        }
        else if (attackNum == 1) //Insight
        {
            return 0;
        }
        else if (attackNum == 2) //Spectral Lunge
        {

        }
        return cA.getBaseDamage() + getAttack();
    }

    void setIt(OpalScript os)
    {
        currentTile.standingOn(os);
    }

    public override List<OpalScript> getOplets()
    {
        List<OpalScript> temp = new List<OpalScript>();
        temp.Add(bombatPrefab);
        return temp;
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
        if (attackNum == 0 && target.currentPlayer == null && !bombatted)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum != 0)
        {
            return 0;
        }
        return -1;
    }
}
