using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BloodMagesticManager : MonoBehaviour
{
    public event Action OnBloodMageVisit;

    public EffectListener effectListenerPlayer1;
    public EffectListener effectListenerPlayer2;
    private EffectManager effectManager;
    public int ChanceforMage = 33;
    private CameraManager CameraManager;
    private GameObject FirstMage;
    private GameObject Center;
    public bool EffectSelected;

    void Start()
    {
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
                    FirstMageActivate();
                    break;
                case 2:
                    FirstMageActivate();
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



