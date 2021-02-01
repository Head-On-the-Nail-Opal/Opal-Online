using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLabel : MonoBehaviour
{
    public Text itemText;
    private MainMenuScript m;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMain(MainMenuScript mm)
    {
        m = mm;
    }

    public void setText(string s)
    {
        itemText.text = s;
    }

    public void OnMouseEnter()
    {
        transform.localScale *= 1.1f;
        m.readDescription(itemText.text);
    }

    public void OnMouseExit()
    {
        transform.localScale /= 1.1f;
    }
}
