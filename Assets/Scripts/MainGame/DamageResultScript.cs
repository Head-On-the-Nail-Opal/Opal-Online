using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageResultScript : MonoBehaviour {
    int i;
    public Text body;
    public Text body2;
    private bool vocal = false;
    private int maxLength = 50;
    private int voiceLength = 1;
    private string myVoice = "";
	// Use this for initialization

    public void setUp(int amount, OpalScript myBoy)
    {
        transform.position = myBoy.transform.position + new Vector3(0, 0.65f, 0);
        transform.rotation = myBoy.transform.rotation;
        transform.localScale /= 3;
        if (amount > 0)
        {
            body.text = "+" + amount;
            body.color = new Color(0, 1, 0);
        }else if(amount < 0)
        {
            body.text = "" + amount;
        }
        else
        {
            body.text = " ";
            body.color = new Color(0, 0, 0);
        }
    }

    public void setUp(int num, int denom, OpalScript myBoy)
    {
        transform.position = myBoy.transform.position + new Vector3(0,0.65f,0);
        transform.rotation = myBoy.transform.rotation;
        transform.localScale /= 3;
        body.text = num+"/"+denom;
        body.color = Color.green;
    }

    public void setUpVocal(string voice, OpalScript myBoy)
    {
        vocal = true;
        transform.position = myBoy.transform.position + new Vector3(0.85f, 0.85f, 0);
        transform.rotation = myBoy.transform.rotation;
        transform.localScale /= 2;
        body.text = voice.Substring(0, voiceLength);
        body2.text = voice.Substring(0,voiceLength);
        myVoice = voice;
        body.color = Color.black;
        body2.color = Color.white;
        maxLength = 50 + voice.Length;
    }

    private void followVocal()
    {
        if(voiceLength < myVoice.Length)
        {
            voiceLength++;
        }
        body.text = myVoice.Substring(0, voiceLength);
        body2.text = myVoice.Substring(0, voiceLength);
    }

    void Awake () {
        i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(i < maxLength)
        {
            i++;
            if (!vocal)
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
            else
            {
                transform.position = new Vector3(transform.position.x + 0.01f * Random.Range(-3, 4), transform.position.y + 0.01f * Random.Range(-3, 4), transform.position.z + 0.01f * Random.Range(-3, 4));
                followVocal();
            }
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
	}
}
