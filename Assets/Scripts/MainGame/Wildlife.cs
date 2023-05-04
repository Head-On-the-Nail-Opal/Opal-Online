using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wildlife : MonoBehaviour
{
    private List<Sprite> sprites = new List<Sprite>();
    public string myName = "";
    public int frameRate = 2;
    public bool doFlip = true;
    public float sizeMult = 1f;

    private int currentFrame = 0;

    private string currentState = "happy";

    private int increment = 0;
    private int indexAdjust = 0;
    private int flipCool = 40;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Texture2D texture = Resources.Load<Texture2D>("Spritesheets/Wildlife/" + myName);
        for (int i = 0; i < texture.width / 32; i++)
        {
            sprites.Add(Sprite.Create(texture, new Rect(i * 32, 0, 32, 32), new Vector2(0.5f, 0.5f)));
        }

        if (Random.Range(0, 2) == 1)
            sr.flipX = !sr.flipX;

        transform.localScale *= sizeMult;

    }


    private void FixedUpdate()
    {
        

        if(increment == frameRate * 5)
        {
            if (currentFrame == 2 || currentFrame == 3)
            {
                currentFrame = 0;
            }
            increment = 0;
            if (indexAdjust == 0 && currentState != "run")
                indexAdjust = 1;
            else
                indexAdjust = 0;

            if (currentState == "happy")
            {
                if (Random.Range(0, 20) == 4)
                {
                    currentFrame = Random.Range(2, 4);
                    indexAdjust = 0;
                }
            }
        }

        if(currentState != "run" && doFlip)
        {
            if(flipCool == 40 && Random.Range(0,30) == 4)
            {
                sr.flipX = !sr.flipX;
                flipCool = 0;
            }
            if (flipCool < 40)
                flipCool++;
        }

        if (currentState == "run")
            indexAdjust = 0;
        sr.sprite = sprites[currentFrame + indexAdjust];
        increment++;
  
    }

    public void adjustState(string change)
    {
        currentState = change;
        if(change == "happy")
        {
            currentFrame = 0;
        }
        else if(change == "scared")
        {
            currentFrame = 4;
        }
        else if(change == "run")
        {
            currentFrame = 6;
            StartCoroutine(runAway());
        }
        sr.sprite = sprites[currentFrame];
    }

    private IEnumerator runAway()
    {
        int xVel = -1;
        float speed = 0.02f;
        if (sr.flipX)
            xVel = 1;
        for(int i = 0; i < 100; i++)
        {
            transform.position = new Vector3(transform.position.x+xVel*speed, transform.position.y, transform.position.z+ xVel*speed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a-0.02f);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
