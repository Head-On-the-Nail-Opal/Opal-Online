using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class playerController : MonoBehaviourPunCallbacks
{

    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerController.LocalPlayerInstance = this.gameObject;
        }
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }


}

