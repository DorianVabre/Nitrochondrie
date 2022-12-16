using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayAnimator : MonoBehaviour
{
    public LoopAnimator overlayAnimator;

    // Start is called before the first frame update
    void Start()
    {
        overlayAnimator.LaunchAnim();
    }

    // Update is called once per frame
    void Update()
    {
        overlayAnimator.Update();
    }
}
