using System;
using UnityEngine;

[Serializable]
public class OnceAnimator : AutoAnimator {

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
