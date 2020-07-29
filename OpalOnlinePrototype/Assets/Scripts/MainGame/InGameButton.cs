using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    public Sprite unpressed;
    public Sprite hover;
    public GroundScript boardScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().sprite = hover;
    }

    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().sprite = unpressed;
    }

    private void OnMouseUp()
    {
        if (boardScript.getMult())
        {
            PhotonNetwork.LeaveRoom();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            Cursor.visible = true;
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            Cursor.visible = true;
        }
    }
}
