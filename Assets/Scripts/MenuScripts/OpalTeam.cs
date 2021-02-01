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
    // Start is called before the first frame update
    void Awake()
    {
        backgroundMat = Resources.Load<GameObject>("Prefabs/TeamBackground");
        gameObject.AddComponent<BoxCollider2D>(); //TODO get this to expand with the background
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
            opalOne.setPersonality(o.getPersonality());
            opalOne.transform.position = new Vector3(transform.position.x + i*1.2f - 3, transform.position.y, -2.5f);
            opalOne.transform.localScale *= 1f;
            opalOne.transform.rotation = Quaternion.Euler(0, 0, 0);
            opals.Add(opalOne);
            GameObject background = Instantiate<GameObject>(backgroundMat);
            background.transform.localPosition = new Vector3(transform.position.x + i * 1.2f - 3, transform.position.y, -1.5f);
            background.transform.localScale *= 1.2f;
            backgrounds.Add(background);
            i++;
        }
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        bc.size = new Vector2(input.Count*1.2f, 1.2f);
        bc.offset = new Vector2(-(3f*1.2f-input.Count*1.2f)-(input.Count*1.2f)/2, 0);
    }

    private void OnMouseUp()
    {
        mms.displayTeam(opals, teamNum);
    }

    private void OnMouseOver()
    {
        if (!hover)
        {
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<MeshRenderer>().material.color += new Color(10, 10, 10);
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
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<MeshRenderer>().material.color -= new Color(10, 10, 10);
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
