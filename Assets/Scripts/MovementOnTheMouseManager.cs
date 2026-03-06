using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOnTheMouseManager : MonoBehaviour
{
    public float edgeTreshold = 150f;
    private bool Player1HoldCan;
    private bool Player2HoldCan;
    public bool newMove;
    public bool playerCanMoveByMouse = true; //можно ли ставить игрока
    public bool playerCanMoveOnMouse = true;
    public bool Player1Move;
    public bool Player2Move;
    private GameObject player1;
    private GameObject player2;
    public GameObject PrephabToMove;
    public float speed = 5;
    private Vector3 originalPositionPlayer1;
    private Vector3 originalPositionPlayer2;
    public bool CanPutHere;
    public bool allSelected = false;
    public HashSet<GameObject> detectedObjects = new HashSet<GameObject>(); //список без повторяемых данных
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        originalPositionPlayer1 = player1.transform.position;
        originalPositionPlayer2 = player2.transform.position;
    }

    void Update()
    {
        if (!allSelected) return;
        if(PrephabToMove != null)
        {
            
            if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer1)
            {
                
            }
            else
            {
                
            }

        }
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                

                //прожатия
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                Collider2D[] allColidersTouch = Physics2D.OverlapPointAll(touchPos);
                HashSet<GameObject> newDetectedObjectsTouch = new HashSet<GameObject>();//новые обнаруженные объекты
                foreach (Collider2D colider in allColidersTouch)
                {
                    GameObject gameObj = colider.gameObject;
                    if (gameObj == gameObject) return;
                    CellType cellType = gameObj.GetComponent<CellType>();
                    if (cellType != null)
                    {
                        newDetectedObjectsTouch.Add(gameObject);
                        if (!detectedObjects.Contains(gameObj)) //еще не обнаружен
                        {
                            if (GameManager.Instance.TypeCantStandOn.Contains(cellType.currentType)) //проверка на можно ли поставить на этом объекте
                            {
                                CanPutHere = false;
                            }
                            else
                            {
                                StartCoroutine(OneSecondTimerToCanMove());
                            }
                        }
                    }
                    else
                    {
                        CanPutHere = true;
                    }

                }
                detectedObjects = newDetectedObjectsTouch; //обновляем список

                if (CanPutHere)
                {
                    if (Player1Move)
                    {
                        float distancePlayer1 = Vector2.Distance(originalPositionPlayer1, touchPos);
                        if (distancePlayer1 < 3.2)
                        {
                            touchPos.z = 0;
                            PrephabToMove.transform.position = touchPos;
                            if (Input.GetMouseButtonDown(0))
                            {
                                player1.transform.position = touchPos;
                                PlayerController playerController = GameObject.Find("GameManager").GetComponent<PlayerController>();
                                playerController.SnapToGrid("Player1");
                                if (GameManager.Instance.player1Tacts == 0)
                                {
                                    playerController.ChangeSchem();
                                    Destroy(PrephabToMove);
                                    newMove = true;
                                }
                                else
                                {
                                    GameManager.Instance.TurnPlayer2++;
                                    originalPositionPlayer1 = player1.transform.position;
                                    GameManager.Instance.player1Tacts--;
                                    playerController.ShowTextTurn("Player1");
                                    GameManager.Instance.PlayerUIEn("");
                                    GameManager.Instance.SwitchTurn();
                                    Debug.Log(1);
                                }

                            }
                        }
                    }
                    else
                    {
                        float distancePlayer2 = Vector2.Distance(originalPositionPlayer2, touchPos);
                        if (distancePlayer2 < 3.2)
                        {
                            touchPos.z = 0;
                            PrephabToMove.transform.position = touchPos;
                            if (Input.GetMouseButtonDown(0))
                            {
                                player2.transform.position = touchPos;
                                PlayerController playerController = GameObject.Find("GameManager").GetComponent<PlayerController>();
                                playerController.SnapToGrid("Player2");
                                if (GameManager.Instance.player2Tacts == 0)
                                {
                                    playerController.ChangeSchem();
                                    Destroy(PrephabToMove);
                                    newMove = true;
                                }
                                else
                                {
                                    GameManager.Instance.TurnPlayer1++;
                                    originalPositionPlayer2 = player2.transform.position;
                                    GameManager.Instance.player2Tacts--;
                                    playerController.ShowTextTurn("Player2");
                                    GameManager.Instance.PlayerUIEn("");
                                    Player2Move = true;
                                    GameManager.Instance.SwitchTurn();
                                    Debug.Log(2);
                                }

                            }
                        }

                    }

                }
            }


            return;
        }
