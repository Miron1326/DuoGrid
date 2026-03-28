using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public Volume GlobalVolume;
    private Bloom bloomEffect;

    void Start()
    {
        GlobalVolume = GameObject.Find("GlobalPostProcessing").GetComponent<Volume>();

        if (GlobalVolume.profile.TryGet(out bloomEffect))
        {
            bloomEffect.tint.overrideState = true;
        }
        else
        {
            Debug.LogError("Bloom не найден в профиле!");
        }
    }

    public void MageActivate()
    {
        bloomEffect.tint.value = Color.red;
    }
    public void MageDiactivate()
    {
        bloomEffect.tint.value = Color.white;
    }
}
