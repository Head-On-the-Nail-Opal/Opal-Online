using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireOpal : MonoBehaviour
{
    private Coroutine currently;
    private SpriteRenderer sr;


    private List<float> hop = new List<float> { 0.0f, 0.03f, 0.02f, 0.02f, 0.01f, 0.0f, -0.01f, -0.02f, -0.02f, -0.03f, 0.0f };
    private List<float> wiggle = new List<float> { 0.0f, 3f, 2f, 2f, 1f, 0.0f, -1f, -2f, -2f, -3f, 0.0f };

    private bool chosen = false;

    private CampfireMain cM;

    private bool pickMe = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currently == null && !chosen && !pickMe)
        {
            if(Random.Range(0,5) < 3)
            {
                currently = StartCoroutine(walkToSpot(transform.position.x + Random.Range(-1, 2), transform.position.y + Random.Range(-1,2)));
            }
            else
            {
                currently = StartCoroutine(stayStill());
            }
        }
        sr.sortingOrder = -Mathf.RoundToInt(transform.position.y*100);
    }

    private IEnumerator walkToSpot(float x, float y)
    {
        if (transform.position.x + x < 14 && transform.position.x + x > -16 && transform.position.y + y < 9 && transform.position.y + y > -8)
        {
            float xVel = 0;
            float yVel = 0;
            float speed = 0.01f;
            int i = 0;
            while (transform.position.x > x + 0.01f || transform.position.x < x - 0.01f || transform.position.y > y + 0.1f || transform.position.y < y - 0.01f)
            {
                xVel = 0;
                yVel = 0;

                if (transform.position.x > x)
                {
                    sr.flipX = false;
                    xVel = -1;
                }
                else if (transform.position.x < x)
                {
                    sr.flipX = true;
                    xVel = 1;
                }

                if (transform.position.y > y)
                {
                    yVel = -1;
                }
                else if (transform.position.y < y)
                {
                    yVel = 1;
                }

                transform.position = new Vector3(transform.position.x + xVel * speed, transform.position.y + yVel * speed + hop[i], transform.position.z);

                i++;
                if (i > hop.Count-1)
                    i = 0;

                yield return new WaitForFixedUpdate();
            }
        }
        currently = null;
    }

    public void setMain(CampfireMain camp)
    {
        cM = camp;
    }

    private IEnumerator stayStill()
    {
        int length = Random.Range(5, 15);
        for(int i = 0; i < length; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        currently = null;
    }

    private IEnumerator follow()
    {

        int i = 0;
        while (true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < 9 && mousePos.x > -9 && mousePos.y < 5 && mousePos.y > -4)
            {
                transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + wiggle[i] * 3);

            if (pickMe && (transform.position.y > -2.5f || transform.position.x > 7.25f))
            {
                cM.toggleHidePicks(true, GetComponent<OpalScript>());
            }
            else if (pickMe)
            {
                cM.toggleHidePicks(false, GetComponent<OpalScript>());
            }

            i++;
            if (i > wiggle.Count - 1)
                i = 0;

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator fall()
    {
        for(int i = 0; i < 10; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y -0.05f, transform.position.z);
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        if(transform.position.x > 7.25f)
        {
            if (!chosen)
            {
                transform.position = new Vector3(8, transform.position.y, transform.position.z);
                transform.localScale /= 1.5f;
                sr.color = new Color(1, 1, 1, 0.5f);
                sr.flipX = true;
                cM.getParty().Add(GetComponent<OpalScript>());
                cM.updatePartyCount();
            }
            chosen = true;
        }
        else
        {
            if (chosen)
            {
                transform.localScale *= 1.5f;
                sr.color = new Color(1, 1, 1, 1);
                cM.getParty().Remove(GetComponent<OpalScript>());
                cM.updatePartyCount();
            }
            chosen = false;
        }
        currently = null;
    }

    public void setPickMe(bool pm)
    {
        pickMe = pm;
    }

    private void OnMouseDown()
    {
        if(currently != null)
            StopCoroutine(currently);
        currently = StartCoroutine(follow());
        cM.displayOpalInfo(GetComponent<OpalScript>());

    }

    private void OnMouseUp()
    {
        if (currently != null)
            StopCoroutine(currently);
        currently = StartCoroutine(fall());
        cM.displayOpalInfo(null);
        if (pickMe && transform.position.y > -2.5f)
        {
            cM.pickOpal(GetComponent<OpalScript>());
        }
    }
}
