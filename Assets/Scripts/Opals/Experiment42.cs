using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment42 : OpalScript
{
    int currentUpgrade = 1;
    public bool isCorpse = false;

    public Sprite deadSprite;
    private List<Sprite> aliveSprites;

    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 3;
        priority = 2;
        myName = "Experiment42";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
        offsetX = 0;
        offsetY = 0;
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
        Attacks[0] = new Attack("Immortal", 0, 0, 0, "<Passive>\nAfter Experiment42 dies it leaves an unbreakable corpse. That corpse can return to being Experiment42.");
        Attacks[1] = new Attack("Upgrade", 1, 1, 6, "Deal damage, and Experiment42 gains +" + currentUpgrade + " attack and +" + currentUpgrade + " defense.",0,3);
        Attacks[2] = new Attack("Fortify", 0, 1, 0, "Gain +1 armor, lose -1 speed for 1 turn.",0,3);
        Attacks[3] = new Attack("Tinker",0,1,0,"Upgrade provides an additional +1 attack and defense permanently. Take 5 damage.",0,3);
        type1 = "Metal";
        type2 = "Spirit";
        og = true;

        getSpeciesPriorities().AddRange(new List<Behave>{
            new Behave("Close-Combat", 1, 10), new Behave("Cautious", 1, 1),
            new Behave("Safety", 0,1) });
    }

    public void corpse(TileScript target)
    {
        health = 1;
        maxHealth = 10;
        type2 = "Plague";
        Attacks[0] = new Attack("Undead", 0, 0, 0, "<Passive>\n Starts damaged. When this reaches full health it will revive itself.");
        Attacks[1] = new Attack("Impervious", 0, 0, 0, "<Passive>\n This corpse cannot be destroyed until every allied Opal is dead.");
        Attacks[2] = new Attack("Returning", 0, 1, 0, "Heal 5 health.");
        Attacks[3] = new Attack("Toxic Byproduct", 0, 1, 0, "Poison Opals on adjacent tiles.");

        clearAllBuffs();
        speed = 0;

        transform.localScale = new Vector3(3f, 3f, 1) * 1f;

        pauseAnim = true;
        pauseFrame = true;
        GetComponent<SpriteRenderer>().sprite = myBoulders;

        doHighlight();
        if (boardScript.myCursor.getCurrentOpal() == this)
            showHighlight();

        isCorpse = true;
    }

    private void revive(TileScript target)
    {
        health = 10;
        maxHealth = health;
        type2 = "Spirit";
        Attacks[0] = new Attack("Immortal", 0, 0, 0, "<Passive>\nAfter Experiment42 dies it leaves an unbreakable corpse. That corpse can return to being Experiment42.");
        Attacks[1] = new Attack("Upgrade", 1, 1, 8, "Deal damage, and Experiment42 gains +" + currentUpgrade + " attack and +" + currentUpgrade + " defense.");
        Attacks[2] = new Attack("Fortify", 0, 1, 0, "Gain +1 armor, lose -1 speed for 1 turn.");
        Attacks[3] = new Attack("Tinker", 0, 1, 0, "Upgrade provides an additional +1 attack and defense permanently. Take 5 damage.");

        clearAllBuffs();
        speed = 3;

        transform.localScale = new Vector3(3f, 3f, 1) * 1f;

        pauseAnim = false;
        pauseFrame = false;

        doHighlight();
        if (boardScript.myCursor.getCurrentOpal() == this)
            showHighlight();

        isCorpse = false;
    }

    public override void onDeathTile(TileScript t)
    {
        corpse(t);
    }

    public override void onHeal(int amount)
    {
        if(health + amount >= maxHealth && isCorpse && currentTile != null)
        {
            revive(currentTile);
        }
    }

    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Upgrade
        {
            return 0;
        }
        else if (attackNum == 1) //Optimize
        {
            doTempBuff(0, -1, currentUpgrade);
            doTempBuff(1, -1, currentUpgrade);
        }
        else if (attackNum == 2) //Sick Shot
        {
            if (isCorpse)
            {
                doHeal(5, false);
                return 0;
            }
            addArmor(1);
            doTempBuff(2, 2, -1);
            return 0;
        }
        else if (attackNum == 3) //Sick Shot
        {
            if (isCorpse)
            {
                foreach(TileScript t in getSurroundingTiles(true))
                {
                    if(t.getCurrentOpal() != null)
                    {
                        t.getCurrentOpal().setPoison(true);
                    }
                }
                return 0;
            }
            currentUpgrade++;
            takeDamage(5, false, true);
            Attacks[1] = new Attack("Upgrade", 1, 1, 0, "Deal damage, and Experiment42 gains +" + currentUpgrade + " attack and +" + currentUpgrade + " defense.");
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
            return Attacks[attackNum].getBaseDamage() + getAttack() + currentUpgrade - target.currentPlayer.getDefense();
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

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if (!isCorpse)
        {
            if (atNum == 0)
            {
                return false;
            }
            else if (atNum == 1)
            {
                if(targettingEnemy(target))
                    return true;
            }
            else if (atNum == 2)
            {
                if(notNearEnemy(target))
                    return true;
            }
            else if (atNum == 3)
            {
                if (notNearEnemy(target) && health > 10)
                    return true;
            }
        }
        else
        {
            if (atNum == 0)
            {
                return false;
            }
            else if (atNum == 1)
            {
                return false;
            }
            else if (atNum == 2)
            {
                return true;
            }
            else if (atNum == 3)
            {
                int output = 0;
                foreach (TileScript t in getSurroundingTiles(true))
                {
                    if (t.getCurrentOpal() != null)
                        output++;
                }
                if(output >= 2)
                    return true;
            }
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }
}
