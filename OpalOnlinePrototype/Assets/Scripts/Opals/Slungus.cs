using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slungus : OpalScript
{
    int inc = 1;
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 1;
        defense = 2;
        speed = 2;
        priority = 4;
        myName = "Slungus";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.5f;
        offsetX = 0;
        offsetY = -0.15f;
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
        Attacks[0] = new Attack("Dispersal", 0, 1, 0, "Place a growth at your feet and Miasma on adjacent tiles.");
        Attacks[1] = new Attack("Fumigate", 0, 1, 0, "Give adjacent Opals -5 attack and -5 defense for 1 turn. Buff them +3 attack and +3 defense.", 2);
        Attacks[2] = new Attack("Infest", 1, 4, 12, "Deal 12 damage. If standing on a Growth this attack has +3 range. If target is on Miasma, spawn Miasma on tiles adjacent to target");
        Attacks[3] = new Attack("Sick Spore", 3, 1, 0, "<Incremental> Poison a target and they lose -1 defense.");
        type1 = "Plague";
        type2 = "Grass";
    }

    public override void onStart()
    {
        if (getPos().x < 0 || getPos().x > 9 || getPos().z < 0 || getPos().z > 9)
        {
            return;
        }
        if (boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)].type == "Growth")
        {
            Attacks[2] = new Attack("Infest", 4, 4, 12, "Deal 12 damage. If standing on a Growth this attack has +3 range. If target is on Miasma, spawn Miasma on tiles adjacent to target");
        }
        else
        {
            Attacks[2] = new Attack("Infest", 1, 4, 12, "Deal 12 damage. If standing on a Growth this attack has +3 range. If target is on Miasma, spawn Miasma on tiles adjacent to target");
        }
    }

    public override void onMove(int d)
    {
        if (getPos().x < 0 || getPos().x > 9 || getPos().z < 0 || getPos().z > 9)
        {
            return;
        }
        if (boardScript.tileGrid[(int)(getPos().x), (int)(getPos().z)].type == "Growth")
        {
            Attacks[2] = new Attack("Infest", 4, 4, 12, "Deal 12 damage. If standing on a Growth this attack has +3 range. If target is on Miasma, spawn Miasma on tiles adjacent to target");
        }
        else
        {
            Attacks[2] = new Attack("Infest", 1, 4, 12, "Deal 12 damage. If standing on a Growth this attack has +3 range. If target is on Miasma, spawn Miasma on tiles adjacent to target");
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            boardScript.setTile((int)getPos().x, (int)getPos().z, "Growth", false);
            boardScript.setTile((int)getPos().x+1, (int)getPos().z, "Miasma", false);
            boardScript.setTile((int)getPos().x - 1, (int)getPos().z, "Miasma", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z + 1, "Miasma", false);
            boardScript.setTile((int)getPos().x, (int)getPos().z - 1, "Miasma", false);
            return 0;
        }
        else if (attackNum == 1) //
        {
           if(target.getPos() != getPos())
            {
                target.doTempBuff(0, 1, -5);
                target.doTempBuff(1, 1, -5);
                target.doTempBuff(0, -1, 3);
                target.doTempBuff(1, -1, 3);
            }
            return 0;
        }
        else if (attackNum == 2) //
        {
            if(boardScript.tileGrid[(int)(target.getPos().x), (int)(target.getPos().z)].type == "Miasma")
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (Mathf.Abs(i) != Mathf.Abs(j))
                        {
                            getBoard().setTile((int)target.getPos().x + i, (int)target.getPos().z + j, "Miasma", false);
                        }
                    }
                }
            }
        }else if(attackNum == 3)
        {

            target.setPoison(true);
            target.doTempBuff(1, -1, -inc);
            inc++;
            Attacks[3] = new Attack("Sick Spore", 3, 1, 0, "<Incremental> Poison a target and they lose -"+inc+" defense.");
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {

        }
        else if (attackNum == 1) //
        {

        }
        else if (attackNum == 2) //
        {

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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
