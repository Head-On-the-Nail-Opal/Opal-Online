using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volcoco : OpalScript
{
    private int eruptionRange = 1;
    private int eruptionDamage = 0;
    private bool eruptionAOE = false;
    private int adjacentTargets = 0;

    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 3;
        speed = 1;
        priority = 6;
        myName = "Volcoco";
        transform.localScale = new Vector3(0.2f, 0.2f, 1) * 0.9f;
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
        Attacks[0] = new Attack("Eruption", 1, 1, 0, "Target tiles light on Fire.");
        Attacks[1] = new Attack("Rumble", 0, 1, 0, "Light current tile on Fire. Eruption gains +2 range.");
        Attacks[2] = new Attack("Spitting Fire", 1, 4, 0, "Light the target tile on Fire. Eruption deals +3 damage.");
        Attacks[3] = new Attack("Lava Flow", 0, 1, 0, "Light adjacent tiles on fire. If there are at least three Opals adjacent to Volcoco, next turn Eruption hits adjacent targets.", 1);
        type1 = "Fire";
        type2 = "Grass";
    }

    public override void onStart()
    {
        adjacentTargets = 0;
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile(target, "Fire", false);
            Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.");
        }
        else if (attackNum == 1)
        {
            boardScript.setTile(target, "Fire", false);
            eruptionRange += 2;
            Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.");
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Fire", false);
            eruptionDamage += 3;
            Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.");
            return 0;
        }
        else if (attackNum == 3)
        {
            if (target != this)
            {
                boardScript.setTile(target, "Fire", false);
                adjacentTargets++;
                if (adjacentTargets >= 3)
                {
                    Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.", 1);
                }
            }
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0)
        {
            boardScript.setTile(target, "Fire", false);
            Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.");
            return 0;
        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Fire", false);
            eruptionDamage += 3;
            Attacks[0] = new Attack("Eruption", eruptionRange, 1, eruptionDamage, "Target tiles light on Fire.");
            return 0;
        }
        else if (attackNum == 2)
        {
            boardScript.setTile(target, "Fire", false);
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

        }
        else if (attackNum == 1)
        {
            return 0;
        }
        else if (attackNum == 2)
        {
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
