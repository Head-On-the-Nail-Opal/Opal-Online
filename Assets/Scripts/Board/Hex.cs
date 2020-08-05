using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    private Color clr;
    private Main myMain;
    private string type = "";
    private bool highlighted = false;

    // Start is called before the first frame update
    void Awake()
    {
        setColor(Color.cyan);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setColor(Color c)
    {
        clr = c;
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.material.color = c;
        }
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.material.color = c;
        }
        setTypeFromColor();
    }

    public Color getColor()
    {
        return clr;
    }

    public void setType(string t)
    {
        type = t;
        if(type == "Water")
        {
            bool surrounded = true;
            foreach(Hex h in myMain.getNeighbors(this))
            {
                if(h.getType() != "Water" && h.getType() != "Deep")
                {
                    surrounded = false;
                }
            }
            if (surrounded)
            {
                type = "Deep";
            }
        }
        setColorFromType();
    }

    public string getType()
    {
        return type;
    }

    private void setColorFromType()
    {
        switch (type) {
            case "Grass":
                clr = Color.green;
                break;
            case "Fire":
                clr = Color.red;
                break;
            case "Water":
                clr = Color.blue;
                break;
            case "Metal":
                clr = Color.grey;
                break;
            case "Poison":
                clr = Color.magenta;
                break;
            case "Dark":
                clr = Color.black;
                break;
            case "Ground":
                clr = new Color(89 / 255f, 61 / 255f, 31 / 255f);
                break;
            case "Light":
                clr = Color.white;
                break;
            case "Deep":
                clr = new Color(0 / 255f, 0 / 255f, 150 / 255f);
                break;
        }
        setColor(clr);
    }

    private void setTypeFromColor()
    {
        if (clr == Color.green)
            type = "Grass";
        else if (clr == Color.red)
            type = "Fire";
        else if (clr == Color.blue)
            type = "Water";
        else if (clr == Color.black)
            type = "Dark";
        else if (clr == Color.grey)
            type = "Metal";
        else if (clr == Color.white)
            type = "Light";
        else if (clr == Color.magenta)
            type = "Poison";
        else if (clr == new Color(89 / 255f, 61 / 255f, 31 / 255f))
            type = "Ground";
        else if (clr == new Color(100 / 255f, 100 / 255f, 255 / 255f))
            type = "Deep";
    }

    public void highlight(bool toggle)
    {
        
        if (toggle && !highlighted)
        {
            foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
            {
                mr.material.color = new Color(clr.r+0.5f, clr.g + 0.5f, clr.b + 0.5f);
            }
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.material.color = new Color(clr.r + 0.5f, clr.g + 0.5f, clr.b + 0.5f);
            }
        }
        else
        {
            setColorFromType();
        }
        highlighted = toggle;
    }

    public void setMyMain(Main m)
    {
        myMain = m;
    }

    public Vector2 getCoordinates()
    {
        if(transform.position.z % 4 != 0)
        {
            return new Vector2(transform.position.x / 3.4f, (transform.position.z-2) / 4);
        }
        return new Vector2(transform.position.x/3.4f, transform.position.z/4);
    }

    public void generateColor()
    {
        if(Random.Range(0,4) != 1) //Take identity of neighboring tile
        {
            List<Color> candidates = new List<Color>();
            foreach (Hex h in myMain.getNeighbors(this))
            {
                Color tempColor = h.getColor();
                if(tempColor != Color.cyan)
                {
                    candidates.Add(tempColor);
                    if(tempColor == Color.red || tempColor == Color.blue || tempColor == Color.green || tempColor == new Color(89 / 255f, 61 / 255f, 31 / 255f))
                    {
                        candidates.Add(tempColor);
                        if(tempColor == Color.blue || tempColor == Color.green)
                        {
                            candidates.Add(tempColor);
                            if (tempColor == Color.blue)
                            {
                                candidates.Add(tempColor);
                            }
                        }
                    }
                }
            }
            if(candidates.Count > 0)
            {
                setColor(candidates[Random.Range(0, candidates.Count)]);
                return;
            }
        }//pick new identity
        List<Color> tileTypes = new List<Color>();
        tileTypes.Add(Color.green); //grass
        tileTypes.Add(Color.blue); //water
        tileTypes.Add(Color.red); //fire
        tileTypes.Add(Color.grey); //metal
        tileTypes.Add(Color.magenta); //poison
        tileTypes.Add(Color.black); //dark
        tileTypes.Add(Color.white); //light
        tileTypes.Add(new Color(89/255f, 61/255f, 31/255f)); //mountains
        setColor(tileTypes[Random.Range(0, tileTypes.Count)]);                          

    }
}
