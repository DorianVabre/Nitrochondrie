using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicBlur : MonoBehaviour
{
    [SerializeField] Volume v;
    [SerializeField] Transform cam;
    private DepthOfField d;
    [Header("juste pour visualier")]
    [SerializeField] float baseFDOffset=0.3f;
    [SerializeField] float basePos;

    void Start()
    {
        v.profile.TryGet<DepthOfField>(out d);
        basePos = Mathf.Abs(cam.transform.position.z);
    }

    void Update()
    {
        d.focusDistance.value = baseFDOffset * (Mathf.Abs(cam.transform.position.z - basePos)+1);
    }
}
