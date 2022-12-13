using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public List<Transform> player1Checkpoints;
    public List<Transform> player2Checkpoints;

    public Transform checkpointLapEnd;

    public int lapsToWin = 3;
    public int amountOfCheckpoints;

    public int checkpointsReachedByP1;
    public int lapsDoneByP1;

    public int checkpointsReachedByP2;
    public int lapsDoneByP2;


    void Start() {
        amountOfCheckpoints = player1Checkpoints.Count;
        checkpointsReachedByP1 = checkpointsReachedByP2 = 0;
    }

    public void AddCheckpointP1() {
        checkpointsReachedByP1++;
    }

    public void AddCheckpointP2() {
        checkpointsReachedByP2++;
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
        if (checkpointsReachedByP1 >= amountOfCheckpoints) {
            Debug.Log("P1 wins");
        } else if (checkpointsReachedByP2 >= amountOfCheckpoints) {
            Debug.Log("P2 wins");           
        }
    }

    void ResetCheckpointsForP1() {
        foreach(Transform checkpoint in player1Checkpoints) {
            checkpoint.GetComponent<Checkpoint>().hasBeenReachedByP1 = false;
        }
    }

    void ResetCheckpointsForP2() {
        foreach(Transform checkpoint in player2Checkpoints) {
            checkpoint.GetComponent<Checkpoint>().hasBeenReachedByP2 = false;
        }
    }
}
