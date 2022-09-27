using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpalBrain : MonoBehaviour
{
    private GroundScript mainGame;
    private CursorScript myCursor;

    private List<Behave> generalBehaviours = new List<Behave>();

    private TileScript attackTarget;
    private TileScript moveTarget;
    private int attackNum = -1;

    public void Awake()
    {
        mainGame = GameObject.FindObjectOfType<GroundScript>();
    }


    //myCursor.doMove(List<Pathscript> paths)
    //myCursor.doAttack(int x, int y, int attackNum)
    //myCursor.doEndTurn()
    public void Update()
    {
        if (myCursor == null)
            myCursor = mainGame.getMyCursor();
        else
        {
            if (myCursor.getCurrentController() == "AI")
            {
                if (myCursor.getCurrentOpal().getBehaviours().Count == 0)
                {
                }
                myCursor.doEndTurn();
            }
        }
    }



    
    private void assessMyBehaviour(OpalScript me, Behave b)
    {
        switch (b.getName()) {
            case "Killer":
                foreach(OpalScript o in mainGame.gameOpals)
                {
                    if(o.getTeam() != me.getTeam())
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            if(me.checkCanAttack(o.getCurrentTile(), i) > -1 && me.getAttackDamage(i, o.getCurrentTile()) >= o.getHealth() && canIReachTarget(me, o, i))
                            {

                            }
                        }
                    }
                }
                break;
            case "Safety":
                break;
            case "Pressure":
                break;

            case "Green-Thumb":
                break;
            case "Growth-Teleport":
                break;

            case "Ambush-Wary":
                break;

        }
    }

    private bool canIReachTarget(OpalScript me, OpalScript target, int intendedAttackRange) //going to modify A-Star to acknowledge behaviours and tile damage risk. (maybe copy it into here so it doesn't mess with other pathing)
    {
        foreach (TileScript t in target.getSurroundingTiles(true))
        {
            List<Vector2> result = myCursor.Astar(me.getPos(), t.getPos());
            return result != null && result.Count < myCursor.getDistance() + intendedAttackRange;
        }
        return false;
    }
}
public class Behave
{
    private int priority;
    private int intensity;
    private string behaviorName;

    public Behave(string n, int p, int i)
    {
        priority = p;
        behaviorName = n;
        intensity = i;
    }

    public int getPriority()
    {
        return priority;
    }

    public void setPriority(int p)
    {
        priority = p;
    }

    public int getIntensity()
    {
        return intensity;
    }

    public void setIntensity(int i)
    {
        intensity = i;
    }

    public string getName()
    {
        return behaviorName;
    }

    public void setName(string name)
    {
        behaviorName = name;
    }
}
