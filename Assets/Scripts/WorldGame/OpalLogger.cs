using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpalLogger : MonoBehaviour
{
    private List<RoamOpal> gameOpals = new List<RoamOpal>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addOpal(RoamOpal o)
    {
        gameOpals.Add(o);
    }

    public void removeOpal(RoamOpal o)
    {
        gameOpals.Remove(o);
    }

    public OpalScript checkTile(int x, int y)
    {
        foreach(RoamOpal o in gameOpals)
        {
            if(Mathf.RoundToInt(o.gameObject.transform.position.x) == x && Mathf.RoundToInt(o.gameObject.transform.position.y) == y)
            {
                return o.gameObject.GetComponent<OpalScript>();
            }
        }
        return null;
    }
}
