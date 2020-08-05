using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;

public class LobbyControl : MonoBehaviourPunCallbacks, IPunObservable
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
    public Text chatTextHistory;
    public Text joinRoomName;
    public InputField chatTextEnter;
    private List<string> activeRooms;
    private string username = "";
    byte numberOfPlayers = 2;
    private string chat = "";
    private bool typing = true;
    private string roomName = "";
    private bool creatingRoom = false;
    private bool joiningRoom = false;
    private bool refreshingLobby = false;
    private bool rejoiningLobby = false;
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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(chat);
        }
        else
        {
            // Network player, receive data
            this.chat = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void sendChatData(string data)
    {
        chat = data;
        chatTextHistory.text = chat;
    }

    // Update is called once per frame
    void Update()
    {
        if (username == "" && Input.GetKeyDown(KeyCode.Return) && usernameInput.GetComponentInChildren<InputField>().text != "")
        {
            username = usernameInput.GetComponentInChildren<InputField>().text;
            usernameInput.SetActive(false);
            lobbyObjects.SetActive(true);

            chat += "\n<color=blue>" + username + " has joined the chat! ";

            int rand = Random.Range(0, 8);
            if (rand < 3)
            {
                chat += "I've just been told that approximately " + Random.Range(100, 1000) + " people hate this person.</color>";
            }
            else if (rand > 2 && rand < 5)
            {
                chat += "I hear this person is incredibly rude.</color>";
            }
            else
            {
                chat += "As of today, this person has kicked " + Random.Range(20, 100) + " puppies.</color>";
            }
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
            chatTextEnter.placeholder.GetComponent<Text>().text = "Press enter to chat...";
            chatTextEnter.text = "";
            typing = false;
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
            chat += "\n<color=blue>" + username + " has left the lobby!</color>";
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("MainMenu");
        }

        if (rejoiningLobby)
        {
            PhotonNetwork.JoinRoom("Lobby");
            rejoiningLobby = false;
        }

        processChat();
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
        if (!refreshingLobby && !rejoiningLobby && activeRooms.Count > 0)
        {
            joiningRoom = true;
            glob.setUsername(username);
            PhotonNetwork.LocalPlayer.NickName = username;
            PhotonNetwork.LeaveRoom();
        }
    }

    public void createRoom()
    {
        if (int.Parse(roomSizeInput.text) >= 1 && int.Parse(roomSizeInput.text) <= 4 && roomNameInput.text != "")
        {
            creatingRoom = true;
            glob.setUsername(username);
            roomName = roomNameInput.text;
            numberOfPlayers = (byte)int.Parse(roomSizeInput.text);
            chat += "\n<color=blue>" + username + " has joined a match!</color>";
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
            PhotonNetwork.LocalPlayer.NickName = username;
            PhotonNetwork.LeaveRoom();
        }
    }

    public void rejoinGame()
    {
        PhotonNetwork.JoinRoom(PlayerPrefs.GetString("CurrentMatch"));
        PhotonNetwork.LoadLevel("MainGame");
    }

    public override void OnConnectedToMaster()
    {
        if (creatingRoom)
        {
            Debug.Log("tried to create room " + roomName + " with " + numberOfPlayers + " people");
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = numberOfPlayers });
        }
        if (refreshingLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        if (joiningRoom)
        {
            Debug.Log("just tried to join a room");
            PhotonNetwork.JoinRoom(activeRooms[roomListPosition]);
        }
    }

    public override void OnJoinedRoom()
    {
        if (creatingRoom || joiningRoom)
        {
            Debug.Log("we joined a room");
            PhotonNetwork.LoadLevel("WaitingRoom");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (joiningRoom)
        {
            Debug.Log("You tried to join a room, but it didn't work");
            PhotonNetwork.JoinRoom("Lobby");
            joiningRoom = false;
        }
    }

    public void refreshRoomsList()
    {
        if (!refreshingLobby && !rejoiningLobby)
        {
            refreshingLobby = true;
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        activeRooms = new List<string>();
        refreshingLobby = false;
        string rooms = "";
        foreach (RoomInfo room in roomList)
        {
            if (room.Name != "Lobby")
            {
                activeRooms.Add(room.Name);
                rooms += (room.IsOpen) ? room.Name + "(" + room.PlayerCount + "/" + room.MaxPlayers + ")\n" : room.Name + "(" + room.PlayerCount + ")\n";
            }
        }
        roomsList.text = rooms;
        rejoiningLobby = true;
    }

    private void processChat()
    {
        if (!typing && Input.GetKeyDown(KeyCode.Return) && username != "")
        {
            chatTextEnter.ActivateInputField();
            chatTextEnter.placeholder.GetComponent<Text>().text = "";
            typing = true;
        }

        if (typing && Input.GetKeyDown(KeyCode.Return) && username != "")
        {
            if (chatTextEnter.text != "")
            {
                if (Regex.IsMatch(chatTextEnter.text, "jack", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "charlie", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "charlue", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "char", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "chuck", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "chris", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "grace", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "reid", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "james", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "victor", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "griffin", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "grif", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "griff", RegexOptions.IgnoreCase) || Regex.IsMatch(chatTextEnter.text, "opal", RegexOptions.IgnoreCase))
                {
                    string[] words = chatTextEnter.text.Split();
                    int pos;
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].ToLower().IndexOf("charlie") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("charlie");
                            words[i] = words[i].Substring(0, pos) + "gay boi" + words[i].Substring(pos + 7, words[i].Length - pos - 7);
                        }
                        if (words[i].ToLower().ToLower().IndexOf("charlue") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("charlue");
                            words[i] = words[i].Substring(0, pos) + "gay boi" + words[i].Substring(pos + 7, words[i].Length - pos - 7);
                        }
                        if (words[i].ToLower().IndexOf("char") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("char");
                            words[i] = words[i].Substring(0, pos) + "gay boi" + words[i].Substring(pos + 4, words[i].Length - pos - 4);
                        }
                        if (words[i].ToLower().IndexOf("chuck") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("chuck");
                            words[i] = words[i].Substring(0, pos) + "gay boi" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                        }
                        if (words[i].ToLower().IndexOf("chris") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("chris");
                            words[i] = words[i].Substring(0, pos) + "sorry chris" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                        }
                        if (words[i].ToLower().IndexOf("grace") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("grace");
                            int rand = Random.Range(0, 8);
                            if (rand < 3)
                            {
                                words[i] = words[i].Substring(0, pos) + "Grace \"I like children\" Bishop" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                            }
                            else if (rand > 2 && rand < 5)
                            {
                                words[i] = words[i].Substring(0, pos) + "Bean \"I like children\" Bishop" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                            }
                            else
                            {
                                words[i] = words[i].Substring(0, pos) + "gronse" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                            }
                        }
                        if (words[i].ToLower().IndexOf("reid") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("reid");
                            words[i] = words[i].Substring(0, pos) + "super Reid" + words[i].Substring(pos + 4, words[i].Length - pos - 4);
                        }
                        if (words[i].ToLower().IndexOf("james") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("james");
                            int rand = Random.Range(0, 9);
                            if (rand < 5)
                            {
                                words[i] = words[i].Substring(0, pos) + "YOB GAF" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                            }
                            else
                            {
                                words[i] = words[i].Substring(0, pos) + "semaj" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                            }
                        }
                        if (words[i].ToLower().IndexOf("victor") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("victor");
                            words[i] = words[i].Substring(0, pos) + "icky vicky" + words[i].Substring(pos + 6, words[i].Length - pos - 6);
                        }
                        if (words[i].ToLower().IndexOf("griffin") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("griffin");
                            words[i] = words[i].Substring(0, pos) + "GORF" + words[i].Substring(pos + 7, words[i].Length - pos - 7);
                        }
                        if (words[i].ToLower().IndexOf("griff") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("griff");
                            words[i] = words[i].Substring(0, pos) + "GORF" + words[i].Substring(pos + 5, words[i].Length - pos - 5);
                        }
                        if (words[i].ToLower().IndexOf("grif") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("grif");
                            words[i] = words[i].Substring(0, pos) + "GORF" + words[i].Substring(pos + 4, words[i].Length - pos - 4);
                        }
                        if (words[i].ToLower().IndexOf("opal") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("opal");
                            words[i] = words[i].Substring(0, pos) + "Opal (Best Game)" + words[i].Substring(pos + 4, words[i].Length - pos - 4);
                        }
                        if (words[i].ToLower().IndexOf("jack") != -1)
                        {
                            pos = words[i].ToLower().IndexOf("jack");
                            words[i] = words[i].Substring(0, pos) + "Condescension King" + words[i].Substring(pos + 4, words[i].Length - pos - 4);
                        }
                    }
                    chat += "\n<color=red>" + username + ":</color> ";
                    foreach (string s in words)
                    {
                        chat += s + " ";
                    }
                }
                else
                {
                    chat += "\n<color=red>" + username + ":</color> " + chatTextEnter.text;
                }
                chatTextEnter.placeholder.GetComponent<Text>().text = "Press enter to chat...";
                chatTextEnter.text = "";
                typing = false;
                this.photonView.RPC("sendChatData", RpcTarget.All, chat);

            }
        }
    }
}
