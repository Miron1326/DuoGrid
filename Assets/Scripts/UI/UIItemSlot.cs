using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public ItemData itemInThisSlot;
    private Image thisSprite;
    [SerializeField] private string objectName;
    [SerializeField] private GameObject parentGameobject;
    [SerializeField] private List<UIItemSlot> items = new List<UIItemSlot>();
    private void Start()
    {
        objectName = transform.name;
        char ninthChar = objectName[8];
        if(ninthChar == '1')
        {
            parentGameobject = GameObject.Find("ItemsInventory1");
        }
        else
        {
            parentGameobject = GameObject.Find("ItemsInventory2");
        }

        List<Transform> transforms = new List<Transform>();
        foreach(Transform transform in parentGameobject.transform)
        {
            transforms.Add(transform);
        }
        foreach(Transform transform in transforms)
        {
            if (transform.TryGetComponent<UIItemSlot>(out UIItemSlot item))
            {
                items.Add(item);
            }
        }

        thisSprite = GetComponent<Image>();
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        if(itemInThisSlot == null)
        {
            thisSprite.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory");
            return;
        }
        thisSprite.sprite = itemInThisSlot.icon;
        CheckToThisItemData();
    }

    private void CheckToThisItemData()
    {
        if (itemInThisSlot == null)
        {
            return;
        }
        foreach(UIItemSlot item in items)
        {
            if(item != this)
            {
                if (item.thisSprite == thisSprite)
                {
                    item.thisSprite.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory");
                }
                if(item.itemInThisSlot == itemInThisSlot)
                {
                    item.itemInThisSlot = null;
                    UpdateVisual();
                }
            }
        }
    }
}
