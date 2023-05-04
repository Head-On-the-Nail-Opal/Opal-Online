using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHighlighter : MonoBehaviour
{
    GroundScript board;
    int Id;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUp(GroundScript b, int id)
    {
        board = b;
        Id = id;
    }

    private void OnMouseEnter()
    {
        if(board != null)
        {
            board.highlightOpal(Id, true);
            gameObject.transform.localScale *= 1.1f;
            board.updateCurrent(Id);
        }
    }

    private void OnMouseExit()
    {
        if (board != null)
        {
            board.highlightOpal(Id, false);
            gameObject.transform.localScale /= 1.1f;
            board.updateCurrent(-1);
        }
    }
}
