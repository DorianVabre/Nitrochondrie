using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] public Animator transition;
    [SerializeField] public float transitionTime = 1f;
    [SerializeField] public RandomSoundManager transitionSound;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource ambSource;

    public void Start() {
        transitionSound.PlaySound(0);
        StartCoroutine(FadeIn(musicSource, 1.0f));
        StartCoroutine(FadeIn(ambSource, 2.0f));
    }

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex) {
        transitionSound.PlaySound(1);
        StartCoroutine(FadeOut(musicSource, 1.0f));
        StartCoroutine(FadeOut(musicSource, 1.0f));
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop ();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
        audioSource.volume = 0;
 
        while (audioSource.volume < startVolume) {
            audioSource.volume += Time.deltaTime / FadeTime;
 
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
