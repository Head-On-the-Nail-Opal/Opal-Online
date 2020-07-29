using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nature : MonoBehaviour {
    public Sprite trampled;
    public GameObject child;
    public List<Sprite> breakFrames = new List<Sprite>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setTrampled()
    {
        DestroyImmediate(GetComponent<Animator>());
        
        //transform.rotation.SetEulerAngles(90,0,0);
        if(child != null)
        {
            child.GetComponent<Nature>().setTrampled();
        }

        StartCoroutine(getTrampled());
        //print("ouch!");
    }

    public IEnumerator getTrampled()
    {
        foreach(Sprite s in breakFrames)
        {
            GetComponent<SpriteRenderer>().sprite = s;
            yield return new WaitForSeconds(0.05f);
        }
        GetComponent<SpriteRenderer>().sprite = trampled;
    }
}
