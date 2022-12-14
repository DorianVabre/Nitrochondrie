using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailAnimator : MonoBehaviour
{
    TrailRenderer trailRenderer;
    public LoopAnimator trailAnimator;

    private float[] _keyBaseValue = new float[5];
    private float[] _keyBaseTime = new float[5];

    private Keyframe[] _replacingKeysArray;
    
    public float[] _sineWavesCoeff;
    public float[] _sineWavesPhase;
    public int[] _sineWavesTurn;

    private float _lifetimeTimer;
    private float _lifetimeDuration;

    // Start is called before the first frame update
    void Awake()
    {
        trailAnimator.LaunchAnim();
        trailRenderer = GetComponent<TrailRenderer>();

        _lifetimeTimer = 0.0f;
        _lifetimeDuration = trailRenderer.time;

        for (int i = 0; i < trailRenderer.widthCurve.keys.Length ; i++) {
            _keyBaseValue[i] = trailRenderer.widthCurve.keys[i].value;
            _keyBaseTime[i] = trailRenderer.widthCurve.keys[i].time;
        }
    }

    public void LaunchTrail () {
        _lifetimeTimer = 0.0f;
        trailRenderer.emitting = true;
        _lifetimeDuration = trailRenderer.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!trailRenderer.emitting) {
            return;
        }

        _lifetimeTimer += Time.deltaTime;

        trailAnimator.Update();

        _replacingKeysArray = new Keyframe[5];

        for (int i = 0; i < trailRenderer.widthCurve.keys.Length ; i++) {
            _replacingKeysArray[i] = new Keyframe (_keyBaseTime[i],
            _keyBaseValue[i] + _sineWavesCoeff[i] * Mathf.Sin( _sineWavesPhase[i] + 
            trailAnimator.value * Mathf.PI * 2f * _sineWavesTurn[i] ) );
        }

        trailRenderer.widthCurve = new AnimationCurve(_replacingKeysArray);

        if (_lifetimeTimer > _lifetimeDuration) {
            trailRenderer.emitting = false;
        }
    }
}
