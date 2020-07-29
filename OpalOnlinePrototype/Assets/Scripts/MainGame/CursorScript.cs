using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour {
    public Camera orthoCam;
    public DummyScript dummyPrefab;
    public DummyScript attackPrefab;
    public PathScript pathPrefab;
    private ReticleScript reticle;

    private GroundScript boardScript;
    private TextScript ts;


    private PathScript originPath;
    private Vector3 originLoc;
    private Vector3 lastPos;
    private Vector3 myPos;

    private TileScript tileFrom;
    private TileScript lastTile;
    private OpalScript selectedPlayer; //opal whose turn it is
    private Target targeter;
    private List<Target> targets = new List<Target>();
    private List<Vector2> pathBuilder = new List<Vector2>();
    private int currentTurn;
    private int distance = -1; //distance left for the opal on the current turn
    private int tdistance = -1; //temporary holder for distance, for generating custom paths
    private int attacking = -1;
    private bool startAttack = false;
    private bool finishAttack = false;
    private bool moving = false;
    private bool pathing = false;
    private int lastPath = 0;
    private string currentController;
    private int moveLag = 10;
    private bool keypadPressed = false;
    private string addon = "";
    private bool placing;
    private Projectile currentProj;
    private int reticleRotate = 0;
    private bool gameOver = false;
    private int roundNum = 0;
    private int fallState = 0;
    private GameObject warningPrefab;
    private List<GameObject> warningList = new List<GameObject>();
    private bool followup = false;
    private int currentOnlinePlayer = 0;
    private string lastCommand = "";

    // Use this for initialization
    void Start () {
        currentController = "keyboard";
        orthoCam = Camera.main;
        transform.position = new Vector3(0,0,0);
        GameObject board = GameObject.Find("Main Camera");
        ts = GameObject.Find("Canvas").GetComponent<TextScript>();
        reticle = GameObject.Find("Reticle").GetComponent<ReticleScript>();
        boardScript = board.GetComponent<GroundScript>();
        currentTurn = 0;
        boardScript.updateTurnOrder(currentTurn);
        targeter = Resources.Load<Target>("Prefabs/Targeter");
        warningPrefab = Resources.Load<GameObject>("Prefabs/Warning");
        placing = true;
        currentProj = Resources.Load<Projectile>("Prefabs/DefaultProjectile");
        Cursor.visible = false;
        ts.enableTileScreen(false, "", -1);
        boardScript.getMM().setUpMM(boardScript, this);
        boardScript.toggleMenu();
    }

    public int getCurrentOnlinePlayer()
    {
        return currentOnlinePlayer;
    }


    // Update is called once per frame
    void Update () {
        //initialize the first turn
        if(selectedPlayer == null && distance == -1)
        {
            StartCoroutine(ts.displayRoundNum(roundNum));
            selectedPlayer = boardScript.gameOpals[currentTurn];
            distance = selectedPlayer.getSpeed();
            boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
            ts.updateCurrent(selectedPlayer, distance);
            selectedPlayer.setMyTurn(true);
            currentController = boardScript.getCurrentController(selectedPlayer.getTeam());
            if(selectedPlayer.getTeam() == "Red")
            {
                currentOnlinePlayer = 1;
            }else if(selectedPlayer.getTeam() == "Green")
            {
                currentOnlinePlayer = 2;
            }
            else if (selectedPlayer.getTeam() == "Orange")
            {
                currentOnlinePlayer = 3;
            }
                if (currentController =="joystick 2")
            {
                addon = " 2";
            }else if(currentController == "joystick 3")
            {
                addon = " 3";
            }
            else if (currentController == "joystick 4")
            {
                addon = " 4";
            }
            else
            {
                addon = "";
            }
        }
        //initialize the first turn

        Vector3 mousePos = Input.mousePosition;
        lastPos = myPos;
        myPos = transform.position;
        moveReticle(currentController);
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Home" + addon))
        {
            boardScript.toggleMenu();
        }
        if (gameOver)
        {
            return;
        }
        moveCursor(myPos, orthoCam.WorldToScreenPoint(myPos), orthoCam.WorldToScreenPoint(reticle.transform.position));
        tileFrom = boardScript.tileGrid[(int)myPos.x, (int)myPos.z];
        if(tileFrom != lastTile)
        {
            if(tileFrom.currentPlayer != null)
            {
                if(tileFrom.currentPlayer.getPos().x != myPos.x || tileFrom.currentPlayer.getPos().z != myPos.z)
                {
                    tileFrom.standingOn(null);
                }
            }
            if (tileFrom.getImpassable())
            {
                if(tileFrom.currentPlayer == null && tileFrom.type != "Boulder" && !tileFrom.getFallen())
                {
                    tileFrom.setImpassable(false);
                }
            }
            lastTile = tileFrom;
        }

        if (targets.Count > 0)
        {
            foreach(Target t in targets)
            {
                t.setRelativeCoordinates((int)myPos.x, (int)myPos.z, attacking, selectedPlayer);
            }
        }
        ts.updateSelected(tileFrom.currentPlayer);
        ts.updateCurrent(selectedPlayer, distance);
        
        if (placing)
        {
            if (((Input.GetMouseButtonUp(0) && currentController == "keyboard") || ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("button 0" + addon))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
            {
                if (boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].getTeam() == selectedPlayer.getTeam() && boardScript.tileGrid[(int)transform.position.x, (int)transform.position.z].currentPlayer == null)
                {
                    if (boardScript.getMult())
                    {
                        boardScript.getMM().sendMultiplayerData("place," + selectedPlayer.getMyName() + "," + (int)myPos.x + "," + (int)myPos.z+","+currentOnlinePlayer);
                    }
                    selectedPlayer.setPos((int)myPos.x, (int)myPos.z);
                    selectedPlayer.setMyTurn(false);
                    selectedPlayer.updateTile();
                    selectedPlayer.getCurrentTile().standingOn(selectedPlayer);
                    nextTurn();
                }
            }
            return;
        }

        //End Turn
        if ((!followup && ((Input.GetKeyDown(KeyCode.Return) && currentController == "keyboard") || ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("RBump" + addon)))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            if (boardScript.getMult())
            {
                
                boardScript.getMM().sendMultiplayerData("end," + currentOnlinePlayer + "," + selectedPlayer.getMyName());
                lastCommand = "end," + currentOnlinePlayer+","+selectedPlayer.getMyName();
            }
            selectedPlayer.setMyTurn(false);
            nextTurn();
        }

        if (Input.GetKeyUp(KeyCode.Equals))
        {
            boardScript.updateFromString(boardScript.getMM().getGameData());
            boardScript.getMM().sendMultiplayerData("reset,"+currentOnlinePlayer);
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            foreach(OpalScript o in boardScript.gameOpals)
            {
                StartCoroutine(o.dancer());
            }
        }

        if ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("button 3" + addon))
        {
            reticle.transform.position = new Vector3(selectedPlayer.getPos().x, 0, selectedPlayer.getPos().z);
            reticle.transform.position += reticle.transform.forward * (-10 + 0.5f*selectedPlayer.getPos().x - 0.5f*selectedPlayer.getPos().z);
        }

        if ((!followup && (Input.GetMouseButtonUp(0) && currentController == "keyboard") || ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("button 0" + addon))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            if (pathing && boardScript.dummies[(int)myPos.x, (int)myPos.z] != null && boardScript.getPath((int)myPos.x, (int)myPos.z) != null )
            {
                /**List<Vector2> temp = Astar(new Vector2((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z), new Vector2(myPos.x, myPos.z));
                if (temp.Count > 0)
                {
                    temp.RemoveAt(0);
                }
                tdistance = 0;
                for (int i = 0; i < distance && i < temp.Count; i++)
                {
                    pathBuilder.Add(temp[i]);
                    tdistance++;
                }
                if (temp.Count > 0)
                {
                    originLoc = temp[tdistance - 1];
                }
                tdistance = selectedPlayer.getSpeed() - tdistance;
                pathing = true;*/
                //return;
            }
            /**
            boardScript.diplayPath(false);
            destroyDummies();
            attacking = -1;
            ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
            moving = false;
            tdistance = -1;
            */
            //display movement options
            if (distance > 0 && tileFrom.currentPlayer == selectedPlayer && boardScript.dummies[(int)myPos.x, (int)myPos.z] == null && attacking == -1) //check if tilefrom current player == selected player
            {
                DummyScript tempDummy = Instantiate<DummyScript>(dummyPrefab);
                tempDummy.setCoordinates((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z);
                boardScript.dummies[(int)myPos.x, (int)myPos.z] = tempDummy;
                boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
                tempDummy.Spawn(distance, 0, 0, true);
                moving = true;
                pathing = true;
            }
            //display movement options
        }
        if (pathing && lastPos != myPos)
        {
            destroyPath();

            PathScript tempPath = Instantiate<PathScript>(pathPrefab);
            tempPath.setCoordinates((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z);
            originPath = tempPath;
            if (tdistance == -1)
            {
                pathBuilder = new List<Vector2>();
                boardScript.paths.Add(tempPath);
                originPath = tempPath;
                pathing = true;
                //tempPath.SpawnPath(myPos);
                List<Vector2> temp = Astar(new Vector2((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z), new Vector2(myPos.x, myPos.z));
                if (temp.Count > 0)
                {
                    temp.RemoveAt(0);
                }
                for (int i = 0; i < distance && i < temp.Count; i++)
                {
                    PathScript pathPiece = Instantiate<PathScript>(tempPath);
                    pathPiece.setCoordinates((int)temp[i].x, (int)temp[i].y);
                    boardScript.paths.Add(pathPiece);
                }
                pathBuilder = new List<Vector2>();
                for (int i = 0; i < distance && i < temp.Count; i++)
                {
                    pathBuilder.Add(temp[i]);
                }
                if (boardScript.paths.Count > 0)
                {
                    //lastPath = boardScript.paths.Count;
                }
            }
            else
            {
                tempPath.setCoordinates((int)originLoc.x, (int)originLoc.y);
                boardScript.paths.Add(tempPath);
                originPath = tempPath;
                pathing = true;
                //tempPath.SpawnPath(myPos);
                List<Vector2> temp = Astar(new Vector2((int)originLoc.x, (int)originLoc.y), new Vector2(myPos.x, myPos.z));
                if (temp.Count > 0)
                {
                    temp.RemoveAt(0);
                    if (pathBuilder.Count > 0)
                    {
                        for (int i = 0; i < pathBuilder.Count; i++)
                        {
                            if (pathBuilder[i].x == originLoc.x && pathBuilder[i].y == originLoc.y)
                            {
                                for (int q = pathBuilder.Count - 1; q > i; q--)
                                {
                                    pathBuilder.RemoveAt(q);
                                }
                                break;
                            }
                        }
                    }
                    for (int i = 0; i < temp.Count && i < tdistance; i++)
                    {
                        pathBuilder.Add(temp[i]);
                    }
                }

                for (int i = 0; i < pathBuilder.Count; i++)
                {
                    PathScript pathPiece = Instantiate<PathScript>(tempPath);
                    pathPiece.setCoordinates((int)pathBuilder[i].x, (int)pathBuilder[i].y);
                    boardScript.paths.Add(pathPiece);
                }
            }
        }
        if ((!followup && (Input.GetMouseButtonUp(1) && currentController == "keyboard") || ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("button 2" + addon))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            boardScript.diplayPath(false);
            destroyDummies();
            attacking = -1;
            ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
            moving = false;
            tdistance = -1;
        }
        if((!followup && currentController == "keyboard" || (keypadPressed && Input.GetAxis("dpadUp" + addon) == 0 && Input.GetAxis("dpadRight" + addon) == 0)) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            keypadPressed = false;
        }
        if ((!followup && ((Input.GetKeyUp(KeyCode.Alpha1) && currentController == "keyboard") || (Input.GetAxis("dpadUp"+addon) > 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")) && !keypadPressed) && attacking == -1 && (!finishAttack || selectedPlayer.Attacks[0].getFreeAction())) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            keypadPressed = true;
            attacking = 0;
            selectedPlayer.prepAttack(0);
            startAttack = true;
        }
        else if ((!followup && ((Input.GetKeyUp(KeyCode.Alpha2) && currentController == "keyboard") || (Input.GetAxis("dpadRight"+ addon) > 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && !keypadPressed)) && attacking == -1 && (!finishAttack || selectedPlayer.Attacks[1].getFreeAction())) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            keypadPressed = true;
            attacking = 1;
            selectedPlayer.prepAttack(1);
            startAttack = true;
        }
        else if ((!followup && ((Input.GetKeyUp(KeyCode.Alpha4) && currentController == "keyboard") || (Input.GetAxis("dpadRight" + addon) < 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && !keypadPressed)) && attacking == -1 && (!finishAttack || selectedPlayer.Attacks[3].getFreeAction())) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            keypadPressed = true;
            attacking = 3;
            selectedPlayer.prepAttack(3);
            startAttack = true;
        }
        else if ((!followup && ((Input.GetKeyUp(KeyCode.Alpha3) && currentController == "keyboard") || (Input.GetAxis("dpadUp" + addon) < 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && !keypadPressed)) && attacking == -1 && (!finishAttack || selectedPlayer.Attacks[2].getFreeAction())) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            keypadPressed = true;
            attacking = 2;
            selectedPlayer.prepAttack(2);
            startAttack = true;
        }
        else if((!followup && !keypadPressed && (((Input.GetKeyUp(KeyCode.Alpha1) && currentController == "keyboard") || (Input.GetAxis("dpadUp" + addon) > 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")))
            || (((Input.GetKeyUp(KeyCode.Alpha2) || (Input.GetKeyUp(KeyCode.Alpha4))) && currentController == "keyboard") || (Input.GetAxis("dpadRight" + addon) != 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")))
            || ((Input.GetKeyUp(KeyCode.Alpha3) && currentController == "keyboard") || (Input.GetAxis("dpadUp" + addon) < 0 && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4"))))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer)) //bug here i think
        {
            keypadPressed = true;
            destroyDummies();
            attacking = -1;
            selectedPlayer.prepAttack(-1);
            startAttack = false;
            ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
        }

        if(attacking > -1 && myPos != lastPos)
        {
            ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
        }

        if ((Input.GetKey(KeyCode.LeftShift) && currentController == "keyboard") || (Input.GetButton("LBump" + addon) && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")))
        {
            ts.checkBuffs(selectedPlayer, tileFrom.currentPlayer);
            if(attacking == -1)
                ts.displayAttacks(selectedPlayer, tileFrom.currentPlayer);
            foreach(OpalScript o in boardScript.gameOpals)
            {
                o.showSpot(true);
            }
            ts.enableTileScreen(true, tileFrom.type, tileFrom.getLife());
        }
        if ((Input.GetKeyUp(KeyCode.LeftShift) && currentController == "keyboard") || (Input.GetButtonUp("LBump" + addon) && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")))
        {
            ts.displayAttacks(null, null);
            foreach (OpalScript o in boardScript.gameOpals)
            {
                o.showSpot(false);
            }
            ts.enableTileScreen(false, "", -1);
        }


        if (attacking != -1 && transform.position != lastPos)
        {
            handleTargets();
        }

        //display attack options
        if (startAttack == true)
        {
            boardScript.diplayPath(false);
            ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
            if (moving == true)
            {
                destroyDummies();
                moving = false;
                tdistance = -1;
            }
            if (attacking != -1)
            {
                //check if it's a passive attack
                if (!(selectedPlayer.Attacks[attacking].getBaseDamage() == 0 && selectedPlayer.Attacks[attacking].getRange() == 0 && selectedPlayer.Attacks[attacking].getShape() == 0))
                {
                    DummyScript tempDummy = Instantiate<DummyScript>(attackPrefab);
                    tempDummy.setCoordinates((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z);
                    boardScript.dummies[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z] = tempDummy;
                    tempDummy.Spawn(selectedPlayer.Attacks[attacking].getRange(), 0, selectedPlayer.Attacks[attacking].getShape(), true);
                    startAttack = false;
                    handleTargets();
                }
                else
                {
                    startAttack = false;
                }
            }
            else
            {
                destroyDummies();
                attacking = -1;
                ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
                startAttack = false;
            }
        }
        //display attack options

        if(((Input.GetMouseButtonUp(0) && currentController == "keyboard") || ((currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4") && Input.GetButtonUp("button 0" + addon))) && (!boardScript.getMult() || boardScript.getOnlineTeam() == currentOnlinePlayer))
        {
            //move player
            if (distance > 0 && boardScript.dummies[(int)myPos.x, (int)myPos.z] != null && boardScript.tileGrid[(int)myPos.x, (int)myPos.z].currentPlayer == null && attacking == -1 && boardScript.getPath((int)myPos.x, (int)myPos.z) != null) 
            {
                for (int i = 0; i < pathBuilder.Count; i++)
                {
                    if (boardScript.tileGrid[(int)pathBuilder[i].x, (int)pathBuilder[i].y].type == "Fire" || boardScript.tileGrid[(int)pathBuilder[i].x, (int)pathBuilder[i].y].type == "Miasma")
                    {
                        if (boardScript.tileGrid[(int)pathBuilder[i].x, (int)pathBuilder[i].y].type == "Fire")
                        {
                            //selectedPlayer.setBurning(true);
                        }
                        else
                        {
                            //selectedPlayer.setPoison(true);
                        }
                    }
                    if(boardScript.tileGrid[(int)pathBuilder[i].x, (int)pathBuilder[i].y].getTrap() != null)
                    {
                        //boardScript.tileGrid[(int)pathBuilder[i].x, (int)pathBuilder[i].y].doTrap(selectedPlayer);
                    }
                }
                lastPath = boardScript.paths.Count;
                tileFrom.standingOn(null);
                if (selectedPlayer != null)
                {
                    boardScript.tileGrid[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z].standingOn(null);
                    int actualPath = selectedPlayer.doFullMove(boardScript.paths, lastPath);
                    if (boardScript.getMult() == true)
                    {
                        string parsePath = "";
                        foreach (PathScript p in boardScript.paths)
                        {
                            parsePath += p.getPos().x + "_" + p.getPos().z + ",";
                        }
                        boardScript.getMM().sendFullGameData(boardScript.generateString());
                        boardScript.getMM().sendMultiplayerData("move," + selectedPlayer.getMyName() + "," + currentOnlinePlayer + "," + parsePath);
                        lastCommand = "move," + selectedPlayer.getMyName() + "," + currentOnlinePlayer + "," + parsePath;
                    }
                    if (tdistance == -1)
                    {
                        distance -= actualPath;
                    }
                    else
                    {
                        distance = tdistance - actualPath;
                    }
                    destroyDummies();
                    boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)(int)selectedPlayer.getPos().z);
                    ts.updateCurrent(selectedPlayer, distance);
                    boardScript.diplayPath(false);
                    moving = false;
                    tdistance = -1;
                    

                }
            }
            //move player

            //make attack
            if(attacking != -1 && (!finishAttack || selectedPlayer.Attacks[attacking].getFreeAction()) &&  boardScript.dummies[(int)myPos.x, (int)myPos.z] != null && !selectedPlayer.getBanned().Contains(attacking) && notAllTargetsNull())
            {
                bool targetedSelf = false;
                //check to make sure they don't target themselves
                if((boardScript.tileGrid[(int)myPos.x, (int)myPos.z].currentPlayer == selectedPlayer && selectedPlayer.getAttacks()[attacking].getRange() != 0))
                {
                    targetedSelf = true;
                }
                foreach (Target t in targets)
                {
                    if (t.getTile() != null)
                    {
                        OpalScript target = t.getTile().currentPlayer;
                        if (target != null && targetedSelf == false)
                        {
                            selectedPlayer.Attacks[attacking].getCurrentUse(1);
                            if (followup)
                            {
                                selectedPlayer.onFollowUp(attacking);
                            }
                            if (boardScript.getMult())
                            {
                                boardScript.getMM().sendMultiplayerData("attack," + currentOnlinePlayer + "," + t.getTile().getPos().x + "," + t.getTile().getPos().z + "," + attacking);
                                lastCommand = "attack," + currentOnlinePlayer + "," + t.getTile().getPos().x + "," + t.getTile().getPos().z + "," + attacking;
                            }
                            Projectile tempProj = Instantiate(currentProj);
                            tempProj.setUp(selectedPlayer.getAttacks()[attacking].getShape(),  selectedPlayer.getMainType());
                            selectedPlayer.adjustProjectile(tempProj, attacking);
                            tempProj.fire(selectedPlayer, target, attacking);
                            if (selectedPlayer.getCurrentTile() != null && selectedPlayer.getCurrentTile().type == "Fire")
                            {
                                target.setBurning(true);
                            }
                        }
                        else if(targetedSelf == false)
                        {
                            selectedPlayer.Attacks[attacking].getCurrentUse(1);
                            if (followup)
                            {
                                selectedPlayer.onFollowUp(attacking);
                            }
                            if (boardScript.getMult())
                            {
                                boardScript.getMM().sendMultiplayerData("attack," + currentOnlinePlayer + "," + t.getTile().getPos().x + "," + t.getTile().getPos().z + "," + attacking);
                                lastCommand = "attack," + currentOnlinePlayer + "," + t.getTile().getPos().x + "," + t.getTile().getPos().z + "," + attacking;
                            }
                            Projectile tempProj = Instantiate(currentProj);
                            tempProj.setUp(selectedPlayer.getAttacks()[attacking].getShape(), selectedPlayer.getMainType());
                            selectedPlayer.adjustProjectile(tempProj, attacking);
                            tempProj.fire(selectedPlayer, t.getTile(), attacking);
                            if(t.getTile().type == "Boulder" && selectedPlayer.getAttacks()[attacking].getBaseDamage() > 0)
                            {
                                boardScript.setTile((int)t.getTile().getPos().x, (int)t.getTile().getPos().z, "Grass", true);
                            }
                            
                        }
                    }
                }
                if (targetedSelf)
                {
                }
                else
                {
                    if (!selectedPlayer.getAttackAgain() && selectedPlayer.Attacks[attacking].getUses() <= selectedPlayer.Attacks[attacking].getCurrentUse(0) && !selectedPlayer.Attacks[attacking].getFreeAction())
                    { 
                        finishAttack = true;
                    }
                    if (!selectedPlayer.getMoveAfter() && !selectedPlayer.Attacks[attacking].getFreeAction())
                    {
                        distance = 0;
                    }
                    if(selectedPlayer.Attacks[attacking].getUses() > selectedPlayer.Attacks[attacking].getCurrentUse(0))
                    {
                        followup = true;
                    }
                    else
                    {
                        attacking = -1;
                        destroyDummies();
                        followup = false;
                    }
                    ts.updateCurrent(selectedPlayer, distance);
                }
                //nextTurn();
            }
            //make attack
        }
    }

    public void updateData()
    {
        if(boardScript.getMult())
            boardScript.getMM().sendFullGameData(boardScript.generateString());
    }

    public void setCurrentOpal(OpalScript o)
    {
        selectedPlayer = o;
        selectedPlayer.setMyTurn(true);
        boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
        if (selectedPlayer.getTeam() == "Red")
        {
            currentOnlinePlayer = 1;
        }
        else if (selectedPlayer.getTeam() == "Green")
        {
            currentOnlinePlayer = 2;
        }
        else if (selectedPlayer.getTeam() == "Orange")
        {
            currentOnlinePlayer = 3;
        }
        else
        {
            currentOnlinePlayer = 0;
        }

        distance = selectedPlayer.getSpeed();
        boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
        boardScript.setChargeDisplay(selectedPlayer.getCharge());
        ts.updateCurrent(selectedPlayer, distance);
        currentController = boardScript.getCurrentController(selectedPlayer.getTeam());

        if (currentController == "joystick 2")
        {
            addon = " 2";
        }
        else if (currentController == "joystick 3")
        {
            addon = " 3";
        }
        else if (currentController == "joystick 4")
        {
            addon = " 4";
        }
        else
        {
            addon = "";
        }
    }

    public bool notAllTargetsNull()
    {
        foreach(Target t in targets)
        {
            if (t.getTile() != null)
                return true;
        }
        return false;
    }

    public void restartAttack()
    {
        
        if (attacking != -1)
        {
            destroyDummies();
            DummyScript tempDummy = Instantiate<DummyScript>(attackPrefab);
            tempDummy.setCoordinates((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z);
            boardScript.dummies[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z] = tempDummy;
            tempDummy.Spawn(selectedPlayer.Attacks[attacking].getRange(), 0, selectedPlayer.Attacks[attacking].getShape(), true);
            startAttack = false;
            handleTargets();
        }
    }

    public void nextTurn()
    {
        boardScript.recallParticles();
        selectedPlayer.handleTempBuffs(true);
        if(!placing)
            selectedPlayer.onEnd();
        if (selectedPlayer.getCurrentTile() != null)
        {
            if(selectedPlayer.getDead())
                selectedPlayer.getCurrentTile().setImpassable(false);
            else
                selectedPlayer.getCurrentTile().setImpassable(true);
        }
        currentTurn += 1;
        if (currentTurn >= boardScript.gameOpals.Count)
        {
            if (gameOver)
            {
                return;
            }
            roundNum++;
            currentTurn = 0;
            foreach(TileScript t in boardScript.tileGrid)
            {
                t.setForUpdate();
            }
            foreach (TileScript t in boardScript.tileGrid)
            {
                t.updateTile();
            }
            if (roundNum > 4 && roundNum % 3 == 2)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if ((i == fallState || j == fallState || i == 9 - fallState || j == 9 - fallState) && (i >= fallState && i <= 9 - fallState) && (j >= fallState && j <= 9 - fallState))
                        {
                            GameObject temp = Instantiate<GameObject>(warningPrefab);
                            temp.transform.position = new Vector3(i, 0.02f, j);
                            warningList.Add(temp);
                        }
                     }
                }
            }
            if (roundNum > 5 && roundNum % 3 == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (i == fallState || j == fallState || i == 9 - fallState || j == 9 - fallState)
                            StartCoroutine(boardScript.tileGrid[i, j].fall());
                    }
                }
                foreach (GameObject w in warningList){
                    DestroyImmediate(w);
                }
                warningList.Clear();

                fallState++;
            }
            boardScript.sortOpals(boardScript.gameOpals);
            if (placing)
            {
                placing = false;
                foreach(TileScript t in boardScript.tileGrid)
                {
                    t.toggleStarting();
                }
                foreach(OpalScript o in boardScript.gameOpals)
                {
                    
                    o.onPlacement();
                }
            }
            StartCoroutine(ts.displayRoundNum(roundNum));
        }
        moving = false;
        attacking = -1;
        ts.updateAttackScreen(selectedPlayer, attacking, boardScript.tileGrid[(int)myPos.x, (int)myPos.z]);
        selectedPlayer = boardScript.gameOpals[currentTurn];
        boardScript.updateTurnOrder(currentTurn);
        if (selectedPlayer.getDead() == true)
        {
            nextTurn();
            return;
        }
        if(roundNum > 0)
            checkWin(); 
        if (selectedPlayer.getSkipTurn())
        {
            selectedPlayer.setSkipTurn(false);
            nextTurn();
            return;
        }
        if(((selectedPlayer.getPos().x >= 0 && selectedPlayer.getPos().x <= 9) && (selectedPlayer.getPos().z >= 0 && selectedPlayer.getPos().z <= 9)) && boardScript.tileGrid[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z].currentPlayer == null)
        {
            //boardScript.tileGrid[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z].standingOn(selectedPlayer);
        }
        selectedPlayer.setMyTurn(true);
        selectedPlayer.StartOfTurn();
        if (selectedPlayer.getTeam() == "Red")
        {
            currentOnlinePlayer = 1;
        }
        else if (selectedPlayer.getTeam() == "Green")
        {
            currentOnlinePlayer = 2;
        }
        else if (selectedPlayer.getTeam() == "Orange")
        {
            currentOnlinePlayer = 3;
        }
        else
        {
            currentOnlinePlayer = 0;
        }
        if (selectedPlayer.getDead() == true)
        {
            nextTurn();
            return;
        }
        
        if (selectedPlayer.getCurrentTile() != null)
            selectedPlayer.getCurrentTile().setImpassable(false);
        //selectedPlayer.handleTempBuffs(true);
        distance = selectedPlayer.getSpeed();
        boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
        destroyDummies();
        boardScript.diplayPath(false);
        boardScript.setChargeDisplay(selectedPlayer.getCharge());
        ts.updateCurrent(selectedPlayer, distance);
        currentController = boardScript.getCurrentController(selectedPlayer.getTeam());
        foreach(TileScript t in boardScript.tileGrid)
        {
            t.fixWeirdness();
        }
        if (currentController == "joystick 2")
        {
            addon = " 2";
        }
        else if (currentController == "joystick 3")
        {
            addon = " 3";
        }
        else if (currentController == "joystick 4")
        {
            addon = " 4";
        }
        else
        {
            addon = "";
        }
        finishAttack = false;
        tdistance = -1;
        if (roundNum > 0 && currentController != "keyboard")
        {
            reticle.transform.position = new Vector3(selectedPlayer.getPos().x, 0, selectedPlayer.getPos().z);
            reticle.transform.position += reticle.transform.forward * (-10 + 0.5f * selectedPlayer.getPos().x - 0.5f * selectedPlayer.getPos().z);
        }
        if (roundNum > 0 && currentController != "keyboard" && distance > 0) //check if tilefrom current player == selected player
        {
            DummyScript tempDummy = Instantiate<DummyScript>(dummyPrefab);
            tempDummy.setCoordinates((int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z);
            boardScript.dummies[(int)myPos.x, (int)myPos.z] = tempDummy;
            boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)selectedPlayer.getPos().z);
            tempDummy.Spawn(distance, 0, 0, true);
            moving = true;
            pathing = true;
        }
        if (boardScript.getMult())
        {
            //boardScript.getMM().sendFullGameData(boardScript.generateString());
            //boardScript.getMM().sendMultiplayerData("reset," + currentOnlinePlayer);
        }
    }


    private void dealDamage(OpalScript target)
    {
        target.takeDamage(selectedPlayer.getAttackEffect(attacking, target), true, true);
    }

    public void placeOpal(int x, int y)
    {
        //print("Placing Opal");
        selectedPlayer.setPos(x, y);
        selectedPlayer.setMyTurn(false);
        selectedPlayer.updateTile();
        selectedPlayer.getCurrentTile().standingOn(selectedPlayer);
        nextTurn();
    }

    public void doMove(List<PathScript> paths)
    {
        print("moving Opal");
        if (selectedPlayer != null)
        {
            boardScript.tileGrid[(int)selectedPlayer.getPos().x, (int)selectedPlayer.getPos().z].standingOn(null);
            int actualPath = selectedPlayer.doFullMove(paths, paths.Count);
            if (tdistance == -1)
            {
                distance -= actualPath;
            }
            else
            {
                distance = tdistance - actualPath;
            }
            boardScript.spotlight.transform.position = new Vector3((int)selectedPlayer.getPos().x, 2, (int)(int)selectedPlayer.getPos().z);
            ts.updateCurrent(selectedPlayer, distance);
            foreach (PathScript p in paths)
            {
                DestroyImmediate(p.gameObject);
            }
        }
    }

    public void doAttack(int x, int y, int at)
    {
        selectedPlayer.prepAttack(at);
        Projectile tempProj = Instantiate(currentProj);
        tempProj.setUp(selectedPlayer.getAttacks()[at].getShape(), selectedPlayer.getMainType());
        selectedPlayer.adjustProjectile(tempProj, at);
        if (boardScript.tileGrid[x, y].currentPlayer != null)
        {
            tempProj.fire(selectedPlayer, boardScript.tileGrid[x, y].currentPlayer, at);
        }
        else
        {
            tempProj.fire(selectedPlayer, boardScript.tileGrid[x, y], at);
        }
        if (boardScript.tileGrid[x, y].type == "Boulder" && selectedPlayer.getAttacks()[at].getBaseDamage() > 0)
        {
            boardScript.setTile(x, y, "Grass", true);
        }
    }

    public void pressEndTurn()
    {
        selectedPlayer.setMyTurn(false);
        nextTurn();
    }

    private void destroyDummies()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (boardScript.dummies[i, j] != null)
                {
                    Destroy(boardScript.dummies[i, j].gameObject);
                    boardScript.dummies[i, j] = null;
                }
            }
        }
        foreach(Target t in targets)
        {
            DestroyImmediate(t.gameObject);
        }
        targets.Clear();
        destroyPath();
    }

    private void destroyPath()
    {
        foreach(PathScript p in boardScript.paths){
            DestroyImmediate(p.gameObject);
        }
        pathing = false;
        //originPath = null;
        boardScript.paths.Clear();
    }

    private int pathPoll()
    {
        int pp = 0;
        foreach(PathScript p in boardScript.paths)
        {
            if (p.doBurn)
            {
                if (pp == 0)
                    pp = 1;
                else if (pp == 2)
                    pp = 3;
            }
            if (p.doPoison)
            {
                if (pp == 0)
                    pp = 2;
                else if (pp == 1)
                    pp = 3;
            }
        }
        foreach (PathScript p in boardScript.generatedPaths)
        {
            if (p.doBurn)
            {
                if (pp == 0)
                    pp = 1;
                else if (pp == 2)
                    pp = 3;
            }
            if (p.doPoison)
            {
                if (pp == 0)
                    pp = 2;
                else if (pp == 1)
                    pp = 3;
            }
        }
        return pp;
    }

    private void moveCursor(Vector3 myPos, Vector3 myPosCam, Vector3 mousePos)
    {
        if (true)
        {
            int xChange = 0;
            int zChange = 0;
            if (myPosCam.x < mousePos.x - 35 && myPosCam.y < mousePos.y - 35 && myPos.z < 9)
            {
                zChange = 1;
            }
            else if (myPosCam.x > mousePos.x + 15 && myPosCam.y > mousePos.y + 15 && myPos.z > 0)
            {
                zChange = -1;
            }
            else if (myPosCam.x < mousePos.x - 35 && myPosCam.y > mousePos.y + 15 && myPos.x < 9)
            {
                xChange = 1;
            }
            else if (myPosCam.x > mousePos.x + 15 && myPosCam.y < mousePos.y - 35 && myPos.x > 0)
            {
                xChange = -1;
            }
            else if (myPosCam.x < mousePos.x - 65 && (myPos.x < 9 && myPos.z < 9))
            {
                xChange = 1;
                zChange = 1;
            }
            else if (myPosCam.x > mousePos.x + 65 && (myPos.x > 0 && myPos.z > 0))
            {
                xChange = -1;
                zChange = -1;
            }
            else if (myPosCam.y < mousePos.y - 50 && (myPos.x > 0 && myPos.z < 9))
            {
                xChange = -1;
                zChange = 1;
            }
            else if (myPosCam.y > mousePos.y + 50 && (myPos.x < 9 && myPos.z > 0))
            {
                xChange = 1;
                zChange = -1;
            }
            if (!boardScript.tileGrid[(int)myPos.x + xChange, (int)myPos.z + zChange].getFallen()) 
                transform.localPosition = new Vector3(myPos.x + xChange, myPos.y, myPos.z + zChange);
            if(boardScript.tileGrid[(int)myPos.x, (int)myPos.z].getFallen())
            {
                transform.localPosition = new Vector3(5, myPos.y, 5);
            }
        }
    }

    private void handleTargets()
    {
        foreach (Target t in targets)
        {
            DestroyImmediate(t.gameObject);
        }
        targets.Clear();
        Target tempTarget = Instantiate<Target>(targeter);
        tempTarget.setCoordinates((int)transform.position.x, (int)transform.position.z, attacking, selectedPlayer);
        targets.Add(tempTarget);
        int targetShape = selectedPlayer.Attacks[attacking].getShape();
        int targetRange = selectedPlayer.Attacks[attacking].getAOE();
        if (targetShape == 6)
        {
            if (tempTarget.transform.position.x > selectedPlayer.getPos().x)
            {
                targetShape = 60;
                targetRange = 10 - (int)selectedPlayer.getPos().x;
            }
            else if (tempTarget.transform.position.x < selectedPlayer.getPos().x) //this gets run when it shouldn't be
            {
                targetShape = 61;
                targetRange = (int)selectedPlayer.getPos().x;
            }
            else if (tempTarget.transform.position.z > selectedPlayer.getPos().z)
            {
                targetShape = 62;
                targetRange = 10 - (int)selectedPlayer.getPos().z;
            }
            else if (tempTarget.transform.position.z < selectedPlayer.getPos().z)
            {
                targetShape = 63;
                targetRange = (int)selectedPlayer.getPos().z;
            }
            else
            {
            }
        }
        tempTarget.Spawn(targetRange, targetShape, targets);
        
    }

    public void checkWin()
    {
        int redAlive = 0;
        int blueAlive = 0;
        int greenAlive = 0;
        int orangeAlive = 0;
        foreach(OpalScript o in boardScript.gameOpals)
        {
            if(o.getDead() == false)
            {
                if(o.getTeam() == "Red")
                {
                    redAlive++;
                }else if(o.getTeam() == "Blue")
                {
                    blueAlive++;
                }else if(o.getTeam() == "Green")
                {
                    greenAlive++;
                }
                else if (o.getTeam() == "Orange")
                {
                    orangeAlive++;
                }
            }
        }
        if (redAlive + greenAlive + orangeAlive == 0 && blueAlive > 0)
        {
            ts.doWin("Blue");
            gameOver = true;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                StartCoroutine(o.dancer());
            }
        }
        else if(blueAlive + greenAlive + orangeAlive == 0 && redAlive > 0)
        {
            ts.doWin("Red");
            gameOver = true;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                StartCoroutine(o.dancer());
            }
        }
        else if (blueAlive + redAlive + orangeAlive == 0 && greenAlive > 0)
        {
            ts.doWin("Green");
            gameOver = true;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                StartCoroutine(o.dancer());
            }
        }
        else if (blueAlive + greenAlive + redAlive == 0 && orangeAlive > 0)
        {
            ts.doWin("Orange");
            gameOver = true;
            foreach (OpalScript o in boardScript.gameOpals)
            {
                StartCoroutine(o.dancer());
            }
        }
        else if(redAlive + blueAlive + greenAlive + orangeAlive == 0)
        {
            ts.doWin("Tie");
            gameOver = true;
        }
    }

    private void moveReticle(string controller)
    {
        Vector3 rlastPos = reticle.transform.position;
        if (controller == "keyboard")
        {
            Vector3 mousePosRel = orthoCam.ScreenToWorldPoint(Input.mousePosition);
            reticle.transform.position = new Vector3(mousePosRel.x - 2, mousePosRel.y - 2, mousePosRel.z + 2);
        }else
        {
            string addon = "";
            if (currentController == "joystick 2")
            {
                addon = " 2";
            }
            else if (currentController == "joystick 3")
            {
                addon = " 3";
            }
            else if (currentController == "joystick 4")
            {
                addon = " 4";
            }
            else
            {
                addon = "";
            }
            float newX = 0f;
            float newY = 0f;
            if (Input.GetAxis("LSUp" + addon) < 0 && Input.GetAxis("LSRight" + addon) > 0)
            {
                newY += 0.5f;
            }
            else if (Input.GetAxis("LSUp" + addon) > 0 && Input.GetAxis("LSRight" + addon) < 0)
            {
                 newY -= 0.5f;
            }
            else if (Input.GetAxis("LSRight" + addon) > 0 && Input.GetAxis("LSUp" + addon) > 0)
            {
                    newX += 0.5f;
            }
            else if (Input.GetAxis("LSRight" + addon) < 0 && Input.GetAxis("LSUp" + addon) < 0)
            {
                    newX -= 0.5f;
            }
            else if (Input.GetAxis("LSRight" + addon) > 0)
            {
                    newX += 0.4f;
                    newY += 0.4f;
            }
            else if (Input.GetAxis("LSRight" + addon) < 0)
            {
                    newX -= 0.4f;
                    newY -= 0.4f;
            }
            else if (Input.GetAxis("LSUp" + addon) < 0)
            {
                    newX -= 0.2f;
                    newY += 0.2f;
            }
            else if (Input.GetAxis("LSUp" + addon) > 0)
            {
                    newX += 0.2f;
                    newY -= 0.2f;
            }
            newX *= Mathf.Abs(reticleRotate) * 0.05f + 1f;
            newY *= Mathf.Abs(reticleRotate) * 0.05f + 1f;
            reticle.transform.position = new Vector3(reticle.transform.position.x + newX * 0.15f, reticle.transform.position.y - newX * 0.15f + newY * 0.15f, reticle.transform.position.z + newY * 0.15f);
        }
        if (rlastPos.x < reticle.transform.position.x)
        {
            if (reticleRotate < 20)
            {
                if(reticleRotate < 0)
                {
                    reticleRotate = 0;
                    reticle.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                reticle.transform.Rotate(0, 0, -1);
                reticleRotate++;
            }
        }
        else if (rlastPos.x > reticle.transform.position.x)
        {
            if (reticleRotate > -20)
            {
                if(reticleRotate > 0)
                {
                    reticleRotate = 0;
                    reticle.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                reticle.transform.Rotate(0, 0, 1);
                reticleRotate--;
            }
        }
        else if (reticle.transform.position == rlastPos)
        {
            if (reticleRotate < 0)
            {
                reticle.transform.Rotate(0, 0, -1);
                reticleRotate++;
            }else if(reticleRotate > 0)
            {
                reticle.transform.Rotate(0, 0, 1);
                reticleRotate--;
            }
        }
        if (reticle.getOpen() && ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && currentController == "keyboard") || ((Input.GetButton("button 0" + addon) || (Input.GetButton("button 1" + addon)) && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4"))))
        {
            reticle.setState(false);
        }
        else if (!reticle.getOpen() && ((!Input.GetMouseButton(0) && !Input.GetMouseButton(1)) && currentController == "keyboard") || (((!Input.GetButton("button 0" + addon) && (!Input.GetButton("button 1" + addon)) && (currentController == "joystick 1" || currentController == "joystick 2" || currentController == "joystick 3" || currentController == "joystick 4")))))
        {
            reticle.setState(true);
        }
    }

    public OpalScript getCurrentOpal()
    {
        return selectedPlayer;
    }

    public int getDistance()
    {
        return distance;
    }

    private int getAliveCount(List<OpalScript> opals)
    {
        int i = 0;
        foreach(OpalScript o in opals)
        {
            if(o.getDead() == true)
            {
                i++;
            }
        }
        return i;
    }

    public void doAttack()
    {

    }

    public void doMove(int x, int y)
    {
        //selectedPlayer.doMove(x,y, );
    }

    public void doEndTurn()
    {
        selectedPlayer.setMyTurn(false);
        nextTurn();
    }

    private List<Vector2> Astar(Vector2 start, Vector2 end)
    {
        int attemptCounter = 0;

        if (boardScript.tileGrid[(int)end.x, (int)end.y].getImpassable())
        {
            return new List<Vector2>();
        }
        if ((int)end.x == 0 || (int)end.y == 0 || (int)end.x == boardScript.tileGrid.GetLength(0) - 1 || (int)end.y == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x - 1, (int)end.y].getFallen() || boardScript.tileGrid[(int)end.x + 1, (int)end.y].getFallen() || boardScript.tileGrid[(int)end.x, (int)end.y - 1].getFallen() || boardScript.tileGrid[(int)end.x, (int)end.y + 1].getFallen())
        {
            if (((int)end.y == 0 || boardScript.tileGrid[(int)end.x, (int)end.y - 1].getFallen()) && ((int)end.x == 0 || boardScript.tileGrid[(int)end.x - 1, (int)end.y].getFallen()))
            {
                if (boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable() && boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if (((int)end.y == 0 || boardScript.tileGrid[(int)end.x, (int)end.y - 1].getFallen()) && ((int)end.x == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x + 1, (int)end.y].getFallen()))
            {
                if (boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable() && boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if (((int)end.y == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x, (int)end.y + 1].getFallen()) && ((int)end.x == 0 || boardScript.tileGrid[(int)end.x - 1, (int)end.y].getFallen()))
            {
                if (boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable() && boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if (((int)end.y == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x, (int)end.y + 1].getFallen()) && ((int)end.x == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x + 1, (int)end.y].getFallen()))
            {
                if (boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable() && boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if ((int)end.x == 0 || boardScript.tileGrid[(int)end.x - 1, (int)end.y].getFallen())
            {
                if (boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if ((int)end.x == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x + 1, (int)end.y].getFallen())
            {
                if (boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if ((int)end.y == 0 || boardScript.tileGrid[(int)end.x, (int)end.y - 1].getFallen())
            {
                if (boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
            else if ((int)end.y == boardScript.tileGrid.GetLength(0) - 1 || boardScript.tileGrid[(int)end.x, (int)end.y + 1].getFallen())
            {
                if (boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable())
                {
                    return new List<Vector2>();
                }
            }
        }
        else if (boardScript.tileGrid[(int)end.x + 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x - 1, (int)end.y].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y - 1].getImpassable() && boardScript.tileGrid[(int)end.x, (int)end.y + 1].getImpassable())
        {
            return new List<Vector2>();
        }

        PathingNode startNode = new PathingNode(null, start);
        startNode.g = startNode.h = startNode.f = 0;
        PathingNode endNode = new PathingNode(null, end);
        endNode.g = endNode.h = endNode.f = 0;

        List<PathingNode> openList = new List<PathingNode>();
        List<PathingNode> closedList = new List<PathingNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            int currentIndex = 0;
            PathingNode currentNode = openList[0];
            attemptCounter++;

            if (attemptCounter >= 500)
            {
                Debug.Log("Something went wrong with the A* pathing");
                return new List<Vector2>();
            }

            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].f < currentNode.f)
                {
                    currentNode = openList[i];
                    currentIndex = i;
                }
            }

            openList.RemoveAt(currentIndex);
            closedList.Add(currentNode);

            if (currentNode.position == endNode.position)
            {
                List<Vector2> path = new List<Vector2>();
                PathingNode current = currentNode;

                while (current != null)
                {
                    path.Add(current.position);
                    current = current.parent;
                }

                path.Reverse();
                return path;
            }

            List<PathingNode> children = new List<PathingNode>();
            Vector2[] newPositions = { new Vector2(0, -1), new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0) };
            Vector2 nodePosition;

            for (int i = 0; i < newPositions.Length; i++)
            {
                nodePosition = new Vector2(currentNode.position.x + newPositions[i].x, currentNode.position.y + newPositions[i].y);

                if (nodePosition.x > 9 || nodePosition.x < 0 || nodePosition.y > 9 || nodePosition.y < 0)
                {
                    continue;
                };

                if (boardScript.tileGrid[(int)nodePosition.x, (int)nodePosition.y].getImpassable())
                {
                    continue;
                }

                if (closedList.Contains(new PathingNode(currentNode, nodePosition)))
                {
                    continue;
                }

                children.Add(new PathingNode(currentNode, nodePosition));
            }

            for (int i = 0; i < children.Count; i++)
            {
                for (int q = 0; q < closedList.Count; q++)
                {
                    if (children[i].position == closedList[q].position)
                    {
                        goto childrenLoopContinue;
                    }
                }

                children[i].g = currentNode.g + 1;
                children[i].h = Mathf.Abs(children[i].position.x - endNode.position.x) + Mathf.Abs(children[i].position.y - endNode.position.y);
                children[i].f = children[i].g + children[i].h;

                for (int q = 0; q < openList.Count; q++)
                {
                    if (children[i].position == openList[q].position && children[i].g >= openList[q].g)
                    {
                        goto childrenLoopContinue;
                    }
                }

                openList.Add(children[i]);
                childrenLoopContinue:;
            }
        }
        return new List<Vector2>();
    }
}

public class PathingNode
{
    public PathingNode parent;
    public Vector2 position;
    public float g, h, f;

    public PathingNode(PathingNode par, Vector2 pos)
    {
        this.parent = par;
        this.position = pos;
        this.g = this.h = this.f = 0;
    }
}
