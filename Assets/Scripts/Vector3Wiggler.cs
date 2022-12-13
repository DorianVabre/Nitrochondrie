using System;
using UnityEngine;

[Serializable]
public class Vector3Wiggler { 
    // Contains a Vector 3 value that changes randomly at determined time intervals for a selected duration, 
    // based on a choice of vector and a selected magnitude. 
    // Eases the magnitude over time according to an animation curve.

    [Header("Wiggle Controller")] [Space(3)] public AnimationCurve durationIntensityControllerCurve;
    public AnimationCurve intervalIntensityControllerCurve;
    public AnimationCurve magnitudeIntensityControllerCurve;
    [Space(3)]
    public Vector3 baseVector = new Vector3(1, 1, 0);

    private float intensityModulatedDurationSeconds = 1.0f;
    private float intensityModulatedIntervalSeconds = 0.1f;
    private float intensityModulatedMagnitude = 1.0f;

    private float timeElapsedSinceLastSeconds = 0.0f;
    private float totalTimeElapsedSeconds = 0.0f;

    [Header("Wiggle magnitude decay curve")] [Space(3)]
    public AnimationCurve magnitudeDecayCurve;
    
    private float easedMagnitude;
    
    [Space(5)] [Header("Exposed internals for debugging")]
    public bool isActive = false;
    [NonSerialized] public Vector3 wiggleAccessValue;

    public void LaunchWiggler (float intensityParameter = 0.0f) {
        intensityModulatedDurationSeconds = durationIntensityControllerCurve.Evaluate(intensityParameter);
        intensityModulatedIntervalSeconds = intervalIntensityControllerCurve.Evaluate(intensityParameter);

        intensityModulatedMagnitude = magnitudeIntensityControllerCurve.Evaluate(intensityParameter);

        isActive = true;
        
        timeElapsedSinceLastSeconds = 0.0f;
        totalTimeElapsedSeconds = 0.0f;
    }

    public void Wiggle () {
        wiggleAccessValue = new Vector3 (baseVector.x * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f), 
        baseVector.y * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f),
        baseVector.z * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f));
    }

    public void Update () {
        
        if (!isActive) {
            return;
        }

        timeElapsedSinceLastSeconds += Time.deltaTime;
        totalTimeElapsedSeconds += Time.deltaTime;

        if (timeElapsedSinceLastSeconds > intensityModulatedIntervalSeconds) {

            easedMagnitude = magnitudeDecayCurve.keys.Length > 0 ? 
            intensityModulatedMagnitude * magnitudeDecayCurve.Evaluate(totalTimeElapsedSeconds / intensityModulatedDurationSeconds) : 
            (1.0f - (totalTimeElapsedSeconds / intensityModulatedDurationSeconds)) * intensityModulatedMagnitude;

            Wiggle();

            timeElapsedSinceLastSeconds -= intensityModulatedIntervalSeconds;
        }

        if (totalTimeElapsedSeconds > intensityModulatedDurationSeconds) {
            wiggleAccessValue = Vector3.zero;
            isActive = false;
            return;
        }
    }    
}
