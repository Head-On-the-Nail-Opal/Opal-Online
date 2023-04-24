using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LilOpalBox : MonoBehaviour
{
    private OpalScript currentOpal;
    private int i = 0;
    private int threshold = 50;

    void Awake()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentOpal == null)
            return;
        if(i >= threshold)
        {
            i = 0;
            if(Random.Range(0,5) == 4)
            {
                if(Random.Range(0,2) == 0)
                {
                    currentOpal.flipOpal(true);
                }
                else
                {
                    currentOpal.flipOpal(false);
                }
            }
            
        }
        i++;
    }

    public void setOpal(OpalScript o)
    {
        clearOpal();
        if(o == null)
        {
            return;
        }
        currentOpal = Instantiate<OpalScript>(o,transform);
        currentOpal.setOpal(null);
        currentOpal.transform.position = new Vector3(transform.position.x, transform.position.y, -2.5f);
        currentOpal.transform.localScale *= 0.25f;
        currentOpal.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (Random.Range(0, 2) == 0)
            currentOpal.flipOpal(true);
    }

    private void clearOpal()
    {
        if(currentOpal != null)
        {
            Destroy(currentOpal.gameObject);
            currentOpal = null;
        }
    }
}
