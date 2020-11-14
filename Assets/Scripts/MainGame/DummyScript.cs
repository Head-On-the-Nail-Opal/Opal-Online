using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour {

    public Material playerSelected;

    private Vector3 pos;
    private GroundScript boardScript;
    public int spawned;

    void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        spawned = -1;
    }

    /**
     * Attack state
     * 0 = movement, no attack
     * 1 = normal shaped attack, no line of sight needed
     * 2 = normal shaped attack, line of sight needed
     * 3 = "adjacent attack", normal shaped but flood tiles extend range
     * 4 = line attack, line of sight needed
     * 5 = target just growth tiles
     * 6 = laser beam, line which ignores line of sight
     * 7 = target all opals
     * 8 = diagonal laser beam
     * */
    public void Spawn(int tD, int d, int attackState, bool original)
    {
        if(attackState == 7)
        {
            findHighestAttack(tD);
            return;
        }
        if(attackState == 5)
        {
            findAllGrowths(tD);
            return;
        }
        if (attackState == 3 && !onFlood() && original)
        {
            Spawn(tD, d, 1, true);
            return;
        }
        if(attackState == 8)
        {
            findDiagonals();
            return;
        }
        if ((d < tD && this.spawned < (tD - d)))
        {
            int numLoop = 0;
            DummyScript[] temps = new DummyScript[4];
            int[] directions = new int[4];
            this.spawned = tD - d;
            for (int i = -1; i < 2; i ++)
            {
                for (int j = -1; j < 2; j ++)
                {
                    if (i == 0 || j == 0)
                    {
                        if ((pos.x + i > -1 && pos.x + i < 10 && pos.z + j > -1 && pos.z + j < 10))
                        {
                            if (boardScript.dummies[(int)pos.x + i, (int)pos.z + j] == null && !boardScript.tileGrid[(int)pos.x + i, (int)pos.z + j].getFallen())
                            {
                                if (boardScript.tileGrid[(int)pos.x + i, (int)pos.z + j].currentPlayer == null && boardScript.tileGrid[(int)pos.x + i, (int)pos.z + j].type != "Boulder" || (attackState == 1 || attackState == 3 || attackState == 4 || attackState == 6))
                                {
                                    if ((attackState == 3 && boardScript.tileGrid[(int)pos.x + i, (int)pos.z + j].type != "Flood")  && !original)
                                    {
                                        if(Mathf.Abs(i) == Mathf.Abs(j))
                                            break;
                                    }
                                    else
                                    {
                                        DummyScript tempDummy = Instantiate<DummyScript>(this);
                                        tempDummy.setCoordinates((int)pos.x + i, (int)pos.z + j);
                                        boardScript.dummies[(int)pos.x + i, (int)pos.z + j] = tempDummy;
                                        temps[numLoop] = tempDummy;
                                        if(i == 1 && j == 0)
                                        {
                                            directions[numLoop] = 3;
                                        }else if (i == -1 && j == 0)
                                        {
                                            directions[numLoop] = 0;
                                        }
                                        else if (i == 0 && j == 1)
                                        {
                                            directions[numLoop] = 2;
                                        }
                                        else
                                        {
                                            directions[numLoop] = 1;
                                        }
                                        numLoop++;
                                    }
                                }
                            }
                            else
                            {
                                if (boardScript.dummies[(int)pos.x + i, (int)pos.z + j] != null)
                                {
                                    boardScript.dummies[(int)pos.x + i, (int)pos.z + j].Spawn(tD, d + 1, attackState, false);
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                if (temps[i] != null)
                {
                    if (attackState == 3)
                    {
                        if(temps[i].onFlood())
                            temps[i].Spawn(tD, d, attackState, false);
                    }
                    else if(attackState == 4 || attackState == 6){
                        temps[i].SpawnLine(attackState == 4, directions[i], tD);
                    }
                    else
                    {
                        temps[i].Spawn(tD, d + 1, attackState, false);
                    }
                }
            }
        }
    }

    //dir: 0 = down, 1 = left, 2 = right, 3 = up
    private void SpawnLine(bool lineOfSight, int dir, int tD) // this code is still bugged
    {
        Vector2 currentPos = new Vector2(pos.x, pos.z);
        for (int i = 0; i < tD; i++)
        {
            if (dir == 0)
            {
                //print("x + i:" + i);
                if (pos.x - i > -1 && boardScript.dummies[(int)pos.x - i, (int)pos.z] == null && !boardScript.tileGrid[(int)pos.x - i, (int)pos.z].getFallen())
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates((int)pos.x - i, (int)pos.z);
                    boardScript.dummies[(int)pos.x - i, (int)pos.z] = tempDummy;
                    currentPos = new Vector2(pos.x - i, pos.z);
                }
            }
            else if (dir == 3)
            {
                //print("x - i:" + i);
                if (pos.x + i < 10 && boardScript.dummies[(int)pos.x + i, (int)pos.z] == null && !boardScript.tileGrid[(int)pos.x + i, (int)pos.z].getFallen())
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates((int)pos.x + i, (int)pos.z);
                    boardScript.dummies[(int)pos.x + i, (int)pos.z] = tempDummy;
                    currentPos = new Vector2(pos.x + i, pos.z);
                }
            }
            else if (dir == 2)
            {
                //print("z - i:" + i );
                if (pos.z + i < 10 && boardScript.dummies[(int)pos.x, (int)pos.z + i] == null && !boardScript.tileGrid[(int)pos.x, (int)pos.z + i].getFallen())
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates((int)pos.x, (int)pos.z + i);
                    boardScript.dummies[(int)pos.x, (int)pos.z + i] = tempDummy;
                    currentPos = new Vector2(pos.x, pos.z + i);
                }
            }
            else if(dir == 1)
            {
                //print("z + i:" + i);
                if (pos.z - i > -1 && boardScript.dummies[(int)pos.x, (int)pos.z - i] == null && !boardScript.tileGrid[(int)pos.x, (int)pos.z - i].getFallen())
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates((int)pos.x, (int)pos.z - i);
                    boardScript.dummies[(int)pos.x, (int)pos.z - i] = tempDummy;
                    currentPos = new Vector2(pos.x, pos.z - i);
                }
            }
            if ((boardScript.tileGrid[(int)currentPos.x, (int)currentPos.y].currentPlayer != null || boardScript.tileGrid[(int)currentPos.x, (int)currentPos.y].type == "Boulder") && lineOfSight)
            {
                return;
            }
        }
    }

    private void findAllGrowths(int range)
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {

                if (boardScript.tileGrid[i, j].type == "Growth" && !boardScript.tileGrid[i,j].getFallen())
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates(i, j);
                    boardScript.dummies[i,j] = tempDummy;
                    //tempDummy.Spawn(range, 0, 1, false);
                }
            }
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (boardScript.tileGrid[i, j].type == "Growth" && !boardScript.tileGrid[i, j].getFallen())
                { 
                    boardScript.dummies[i,j].Spawn(range, 0, 1, false);
                }
            }
        }
        DestroyImmediate(this.gameObject);
    }

    private void findDiagonals()
    {
        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                if(Mathf.Abs(i) == Mathf.Abs(j) && (i != 0 && j != 0) && (pos.x + i > -1 && pos.x + i < 10 && pos.z + j > -1 && pos.z + j < 10))
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(this);
                    tempDummy.setCoordinates((int)(pos.x + i),(int)( pos.z + j));
                    boardScript.dummies[(int)(pos.x + i),(int)( pos.z + j)] = tempDummy;
                }
            }
        }
        DestroyImmediate(this.gameObject);
    }

    private void findHighestAttack(int range)
    {
        foreach(OpalScript o in boardScript.gameOpals)
        {
            if (!o.getDead() && o != boardScript.myCursor.getCurrentOpal())
            {
                DummyScript tempDummy = Instantiate<DummyScript>(this);
                tempDummy.setCoordinates((int)o.getPos().x, (int)o.getPos().z);
                boardScript.dummies[(int)o.getPos().x, (int)o.getPos().z] = tempDummy;
            }
        }
        foreach (OpalScript o in boardScript.gameOpals)
        {
            if (!o.getDead() && o != boardScript.myCursor.getCurrentOpal())
                boardScript.dummies[(int)o.getPos().x, (int)o.getPos().z].Spawn(range, 0, 1, false);
        }
        DestroyImmediate(this.gameObject);
    }

    public void updateDist(int tD, int D)
    {
    }

    public bool onFlood()
    {
        return (boardScript.tileGrid[(int)pos.x, (int)pos.z].type == "Flood");
    }

    public void setCoordinates(int row, int col)
    {
        transform.position = new Vector3(row, 0, col);
        pos = new Vector3(row, 0, col);
    }
}
