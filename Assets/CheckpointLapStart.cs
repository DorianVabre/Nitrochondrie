using UnityEngine;

public class CheckpointLapStart : MonoBehaviour
{
    private CheckPointManager cpm;

    void Start() {
        cpm = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckPointManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.transform.parent.CompareTag("Player1")) {
            cpm.CheckLapP1();
        } else if (other.gameObject.transform.parent.CompareTag("Player2")) {
            cpm.CheckLapP2();
        }
    }
}
