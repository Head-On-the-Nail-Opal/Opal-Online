using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedTile : MonoBehaviour
{
    public List<Sprite> connectedTileSprites = new List<Sprite>();
    private SpriteRenderer changeSpriteRenderer;
    // Start is called before the first frame update
    void Awake()
    {
        changeSpriteRenderer = GetComponent<SpriteRenderer>();
        sortSprites();
    }

    private void sortSprites()
    {

    }

    public void changeSprite(string code) //code is format 111141111
    {
        int easy = tryEasy(code);
        if(easy == -1)
        {
            tryHard(int.Parse(code));
            return;
        }
        changeSpriteRenderer.sprite = connectedTileSprites[easy];
    }

    private int tryEasy(string code)
    {
        //remove corners because they don't matter in a lot of cases
        string newCode = ""+'*'+code[1]+'*'+code[3]+code[4]+code[5]+'*'+code[7]+'*';
        switch (newCode) {
            case "*1*141*1*": //true
                return 18;
            case "*4*141*1*": //true
                return 13;
            case "*1*441*1*": //true
                return 17;
            case "*1*444*1*": //true
                return 16;
            case "*1*144*1*": //true
                return 15;
            case "*4*141*4*": //true
                return 8;
            case "*1*141*4*": //true
                return 3;
        }

        return -1;
    }

    private void tryHard(int code)
    {
        if (code == 444444444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[6];
        }
        else if (code == 441441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[0];
        }
        else if (code == 144144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[2];
        }
        else if (code == 111441441)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[10];
        }
        else if (code == 111144144)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[12];
        }
        else if (code == 144144144)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[7];
        }
        else if (code == 441441441)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[5];
        }
        else if (code == 111444444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[11];
        }
        else if (code == 444444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[1];
        }
        else if (code == 141141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (code == 141141444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if (code == 444141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if (code == 111141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }
        else if (code == 141141141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[8];
        }
        else if (code == 141444141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[23];
        }
        else if (code == 111141444)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }
        else if (code == 444141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (code == 411441411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (code == 114144114)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
        else if (code == 111441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (code == 111144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
        else if (code == 111444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[16];
        }
        else if (code == 411441111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (code == 114144111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
        else if (code == 141444111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[0];
        }
        else if (code == 441141111)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[3];
        }
        else if (code == 141441141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[5];
        }
        else if (code == 111441411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[13];
        }

        else if (code == 111411411)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[11];
        }
        else if (code == 111444141)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[15];
        }
        else if (code == 111144114)
        {
            changeSpriteRenderer.sprite = connectedTileSprites[17];
        }
    }

}
