using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region  Общий
    private void Start()
    {

    }

    #endregion Общий


    #region ИнвентариСоздание

    [SerializeField] private PlayerInventory playerInventoryPlayer1;
    [SerializeField] private PlayerInventory playerInventoryPlayer2;
    [SerializeField] private GameObject gameObjectNew;

    public void UpdateInventory(string PlayerName, bool Delete = false, int indexOnDelete = 0)
    {
        if(Delete)
        {
            if (PlayerName == "Player1")
            {
                gameObjectNew = GameObject.Find("ItemSlot1" + indexOnDelete);
                UIItemSlot itemSlot = gameObjectNew.GetComponent<UIItemSlot>();
                itemSlot.itemInThisSlot = null;
                itemSlot.UpdateVisual();
            }
            if(PlayerName == "Player2")
            {
                gameObjectNew = GameObject.Find("ItemSlot2" + indexOnDelete);
                UIItemSlot itemSlot = gameObjectNew.GetComponent<UIItemSlot>();
                itemSlot.itemInThisSlot = null;
                itemSlot.UpdateVisual();
            }
            return;
        }
        if (PlayerName == "Player1")
        {
            playerInventoryPlayer1 = GameManager.Instance.GetPlayerInventory("Player1");
            int indexNew = playerInventoryPlayer1.data.Count - 1;
            gameObjectNew = GameObject.Find("ItemSlot1" + indexNew);
        }
        else
        {
            playerInventoryPlayer2 = GameManager.Instance.GetPlayerInventory("Player2");
            int indexNew = playerInventoryPlayer2.data.Count - 1;
            gameObjectNew = GameObject.Find("ItemSlot2" + indexNew);
        }

        if(playerInventoryPlayer1.data.Count != 0)
        {
            for (int index = 0; index < playerInventoryPlayer1.data.Count || index < GameManager.Instance.MaxNumbersOfItem; index++)
            {
                GameObject slotItemPlayer1 = GameObject.Find("ItemSlot1" + index);
                UIItemSlot itemSlot = slotItemPlayer1.GetComponent<UIItemSlot>();
                itemSlot.itemInThisSlot = playerInventoryPlayer1.data[index];
                itemSlot.UpdateVisual();
            }
        }
        

        if (playerInventoryPlayer2.data.Count != 0)
        {
            for (int index = 0; index < playerInventoryPlayer2.data.Count || index < GameManager.Instance.MaxNumbersOfItem; index++)
            {
                GameObject slotItemPlayer2 = GameObject.Find("ItemSlot2" + index);
                UIItemSlot itemSlot = slotItemPlayer2.GetComponent<UIItemSlot>();
                itemSlot.itemInThisSlot = playerInventoryPlayer2.data[index];
                itemSlot.UpdateVisual();
            }
        }

    }


    #endregion ИнвентариСоздание



    #region ПредметныйUI


    public void UpdateUIItems(string PlayerItems, PlayerInventory player1Inventory, PlayerInventory player2Inventory)
    {
        if(PlayerItems == "1")
        {
            Text HaveFirstText = GameObject.Find("PlayerItem10Text").GetComponent<Text>();
            HaveFirstText.text = "";
            Text HaveTwoText = GameObject.Find("PlayerItem11Text").GetComponent<Text>();
            HaveTwoText.text = "";
            string FirstItemName = "";
            if (player1Inventory.data.Count == 0)
            {
                GameManager.Instance.Player1ItemsCloseCanvas();
                return;
            }
            else
            {
                FirstItemName = player1Inventory.data[0].itemName;
            }

            
            


            if (FirstItemName == "Poison")
            {
                HaveFirstText.text = $"{GameManager.Instance.player1PoisonHave} {player1Inventory.data[0].displayName}";
            }
            else
            {
                HaveFirstText.text = $"{GameManager.Instance.ItemStatsPlayer1[FirstItemName]} {player1Inventory.data[0].displayName}";
            }

            if (player1Inventory.data.Count > 1)
            {
                
                string TwoItemName = player1Inventory.data[1].itemName;
                if (TwoItemName == "Poison")
                {
                    HaveTwoText.text = $"{GameManager.Instance.player1PoisonHave} {player1Inventory.data[1].displayName}";
                }
                else
                {
                    HaveTwoText.text = $"{GameManager.Instance.ItemStatsPlayer1[TwoItemName]} {player1Inventory.data[1].displayName}";
                }
            }
            
            
        }
        else
        {
            Text HaveFirstText = GameObject.Find("PlayerItem20Text").GetComponent<Text>();
            HaveFirstText.text = "";
            Text HaveTwoText = GameObject.Find("PlayerItem21Text").GetComponent<Text>();
            HaveTwoText.text = "";

            string FirstItemName = "";
            if (player2Inventory.data.Count == 0)
            {
                GameManager.Instance.Player2ItemsCloseCanvas();
                return;
            }
            else
            {
                FirstItemName = player2Inventory.data[0].itemName;
            }
            
            if (FirstItemName == "Poison")
            {
                HaveFirstText.text = $"{GameManager.Instance.player2PoisonHave} {player2Inventory.data[0].displayName}";
            }
            else
            {
                HaveFirstText.text = $"{GameManager.Instance.ItemStatsPlayer2[FirstItemName]} {player2Inventory.data[0].displayName}";
            }

            if (player2Inventory.data.Count > 1)
            {
                string TwoItemName = player2Inventory.data[1].itemName;
                if (TwoItemName == "Poison")
                {
                    HaveTwoText.text = $"{GameManager.Instance.player2PoisonHave} {player2Inventory.data[1].displayName}";
                }
                else
                {
                    HaveTwoText.text = $"{GameManager.Instance.ItemStatsPlayer2[TwoItemName]} {player2Inventory.data[1].displayName}";
                }
            }
            
        }
    }

    #endregion ПредметныйUI
}
