using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollisionHandler : MonoBehaviour
{
    public OnceAnimator bumpAnimator;
    public SlingshotMovement bacteria;

    public AnimationCurve bumpMinScaleIntensityCurve;
    public AnimationCurve bumpMaxScaleIntensityCurve;

    public GameObject objectToBump;
    public CameraShaker cameraShaker;

    private Vector3 _baseLocalScale;
    private float _currentMinScaleFactor = 1.0f;
    private float _currentMaxScaleFactor = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _baseLocalScale = objectToBump.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        objectToBump.transform.localScale = (_currentMinScaleFactor * bumpAnimator.valueInv
                                + _currentMaxScaleFactor * bumpAnimator.value)
                                * _baseLocalScale;
        bumpAnimator.Update();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        bacteria = collision.gameObject.GetComponent<SlingshotMovement>();
        // Checks for a match with the SlingshotMovement component on any GameObject that collides with the wall
        if (bacteria == null) {
            return;
        }

        float intensityFactor = Mathf.Min(collision.relativeVelocity.magnitude, 5.0f) / 5.0f;

        cameraShaker.LaunchShake(intensityFactor);
        _currentMinScaleFactor = bumpMinScaleIntensityCurve.Evaluate(intensityFactor);
        _currentMaxScaleFactor = bumpMaxScaleIntensityCurve.Evaluate(intensityFactor);

        if (!bumpAnimator.isActive)
        {
            bumpAnimator.LaunchAnim();
        }
    }
}
