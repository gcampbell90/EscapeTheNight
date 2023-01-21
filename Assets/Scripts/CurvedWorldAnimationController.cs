using System.Collections;
using UnityEngine;

public class CurvedWorldAnimationController : MonoBehaviour
{
    public AmazingAssets.CurvedWorld.CurvedWorldController CW_Controller { get; private set; }

    [Range(-10, 0)]
    public float Xmin;
    [Range(0, 10)]
    public float Xmax;
    [Range(-10, 0)]
    public float Ymin;
    [Range(0, 10)]
    public float Ymax;

    [SerializeField] bool curvedWorldToggle;

    private void Awake()
    {
        CW_Controller = GetComponent<AmazingAssets.CurvedWorld.CurvedWorldController>();
    }

    private void Start()
    {
        if (curvedWorldToggle) StartCoroutine(AnimateCurvedWorld());
    }

    private IEnumerator AnimateCurvedWorld()
    {

        while (true)
        {
            float currHorizontalSize = CW_Controller.bendHorizontalSize;
            float currVerticalSize = CW_Controller.bendVerticalSize;

            float progress = 0f;
            float duration = 5f;

            float bendSizeX = Random.Range(Xmin, Xmax);
            float bendSizeY = Random.Range(Ymin, Ymax);

            while (progress < 1f)
            {
                CW_Controller.SetBendHorizontalSize(Mathf.Lerp(currHorizontalSize, bendSizeX, progress));
                CW_Controller.SetBendVerticalSize(Mathf.Lerp(currVerticalSize, bendSizeY, progress));

                progress += Time.deltaTime / duration;
                yield return null;
            }

            yield return null;
        }
    }
}
