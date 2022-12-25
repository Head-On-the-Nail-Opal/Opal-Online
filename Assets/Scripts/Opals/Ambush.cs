using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : OpalScript { 
    override public void setOpal(string pl)
    {
        health = 15;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 4;
        priority = 9;
        myName = "Ambush";
        transform.localScale = new Vector3(3f, 3f, 1)*0.85f;
        if (pl == "Red" || pl == "Green")
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        offsetX = 0;
        offsetY = -0.1f;
        offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Relocation", 0, 5, 0, "<Free Ability>\n Teleport from a Growth tile to any Growth tile.", 0,0);
        Attacks[0].addProjectile("Default", "Default", 1, Color.black, 5);
        Attacks[0].setFreeAction(true);
        Attacks[1] = new Attack("Seed Launch", 4, 1, 0, "Place a Growth at the feet of your target and at your feet.",0,3);
        Attacks[1].addProjectile("Single", "Default", 20, Color.green, 1);
        Attacks[1].getTags().Add("Growth");
        Attacks[2] = new Attack("Thorned Swipe", 1, 1, 8, "May move after using this ability.",0,3);
        Attacks[2].getTags().Add("MoveAfter");
        Attacks[3] = new Attack("Dimishing Wrap", 0, 1, 0, "Gain +1 attack for each adjacent Opal. Adjacent Opals lose -1 defense. May move after using this ability.", 1,3);
        type1 = "Grass";
        type2 = "Dark";

        getSpeciesPriorities().AddRange(new List<Behave>{ 
            new Behave("Cautious", 1, 3), new Behave("Ambush", 1, 10),
            new Behave("Acrophobic", 0,1), new Behave("Green-Thumb", 0, 5) });
        getSpeciesAwareness().AddRange(new List<Behave> { new Behave("Ambush-Wary", 0, 1) });
    }

    public override void prepAttack(int attackNum)
    {
        if (attackNum == 0)
        {
            moveAfter = true;
        }
        else if (attackNum == 1)
        {
            moveAfter = false;
        }
        else if (attackNum == 2)
        {
            moveAfter = true;
        }
        else if (attackNum == 3)
        {
            moveAfter = true;
        }
    }

    public override void preFire(int attackNum, TileScript target)
    {
        base.preFire(attackNum, target);
        if(attackNum == 0)
        {
            if(target.type == "Growth" && target.currentPlayer == null)
            {
                //summonNewParticle("Vanish");
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
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {

        }
        else if (attackNum == 3) //Grass Cover
        {
            if(target != this)
            {
                doTempBuff(0, -1, 1);
                target.doTempBuff(1, -1, -1);
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
            if(currentTile.type == "Growth") //there is some issue with setting the currentTile type
            {
                if(target.type == "Growth")
                {
                    doMove((int)target.getPos().x, (int)target.getPos().z, 0);
                }
            }
            return 0;
        }
        else if (attackNum == 1) //Seed Launch
        {
            getBoard().setTile((int)target.getPos().x, (int)target.getPos().z, "Growth", false);
            boardScript.setTile(this, "Growth", false);
            return 0;
        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
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
            return 0;
        }
        else if (attackNum == 2)
        {
            
        }
        else if (attackNum == 3)
        {
            return 0;
        }
        return Attacks[attackNum].getBaseDamage() + getAttack() - target.currentPlayer.getDefense();
    }

    public override int checkCanAttack(TileScript target, int attackNum)
    {
        if (attackNum == 0 || attackNum == 1)
        {
            if(attackNum == 0 && target.currentPlayer != null)
            {
                return -1;
            }
            if(attackNum == 0 && currentTile.type != "Growth")
            {
                return -1;
            }
            return 0;
        }
        
        if(attackNum == 3)
        {
            foreach(TileScript t in getSurroundingTiles(true))
            {
                if (t.currentPlayer != null)
                    return 0;
            }
            return -1;
        }

        if (target.currentPlayer != null)
        {
            return 0;
        }
        return -1;
    }

    public override bool getIdealAttack(int atNum, TileScript target)
    {
        if(atNum == 0)
        {
            //teleport to opponents at the start of the turn and away from them at the end
            if((!useAroundOpal(currentTile, true, 2) && useAroundOpal(target, true, 3) && !boardScript.myCursor.getFinishAttack()) || (boardScript.myCursor.getFinishAttack() && useAroundOpal(currentTile, true, 2) && !useAdjacentToOpal(target, true))
                && !boardScript.myCursor.tileIsFalling((int)target.getPos().x, (int)target.getPos().z))
            {
                return true;
            }
        }
        else if(atNum == 1)
        {
            if(useAdjacentToOpal(target, true) && target.type != "Growth" && !useAdjacentToOpal(currentTile, true) && target.currentPlayer == null && !boardScript.myCursor.tileIsFalling((int)target.getPos().x, (int)target.getPos().z))
            {
                return true;
            }
        }else if(atNum == 2)
        {
            if (targettingEnemy(target))
                return true;
        }else if(atNum == 3)
        {
            return false;
        }
        return false;
    }

    public override int getMaxRange()
    {
        return 1;
    }
}
