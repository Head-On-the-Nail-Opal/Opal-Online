using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inseedious : OpalScript
{
    override public void setOpal(string pl)
    {
        health = 20;
        maxHealth = health;
        attack = 3;
        defense = 1;
        speed = 4;
        priority = 2;
        myName = "Inseedious";
        transform.localScale = new Vector3(3f, 3f, 1) * 1f;
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
        Attacks[0] = new Attack("Thorned Rose", 1, 1, 0, "Place a flower trap which deals 16 damage. During game this attack is disguised.",0,3);
        Attacks[1] = new Attack("Honey Suckle", 1, 1, 0, "Place a flower trap which heals its victim 8 health. During game this attack is disguised.",0,3);
        Attacks[2] = new Attack("Sunflower", 1, 1, 0, "Place a flower trap which buffs its victim +3 attack and +3 defense. During game this attack is disguised.",0,3);
        Attacks[3] = new Attack("Petal Brew", 1, 1, 0, "Reduce target Opal by -4 attack and -4 defense. Place a Growth at their feet.",0,3);
        type1 = "Dark";
        type2 = "Grass";
    }

    public override void onStart()
    {
        Attacks[0] = new Attack("Mystery Flower", 1, 1, 0, "Place a flower trap which 1-damages (16), 2-heals (8), or 3-buffs its victim (+3,+3).",0,3);
        Attacks[1] = new Attack("Mystery Flower", 1, 1, 0, "Place a flower trap which 1-damages (16), 2-heals (8), or 3-buffs its victim (+3,+3).", 0,3);
        Attacks[2] = new Attack("Mystery Flower", 1, 1, 0, "Place a flower trap which 1-damages (16), 2-heals (8), or 3-buffs its victim (+3,+3).", 0,3);
    }

    public override void prepAttack(int attackNum)
    {
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
        }
        else if (attackNum == 2) //Grass Cover
        {
            return 0;
        }
        else if (attackNum == 3) //Grass Cover
        {
            target.doTempBuff(0, -1, -4);
            target.doTempBuff(1, -1, -4);
            boardScript.setTile(target, "Growth", false);
            return 0;
        }
        return cA.getBaseDamage() + getAttack();
    }

    public override int getAttackEffect(int attackNum, TileScript target)
    {
        Attack cA = Attacks[attackNum];
        if (attackNum == 0) //Thorns
        {
            target.setTrap("Rose");
        }
        else if (attackNum == 1) //Seed Launch
        {
            target.setTrap("Honeysuckle");
        }
        else if (attackNum == 2) //Grass Cover
        {
            target.setTrap("Sunflower");
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
        if (attackNum != 3 && target.currentPlayer == null)
        {
            return 0;
        }
        if (target.currentPlayer != null && attackNum == 3)
        {
            return 0;
        }
        return -1;
    }
}
