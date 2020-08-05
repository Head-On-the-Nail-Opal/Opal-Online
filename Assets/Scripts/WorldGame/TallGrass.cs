using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TallGrass : MonoBehaviour {
    private int rand = 0;
    public List<string> opalname = new List<string>();
    public List<int> percent = new List<int>();
    private List<OpalScript> opalPrefabs = new List<OpalScript>();
    private List<OpalScript> currentOpals = new List<OpalScript>();
    public bool isWater;

	// Use this for initialization
	void Start () {
        rand = Random.Range(10, 100);
        foreach(string s in opalname)
        {
            opalPrefabs.Add(Resources.Load<OpalScript>("Prefabs/Opals/"+s));
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rand--;
            //print("duh hello");
            if (rand < 1)
            {
                Spawn();
                rand = Random.Range(50, 150);
            }
        }
    }

    private void Spawn()
    {
        int temp = Random.Range(0, 100);
        int store = 0;
        int i = 0;
        foreach(int num in percent)
        {
            store += num;
            if(temp <= store)
            {
                //print("Spawning a " + opalname[i]);
                OpalScript opal = Instantiate<OpalScript>(opalPrefabs[i], this.transform);
                opal.setOpal(null);
                opal.transform.localPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), -1);
                opal.transform.rotation = Quaternion.Euler(0, 0, 0);
                opal.transform.localScale *= 5;
                //Destroy(opal.GetComponent<Rigidbody2D>());
                opal.gameObject.AddComponent<RoamOpal>();
                currentOpals.Add(opal);
                break;
            }
            i++;
        }
    }
}
