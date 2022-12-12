using UnityEngine;

[System.Serializable]
public class Sling {

    // Creates sling-like movements.
    // The update method takes in a vector2.
    // If the joystick is being pushed, it charges a power meter and calculates a vector access value based on that.
    // If the power meter is full, it indicates that it must fire after a set duration. (That duration is a parameter.)
    // If the joystick is in the deadzone, it also says it must fire after a set duration within the deadzone. (Parameter.)

    [Header("Sling charge parameters")] [Space(3)]
    public AnimationCurve chargeCurve;
    public float timeToFullChargeSeconds;

    [Header("Throw parameters")] [Space(3)]
    public float strength;
    public float deadzone;
    public float deadzoneFireThreshold;
    public float cooldownSeconds;

    [Header("Throw trigger parameters")] [Space(3)]
    public float timeInDeadzoneToTriggerSeconds;
    public float timeAtFullChargeToTriggerSeconds;

    public bool mustFire;

    public float vectorMagnitude;

    [SerializeField]
    private float _timeSinceChargeStartSeconds;

    [SerializeField]
    private float _timeSinceFullChargeSeconds;

    [SerializeField]
    private float _timeSinceDeadZoneSeconds;

    private bool _isInCooldown;
    private float _timeInCooldownSeconds;

    [System.NonSerialized] public Vector2 accessVector;
    [System.NonSerialized] public float accessValue;
    [System.NonSerialized] public float accessValueInv;

    public void InputInDeadzone () {

        _timeSinceDeadZoneSeconds += Time.deltaTime;

        if (_timeSinceChargeStartSeconds/timeToFullChargeSeconds < deadzoneFireThreshold) {
            _timeSinceChargeStartSeconds = Mathf.Max(_timeSinceChargeStartSeconds - Time.deltaTime, 0);
            return;
        }

        if (_timeSinceDeadZoneSeconds > timeInDeadzoneToTriggerSeconds) {
            mustFire = true;
        }
    }

    public void SlingInCooldown () {

        _timeInCooldownSeconds += Time.deltaTime;

        if (_timeInCooldownSeconds > cooldownSeconds) {
            _timeInCooldownSeconds = 0.0f;
            _isInCooldown = false;
        }
    }

    public void SlingAtFullCharge () {
        _timeSinceFullChargeSeconds += Time.deltaTime;

        if (_timeSinceFullChargeSeconds > timeAtFullChargeToTriggerSeconds) {
            _timeSinceFullChargeSeconds = 0.0f;
            mustFire = true;
        }
    }

    public void Fire () {
        _timeInCooldownSeconds = 0.0f;
        _timeSinceChargeStartSeconds = 0.0f;
        _timeSinceFullChargeSeconds = 0.0f;
        _isInCooldown = true;
        mustFire = false;
    }

    public void Update(Vector2 movementVector) {

        vectorMagnitude = movementVector.magnitude;

        if (_isInCooldown) {
            SlingInCooldown ();
            return;
        }

        if (vectorMagnitude < deadzone) {
            InputInDeadzone ();
            return;
        }

        _timeSinceDeadZoneSeconds = 0;

        if (_timeSinceChargeStartSeconds >= timeToFullChargeSeconds) {
            SlingAtFullCharge();
        }
        else {
            _timeSinceChargeStartSeconds += Time.deltaTime;
        }

        accessValue = chargeCurve.Evaluate(_timeSinceChargeStartSeconds / timeToFullChargeSeconds);
        accessValueInv = 1.0f - chargeCurve.Evaluate(_timeSinceChargeStartSeconds / timeToFullChargeSeconds);

        accessVector = (-1.0f) * accessValue * strength * movementVector.normalized;
    }

}