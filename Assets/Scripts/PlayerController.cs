using System.Collections;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class PlayerController : MonoBehaviour
{
    public bool PlayersCanSavesTacks;
    public bool ItemOpen = false;
    public bool PlayerCanSnap;
    public float player1Speed = 5f;
    public float player2Speed = 5f;
    public float cellSize = 1.0f;
    private float gridCellSpacing = 2.1f;
    private DisastersManager disastersManager;
    public bool anotherPlayerStunned;

    // Начальные позиции
    private Vector3 _startPosition1;
    private Vector3 _startPosition2;

    // Флаги для отслеживания движения
    private bool player1WasMoving = false;
    private bool player2WasMoving = false;

    // Флаг чтобы SnapToGrid вызывался только один раз после остановки
    private bool player1NeedsSnap = false;
    private bool player2NeedsSnap = false;

    // Чей ход
    public bool P1Move = true;
    public bool P2Move = false;

    private void Start()
    {
        disastersManager = GetComponent<DisastersManager>();
        anotherPlayerStunned = false;
        GameManager.Instance.Player1ItemsCloseCanvas();
        GameManager.Instance.Player2ItemsCloseCanvas();
        PlayersCanSavesTacks = true;
        PlayerCanSnap = true;
        GameObject playerOne = GameObject.Find("Player1");
        GameObject playerTwo = GameObject.Find("Player2");
        playerOne.transform.position = new Vector3(-17.3f, 0, 0);
        playerTwo.transform.position = new Vector3(17.3f, 0, 0);
        _startPosition1 = playerOne.transform.position;
        _startPosition2 = playerTwo.transform.position;
        GameManager.Instance.PlayerUIEn("Player1");
        if (GameObject.Find("AI"))
        {
            playerOne.transform.position = new Vector3(-17.3f, 0, 0);
            GameObject AI = GameObject.Find("AI");
            AI.transform.position = new Vector3(17.3f, 0, 0);
        }
    }

    public void RestartInitialize()
    {
        anotherPlayerStunned = false;
        PlayersCanSavesTacks = true;
        PlayerCanSnap = true;
        GameObject playerOne = GameObject.Find("Player1");
        GameObject playerTwo = GameObject.Find("Player2");
        playerOne.transform.position = new Vector3(-17.3f, 0, 0);
        playerTwo.transform.position = new Vector3(17.3f, 0, 0);
        _startPosition1 = playerOne.transform.position;
        _startPosition2 = playerTwo.transform.position;
        GameManager.Instance.PlayerUIEn("Player1");
    }

    void Update()
    {

        GameManager.Instance.CheckPlayerPoisoned();
        GameManager.Instance.previousPosition1 = _startPosition1;
        GameManager.Instance.previousPosition2 = _startPosition2;
        if (P1Move)
        {
            Player1Step();
        }

        if (P2Move)
        {
            Player2Step();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ItemOpen = !ItemOpen;
            if (ItemOpen)
            {
                if (P1Move)
                {
                    GameManager.Instance.Player1ItemsOpenCanvas();
                }
                if (P2Move)
                {
                    GameManager.Instance.Player2ItemsOpenCanvas();
                }
            }
            if (!ItemOpen)
            {
                if (P1Move)
                {
                    GameManager.Instance.Player1ItemsCloseCanvas();
                }
                if (P2Move)
                {
                    GameManager.Instance.Player2ItemsCloseCanvas();
                }
            }

        }

    }
    public void SaveTacts()
    {
        MovementOnTheMouseManager movementOnTheMouseManager = GetComponent<MovementOnTheMouseManager>();
        if(movementOnTheMouseManager.Player1Move)
        {
            Player1SavesTacts();
        }
        else
        {
            Player2SavesTacts();
        }
    }
    private void Player1SavesTacts()
    {
        GameManager.Instance.player1Tacts++;
        if (GameManager.Instance.player1Tacts == 4)
        {
            GameManager.Instance.player1Tacts = 3;
        }
        player1NeedsSnap = true;
        SnapToGrid("Player1");
        StartCoroutine(TimerToNextTurn());
        GameManager.Instance.PlayerUIEn("Player2");
        ShowTextTurn("Player2");
        PlayersCanSavesTacks = false;
    }
    private void Player2SavesTacts()
    {
        GameManager.Instance.player2Tacts++;
        if (GameManager.Instance.player2Tacts == 4)
        {
            GameManager.Instance.player2Tacts = 3;
        }
        player2NeedsSnap = true;
        SnapToGrid("Player2");
        StartCoroutine(TimerToNextTurn());
        GameManager.Instance.PlayerUIEn("Player1");
        ShowTextTurn("Player1");
        PlayersCanSavesTacks = false;
    }
    bool MovePlayer1()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayersCanSavesTacks)
        {
            Player1SavesTacts();
        }
        GameObject playerOne = GameObject.Find("Player1");
        if (playerOne == null) return false;

        Vector3 newPosition1 = playerOne.transform.position;
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        if (Input.GetKey(KeyCode.D)) moveX = 1;

        // Проверка границ движения
        if (newPosition1.x > _startPosition1.x + 3 || newPosition1.y > _startPosition1.y + 3 ||     //произошло движение этого игрока дальше цели
            newPosition1.x < _startPosition1.x - 3 || newPosition1.y < _startPosition1.y - 3)
        {
            
            
            PlayersCanSavesTacks = false;
            playerOne.transform.position = _startPosition1;
            return true;
        }

        // Если нет ввода
        if (moveX == 0 && moveY == 0)
        {
            return false;
        }

        PlayersCanSavesTacks = false;
        Vector3 movement = new Vector3(moveX, moveY, 0) * player1Speed * Time.deltaTime;           //произошло движение этого игрока 
        if (ItemOpen)
        {
            ItemOpen = false;
            GameManager.Instance.Player1ItemsCloseCanvas();
        }
        playerOne.transform.position += movement;
        return true;
    }

    bool MovePlayer2()
    {
        if (Input.GetKeyDown(KeyCode.Space) && PlayersCanSavesTacks)
        {
            Player2SavesTacts();
        }
        GameObject playerTwo = GameObject.Find("Player2");
        if (playerTwo == null) return false;

        Vector3 newPosition2 = playerTwo.transform.position;
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.UpArrow)) moveY = 1;
        if (Input.GetKey(KeyCode.DownArrow)) moveY = -1;
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1;
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1;

        if (newPosition2.x > _startPosition2.x + 3 || newPosition2.y > _startPosition2.y + 3 ||
            newPosition2.x < _startPosition2.x - 3 || newPosition2.y < _startPosition2.y - 3)   //произошло движение этого игрока дальше цели
        {
            PlayersCanSavesTacks = false;
            playerTwo.transform.position = _startPosition2;
            return true;
        }

        if (moveX == 0 && moveY == 0)
        {
            return false;
        }

        PlayersCanSavesTacks = false;
        Vector3 movement = new Vector3(moveX, moveY, 0) * player2Speed * Time.deltaTime;        //произошло движение этого игрока 
        if (ItemOpen)
        {
            ItemOpen = false;
            GameManager.Instance.Player2ItemsCloseCanvas();
        }
        playerTwo.transform.position += movement;
        return true;
    }

    public void SnapToGrid(string playerName)
    {
        if (!PlayerCanSnap) return;

        GameObject player = GameObject.Find(playerName);
        if (player == null) return;

        float snapX = Mathf.Round(player.transform.position.x / gridCellSpacing) * gridCellSpacing;
        float snapY = Mathf.Round(player.transform.position.y / gridCellSpacing) * gridCellSpacing;

        player.transform.position = new Vector3(snapX, snapY, player.transform.position.z);

        // Обновляем стартовую позицию
        disastersManager.MovementAppear++;
        if (playerName == "Player1")
            _startPosition1 = player.transform.position;
        else if (playerName == "Player2")
            _startPosition2 = player.transform.position;

        CheckCellEffect(player);
        if (playerName == "Player1")
        {
            GameManager.Instance.TurnPlayer1++;
        }
        if (playerName == "Player2")
        {
            GameManager.Instance.TurnPlayer2++;
        }
        Debug.Log($"{playerName} примагничен к клетке [{snapX}, {snapY}]");
        MovementOnTheMouseManager movement = GetComponent<MovementOnTheMouseManager>();
        movement.NewPrephabToMovement();
        movement.newMove = true;
        

    }

    void CheckCellEffect(GameObject player)
    {
        Collider2D[] nearbyCells = Physics2D.OverlapCircleAll(player.transform.position, 0.5f);
        foreach (Collider2D colider in nearbyCells)
        {

            CellType nearbyEffect = colider.GetComponent<CellType>();
            //if (nearbyEffect.currentType == EffectType.GuavaBoom)
           // {
            //    if (Input.GetKeyDown(KeyCode.F))
            //    {
            //        PlayerKickThisCell(player, nearbyEffect);
            //    }

           // }
            if (nearbyEffect != null)
            {
                nearbyEffect.ActivateEffect(player);
                break;
            }
        }
    }

    private void PlayerKickThisCell(GameObject player, CellType thisCell)
    {
        Vector3 direction = new Vector3(0, 0, 0);
        if(player.name == "Player1")
        {
            direction = Vector3.Normalize(GameObject.Find("Player2").transform.position - player.transform.position);
            GameObject CellGameObject = thisCell.gameObject;
            CellGameObject.transform.position += direction * 20 * Time.deltaTime;
        }
        if (player.name == "Player2")
        {

        }
    }

    public void Player1Step()
    {
        bool p1MovingNow = MovePlayer1();

        // Игрок двигался, но сейчас остановился
        if (player1WasMoving && !p1MovingNow)
        {
            // Вызываем Snap только если нужно
            if (!player1NeedsSnap)
            {
                player1NeedsSnap = true;
                SnapToGrid("Player1");
                if (GameManager.Instance.player1Tacts != 0)
                {
                    GameManager.Instance.player1Tacts--;
                    GameManager.Instance.PlayerUIEn("");
                    Player1Step();
                    ShowTextTurn("Player1");
                }
                else if(GameManager.Instance.player1Tacts == 0)
                {
                    PlayersCanSavesTacks = true;
                    ChangeSchem();
                    ShowTextTurn("Player2");
                }

                
            }
        }
        else if (p1MovingNow)
        {
            // Игрок начал двигаться - сбрасываем флаг
            player1NeedsSnap = false;
        }

        player1WasMoving = p1MovingNow;
    }

    public void Player2Step()
    {
        bool p2MovingNow = MovePlayer2();

        if (player2WasMoving && !p2MovingNow)
        {
            if (!player2NeedsSnap)
            {
                player2NeedsSnap = true;
                SnapToGrid("Player2");
                if (GameManager.Instance.player2Tacts != 0)
                {
                    GameManager.Instance.player2Tacts--;
                    GameManager.Instance.PlayerUIEn("");
                    Player2Step();
                    ShowTextTurn("Player2");
                }
                else if (GameManager.Instance.player2Tacts == 0)
                {
                    PlayersCanSavesTacks = true;
                    ChangeSchem();
                    ShowTextTurn("Player1");
                }


            }
        }
        else if (p2MovingNow)
        {
            player2NeedsSnap = false;
        }

        player2WasMoving = p2MovingNow;
    }

    public void ChangeSchem()
    {
        GameManager.Instance.SwitchTurn();
        CellEnemyType[] enemys = GameObject.FindObjectsByType<CellEnemyType>(FindObjectsSortMode.InstanceID);
        
        if (!anotherPlayerStunned)
        {
            P1Move = !P1Move;
            P2Move = !P2Move;
        }
        else
        {
            if (GameManager.Instance.Player1isStunnedFor != 0 )
            {
                GameManager.Instance.Player1isStunnedFor--;
                GameManager.Instance.TurnPlayer1++;
            }
            else if(GameManager.Instance.Player2isStunnedFor != 0)
            {
                GameManager.Instance.Player2isStunnedFor--;
                GameManager.Instance.TurnPlayer2++;
            }
        }

        if (GameManager.Instance.Player1isStunnedFor != 0 || GameManager.Instance.Player2isStunnedFor != 0)
        {
            anotherPlayerStunned = true;
        }
        else if(GameManager.Instance.Player1isStunnedFor == 0 && GameManager.Instance.Player2isStunnedFor == 0)
        {
            anotherPlayerStunned = false;
        }
        if (GameManager.Instance.Player1isStunnedFor != 0 || GameManager.Instance.Player2isStunnedFor != 0)
        {
            if (P1Move)
            {
                GameManager.Instance.PlayerUIEn("Player2");
                Debug.Log("1");
            }
            else
            {
                GameManager.Instance.PlayerUIEn("Player1");
                Debug.Log("2");
            }
            return;
        }
            if (P1Move)
            {
                GameManager.Instance.PlayerUIEn("Player1");
            }
            else
            {
                GameManager.Instance.PlayerUIEn("Player2");
            }
        
    }

    public void ShowTextTurn(string PlayerName)
    {
        if (PlayerName == "Player1")
        {
            GameObject TextGameObject = GameObject.Find("Player1TurnUp");
            Animator textAnimator = TextGameObject.GetComponent<Animator>();
            textAnimator.Play("FadeOut", -1, 0);
        }
        if (PlayerName == "Player2")
        {
            GameObject TextGameObject = GameObject.Find("Player2TurnUp");
            Animator textAnimator = TextGameObject.GetComponent<Animator>();
            textAnimator.Play("FadeOut2", -1, 0);
        }
    }

    // Метод для сброса игрока при касании финиша
    public void TeleportPlayerToStart(string playerName)
    {
        ParticleSystem partycleSystemFinishPlayerOne = GameObject.Find("PlayerOneToFinish").GetComponent<ParticleSystem>();
        Debug.Log($"TeleportPlayerToStart вызван для {playerName}");

        // Временно отключаем примагничивание
        bool originalSnapState = PlayerCanSnap;
        PlayerCanSnap = false;

        GameObject player = GameObject.Find(playerName);

        if (player == null)
        {
            Debug.LogError($"Игрок {playerName} не найден!");
            PlayerCanSnap = originalSnapState;
            return;
        }

        if (playerName == "Player1")
        {
            partycleSystemFinishPlayerOne.transform.position = player.transform.position;
            Vector3 newPos = new Vector3(-17.3f, 0, 0);
            player.transform.position = newPos;
            _startPosition1 = newPos;
            player1WasMoving = false;
            player1NeedsSnap = false;
            partycleSystemFinishPlayerOne.Play();
            float step = 200000 * Time.deltaTime;
            partycleSystemFinishPlayerOne.transform.position = Vector3.MoveTowards(partycleSystemFinishPlayerOne.transform.position, player.transform.position, step);
            Debug.Log($"Player1 телепортирован в {newPos}");
        }
        else if (playerName == "Player2")
        {
            Vector3 newPos = new Vector3(17.3f, 0, 0);
            player.transform.position = newPos;
            _startPosition2 = newPos;
            player2WasMoving = false;
            player2NeedsSnap = false;
            // НЕ ОТКЛЮЧАЕМ УПРАВЛЕНИЕ! // P2Move = false;  УБЕРИ ЭТУ СТРОКУ
            Debug.Log($"Player2 телепортирован в {newPos}");
        }

        StartCoroutine(EnableSnapAfterDelay(originalSnapState, 0.1f));
    }
    public IEnumerator EnableSnapAfterDelay(bool originalState, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerCanSnap = originalState;
        Debug.Log($"PlayerCanSnap восстановлен: {PlayerCanSnap}");
    }
    private IEnumerator TimerToNextTurn()
    {
        yield return new WaitForSeconds(1);
        PlayersCanSavesTacks = true;
        ChangeSchem();
    }
    public void SnapingAC(string playerName, bool originalSnapState)
    {
        if (playerName == "Player1")
        {
            player1WasMoving = false;
            player1NeedsSnap = false;
        }
        if (playerName == "Player2")
        {
            player2WasMoving = false;
            player2NeedsSnap = false;
        }
        StartCoroutine(EnableSnapAfterDelay(originalSnapState, 0.1f));
    }
}