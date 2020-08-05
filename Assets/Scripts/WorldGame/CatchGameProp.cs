using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchGameProp : MonoBehaviour
{
    private BoxCollider2D bc;
    private CatchGame parent;
    // Start is called before the first frame update
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        parent = GetComponentInParent<CatchGame>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Catch")
        {
            collision.transform.parent = this.transform;
            collision.gameObject.GetComponent<CatchGameOpal>().setSnagged(true);
            parent.StopAllCoroutines();
            parent.setSnagged(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.tag == "CatchWall")
        {
            parent.doCatch(false);
        }
    }
}
