using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckPointManager cpm;

    public bool hasBeenReachedByP1 = false;
    public bool hasBeenReachedByP2 = false;

    void Start() {
        cpm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CheckPointManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.transform.parent.CompareTag("Player1")) {
            if (!hasBeenReachedByP1) {
                cpm.AddCheckpointP1();
                hasBeenReachedByP1 = true;
            }
        } else if (other.gameObject.transform.parent.CompareTag("Player2")) {
            if (!hasBeenReachedByP2) {
                cpm.AddCheckpointP2();
                hasBeenReachedByP2 = true;
            }
        }
    }
}
