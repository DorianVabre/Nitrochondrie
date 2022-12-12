using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundManager : MonoBehaviour {
    [SerializeField] private AudioClip[] playlist;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int playlistSize;
    [SerializeField] private int lastPlayedIndex;
    [SerializeField] private float lastSoundTime;
    [SerializeField] private float randomSecondsBetweenSounds;
    [SerializeField] private float minimumSecondsBetweenSounds;
    [SerializeField] private float maximumSecondsBetweenSounds;

    void Start() {
        playlistSize = playlist.Length;
        audioSource.clip = playlist[0];
        lastPlayedIndex = 0;

        lastSoundTime = Time.time;
        randomSecondsBetweenSounds = Random.Range(minimumSecondsBetweenSounds, maximumSecondsBetweenSounds);

        audioSource.Play();
    }

    void Update() {
        if (!audioSource.isPlaying && lastSoundTime + randomSecondsBetweenSounds < Time.time) {
            lastSoundTime = Time.time;
            randomSecondsBetweenSounds = Random.Range(minimumSecondsBetweenSounds, maximumSecondsBetweenSounds);
            PlayNextSound();
        }
    }

    void PlayNextSound() {
        float randomPitch = Random.Range(0.75f, 1.25f);
        
        int soundIndex = Random.Range(0, playlistSize);
        while (soundIndex == lastPlayedIndex) {
            soundIndex = Random.Range(0, playlistSize);
        }
        lastPlayedIndex = soundIndex;
        
        audioSource.clip = playlist[soundIndex];
        audioSource.pitch = randomPitch;
        
        audioSource.Play();
    }
}
