using System;
using UnityEngine;

[Serializable]
public abstract class AutoAnimator {
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
