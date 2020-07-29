using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    public Text amountHolder;
    private int amount = 0;
    public Sprite burning;
    public Sprite poisoned;
    public Sprite lifted;
    public Sprite charge;
    public Sprite armor;
    public Transform tic;
    private List<Transform> tics = new List<Transform>();
    private string myType = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAmount(int a)
    {
        amount = a;
        if(amountHolder != null)
        {
            amountHolder.text = a+"";
        }
    }

    public void setTimer(int num)
    {
        setEnable(true);
        foreach (Transform t in tics)
        {
            DestroyImmediate(t.gameObject);
        }
        tics.Clear();
        if(num == 0)
        {
            setEnable(false);
        }
        if(myType == "burning" || myType == "poisoned" || myType == "lifted")
        {
            for(int i = 1; i < num; i++)
            {
                Transform temp = Instantiate<Transform>(tic,transform);
                temp.transform.localPosition = new Vector3(tic.localPosition.x+1.333333f*i, tic.localPosition.y, tic.localPosition.z);
                tics.Add(temp);
            }
        }
    }

    public int getAmount()
    {
        return amount;
    }

    public void setEnable(bool input)
    {
        GetComponent<SpriteRenderer>().enabled = input;
        if (!input)
        {
            amountHolder.text = "";
            tic.GetComponent<MeshRenderer>().enabled = false;
            foreach(Transform t in tics)
            {
                t.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            amountHolder.text = amount + "";
            tic.GetComponent<MeshRenderer>().enabled = true;
            foreach (Transform t in tics)
            {
                t.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void setType(string type)
    {
        if (type == "burning")
        {
            GetComponent<SpriteRenderer>().sprite = burning;
            Material temp = new Material(Shader.Find("Standard"));
            temp.color = Color.red;
            tic.GetComponent<MeshRenderer>().material = temp;
        }
        else if (type == "poisoned")
        {
            GetComponent<SpriteRenderer>().sprite = poisoned;
            Material temp = new Material(Shader.Find("Standard"));
            temp.color = new Color(1,0,1);
            tic.GetComponent<MeshRenderer>().material = temp;
            amountHolder.color = Color.white;
        }
        else if (type == "lifted")
        {
            GetComponent<SpriteRenderer>().sprite = lifted;
        }
        else if (type == "charge")
        {
            GetComponent<SpriteRenderer>().sprite = charge;
            tic.GetComponent<MeshRenderer>().enabled = false;
        }
        else if (type == "armor")
        {
            GetComponent<SpriteRenderer>().sprite = armor;
            amountHolder.color = Color.white;
            tic.GetComponent<MeshRenderer>().enabled = false;
        }
        myType = type;
    }
}
