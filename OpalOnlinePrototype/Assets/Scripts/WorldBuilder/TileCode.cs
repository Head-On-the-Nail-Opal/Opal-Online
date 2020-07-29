using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCode : MonoBehaviour
{
    public string baseCode;
    private BoxCollider2D borrowing;
    private int assigned = -1;
    private string secondaryCode = "__";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getCode()
    {
        return baseCode;
    }

    public Vector2 getPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public BoxCollider2D getBorrowing()
    {
        return borrowing;
    }

    public void setBorrowing(BoxCollider2D b)
    {
        borrowing = b;
    }

    public int getAssigned()
    {
        return assigned;
    }

    public void setAssigned(int a)
    {
        assigned = a;
    }

    public void setSecondaryCode(string s)
    {
        if(s.Length == 1)
        {
            s = '0' + s;
        }
        secondaryCode = s;
    }

    public string getSecondary()
    {
        return secondaryCode;
    }

    public void setSpawning(bool spawn)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (spawn)
        {
            sr.color = Color.red;
        }
        else
        {
            sr.color = Color.white;
        }
    }
}
