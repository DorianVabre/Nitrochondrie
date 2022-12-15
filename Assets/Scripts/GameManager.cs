using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//  Sound system by Dorian Vabre

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform playerOne;
    [SerializeField] private Transform playerTwo;

    [SerializeField] private SFXManager sfxManagerP1;
    [SerializeField] private SFXManager sfxManagerP2;

    [SerializeField] private AudioSource[] lapMusic;
    [SerializeField] private AudioSource[] quarterSounds;

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

    private int firstQuarterCheckpoint;
    private int secondQuarterCheckpoint;
    private int thirdQuarterCheckpoint;

    public float volume2;
    public float volume3;

    private float startTimer;
    private float startTimerLimit;
    public GameObject inputManager;
    private bool raceStarted;
    public AudioSource countdownSound;
    private bool countdownSoundPlayed;

    private void Awake() {
        cpm = GetComponent<CheckPointManager>();
    }

    void Start() {
        startTimer = 0f;
        startTimerLimit = 4.2f;
        raceStarted = false;
        countdownSoundPlayed = false;
    }

    void Update() {
        if(!raceStarted){
            startTimer += Time.deltaTime;
            if(startTimer >= startTimerLimit){
                StartRace();
                raceStarted = true;
            }
            if(startTimer >= 1.2f && !countdownSoundPlayed){
                countdownSound.Play();
                countdownSoundPlayed = true;
            }
            
        }
    }

    void StartRace() {
        if(timerManager){
            timerManager.StartTimer();
            amountOfCheckpoints = cpm.player1Checkpoints.Count;
            startCheckpointForSecondLayer = amountOfCheckpoints * secondLayerStartCheckpointReachedPercentage / 100;
            startCheckpointForThirdLayer = amountOfCheckpoints * thirdLayerStartCheckpointReachedPercentage / 100;
            secondLayerDeltaVolume = secondLayerMaxVolume - secondLayerMinVolume;
            thirdLayerDeltaVolume = thirdLayerMaxVolume - thirdLayerMinVolume;

            firstQuarterCheckpoint = (int)amountOfCheckpoints / 4;
            secondQuarterCheckpoint = (int)amountOfCheckpoints / 2;
            thirdQuarterCheckpoint = (int)amountOfCheckpoints * 3 / 4;
            if(inputManager){
                inputManager.SetActive(true);
            }
        }
    }

    public void CheckMusicChange(int checkpointsReachedByP1, int checkpointsReachedByP2){
        IncrementMusic(Mathf.Max(checkpointsReachedByP1, checkpointsReachedByP2));
    }

    private void IncrementMusic(int bestCheckpoint){
        ManageQuarters(bestCheckpoint);

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

    private void ManageQuarters(int bestCheckpoint) {
        if (bestCheckpoint == firstQuarterCheckpoint) {
            quarterSounds[0].Play();
            PlaySoundOfAheadPlayer();
        } else if (bestCheckpoint == secondQuarterCheckpoint) {
            quarterSounds[1].Play();
            PlaySoundOfAheadPlayer();
        } else if (bestCheckpoint == thirdQuarterCheckpoint) {
            quarterSounds[2].Play();
            PlaySoundOfAheadPlayer();
        } else if (bestCheckpoint == amountOfCheckpoints) {
            quarterSounds[3].Play();
            PlaySoundOfAheadPlayer();
        }
    }

    private void PlaySoundOfAheadPlayer() {
        if (cpm.checkpointsReachedByP1 >= cpm.checkpointsReachedByP2) {
            sfxManagerP1.PlayLapVoice();
        } else {
            sfxManagerP2.PlayLapVoice();
        }
    }
}
