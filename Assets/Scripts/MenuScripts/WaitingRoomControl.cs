using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Text.RegularExpressions;

public class WaitingRoomControl : MonoBehaviourPunCallbacks, IPunObservable
{
    private GlobalScript glob;
    public Text roomName;
    public Text chatTextHistory;
    public InputField chatTextEnter;
    private string username = "";
    private string chat = "";
    private bool typing = true;
    private bool joiningRoom = true;
    public Text playerList;
    public GameObject startGameButton;

    // Start is called before the first frame update
    void Start()
    {
        glob = GameObject.Find("GlobalObject").GetComponent<GlobalScript>();
        username = glob.getUsername();
        roomName.text = PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
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
            joiningRoom = false;
        }
        else
        {
            startGameButton.SetActive(false);
        }

        string playerListBuilder = "";
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            playerListBuilder += p.NickName + "\n";
        }
        playerList.text = playerListBuilder;
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

    [PunRPC]
    private void updatePlayerCount()
    {
        glob.setNumPlayers(PhotonNetwork.CurrentRoom.PlayerCount);
    }



    public void startMatch() //needs to detect number of players
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            this.photonView.RPC("updatePlayerCount", RpcTarget.All);
            PhotonNetwork.LoadLevel("MainGame");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (joiningRoom && !chat.Equals(""))
        {
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
            joiningRoom = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chat += "\n<color=blue>" + username + " has left the match!</color>";
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("MainMenu");
        }

        processChat();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
        }

        string playerListBuilder = "";
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            playerListBuilder += p.NickName + "\n";
        }
        playerList.text = playerListBuilder;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        string playerListBuilder = "";
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            playerListBuilder += p.NickName + "\n";
        }
        playerList.text = playerListBuilder;
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
