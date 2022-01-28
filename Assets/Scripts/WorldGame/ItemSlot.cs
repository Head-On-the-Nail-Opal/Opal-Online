using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{

    public Text itemLabel;
    private bool pressed = false;
    private MeshRenderer mR;
    private Player player;

    // Start is called before the first frame update
    void Awake()
    {
        mR = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlayer(Player p)
    {
        player = p;
    }

    public void setText(string t)
    {
        itemLabel.text = t;
    }

    public void OnMouseEnter()
    {
        if (!pressed)
        {
            pressed = true;
            mR.material.color /= 2;
            player.generateDescription(itemLabel.text);
        }
    }

    public void OnMouseDown()
    {
        player.charmClicked(itemLabel.text);
    }

    public void OnMouseExit()
    {
        if (pressed)
        {
            pressed = false;
            mR.material.color *= 2;
        }
    }
}
