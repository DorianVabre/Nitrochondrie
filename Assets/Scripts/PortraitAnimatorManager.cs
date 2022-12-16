using UnityEngine;

public class PortraitAnimatorManager : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb;
    public float speedThresholdBeforeSpeedyAnim = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.magnitude >= speedThresholdBeforeSpeedyAnim) {
            anim.SetBool("speed", true);
        } else {
            anim.SetBool("speed", false);
        }
    }

    public void SetVictory(bool hasWon) {
        if (hasWon) {
            anim.SetTrigger("hasWon");
        } else {
            anim.SetTrigger("hasLost");
        }

    }

    public void TriggerHurt() {
        anim.SetTrigger("hurt");
    }
}
