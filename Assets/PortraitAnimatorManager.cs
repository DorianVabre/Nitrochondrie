using UnityEngine;

public class PortraitAnimatorManager : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D rb;
    public float speedThresholdBeforeSpeedyAnim = 2f;

    private bool isActive = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isActive) {
            return;
        }

        if (rb.velocity.magnitude >= speedThresholdBeforeSpeedyAnim) {
            anim.SetBool("speed", true);
        } else {
            anim.SetBool("speed", false);
        }
    }
    public void SetVictory(bool hasWon) {
        isActive = false;
        anim.SetBool("speed", false);

        if (hasWon) {
            anim.SetBool("hasWinned", true);
        } else {
            anim.SetBool("hasLost", true);
        }

    }
}
