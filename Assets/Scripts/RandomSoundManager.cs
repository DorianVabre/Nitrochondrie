using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomSoundManager {
    [SerializeField] private AudioClip[] playlist;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float randomPitchMin = 1.0f;
    [SerializeField] private float randomPitchMax = 1.0f;

    private bool onCooldown = false;

    private float cooldownDuration = 0f;
    private float cooldownTimer = 0f;

    public void Update(){
        cooldownTimer += Time.deltaTime;
        if(cooldownTimer >= cooldownDuration ){
            onCooldown = false;
        }
    }

    public void PlayRandomSound(bool isOverride = false, float delay = 0f) {
        if(onCooldown && !isOverride){
            return;
        }

        float randomPitch = Random.Range(randomPitchMin, randomPitchMax);
        int soundIndex = Random.Range(0, playlist.Length);

        audioSource.pitch = randomPitch;
        audioSource.clip = playlist[soundIndex];

        cooldownDuration = audioSource.clip.length;
        cooldownTimer = 0f;
        onCooldown = true;

        audioSource.PlayDelayed(delay);
    }

    public void StopCooldown(){
        onCooldown = false;
    }

    public void PlaySound(int index) {
        float randomPitch = Random.Range(randomPitchMin, randomPitchMax);

        audioSource.pitch = randomPitch;
        audioSource.clip = playlist[index];
        audioSource.Play();
    }
}
