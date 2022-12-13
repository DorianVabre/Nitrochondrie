using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    public float maxDistanceBetweenTargets;
    [SerializeField]
    private float _currentTargetDistance;

    public float maxAverageX, minAverageX;
    public float maxAverageY, minAverageY;
    [SerializeField]
    private float _targetCurrentAverageX, _targetCurrentAverageY;

    public float maxSize, minSize;

    public Camera cameraToSize;
    private float _baseCameraZ;

    // Where on the X, Y and scale axis we are currently. 
    // Looks at the average position of the bacteria on each axis.
    // For scale, looks at the magnitude of their difference vector over the max distance.
    public AnimationCurve cameraXMovementCurve, cameraYMovementCurve, cameraSizeCurve;

    // to store the evaluation of the curves
    [SerializeField]
    private float _easedXValue, _easedYValue, _easedSizeValue;

    // The actual amplitude of the X and Y movements must look at the current scale factor,
    // So this is evaluated against the same values as cameraSizeCurve.
    public AnimationCurve cameraScaledMinXCurve, cameraScaledMinYCurve;
    public AnimationCurve cameraScaledMaxXCurve, cameraScaledMaxYCurve;

    [SerializeField]
    private float _easedMinX, _easedMaxX, _easedMinY, _easedMaxY;
    
    [SerializeField]
    private float _resultingX, _resultingY;

    [Header("Targets to follow")] [Space(3)]
    public GameObject followOne;
    public GameObject followTwo;

    [Header("Smooth movement parameters")] [Space(3)]
    // Time it takes to reach the new positions with the SmoothDamps
    public float SmoothTime;

    // Values to SmoothDamp the Scale
    private float _targetSize;
    private float _scaleVelocity;

    // Values to SmoothDamp the vector.
    private Vector3 _targetPosition;
    private Vector3 _velocity;

    public void UpdatePositions () {
        float distanceGauge = _currentTargetDistance / maxDistanceBetweenTargets;

        _targetCurrentAverageX = ( followOne.transform.position.x + followTwo.transform.position.x ) / 2;
        _targetCurrentAverageY = ( followOne.transform.position.y + followTwo.transform.position.y ) / 2;
        _currentTargetDistance = ( followOne.transform.position - followTwo.transform.position ).magnitude;

        _easedXValue = cameraXMovementCurve.Evaluate( (_targetCurrentAverageX - minAverageX) / ( maxAverageX - minAverageX ));
        _easedYValue = cameraYMovementCurve.Evaluate( (_targetCurrentAverageY - minAverageY) / ( maxAverageY - minAverageY));
        
        _easedSizeValue = cameraSizeCurve.Evaluate(distanceGauge);

        _easedMinX = cameraScaledMinXCurve.Evaluate(distanceGauge);
        _easedMaxX = cameraScaledMaxXCurve.Evaluate(distanceGauge);

        _easedMinY = cameraScaledMinYCurve.Evaluate(distanceGauge);
        _easedMaxY = cameraScaledMaxYCurve.Evaluate(distanceGauge);

        _resultingX = _easedXValue * _easedMaxX + (1.0f - _easedXValue) * _easedMinX;
        _resultingY = _easedYValue * _easedMaxY + (1.0f - _easedYValue) * _easedMinY;

        _targetPosition = new Vector3 (_resultingX, _resultingY, _baseCameraZ);
        _targetSize = _easedSizeValue * maxSize + (1.0f - _easedSizeValue) * minSize;

    }

    // Start is called before the first frame update
    void Start()
    {
        _baseCameraZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositions();

        transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, SmoothTime);
        cameraToSize.orthographicSize = Mathf.SmoothDamp(cameraToSize.orthographicSize, _targetSize, ref _scaleVelocity, SmoothTime);
    }
}
