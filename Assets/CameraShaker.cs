using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public Vector3Wiggler wiggler;
    public GameObject cameraHolder;
    
    public Vector3 originPosition;

    public bool isShaking = false;

    public void Start() {
        originPosition = cameraHolder.transform.position;
    }

    public void LaunchShake (float intensityParameter = 0.0f) {
        if (isShaking) {
            wiggler.LaunchWiggler(intensityParameter);
            return;
        }

        isShaking = true;
        originPosition = cameraHolder.transform.position;
        wiggler.LaunchWiggler(intensityParameter);
    }

    public void LateUpdate () {

        if (!isShaking) {
            return;
        }

        if (!wiggler.isActive) {
            isShaking = false;
            cameraHolder.transform.position = originPosition;
            return;
        }

        wiggler.Update();
        cameraHolder.transform.position = originPosition + wiggler.wiggleAccessValue;
    }
}