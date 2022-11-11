using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedWorldAnimationController : MonoBehaviour
{
    public AmazingAssets.CurvedWorld.CurvedWorldController CW_Controller { get; private set; }

    [Range(-10,0)]
    public float Xmin, Ymin;

    [Range(0,10)]
    public float Xmax, Ymax;

    private void Awake()
    {
        CW_Controller= GetComponent<AmazingAssets.CurvedWorld.CurvedWorldController>();

    }

    private void Start()
    {
        StartCoroutine(AnimateCurvedWorld());
    }

    bool toggle = false;
    private IEnumerator AnimateCurvedWorld()
    {
        toggle = !toggle;

        float progress = 0f;
        float duration = 5f;

        var currHorizontalSize = CW_Controller.bendHorizontalSize;
        var currVerticalSize = CW_Controller.bendHorizontalSize;

        float bendSizeX = toggle ? Xmin : Xmax;
        float bendSizeY = toggle ? Ymin : Ymax;

        while (progress <= 1f)
        {
            CW_Controller.SetBendHorizontalSize(Mathf.Lerp(currHorizontalSize, bendSizeX, progress));
            CW_Controller.SetBendVerticalSize(Mathf.Lerp(currVerticalSize, bendSizeY, progress));

            progress += Time.deltaTime / duration;
            yield return null;
        }

        StartCoroutine(AnimateCurvedWorld());
    }
}
