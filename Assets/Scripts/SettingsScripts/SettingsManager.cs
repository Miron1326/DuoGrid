using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider WinsSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private CanvasGroup canvasGroupSettings;
    [SerializeField] private Text winsText;
    [SerializeField] private Text musicText;
    public string keyWins = "WinsForWin";
    public string keyVolumeMusic = "VolumeMusic";

    public static SettingsManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        FadePanel();
        WinsSlider.onValueChanged.AddListener(OnVolumeChangedWinsCondition);
        MusicSlider.onValueChanged.AddListener(OnVolumeChangedMusic);
    }

    private void OnVolumeChangedWinsCondition(float value)
    {
        PlayerPrefs.SetInt(keyWins, (int)value);
        PlayerPrefs.Save();

        if((int)value == 0)
        {
            winsText.text = $"Необходимо побед - {(int)value} (по умолчанию)";
        }
        else
        {
            winsText.text = $"Необходимо побед - {(int)value}";
        }
        
    }

    private void OnVolumeChangedMusic(float value)
    {
        PlayerPrefs.SetFloat(keyVolumeMusic, value);
        PlayerPrefs.Save();

        if (value == 0.7)
        {
            musicText.text = $"Громкость музыки - {value} (по умолчанию)";
        }
        else
        {
            musicText.text = $"Громкость музыки - {value:F1}";
        }


    }

    public void ShowPanel()
    {
        canvasGroupSettings.alpha = 1;
        canvasGroupSettings.blocksRaycasts = true;
        canvasGroupSettings.interactable = true;
    }

    public void FadePanel()
    {
        canvasGroupSettings.alpha = 0;
        canvasGroupSettings.blocksRaycasts = false;
        canvasGroupSettings.interactable = false;
    }
}
