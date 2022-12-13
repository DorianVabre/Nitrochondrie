using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public RandomSoundManager randomHurt;
    public RandomSoundManager randomHitWalls;
    public RandomSoundManager randomHitPlayer;
    public RandomSoundManager randomStretch;
    public RandomSoundManager randomLaunch;
    public RandomSoundManager randomRelease;
    public RandomSoundManager lapVoices;

    private void Update() {
        randomHurt.Update();
        randomHitWalls.Update();
        randomHitPlayer.Update();
        randomStretch.Update();
        randomLaunch.Update();
        randomRelease.Update();
    }

    public void PlayRandomHitWalls(){
        randomHitWalls.PlayRandomSound();
    }

    public void PlayRandomHurt(){
        randomHurt.PlayRandomSound();
    }

    public void PlayRandomHitPlayer(){
        randomHitPlayer.PlayRandomSound();
    }

    public void PlayRandomStretch(){
        randomStretch.PlayRandomSound();
    }

    public void PlayRandomRelease(){
        randomRelease.PlayRandomSound(true);
        randomStretch.StopCooldown();
    }

    public void PlayRandomLaunch(){
        randomLaunch.PlayRandomSound(false, 0.1f);
    }

    public void PlayWallCollisionSound() {
        float randomNumber = UnityEngine.Random.Range(0f, 1f);
        
        PlayRandomHitWalls();

        if (randomNumber < 0.2f) {
            PlayRandomHurt();
        }

    }

    public void PlayLapVoice(int index){
        lapVoices.PlaySound(index);
    }
}