#endif


        Vector3 mousePosition = GetMouseWorldPosition();
        Collider2D[] allColiders = Physics2D.OverlapPointAll(mousePosition);
        HashSet<GameObject> newDetectedObjects = new HashSet<GameObject>();//новые обнаруженные объекты
        foreach (Collider2D colider in allColiders)
        {
            GameObject gameObj = colider.gameObject;
            if (gameObj == gameObject) return;
            CellType cellType = gameObj.GetComponent<CellType>();
            if(cellType != null)
            {
                newDetectedObjects.Add(gameObject);
                if (!detectedObjects.Contains(gameObj)) //еще не обнаружен
                {
                    if (GameManager.Instance.TypeCantStandOn.Contains(cellType.currentType)) //проверка на можно ли поставить на этом объекте
                    {
                        CanPutHere = false;
                    }
                    else
                    {
                        StartCoroutine(OneSecondTimerToCanMove());
                    }
                }
            }
            else
            {
                CanPutHere = true;
            }

        }
        detectedObjects = newDetectedObjects; //обновляем список


            if (newMove)
            {
                originalPositionPlayer1 = SetPositionOfPlayer("Player1");
                originalPositionPlayer2 = SetPositionOfPlayer("Player2");
                Destroy(PrephabToMove);
                PrephabToMove = null;
                newMove = false;
            }

            if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
            {
                Player1Move = true;
                Player2Move = false;
            }
            else
            {
                Player1Move = false;
                Player2Move = true;
            }
        if (newMove) return;
            if (PrephabToMove == null)
            {
                if (Player1Move)
                {
                    PrephabToMove = Instantiate(player1, new Vector3(0, 8.5f, 0), Quaternion.identity);
                    Rigidbody2D rb2d = PrephabToMove.GetComponent<Rigidbody2D>();
                    BoxCollider2D boxCollider2D = PrephabToMove.GetComponent<BoxCollider2D>();
                    boxCollider2D.isTrigger = true;
                    Destroy(rb2d);
                }
                if (Player2Move)
                {
                    PrephabToMove = Instantiate(player2, new Vector3(0, 8.5f, 0), Quaternion.identity);
                    Rigidbody2D rb2d = PrephabToMove.GetComponent<Rigidbody2D>();
                    BoxCollider2D boxCollider2D = PrephabToMove.GetComponent<BoxCollider2D>();
                    boxCollider2D.isTrigger = true;
                    Destroy(rb2d);
                }
            }
        if (CanPutHere)
        {


            if (playerCanMoveOnMouse)
            {
                if (playerCanMoveByMouse)
                {
                    if (Player1Move)
                    {
                        float distancePlayer1 = Vector2.Distance(originalPositionPlayer1, mousePosition);
                        if (distancePlayer1 < 3.2)
                        {
                            Cursor.visible = false;
                            PrephabToMove.transform.position = mousePosition;
                            if (Input.GetMouseButtonDown(0))
                            {
                                player1.transform.position = mousePosition;
                                PlayerController playerController = GameObject.Find("GameManager").GetComponent<PlayerController>();
                                playerController.SnapToGrid("Player1");
                                if (GameManager.Instance.player1Tacts == 0)
                                {
                                    playerController.ChangeSchem();
                                    Destroy(PrephabToMove);
                                    newMove = true;
                                    GameManager.Instance.SwitchTurn();
                                }
                                else
                                {
                                    originalPositionPlayer1 = player1.transform.position;
                                    GameManager.Instance.player1Tacts--;
                                    GameManager.Instance.PlayerUIEn("Player1");
                                }

                            }
                        }
                        else
                        {
                            Cursor.visible = true;
                        }
                    }
                    if (Player2Move)
                    {
                        float distancePlayer2 = Vector2.Distance(originalPositionPlayer2, mousePosition);
                        if (distancePlayer2 < 3.2)
                        {
                            Cursor.visible = false;
                            PrephabToMove.transform.position = mousePosition;
                            if (Input.GetMouseButtonDown(0))
                            {
                                player2.transform.position = mousePosition;
                                PlayerController playerController = GameObject.Find("GameManager").GetComponent<PlayerController>();
                                playerController.SnapToGrid("Player2");
                                if (GameManager.Instance.player2Tacts == 0)
                                {
                                    playerController.ChangeSchem();
                                    Destroy(PrephabToMove);
                                    newMove = true;
                                    GameManager.Instance.SwitchTurn();
                                }
                                else
                                {
                                    originalPositionPlayer2 = player2.transform.position;
                                    GameManager.Instance.player2Tacts--;
                                    GameManager.Instance.PlayerUIEn("Player2");
                                }
                                playerController.ItemOpen = false;
                            }
                        }
                        else
                        {
                            Cursor.visible = true;
                        }
                    }
                }
                else
                {
                    PrephabToMove.transform.position = new Vector3(0, 8.5f, 0);
                }
            }
        }  


        }
    
    


    public Vector3 GetPositionOfPlayer(string Player)
    {
        if (Player == player1.name)
        {
            return originalPositionPlayer1;
        }
        else if(Player == player2.name) 
        {
            return originalPositionPlayer2;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }
    }
    public Vector3 SetPositionOfPlayer(string Player)
    {
        if(Player == player1.name)
        {
            return GameObject.Find("Player1").transform.position;
        }
        else if( Player == player2.name)
        {
            return GameObject.Find("Player2").transform.position;
        }
        else
        {
            return new Vector3(0, 0, 0);
        }

    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        return worldPos;
    }

    public void NewPrephabToMovement()
    {
        if (!allSelected) return;
        Destroy(PrephabToMove);

        SpriteRenderer spriteRenderer = PrephabToMove.GetComponent<SpriteRenderer>();

            if (Player1Move)
            {
                PrephabToMove = Instantiate(player1, new Vector3(0, 8.5f, 0), Quaternion.identity);
                Rigidbody2D rb2d = PrephabToMove.GetComponent<Rigidbody2D>();
                BoxCollider2D boxCollider2D = PrephabToMove.GetComponent<BoxCollider2D>();
                spriteRenderer.sprite = GameObject.Find("Player1").GetComponent<SpriteRenderer>().sprite;
                boxCollider2D.isTrigger = true;
                Destroy(rb2d);
            }
            if (Player2Move)
            {
                PrephabToMove = Instantiate(player2, new Vector3(0, 8.5f, 0), Quaternion.identity);
                Rigidbody2D rb2d = PrephabToMove.GetComponent<Rigidbody2D>();
                BoxCollider2D boxCollider2D = PrephabToMove.GetComponent<BoxCollider2D>();
                spriteRenderer.sprite = GameObject.Find("Player2").GetComponent<SpriteRenderer>().sprite;
                boxCollider2D.isTrigger = true;
                Destroy(rb2d);
            }
        
    }
    public void StartCheckCoruetine()
    {
        StartCoroutine(CheckCountDown());
    }
    public IEnumerator CheckCountDown()
    {
        playerCanMoveOnMouse = false;
        yield return new WaitForSeconds(1);
        playerCanMoveOnMouse = true;
    }
    private IEnumerator OneSecondTimerToCanMove()
    {
        yield return new WaitForSeconds(1);
        CanPutHere = true;
    }
    public void RestartMovementOnTheMouse()
    {
        if (PrephabToMove != null)
        {
            Destroy(PrephabToMove);
            originalPositionPlayer1 = new Vector3(-17.3f, 0, 0);
            originalPositionPlayer2 = new Vector3(17.3f, 0, 0);
        }
    }
}
