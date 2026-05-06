using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BloodMagesticManager : MonoBehaviour
{
    public event Action OnBloodMageVisit;

    public bool EffectSelected;
    private int hpExchange;
    private string playerToExchange;
    public EffectListener effectListenerPlayer1;
    public EffectListener effectListenerPlayer2;
    private EffectManager effectManager;
    public int ChanceforMage = 33;
    private CameraManager CameraManager;
    private GameObject FirstMage;
    private GameObject Center;
    [SerializeField] private CanvasGroup ExchangeCanvasGroup;
    [SerializeField] private Slider ExchangeSlider;
    [SerializeField] private Text ExchangeText;
    [SerializeField] private Text ExchangeTextWithName;
    [SerializeField] private Button buttonToExchange;


    void Start()
    {
        buttonToExchange.onClick.AddListener(OnEchange);
        ExchangeSlider.onValueChanged.AddListener(OnExchangeChangeText);
        ExchangeCanvasGroup.alpha = 0;
        ExchangeCanvasGroup.blocksRaycasts = false;
        ExchangeCanvasGroup.interactable = false;
        effectListenerPlayer1 = GameObject.Find("Player1").GetComponent<EffectListener>();
        effectListenerPlayer2 = GameObject.Find("Player2").GetComponent<EffectListener>();
        effectManager = GameObject.Find("GameManager").GetComponent<EffectManager>();
        CameraManager = GameObject.Find("CameraManager").GetComponent<CameraManager>();
        FirstMage = GameObject.Find("FirstMage");
        Center = GameObject.Find("MagesticCenter");
        GameManager.Instance.OnTakeDamage += OnPlayerTakeDamage;
        GameManager.Instance.OnSwitchTurn += OnTurnEnded;
    }

    private void OnTurnEnded()
    {
        if (!EffectSelected)
        {
            ChanceforMage++;
        }
        if (EffectSelected)
        {
            ChanceforMage = 33;
        }
    }

    private void OnPlayerTakeDamage()
    {
        int RandomNumberForInvoke = UnityEngine.Random.Range(0, 101);
        int RandomNumberEffect = UnityEngine.Random.Range(0, 3);
        if (RandomNumberForInvoke <= ChanceforMage) // Activate
        {
            if (EffectSelected) return;
            EffectSelected = true;
            Debug.Log("FAR2");
            OnBloodMageVisit.Invoke();
            switch (RandomNumberEffect)
            {
                case 0:
                    FirstMageActivate();
                    break;
                case 1:
                    TwiceMageActivate();
                    break;
                case 2:
                    TwiceMageActivate();
                    break;
            }
            
            
            OnTurnEnded();

        }
    }

    private void FirstMageActivate()
    {
        FirstMage.transform.DOMove(Center.transform.position, 1);
        CameraManager.MageActivate();
        effectManager.StartBloodRain();
        effectListenerPlayer1.AddEffect(PlayerEffect.poisonedBlood);
        effectListenerPlayer2.AddEffect(PlayerEffect.poisonedBlood);
        Invoke("FirstMageDiactivate", 5);
    }

    private void TwiceMageActivate()
    {
        int randomInt = UnityEngine.Random.Range(0, 101);
        if(randomInt <= 50)
        {
            playerToExchange = "1";
        }
        else
        {
            playerToExchange = "2";
        }
        FirstMage.transform.DOMove(Center.transform.position, 1);
        ExchangeCanvasGroup.alpha = 1;
        ExchangeCanvasGroup.blocksRaycasts = true;
        ExchangeCanvasGroup.interactable = true;
    }

    private void OnExchangeChangeText(float value)
    {
        hpExchange = (int)value;
        ExchangeText.text = $"Разменять {(int)value} здоровья на {(int)value / 2} побед";
        ExchangeTextWithName.text = $"Предложение рзамена игроку {playerToExchange}";
    }

    private void OnEchange()
    {
        GameManager.Instance.ExchangeHPForWins(playerToExchange, hpExchange, (int)hpExchange / 2);
        ExchangeCanvasGroup.alpha = 0;
        ExchangeCanvasGroup.blocksRaycasts = false;
        ExchangeCanvasGroup.interactable = false;
    }

    private void FirstMageDiactivate()
    {
        FirstMage.transform.DOMove(new Vector3(0, 13, 0), 1);
        CameraManager.MageDiactivate();
        EffectSelected = false;
    }

    public void Restart()
    {
        ChanceforMage = 33;
        effectListenerPlayer1.RestartEffectListener();
        effectListenerPlayer2.RestartEffectListener();
        effectManager.StopBloodRain();
        FirstMageDiactivate();
    }

    public void AddEffectToPlayer(string playerName, PlayerEffect playerEffect)
    {
        if(playerName == "Player1")
        {
            effectListenerPlayer1.AddEffect(playerEffect);
        }
        else
        {
            effectListenerPlayer2.AddEffect(playerEffect);
        }
    }
}



