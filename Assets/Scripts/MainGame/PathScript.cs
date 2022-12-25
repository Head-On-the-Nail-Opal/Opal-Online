using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour {

    private GroundScript boardScript;
    public bool doBurn = false;
    public bool doPoison = false;

    // Use this for initialization
    void Awake () {
        GameObject board = GameObject.Find("Main Camera");
        boardScript = board.GetComponent<GroundScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setCoordinates(int x, int z)
    {
        if ((int)transform.position.x > -1 && (int)transform.position.z > -1 && (int)transform.position.x < 10 && (int)transform.position.z < 10)
        {
            transform.position = new Vector3(x, 0f, z);
            if (boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Fire" || boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Miasma" || boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Spore")
            {
                if (boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Fire")
                {
                    doBurn = true;
                }
                else
                {
                    doPoison = true;
                }
            }
        }
    }


    public Vector3 getPos()
    {
        return transform.position;
    }

    public void SpawnPath(Vector3 pathTo)
    {
        if(boardScript.dummies[(int)pathTo.x, (int)pathTo.z] == null)
        {
            return;
        }
        if(boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Fire" || boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Miasma" || boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Spore")
        {
            if(boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].type == "Fire")
            {
                doBurn = true;
            }
            else
            {
                doPoison = true;
            }
        }
        if (transform.position == pathTo)
        {
            return;
        } else if(transform.position.x < pathTo.x)
        {
            if (boardScript.tileGrid[(int)transform.position.x + 1, (int)transform.position.z].currentPlayer == null && boardScript.tileGrid[(int)transform.position.x + 1, (int)transform.position.z].type != "Boulder")
            {
                PathScript tempPath = Instantiate<PathScript>(this);
                tempPath.setCoordinates((int)transform.position.x + 1, (int)transform.position.z);
                tempPath.SpawnPath(pathTo);
                boardScript.paths.Add(tempPath);
            }
        }
        else if (transform.position.x > pathTo.x)
        {
            if (boardScript.tileGrid[(int)transform.position.x-1, (int)transform.position.z].currentPlayer == null && boardScript.tileGrid[(int)transform.position.x - 1, (int)transform.position.z].type != "Boulder")
            {
                PathScript tempPath = Instantiate<PathScript>(this);
                tempPath.setCoordinates((int)transform.position.x - 1, (int)transform.position.z);
                tempPath.SpawnPath(pathTo);
                boardScript.paths.Add(tempPath);
            }
        }
        else if (transform.position.z < pathTo.z)
        {
            if (boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z+1].currentPlayer == null && boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z+1].type != "Boulder")
            {
                PathScript tempPath = Instantiate<PathScript>(this);
                tempPath.setCoordinates((int)transform.position.x, (int)transform.position.z + 1);
                tempPath.SpawnPath(pathTo);
                boardScript.paths.Add(tempPath);
            }
        }
        else if (transform.position.z > pathTo.z)
        {
            if (boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z-1].currentPlayer == null && boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z-1].type != "Boulder")
            {
                PathScript tempPath = Instantiate<PathScript>(this);
                tempPath.setCoordinates((int)transform.position.x, (int)transform.position.z - 1);
                tempPath.SpawnPath(pathTo);
                boardScript.paths.Add(tempPath);
            }
        }
    }
}
