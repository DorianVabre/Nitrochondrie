using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    public List<Transform> player1Checkpoints;
    public List<Transform> player2Checkpoints;

    PortraitAnimatorManager portraitP1;
    PortraitAnimatorManager portraitP2;

    public GameObject winningUI;
    public TextMeshProUGUI topTimerText;
    public TextMeshProUGUI playerWinText;
    public TextMeshProUGUI timerWinText;

    public Transform checkpointLapEnd;

    public int lapsToWin = 3;
    public int amountOfCheckpoints;

    public Transform positionP1;
    public int checkpointsReachedByP1;
    public int lapsDoneByP1;

    public Transform positionP2;
    public int checkpointsReachedByP2;
    public int lapsDoneByP2;
    private GameManager gm;

    private int whoIsAhead = 1;

    public InputActionAsset actionAssetForReload;

    private void Awake() {
        winningUI.SetActive(false);
        portraitP1 = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PortraitAnimatorManager>();
        portraitP2 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PortraitAnimatorManager>();
    }

    void Start() {
        amountOfCheckpoints = player1Checkpoints.Count;
        checkpointsReachedByP1 = checkpointsReachedByP2 = 0;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void AddCheckpointP1() {
        checkpointsReachedByP1++;
        UpdatePositions();
        gm.CheckMusicChange(checkpointsReachedByP1, checkpointsReachedByP2);
    }

    public void AddCheckpointP2() {
        checkpointsReachedByP2++;
        UpdatePositions();
        gm.CheckMusicChange(checkpointsReachedByP1, checkpointsReachedByP2);
    }

    public void CheckLapP1() {
        if (checkpointsReachedByP1 == amountOfCheckpoints) {
            lapsDoneByP1++;
            ResetCheckpointsForP1();
            CheckVictory();
        }
    }

    public void CheckLapP2() {
        if (checkpointsReachedByP2 == amountOfCheckpoints) {
            lapsDoneByP2++;
            ResetCheckpointsForP2();
            CheckVictory();
        }
    }

    void CheckVictory() {
        bool gameHasEnded = false;
        if (lapsDoneByP1 >= lapsToWin) {
            portraitP1.SetVictory(true);
            portraitP2.SetVictory(false);
            playerWinText.text = "J1  won!";
            gameHasEnded = true;
        } else if (lapsDoneByP2 >= lapsToWin) {
            portraitP1.SetVictory(false);
            portraitP2.SetVictory(true);
            playerWinText.text = "J2  won!";
            gameHasEnded = true;
        }

        if (gameHasEnded) {
            /*foreach (var action in actionAssetForReload){
                if (action.name == "ReloadScene") {
                    action.AddBinding("<Keyboard>/r");
                }
            }*/
            topTimerText.text = "";
            
            ReloadManager rm = GetComponent<ReloadManager>();
            rm.ActivateReload();

            TimerManager tm = GetComponent<TimerManager>();
            tm.StopTimer();
            timerWinText.text = tm.GetCurrentTime();
            winningUI.SetActive(true);
        }
    }

    void ResetCheckpointsForP1() {
        foreach(Transform checkpoint in player1Checkpoints) {
            checkpoint.GetComponent<Checkpoint>().hasBeenReachedByP1 = false;
            checkpointsReachedByP1 = 0;
        }
    }

    void ResetCheckpointsForP2() {
        foreach(Transform checkpoint in player2Checkpoints) {
            checkpoint.GetComponent<Checkpoint>().hasBeenReachedByP2 = false;
            checkpointsReachedByP2 = 0;
        }
    }

    void UpdatePositions() {

        bool p1HasMoreCheckpoints = checkpointsReachedByP1 + amountOfCheckpoints * lapsDoneByP1 > checkpointsReachedByP2 + amountOfCheckpoints * lapsDoneByP2;
        bool p2HasMoreCheckpoints = checkpointsReachedByP1 + amountOfCheckpoints * lapsDoneByP1 < checkpointsReachedByP2 + amountOfCheckpoints * lapsDoneByP2;

        if (whoIsAhead == 2 && p2HasMoreCheckpoints) {
            return;
        }
        if (whoIsAhead == 1 && p1HasMoreCheckpoints) {
            return;
        }

        if (p1HasMoreCheckpoints) {
            // p1 is ahead
            whoIsAhead = 1;
            positionP1.GetComponent<Animator>().SetBool("isWinnening", true);
            positionP2.GetComponent<Animator>().SetBool("isWinnening", false);
        } else if (p2HasMoreCheckpoints){
            // p2 is ahead
            whoIsAhead = 2;
            positionP1.GetComponent<Animator>().SetBool("isWinnening", false);
            positionP2.GetComponent<Animator>().SetBool("isWinnening", true);
        }
    }
}
