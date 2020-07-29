using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamOpal : MonoBehaviour {
    Rigidbody2D rigidbody2D;
    BoxCollider2D bc;
    SpriteRenderer sr;
    OpalScript myOpal;
    private float speedMod = 8;
    private int decision = 0;
    private int upSpeed = 0;
    private int rightSpeed = 0;
    private int lifetime;
    private bool freeze = false;
    // Use this for initialization
    void Start () {
        if (GetComponent<Rigidbody2D>() == null)
            rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        else
            rigidbody2D = GetComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        myOpal = GetComponent<OpalScript>();
        sr = GetComponent<SpriteRenderer>();
        bc.size = new Vector2(4, 4);
        if (rigidbody2D != null)
        {
            rigidbody2D.gravityScale = 0;
            rigidbody2D.mass = 0.0000001f;
            rigidbody2D.freezeRotation = true;
        }
        lifetime = Random.Range(200, 1000);
        transform.localScale = transform.localScale * 0.05f;
        StartCoroutine(grower());
        tag = "Opal";
        bc.isTrigger = false;
        string skin = "00";
        string particle = "00";
        string color = "00";
        string size = "00";

        //skin
        int rand = Random.Range(1, 1000);
        if (rand < 100) { skin = "01"; }
        if (rand < 10) { skin = "02"; }

        //particle
        rand = Random.Range(1, 1000);
        if (rand < 100) { particle = "05"; }
        if (rand < 70) { particle = "02"; }
        if (rand < 40) { particle = "03"; }
        if (rand < 30) { particle = "01"; }
        if (rand < 20) { particle = "04"; }
        if (rand < 15) { particle = "06"; }
        if (rand < 10) { particle = "07"; }
        if (rand < 5) { particle = "05"; }

        //color
        rand = Random.Range(1, 100);
        if (rand < 91) { color = "01"; }
        if (rand < 82) { color = "02"; }
        if (rand < 73) { color = "03"; }
        if (rand < 64) { color = "04"; }
        if (rand < 55) { color = "05"; }
        if (rand < 46) { color = "06"; }
        if (rand < 37) { color = "07"; }
        if (rand < 28) { color = "08"; }
        if (rand < 19) { color = "09"; }
        if (rand < 10) { color = "11"; }
        if (rand < 4) { color = "10"; }

        //size
        rand = Random.Range(1, 100);
        if (rand < 60) { size = "01"; }
        if (rand < 50) { size = "02"; }
        if (rand < 30) { size = "03"; }
        if (rand < 10) { size = "04"; }

        //print("Skin: " + skin + " Particle: " + particle + " Color: " + color);
        string variant = size + color + particle + skin;
        myOpal.setVariant(""+variant);

    }
	
	// Update is called once per frame
	void Update () {
        if(transform.position.z == -100)
        {
            //DestroyImmediate(this.gameObject);
            DestroyImmediate(GetComponent<RoamOpal>());
            return;
        }
        if(lifetime <= 0)
        {
            StartCoroutine(shrinker());
            return;
        }
        if (decision <= 0)
        {
            decision = Random.Range(10, 20);
            int makeChoice = Random.Range(0, 100);
            if(makeChoice == 0)
            {
                return;
            }
            else if(makeChoice % 2 == 0 )
            {
                if(makeChoice/2 % 2 == 0)
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
                    if(myOpal.getMyName()  == "Succuum" || myOpal.getMyName() == "Mechalodon")
                    {
                        sr.flipX = true;
                    }
                }
            }
        }
        if (rigidbody2D != null && !freeze)
        {
            rigidbody2D.velocity = new Vector2(rightSpeed * speedMod, upSpeed * speedMod);
        }
        decision--;
        lifetime--;
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

    public IEnumerator grower()
    {
        Vector2 size = bc.size;
        bc.size *= 0;
        float shrink = 0.05f;
        for (int i = 0; i < 11; i++)
        {
            transform.localScale = transform.localScale * (1+shrink);
            shrink += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        bc.size = size;
    }

    public void freezeMe()
    {
        freeze = true;
    }
}
