using System;
using UnityEngine;

[Serializable]
public class ObjectShaker {
    // Shakes the position of an injectable game object according to the access value of a wiggler.
    // Stores the origin position of the object and restores that position at the end of the wiggler's timer.

    public Vector3Wiggler wiggler;
    public GameObject objectToShake;
    
    public Vector3 originPosition;

    public bool isActive = false;

    public void LaunchShake () {
        isActive = true;
        originPosition = objectToShake.transform.position;
        wiggler.LaunchWiggler();
    }

    public void Update () {

        if (!isActive) {
            return;
        }

        if (!wiggler.isActive) {
            isActive = false;
            objectToShake.transform.position = originPosition;
            return;
        }

        wiggler.Update();
        objectToShake.transform.position = originPosition + wiggler.wiggleAccessValue;
    }
}
