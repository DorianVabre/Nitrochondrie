using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckPointManager : MonoBehaviour
{
    public List<Transform> player1Checkpoints;
    public List<Transform> player2Checkpoints;

    PortraitAnimatorManager portraitP1;
    PortraitAnimatorManager portraitP2;

    public Transform checkpointLapEnd;

    public int lapsToWin = 3;
    public int amountOfCheckpoints;

    public TextMeshProUGUI lapsP1;
    public int checkpointsReachedByP1;
    public int lapsDoneByP1;

    public TextMeshProUGUI lapsP2;
    public int checkpointsReachedByP2;
    public int lapsDoneByP2;
    private GameManager gm;

    private void Awake() {
        portraitP1 = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PortraitAnimatorManager>();
        portraitP2 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PortraitAnimatorManager>();
    }

    void Start() {
        amountOfCheckpoints = player1Checkpoints.Count;
        checkpointsReachedByP1 = checkpointsReachedByP2 = 0;
        lapsP1.text = "Lap 1/" + lapsToWin;
        lapsP2.text = "Lap 1/" + lapsToWin;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void AddCheckpointP1() {
        checkpointsReachedByP1++;
        gm.CheckMusicChange(lapsDoneByP1, lapsDoneByP2,
                            checkpointsReachedByP1, checkpointsReachedByP2);
    }

    public void AddCheckpointP2() {
        checkpointsReachedByP2++;
        gm.CheckMusicChange(lapsDoneByP1, lapsDoneByP2,
                            checkpointsReachedByP1, checkpointsReachedByP2);
    }

    public void CheckLapP1() {
        if (checkpointsReachedByP1 == amountOfCheckpoints) {
            lapsDoneByP1++;
            ResetCheckpointsForP1();
            CheckVictory();
            int numberToPrint = lapsDoneByP1 + 1;
            if (numberToPrint <= lapsToWin)
                lapsP1.text = "Lap " + numberToPrint + "/" + lapsToWin;
        }
    }

    public void CheckLapP2() {
        if (checkpointsReachedByP2 == amountOfCheckpoints) {
            lapsDoneByP2++;
            ResetCheckpointsForP2();
            CheckVictory();
            int numberToPrint = lapsDoneByP2 + 1;
            if (numberToPrint <= lapsToWin)
                lapsP2.text = "Lap " + numberToPrint + "/" + lapsToWin;
        }
    }

    void CheckVictory() {
        if (lapsDoneByP1 >= lapsToWin) {
            portraitP1.SetVictory(true);
            portraitP2.SetVictory(false);
            Time.timeScale = 0.4f;
        } else if (lapsDoneByP2 >= lapsToWin) {
            portraitP1.SetVictory(false);
            portraitP2.SetVictory(true);
            Time.timeScale = 0.4f;
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
}
