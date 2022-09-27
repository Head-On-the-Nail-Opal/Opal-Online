using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nachteous : OpalScript
{

    override public void setOpal(string pl)
    {
        health = 30;
        maxHealth = health;
        attack = 0;
        defense = 4;
        speed = 2;
        priority = 1;
        myName = "Nachteous";
        transform.localScale = new Vector3(3f, 3f, 1f)*1.25f;
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
        Attacks[0] = new Attack("Vector", 0, 1, 0, "Lower Nachteous's attack and defense by -2, spawn miasma at Nachteous's feet and on adjacent tiles", 0,3);
        Attacks[1] = new Attack("Spead Sickness", 3, 4, 0, "Spawn miasma at the feet of all targets, and inflict them with all stat changes Nachteous carries", 2, 3);
        Attacks[2] = new Attack("Bile Shot", 3, 4, 1, "Clear all stat changes on Nachteous and deal damage for each negative point of attack.", 0,3);
        Attacks[3] = new Attack("Re-Uptake", 0, 1, 0, "<Free Ability>\n Remove Miasma at your feet in order to lower your attack by -2.",0,3);
        Attacks[3].setFreeAction(true);
        type1 = "Plague";
        type2 = "Dark";
        og = true;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //
        {
            doTempBuff(0, -1, -2);
            doTempBuff(1, -1, -2);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z, "Miasma", false);
            boardScript.setTile((int)transform.position.x + 1, (int)transform.position.z, "Miasma", false);
            boardScript.setTile((int)transform.position.x - 1, (int)transform.position.z, "Miasma", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z + 1, "Miasma", false);
            boardScript.setTile((int)transform.position.x, (int)transform.position.z - 1, "Miasma", false);
            return 0;
        }
        else if (attackNum == 1) //
        {
            List<TempBuff> temp = getBuffs();
            foreach (TempBuff t in temp)
            {
                target.doTempBuff(t.getTargetStat(), t.getTurnlength(), t.getAmount());
            }
            boardScript.setTile((int)target.transform.position.x, (int)target.transform.position.z, "Miasma", false);
            return 0;
        }
        else if (attackNum == 2) //
        {
            int addon = 0;
            if (getAttack() < 0)
                addon -= getAttack();
            clearBuffs();
            return cA.getBaseDamage() + addon;
        }else if(attackNum == 3)
        {
            if(currentTile.type == "Miasma")
            {
                boardScript.setTile(target, "Grass", true);
                doTempBuff(0, -1, -2);
            }
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
            int addon = 0;
            if(getAttack() < 0)
                addon -= getAttack();
            return Attacks[attackNum].getBaseDamage() + addon - target.currentPlayer.getDefense();
        }else if(attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }
}
