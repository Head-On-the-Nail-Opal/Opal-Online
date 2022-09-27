using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InanimateOpal : OpalScript
{
    public override int getAttackEffect(int attackNum, OpalScript target)
    {
        return 0;
    }

    public override void onMove(int x, int z)
    {
        print("tile at " + x + ", " + z+" is a " + boardScript.tileGrid[x, z].type);
        if(boardScript.tileGrid[x, z].type == "Flood")
        {
            boardScript.setTile(x, z, "Grass", false);
            takeDamage(getHealth(), false, true);
            print("du hello");
        }
    }

    public override void onStatusCondition(bool poison, bool burn, bool lift)
    {
        healStatusEffects();
    }

    override public void setOpal(string pl)
    {
        health = 5;
        maxHealth = health;
        attack = 0;
        defense = 0;
        speed = 0;
        priority = 0;
        myName = "Boulder";
        transform.localScale = new Vector3(0.12f, 0.12f, 1);
        ///transform.position = new Vector3(transform.position.x + 0.3f, transform.position.y-0.3f, transform.position.z - 0.3f);
        //offsetX = 0.6f;
        offsetY = -0.2f;
        //offsetZ = -0.6f;
        if (Random.Range(0,2) == 0)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }
        //offsetX = 0;
        //offsetY = 0f;
        //offsetZ = 0;
        player = pl;
        Attacks[0] = new Attack("Boulder", 0, 0, 0, "Boulders do not act and are stationary obstacles");
        Attacks[1] = new Attack("Boulder", 0, 0, 0, "Boulders can be pushed and broken by attacks");
        Attacks[2] = new Attack("Boulder", 0, 0, 0, "Boulders can be buffed and healed");
        Attacks[3] = new Attack("Boulder", 0, 0, 0, "");
        type1 = "Ground";
        type2 = "Ground";
    }

    //public void setPos(int x, int y)
    //{
     //   transform.position = new Vector3(x + offsetX, 0.4f + offsetY, y + offsetZ);
   // }
}
