using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTracker : MonoBehaviour
{
    public List<GameObject> NPCs = new List<GameObject>();

    public string getNPC(int x, int y)
    {
        foreach (GameObject npc in NPCs)
        {
            if(npc.transform.position.x == x && npc.transform.position.y == y)
            {
                return npc.name;
            }
        }
        return "";
    }
}
