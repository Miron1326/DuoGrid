using DG.Tweening;
using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellInShop : MonoBehaviour
{
    public StilisticType CurrentStilisticType;
    public string Name;
    public EffectType CurrentCellType;
    public bool allowsInBothSt;
    private float speedItem = 50f;
    private bool selected;
    private Vector2 StartPos;
    private GameObject CursorInst;
    private bool animatingStarted = false;
    private bool animatingFinished;
    private bool TurnPlayer1;
    private bool TurnPlayer2;
    private Color ColorForE = Color.red;
    private Color normalColor = Color.white;
    private Color AntidoteColor = Color.springGreen;
    private List<EffectType> bannedTypesToReplace = new List<EffectType>();
    private List<EffectType> allowedTypesInYourSt = new List<EffectType>();

    private void Start()
    {
        CurrentStilisticType = GameObject.Find("StilisticManager").GetComponent<StilisticManager>().Currenttype;
        ChangeVisual();
        
    }
    public void ChangeCellType(EffectType type)
    {
        CurrentCellType = type;
    }
    public void ChangeVisual()
    {
        Image image = GetComponent<Image>();
        switch (CurrentStilisticType)
        {
            case StilisticType.Standart:
                switch (CurrentCellType)
                {
                    case EffectType.None:

                        image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCell1");
                        break;

                    case EffectType.Finish: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/finish"); break;
                    case EffectType.Damage: image.sprite = Resources.Load<Sprite>("sprites/CellsType/needlemoss"); image.color = normalColor; break;
                    case EffectType.Antidote: image.color = AntidoteColor; break;
                    case EffectType.Wall: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/vine"); break;
                    case EffectType.MushroomMines: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/mushroomMines"); break;
                    case EffectType.Capsule: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/CapsuleCell"); break;

                }
                break;


            case StilisticType.Desert:
                switch (CurrentCellType)
                {
                    case EffectType.None:

                        image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/NoneCellDesert");

                        break;

                    case EffectType.Finish: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/FinishDesert"); break;
                    case EffectType.Damage: image.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/DamageCellDesert"); image.color = normalColor; break;
                    case EffectType.Antidote: image.color = AntidoteColor; break;
                    case EffectType.Wall: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/vine"); break;
                    case EffectType.MushroomMines: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/mushroomMines"); break;
                    case EffectType.Capsule: image.color = normalColor; image.sprite = Resources.Load<Sprite>("sprites/CellsType/CapsuleCell"); break;

                }
                break;


        }
    }

    public void SelectCell()
    {
        

            Cursor.visible = false;
        if (GameManager.Instance.player1IsOpenItems)
        {
            GameManager.Instance.Player1ItemsCloseCanvas();
        }
        if (GameManager.Instance.player2IsOpenItems)
        {
            GameManager.Instance.Player2ItemsCloseCanvas();
        }

        selected = true;
        CursorInst = Instantiate(gameObject,transform.position, Quaternion.identity);
        CursorInst.name = "Cursor" + gameObject.name;
        SpriteRenderer spriteRenderer = CursorInst.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1;
        Color color = spriteRenderer.color;
        color.a = 0.5f;
        spriteRenderer.color = color;
        allowedTypesInYourSt = GameManager.Instance.allowedTypesInYourSt;
        bannedTypesToReplace = GameManager.Instance.bannedTypesToReplace;
        TurnPlayer1 = GameManager.Instance.Player1Select;
        allowsInBothSt = false;
        if (TurnPlayer1)
        {
            GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject cell in player1Cells)
            {
                Debug.Log(player1Cells.Length);

                if (GameManager.Instance.allowedTypesInBothSt.Contains(CurrentCellType))
                {
                    allowsInBothSt = true;
                }

                CellType cellType = cell.GetComponent<CellType>();
                if (allowedTypesInYourSt.Contains(CurrentCellType) || allowsInBothSt)
                {
                    Debug.Log(allowedTypesInYourSt.Contains(CurrentCellType) || allowsInBothSt);

                    cellType.CanPlaceInYourSt = true;
                    cellType.DisableAllColor = true;
                    cellType.UpdateVisual();

                }
                else
                {
                    SpriteRenderer spriteRenderer2 = cell.GetComponent<SpriteRenderer>();
                    cellType.DisableAllColor = false;
                    spriteRenderer2.color = ColorForE;
                }

            }

            GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject cell in player2Cells)
            {
                Debug.Log(player2Cells.Length);

                if (GameManager.Instance.allowedTypesInBothSt.Contains(CurrentCellType))
                {
                    allowsInBothSt = true;
                }

                CellType cellType = cell.GetComponent<CellType>();
                if (GameManager.Instance.allowedTypesInOtherSt.Contains(CurrentCellType) || allowsInBothSt)
                {
                    cellType.CanPlaceInYourSt = true;
                    cellType.DisableAllColor = true;
                    cellType.UpdateVisual();

                }
                else
                {
                    SpriteRenderer spriteRenderer2 = cell.GetComponent<SpriteRenderer>();
                    spriteRenderer2.color = ColorForE;
                }

            }
            TurnPlayer1 = false;
        }
        TurnPlayer2 = GameManager.Instance.Player2Select;
        allowsInBothSt = false;
        if (TurnPlayer2)
        {
            GameManager.Instance.Player2ItemsOpenCanvas();
            GameObject[] player4Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject cell in player4Cells)
            {

                CellType cellType = cell.GetComponent<CellType>();
                if (GameManager.Instance.allowedTypesInOtherSt.Contains(CurrentCellType) || allowsInBothSt)
                {
                    cellType.CanPlaceInYourSt = true;
                    cellType.DisableAllColor = true;
                    cellType.UpdateVisual();

                }
                else
                {

                }

            }

            
            GameObject[] player3Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject cell in player3Cells)
            { 
                if (GameManager.Instance.allowedTypesInBothSt.Contains(CurrentCellType))
                {
                    allowsInBothSt = true;
                }

                CellType cellType = cell.GetComponent<CellType>();
                if (GameManager.Instance.allowedTypesInOtherSt.Contains(CurrentCellType) )
                {
                    
                    SpriteRenderer spriteRenderer2 = cell.GetComponent<SpriteRenderer>();
                    spriteRenderer2.color = ColorForE;

                }
                else if(allowsInBothSt)
                {
                    Debug.Log("sdasdasdasda");
                    cellType.CanPlaceInYourSt = true;
                    cellType.DisableAllColor = true;
                    cellType.UpdateVisual();
                }

            }

            
            TurnPlayer2 = false;
        }
    }
    private void Update()
    {
        Button thisButton = GetComponent<Button>();

        if (selected)
        {
            if (CurrentCellType == EffectType.Finish)
            {
                if (!GameManager.Instance.Player1CanPlaceFinish || !GameManager.Instance.Player2CanPlaceFinish)
                {
                    CancelEditing();
                }
            }
            if (GameManager.Instance.player1IsOpenItems)
            {
                GameManager.Instance.Player1ItemsCloseCanvas();
            }
            if (GameManager.Instance.player2IsOpenItems)
            {
                GameManager.Instance.Player2ItemsCloseCanvas();
            }
            if (CurrentCellType == EffectType.Capsule) //добавление модификатора
            {
                GameManager.Instance.bannedTypesToReplace.Remove(EffectType.Tentacle);
            }
            else
            {
                if (!GameManager.Instance.bannedTypesToReplace.Contains(EffectType.Tentacle))
                {
                    GameManager.Instance.bannedTypesToReplace.Add(EffectType.Tentacle);
                }
                
            }
            if (CurrentCellType == EffectType.Finish) //нельзя ставить на грибы
            {
                
                if (!GameManager.Instance.bannedTypesToReplace.Contains(EffectType.NoneWithNoneEffectedMushrooms))
                {
                    GameManager.Instance.bannedTypesToReplace.Add(EffectType.NoneWithNoneEffectedMushrooms);
                }
            }
            else
            {
                if (!GameManager.Instance.bannedTypesToReplace.Contains(EffectType.NoneWithNoneEffectedMushrooms))
                {
                    GameManager.Instance.bannedTypesToReplace.Remove(EffectType.NoneWithNoneEffectedMushrooms);
                }

            }
            MovementOnTheMouseManager movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
            movementOnTheMouseManager.playerCanMoveByMouse = false;


            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TurnPlayer1 = GameManager.Instance.Player1Select;
            TurnPlayer2 = GameManager.Instance.Player2Select;
            mousePos.z = 0;
            CursorInst.transform.position = mousePos;



            if (Input.GetMouseButtonDown(0))
            {

                if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
                {
                    TurnPlayer1 = true;
                    TurnPlayer2 = false;
                }
                else if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
                {
                    TurnPlayer2 = true;
                    TurnPlayer1 = false;
                }


                switch (CurrentCellType)
                {
                    case EffectType.Poison:
                        if (GameManager.Instance.Poison <= 0)
                        {
                            CancelEditing();
                            return;
                        }
                        animatingStarted = true;
                        GameObject ThrowObject = Instantiate(GameObject.Find("PrephabToThrow"));

                        
                        if (TurnPlayer1)
                        {
                            GameManager.Instance.PlayerUsePoison("Player1");
                            ThrowObject.transform.position = GameObject.Find("Player1").transform.position;
                        }
                        Debug.Log(TurnPlayer2);
                        if (TurnPlayer2)
                        {
                            GameManager.Instance.PlayerUsePoison("Player2");
                            ThrowObject.transform.position = GameObject.Find("Player2").transform.position;
                        }

                        ThrowItem(ThrowObject, mousePos);
                        CancelEditing();
                        break;

                    case EffectType.GuavaBoom:


                        if (TurnPlayer1)
                        {
                            if (GameManager.Instance.ItemStatsPlayer1["GuavaBoom"] <= 0)
                            {
                                CancelEditing();
                                return;
                            }
                        }
                        if (TurnPlayer2)
                        {
                            if (GameManager.Instance.ItemStatsPlayer2["GuavaBoom"] <= 0)
                            {
                                CancelEditing();
                                return;
                            }
                        }
                        
                            animatingStarted = true;
                            GameObject ThrowObjectGuava = Instantiate(GameObject.Find("PrephabToThrow"));
                           
                            if (TurnPlayer1)
                            {
                                GameManager.Instance.PlayerUseItem("Player1", "GuavaBoom");
                                ThrowObjectGuava.transform.position = GameObject.Find("Player1").transform.position;
                            }
                            Debug.Log(TurnPlayer2);
                            if (TurnPlayer2)
                            {
                                GameManager.Instance.PlayerUseItem("Player2", "GuavaBoom");
                                ThrowObjectGuava.transform.position = GameObject.Find("Player2").transform.position;
                            }

                            ThrowItem(ThrowObjectGuava, mousePos);
                            CancelEditing();


                        break;

                    case EffectType.Medkit:


                        if (TurnPlayer1)
                        {
                            
                            if (GameManager.Instance.ItemStatsPlayer1["Medkit"] <= 0)
                            {
                                CancelEditing();
                                return;
                            }
                            GameManager.Instance.ItemStatsPlayer1["Medkit"]--;
                        }
                        if (TurnPlayer2)
                        {
                            if (GameManager.Instance.ItemStatsPlayer2["Medkit"] <= 0)
                            {
                                CancelEditing();
                                return;
                            }
                            GameManager.Instance.ItemStatsPlayer2["Medkit"]--;
                        }

                        ChangeCell(mousePos);
                        GameManager.Instance.SwitchTurn();
                        break;

                    default:
                        ChangeCell(mousePos);
                        GameManager.Instance.SwitchTurn();
                        break;
                }
                
            }



            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelEditing();
            }
        }
    }

    private void ThrowItem(GameObject ThrowObject, Vector3 mousePos)
    {
        GameManager.Instance.SwitchTurn();
        Rigidbody2D rb = ThrowObject.GetComponent<Rigidbody2D>();
        Vector2 startPosition = ThrowObject.transform.position;
        Vector2 targetPosition = mousePos;

        // Направление и сила
        Vector2 direction = (targetPosition - startPosition).normalized;
        float distance = Vector2.Distance(startPosition, targetPosition);
        float force = 95f;

        // Применение
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        // Вращение
        rb.AddTorque(240f);

        StartCoroutine(TrackPhysicsThrow2D(ThrowObject, targetPosition));
    }

    private IEnumerator TrackPhysicsThrow2D(GameObject throwObject, Vector3 targetPosition)
    {
        float distance = Vector2.Distance(throwObject.transform.position, targetPosition);
        float duration = distance / speedItem;
        throwObject.transform.DOMove(targetPosition, duration).OnComplete(() => 
        { // Меняем ячейку
            ChangeCell(targetPosition);

            // Можно уничтожить объект
            Destroy(throwObject);
        });
        yield return null;
        // Когда достигли цели или время вышло
        
            
        
    }

    private void ChangeCell(Vector3 MousePos)
    {
       
        MovementOnTheMouseManager movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
        if (selected)
        {
            movementOnTheMouseManager.StartCheckCoruetine();
        }
        
        Collider2D hit = Physics2D.OverlapPoint(MousePos);
        if (hit != null)
        {

            CellType cellUnderCursor = hit.GetComponent<CellType>();

            SpriteRenderer spriteRenderer = hit.GetComponent<SpriteRenderer>();
            if(spriteRenderer.color == ColorForE)
            {
                CancelEditing();
                return;
            }

                if (bannedTypesToReplace.Contains(cellUnderCursor.currentType))
                {
                CancelEditing();
                return;
            } 
                else if (cellUnderCursor.CanPlaceInYourSt)
                {

                   if (allowedTypesInYourSt.Contains(CurrentCellType))
                   {
                      
                   }else if (GameManager.Instance.allowedTypesInOtherSt.Contains(CurrentCellType))
                {

                }
                   else
                   {
                      CancelEditing();
                      return;
                   }
                     
                }
            PlayerController playerController = GameObject.Find("GameManager").GetComponent<PlayerController>();
            playerController.ItemOpen = false;
            MovementOnTheMouseManager movementManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
            movementManager.playerCanMoveByMouse = !movementManager.playerCanMoveByMouse;
            movementManager.NewPrephabToMovement();
            movementManager.newMove = true;
            if (CurrentCellType == EffectType.Capsule)
            {
                cellUnderCursor.AddModificator(Modificator.InCapsule);
            }
            if (CurrentCellType != EffectType.Capsule)
            {
                cellUnderCursor.ChangeType(CurrentCellType);
            }

            movementManager.Player1Move = !movementManager.Player1Move;
            movementManager.Player2Move = !movementManager.Player2Move;

            
            GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject cell in player1Cells)
            {
                CellType cellType = cell.GetComponent<CellType>();
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }
            GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject cell in player2Cells)
            {
                CellType cellType = cell.GetComponent<CellType>();
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }


            GameManager.Instance.ChangeAfterEditing();
            if (CurrentCellType == EffectType.Finish)
            {
                if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
                {
                    GameManager.Instance.Player1CanPlaceFinish = false;
                    GameManager.Instance.Player2CanPlaceFinish = false;
                }
                else if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
                {
                    GameManager.Instance.Player2CanPlaceFinish = false;
                    GameManager.Instance.Player1CanPlaceFinish = false;
                }
            }
            if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
            {
                GameManager.Instance.TurnPlayer1++;
                cellUnderCursor.PlayerChangeModifire("Player2");

            } else if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
            {
                GameManager.Instance.TurnPlayer2++;
                cellUnderCursor.PlayerChangeModifire("Player1");
            }
            CancelEditing();


        }
    }
    
    public void CancelEditing()
    {
        allowsInBothSt = false;
        selected = false;
        Cursor.visible = true;
        Destroy(CursorInst);
        if (TurnPlayer1)
        {
            GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject cell in player1Cells)
            {
                CellType cellType = cell.GetComponent<CellType>();
                cellType.CanPlaceInYourSt = false;
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }
            GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject cell in player2Cells)
            {
                CellType cellType = cell.GetComponent<CellType>();
                cellType.CanPlaceInYourSt = false;
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }
        }

        if (TurnPlayer2)
        {
            GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
            foreach (GameObject cell in player2Cells)
            {
                SpriteRenderer spriteRenderer3 = cell.GetComponent<SpriteRenderer>();
                spriteRenderer3.color = ColorForE;
                CellType cellType = cell.GetComponent<CellType>();
                cellType.CanPlaceInYourSt = false;
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }
            GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
            foreach (GameObject cell in player1Cells)
            {
                SpriteRenderer spriteRenderer2 = cell.GetComponent<SpriteRenderer>();
                spriteRenderer2.color = ColorForE;
                CellType cellType = cell.GetComponent<CellType>();
                cellType.CanPlaceInYourSt = false;
                cellType.DisableAllColor = false;
                cellType.UpdateVisual();
            }
        }
    }

   

}
