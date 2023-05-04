using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour {
    private string currentCollision = null;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    public Player pl;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }


    public string getCol()
    {
        return currentCollision;
    }


    public IEnumerator slash()
    {
        bc.enabled = true;
        sr.enabled = true;
        transform.localPosition = new Vector3(0, 0, 0);
        Quaternion startingQ = transform.rotation;
        for (int i = 0; i < 6; i++)
        {
            transform.rotation = startingQ;
            transform.localPosition += transform.up*-0.5f;
            yield return new WaitForSeconds(0.01f);
        }
        bc.enabled = false;
        for (int i = 0; i < 6; i++)
        {
            transform.rotation = startingQ;
            transform.localPosition += transform.up * 0.5f;
            yield return new WaitForSeconds(0.01f);
        }
        sr.enabled = false;
    }
}
