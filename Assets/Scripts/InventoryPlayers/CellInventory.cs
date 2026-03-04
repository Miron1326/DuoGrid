using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellInventory : MonoBehaviour
{
    public string PlayerInventory;
    public List<EffectType> playerHave = new List<EffectType>();
    public List<Image> playerHaveSprite = new List<Image>();
    private void Start()
    {
        ChangeInventory();
    }
    public void ChangeInventory()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject gameObjectToChange = GameObject.Find("Slot" + PlayerInventory + i);
            if (playerHave.Count == -1) 
            {
                Debug.Log(i);
                Image spriteThisObject = gameObjectToChange.GetComponent<Image>();
                spriteThisObject.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory"); 
                return;
            }
            
            if (i < playerHave.Count)
            {
                if (playerHaveSprite[i] != null)
                {
                    Image spriteThisObject = gameObjectToChange.GetComponent<Image>();
                    spriteThisObject.sprite = playerHaveSprite[i].sprite;
                }
            }
            else
            {
                Debug.Log(i);
                Image spriteThisObject = gameObjectToChange.GetComponent<Image>();
                spriteThisObject.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory"); 
            }
        }
    }
}
