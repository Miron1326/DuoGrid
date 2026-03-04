using UnityEngine;
using UnityEngine.UI;

public class CellEditor : MonoBehaviour
{
    public bool ThisPlayerStunned;
    public bool Player1Blocked;
    public bool Player2Blocked;
    private bool Pl1;
    private GameObject AllButtonShop;
    private Vector2 StartPosition = new Vector2(0, 200);
    private Vector2 NewPosition = new Vector2(0, -100);
    public bool ShowPanel = false;
    private bool isOpen = false;
    private int Push;
    private void Start()
    {
        AllButtonShop = GameObject.Find("Panel_CellEditor");
        GameObject Panel = GameObject.Find("Panel_CellEditor");
        RectTransform rectTransform = Panel.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, StartPosition, 200);
    }
    void Update()
    {
        if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
        {


            if (Player1Blocked)
            {
                Button[] buttons = AllButtonShop.GetComponentsInChildren<Button>();
                foreach(Button button in buttons)
                {
                    Pl1 = true;
                    button.interactable = false;
                }
            }
        }
        else
        {
            Button[] buttons = AllButtonShop.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                Pl1 = false;
                button.interactable = true;
            }
        }

        if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
        {

            if (Player2Blocked)
            {
                Button[] buttons = AllButtonShop.GetComponentsInChildren<Button>();
                foreach (Button button in buttons)
                {
                    button.interactable = false;
                }
            }
        }
        else
        {
            Button[] buttons = AllButtonShop.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                if (Pl1) return;
                button.interactable = true;
            }
        }



        if (Input.GetKeyDown(KeyCode.C) || Input.GetMouseButtonDown(1))
        {
            if (ThisPlayerStunned) return;
            Cursor.visible = true;
            Push++;
            if(Push > 1)
            {
                MovementOnTheMouseManager movementManager = GetComponent<MovementOnTheMouseManager>();
                movementManager.playerCanMoveByMouse = !movementManager.playerCanMoveByMouse;
            }
            
            if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
            {
                Button FinishCellButton = null;
                for (int indexer = 0; indexer < 4; indexer++)
                {
                    GameObject invetoryGameObject = GameObject.Find("StandartCellInInventory" + indexer);
                    if(invetoryGameObject.GetComponent<CellInShop>().CurrentCellType == EffectType.Finish)
                    {
                        FinishCellButton = invetoryGameObject.GetComponent<Button>();
                    }
                }

                if(FinishCellButton != null)
                {
                    FinishCellButton.interactable = true;

                    if (!GameManager.Instance.Player1CanPlaceFinish)
                    {
                        FinishCellButton.interactable = false;
                    }
                }


                GameManager.Instance.Player1Select = true;


            }
            if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
            {
                Button FinishCellButton = null;
                for (int indexer = 0; indexer < 4; indexer++)
                {
                    GameObject invetoryGameObject = GameObject.Find("StandartCellInInventory" + indexer);
                    if (invetoryGameObject.GetComponent<CellInShop>().CurrentCellType == EffectType.Finish)
                    {
                        FinishCellButton = invetoryGameObject.GetComponent<Button>();
                    }
                }

                if (FinishCellButton != null)
                {
                    FinishCellButton.interactable = true;

                    if (!GameManager.Instance.Player2CanPlaceFinish)
                    {
                        FinishCellButton.interactable = false;
                    }
                }
            }
            
            
            if (!isOpen)
            {
                isOpen = true;
            }
            ShowUIPanel();

        }
        
    } 
    public void ShowUIPanel()
    {
        
        GameObject Panel = GameObject.Find("Panel_CellEditor");
        RectTransform rectTransform = Panel.GetComponent<RectTransform>();
        ShowPanel = !ShowPanel;
        isOpen = true;
        if (!ShowPanel)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, NewPosition, 200);
        }
        else
        {
            GameManager.Instance.Player1Select = false;
            GameManager.Instance.Player2Select = false;
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, StartPosition, 200);
        }
        if (GetComponent<Tutorial>() != null)
        {
            Tutorial tutorialScript = GetComponent<Tutorial>();
            tutorialScript.CreateOpen();
        }
        
    }

   
}
