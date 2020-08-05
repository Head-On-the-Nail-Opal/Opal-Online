using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private GameObject startGameButton;
	[SerializeField] private GameObject loading;

	public GameObject playerPrefab;
    public int numPlayers;
	private GlobalScript glob;
	private MultiplayerManager gameStarter;

	private void Start()
	{
		//PhotonNetwork.Instantiate("Prefabs/MultiplayerManager", new Vector3(0, 0, 0), Quaternion.identity, 0);
		glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
		startGameButton.SetActive(false);
		loading.SetActive(false);
        glob.setNumPlayers(numPlayers);
		if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
		{
			startGameButton.SetActive(true);
		}
		if (playerPrefab == null)
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
		}
		else
		{
			Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene());
			if (playerController.LocalPlayerInstance == null)
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
				// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
				PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 2f, 0f), Quaternion.identity, 0);
			}
			else
			{
				Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
		}
	}

	//Called when the local player left the room. We need to load the launcher scene.
	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	void LoadArena()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
		}
		else
		{
			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
		}
	}

    

	public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
	{
		Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName); // not seen if you're the player connecting
        glob.setNumPlayers(numPlayers);

        if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


			LoadArena();
		}
	}


	public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
	{
		Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName); // seen when other disconnects


		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


			LoadArena();
		}
	}

    public void onStart()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("MainGame");
    }


	//public void constructOnlineTeams(int numPlayers)
	//{
	//	gameStarter = GameObject.Find("MultiplayerManager(Clone)").GetComponent<MultiplayerManager>();
	//	loading.SetActive(true);
	//	gameStarter.sendInfo(numPlayers);
 //   }
}