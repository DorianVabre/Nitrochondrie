using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private SFXManager _bacteriaSFXManager;
    public CameraShaker cameraShaker;

    void OnCollisionEnter2D(Collision2D collision)
    {
        _bacteriaSFXManager = collision.gameObject.transform.parent.GetComponentInChildren<SFXManager>();

        // Checks for a match with the SlingshotMovement component on any GameObject that collides with the wall
        if (_bacteriaSFXManager == null) {
            return;
        }

        float intensityFactor = Mathf.Min(collision.relativeVelocity.magnitude, 5.0f) / 5.0f;

        cameraShaker.LaunchShake(intensityFactor);
        _bacteriaSFXManager.PlayWallCollisionSound();
    }
}