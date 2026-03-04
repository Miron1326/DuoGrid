using UnityEngine;
using UnityEngine.UI;

public class CanvasSizeChanger : MonoBehaviour
{
    public Vector2 pcResolution = new Vector2(1920, 1080);
    public Vector2 PhoneResolution = new Vector2(1920, 1080);
    private float pcMatch = 0.0f;
    private float phoneMatch = 0f;
    private CanvasScaler[] canvasec;
    void Start()
    {
        canvasec = FindObjectsOfType<CanvasScaler>();

#if UNITY_ANDROID
        foreach (CanvasScaler scaler in canvasec)
        {
            scaler.referenceResolution = PhoneResolution;
            scaler.matchWidthOrHeight = phoneMatch;
            Debug.Log("Android");
        }
#else
        foreach (CanvasScaler scaler in canvasec)
            {
                scaler.referenceResolution = pcResolution;
                scaler.matchWidthOrHeight = pcMatch;
                Debug.Log("PC");
            }
#endif

        
    }

}
