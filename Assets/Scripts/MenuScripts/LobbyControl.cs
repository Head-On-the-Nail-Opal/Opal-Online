using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    private GlobalScript glob;
    public GameObject usernameInput;
    public GameObject lobbyObjects;
    public GameObject createRoomObjects;
    public GameObject joinRoomObjects;
    public InputField roomNameInput;
    public InputField roomSizeInput;
    public Text roomsList;
    public Text joinRoomName;
    private List<string> activeRooms;
    private string username = "";
    byte numberOfPlayers = 2;
    private string roomName = "";
    private int roomListPosition;

    // Start is called before the first frame update
    void Start()
    {
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        activeRooms = new List<string>();
        usernameInput.GetComponentInChildren<InputField>().ActivateInputField();
        lobbyObjects.SetActive(false);
        createRoomObjects.SetActive(false);
        joinRoomObjects.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    // Update is called once per frame
    void Update()
    {
        if (username == "" && Input.GetKeyDown(KeyCode.Return) && usernameInput.GetComponentInChildren<InputField>().text != "")
        {
            username = usernameInput.GetComponentInChildren<InputField>().text;
            usernameInput.SetActive(false);
            lobbyObjects.SetActive(true);
        }

        if (username == "" && Input.GetKeyDown(KeyCode.Return) && usernameInput.GetComponentInChildren<InputField>().text == "")
        {
            usernameInput.GetComponentInChildren<InputField>().ActivateInputField();
        }

        if (Input.GetMouseButton(0))
        {
            if (Physics.RaycastAll(Input.mousePosition, Vector3.forward).Length == 1)
            {
                createRoomObjects.SetActive(false);
                joinRoomObjects.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && username != "")
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("MainMenu");
        }
    }

    public void openRoomCreateMenu()
    {
        createRoomObjects.SetActive(true);
    }

    public void openJoinRoomMenu()
    {
        roomListPosition = 0;
        joinRoomObjects.SetActive(true);
        if (activeRooms.Count > 0)
        {
            joinRoomName.text = activeRooms[roomListPosition];
        }
    }

    public void nextRoom()
    {
        if (activeRooms.Count > 0)
        {
            if (roomListPosition == activeRooms.Count - 1)
            {
                roomListPosition = 0;
            }
            else
            {
                roomListPosition++;
            }
            joinRoomName.text = activeRooms[roomListPosition];
        }
    }

    public void previousRoom()
    {
        if (activeRooms.Count > 0)
        {
            if (roomListPosition == 0)
            {
                roomListPosition = activeRooms.Count - 1;
            }
            else
            {
                roomListPosition--;
            }
            joinRoomName.text = activeRooms[roomListPosition];
        }
    }

    public void joinRoom()
    {
        if (activeRooms.Count > 0)
        {
            if (activeRooms[roomListPosition].Substring(activeRooms[roomListPosition].Length - 1).Equals("*"))
            {
                if (PlayerPrefs.GetString("CurrentMatch").Equals(activeRooms[roomListPosition].Substring(0, activeRooms[roomListPosition].IndexOf("*"))))
                {
                    glob.setUsername(username);
                    PhotonNetwork.LocalPlayer.NickName = username;
                    Debug.Log("just tried to join a room");
                    PhotonNetwork.JoinRoom(PlayerPrefs.GetString("CurrentMatch"));
                }
            } else
            {
                glob.setUsername(username);
                PhotonNetwork.LocalPlayer.NickName = username;
                Debug.Log("just tried to join a room");
                PhotonNetwork.JoinRoom(activeRooms[roomListPosition]);
            }
        }
    }

    public void createRoom()
    {
        if (int.Parse(roomSizeInput.text) > 1 && int.Parse(roomSizeInput.text) <= 4 && roomNameInput.text != "" && roomNameInput.text.IndexOf("*") == -1)
        {
            glob.setUsername(username);
            roomName = roomNameInput.text;
            numberOfPlayers = (byte)int.Parse(roomSizeInput.text);
            PhotonNetwork.LocalPlayer.NickName = username;

            Debug.Log("tried to create room " + roomName + " with " + numberOfPlayers + " people");
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = numberOfPlayers, CustomRoomPropertiesForLobby = new string[] { "GameActive" } });
        } 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("we joined a room");
        if (PlayerPrefs.GetString("CurrentMatch").Equals(PhotonNetwork.CurrentRoom.Name))
        {
            PhotonNetwork.LoadLevel("MainGame");
        } else
        {
            PhotonNetwork.LoadLevel("WaitingRoom");
            PlayerPrefs.SetString("CurrentMatch", PhotonNetwork.CurrentRoom.Name);
            Hashtable customProps = new Hashtable();
            customProps.Add("GameActive", false);
            PhotonNetwork.CurrentRoom.SetCustomProperties(customProps);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PlayerPrefs.SetString("CurrentMatch", "");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        activeRooms = new List<string>();
        string rooms = "";

        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount != 0)
            {
                if (room.PlayerCount!= room.MaxPlayers)
                {
                    activeRooms.Add(room.Name + (room.CustomProperties["GameActive"].ToString().Equals("True") ? "*" : ""));
                }
                rooms += (room.MaxPlayers != room.PlayerCount) ? room.Name + "(" + room.PlayerCount + "/" + room.MaxPlayers + ")" + (room.CustomProperties["GameActive"].ToString().Equals("True") ? "*\n" : "\n") : room.Name + "(" + room.PlayerCount + ")\n";
            }
        }
        roomsList.text = rooms;
        if (joinRoomObjects.activeInHierarchy)
        {
            openJoinRoomMenu();
        }
    }
}
