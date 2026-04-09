using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        audioSource.volume = PlayerPrefs.GetFloat(SettingsManager.Instance.keyVolumeMusic, 0.7f);
    }

}
