using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public Vector3Wiggler wiggler;
    public GameObject cameraHolder;

    public float cooldownDuration;
    private float _cooldownTimer;
    private bool _onCooldown;

    public float intensityDecayerIntervalSeconds;
    private float _intensityDecayerSeconds;

    public AnimationCurve intensityMuterCurve;
    
    public Vector3 originPosition;

    public bool isShaking = false;

    public void Start() {
        originPosition = cameraHolder.transform.localPosition;
    }

    public void Cooldown () {
        _cooldownTimer += Time.deltaTime;
        
        if (_cooldownTimer > cooldownDuration) {
            _cooldownTimer = 0.0f;
            _onCooldown = false;
            return;
        }
    }

    public void addIntensityDecay() {
        _intensityDecayerSeconds = Mathf.Min( _intensityDecayerSeconds + intensityDecayerIntervalSeconds, 3.0f);
    }

    public void LaunchShake (float intensityParameter = 0.0f) {

        if (_onCooldown) {
            return;
        }

        _onCooldown = true;

        
        wiggler.LaunchWiggler(intensityParameter);
        addIntensityDecay();
        
        if (isShaking) {
            return;
        }

        isShaking = true;
        originPosition = cameraHolder.transform.localPosition;
    }

    public void LateUpdate () {
        
        _intensityDecayerSeconds = Mathf.Max(0, _intensityDecayerSeconds -= Time.deltaTime);

        if (_onCooldown) {
            Cooldown ();
        }

        if (!isShaking) {
            return;
        }

        if (!wiggler.isActive) {
            isShaking = false;
            cameraHolder.transform.localPosition = originPosition;
            return;
        }

        wiggler.Update();

        cameraHolder.transform.localPosition = originPosition
        + wiggler.wiggleAccessValue * intensityMuterCurve.Evaluate(_intensityDecayerSeconds);
    }
}
