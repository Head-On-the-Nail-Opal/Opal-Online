using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchGameOpal : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    BoxCollider2D bc;
    SpriteRenderer sr;
    OpalScript myOpal;
    private float speedMod = 5;
    private int decision = 0;
    private int upSpeed = 0;
    private int rightSpeed = 0;
    private int lifetime;
    private bool freeze = false;
    private bool snagged = false;
    private CatchGame parent;

    // Use this for initialization
    void Start()
    {
        parent = GetComponentInParent<CatchGame>();
        if (GetComponent<Rigidbody2D>() == null)
            rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        else
            rigidbody2D = GetComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        myOpal = GetComponent<OpalScript>();
        sr = GetComponent<SpriteRenderer>();
        transform.localScale *= 2;
        bc.size = new Vector2(4, 4);
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.mass = 0.0000001f;
            rigidbody2D.freezeRotation = true;
        }
        tag = "Catch";
        bc.isTrigger = true;
        lifetime = 500;
        StartCoroutine(fade());
    }

        // Update is called once per frame
    void Update()
    {
        if (transform.position.z == -100)
        {
            //DestroyImmediate(this.gameObject);
            DestroyImmediate(GetComponent<RoamOpal>());
            return;
        }
        if (decision <= 0)
        {
            //decision = Random.Range(10, 20);
            /**int makeChoice = Random.Range(0, 100);
            if (makeChoice == 0)
            {
                return;
            }
            else if (makeChoice % 2 == 0)
            {
                if (makeChoice / 2 % 2 == 0)
                {
                    upSpeed = 1;
                }
                else
                {
                    rightSpeed = 1;
                    sr.flipX = true;
                    if (myOpal.getMyName() == "Succuum" || myOpal.getMyName() == "Mechalodon")
                    {
                        sr.flipX = false;
                    }
                }
            }
            else
            {
                if (makeChoice / 2 % 2 == 0)
                {
                    upSpeed = -1;
                }
                else
                {
                    rightSpeed = -1;
                    sr.flipX = false;
                    if (myOpal.getMyName() == "Succuum" || myOpal.getMyName() == "Mechalodon")
                    {
                        sr.flipX = true;
                    }
                }
            }*/
        }
        if (rigidbody2D != null && !freeze)
        {
            if(transform.localPosition.x >= 0.4f && rightSpeed > 0)
            {
                rightSpeed = -1;
            }
            else if (transform.localPosition.x <= -0.4f && rightSpeed < 0)
            {
                rightSpeed = 1;
            }
            if (transform.localPosition.y > 0.4f && upSpeed > 0)
            {
                upSpeed = -1;
            }
            if (transform.localPosition.y <= 0.1f && upSpeed < 0)
            {
                upSpeed = 1;
            }
            rigidbody2D.velocity = new Vector2(rightSpeed * speedMod, upSpeed * speedMod);
        }
        decision--;
        lifetime--;
    }

    public void setSnagged(bool s)
    {
        snagged = s;
    }

    public IEnumerator shrinker()
    {
        float shrink = 1f;
        for (int i = 0; i < 20; i++)
        {
            transform.localScale = transform.localScale * shrink;
            shrink -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        DestroyImmediate(gameObject);
    }

    public IEnumerator fade()
    {
        SpriteRenderer sr = myOpal.GetComponent<SpriteRenderer>();
        sr.color = Color.black;
        for (int i = 0; i < 20; i++)
        {
            sr.color= new Color(i*0.05f,i*0.05f,i*0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
