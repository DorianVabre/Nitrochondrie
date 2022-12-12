using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionHandler : MonoBehaviour
{
    public OnceAnimator bumpAnimator;
    private Vector3 baseLocalScale;
    public SlingshotMovement bacteria;

    public CameraShaker cameraShaker;

    // Start is called before the first frame update
    void Start()
    {
        baseLocalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = baseLocalScale * (1.0f + bumpAnimator.value);
        bumpAnimator.Update();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bacteria = collision.gameObject.GetComponent<SlingshotMovement>();
        // Checks for a match with the SlingshotMovement component on any GameObject that collides with the wall
        if (bacteria == null) {
            return;
        }

        cameraShaker.LaunchShake(Mathf.Min(Mathf.Abs(collision.relativeVelocity.magnitude), 5.0f)/5.0f);

        if (!bumpAnimator.isActive)
        {
            bumpAnimator.magnitude = 0.05f * Mathf.Min(collision.relativeVelocity.magnitude, 5.0f)/5.0f;
            bumpAnimator.LaunchAnim();
        }
    }
}
