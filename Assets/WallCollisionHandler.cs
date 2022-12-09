using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable] public class AutoAnimator {
        
    [Header("Animation Controller")] [Space(3)] public float animationSpeed = 25.0f;
    public AnimationCurve curve;
    public float magnitude = 1.0f;
    public bool isActive = false;
    
    public float animationSlider = 0.0f; // Goes from 0 to 100 and serves to control the animation.

    [NonSerialized] public float easedValue = 0.0f; // The value when evaluated against the animator curve.

    public float value = 0.0f; // The values that are meant to be read ; the eased value modulated by magnitude.
    [NonSerialized] public float valueInv = 1.0f;

    public void RestartAnim () {
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

        if (isActive) {
            animationSlider = Mathf.Min(100.0f, animationSlider + Time.deltaTime*animationSpeed);
            
            // Evaluates the curve if it has any keys; if not it's just linear.
            easedValue = curve.keys.Length > 0 ? curve.Evaluate(animationSlider/100) : animationSlider / 100;
            value = easedValue * magnitude;
            valueInv = (1.0f - easedValue)*magnitude; // Use the magnitude to determine the apparent value and inverse value.

            if (animationSlider == 100.0f) {
                    isActive = false;
            }
        }
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
            bumpAnimator.magnitude = 0.2f * Mathf.Max(collision.relativeVelocity.magnitude, 5.0f)/5.0f;
            bumpAnimator.RestartAnim();
        }
    }
}
