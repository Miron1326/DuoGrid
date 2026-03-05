using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;
using Slider = UnityEngine.UI.Slider;

public class GameManager : MonoBehaviour
{
    public event Action OnSwitchTurn;

    [Header("Состояния игроков")]
    public bool CellsPlayer1;
    public bool CellsPlayer2;
    public List<EffectType> inventoryCellTypePlayer1 = new List<EffectType>();
    public List<EffectType> inventoryCellTypePlayer2 = new List<EffectType>();
    public List<CanvasGroup> canvasGroupsForPhone = new List<CanvasGroup>();
    public List<CanvasGroup> canvasGroupsForInventoryPlayer1 = new List<CanvasGroup>();
    public List<CanvasGroup> canvasGroupsForInventoryPlayer2 = new List<CanvasGroup>();
    public bool Player1Ready = false;
    public bool Player2Ready = false;
    public string PlayerLocked;
    public int Player1StunEditing;
    public int Player2StunEditing;
    public int player1Tacts;
    public int player2Tacts;
    public int Player1isStunnedFor;
    public int Player2isStunnedFor;
    public int player1Health = 3;
    public int player2Health = 3;
    public int player1Wins = 0;
    public int player2Wins = 0;
    public bool player1IsPoisoned;
    public bool StartGameBoolPlayer1;
    public bool StartGameBoolPlayer2;
    public bool player2IsPoisoned;
    public int player1PoisonedInTurn;
    public int player2PoisonedInTurn;
    public int TurnPlayer1;
    public int TurnPlayer2;
    public Vector2 previousPosition1;
    public Vector2 previousPosition2;
    public string PlayerName;
    public bool Player1Select;
    public bool Player2Select;
    public bool Player1CanPlaceFinish;
    public bool Player2CanPlaceFinish;
    public List<EffectType> bannedTypesToReplace = new List<EffectType>();
    public List<EffectType> allowedTypesInYourSt = new List<EffectType>();
    public List<EffectType> allowedTypesInOtherSt = new List<EffectType>();
    public List<EffectType> allowedTypesInBothSt = new List<EffectType>();
    public List<EffectType> TypeCantStandOn = new List<EffectType>();
    public List<EffectType> TypeCellEnemy = new List<EffectType>();
    public List<EffectType> TypeCellToThrow= new List<EffectType>();
    public List<EffectType> TypeCellCanToBoom = new List<EffectType>();
    public Dictionary<string, int> ItemStatsPlayer1 = new Dictionary<string, int>();
    public Dictionary<string, int> ItemStatsPlayer2 = new Dictionary<string, int>();
    private GameObject _player1GameObject;
    private GameObject _player2GameObject;
    private Vector3 _startPositionPlayer1;
    private Vector3 _startPositionPlayer2;
    private GameObject gameObjectScreenLoading;
    private GameObject gameObjectScreenLoadingSlider;
    public int player1GetDamageForPoisonInTurn;
    private int player2GetDamageForPoisonInTurn;
    private GameObject[] allGameObjects;
    private CellType[] allCellScripts;
    private bool _canCheck = true;
    private EffectManager effectManager;
    public int player1PoisonHave = 3;
    public int player2PoisonHave = 3;
    private bool player1OpenCanvas = false;
    private bool player2OpenCanvas = false;
    private float TimeToBoom;
    private float TimeRemained;
    public Text textForTimer;
    private Canvas canvasTimer;
    private GameObject thisGameObjectTimer;
    private CellInventory cellInventoryPl1;
    private CellInventory cellInventoryPl2;

    public static GameManager Instance
    {
        get; private set; 
    }

    public bool player1IsOpenItems;

    public bool player2IsOpenItems;
    
    public int Poison
    {
        get
        {
            return player1PoisonHave;
        } 
        private set
        {
            player1PoisonHave = value;
        }
    }

