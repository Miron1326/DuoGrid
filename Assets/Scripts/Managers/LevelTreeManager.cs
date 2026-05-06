using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class LevelTreeManager : MonoBehaviour
{
    public RectTransform center;
    public ScillSO startLevelStandart;
    public ScillSO startLevelTechnichion;

    public ScillSO currentLevelPlayer1;
    public ScillSO currentLevelPlayer2;

    public List<ScillSO> UpdatesPlayer1 = new List<ScillSO>();
    public List<ScillSO> UpdatesPlayer2 = new List<ScillSO>();

    public CharacterData CharacterDataPlayer1;
    public int LevelSkipEffectPlayer1;
    public CharacterData CharacterDataPlayer2;
    public int LevelSkipEffectPlayer2;

    private Vector3 startPosLeft;
    private string PlayerWin;
    [SerializeField] private Image firstLevelUpImage;
    [SerializeField] private Image twoLevelUpImage;
    [SerializeField] private CanvasGroup levelUI;
    [SerializeField] private Button firstLevelButton;
    [SerializeField] private Button twoLevelButton;
    [SerializeField] private AbilityButton firstAbility;
    [SerializeField] private AbilityButton twoAbility;
    [SerializeField] private CanvasGroup canvasGroupAddSlot;
    [SerializeField] private CellInShop cellAddCell;
    [SerializeField] private Text FirstText;
    [SerializeField] private Text TwiceText;
    private bool TryEveryTurnAddSlot = false;
    private bool TryEveryTurnRegeneration = false;
    private bool player1HaveAddSlot = false;
    private bool player2HaveAddSlot = false;
    private bool player1HaveRegeneration = false;
    private int currentRegeneraionCooldownPlayer1;
    private int currentRegeneraionCooldownPlayer2;
    private int TurnsEndedPlayer1BeforeRegen = 0;
    private int TurnsEndedPlayer2BeforeRegen = 0;
    private bool player2HaveRegeneration = false;
    public int levelOfCellInNewSlotPlayer1;
    public int levelOfCellInNewSlotPlayer2;

    void Start()
    {
        levelUI.alpha = 0;
        levelUI.interactable = false;
        levelUI.blocksRaycasts = false;
        GameManager.Instance.OnPlayer1Win += OnPlayer1Win;
        GameManager.Instance.OnPlayer2Win += OnPlayer2Win;
        startPosLeft = firstLevelUpImage.rectTransform.position;
        firstLevelButton.onClick.AddListener(() => OnLevelUpSelectedAndAdd("one"));
        twoLevelButton.onClick.AddListener(() => OnLevelUpSelectedAndAdd("two"));
        FadeNewCell();
    }

    public void OnLevelUpSelectedAndAdd(string Level)
    {
        if(PlayerWin == "Player1")
        {
            if (Level == "one")
            {
                currentLevelPlayer1 = currentLevelPlayer1.scillsNext[0];
            }
            if (Level == "two")
            {
                currentLevelPlayer1 = currentLevelPlayer1.scillsNext[1];
            }
            UpdatesPlayer1.Add(currentLevelPlayer1);
        }
        if (PlayerWin == "Player2")
        {
            if (Level == "one")
            {
                currentLevelPlayer2 = currentLevelPlayer2.scillsNext[0];
            }
            if (Level == "two")
            {
                currentLevelPlayer2 = currentLevelPlayer2.scillsNext[1];
            }
            UpdatesPlayer2.Add(currentLevelPlayer2);
        }
        ActivateEffect(PlayerWin);
        levelUI.alpha = 0;
        levelUI.interactable = false;
        levelUI.blocksRaycasts = false;

    }

    private void ActivateEffect(string Player)
    {
        if(Player == "Player1")
        {
            if (currentLevelPlayer1.PlayerUpHealth)
            {
                GameManager.Instance.HealPlayer(Player, currentLevelPlayer1.HealthUp);
            }
            if (currentLevelPlayer1.ZoneBuffOfAbility)
            {
                firstAbility.Changed = true;
                firstAbility.MoggedZone += currentLevelPlayer1.BuffTo;
            }
            if (currentLevelPlayer1.AddNewSlot)
            {
                player1HaveAddSlot = true;
                VisionNewCell();
                cellAddCell.ChangeCellType(currentLevelPlayer1.EffectTypeInNewSlot);
                if (!TryEveryTurnAddSlot)
                {
                    GameManager.Instance.OnSwitchTurn += TryToVisionNewCell;
                }
                levelOfCellInNewSlotPlayer1 = currentLevelPlayer1.LevelEffectTypeInNewSlot;
                TryEveryTurnAddSlot = true;
            }
            if (currentLevelPlayer1.AddRegeneration)
            {
                player1HaveRegeneration = true;
                currentRegeneraionCooldownPlayer1 = currentLevelPlayer1.TurnsForRegeneration;
                if (!TryEveryTurnRegeneration)
                {
                    GameManager.Instance.OnSwitchTurn += TryToRegeneration;
                }

                TryEveryTurnRegeneration = true;
            }
            if (currentLevelPlayer1.AddedChanceToSkipCell != 0)
            {
                LevelSkipEffectPlayer1 += currentLevelPlayer1.AddedChanceToSkipCell;
            }
        }
        else
        {
            if (currentLevelPlayer2.PlayerUpHealth)
            {
                GameManager.Instance.HealPlayer(Player, currentLevelPlayer2.HealthUp);
            }
            if (currentLevelPlayer2.ZoneBuffOfAbility)
            {
                twoAbility.Changed = true;
                twoAbility.MoggedZone += currentLevelPlayer2.BuffTo;
            }
            if (currentLevelPlayer2.AddNewSlot)
            {
                player2HaveAddSlot = true;
                VisionNewCell();
                cellAddCell.ChangeCellType(currentLevelPlayer2.EffectTypeInNewSlot);

                if (!TryEveryTurnAddSlot)
                {
                    GameManager.Instance.OnSwitchTurn += TryToVisionNewCell;
                }
                TryEveryTurnAddSlot = true;
                levelOfCellInNewSlotPlayer2 = currentLevelPlayer2.LevelEffectTypeInNewSlot;
            }
            if (currentLevelPlayer2.AddRegeneration)
            {
                player2HaveRegeneration = true;
                currentRegeneraionCooldownPlayer2 = currentLevelPlayer2.TurnsForRegeneration;
                if (!TryEveryTurnRegeneration)
                {
                    GameManager.Instance.OnSwitchTurn += TryToRegeneration;
                }

                TryEveryTurnRegeneration = true;
            }
            if (currentLevelPlayer2.AddedChanceToSkipCell != 0)
            {
                LevelSkipEffectPlayer2 += currentLevelPlayer2.AddedChanceToSkipCell;
            }
        }

    }

    private void TryToVisionNewCell()
    {
        if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2 && player1HaveAddSlot)
        {
            VisionNewCell();
        }else if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2 && player2HaveAddSlot)
        {
            VisionNewCell();
        }
        else
        {
            FadeNewCell();
        }
    }

    private void TryToRegeneration()
    {
        if (player1HaveRegeneration)
        {
            TurnsEndedPlayer1BeforeRegen++;
            if(TurnsEndedPlayer1BeforeRegen >= currentRegeneraionCooldownPlayer1)
            {
                GameManager.Instance.HealPlayer("Player1", 1);
                TurnsEndedPlayer1BeforeRegen = 0;
            }
        }
        if (player2HaveRegeneration)
        {
            TurnsEndedPlayer2BeforeRegen++;
            if (TurnsEndedPlayer2BeforeRegen >= currentRegeneraionCooldownPlayer2)
            {
                GameManager.Instance.HealPlayer("Player2", 1);
                TurnsEndedPlayer2BeforeRegen = 0;
            }
        }
    }

    private void FadeNewCell()
    {
        canvasGroupAddSlot.alpha = 0;
        canvasGroupAddSlot.interactable = false;
        canvasGroupAddSlot.blocksRaycasts = false;
    }

    private void VisionNewCell()
    {
        canvasGroupAddSlot.alpha = 1;
        canvasGroupAddSlot.interactable = true;
        canvasGroupAddSlot.blocksRaycasts = true;
    }

    public void OnPlayer1Win()
    {
        PlayerWin = "Player1";
        FirstText.text = currentLevelPlayer1.scillsNext[0].description;
        if (currentLevelPlayer1.scillsNext[0].scillsNext.Count == 0)
        {
            FirstText.color = Color.red;
        }
        else
        {
            FirstText.color = Color.black;
        }
        if (currentLevelPlayer1.scillsNext.Count > 1)
        {
            TwiceText.text = currentLevelPlayer1.scillsNext[1].description;
            if (currentLevelPlayer1.scillsNext[1].scillsNext.Count == 0)
            {
                TwiceText.color = Color.red;
            }
            else
            {
                TwiceText.color = Color.black;
            }
        }
        else
        {
            TwiceText.text = "";
        }

        if (currentLevelPlayer1.scillsNext.Count != 0)
        {
            levelUI.alpha = 1;
            levelUI.interactable = true;
            levelUI.blocksRaycasts = true;
            firstLevelUpImage.sprite = currentLevelPlayer1.scillsNext[0].icon;
            if(currentLevelPlayer1.scillsNext.Count == 1)
            {
                twoLevelUpImage.enabled = false;
                firstLevelUpImage.rectTransform.position = center.position;
            }
            else
            {
                firstLevelUpImage.rectTransform.position = startPosLeft;
                twoLevelUpImage.enabled = true;
                twoLevelUpImage.sprite = currentLevelPlayer1.scillsNext[1].icon;
            }

        }
    }

    public void OnPlayer2Win()
    {
        PlayerWin = "Player2";
        FirstText.text = currentLevelPlayer2.scillsNext[0].description;
        if (currentLevelPlayer2.scillsNext[0].scillsNext.Count == 0)
        {
            FirstText.color = Color.red;
        }
        else
        {
            FirstText.color = Color.black;
        }
        if(currentLevelPlayer2.scillsNext.Count > 1)
        {
            TwiceText.text = currentLevelPlayer2.scillsNext[1].description;
            if (currentLevelPlayer2.scillsNext[1].scillsNext.Count == 0)
            {
                TwiceText.color = Color.red;
            }
            else
            {
                TwiceText.color = Color.black;
            }
        }
        else
        {
            TwiceText.text = "";
        }

        if (currentLevelPlayer2.scillsNext.Count != 0)
        {
            levelUI.alpha = 1;
            levelUI.interactable = true;
            levelUI.blocksRaycasts = true;
            firstLevelUpImage.sprite = currentLevelPlayer2.scillsNext[0].icon;
            firstLevelUpImage.sprite = currentLevelPlayer2.scillsNext[0].icon;
            if (currentLevelPlayer2.scillsNext.Count == 1)
            {
                twoLevelUpImage.enabled = false;
                firstLevelUpImage.rectTransform.position = center.position;
            }
            else
            {
                firstLevelUpImage.rectTransform.position = startPosLeft;
                twoLevelUpImage.enabled = true;
                twoLevelUpImage.sprite = currentLevelPlayer2.scillsNext[1].icon;
            }
        }
    }


    public void UpdateStandartEffect()
    {
        if (CharacterDataPlayer1 != null)
        {
            switch (CharacterDataPlayer1.CharacterName)
            {
                case "BasicMage":
                    currentLevelPlayer1 = startLevelStandart;
                    break;
                case "Technichial":
                    currentLevelPlayer1 = startLevelTechnichion;
                    break;

            }
        }
        
        if(CharacterDataPlayer1 != null && !UpdatesPlayer1.Contains(currentLevelPlayer1))
        {
            UpdatesPlayer1.Add(currentLevelPlayer1);
        }
        if(CharacterDataPlayer2 != null)
        {
            switch (CharacterDataPlayer2.CharacterName)
            {
                case "BasicMage":
                    currentLevelPlayer2 = startLevelStandart;
                    break;
                case "Technichial":
                    currentLevelPlayer2 = startLevelTechnichion;
                    break;
            }
        }
        
        if (CharacterDataPlayer2 != null && !UpdatesPlayer2.Contains(currentLevelPlayer1))
        {
            UpdatesPlayer2.Add(currentLevelPlayer2);
        }
    }

    public void Restart()
    {
        if(player1HaveRegeneration || player2HaveRegeneration)
        {
            GameManager.Instance.OnSwitchTurn -= TryToRegeneration;
            player2HaveRegeneration = false;
            player1HaveRegeneration = false;
        }
        if(firstAbility.Changed == true)
        {
            firstAbility.Changed = false;
            firstAbility.MoggedZone = 0;
        }
        if (twoAbility.Changed == true)
        {
            twoAbility.Changed = false;
            twoAbility.MoggedZone = 0;
        }
        if(player1HaveAddSlot || player2HaveAddSlot)
        {
            GameManager.Instance.OnSwitchTurn -= TryToVisionNewCell;
            player1HaveAddSlot = false;
            player2HaveAddSlot = false;
        }
        UpdatesPlayer1.Clear();
        UpdatesPlayer2.Clear();
        UpdateStandartEffect();
    }

}
