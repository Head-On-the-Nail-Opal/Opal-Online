using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamOpal : MonoBehaviour {

    SpriteRenderer sr;
    OpalScript myOpal;
    private float speedMod = 1f;
    private int decision = 20;
    private int upSpeed = 0;
    private int rightSpeed = 0;
    private int lifetime;
    private bool freeze = false;
    private TileCode[,] map;
    private Dictionary<string, bool> getPassable;
    private bool moving = false;
    private OpalLogger oL;

    // Use this for initialization
    void Start () {

        myOpal = GetComponent<OpalScript>();
        sr = GetComponent<SpriteRenderer>();
        lifetime = Random.Range(200, 1000);
        transform.localScale = transform.localScale * 0.05f;
        StartCoroutine(grower());
        tag = "Opal";
        string skin = "00";
        string particle = "00";
        string color = "00";
        string size = "00";

        //skin
        int rand = Random.Range(1, 1000);
        //if (rand < 100) { skin = "01"; }
        //if (rand < 10) { skin = "02"; }

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
        //print(currentGridPos.x + "," + currentGridPos.y);
    }
	
	// Update is called once per frame
	void Update () {
        if(transform.position.z == -100)
        {
            DestroyImmediate(GetComponent<RoamOpal>());
            return;
        }
        if(lifetime <= 0)
        {
            StartCoroutine(shrinker());
            return;
        }
        if (decision <= 0 && !moving)
        {
            
            int makeChoice = Random.Range(0, 5);
            if (!freeze)
            {
                if (makeChoice == 0)
                {
                    if (getPassable[map[Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y)].getCode()] && oL.checkTile(Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y)) == null)
                    {
                        //print(map[Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y)].getCode());
                        StartCoroutine(move(1, 0));
                    }
                }
                else if (makeChoice == 1)
                {
                    if (getPassable[map[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y+1)].getCode()] && oL.checkTile(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y+1)) == null)
                    {
                        //print(map[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y+1)].getCode());
                        StartCoroutine(move(0, 1));
                    }
                }
                else if (makeChoice == 2)
                {
                    if (getPassable[map[Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y)].getCode()] && oL.checkTile(Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y)) == null)
                    {
                        //print(map[Mathf.RoundToInt(transform.position.x - 1), Mathf.RoundToInt(transform.position.y)].getCode());
                        StartCoroutine(move(-1, 0));
                    }
                }
                else if (makeChoice == 3)
                {
                    if (getPassable[map[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1)].getCode()] && oL.checkTile(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y-1)) == null)
                    {
                        //print(map[Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y - 1)].getCode());
                        StartCoroutine(move(0, -1));
                    }
                }
            }
        }
        decision--;
        lifetime--;
    }

    public IEnumerator move(int x, int y)
    {
        moving = true;
        float steps = 5;
        if (x == 1)
        {
            sr.flipX = true;
            if (myOpal.getMyName() == "Succuum" || myOpal.getMyName() == "Mechalodon")
            {
                sr.flipX = false;
            }
        }
        else if (x == -1)
        {
            sr.flipX = false;
            if (myOpal.getMyName() == "Succuum" || myOpal.getMyName() == "Mechalodon")
            {
                sr.flipX = true;
            }
        }
        for (int i = 0; i < steps; i++)
        {
            if(x != 0)
            {
                transform.position = new Vector2((x * speedMod) / steps + transform.position.x, transform.position.y);
            }
            else if (y != 0)
            {
                transform.position = new Vector2(transform.position.x, (y * speedMod) / steps + transform.position.y);
            }
            else
            {

            }
            yield return new WaitForSeconds(0.0001f);
        }
        //print(currentGridPos.x + "," + currentGridPos.y);
        //print(map[Mathf.RoundToInt(transform.position.x + 1), Mathf.RoundToInt(transform.position.y)].getCode());
        decision = Random.Range(20, 60);
        OpalScript overlap = oL.checkTile(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (overlap != null && overlap != this.gameObject.GetComponent<OpalScript>())
            decision = 0;
        moving = false;
    }

        public void setMap(TileCode[,] cS, Dictionary<string, bool> b)
    {
        map = cS;
        getPassable = b;
    }

    public IEnumerator shrinker()
    {
        oL.removeOpal(this);
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
        float shrink = 0.05f;
        for (int i = 0; i < 11; i++)
        {
            transform.localScale = transform.localScale * (1+shrink);
            shrink += 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        oL.addOpal(this);
    }

    public void freezeMe()
    {
        freeze = true;
    }

    public void addOpalLogger(OpalLogger o)
    {
        oL = o;
    }
}
