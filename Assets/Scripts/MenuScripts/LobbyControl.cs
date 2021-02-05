using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;

public class LobbyControl : MonoBehaviourPunCallbacks
{
    private GlobalScript glob;
    public GameObject usernameInput;
    public GameObject lobbyObjects;
    public GameObject createRoomObjects;
    public GameObject joinRoomObjects;
    public GameObject rejoinGameButton;
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
        if (!PlayerPrefs.GetString("CurrentMatch").Equals("none") && !PlayerPrefs.GetString("CurrentMatch", "").Equals(""))
        {
            rejoinGameButton.SetActive(true);
        }
        else
        {
            rejoinGameButton.SetActive(false);
        }
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

        if (Input.GetMouseButton(0) && (Input.mousePosition.x < 303 || Input.mousePosition.x > 1616 || Input.mousePosition.y < 269 || Input.mousePosition.y > 806))
        {
            createRoomObjects.SetActive(false);
            joinRoomObjects.SetActive(false);
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
            glob.setUsername(username);
            PhotonNetwork.LocalPlayer.NickName = username;
            Debug.Log("just tried to join a room");
            PhotonNetwork.JoinRoom(activeRooms[roomListPosition]);
        }
    }

    public void createRoom()
    {
        if (int.Parse(roomSizeInput.text) >= 1 && int.Parse(roomSizeInput.text) <= 4 && roomNameInput.text != "")
        {
            glob.setUsername(username);
            roomName = roomNameInput.text;
            numberOfPlayers = (byte)int.Parse(roomSizeInput.text);
            PhotonNetwork.LocalPlayer.NickName = username;

            Debug.Log("tried to create room " + roomName + " with " + numberOfPlayers + " people");
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = numberOfPlayers });
        }
    }

    public void rejoinGame()
    {
        PhotonNetwork.JoinRoom(PlayerPrefs.GetString("CurrentMatch"));
        PhotonNetwork.LoadLevel("MainGame");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("we joined a room");
        PhotonNetwork.LoadLevel("WaitingRoom");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        activeRooms = new List<string>();
        string rooms = "";

        foreach (RoomInfo room in roomList)
        {
            if (room.PlayerCount != 0)
            {
                Debug.Log("Open Room: " + room.Name);
                activeRooms.Add(room.Name);
                rooms += (room.IsOpen) ? room.Name + "(" + room.PlayerCount + "/" + room.MaxPlayers + ")\n" : room.Name + "(" + room.PlayerCount + ")\n";
            }
        }
        roomsList.text = rooms;
        if (joinRoomObjects.activeInHierarchy)
        {
            openJoinRoomMenu();
        }
    }
}