    private void Awake()
    {
       
        

    
        gameObjectScreenLoading = GameObject.Find("AsyncCanvas");
        gameObjectScreenLoading.GetComponent<Canvas>().enabled = false;

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        effectManager = GetComponent<EffectManager>();
        StartGame();

        foreach (CanvasGroup canvasGroup in canvasGroupsForPhone)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    private void Update()
    {
        if (TurnPlayer1 == TurnPlayer2)
        {
            if (CellsPlayer2)
            {
                UpdateGameInventory();
            }
        }
        if (TurnPlayer1 > TurnPlayer2)
        {
            if (CellsPlayer1)
            {
                UpdateGameInventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullscreen();
        }

        allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        if (!_canCheck) return;
        StartCoroutine(Timer1SecondToWallChecks());
        _canCheck = false;

    }

    public void SwitchTurn()
    {
        OnSwitchTurn?.Invoke();
    }

    private void StartGame()
    {
        cellInventoryPl1 = GameObject.Find("InventoryPlayer1").transform.Find("CellContainer").GetComponent<CellInventory>();
        cellInventoryPl2 = GameObject.Find("InventoryPlayer2").transform.Find("CellContainer").GetComponent<CellInventory>();
        TimeToBoom = 5;
        TimeRemained = 5;
        textForTimer = GameObject.Find("TextTimerCanvas").GetComponent<Text>();
        canvasTimer = GameObject.Find("TimerCanvas").GetComponent<Canvas>();
        canvasTimer.enabled = false;

        ItemStatsPlayer1.Clear();
        ItemStatsPlayer2.Clear();
        ItemStatsPlayer1.Add("GuavaBoom", 3);
        ItemStatsPlayer2.Add("GuavaBoom", 3);
        int randomMedkitPl1 = UnityEngine.Random.Range(0, 4);
        int randomMedkitPl2 = UnityEngine.Random.Range(0, 4);
        ItemStatsPlayer1.Add("Medkit", randomMedkitPl1);
        ItemStatsPlayer2.Add("Medkit", randomMedkitPl2);
        StartGameBoolPlayer1 = true;
        StartGameBoolPlayer2 = true;
        CanvasGroup canvasGroup1 = GameObject.Find("Player1OfWin").GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup2 = GameObject.Find("Player2OfWin").GetComponent<CanvasGroup>();
        canvasGroup1.alpha = 0f;
        canvasGroup1.interactable = false;
        canvasGroup1.blocksRaycasts = false;
        canvasGroup2.alpha = 0f;
        canvasGroup2.interactable = false;
        canvasGroup2.blocksRaycasts = false;
        player1PoisonHave = 3;
        player2PoisonHave = 3;
         
        player1IsPoisoned = false;
        player2IsPoisoned = false;
        _player1GameObject = GameObject.Find("Player1");
        _player2GameObject = GameObject.Find("Player2");
        _startPositionPlayer1 = new Vector3(17.3f,0,0);
        _startPositionPlayer2 = new Vector3(-17.3f, 0, 0);
        player1Tacts = 0;
        player2Tacts = 0;
        player1Health = 3;
        player2Health = 3;
        player1Wins = 0;
        player2Wins = 0;
        Player1CanPlaceFinish = true;
        Player2CanPlaceFinish = true;
        PlayerUIEn("");
    }

    public void TimerStart(GameObject gameObjectCellTimer)
    {
        canvasTimer.enabled = true;
        TimeRemained = TimeToBoom;
        thisGameObjectTimer = gameObjectCellTimer;
        TimerUpdate();
    }
    private void TimerUpdate()
    {
        if (TimeRemained == 0)
        {
            canvasTimer.enabled = false;
            CellType thisCell = thisGameObjectTimer.GetComponent<CellType>();
            RealizeMechanicsCellItems realizeMechanicsCellItems = thisGameObjectTimer.GetComponent<RealizeMechanicsCellItems>();
            realizeMechanicsCellItems.currentBoomType = TypeOfBoom.Standart;
            realizeMechanicsCellItems.RealizeMechanic(thisCell);
        }
        else
        {
            StartCoroutine(Timer());
        }
        textForTimer.text = "Время до взрыва " + TimeRemained;
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        TimeRemained -= 1;
        TimerUpdate();
    }

    public void PlayerUsePoison(string PlayerName)
    {
        if (PlayerName == "Player1")
        {
            player1PoisonHave--;
        }
        else if(PlayerName == "Player2")
        {
            player2PoisonHave--;
        }
    }
    public void PlayerUseItem(string PlayerName, string ItemName)
    {
        if (PlayerName == "Player1")
        {
            Debug.Log("P1 Use " + ItemName);
            ItemStatsPlayer1[ItemName] --;
        }
        else if (PlayerName == "Player2")
        {
            Debug.Log("P2 Use " + ItemName);
            ItemStatsPlayer2[ItemName] --;
        }
    }
    public void Player1ItemsOpenCanvas()
    {
        player1IsOpenItems = true;
        player1OpenCanvas = true;
        //добавить про текст
        Text HavePoisonText = GameObject.Find("Player1PosionHave").GetComponent<Text>();
        Text HaveMedicText = GameObject.Find("Player1Medic").GetComponent<Text>();
        Text HaveGuavaBoomText = GameObject.Find("Player1GuavaBoom").GetComponent<Text>();
        string bottles;
        if (player1PoisonHave > 1)
        {
            bottles = "бутылки";
        }
        else
        {
            bottles = "бутылку";
        }
        HavePoisonText.text = "Имеет " + player1PoisonHave  + " " + bottles +" яда";
        HaveMedicText.text = "Имеет " + ItemStatsPlayer1["Medkit"] + " " + "аптечек";
        HaveGuavaBoomText.text = "Имеет " + ItemStatsPlayer1["GuavaBoom"] + " "  + " гуава бомю";
        GameObject ItemCanvasGameObject = GameObject.Find("ImageItemsPlayer1");
        RectTransform rectTransform = ItemCanvasGameObject.gameObject.GetComponent<RectTransform>();
        rectTransform.transform.position += new Vector3(2000, 0, 0);
        MovementOnTheMouseManager movementOnTheMouse = GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouse.playerCanMoveByMouse = false;
        UnityEngine.Cursor.visible = true;
    }
    public void Player1ItemsCloseCanvas()
    {
        player1IsOpenItems = false;
        player1OpenCanvas = false;
        GameObject ItemCanvasGameObject = GameObject.Find("ImageItemsPlayer1");
        RectTransform rectTransform = ItemCanvasGameObject.gameObject.GetComponent<RectTransform>();
        if (StartGameBoolPlayer1)
        {
            rectTransform.transform.position -= new Vector3(3000, 0, 0);
            StartGameBoolPlayer1 = false;
        }
        else
        {
            rectTransform.transform.position -= new Vector3(2000, 0, 0);
        }

        MovementOnTheMouseManager movementOnTheMouse = GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouse.playerCanMoveByMouse = true;
    }

    public void Player2ItemsOpenCanvas()
    {
        player2IsOpenItems = true;
        player2OpenCanvas = true;
        //добавить про текст
        Text HavePoisonText = GameObject.Find("Player2PosionHave").GetComponent<Text>();
        Text HaveMedicText = GameObject.Find("Player2Medic").GetComponent<Text>();
        Text HaveGuavaBoomText = GameObject.Find("Player2GuavaBoom").GetComponent<Text>();
        HaveGuavaBoomText.text = "Имеет " + ItemStatsPlayer2["GuavaBoom"] + " " + " гуава бума";
        string bottles;
        if (player2PoisonHave > 1)
        {
            bottles = "бутылки";
        }
        else
        {
            bottles = "бутылку";
        }
        HaveMedicText.text = "Имеет " + ItemStatsPlayer2["Medkit"] + " " + "аптечек";
        HavePoisonText.text = "Имеет " + player2PoisonHave + " " + bottles + " яда";
        GameObject ItemCanvasGameObject = GameObject.Find("ImageItemsPlayer2");
        RectTransform rectTransform = ItemCanvasGameObject.gameObject.GetComponent<RectTransform>();
        rectTransform.transform.position -= new Vector3(2000, 0, 0);
        MovementOnTheMouseManager movementOnTheMouse = GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouse.playerCanMoveByMouse = false;
        UnityEngine.Cursor.visible = true;
    }
    public void Player2ItemsCloseCanvas()
    {
        player2IsOpenItems = false;
        player2OpenCanvas = false;
        GameObject ItemCanvasGameObject = GameObject.Find("ImageItemsPlayer2");
        RectTransform rectTransform = ItemCanvasGameObject.gameObject.GetComponent<RectTransform>();
        if (StartGameBoolPlayer2)
        {
            rectTransform.transform.position += new Vector3(3000, 0, 0);
            StartGameBoolPlayer2 = false;
        }
        else
        {
            rectTransform.transform.position += new Vector3(2000, 0, 0);
        }

        MovementOnTheMouseManager movementOnTheMouse = GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouse.playerCanMoveByMouse = true;
    }
    public void HealPlayer(string playerName, int healTo)
    {
        if(playerName == "Player1")
        {
            player1Health++;
        }
        if (playerName == "Player2")
        {
            player2Health++;
        }
        PlayerUIEn("");
    }
    public void TakeDamage(string playerName, int damage)
    {
        if (playerName == "Explosure Player1")
        {
            player1Health -= damage;
        }
        if (playerName == "Explosure Player2")
        {
            player2Health -= damage;
        }
        if (playerName == "Player1")
        {
            player1Health -= damage;
            if (effectManager != null)
            {
                effectManager.Player1Damaged(GameObject.Find(playerName).transform.position);
            }
            else
            {
                Debug.Log("sdasdasdasdasdasdsad");
            }
        }
        if (playerName == "Player2")
        {
            player2Health -= damage;
            effectManager.Player2Damaged(GameObject.Find(playerName).transform.position);
        }
        if (player1Health < 0)
        {
            player1Health = 0;
        }
        if (player2Health < 0)
        {
            player2Health = 0;
        }
        PlayerUIEn("");
        CheckPlayerDeath();
    }
    private void CheckPlayerDeath()
    {
        if (player1Health == 0)
        {
            CanvasGroup canvasGroup1 = GameObject.Find("Player2OfWin").GetComponent<CanvasGroup>();
            canvasGroup1.alpha = 1f;
            canvasGroup1.interactable = true;
            canvasGroup1.blocksRaycasts = true;
            StartCoroutine(WinCounterSave());
        }
        if(player2Health == 0)
        {
            CanvasGroup canvasGroup1 = GameObject.Find("Player1OfWin").GetComponent<CanvasGroup>();
            canvasGroup1.alpha = 1f;
            canvasGroup1.interactable = true;
            canvasGroup1.blocksRaycasts = true;
            StartCoroutine(WinCounterSave());
        }
    }

    public void PlayerStun(string PlayerName, int stunFor)
    {
        if (PlayerName == "Player1")
        {
            Player1isStunnedFor += stunFor;
        }
        if (PlayerName == "Player2")
        {
            Player2isStunnedFor += stunFor;
        }
    }

    public void StunAllEditings(int stunsFor, string player)
    {
        CellEditor cellEditor = GetComponent<CellEditor>();
        if (stunsFor != Player2StunEditing)
        {
            if (player == "Player2")
            {
                Player2StunEditing = stunsFor;
                cellEditor.Player2Blocked = true;
            }
        }
        if (stunsFor != Player1StunEditing)
        {
            if (player == "Player1")
            {
                Player1StunEditing = stunsFor;
                cellEditor.Player1Blocked = true;
            }
        }

        
        

        OnSwitchTurn -= PlayerCheckStunsEdit;
        OnSwitchTurn += PlayerCheckStunsEdit;        
    }

    public void PlayerCheckStunsEdit()
    {
        CellEditor cellEditor = GetComponent<CellEditor>();
        if (Player1StunEditing > 0)
        {
            if (TurnPlayer1 > TurnPlayer2)
            {
                Player1StunEditing--;
                PlayerLocked = "Player1";
                DiactivateAllEditings("Player1");
            }
        }
        if(Player1StunEditing == 0)
        {
            cellEditor.Player1Blocked = false;
        }
        if (Player2StunEditing > 0)
        {
            if (TurnPlayer1 == TurnPlayer2)
            {
                Player2StunEditing--;
                PlayerLocked = "Player1";
                DiactivateAllEditings("Player2");
            }
        }
        if (Player2StunEditing == 0)
        {
            cellEditor.Player2Blocked = false;
        }
        if (Player1StunEditing == 0 && Player2StunEditing == 0)
        {
            OnSwitchTurn -= PlayerCheckStunsEdit;
        }
    }

    void DiactivateAllEditings(string Player)
    {
        if(Player == "Player1")
        {
            Player = "Player2";
        }
        if (Player == "Player2")
        {
            Player = "Player2";
        }
        Debug.Log(Player);
        PlayerLocked = Player;
        CellEditor cellEditor = GetComponent<CellEditor>();
        if(PlayerLocked == "Player1")
        {
            if (Player == "Player1")
            {
                Debug.Log("P1");
            }
            
        }
        if (PlayerLocked == "Player2")
        {
            if (Player == "Player2")
            {
                Debug.Log("P2");
            }
            
        }

    }

    private IEnumerator WinCounterSave()
    {
        yield return new WaitForSeconds(5);
        RestartGame();
    }
    public void PlayerToWin(string PlayerName)
    {
        if (PlayerName == "Player1")
        {
            player1Wins += 1;
        }
        if(PlayerName == "Player2")
        {
            player2Wins += 1;
        }
        PlayerUIEn("");
        PlayerController playerController = GetComponent<PlayerController>();
        GameObject Player1 = GameObject.Find("Player1");
        GameObject Player2 = GameObject.Find("Player2");
        playerController.PlayerCanSnap = false;
        StartCoroutine(TimerToSnap());
        Player1.transform.position = _startPositionPlayer1;
        Player2.transform.position = _startPositionPlayer2;
        playerController.SnapToGrid("Player1");
        playerController.SnapToGrid("Player2");
    }

    public void ChangeAfterEditing()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        CellEditor cellEditor = GetComponent<CellEditor>();
        cellEditor.ShowUIPanel();
        playerController.ChangeSchem();
    }
    public void PlayerUIEn(string PlayerTurn)
    {
        GameObject TextGameObject1 = GameObject.Find("PlayerUI1");
        if (TextGameObject1 == null) return;
        int p1poisondamageafter = player1GetDamageForPoisonInTurn - TurnPlayer1;
        int p2poisondamageafter = player2GetDamageForPoisonInTurn - TurnPlayer2;
        Text Player1Text = TextGameObject1.GetComponent<Text>();

        if (player1IsPoisoned)
        {
            Player1Text.text = "Игрок 1 \nЗдоровье " + player1Health + " ОТРАВЛЕН, УМРЕТ ЧЕРЕЗ " + p1poisondamageafter + " Ходов" + "\n побед " + player1Wins + " \nТактов " + player1Tacts;
        }
        else
        {
            Player1Text.text = "Игрок 1 \nЗдоровье " + player1Health + "\n побед " + player1Wins + " \nТактов " + player1Tacts;
        }
        

        GameObject TextGameObject2 = GameObject.Find("PlayerUI2");
        Text Player2Text = TextGameObject2.GetComponent<Text>();

        if (player2IsPoisoned)
        {
            Player2Text.text = "Игрок 2 \nЗдоровье " + player2Health + " ОТРАВЛЕН, УМРЕТ ЧЕРЕЗ " + p2poisondamageafter + " Ходов" + "\n побед " + player2Wins + " \nТактов " + player2Tacts;
        }
        else
        {
            Player2Text.text = "Игрок 2 \nЗдоровье " + player2Health + "\n побед " + player2Wins + " \nТактов " + player2Tacts;
        }

        if (PlayerTurn != "")
        {
            if (PlayerTurn == "Player1")
            {
                Player1Text.text = "Игрок 1 ХОДИТ \nЗдоровье " + player1Health + "\n побед " + player1Wins + " \nТактов " + player1Tacts;
            }
            if (PlayerTurn == "Player2")
            {
                Player2Text.text = "Игрок 2 ХОДИТ \nЗдоровье " + player2Health + "\n побед " + player2Wins + " \nТактов " + player2Tacts;
            }
            if (player1IsPoisoned)
            {
                Player1Text.text = "Игрок 1 ХОДИТ \nЗдоровье " + player1Health + " ОТРАВЛЕН, УМРЕТ ЧЕРЕЗ " + p1poisondamageafter + " Ходов" + "\n побед " + player1Wins + " \nТактов " + player1Tacts;
            }
            if (player2IsPoisoned)
            {
                Player2Text.text = "Игрок 2 ХОДИТ \nЗдоровье " + player2Health + " ОТРАВЛЕН, УМРЕТ ЧЕРЕЗ " + p2poisondamageafter + " Ходов" + "\n побед " + player2Wins + " \nТактов " + player1Tacts;
            }
        }
    }
    private IEnumerator TimerToSnap()
    {
        yield return new WaitForSeconds(1);
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.PlayerCanSnap = true;
    }



    public void ResetPlayerPosition(string playerName)
    {
        // Находим PlayerController и телепортируем игрока
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null)
        {
            // Создадим публичный метод в PlayerController для телепортации
            GameObject player = GameObject.Find(playerName);
            if (player != null)
            {
                // Телепортируем Player1 в (-13, 0, 0), Player2 в (13, 0, 0)
                if (playerName == "Player1")
                    player.transform.position = new Vector3(-13, 0, 0);
                else if (playerName == "Player2")
                    player.transform.position = new Vector3(13, 0, 0);
            }
        }
    }

