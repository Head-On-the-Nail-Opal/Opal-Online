using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffMarker : MonoBehaviour
{
    public Text buffAmount;
    public Text buffLength;

    private int internalLength= 0;
    private int internalAmount=0;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    public void adjustAmount(string input)
    {
        internalAmount = int.Parse(input);
        if(internalAmount > -1)
            buffAmount.text = "+" + input;
        else
            buffAmount.text = "-" + input;
    }

    public void adjustLength(string input)
    {
        if (input == "0")
            buffLength.text = "-";
        else
            buffLength.text = input;
        internalLength = int.Parse(input);
    }

    public int getLength()
    {
        return internalLength;
    }

    public void hide(bool hideMe, bool justText)
    {
        if (hideMe)
        {
            if (justText)
            {
                buffAmount.text = "";
                buffLength.text = "";
            }
            else
                GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            if (justText)
            {
                adjustAmount(internalAmount + "");
                adjustLength(internalLength + "");
            }
            else
                GetComponent<SpriteRenderer>().enabled = true;
        }
    }

}
