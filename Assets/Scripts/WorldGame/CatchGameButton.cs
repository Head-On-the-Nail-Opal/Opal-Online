using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchGameButton : MonoBehaviour
{
    public string purpose;
    private bool pressed = false;
    private MeshRenderer mR;
    public CatchGame mainGame;
    public Sprite unpressedSprite;
    public Sprite pressedSprite;
    // Start is called before the first frame update
    void Start()
    {
        if(pressedSprite == null)
            mR = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        if (!pressed)
        {
            if(pressedSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = pressedSprite;
                pressed = true;
                return;
            }
            pressed = true;
            mR.material.color *= 2;
        }
    }

    private void OnMouseDown()
    {
        if(purpose == "Talk")
        {
            mainGame.doTalk();
        }else if(purpose == "Item")
        {
            mainGame.doItem();
        }else if(purpose == "Flee")
        {
            mainGame.doFlee();
        }else if(purpose == "Charms")
        {
            mainGame.doCharms();
        }else if(purpose == "Personality")
        {
            mainGame.doPersonality();
        }else if(purpose == "Left")
        {
            mainGame.nextPage(false);
        }else if(purpose == "Right")
        {
            mainGame.nextPage(true);
        }else if(purpose == "back")
        {
            mainGame.back();
        }

    }

    private void OnMouseExit()
    {
        if (pressed)
        {
            if (pressedSprite != null)
            {
                GetComponent<SpriteRenderer>().sprite = unpressedSprite;
                pressed = false;
                return;
            }
            pressed = false;
            mR.material.color /= 2;
        }
    }
}
