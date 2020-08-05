using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    public Sprite NoAttack;
    public Sprite Attack0;
    public Sprite Attack1;
    public Sprite Attack2;
    public Sprite Attack3;
    private SpriteRenderer sr;
    private int currentAttack = -1;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayAttack(int attackNum)
    {
        if(attackNum == -1)
        {
            sr.sprite = NoAttack;
        }else if(attackNum == 0)
        {
            sr.sprite = Attack0;
        }
        else if (attackNum == 1)
        {
            sr.sprite = Attack1;
        }
        else if (attackNum == 2)
        {
            sr.sprite = Attack2;
        }
        else if (attackNum == 3)
        {
            sr.sprite = Attack3;
        }
    }
}
