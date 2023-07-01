using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pal : MonoBehaviour
{
    public Sprite sleep1;
    public Sprite sleep2;

    public Sprite cheer;

    public string palName;

    private GroundScript main;
    private Coroutine meander;
    private SpriteRenderer sr;
    private string team;

    private string condition;
    private string effect;

    public string getMyName()
    {
        return palName;
    }

    public void setMain(GroundScript m, string myTeam)
    {
        main = m;
        team = myTeam;
        transform.eulerAngles = new Vector3(40, -45, 0);
        transform.localScale *= 3;
        
        sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = 0;

        meander = StartCoroutine(wander());
    }

    public void flip(bool f)
    {
        GetComponent<SpriteRenderer>().flipX = f;
    }

    void Awake()
    {
        
    }

    void FixedUpdate()
    {
        if(main != null)
        {

        }
    }

    public void setTile(int x, int z)
    {
        transform.position = new Vector3(x, 0.25f, z);
    }

    private IEnumerator wander()
    {
        while (true)
        {
            float yMod = 0.25f;
            int rand = Random.Range(0, 10);
            OpalScript friend = adjacentToTeammate();
            OpalScript nonFriend = adjacentToEnemy();
            if (friend != null && Random.Range(0, 3) == 0)
            {

            }
            else if(nonFriend != null && Random.Range(0,5) == 0)
            {
                sr.flipX = false;
                if(transform.position.x < nonFriend.transform.position.x || transform.position.z < nonFriend.transform.position.z)
                {
                    sr.flipX = true;
                }
                GetComponent<Animator>().enabled = false;
                Sprite normal = sr.sprite;
                sr.sprite = cheer;
                StartCoroutine(nonFriend.playFrame("hurt", 5));
                for (int i = 0; i < 20; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
                GetComponent<Animator>().enabled = true;
                sr.sprite = normal;
                for (int i = 0; i < 5; i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            else if (rand < 6)
            {
                for (int i = 0; i < Random.Range(10, 20); i++)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            else
            {
                Vector3 targetTile = getAdjacentTile();
                if (targetTile.x == -100)
                {
                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    int i = 0;
                    while (transform.position.x != targetTile.x || transform.position.z != targetTile.z)
                    {
                        if(i < 5)
                        {
                            yMod += 0.05f;
                        }else if(i < 9)
                        {
                            yMod -= 0.05f;
                        }
                        else
                        {
                            i = 0;
                        }
                        i++;
                        float incr = 0.025f;
                        if (transform.position.x != targetTile.x)
                        {
                            sr.flipX = true;
                            if (transform.position.x > targetTile.x)
                            {
                                incr *= -1;
                                sr.flipX = false;
                            }
                            transform.position = new Vector3(transform.position.x + incr, yMod, transform.position.z);
                            if (Mathf.Abs(transform.position.x - targetTile.x) < 0.1f)
                            {
                                yMod = 0.25f;
                                transform.position = new Vector3(targetTile.x, yMod, targetTile.z);
                            }
                        }
                        else
                        {
                            sr.flipX = true;
                            if (transform.position.z > targetTile.z)
                            {
                                incr *= -1;
                                sr.flipX = false;
                            }
                            transform.position = new Vector3(transform.position.x, yMod, transform.position.z + incr);
                            if (Mathf.Abs(transform.position.z - targetTile.z) < 0.1f)
                            {
                                yMod = 0.25f;
                                transform.position = new Vector3(targetTile.x, yMod, targetTile.z);
                            }
                        }
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
        }
    }

    private IEnumerator sleep()
    {
        int i = 0;
        while (true) {
            GetComponent<Animator>().enabled = false;

            if (i % 2 == 0)
                sr.sprite = sleep1;
            else
                sr.sprite = sleep2;
            for(int j = 0; j < 30; j++)
            {
                yield return new WaitForEndOfFrame();
            }
            i++;
            if (i > 100)
                i = 0;
        }
    }

    public void setSleep()
    {
        StopCoroutine(meander);
        condition = "-1";
        StartCoroutine(sleep());
    }

    public void doDance(TileScript t)
    {
        doDance(new Vector3(t.getPos().x, t.getPos().y+1.5f, t.getPos().z));
    }

    public void doDance(OpalScript o)
    {
        doDance(new Vector3(o.getPos().x, o.getPos().y+0.5f, o.getPos().z));
    }

    public void doDance(Vector3 pos)
    {
        Pal newPal = Instantiate<Pal>(this);
        newPal.transform.position = pos;
        newPal.GetComponent<SpriteRenderer>().sortingOrder = 1;
        StartCoroutine(newPal.dance());
    }

    private IEnumerator dance()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        GetComponent<Animator>().enabled = false;
        Sprite normal = sr.sprite;
        sr.sprite = cheer;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                transform.localScale *= 1.01f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 0.5f);

                if(j % 15 == 0)
                {
                    if (sr.sprite == normal)
                        sr.sprite = cheer;
                    else
                        sr.sprite = normal;
                }
                yield return new WaitForEndOfFrame();
            }
            for (int j = 0; j < 30; j++)
            {
                transform.localScale /= 1.01f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 0.5f);
                if (j % 15 == 0)
                {
                    if (sr.sprite == normal)
                        sr.sprite = cheer;
                    else
                        sr.sprite = normal;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        Destroy(this.gameObject);
    }

    public void setDetails(string c, string e)
    {
        condition = c;
        effect = e;
    }

    public string getCondition()
    {
        return condition;
    }

    public string getEffect()
    {
        return effect;
    }

    private Vector3 getAdjacentTile()
    {
        List<TileScript> availableTiles = new List<TileScript>();
        if(main.tileGrid[(int)transform.position.x, (int)transform.position.z] == null)
                return new Vector2(-100, -100);
        foreach(TileScript t in main.getSurroundingTiles(main.tileGrid[(int)transform.position.x, (int)transform.position.z], true)){
            if(t.getCurrentOpal() == null && !t.getFallen())
            {
                availableTiles.Add(t);
            }
        }
        return availableTiles[Random.Range(0, availableTiles.Count)].getPos();
    }

    private OpalScript adjacentToTeammate()
    {
        List<OpalScript> teammates = new List<OpalScript>();
        if (main.tileGrid[(int)transform.position.x, (int)transform.position.z] == null)
            return null;
        foreach (TileScript t in main.getSurroundingTiles(main.tileGrid[(int)transform.position.x, (int)transform.position.z], true))
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() == team)
            {
                teammates.Add(t.getCurrentOpal());
            }
        }
        if (teammates.Count == 0)
            return null;
        return teammates[Random.Range(0, teammates.Count)];
    }

    private OpalScript adjacentToEnemy()
    {
        List<OpalScript> nonteammates = new List<OpalScript>();
        if (main.tileGrid[(int)transform.position.x, (int)transform.position.z] == null)
            return null;
        foreach (TileScript t in main.getSurroundingTiles(main.tileGrid[(int)transform.position.x, (int)transform.position.z], true))
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != team)
            {
                nonteammates.Add(t.getCurrentOpal());
            }
        }
        if (nonteammates.Count == 0)
            return null;
        return nonteammates[Random.Range(0, nonteammates.Count)];
    }
}
