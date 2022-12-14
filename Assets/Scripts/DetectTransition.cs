using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DetectTransition : MonoBehaviour {
    [SerializeField] public LevelTransition levelTransition;
    private int arrived;

    private void Start() {
        arrived = 0;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.parent.CompareTag("Player") ||
            other.transform.parent.CompareTag("Player1") ||
            other.transform.parent.CompareTag("Player2")){
            other.attachedRigidbody.bodyType = RigidbodyType2D.Static;
            other.attachedRigidbody.simulated = false;
            arrived++;
        }
        if(arrived >= 2){
            levelTransition.LoadNextLevel();
        }
    }
}
