using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private static int mapSize = 100;
    private Hex[,] hexGrid = new Hex[mapSize, mapSize];
    private Vector3 mousePos = new Vector3();
    private Hex selectedHex;
    private bool canHighlight = true;
    // Start is called before the first frame update
    void Start()
    {
        generateBoard();
        colorBoard();
        populateDeeps();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(mousePos);
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (canHighlight && Physics.Raycast(mouseRay, out hit))
        {
            Hex temp = hit.collider.GetComponentInParent<Hex>();
            if(temp != null)
            {
                if(selectedHex != null)
                    selectedHex.highlight(false);
                selectedHex = temp;
            }
            
        }
        if (selectedHex != null && canHighlight)
        {
            selectedHex.highlight(true);
        }

        if (Input.GetMouseButton(0) && canHighlight == true)
        {
            canHighlight = false;
        }else if (!Input.GetMouseButton(0) && canHighlight == false)
        {
            canHighlight = true;
        }
        moveCamera();
    }

    private void moveCamera()
    {
        int xMove = 0;
        int yMove = 0;
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W))
            {
                yMove += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                yMove -= 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                xMove -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                xMove += 1;
            }
        }
        if (xMove != 0 || yMove != 0)
        {
            transform.position = new Vector3(transform.position.x + xMove * 0.5f, transform.position.y, transform.position.z + yMove * 0.5f);
        }
    }

    private void generateBoard()
    {
        Hex hexPrefab = Resources.Load<Hex>("Prefabs/Board/Hex");
        for(int i = 0; i < mapSize; i++)
        {
            for(int j = 0; j < mapSize; j++)
            {
                float posY = 0;
                if((i+j) % 2 != 0)
                {
                    posY = -0.1f;
                }
                Hex temp = Instantiate<Hex>(hexPrefab);
                hexGrid[i, j] = temp;
                temp.setMyMain(this);
                if (i % 2 == 0)
                    temp.transform.position = new Vector3(i * 3.4f, 0, j * 4f);
                else
                    temp.transform.position = new Vector3(i * 3.4f, 0, j * 4f+2f);
            }
        }
    }

    private void colorBoard()
    {
        foreach(Hex h in hexGrid)
        {
            h.generateColor();
        }
    }

    private void populateDeeps()
    {
        foreach(Hex h in hexGrid)
        {
            h.setType(h.getType());
        }
    }

    public List<Hex> getNeighbors(Hex h)
    {
        List<Hex> output = new List<Hex>();
        Vector2 pos = h.getCoordinates();
        int col = (int)pos.x;
        int row = (int)pos.y;
        int mod = -1;
        if(col % 2 != 0)
        {
            mod = 1;
        }
        if(col > 0)
        {
            output.Add(hexGrid[col - 1, row]);
            if ((row > 0 && mod == -1) || (row < mapSize - 1 && mod == 1))
            {
                output.Add(hexGrid[col - 1, row + mod]);
            }
        }
        if(col < mapSize - 1)
        {
            output.Add(hexGrid[col + 1, row]);
            if ((row > 0 && mod == -1) || (row < mapSize - 1 && mod == 1))
            {
                output.Add(hexGrid[col + 1, row + mod]);
            }
        }
        if (row > 0)
        {
            output.Add(hexGrid[col, row - 1]);
        }
        if (row < mapSize -1)
        {
            output.Add(hexGrid[col, row + 1]);
        }
        return output;
    }

}
