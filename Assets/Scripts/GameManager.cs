using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  Sound system by Dorian Vabre

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    [SerializeField] private AudioSource[] lapMusic;

    public TimerManager timerManager;

    private CheckPointManager cpm;
    private int amountOfCheckpoints;

    public int secondLayerStartCheckpointReachedPercentage = 33;
    public float secondLayerMinVolume = 0.33f;
    public float secondLayerMaxVolume = 0.77f;
    private float secondLayerDeltaVolume;
    private int startCheckpointForSecondLayer;

    public int thirdLayerStartCheckpointReachedPercentage = 66;
    public float thirdLayerMinVolume = 0.33f;
    public float thirdLayerMaxVolume = 0.77f;
    private float thirdLayerDeltaVolume;
    private int startCheckpointForThirdLayer;

    public float volume1;
    public float volume2;
    public float volume3;

    private void Awake() {
        cpm = GetComponent<CheckPointManager>();
    }

    // Start is called before the first frame update
    void Start() {
        if(timerManager){
            timerManager.StartTimer();
        }
        amountOfCheckpoints = cpm.player1Checkpoints.Count;
        startCheckpointForSecondLayer = amountOfCheckpoints * secondLayerStartCheckpointReachedPercentage / 100;
        startCheckpointForThirdLayer = amountOfCheckpoints * thirdLayerStartCheckpointReachedPercentage / 100;
        secondLayerDeltaVolume = secondLayerMaxVolume - secondLayerMinVolume;
        thirdLayerDeltaVolume = thirdLayerMaxVolume - thirdLayerMinVolume;
    }

    public void CheckMusicChange(int checkpointsReachedByP1, int checkpointsReachedByP2){
        IncrementMusic(Mathf.Max(checkpointsReachedByP1, checkpointsReachedByP2));
    }

    private void IncrementMusic(int bestCheckpoint){
        // Second layer management
        if (bestCheckpoint >= startCheckpointForSecondLayer) {
            // current progress in layer / total checkpoints in current layer --> percentage of the current layer
            float percentageForSecondLayerProgression = (bestCheckpoint-startCheckpointForSecondLayer) / (float)(startCheckpointForThirdLayer-startCheckpointForSecondLayer);
            float volumeToSet = secondLayerMinVolume + percentageForSecondLayerProgression * secondLayerDeltaVolume;
            if (volumeToSet > secondLayerMaxVolume) {
                volumeToSet = secondLayerMaxVolume;
            }
            lapMusic[1].volume = volumeToSet;
            volume2 = lapMusic[1].volume;
        }

        // Third layer management
        if (bestCheckpoint >= startCheckpointForThirdLayer) {
            // current progress in layer / total checkpoints in current layer --> percentage of the current layer
            float percentageForThirdLayerProgression = (bestCheckpoint-startCheckpointForThirdLayer) / (float)(amountOfCheckpoints-startCheckpointForThirdLayer);
            float volumeToSet = thirdLayerMinVolume + percentageForThirdLayerProgression * thirdLayerDeltaVolume;
            if (volumeToSet > thirdLayerMaxVolume) {
                volumeToSet = thirdLayerMaxVolume;
            }
            lapMusic[2].volume = volumeToSet;
            volume3 = lapMusic[2].volume;
        }
    }
}
