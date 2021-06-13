using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpalTeam : MonoBehaviour
{
    private List<OpalScript> opals = new List<OpalScript>();
    private GameObject backgroundMat;
    private List<GameObject> backgrounds = new List<GameObject>();
    private bool hover = false;
    private MainMenuScript mms;
    private int teamNum = -1;
    private int downCounter = 0;
    private Vector3 startingPosition;
    private BoxCollider2D bC;
    // Start is called before the first frame update
    void Awake()
    {
        backgroundMat = Resources.Load<GameObject>("Prefabs/TeamBackground");
        bC = gameObject.AddComponent<BoxCollider2D>(); //TODO get this to expand with the background
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMain(MainMenuScript m, int tN)
    {
        mms = m;
        teamNum = tN;
    }

    public void displayTeam(List<OpalScript> input)
    {
        int i = 0;
        foreach(OpalScript o in input)
        {
            if(o.gameObject.activeSelf == false)
            {
                o.gameObject.SetActive(true);
            }
            OpalScript opalOne = Instantiate<OpalScript>(o);
            opalOne.setOpal(null);
            opalOne.setDetails(o);
            opalOne.transform.position = new Vector3(transform.position.x + (i * (2f / input.Count) - 3), transform.position.y, -2.5f + i*0.0001f);
            opalOne.transform.localScale *= 1f;
            opalOne.transform.rotation = Quaternion.Euler(0, 0, 0);
            opals.Add(opalOne);
            GameObject background = Instantiate<GameObject>(backgroundMat);
            background.transform.localPosition = new Vector3(transform.position.x + (i * (2f / input.Count) - 3), transform.position.y, -1.5f);
            background.transform.localScale *= 1.2f;
            backgrounds.Add(background);
            i++;
        }
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        int num = input.Count;
        if(num > 4)
        {
            num = 4;
        }
        bc.size = new Vector2(num*1.2f, 1.2f);
        bc.offset = new Vector2(-(3f*1.2f-num*1.2f)-(num*1.2f)/2, 0);
    }

    private void OnMouseUp()
    {
        if(mms.getWaiting() != -1)
        {
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<MeshRenderer>().material.color += new Color(10, 10, 10);
                }
            }
        }
        mms.displayTeam(opals, teamNum);
    }

    private void moveEverything(Vector3 target)
    {
        transform.position = new Vector3(transform.position.x, target.y, transform.position.z);
        foreach (OpalScript o in opals)
        {
            o.transform.position = new Vector3(o.transform.position.x, target.y, -2.5f);
        }
        foreach (GameObject m in backgrounds)
        {
            m.transform.position = new Vector3(m.transform.position.x, target.y, -1.5f);
        }
    }

    private void OnMouseDown()
    {
        startingPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        /**
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, mousePos.y, transform.position.z);
            foreach (OpalScript o in opals)
            {
            if(o != null)
                o.transform.position = new Vector3(o.transform.position.x, mousePos.y, -9f);
            }
            foreach (GameObject m in backgrounds)
            {
            if(m != null)
                m.transform.position = new Vector3(m.transform.position.x, mousePos.y, -2.7f);
            }*/
    }

    private void OnMouseOver()
    {
        if (!hover)
        {
            int i = 0;
            foreach(OpalScript o in opals)
            {
                if(o != null)
                {
                    o.transform.position = new Vector3(o.transform.position.x + i * 0.05f* opals.Count, o.transform.position.y, o.transform.position.z);
                    i++;
                }
            }
            i = 0;
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<MeshRenderer>().material.color += new Color(10, 10, 10);
                    b.transform.position = new Vector3(b.transform.position.x + i * 0.05f * opals.Count, b.transform.position.y, b.transform.position.z);
                    i++;
                }
                hover = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            mms.deleteTeam(this, teamNum);
        }
    }

    private void OnMouseExit()
    {
        if (hover)
        {
            int i = 0;
            foreach (OpalScript o in opals)
            {
                if (o != null)
                {
                    o.transform.position = new Vector3(o.transform.position.x - i * 0.05f * opals.Count, o.transform.position.y, o.transform.position.z);
                    i++;
                }
            }
            i = 0;
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<MeshRenderer>().material.color -= new Color(10, 10, 10);
                    b.transform.position = new Vector3(b.transform.position.x - i * 0.05f * opals.Count, b.transform.position.y, b.transform.position.z);
                    i++;
                }
                hover = false;
            }
        }
    }

    public void selfDestruct()
    {
        foreach(OpalScript o in opals)
        {
            DestroyImmediate(o.gameObject);
            //o.gameObject.SetActive(false);
        }
        foreach(GameObject g in backgrounds)
        {
            DestroyImmediate(g);
        }
    }
}
