using UnityEngine;

public class ItemAddToInventory : MonoBehaviour
{
    public ItemData currentItem;
    public string ItemPlayer;

    public void OnClickButton()
    {
        if (!GameManager.Instance.PlayerContainsItem(ItemPlayer, currentItem))
        {
            GameManager.Instance.AddItemPlayer(ItemPlayer, currentItem);
        }
        else
        {
            GameManager.Instance.DeleteItemFromInventory(ItemPlayer, currentItem);
        }

    }
}
