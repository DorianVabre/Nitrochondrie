using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusAnimator : MonoBehaviour
{
    public LoopAnimator virusAnimator;

    public int rotatingTurns;
    public float rotatingPhase;

    public float upDownAmplitude;
    public int upDownTurns;

    public float leftRightAmplitude;
    public int leftRightTurns;

    public float sineYawAmplitude;
    public int sineYawTurns;
    public float sineYawPhase;

    public Quaternion currentRotation;
    public Vector3 baseRotation;
    public Vector3 basePosition;

    // Start is called before the first frame update
    void Start()
    {
        virusAnimator.LaunchAnim();
        currentRotation = transform.rotation;
        basePosition = transform.position;
        baseRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        virusAnimator.Update();

        currentRotation.eulerAngles = new Vector3 (baseRotation.x, baseRotation.y, baseRotation.z + rotatingTurns * 360.0f * (virusAnimator.animationSlider/100.0f) + 
        sineYawAmplitude * Mathf.Sin(Mathf.PI * 2.0f * virusAnimator.value * sineYawTurns + sineYawPhase) );

        transform.rotation = currentRotation;

        transform.position = new Vector3( basePosition.x + leftRightAmplitude * Mathf.Sin(Mathf.PI * 2.0f * virusAnimator.value), 
        basePosition.y + upDownAmplitude * Mathf.Sin(Mathf.PI * 2.0f * virusAnimator.value), 
        basePosition.z);
    }
}
