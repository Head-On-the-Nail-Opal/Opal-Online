using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWave : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator doTheWave()
    {
        int i = 0;
        int randY = 0;
        while (true)
        {
            int starty = -10;
            int endy = 12;
            if(transform.position.y >= -1)
            {
                endy = 0;
            }
            else if (transform.position.y <= -1.5)
            {
                starty = 0;
            }
            if (i % 3 == 0)
                randY = Random.Range(starty, endy);
            transform.position = new Vector3(transform.position.x, transform.position.y + randY * 0.0001f, transform.position.z);
            i++;
            if(i == 12)
            {
                i = 0;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}
