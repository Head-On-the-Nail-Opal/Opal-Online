using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour {
    private List<OneWave> waves = new List<OneWave>();
    public OneWave wavePrefab;
    //int a;
	// Use this for initialization
	void Start () {
        //a = 0;
		for(int i = 0; i < 34; i++)
        {
            for (int j = 0; j < 34; j++)
            {
                OneWave temp = Instantiate<OneWave>(wavePrefab);
                if(-15+j != 14 || -10 + i != 8) 
                    temp.transform.position = new Vector3(-15+j, -1f, -10 + i);
                else
                    temp.transform.position = new Vector3(-15 + j, -100f, -10 + i);
                waves.Add(temp);
                //StartCoroutine(temp.doTheWave());
            }
        }
	}
}
