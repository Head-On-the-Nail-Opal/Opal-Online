using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelOpalButton : MonoBehaviour
{
    public Vector3 target;
    public string purpose;
    public Sprite unpressedS;
    public Sprite pressedS;
    private GameObject mainCam;
    private MainMenuScript main;
    private SpriteRenderer sR;
    private bool hovering = false;
    private OpalScript myOpal;
    private TeamDisplay team;


    void Start()
    {
        mainCam = GameObject.Find("Main Camera");
        main = GameObject.Find("MainMenuController").GetComponent<MainMenuScript>();
        sR = GetComponent<SpriteRenderer>();
        team = GetComponentInParent<TeamDisplay>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setOpal(OpalScript o)
    {
        myOpal = o;
    }

    private void OnMouseDown()
    {
        main.clearSingleOpal(myOpal);
        team.clearOpal(myOpal);
        DestroyImmediate(this.gameObject);
    }

    private void OnMouseUp()
    {

    }

    private void OnMouseOver()
    {
        //rend.material = hover;
        if (pressedS != null)
        {
            if (!hovering)
            {
                hovering = true;
                sR.sprite = pressedS;
                transform.localScale *= 1.5f;
            }
            
        }
        else
        {
            sR.color = new Color(1f, 0.2f, 1f);
        }
    }

    private void OnMouseExit()
    {
        //rend.material = unpressed;
        if (unpressedS != null)
        {
            if (hovering)
            {
                sR.sprite = unpressedS;
                hovering = false;
                transform.localScale /= 1.5f;
            }
        }
        else
        {
            sR.color = new Color(1f, 1f, 1f);
        }
    }
}
