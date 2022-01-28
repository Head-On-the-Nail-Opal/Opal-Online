using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchGame : MonoBehaviour
{
    private OpalScript myOpal;
    private bool action = false;
    public Player p;
    public GameObject caughtOpals;
    //public GameObject grabtool;
    //private Rigidbody2D grabBody;
    //private ParticleSystem miniPortal;
    public Text combatLog;
    public GameObject flash;
    // Start is called before the first frame update
    void Start()
    {
        //grabBody = grabtool.GetComponent<Rigidbody2D>();
        //miniPortal = grabtool.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (action == true)
                doTalk();

        }else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (action == true)
                doFlee();
        }
    }


    public void Launch(string opal, Vector3 playerPos)
    {
        //StopAllCoroutines();
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z-4);
        flash.transform.position = new Vector3(0, 0, 0);
        int variantLength = 8;
        string opalN = opal.Substring(0, opal.Length - variantLength);
        string opalV = opal.Substring(opal.Length - variantLength, variantLength);
        combatLog.text = "You run across a " + opalN+"!";
        string firstLetter = opalN.Substring(0, 1);
        if(firstLetter == "A" || firstLetter == "E" || firstLetter == "I" || firstLetter == "O" || firstLetter == "U")
        {
            combatLog.text = "You run across an " + opalN + "!";
        }
        OpalScript opalPrefab = Resources.Load<OpalScript>("Prefabs/Opals/" + opalN);
        if (myOpal != null)
        {
            Destroy(myOpal.gameObject);
        }
        if (opal == null)
        {
            return;
        }
        myOpal = Instantiate<OpalScript>(opalPrefab, this.transform);
        myOpal.setOpal(null);
        //myOpal.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        myOpal.transform.localScale *= 0.1f;
        myOpal.setVariant(opalV);
        myOpal.GetComponent<Animator>().enabled = true;
        myOpal.tag = "Catch";
        myOpal.transform.localPosition = new Vector3(Random.Range(-0.3f,0.3f), 0.25f, -2);
        myOpal.transform.rotation = Quaternion.Euler(0,0,0);
        myOpal.gameObject.AddComponent<CatchGameOpal>();
        action = true;
    }

    public IEnumerator doFlash(string opal, Vector3 playerPos)
    {
        flash.transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z - 4);
        MeshRenderer mr = flash.GetComponent<MeshRenderer>();
        //flash.enabled = false;
        mr.material.color = new Color(1,1,1,0);
        for (int j = 0; j < 2; j++) {
            for (int i = 0; i < 10; i++)
            {
                mr.material.color = new Color(1, 1, 1, i * 0.08f);
                yield return new WaitForSeconds(0.001f);
            }
            for (int i = 10; i > 0; i--)
            {
                mr.material.color = new Color(1, 1, 1, i * 0.08f);
                yield return new WaitForSeconds(0.001f);
            }
        }
        mr.material.color = new Color(1, 1, 1, 0);
        Launch(opal, playerPos);
    }



    public void doTalk()
    {
        if (Random.Range(0, 1) == 0f)
        {
            transform.position = new Vector3(-1000, -1000, -1000);
            action = false;
            p.addOpal(myOpal.getVariant());
        }
        else
        {
            if(combatLog.text == myOpal.getMyName() + " does not seem convinced")
            {
                combatLog.text = myOpal.getMyName() + " remains resistant";
            }
            else
                combatLog.text = myOpal.getMyName() + " does not seem convinced";
        }
    }

    public void doItem()
    {

    }

    public void doFlee()
    {
        transform.position = new Vector3(-1000, -1000, -1000);
        action = false;
        p.addOpal(null);
        flash.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 0);
    }

    public void doCharms()
    {
        p.doCharms();
    }

    public void doPersonality()
    {
        p.doPersonality();
    }

    public void nextPage(bool next)
    {
        p.nextPage(next);
    }

    public void back()
    {
        p.back();
    }
}
