using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public abstract class AutoAnimator {
    [Header("Animation Controller")] [Space(3)] public float animationSpeed = 25.0f;
    public AnimationCurve curve;
    public float magnitude = 1.0f;

    [NonSerialized] public bool isActive = false;
    
    [NonSerialized] public float animationSlider = 0.0f; // Goes from 0 to 100 and serves to control the animation.

    [NonSerialized] public float easedValue = 0.0f; // The value when evaluated against the animator curve.

    [NonSerialized] public float value = 0.0f; // The values that are meant to be read ; the eased value modulated by magnitude.
    [NonSerialized] public float valueInv = 1.0f;

    public virtual void LaunchAnim () {
        isActive = true;
        animationSlider = 0.0f;
        Update();
    }

    public virtual void PauseAnim () {
        isActive = false;
    }

    public virtual void ResumeAnim () {
        isActive = true;
    }

    public abstract void Update ();
}

[Serializable] public class OnceAnimator : AutoAnimator {

    public override void Update () {

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
