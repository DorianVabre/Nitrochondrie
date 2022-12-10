using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class AutoAnimator {
        
    [Header("Animation Controller")] [Space(3)] public float animationSpeed = 25.0f;
    public AnimationCurve curve;
    public float magnitude = 1.0f;

    [NonSerialized] public bool isActive = false;
    
    [NonSerialized] private float animationSlider = 0.0f; // Goes from 0 to 100 and serves to control the animation.

    [NonSerialized] public float easedValue = 0.0f; // The value when evaluated against the animator curve.

    [NonSerialized] public float value = 0.0f; // The values that are meant to be read ; the eased value modulated by magnitude.
    [NonSerialized] public float valueInv = 1.0f;

    public void LauchAnim () {
        isActive = true;
        animationSlider = 0.0f;
        Update();
    }

    public void PauseAnim () {
        isActive = false;
    }

    public void ResumeAnim () {
        isActive = true;
    }

    public void Update () {

        if (!isActive) {
            return; 
        }

        animationSlider = Mathf.Min(100.0f, animationSlider + Time.deltaTime*animationSpeed);
        
        // Evaluates the curve if it has any keys; if not it's just linear.
        easedValue = curve.keys.Length > 0 ? curve.Evaluate(animationSlider/100) : animationSlider / 100;

        // Use the magnitude to determine the apparent value and inverse value.
        value = easedValue * magnitude;
        valueInv = (1.0f - easedValue)*magnitude;

        if (animationSlider == 100.0f) {
                isActive = false;
        }
    }
}

[Serializable] public class Vector3Wiggler { 
    // Contains a Vector 3 value that changes randomly at determined time intervals for a selected duration, 
    // based on a choice of vector and a selected magnitude. 
    // Eases the magnitude over time according to an animation curve.

    [Header("Wiggle Controller")] [Space(3)] public float totalWiggleDurationSeconds = 1.0f;
    public float wiggleIntervalSeconds = 0.1f;

    private float timeElapsedSinceLastWiggleSeconds = 0.0f;
    private float totalTimeElapsedSinceWiggleLaunchSeconds = 0.0f;

    public AnimationCurve wiggleIntensityCurve;
    
    public float wiggleMagnitude = 1.0f;
    private float easedMagnitude;
    
    public bool isActive = false;

    public Vector3 wiggleBaseValue = new Vector3(1, 1, 0);
    [NonSerialized] public Vector3 wiggleAccessValue;

    public void LauchWiggler () {
        isActive = true;
        timeElapsedSinceLastWiggleSeconds = 0.0f;
        totalTimeElapsedSinceWiggleLaunchSeconds = 0.0f;
    }

    public void Wiggle () {
        wiggleAccessValue = new Vector3 (wiggleBaseValue.x * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f), 
        wiggleBaseValue.y * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f),
        wiggleBaseValue.z * easedMagnitude * UnityEngine.Random.Range(-1.0f, 1.0f));
    }

    public void Update () {
        
        if (!isActive) {
            return;
        }

        timeElapsedSinceLastWiggleSeconds += Time.deltaTime;
        totalTimeElapsedSinceWiggleLaunchSeconds += Time.deltaTime;

        if (timeElapsedSinceLastWiggleSeconds > wiggleIntervalSeconds) {

            easedMagnitude = wiggleIntensityCurve.keys.Length > 0 ? 
            wiggleMagnitude * wiggleIntensityCurve.Evaluate(totalTimeElapsedSinceWiggleLaunchSeconds / totalWiggleDurationSeconds) : 
            (1.0f - (totalTimeElapsedSinceWiggleLaunchSeconds / totalWiggleDurationSeconds)) * wiggleMagnitude;

            Wiggle();

            timeElapsedSinceLastWiggleSeconds -= wiggleIntervalSeconds;
        }

        if (totalTimeElapsedSinceWiggleLaunchSeconds > totalWiggleDurationSeconds) {
            wiggleAccessValue = Vector3.zero;
            isActive = false;
            return;
        }
    }    
}

[Serializable] public class ObjectShaker {
    // Shakes the position of an injectable game object according to the access value of a wiggler.
    // Stores the origin position of the object and restores that position at the end of the wiggler's timer.

    public Vector3Wiggler wiggler;
    public GameObject objectToShake;
    
    public Vector3 originPosition;

    public bool isActive = false;

    public void LaunchShake () {
        isActive = true;
        originPosition = objectToShake.transform.position;
        wiggler.LauchWiggler();
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

public class WallCollisionHandler : MonoBehaviour
{
    public AutoAnimator bumpAnimator;
    private Vector3 baseLocalScale;
    public SlingshotMovement bacteria;

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
        if (bacteria && !bumpAnimator.isActive)
        {
            bumpAnimator.magnitude = 0.05f * Mathf.Max(collision.relativeVelocity.magnitude, 5.0f)/5.0f;
            bumpAnimator.LauchAnim();
        }
    }
}
