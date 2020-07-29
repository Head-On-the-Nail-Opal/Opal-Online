using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

    private Vector3 pos;
    private GroundScript boardScript;
    private TileScript currentTile;
    private int zOffset = 0;
    private int xOffset = 0;
    public int spawned;
    private int attack;
    private OpalScript current;
    SpriteRenderer sr;

    void Awake()
    {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
        sr = GetComponentInChildren<SpriteRenderer>();
        spawned = -1;
    }

    public void Spawn(int range, int shape, List<Target> buff)
    {
        if(range == 0)
        {
            return;
        }
        if (shape > 59 && shape < 64)
        {
            for(int i = 0; i < range - 1; i++)
            {
                if(shape == 60)
                {
                    Target tempTarget = Instantiate<Target>(this);
                    tempTarget.setCoordinates((int)pos.x + i + 1, (int)pos.z, attack, current);
                    tempTarget.setOffsets(i + 1, 0);
                    buff.Add(tempTarget);
                }
                else if(shape == 61)
                {
                    Target tempTarget = Instantiate<Target>(this);
                    tempTarget.setCoordinates((int)pos.x - i - 1, (int)pos.z, attack, current);
                    tempTarget.setOffsets(-i - 1, 0);
                    buff.Add(tempTarget);
                }
                else if(shape == 62)
                {
                    Target tempTarget = Instantiate<Target>(this);
                    tempTarget.setCoordinates((int)pos.x, (int)pos.z + i + 1, attack, current);
                    tempTarget.setOffsets(0, i + 1);
                    buff.Add(tempTarget);
                }
                else if(shape == 63)
                {
                    Target tempTarget = Instantiate<Target>(this);
                    tempTarget.setCoordinates((int)pos.x, (int)pos.z - i - 1, attack, current);
                    tempTarget.setOffsets(0, -i - 1);
                    buff.Add(tempTarget);
                }
            }
        }
        else
        {
            Target tempTarget = Instantiate<Target>(this);
            tempTarget.setCoordinates((int)pos.x + 1, (int)pos.z, attack, current);
            tempTarget.setOffsets(1, 0);
            buff.Add(tempTarget);

            tempTarget = Instantiate<Target>(this);
            tempTarget.setCoordinates((int)pos.x - 1, (int)pos.z, attack, current);
            tempTarget.setOffsets(-1, 0);
            buff.Add(tempTarget);

            tempTarget = Instantiate<Target>(this);
            tempTarget.setCoordinates((int)pos.x, (int)pos.z + 1, attack, current);
            tempTarget.setOffsets(0, 1);
            buff.Add(tempTarget);

            tempTarget = Instantiate<Target>(this);
            tempTarget.setCoordinates((int)pos.x, (int)pos.z - 1, attack, current);
            tempTarget.setOffsets(0, -1);
            buff.Add(tempTarget);
        }
    }

    public void setCoordinates(int row, int col, int attackNum, OpalScript currentOpal)
    {
        attack = attackNum;
        current = currentOpal;
        
        if (row > 9 || row < 0 || col > 9 || col < 0 || boardScript.dummies[row, col] == null)
        {
            transform.position = new Vector3(-100, -100, -100);
            pos = new Vector3(row, 0, col);
            currentTile = null;
            return;
        }
        transform.position = new Vector3(row, 0, col);
        pos = new Vector3(row, 0, col);
        currentTile = boardScript.tileGrid[row, col];
        if (attackNum != -1)
        {
            if (currentOpal.checkCanAttack(currentTile, attackNum) == -1 && !(currentTile.type == "Boulder" && currentOpal.getAttacks()[attackNum].getBaseDamage() > 0))
            {
                transform.position = new Vector3(-100, -100, -100);
                pos = new Vector3(-100, -100, -100);
                currentTile = null;
                return;
            }
        }
    }

    public bool checkCoordinates(int attackNum, OpalScript currentOpal)
    {
        if (attackNum != -1)
        {
            if (currentOpal.checkCanAttack(currentTile, attackNum) == -1)
            {
                sr.color = new Color(0, 0, 0, 0.5f);
                currentTile = null;
                return false;
            }
            return true;
        }
        return false;
    }

    public void setOffsets(int x, int z)
    {
        xOffset = x;
        zOffset = z;
    }

    public void setRelativeCoordinates(int x, int z, int attackNum, OpalScript currentOpal)
    {
        
        if (x + xOffset > 9 || x + xOffset < 0 || z + zOffset > 9 || z + zOffset < 0 || boardScript.dummies[x, z] == null || boardScript.tileGrid[x+xOffset, z+zOffset].getFallen())
        {
            transform.position = new Vector3(-100,-100,-100);
            pos = new Vector3(-100, -100, -100);
            currentTile = null;
            return;
        }
        transform.position = new Vector3(x + xOffset, 0, z + zOffset);
        pos = new Vector3(x + xOffset, 0, z + zOffset);
        currentTile = boardScript.tileGrid[x + xOffset, z + zOffset];
        if (attackNum != -1)
        {
            if (currentOpal.checkCanAttack(currentTile, attackNum) == -1 && !(currentTile.type == "Boulder" && currentOpal.getAttacks()[attackNum].getBaseDamage() > 0))
            {
                sr.color = new Color(0, 0, 0, 0.5f);
                currentTile = null;
                return;
            }
        }
    }

    public TileScript getTile()
    {
        return currentTile;
    }

}