    public void RestartGame()
    {
        DisastersManager disastersManager = GetComponent<DisastersManager>();
        disastersManager.DeleteAllEnemy();
        Debug.Log("dfsdfdfdsfdsfdsds");
        TurnPlayer1 = 0;
        TurnPlayer2 = 0;
        MovementOnTheMouseManager movementOnTheMouseManager = GetComponent<MovementOnTheMouseManager>();
        movementOnTheMouseManager.RestartMovementOnTheMouse();
        GameObject[] player1Cells = GameObject.FindGameObjectsWithTag("Player1Cell");
        Transform WallParent = GameObject.Find("Wall").transform;
        Debug.Log(WallParent);
        
        foreach (GameObject GameObjectcell in player1Cells)
        {
            CellType cellType = GameObjectcell.GetComponent<CellType>();
            cellType.ChangeType(EffectType.None);
            cellType.DisableAllColor = false;
            cellType.UpdateVisual();
            if (cellType.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            {
                Destroy(rigidbody);
                BoxCollider2D boxCollider2D = cellType.gameObject.GetComponent<BoxCollider2D>();
                boxCollider2D.isTrigger = true;
            }
        }
        GameObject[] player2Cells = GameObject.FindGameObjectsWithTag("Player2Cell");
        foreach (GameObject GameObjectcell in player2Cells)
        {
            CellType cellType = GameObjectcell.GetComponent<CellType>();
            cellType.ChangeType(EffectType.None);
            cellType.DisableAllColor = false;
            cellType.UpdateVisual();
        }
        StartGame();
        PlayerController playerController = GetComponent<PlayerController>();
        playerController.RestartInitialize();
        MainMenu mainMenu = GetComponent<MainMenu>();
        mainMenu.CloseMenu();


        for (int i = 0; i < WallParent.childCount; i++)
        {
            Transform child = WallParent.GetChild(i);
            CellType cellType = child.GetComponent<CellType>();
            cellType.currentType = EffectType.Wall;
            cellType.UpdateVisual();
        }
    }
    public void OpenTraining()
    {
        StartCoroutine(LoadTrainingAsync("TrainingScene"));
    }

    public void OpenMainGame()
    {
        StartCoroutine(LoadTrainingAsync("SampleScene"));
    }

    public void OpenMainMenuAllGame()
    {
        StartCoroutine(LoadTrainingAsync("MainMenu"));
    }
    public void OpenInfo()
    {
        GameObject canvasInfo = GameObject.Find("Scroll View");
        CanvasGroup canvasGroup = canvasInfo.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public void CloseInfo()
    {
        GameObject canvasInfo = GameObject.Find("Scroll View");
        CanvasGroup canvasGroup = canvasInfo.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    IEnumerator LoadTrainingAsync(string OpenSceneName)
    {
        gameObjectScreenLoading.GetComponent<Canvas>().enabled = true;
        gameObjectScreenLoadingSlider = GameObject.Find("ScreenLoadingSlider");
        Slider progressBar = gameObjectScreenLoadingSlider.GetComponent<Slider>();
        // Минимальная задержка 2 секунды
        float minLoadTime = 4f;
        float timer = 0f;

        // Начать асинхронную загрузку
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(OpenSceneName);
        asyncLoad.allowSceneActivation = false; // Не активировать сразу

        // Пока не прошло 2 секунды И сцена не загружена на 90%
        while (timer < minLoadTime || asyncLoad.progress < 0.9f)
        {
            timer += Time.deltaTime;

            // Прогресс от 0 до 1
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // Обновить UI
            if (progressBar != null)
                progressBar.value = progress;

            //if (progressText != null)
                //progressText.text = $"Загрузка: {Mathf.Round(progress * 100)}%";

            yield return null;
        }

        // Активировать сцену
        asyncLoad.allowSceneActivation = true;
    }
    public void PlayerPoisoned(string PlayerName)
    {
        if (!player1IsPoisoned)
        {
            if (PlayerName == "Player1")
            {
                player1IsPoisoned = true;
                player1PoisonedInTurn = TurnPlayer1;
                player1GetDamageForPoisonInTurn = TurnPlayer1 + 4;
            }
        }
        if(!player2IsPoisoned)
        {
            if (PlayerName == "Player2")
            {
                player2IsPoisoned = true;
                player2PoisonedInTurn = TurnPlayer2;
                player2GetDamageForPoisonInTurn = TurnPlayer2 + 4;
            }
        }
        
    }
    public void CheckPlayerPoisoned()
    {
        if (player1IsPoisoned)
        {
            PlayerUIEn("");
            if (TurnPlayer1 == player1GetDamageForPoisonInTurn)
            {
                TakeDamage("Player1", 3);
                player1IsPoisoned = false;
                PlayerUIEn("");
            }
        }
        if (player2IsPoisoned)
        {
            PlayerUIEn("");
            if (TurnPlayer2 == player2GetDamageForPoisonInTurn)
            {
                TakeDamage("Player2", 3);
                player2IsPoisoned = false;
                PlayerUIEn("");
            }
        }
    }

    public void AntidoteCellUse(string playerName)
    {
        Debug.Log(playerName);
        if(playerName == "Player1")
        {
            player1IsPoisoned = false;
        }
        if (playerName == "Player2")
        {
            player2IsPoisoned = false;
        }
    }

    private IEnumerator Timer1SecondToWallChecks()
    {
        yield return new WaitForSeconds(1);
        _canCheck = true;
        GameObject[] gameObjectsCell1 = GameObject.FindGameObjectsWithTag("Player1Cell");
        GameObject[] gameObjectsCell2 = GameObject.FindGameObjectsWithTag("Player2Cell");
        foreach (GameObject gameObjectCell in gameObjectsCell1)
        {
            foreach (GameObject gameObjectCell2 in gameObjectsCell2)
            {
                CellType cell1 = gameObjectCell.GetComponent<CellType>();
                CellType cell2 = gameObjectCell2.GetComponent<CellType>();
                if (cell1.currentType == EffectType.Wall)
                {
                    //Rigidbody2D rb = cell.GetComponent<Rigidbody2D>();
                    GameObject ThisGameObject = GameObject.Find(cell1.gameObject.name);
                    BoxCollider2D boxCollider2D = cell1.gameObject.GetComponent<BoxCollider2D>();
                    boxCollider2D.isTrigger = false;
                }
                if (cell2.currentType == EffectType.Wall)
                {
                    //Rigidbody2D rb = cell.GetComponent<Rigidbody2D>();
                    GameObject ThisGameObject = GameObject.Find(cell2.gameObject.name);
                    BoxCollider2D boxCollider2D = cell2.gameObject.GetComponent<BoxCollider2D>();
                    boxCollider2D.isTrigger = false;
                }
            }
                
        }
    }
    public void ApplicationExit()
    {
        Application.Quit();
    }

    public void ToggleFullscreen()
    {
        // Меняем состояние на противоположное
        bool newFullscreenState = !Screen.fullScreen;

        // Применяем
        SetFullscreen(newFullscreenState);
    }

    // Метод для установки конкретного состояния
    public void SetFullscreen(bool isFullscreen)
    {
        // Устанавливаем полный экран
        Screen.fullScreen = isFullscreen;

        // Можно добавить логирование для отладки
        Debug.Log($"Полный экран: {isFullscreen}");
    }

    public void StartGameWithInventoriesPlayer1()
    {
        inventoryCellTypePlayer1.Clear();
        foreach (EffectType effectType in cellInventoryPl1.playerHave)
        {
            inventoryCellTypePlayer1.Add(effectType);
        }

        if (inventoryCellTypePlayer1.Contains(EffectType.Finish))
        {
            Player1Ready = !Player1Ready;
        }
        
        if( Player1Ready )
        {
            foreach (CanvasGroup canvasGroup in canvasGroupsForInventoryPlayer1)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
            
        }
        
        if (Player1Ready && Player2Ready)
        {
            LoadGame();
        }

    }
    public void StartGameWithInventoriesPlayer2()
    {
        inventoryCellTypePlayer2.Clear();
        foreach (EffectType effectType in cellInventoryPl2.playerHave)
        {
            inventoryCellTypePlayer2.Add(effectType);
        }

        if (inventoryCellTypePlayer2.Contains(EffectType.Finish))
        {
            Player2Ready = !Player2Ready;
        }

        if (Player2Ready)
        {
            foreach (CanvasGroup canvasGroup in canvasGroupsForInventoryPlayer2)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
            }
            
        }
        if (Player1Ready && Player2Ready)
        {
            LoadGame();
#if UNITY_ANDROID
        foreach (CanvasGroup canvasGroup in canvasGroupsForPhone)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
#endif
        }
    }

    public void LoadGame()
    {
            if(inventoryCellTypePlayer1.Contains(EffectType.Finish) && inventoryCellTypePlayer2.Contains(EffectType.Finish))
            {
                UpdateGameInventory();
                OnSwitchTurn += UpdateGameInventory;
            }
    }

    public void UpdateGameInventory()
    {
        Debug.Log("fgfdgfdgdfg");
        GameObject InventoryGameObject = GameObject.Find("Panel_CellEditor");
        GameObject InventoryContainerGameObject = InventoryGameObject.transform.Find("Container").gameObject;
        for (int i = 0; i < 5; )
        {
            
            GameObject prephabToChange = GameObject.Find("StandartCellInInventory" + i).gameObject;

            if (TurnPlayer1 == TurnPlayer2)
            {
                CellsPlayer1 = true;
                CellsPlayer2 = false;
                if (i < inventoryCellTypePlayer1.Count)
                {
                    CellInShop cellInShop = prephabToChange.GetComponent<CellInShop>();
                    cellInShop.ChangeCellType(inventoryCellTypePlayer1[i]);
                    cellInShop.ChangeVisual();
                }
                else
                {
                    Debug.Log(i);
                    UnityEngine.UI.Image spriteThisObject = prephabToChange.GetComponent<UnityEngine.UI.Image>();
                    spriteThisObject.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory");
                }

            } else if (TurnPlayer1 > TurnPlayer2)
            {
                CellsPlayer1 = false;
                CellsPlayer2 = true;
                if (i < inventoryCellTypePlayer2.Count)
                {
                    CellInShop cellInShop = prephabToChange.GetComponent<CellInShop>();
                    cellInShop.ChangeCellType(inventoryCellTypePlayer2[i]);
                    cellInShop.ChangeVisual();
                }
                else
                {
                    Debug.Log(i);
                    UnityEngine.UI.Image spriteThisObject = prephabToChange.GetComponent<UnityEngine.UI.Image>();
                    spriteThisObject.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellInInventory");
                }

            }

            i++;
        }
    }

    public void CancelChangingInAllButtonAndroid()
    {
        CellInShop[] buttons = FindObjectsOfType<CellInShop>();
        foreach (var cellInShop in buttons)
        {
            cellInShop.CancelEditing();
        }
    }
}
