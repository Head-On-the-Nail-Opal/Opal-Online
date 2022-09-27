using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drizziphyl : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 2;
        defense = 2;
        speed = 2;
        priority = 6;
        myName = "Drizziphyl";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = 0f;
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
        Attacks[0] = new Attack("Shower", 0, 1, 0, "Adjacent tiles turn to Flood. Place a Growth under your feet. Buff adjacent Opals by +2 defense.",0,3);
        Attacks[1] = new Attack("Hydrate", 1, 3, 0, "<Water Rush>\nHeal the target and self 2 health. Place a Growth under the target. May use up to 4 times.",0,3);
        Attacks[1].setUses(4);
        Attacks[2] = new Attack("Aquaspit", 1, 3, 3, "<Water Rush>\n Deal damage and place Flood below and adjacent to target. May use this twice.",0,3);
        Attacks[2].setUses(2);
        Attacks[3] = new Attack("Wash", 1, 5, 0, "Place a Flood next to any Growth on the map. May use four times.",0,3);
        Attacks[3].setUses(4);
        type1 = "Grass";
        type2 = "Water";
        og = true;
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            boardScript.setTile((int)target.transform.position.x, (int)target.transform.position.z, "Growth", false);
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (Mathf.Abs(i) != Mathf.Abs(j))
                    {
                        getBoard().setTile((int)target.getPos().x + i, (int)target.getPos().z + j, "Flood", false);
                        if (boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].currentPlayer != null)
                        {
                            boardScript.tileGrid[(int)target.getPos().x + i, (int)target.getPos().z + j].currentPlayer.doTempBuff(1, -1, 2);
                        }
                    }
                }
            }
        }
        else if (attackNum == 1) //
        {
            target.doHeal(2, false);
            doHeal(2, false);
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //
        {
            getBoard().setTile(target, "Flood", false);
            foreach (TileScript t in target.getSurroundingTiles(true))
            {
                getBoard().setTile(t, "Flood", false);
            }
        }
        else if (attackNum == 3) //
        {
            boardScript.setTile(target, "Flood", false);
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
        else if (attackNum == 3) //
        {
            boardScript.setTile(target, "Flood", false);
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
            return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense(); ;
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
