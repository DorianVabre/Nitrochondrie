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
    private GameManager gm;


    void Start() {
        amountOfCheckpoints = player1Checkpoints.Count;
        checkpointsReachedByP1 = checkpointsReachedByP2 = 0;
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
        if (lapsDoneByP1 >= lapsToWin) {
            Debug.Log("P1 wins");
        } else if (lapsDoneByP2 >= lapsToWin) {
            Debug.Log("P2 wins");           
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
