using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    [SerializeField] private AudioSource[] lapMusic;

    // Start is called before the first frame update
    void Start() {
        for (int i = 1; i < lapMusic.Length; i++){
            lapMusic[i].volume = 0f;
        }
    }

    public void CheckMusicChange(int lapsDoneByP1, int lapsDoneByP2,
                                 int  checkpointsReachedByP1, int checkpointsReachedByP2){
        IncrementMusic(new Vector2(
            Mathf.Max(lapsDoneByP1, lapsDoneByP2),
            Mathf.Max(checkpointsReachedByP1, checkpointsReachedByP2)
        ));
    }

    private void IncrementMusic(Vector2 bestCheckpoint){
        if(bestCheckpoint.x >= 1f){
            lapMusic[(int)bestCheckpoint.x].volume = 0.34f * bestCheckpoint.y;
        }
    }
}
