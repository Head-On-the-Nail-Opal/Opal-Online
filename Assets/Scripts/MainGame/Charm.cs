using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : MonoBehaviour
{
    string myName = "None";

    public void setName(string n)
    {
        myName = n;
    }

    public string getName()
    {
        return myName;
    }
}
