using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ConnectedTile : MonoBehaviour
{
    private Sprite[] connectedTileSprites;
    private SpriteRenderer changeSpriteRenderer;
    private List<List<int>> waterTileGroups = new List<List<int>>();
    private List<List<int>> grassTileGroups = new List<List<int>>();

    // Start is called before the first frame update
    void Awake()
    {
        changeSpriteRenderer = GetComponent<SpriteRenderer>();
        connectedTileSprites = Resources.LoadAll<Sprite>("Sprites/Water_Tiles");

        //Top left water sprites
        waterTileGroups.Add(new List<int>() { 4, 8, 9, 10, 17, 23, 29, 31, 34, 35, 45, 46 });

        //Top water sprites
        waterTileGroups.Add(new List<int>() { 0, 1, 2, 3, 5, 6, 7, 28, 36, 37, 38, 39, 40 });

        //Top right water sprites
        waterTileGroups.Add(new List<int>() { 2, 9, 10, 11, 16, 20, 21, 22, 29, 30, 35, 41, 42, 46 });

        //Left water sprites
        waterTileGroups.Add(new List<int>() { 0, 3, 12, 15, 16, 24, 27, 28, 36, 39, 41, 42, 43 });

        //Right water sprites
        waterTileGroups.Add(new List<int>() { 2, 3, 4, 14, 15, 17, 18, 19, 26, 27, 38, 39, 40 });

        //Bottom left water sprites
        waterTileGroups.Add(new List<int>() { 5, 7, 11, 17, 18, 22, 33, 34, 35, 40, 44, 45, 46 });

        //Bottom water sprites
        waterTileGroups.Add(new List<int>() { 4, 16, 24, 25, 26, 27, 29, 30, 31, 36, 37, 38, 39 });

        //Bottom right water sprites
        waterTileGroups.Add(new List<int>() { 5, 6, 10, 21, 22, 23, 28, 32, 33, 34, 35, 41, 43 });

        //Top left grass sprites
        grassTileGroups.Add(new List<int>() { 13, 14, 18, 20, 21, 22, 25, 26, 30, 32, 33, 44 });

        //Top grass sprites
        grassTileGroups.Add(new List<int>() { 4, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 41, 42, 43, 44, 45, 46 });

        //Top right grass sprites
        grassTileGroups.Add(new List<int>() { 8, 12, 13, 23, 24, 25, 31, 32, 33, 34, 43, 44, 45 });

        //Left grass sprites
        grassTileGroups.Add(new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 13, 14, 17, 18, 19, 20, 21, 22, 23, 25, 26, 29, 30, 31, 32, 33, 34, 35, 37, 38, 40, 44, 45, 46 });

        //Right grass sprites
        grassTileGroups.Add(new List<int>() { 0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 20, 21, 22, 23, 24, 25, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 41, 42, 43, 44, 45, 46 });

        //Bottom left grass sprites
        grassTileGroups.Add(new List<int>() { 1, 2, 6, 8, 9, 10, 13, 14, 19, 20, 21, 23, 32 });

        //Bottom grass sprites
        grassTileGroups.Add(new List<int>() { 0, 1, 2, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 17, 18, 19, 20, 21, 22, 23, 28, 32, 33, 34, 35, 40, 41, 42, 43, 44, 45, 46 });

        //Bottom right grass sprites
        grassTileGroups.Add(new List<int>() { 0, 1, 7, 8, 9, 11, 13, 20, 42, 44, 45, 46 });
    }

    public void changeSprite(string code) //code is format 111141111
    {
        List<int> possibleTiles = new List<int>();
        for (int i = 0; i < 47; i++)
        {
            possibleTiles.Add(i);
        }

        code = code.Substring(0, 4) + code.Substring(5, 4);
        for (int i = 0; i < code.Length; i++)
        {
            if (code.Substring(i, 1).Equals("4"))
            {
                foreach (int tile in waterTileGroups[i])
                {
                    if (possibleTiles.Contains(tile))
                    {
                        possibleTiles.Remove(tile);
                    }
                }
            }

            if (code.Substring(i, 1).Equals("1"))
            {
                foreach (int tile in grassTileGroups[i])
                {
                    if (possibleTiles.Contains(tile))
                    {
                        possibleTiles.Remove(tile);
                    }
                }
            }

            Debug.Log("Possible tiles(step " + i + "): " + debugToString(possibleTiles));
        }

        if (possibleTiles.Count > 0)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[possibleTiles[0]];
        }
        else
        {
            changeSpriteRenderer.sprite = connectedTileSprites[0];
            Debug.Log("Something went wrong with water tile sprite selection! The problem tile had the following code: " + code);
        }

    }

    private string debugToString(List<int> nums)
    {
        string temp = "[ ";

        for (int i = 0; i < nums.Count - 1; i++)
        {
            temp += nums[i] + ", ";
        }

        temp += nums[nums.Count - 1] + " ]";

        return temp;
    }
}
