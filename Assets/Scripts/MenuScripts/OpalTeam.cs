using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpalTeam : MonoBehaviour
{
    private List<OpalScript> opals = new List<OpalScript>();
    private Pal myPal = null;
    private GameObject backgroundMat;
    private GameObject typePlate;
    private List<GameObject> backgrounds = new List<GameObject>();
    private List<GameObject> types = new List<GameObject>();
    private bool hover = false;
    private MainMenuScript mms;
    private int teamNum = -1;
    private int downCounter = 0;
    private Vector3 startingPosition;
    private BoxCollider2D bC;
    private GameObject palPlate;
    private GameObject actualPalPlate;
    public Text dangerText;
    private bool expanded = false;
    // Start is called before the first frame update
    void Awake()
    {
        backgroundMat = Resources.Load<GameObject>("Prefabs/OpalPlate2");
        typePlate = Resources.Load<GameObject>("Prefabs/DualTypePlate");
        palPlate = Resources.Load<GameObject>("Prefabs/PalPlate");
        bC = gameObject.AddComponent<BoxCollider2D>();
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
            if (i > 0)
                opalOne.gameObject.SetActive(false);
            opalOne.transform.position = new Vector3(transform.position.x + (0 * (2f / input.Count) - 3), transform.position.y, -2.5f + i*0.0001f);
            opalOne.transform.localScale *= 1f;
            opalOne.transform.rotation = Quaternion.Euler(0, 0, 0);
            opals.Add(opalOne);
            GameObject background = Instantiate<GameObject>(backgroundMat);
            background.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1);
            background.transform.localPosition = new Vector3(transform.position.x + (0 * (2f / input.Count) - 3), transform.position.y, -1.5f);
            background.transform.localScale *= 3.6f;
            backgrounds.Add(background);
            i++;
        }
        BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
        int num = input.Count;
        if(num > 4)
        {
            num = 4;
        }
        bc.size = new Vector2(1f, 1f);
        //bc.offset = new Vector2(-(3f*1.2f-num*1.2f)-(num*1.2f)/2, 0);
        bc.offset = new Vector2(-3, 0); 
        displayTeamTypes(true);
        displayPal(true);
        setLimitations();
    }

    private void displayTeamTypes(bool show)
    {
        if (types.Count > 0)
        {
            foreach (GameObject tp in types)
            {
                Destroy(tp.gameObject);
            }
            types.Clear();
        }
        if (show)
        {
            int i = 0;
            float x = 0.425f;
            float y = 0.20f;
            foreach(OpalScript o in opals)
            {
                if (i % 2 == 0)
                {
                    x += 0.34f;
                    y = 0.20f;
                }
                y += 0.325f;
                GameObject temp = Instantiate<GameObject>(typePlate, transform);
                temp.transform.position = new Vector3(o.transform.position.x + x, o.transform.position.y - y + 0.925f, -1);
                
                temp.transform.localScale *= 1f;
                //temp.GetComponent<SpriteRenderer>().color = o.getColorFromType(o.getMainType(), true);
                bool first = true;
                foreach(SpriteRenderer sr in temp.GetComponentsInChildren<SpriteRenderer>())
                {
                    if(first)
                        sr.color = o.getColorFromType(o.getSecondType(), first);
                    else
                        sr.color = o.getColorFromType(o.getMainType(), first);
                    first = false;
                }
                types.Add(temp);
                i++;
            }
        }
    }

    public void setPal(Pal p)
    {
        if (myPal != null)
            Destroy(myPal.gameObject);
        if (p == null)
        {
            myPal = null;
        }
        else
        {
            myPal = Instantiate<Pal>(p, actualPalPlate.transform);
            myPal.transform.localPosition = new Vector3(0, 0, 0);
            myPal.GetComponent<SpriteRenderer>().sortingOrder = 1;
            myPal.gameObject.SetActive(true);
        }
    }

    private void displayPal(bool show)
    {
        if(actualPalPlate == null)
        {
            actualPalPlate = Instantiate<GameObject>(palPlate, transform);
            actualPalPlate.transform.localPosition = new Vector3(((opals.Count+1)/2)*0.325f-1.95f,0.25f,-1);
            actualPalPlate.transform.localScale *= 4f;
            actualPalPlate.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1);
        }
        actualPalPlate.SetActive(show);
    }

    private void movePalPlate(bool start)
    {
        if (start)
        {
            actualPalPlate.transform.localPosition = new Vector3(((opals.Count + 1) / 2) * 0.325f - 1.95f, 0.25f, -1);
        }
        else
        {
            actualPalPlate.transform.localPosition = new Vector3(((opals.Count - 1)) * 1.1f - 1.95f, 0.25f, -1);
        }

    }

    private void destroyPal()
    {
        myPal = null;
        if(actualPalPlate != null)
            Destroy(actualPalPlate.gameObject);
    }

    private void OnMouseUp()
    {
        if(mms.getWaiting() != -1)
        {
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 1);
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

    private void expand(bool over)
    {
        expanded = over;
        if (over)
        {
            displayTeamTypes(false);
            displayPal(true);
            movePalPlate(false);

            int i = 0;
            foreach (OpalScript o in opals)
            {
                if (o != null)
                {
                    o.transform.position = new Vector3(o.transform.position.x + i * 0.275f * 4, o.transform.position.y, o.transform.position.z);
                    if (i > 0)
                        o.gameObject.SetActive(true);
                    i++;
                }
            }
            i = 0;
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 1);
                    b.transform.position = new Vector3(b.transform.position.x + i * 0.275f * 4, b.transform.position.y, b.transform.position.z);
                    i++;
                }
            }
        }
        else
        {
            
            int i = 0;
            foreach (OpalScript o in opals)
            {
                if (o != null)
                {
                    o.transform.position = new Vector3(o.transform.position.x - i * 0.275f * 4, o.transform.position.y, o.transform.position.z);
                    if (i > 0)
                        o.gameObject.SetActive(false);
                    i++;
                }
            }
            i = 0;
            foreach (GameObject b in backgrounds)
            {
                if (b != null)
                {
                    b.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.2f, 1);
                    b.transform.position = new Vector3(b.transform.position.x - i * 0.275f * 4, b.transform.position.y, b.transform.position.z);
                    i++;
                }
            }
            displayTeamTypes(true);
            displayPal(true);
            movePalPlate(true);
        }
        setLimitations();
    }

    private void OnMouseOver()
    {
        if (!hover)
        {
            expand(true);
            hover = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            displayTeamTypes(false);
            movePalPlate(false);
            mms.deleteTeam(this, teamNum);
            //mms.loadTeams();
        }
    }

    private void OnMouseExit()
    {
        if (hover)
        {
            expand(false);
            hover = false;
        }
    }

    public void selfDestruct()
    {
        displayTeamTypes(false);
        destroyPal();
        foreach(OpalScript o in opals)
        {
            Destroy(o.gameObject);
        }
        opals.Clear();
        foreach(GameObject g in backgrounds)
        {
            Destroy(g.gameObject);
        }
        backgrounds.Clear();
        Destroy(dangerText);
        Destroy(gameObject);
    }

    private void setLimitations()
    {
        List<string> types = new List<string>();
        if(mms != null)
            types = mms.calculateTypeOverload(opals);
        if(types.Count > 0)
        {
            int i = 0;
            foreach(OpalScript o in opals)
            {
                if(types.Contains(o.getMainType()) || types.Contains(o.getSecondType()) || (!expanded && i == backgrounds.Count-1)){
                    backgrounds[i].GetComponent<SpriteRenderer>().color = Color.red;
                }
                i++;
            }
            dangerText.text = "DANGER: TYPE OVERLOAD\nUNSTABLE OPALS LOSE FIRST TURN";
        }
        else
        {
            dangerText.text = "";
        }
    }
}
