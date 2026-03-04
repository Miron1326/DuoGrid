using UnityEngine;
using UnityEngine.UI;

public class CellAddToInventory : MonoBehaviour
{
    public string PlayerAdd;
    public Variate currentVariate;
    public EffectType currentCellType;
    public string description;

    private Text textDescription;
    private Image currentSprite;
    private GameObject PanelYourInventory;
    private GameObject ContentYourInventory;
    private CellInventory inventory;
    void Start()
    {
        currentSprite = GetComponent<Image>();

        switch(currentCellType)
        {
            case EffectType.Damage:
                description = "Клетка наносящая урон наступившему";
                break;
            case EffectType.Antidote:
                description = "Клетка убирающая эффект яда у наступившего";
                break;
            case EffectType.Finish:
                description = "Клетка получения очков финиша. обязателен для взятия двумя игроками. может быть один на поле";
                break;
            case EffectType.Wall:
                description = "Клетка стены на которую нельзя наступить";
                break;
            case EffectType.Capsule:
                description = "Клетка капсулы, начинающая мутацию с некоторыми типами клеток или врагов";
                break;
            case EffectType.MushroomMines:
                description = "Клетка волшебных грибов, наносящие два урона наступившему и дающие стан этому игроку.";
                break;
        }
    }

    public void AddCellIntoInventory()
    {
        if (PlayerAdd == "Player1")
        {
            PanelYourInventory = GameObject.Find("InventoryPlayer1");
            textDescription = GameObject.Find("DescriptionLastCellPlayer1").GetComponent<Text>();
        }
        if (PlayerAdd == "Player2")
        {
            PanelYourInventory = GameObject.Find("InventoryPlayer2");
            textDescription = GameObject.Find("DescriptionLastCellPlayer2").GetComponent<Text>();
        }

        if(currentVariate == Variate.Cell)
        {
            ContentYourInventory = PanelYourInventory.transform.Find("CellContainer").gameObject;
            inventory = ContentYourInventory.GetComponent<CellInventory>();

            if (!inventory.playerHave.Contains(currentCellType))
            {
                if (inventory.playerHave.Count >= 5) return;                  //максимум
                inventory.playerHave.Add(currentCellType);
                inventory.playerHaveSprite.Add(currentSprite);
                inventory.ChangeInventory();
            }
            else
            {
                inventory.playerHave.Remove(currentCellType);
                inventory.playerHaveSprite.Remove(currentSprite);
                inventory.ChangeInventory();
            }

        }

        textDescription.text = description;



        
    }

}
