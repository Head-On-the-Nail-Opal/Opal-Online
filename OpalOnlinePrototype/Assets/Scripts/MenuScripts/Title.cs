using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour {
    bool growing = true;
    int grow = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grow == 100)
        {
            growing = !growing;
            grow = 0;
        }
        if (growing)
        {
            transform.localScale = transform.localScale * 1.0003f;
        }
        else
        {
            transform.localScale = transform.localScale / 1.0003f;
        }
        grow++;
    }
}
