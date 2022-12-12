using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInEffector : MonoBehaviour
{
    public OnceAnimator zoomInAnimator;
    public float minSize, maxSize;
    public Vector3 baseScaleVector;
    
    
    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        zoomInAnimator.LaunchAnim();
        transform.localScale = minSize * baseScaleVector;
    }

    // Update is called once per frame
    void Update()
    {
        if (!zoomInAnimator.isActive) {
            Destroy(gameObject);
        }
        
        zoomInAnimator.Update();
        transform.localScale = ( minSize * zoomInAnimator.valueInv + maxSize * zoomInAnimator.value) * baseScaleVector;
    }
}
