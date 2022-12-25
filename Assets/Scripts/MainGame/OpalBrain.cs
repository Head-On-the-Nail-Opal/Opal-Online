﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpalBrain : MonoBehaviour
{
    private GroundScript mainGame;
    private CursorScript myCursor;

    private List<Behave> generalBehaviours = new List<Behave>();

    private TileScript attackTarget;
    private TileScript moveTarget;
    private int attackNum = -1;

    private bool startOfTurn = true;
    private bool moved = false;

    private Coroutine currentAction;

    public TileScript[,] snapshotTileGrid = new TileScript[10, 10]; 

    private List<List<int>> moves = new List<List<int>>();
    private List<List<int>> bestMoves = new List<List<int>>();

    private List<Vector3> allPossibleAbilities = new List<Vector3>();
    private List<Vector2> possibleTargets = new List<Vector2>();

    public void Awake()
    {
        mainGame = GameObject.FindObjectOfType<GroundScript>();
    }


    //myCursor.doMove(List<Pathscript> paths)
    //myCursor.doAttack(int x, int y, int attackNum)
    //myCursor.doEndTurn()
    public void Update()
    {
        if (myCursor == null)
            myCursor = mainGame.getMyCursor();
        else
        {
            if (myCursor.getCurrentController() == "AI")
            {
                if (myCursor.getGameOver())
                    return;
                if (startOfTurn)
                    moved = false;
                if (currentAction == null)
                    currentAction = StartCoroutine(doRandomMove());
            }
        }
    }

    private IEnumerator doRandomMove()
    {
        if(findBestRandomAbility(true))
            yield return new WaitForSeconds(1f);
        findBestRandomMove();
        yield return new WaitForSeconds(1f);
        if (findBestRandomAbility(true))
            yield return new WaitForSeconds(1f);
        findBestRandomAbility(false);
        yield return new WaitForSeconds(1f);
        if (findBestRandomMove())
            yield return new WaitForSeconds(1f);
        if (findBestRandomAbility(true))
            yield return new WaitForSeconds(1f);

        myCursor.doEndTurn();
        currentAction = null;
        startOfTurn = true;
    }

    private bool findBestRandomMove()
    {
        List<Vector2> possibleTargets = myCursor.getAllTargetTiles();
        possibleTargets.Add(new Vector2(myCursor.getCurrentOpal().getPos().x, myCursor.getCurrentOpal().getPos().z));
        List<Vector2> bestTargets = new List<Vector2>();
        List<int> targetScore = new List<int>();
        float max = Mathf.NegativeInfinity;
        for (int i = 0; i < possibleTargets.Count; i++)
        {
            targetScore.Add(calculateScore(mainGame.tileGrid, mainGame.tileGrid[(int)possibleTargets[i].x, (int)possibleTargets[i].y]));
            if (targetScore[i] > max)
            {
                max = targetScore[i];
                bestTargets.Clear();
                bestTargets.Add(possibleTargets[i]);
            }
            else if (targetScore[i] == max)
            {
                bestTargets.Add(possibleTargets[i]);
            }
        }
        if (bestTargets.Count != 0)
        {
            int pickOne = Random.Range(0, bestTargets.Count);
            doMove(bestTargets[pickOne], false);
            return true;
        }
        return false;
    }

    private bool findBestRandomAbility(bool free)
    {
        List<Vector3> allPossibleAbilities = myCursor.getAllPossibleAbilities(free);
        if (allPossibleAbilities.Count != 0)
        {
            Vector3 pickOne = chooseRandomAbilityEvenly(allPossibleAbilities);
            doAttack(pickOne, false);
            return true;
        }
        else
        {
            return false;
        }
    }


    private void doMove(TileScript target, bool instant)
    {
        myCursor.toggleCursorLock(true);
        myCursor.generateMovementDummies();
        transformCursor((int)target.getPos().x, (int)target.getPos().z);
        myCursor.generatePaths();
        myCursor.doMove(mainGame.paths, instant);
        myCursor.getCurrentOpal().getCurrentTile().standingOn(myCursor.getCurrentOpal());
        myCursor.toggleCursorLock(false);
    }

    private void doMove(Vector2 target, bool instant)
    {
        doMove(mainGame.tileGrid[(int)target.x, (int)target.y], instant);
    }

    private void doAttack(Vector3 abilityInfo, bool instant)
    {
        if (abilityInfo.x == -1)
            return;
        myCursor.toggleCursorLock(true);
        myCursor.generateAbilityDummies((int)abilityInfo.x);
        transformCursor((int)abilityInfo.y, (int)abilityInfo.z);
        myCursor.showTargets();
        myCursor.attackCurrentTile((int)abilityInfo.x, instant);
        myCursor.toggleCursorLock(false);
    }

    private void transformCursor(int x, int z)
    {
        if(x > -1 && x < 10 && z > -1 && z < 10)
        {
            myCursor.transform.position = new Vector3(x, myCursor.transform.position.y, z);
        }
    }

    private Vector3 chooseRandomAbilityEvenly(List<Vector3> allPossible)
    {
        List<int> possibleChoices = new List<int>();
        foreach (Vector3 v in allPossible)
        {
            if (!possibleChoices.Contains((int)v.x))
            {
                possibleChoices.Add((int)v.x);
            }
        }
        int chosen = possibleChoices[Random.Range(0, possibleChoices.Count)];
        List<Vector3> chosenAbility = new List<Vector3>();
        foreach (Vector3 v in allPossible)
        {
            if ((int)v.x == chosen)
            {
                chosenAbility.Add(v);
            }
        }

        return chosenAbility[Random.Range(0, chosenAbility.Count)];
    }



    
    private void assessMyBehaviour(OpalScript me, Behave b)
    {
        switch (b.getName()) {
            case "Killer":
                break;
            case "Safety":
                break;
            case "Pressure":
                break;

            case "Green-Thumb":
                break;
            case "Growth-Teleport":
                break;

            case "Ambush-Wary":
                break;

        }
    }


    private int calculateScore(TileScript[,] current, TileScript t)
    {
        int output = 0;
        List<int> numBehaviourPriorities = new List<int>();
        int randPriority = 0;
        foreach(Behave b in mainGame.getCurrentOpal().getBehaviours())
        {
            if (!numBehaviourPriorities.Contains(b.getPriority()) && b.getPriority() != 0)
                numBehaviourPriorities.Add(b.getPriority());
        }
        randPriority = numBehaviourPriorities[Random.Range(0, numBehaviourPriorities.Count)];
        foreach (Behave b in myCursor.getCurrentOpal().getBehaviours())
        {
            if (b.getPriority() == randPriority || b.getPriority() == 0)
            {
                switch (b.getName()) {
                    case "Cautious":
                        output += doCautious(t) * b.getIntensity();
                        break;
                    case "Acrophobic":
                        output += doAcrophobic(t) * b.getIntensity();
                        break;
                    case "Close-Combat":
                        output += doCloseCombat(t) * b.getIntensity();
                        break;
                    case "Green-Thumb":
                        output += doGreenThumb(t) * b.getIntensity();
                        break;
                    case "Ambush":
                        output += doAmbush(t) * b.getIntensity();
                        break;
                    case "Weasely":
                        output += doWeasely(t) * b.getIntensity();
                        break;
                }
            }
        }
        return output;
    }


    private int doCautious(TileScript target)
    {
        int output = 0;
        foreach (TileScript t in mainGame.tileGrid)
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != myCursor.getCurrentOpal().getTeam())
            {
                int range = t.getCurrentOpal().getMaxRange() + t.getCurrentOpal().getSpeed();
                if ((int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z)) - range > 0)
                    output +=  -(int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z)) / 2;
                else
                    output += (int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z)) - range;
            }
        }
        return output;
    }


    private int doAcrophobic(TileScript target)
    {
        int output = 0;
        if (myCursor.tileIsFalling((int)target.getPos().x, (int)target.getPos().z))
            output -= 100;
        return output;
    }
    private int doKiller(TileScript[,] snapshot, TileScript[,] current)
    {
        int output = 0;
        foreach(TileScript t in current)
        {
            if(t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != myCursor.getCurrentOpal().getTeam())
            {
                output -= t.getCurrentOpal().getHealth();
            }
        }
        return output;
    }

    private int doCloseCombat(TileScript t)
    {
        int output = 0;
        foreach(TileScript tile in t.getSurroundingTiles(true))
        {
            if(tile.currentPlayer != null && tile.currentPlayer.getTeam() != myCursor.getCurrentOpal().getTeam())
            {
                output += 15 - tile.currentPlayer.getHealth()/2;
            }
        }
        return output;
    }

    private int doGreenThumb(TileScript t)
    {
        if (t.type == "Growth")
            return 1;
        return 0;
    }

    private int doAmbush(TileScript t)
    {
        if(t.type == "Growth")
        {
            foreach(TileScript tile in t.getSurroundingTiles(true))
            {
                if (tile.getCurrentOpal() != null)
                    return 1;
            }
        }
        return 0;
    }

    private int doWeasely(TileScript target)
    {
        int mult = -1;
        if(myCursor.getCurrentOpal().getAttack() < 10 || myCursor.getCurrentOpal().getHealth() < 4)
        {
            mult = 1;
        }

        int output = 0;
        foreach (TileScript t in mainGame.tileGrid)
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != myCursor.getCurrentOpal().getTeam())
            {
                int range = t.getCurrentOpal().getMaxRange() + t.getCurrentOpal().getSpeed();
                if ((int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z)) - range > 0)
                    output +=  -(int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z));
                else
                    output += (int)(Mathf.Abs(target.getPos().x - t.getPos().x) + Mathf.Abs(target.getPos().z - t.getPos().z)) - range;
            }
        }
        return output * mult;
    }

    public IEnumerator doStateBasedMove()
    {
        moves.Clear();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                snapshotTileGrid[i, j] = mainGame.tileGrid[i, j];
            }
        }
        mainGame.saveGame(0);
        mainGame.setNoUpdate(true);
        possibleTargets = myCursor.getAllTargetTiles();
        for (int i = 0; i < possibleTargets.Count; i++)
        {
            doMove(possibleTargets[i], true);
            mainGame.saveGame(1);
            allPossibleAbilities = myCursor.getAllPossibleAbilities();
            if (allPossibleAbilities.Count == 0)
                allPossibleAbilities.Add(new Vector3(-1, 0, 0));
            for (int j = 0; j < allPossibleAbilities.Count; j++)
            {
                doAttack(allPossibleAbilities[j], true);
                yield return 0;
                if (mainGame.myCursor.getDistance() > 0 && myCursor.getCurrentOpal().getMoveAfter())
                {
                    mainGame.saveGame(2);
                    possibleTargets = myCursor.getAllTargetTiles();
                    for (int k = 0; k < possibleTargets.Count; k++)
                    {
                        doMove(possibleTargets[k], true);
                        moves.Add(new List<int> { calculateScore(snapshotTileGrid, mainGame.tileGrid), i, j, k });
                        mainGame.loadGame(2);
                    }
                }
                else
                {
                    moves.Add(new List<int> { calculateScore(snapshotTileGrid, mainGame.tileGrid), i, j });
                }
                mainGame.loadGame(1);
            }
            mainGame.loadGame(0);
        }

        mainGame.setNoUpdate(false);
        bestMoves.Clear();
        int best = moves[0][0];
        foreach (List<int> instructions in moves)
        {
            if (instructions[0] > best)
            {
                best = instructions[0];
                bestMoves.Clear();
                bestMoves.Add(instructions);
            }
            else if (instructions[0] == best)
            {
                bestMoves.Add(instructions);
            }
        }

        int randBestMove = Random.Range(0, bestMoves.Count);
        possibleTargets = myCursor.getAllTargetTiles();
        doMove(possibleTargets[bestMoves[randBestMove][1]], false);
        yield return new WaitForSeconds(1f);
        allPossibleAbilities = myCursor.getAllPossibleAbilities();
        if (allPossibleAbilities.Count == 0)
            allPossibleAbilities.Add(new Vector3(-1, 0, 0));
        doAttack(allPossibleAbilities[bestMoves[randBestMove][2]], false);
        yield return new WaitForSeconds(1f);
        if (myCursor.getDistance() != 0 && myCursor.getCurrentOpal().getMoveAfter())
        {
            possibleTargets = myCursor.getAllTargetTiles();
            doMove(possibleTargets[bestMoves[randBestMove][3]], false);
        }
        myCursor.doEndTurn();
        currentAction = null;
        startOfTurn = true;
    }

    private int calculateScore(TileScript[,] snapshot, TileScript[,] current)
    {
        int output = 0;
        foreach (Behave b in myCursor.getCurrentOpal().getBehaviours())
        {
            if (b.getName() == "Green-Thumb")
            {
                int result = containsMore("Growth", snapshot, current);
                if (result > 0)
                {
                    output += result * b.getIntensity();
                }
            }
            if (b.getName() == "Cautious")
            {
                int result = doCautious(snapshot, current);
                output += result * b.getIntensity();
            }
            if (b.getName() == "Acrophobic")
            {
                output += doAcrophobic(current) * b.getIntensity();
            }
            if (b.getName() == "Killer")
            {
                //output += doKiller(snapshot, current) * b.getIntensity();
            }
        }
        return output;
    }

    private int containsMore(string type, TileScript[,] snapshot, TileScript[,] current)
    {
        int sCount = 0;
        int cCount = 0;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (snapshot[i, j].type == type)
                    sCount++;
                if (snapshot[i, j].type == type)
                    cCount++;
            }
        }
        return sCount - cCount;
    }

    private int doCautious(TileScript[,] snapshot, TileScript[,] current)
    {
        int output = 0;
        foreach (TileScript t in current)
        {
            if (t.getCurrentOpal() != null && t.getCurrentOpal().getTeam() != myCursor.getCurrentOpal().getTeam())
            {
                int range = t.getCurrentOpal().getMaxRange() + t.getCurrentOpal().getSpeed();
                if ((int)(Mathf.Abs(myCursor.getCurrentOpal().getPos().x - t.getPos().x) + Mathf.Abs(myCursor.getCurrentOpal().getPos().z - t.getPos().z)) - range > 0)
                    return -(int)(Mathf.Abs(myCursor.getCurrentOpal().getPos().x - t.getPos().x) + Mathf.Abs(myCursor.getCurrentOpal().getPos().z - t.getPos().z)) / 2;
                output += (int)(Mathf.Abs(myCursor.getCurrentOpal().getPos().x - t.getPos().x) + Mathf.Abs(myCursor.getCurrentOpal().getPos().z - t.getPos().z)) - range;
            }
        }
        return output;
    }

    private int doAcrophobic(TileScript[,] current)
    {
        int output = 0;
        if (myCursor.tileIsFalling((int)myCursor.getCurrentOpal().getPos().x, (int)myCursor.getCurrentOpal().getPos().z))
            output -= 100;
        return output;
    }
}
public class Behave
{
    private int priority;
    private int intensity;
    private string behaviorName;

    public Behave(string n, int p, int i)
    {
        priority = p;
        behaviorName = n;
        intensity = i;
    }

    public int getPriority()
    {
        return priority;
    }

    public void setPriority(int p)
    {
        priority = p;
    }

    public int getIntensity()
    {
        return intensity;
    }

    public void setIntensity(int i)
    {
        intensity = i;
    }

    public string getName()
    {
        return behaviorName;
    }

    public void setName(string name)
    {
        behaviorName = name;
    }
}
