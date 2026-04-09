using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectListener : MonoBehaviour
{
    public UIPlayersCurse UIPlayersCurse;
    public string PlayerName;
    public PlayerEffect currentEffect;
    public int TurnsToActive;
    public int TurnsToNonActive;
    private CanvasGroup buttonAbilityPlayer1;
    private CanvasGroup buttonAbilityPlayer2;
    private CharacterManager characterManager;

    private void Start()
    {
        UIPlayersCurse = GameObject.Find("BloodManager").GetComponent<UIPlayersCurse>();
        buttonAbilityPlayer1 = GameObject.Find("PlayerAbilityUse1").GetComponent<CanvasGroup>();
        buttonAbilityPlayer2 = GameObject.Find("PlayerAbilityUse2").GetComponent<CanvasGroup>();
        characterManager = GameObject.Find("GameManager").GetComponent<CharacterManager>();
        UIPlayersCurse.FadeCursePlayer1();
        UIPlayersCurse.FadeCursePlayer2();
    }

    public void AddEffect(PlayerEffect effectAdd)
    {

        if(PlayerName == "Player1")
        {
            UIPlayersCurse.ShowCursePlayer1();
        }
        if(PlayerName == "Player2")
        {
            UIPlayersCurse.ShowCursePlayer2();
        }

        currentEffect = effectAdd;
        InitializeEffect();
        GameManager.Instance.OnSwitchTurn += EffectsCheck;
        if(PlayerName == "Player1")
        {
            GameManager.Instance.OnTakeDamagePlayer1 += CheckToDestroyEffectAfterDamagePlayer1;
        }
        if(PlayerName == "Player2")
        {
            GameManager.Instance.OnTakeDamagePlayer2 += CheckToDestroyEffectAfterDamagePlayer2;
        }

    }

    private void InitializeEffect()
    {
        switch (currentEffect)
        {
            case PlayerEffect.poisonedBlood:
                TurnsToActive = 3;
                TurnsToNonActive = 5;
                break;
        }
    }

    private void EffectsCheck()
    {
        if (TurnsToActive <= 0)
        {
            switch(currentEffect)
            {
                case PlayerEffect.poisonedBlood:
                    if (PlayerName == "Player1")
                    {
                        CantSeeButtonPlayer1();
                    }
                    if(PlayerName == "Player2")
                    {
                        CantSeeButtonPlayer2();
                    }
                    break;
            }
            if(TurnsToNonActive <= 0)
            {
                if (PlayerName == "Player1")
                {
                    CanSeeButtonPlayer1();
                    DeleteEffect();
                }
                if (PlayerName == "Player2")
                {
                    CanSeeButtonPlayer2();
                    DeleteEffect();
                }
            }
            else
            {
                TurnsToNonActive--;
            }
        }
        else
        {
            TurnsToActive--;
        }
    }

    private void CheckToDestroyEffectAfterDamagePlayer1()
    {
        switch(currentEffect)
        {
            case PlayerEffect.poisonedBlood:
                DeleteEffect();
                break;
        }
    }

    private void CheckToDestroyEffectAfterDamagePlayer2()
    {
        switch (currentEffect)
        {
            case PlayerEffect.poisonedBlood:
                DeleteEffect();
                break;
        }
    }


    public void DeleteEffect()
    {
        currentEffect = PlayerEffect.None;

        if (PlayerName == "Player1")
        {
            UIPlayersCurse.FadeCursePlayer1();
        }
        if (PlayerName == "Player2")
        {
            UIPlayersCurse.FadeCursePlayer2();
        }
    }

    //Effectscomp

    private void CantSeeButtonPlayer1()
    {
        buttonAbilityPlayer1.interactable = false;
        buttonAbilityPlayer1.blocksRaycasts = false;
        buttonAbilityPlayer1.alpha = 0f;
    }
    private void CanSeeButtonPlayer1()
    {
        if (characterManager.CharacterDataPlayer1.abilityForCharacter == null) return;
        buttonAbilityPlayer1.interactable = true;
        buttonAbilityPlayer1.blocksRaycasts = true;
        buttonAbilityPlayer1.alpha = 1f;
    }

    private void CantSeeButtonPlayer2()
    {
        buttonAbilityPlayer2.interactable = false;
        buttonAbilityPlayer2.blocksRaycasts = false;
        buttonAbilityPlayer2.alpha = 0f;
    }

    private void CanSeeButtonPlayer2()
    {
        if (characterManager.CharacterDataPlayer2.abilityForCharacter == null) return;
        buttonAbilityPlayer2.interactable = true;
        buttonAbilityPlayer2.blocksRaycasts = true;
        buttonAbilityPlayer2.alpha = 1f;
    }

    public void RestartEffectListener()
    {
        if(PlayerName == "Player1")
        {
            currentEffect = PlayerEffect.None;
            if (characterManager.abilityButtonPlayer1.thisAbility != null)
            {
                CanSeeButtonPlayer1();
            }
        }
        if(PlayerName == "Player2")
        {
            currentEffect = PlayerEffect.None;
            if (characterManager.abilityButtonPlayer2.thisAbility != null)
            {
                CanSeeButtonPlayer2();
            }
        }
    }
}
