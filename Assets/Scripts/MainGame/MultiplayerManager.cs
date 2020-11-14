using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class MultiplayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    private string multiplayerData = "";
    private string chat = "";
    private string username = "";
    private List<string> buffer = new List<string>();
    private string teamOne = "";
    private string teamTwo = "";
    private string teamThree = "";
    private string teamFour = "";
    private string gameString = "";
    private string gameHistory = "";
    private GroundScript boardScript;
    private CursorScript cs;
    public Text chatTextHistory;
    public InputField chatTextEnter;
    private bool typing = false;
    private string numPlayers = "2";


    public void Awake()
    {
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(multiplayerData);
            stream.SendNext(teamOne);
            stream.SendNext(teamTwo);
            stream.SendNext(teamThree);
            stream.SendNext(teamFour);
            stream.SendNext(gameHistory);
            stream.SendNext(numPlayers);
            stream.SendNext(chat);
        }
        else
        {
            // Network player, receive data
            this.multiplayerData = (string)stream.ReceiveNext();
            this.teamOne = (string)stream.ReceiveNext();
            this.teamTwo = (string)stream.ReceiveNext();
            this.teamThree = (string)stream.ReceiveNext();
            this.teamFour = (string)stream.ReceiveNext();
            this.gameHistory = (string)stream.ReceiveNext();
            this.numPlayers = (string)stream.ReceiveNext();
            this.chat = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void sendData(string data)
    {
        Debug.LogError("Griff's Dumb Data!!!: " + data);
        string command = data;
        if (boardScript.getOnlineTeam() != cs.getCurrentOnlinePlayer())
        {
            if (command != "")
            {
                print("Received command: " + command);
                gameHistory += command + '\n';
                string[] parsedCommand = command.Split(',');
                if (parsedCommand[0] == "place" && int.Parse(parsedCommand[4]) != boardScript.getOnlineTeam())
                {
                    if (cs.getCurrentOpal().getMyName() == parsedCommand[1])
                    {
                        cs.getCurrentOpal().setPos(int.Parse(parsedCommand[2]), int.Parse(parsedCommand[3]));
                        cs.getCurrentOpal().setMyTurn(false);
                        cs.getCurrentOpal().updateTile();
                        cs.getCurrentOpal().getCurrentTile().standingOn(cs.getCurrentOpal());
                        cs.nextTurn();
                    }
                }
                else if (parsedCommand[0] == "move" && int.Parse(parsedCommand[2]) != boardScript.getOnlineTeam())
                {
                    //print("moving");
                    List<PathScript> p = new List<PathScript>();
                    for (int i = 3; i < parsedCommand.Count(); i++)
                    {
                        if (parsedCommand[i] != "")
                        {
                            PathScript t = Instantiate<PathScript>(Resources.Load<PathScript>("Prefabs/Path"));
                            p.Add(t);
                            t.setCoordinates(int.Parse(parsedCommand[i].Split('_')[0]), int.Parse(parsedCommand[i].Split('_')[1]));
                        }
                    }
                    cs.doMove(p);
                }
                else if (parsedCommand[0] == "end" && int.Parse(parsedCommand[1]) != boardScript.getOnlineTeam())
                {
                    cs.pressEndTurn();

                }
                else if (parsedCommand[0] == "attack" && int.Parse(parsedCommand[1]) != boardScript.getOnlineTeam())
                {
                    cs.doAttack(int.Parse(parsedCommand[2]), int.Parse(parsedCommand[3]), int.Parse(parsedCommand[4]));
                }
                else if (parsedCommand[0] == "reset")
                {

                }
            }
            else
            {

                if (buffer.Count() > 0)
                {
                    foreach (string s in buffer)
                    {
                        print("Waiting: " + s);
                    }
                    //multiplayerData = buffer[0];
                    buffer.RemoveAt(0);
                }
            }
        }
        //if (multiplayerData != "")
        //{
         //   buffer.Add(data);

        //}
        //else
        //{
        //    multiplayerData = data;

        //}
        //Update();
    }

    [PunRPC]
    private void sendNumPlayers(int num)
    {
        numPlayers = num + "";
    }

    [PunRPC]
    private void sendChatData(string data)
    {
        chat = data;
        chatTextHistory.text = chat;
    }

    [PunRPC]
    private void sendFullGame(string data)
    {
        gameString = data;
    }

    public string getGameData()
    {
        return gameHistory;
    }

    public void sendMultiplayerData(string data)
    {
        //Debug.LogError("Griff's Dumb Data!!!: " + data);
        this.photonView.RPC("sendData", RpcTarget.All, data);
    }

    public void sendFullGameData(string data)
    {
        //Debug.LogError("Griff's Dumb Data!!!: " + data);
        this.photonView.RPC("sendFullGame", RpcTarget.All, data);
    }

    public void sendNum(int num)
    {
        this.photonView.RPC("sendNumPlayers", RpcTarget.All, num);
    }

    [PunRPC]
    private void sendTeam(string data)
    {
        if(teamOne == "")
        {
            teamOne = data;
        }else if(teamTwo == "")
        {
            teamTwo = data;
        }
        else if(teamThree == "")
        {
            teamThree = data;
        }
        else if(teamFour == "")
        {
            teamFour = data;
        }
    }

    public void sendMultiplayerTeam(string myTeam)
    {
        this.photonView.RPC("sendTeam", RpcTarget.All, myTeam);
    }

    public void setUpMM(GroundScript bs, CursorScript c)
    {
        PlayerPrefs.SetString("CurrentMatch", PhotonNetwork.CurrentRoom.Name);
        boardScript = bs;
        cs = c;
    }

    public string getTeamOne()
    {
        return teamOne;
    }

    public string getTeamTwo()
    {
        return teamTwo;
    }

    public string getTeamThree()
    {
        return teamThree;
    }

    public string getTeamFour()
    {
        return teamFour;
    }

    public int getNumPlayers()
    {
        return int.Parse(numPlayers);
    }

    public string getData()
    {
        return multiplayerData;
    }

    void Update()
    {
        if (boardScript != null)
        {
            if (boardScript.getMult() == false)
            {
                return;
            }
            string command = multiplayerData;
            if (boardScript.getOnlineTeam() != cs.getCurrentOnlinePlayer())
            {
                if (command != "")
                {
                    print("Received command: " + command);
                    multiplayerData = "";
                    string[] parsedCommand = command.Split(',');
                    if (parsedCommand[0] == "place" && int.Parse(parsedCommand[4]) != boardScript.getOnlineTeam())
                    {
                        if (cs.getCurrentOpal().getMyName() == parsedCommand[1])
                        {
                            cs.getCurrentOpal().setPos(int.Parse(parsedCommand[2]), int.Parse(parsedCommand[3]));
                            cs.getCurrentOpal().setMyTurn(false);
                            cs.getCurrentOpal().updateTile();
                            cs.getCurrentOpal().getCurrentTile().standingOn(cs.getCurrentOpal());
                            cs.nextTurn();
                        }
                    }
                    else if (parsedCommand[0] == "move" && int.Parse(parsedCommand[2]) != boardScript.getOnlineTeam())
                    {
                        //print("moving");
                        List<PathScript> p = new List<PathScript>();
                        for (int i = 3; i < parsedCommand.Count(); i++)
                        {
                            if (parsedCommand[i] != "")
                            {
                                PathScript t = Instantiate<PathScript>(Resources.Load<PathScript>("Prefabs/Path"));
                                p.Add(t);
                                t.setCoordinates(int.Parse(parsedCommand[i].Split('_')[0]), int.Parse(parsedCommand[i].Split('_')[1]));
                            }
                        }
                        cs.doMove(p);
                    }
                    else if (parsedCommand[0] == "end" && int.Parse(parsedCommand[1]) != boardScript.getOnlineTeam())
                    { 
                        cs.pressEndTurn();

                    }
                    else if (parsedCommand[0] == "attack" && int.Parse(parsedCommand[1]) != boardScript.getOnlineTeam())
                    {
                        cs.doAttack(int.Parse(parsedCommand[2]), int.Parse(parsedCommand[3]), int.Parse(parsedCommand[4]));
                    }
                    else if (parsedCommand[0] == "reset")
                    {
                        boardScript.updateFromString(boardScript.getMM().getGameData());
                    }
                }
                else
                {
                    
                    if (buffer.Count() > 0)
                    {
                        foreach(string s in buffer)
                        {
                            print("Waiting: " + s);
                        }
                        multiplayerData = buffer[0];
                        buffer.RemoveAt(0);
                    }
                }
            }
            else
            {

                if (command != "")
                {
                    string[] parsedCommand = command.Split(',');
                    if (parsedCommand[0] == "end" && int.Parse(parsedCommand[1]) != boardScript.getOnlineTeam())
                    {
                        print("Catching late: " + command);
                        cs.pressEndTurn();

                    }
                    print("On my turn I see: " + command);
                    multiplayerData = "";
                }
            }

        }
        else
        {
            //processChat();
        }
    }

    private void processChat()
    {
        if (!typing && Input.GetKeyDown(KeyCode.Return) && username == "")
        {
            chatTextEnter.ActivateInputField();
            chatTextEnter.placeholder.GetComponent<Text>().text = "";
            typing = true;
        }

        if (typing && Input.GetKeyDown(KeyCode.Return) && username == "" && chatTextEnter.text != "")
        {
            username = chatTextEnter.text;
            chat += "\n<color=blue>" + username + " has joined the chat! ";
            int rand = Random.Range(0, 8);
            if (rand < 3)
            {
                chat += "I've just been told that approximately " + Random.Range(100, 1000) + " people want to date this person.</color>";
            }
            else if (rand > 2 && rand < 5)
            {
                chat += "I hear this person is incredibly cute.</color>";
            }
            else
            {
                chat += "As of today, this person has hugged " + Random.Range(20, 100) + " puppies.</color>";
            }
            this.photonView.RPC("sendChatData", RpcTarget.All, chat);
            chatTextEnter.placeholder.GetComponent<Text>().text = "Press enter to chat...";
            chatTextEnter.text = "";
            typing = false;
        }

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chatTextEnter.enabled = false;
            chatTextEnter.enabled = true;
        }
    }

    public void loadGame()
    {
        SceneManager.LoadScene("MainGame");
    }
}
