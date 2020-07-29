using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchGame : MonoBehaviour
{
    private OpalScript myOpal;
    private bool action = false;
    public Player p;
    public GameObject grabtool;
    private Rigidbody2D grabBody;
    private ParticleSystem miniPortal;
    private int fireReady = 0;
    private bool fired = false;
    private bool snagged = false;
    // Start is called before the first frame update
    void Start()
    {
        grabBody = grabtool.GetComponent<Rigidbody2D>();
        miniPortal = grabtool.GetComponentInChildren<ParticleSystem>();
        miniPortal.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (action)
        {
            if (snagged)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    grabtool.transform.localPosition -= grabtool.transform.up * 0.08f;

                }
                if (grabtool.transform.localPosition.y <= -0.5f)
                {
                    doCatch(true);
                    miniPortal.gameObject.SetActive(false);
                }
                return;
            }
            int right = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (fireReady > 20)
                {
                    StartCoroutine(fire());
                }

            }
            if (Input.GetKey(KeyCode.D))
            {
                if (grabBody.rotation > -80)
                {
                    right = +1;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (grabBody.rotation < 80)
                {
                    right = -1;
                }
            }
            grabBody.MoveRotation(grabBody.rotation + -5 * right);
            if(fireReady <= 20)
            {
                fireReady++;
            }
            else
            {
                miniPortal.gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator fire()
    {
        fired = true;
        grabBody.freezeRotation = true;
        for (int i = 0; i < 100; i++)
        {
            grabtool.transform.localPosition += grabtool.transform.up * 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
        grabtool.transform.localPosition = new Vector3(0, -0.5f, -2);
        grabBody.freezeRotation = false;
        doCatch(false);
    }

    public void Launch(string opal, Vector3 playerPos)
    {
        fireReady = 0;
        fired = false;
        snagged = false;
        StopAllCoroutines();
        grabBody.freezeRotation = false;
        transform.position = new Vector3(playerPos.x, playerPos.y, playerPos.z-4);
        grabtool.transform.localPosition = new Vector3(0, -0.5f, -2);
        grabtool.transform.localRotation = Quaternion.Euler(0,0,0);
        int variantLength = 8;
        string opalN = opal.Substring(0, opal.Length - variantLength);
        string opalV = opal.Substring(opal.Length - variantLength, variantLength);

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
        myOpal.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        myOpal.setVariant(opalV);
        myOpal.GetComponent<Animator>().enabled = true;
        myOpal.tag = "Catch";
        myOpal.transform.localPosition = new Vector3(Random.Range(-0.3f,0.3f), 0.25f, -2);
        myOpal.transform.rotation = Quaternion.Euler(0,0,0);
        myOpal.gameObject.AddComponent<CatchGameOpal>();
        action = true;
        fireReady = 0;

    }

    public void doCatch(bool caught)
    {
        if (!fired)
        {
            return;
        }
        transform.position = new Vector3(-1000, -1000, -1000);
        fireReady = 0;
        action = false;
        if (caught)
            p.addOpal(myOpal.getVariant());
        else
        {
            miniPortal.gameObject.SetActive(false);
            p.addOpal(null);
        }
    }

    public void setSnagged(bool s)
    {
        snagged = s;
    }
}
