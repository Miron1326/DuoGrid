using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public CharacterData CharacterDataPlayer1;
    public CharacterData CharacterDataPlayer2;
    public List<CanvasGroup> canvasesForPlayerCheckPlayer1 = new List<CanvasGroup>();
    public List<CanvasGroup> canvasesForPlayerCheckPlayer2 = new List<CanvasGroup>();

    private void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
    }

    public void PlayerSellected()
    {
        if(CharacterDataPlayer1 != null)
        {
            switch (CharacterDataPlayer1.CharacterName)
            {
                case "BaseMage":
                    Player1SelectBasicCharacter();
                    break;
            }
            foreach (CanvasGroup canvas in canvasesForPlayerCheckPlayer1)
            {
                canvas.interactable = false;
                canvas.blocksRaycasts = false;
                canvas.alpha = 0;
            }
        }
        if (CharacterDataPlayer2 != null)
        {
            switch (CharacterDataPlayer2.CharacterName)
            {
                case "BaseMage":
                    Player2SelectBasicCharacter();
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
    }

    public void Player1SelectBasicCharacter()
    {
        CharacterDataPlayer1 = Resources.Load<CharacterData>("CharactersData/BasicMage");
    }

    public void Player2SelectBasicCharacter()
    {
        CharacterDataPlayer2 = Resources.Load<CharacterData>("CharactersData/BasicMage");
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
}
