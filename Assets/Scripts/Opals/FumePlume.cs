using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumePlume : OpalScript
{
    private int ashIncrement = 1;
    //(10,0,0,3,"FumePlume", 0.8f,0, 0, 0, "Red", "Fire", "Plague")
    override public void setOpal(string pl)
    {
        health = 25;
        maxHealth = health;
        attack = 1;
        defense = 3;
        speed = 3;
        priority = 4;
        myName = "FumePlume";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.8f;
        offsetX = 0;
        offsetY = 0.02f;
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
        Attacks[0] = new Attack("Smoke Screen", 0, 1, 0, "Place 2 miasma and 2 flame tiles surrounding you. If you're standing on a flame or miasma tile, spawn only that tile.");
        Attacks[1] = new Attack("Exhaust", 4, 1, 6, "Place flames at your feet and miasma at the target's feet. If either are already occupied with said tiles then spawn on adjacent tiles.");
        Attacks[2] = new Attack("Ash Breath", 2, 1, 0, "<Incremental>\nBurn and poison a target. If you're on a miasma or flame tile, the target loses 1 attack or defense respectively.");
        Attacks[3] = new Attack("Furnace", 3, 1, 0, "<Free Ability>\n If standing on Miasma or Flame, place that tile. Lose 1 speed.");
        Attacks[3].setFreeAction(true);
        type1 = "Fire";
        type2 = "Plague";
        og = true;
    }


    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Smokescreen
        {
            if(getBoard().tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire")
            {
                getBoard().setTile((int)getPos().x + 1, (int)getPos().z, "Fire", false);
                getBoard().setTile((int)getPos().x - 1, (int)getPos().z, "Fire", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z + 1, "Fire", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z - 1, "Fire", false);
            }
            else if (getBoard().tileGrid[(int)getPos().x, (int)getPos().z].type == "Miasma")
            {
                getBoard().setTile((int)getPos().x + 1, (int)getPos().z, "Miasma", false);
                getBoard().setTile((int)getPos().x - 1, (int)getPos().z, "Miasma", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z + 1, "Miasma", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z - 1, "Miasma", false);
            }
            getBoard().setTile((int)getPos().x + 1, (int)getPos().z, "Fire", false);
            getBoard().setTile((int)getPos().x - 1, (int)getPos().z, "Fire", false);
            getBoard().setTile((int)getPos().x, (int)getPos().z + 1, "Miasma", false);
            getBoard().setTile((int)getPos().x, (int)getPos().z - 1, "Miasma", false);
            return cA.getBaseDamage() - getAttack();
        }
        else if (attackNum == 1) //Exhaust
        {
            if (getBoard().tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire")
            {
                getBoard().setTile((int)getPos().x + 1, (int)getPos().z, "Fire", false);
                getBoard().setTile((int)getPos().x - 1, (int)getPos().z, "Fire", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z + 1, "Fire", false);
                getBoard().setTile((int)getPos().x, (int)getPos().z - 1, "Fire", false);
            }
            else
            {
                getBoard().setTile((int)getPos().x, (int)getPos().z, "Fire", false);
            }
            if (getBoard().tileGrid[(int)target.getPos().x, (int)target.getPos().z].type == "Miasma")
            {
                getBoard().setTile((int)target.getPos().x + 1, (int)target.getPos().z, "Miasma", false);
                getBoard().setTile((int)target.getPos().x - 1, (int)target.getPos().z, "Miasma", false);
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z + 1, "Miasma", false);
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z - 1, "Miasma", false);
            }
            else { 
                getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Miasma", false);
            }
        }
        else if (attackNum == 2) //Ash Breath
        {
            target.setBurning(true);
            target.setPoison(true);
            if (getBoard().tileGrid[(int)getPos().x, (int)getPos().z].type == "Fire")
            {
                target.doTempBuff(1, -1, -ashIncrement);
            }
            if (getBoard().tileGrid[(int)getPos().x, (int)getPos().z].type == "Miasma")
            {
                target.doTempBuff(0, -1, -ashIncrement);
            }
            ashIncrement++;
            Attacks[2].setDescription("Incremental\nBurn and poison a target. If you're on a miasma or flame tile, the target loses "+ ashIncrement +" attack or defense respectively.");
            return 0;
        }
        else if (attackNum == 3) //Ash Breath
        {
            if(currentTile.type == "Miasma")
            {
                boardScript.setTile(target, "Miasma", false);
            }
            else if(currentTile.type == "Fire")
            {
                boardScript.setTile(target, "Fire", false);
            }
            doTempBuff(2, -1, -1);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 3) //Thorns
        {
            if (currentTile.type == "Miasma")
            {
                boardScript.setTile(target, "Miasma", false);
            }
            else if (currentTile.type == "Fire")
            {
                boardScript.setTile(target, "Fire", false);
            }
            doTempBuff(2, -1, -1);
        }
        return 0;
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
