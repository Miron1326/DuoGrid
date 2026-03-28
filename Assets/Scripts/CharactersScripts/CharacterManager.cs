using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public AbilityButton abilityButtonPlayer1;
    public AbilityButton abilityButtonPlayer2;
    public CharacterData CharacterDataPlayer1;
    public CharacterData CharacterDataPlayer2;
    private Button ButtonPlayer1;
    private Button ButtonPlayer2;
    public List<CanvasGroup> canvasesForPlayerCheckPlayer1 = new List<CanvasGroup>();
    public List<CanvasGroup> canvasesForPlayerCheckPlayer2 = new List<CanvasGroup>();

    private void Start()
    {
        ButtonPlayer1 = GameObject.Find("PlayerAbilityUse1").GetComponent<Button>();
        ButtonPlayer2 = GameObject.Find("PlayerAbilityUse2").GetComponent<Button>();
        FadePlayerAbilities();
        GameManager.Instance.OnSwitchTurn += FadePlayerAbilities;
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        abilityButtonPlayer1 = GameObject.Find("PlayerAbilityUse1").GetComponent<AbilityButton>();
        abilityButtonPlayer2 = GameObject.Find("PlayerAbilityUse2").GetComponent<AbilityButton>();
    }

    public void PlayerSellected()
    {
        string playerName = null;
        if(CharacterDataPlayer1 != null)
        {
            
            playerName = "Player1";
            switch (CharacterDataPlayer1.CharacterName)
            {
                case "BaseMage":
                    Player1SelectBasicCharacter();
                    break;
                case "Technichial":
                    Player1SelectTechnic();
                    break;
            }
            foreach (CanvasGroup canvas in canvasesForPlayerCheckPlayer1)
            {
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
                canvas.alpha = 0;
            }
            if (CharacterDataPlayer2 != null)
            {
                Standart(playerName);
                UpdateVisual();
            }
        }
        if (CharacterDataPlayer2 != null)
        {
            playerName = "Player2";
            switch (CharacterDataPlayer2.CharacterName)
            {
                case "BaseMage":
                    Player2SelectBasicCharacter();
                    break;
                case "Technichial":
                    Player2SelectTechnic();
                    break;
            }
            foreach (CanvasGroup canvas in canvasesForPlayerCheckPlayer2)
            {
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
                canvas.alpha = 0;
            }
        }
        UpdateVisual();
        Standart(playerName);
    }

    //Player1
    public void Player1SelectBasicCharacter()
    {
        CharacterDataPlayer1 = Resources.Load<CharacterData>("CharactersData/BasicMage");
    }
    public void Player1SelectTechnic()
    {
        CharacterDataPlayer1 = Resources.Load<CharacterData>("CharactersData/Technichial");
    }


    //Player2

    public void Player2SelectBasicCharacter()
    {
        CharacterDataPlayer2 = Resources.Load<CharacterData>("CharactersData/BasicMage");
    }

    public void Player2SelectTechnic()
    {
        CharacterDataPlayer2 = Resources.Load<CharacterData>("CharactersData/Technichial");
    }

    private void Standart(string PlayerName)
    {
        
        if(PlayerName == "Player1")
        {
            abilityButtonPlayer1.PlayerName = PlayerName;
            abilityButtonPlayer1.AbilityInitialize(CharacterDataPlayer1);
        }
        if(PlayerName == "Player2")
        {
            abilityButtonPlayer2.PlayerName = PlayerName;
            abilityButtonPlayer2.AbilityInitialize(CharacterDataPlayer2);
        }
    }

    public void UpdateVisual()
    {
        MovementOnTheMouseManager movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouseManager.NewPrephabToMovement();
        SpriteRenderer spriteRendererPlayer1 = player1.GetComponent<SpriteRenderer>();
        SpriteRenderer spriteRendererPlayer2 = player2.GetComponent<SpriteRenderer>();

        if(CharacterDataPlayer1 != null)
        {
            switch (CharacterDataPlayer1.CharacterName)
            {
                case "BaseMage":
                    spriteRendererPlayer1.sprite = Resources.Load<Sprite>("sprites/Characters/Ashen Wanderer");
                    break;
                case "Technichial":
                    spriteRendererPlayer1.sprite = Resources.Load<Sprite>("sprites/Characters/Technichion");
                    break;
            }
        }
        
        if (CharacterDataPlayer2 != null)
        {
            switch (CharacterDataPlayer2.CharacterName)
            {
                case "BaseMage":
                    spriteRendererPlayer2.sprite = Resources.Load<Sprite>("sprites/Characters/Ashen WandererTwo");
                    break;
                case "Technichial":
                    spriteRendererPlayer2.sprite = Resources.Load<Sprite>("sprites/Characters/Technichion");
                    break;
            }
        }
    }

    public void FadePlayerAbilities()
    {
        if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
        {
            if(ButtonPlayer1 != null)
            {
                ButtonPlayer1.interactable = true;
            }

            if(ButtonPlayer2 != null)
            {
                ButtonPlayer2.interactable = false;
            }

        }
        if(GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
        {
            if (ButtonPlayer1 != null)
            {
                ButtonPlayer1.interactable = false;
            }

            if (ButtonPlayer2 != null)
            {
                ButtonPlayer2.interactable = true;
            }

        }
    }
}
