using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private SlingshotMovement _checkForBacteria;
    public CameraShaker cameraShaker;

    void OnCollisionEnter2D(Collision2D collision)
    {
        _checkForBacteria = collision.gameObject.GetComponent<SlingshotMovement>();

        // Checks for a match with the SlingshotMovement component on any GameObject that collides with the wall
        if (_checkForBacteria == null) {
            return;
        }

        float intensityFactor = Mathf.Min(collision.relativeVelocity.magnitude, 5.0f) / 5.0f;

        cameraShaker.LaunchShake(intensityFactor);
    }
}