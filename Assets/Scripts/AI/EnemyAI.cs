using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int itemsToStealBloodSucker;
    public int EnemyLevel;
    private EnemyType currentType;
    private SpriteRenderer spriteRenderer;
    private float visionRadius = 2;
    private int stunsEnemy;
    private float damageRadius = 1;
    private float colissionRadius = 1;
    private float moveDistance = 0.525f;
    public int health = 3;
    private string PlayerTarget;
    private Tween currentTween;
    private GameObject Player1;
    private GameObject Player2;
    private Vector3 EscapePosition;
    [SerializeField] private EnemyStage stageOfEnemy;
    [SerializeField] private List<CellType> cellsToEscape = new List<CellType>();
    [SerializeField] private List<ItemData> itemsInThisEnemy = new List<ItemData>();

    private void Start()
    {
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
    }

    private void Update()
    {
        switch (currentType)
        {
            case EnemyType.BloodSucker:
                if(itemsInThisEnemy.Count == 0)
                {
                    CheckCollision();
                }
                else
                {
                    EnemyCheckCellUnder();
                }

                break;
        }
    }
    public EnemyType CurrentType
    {
        get
        {
            return currentType;
        }
        set
        {
            currentType = value;
        }
    }

    public void StartInitialize() // начальные действия
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemsInThisEnemy.Clear();
        switch (currentType)
        {
            case EnemyType.Cactus:
                stageOfEnemy = EnemyStage.Standart;
                EnemyLevel = 2;
                spriteRenderer.sprite = Resources.Load<Sprite>("sprites/Enemys/CactusEnemyBase");
                BasicItemsToEnemy();
                GameManager.Instance.OnSwitchTurn += EnemyMakeMove;
                GameManager.Instance.OnSwitchTurn += CheckTargetInRadiusAttack;
                GameManager.Instance.OnSwitchTurn += CheckCollision;
                break;
            case EnemyType.BloodSucker:
                AddItemsInThisEnemyTo();
                stageOfEnemy = EnemyStage.Standart;
                EnemyLevel = 1;
                itemsToStealBloodSucker = 1;
                spriteRenderer.sprite = Resources.Load<Sprite>("sprites/Enemys/bloodSucker");
                GameManager.Instance.OnSwitchTurn += TargetInMeeleRadius;
                GameManager.Instance.OnSwitchTurn += EnemyMakeMove;
                GameManager.Instance.OnSwitchTurn += TryToEscapingToEscapingPos;
                break;
        }

    }
    public void SetPlayerTarget(string Target)
    {
        PlayerTarget = Target;
    }

    private void TargetInMeeleRadius()//близжайший игрок
    {
        if (Vector3.Distance(transform.position, Player1.transform.position) < Vector3.Distance(transform.position, Player2.transform.position))
        {
            PlayerTarget = "Player1";
        }
        else
        {
            PlayerTarget = "Player2";
        }
    }

    private void EnemyMakeMove() //движение, стандартное движение
    {
        if (stageOfEnemy == EnemyStage.Standart)
        {
            switch (currentType)
            {
                case EnemyType.BloodSucker:
                case EnemyType.Cactus:
                    GameObject playerGameObject = GameObject.Find(PlayerTarget);
                    Vector3 PlayerPos = playerGameObject.transform.position;
                    Vector3 direction = (PlayerPos - transform.position).normalized;
                    Vector3 TargetPos = transform.position + direction * moveDistance;

                    if (currentTween != null && currentTween.IsActive())
                    {
                        currentTween.Kill();
                    }
                    if (stunsEnemy == 0)
                    {
                        currentTween = transform.DOMove(TargetPos, 0.3f).OnComplete(() => { EnemyCheckCellUnder(); });
                    }
                    else
                    {
                        stunsEnemy--;
                    }

                    break;
            }

        }
    }

    private void CheckCollision()
    {

        Collider2D[] nearbyCells = Physics2D.OverlapCircleAll(transform.position, colissionRadius);

        switch (currentType)
        {
            case EnemyType.Cactus:
                
                foreach (Collider2D col in nearbyCells)
                {
                    AbilityItem abilityItem = col.GetComponent<AbilityItem>();
                    if(abilityItem != null)
                    {
                        if(abilityItem.abilityType == AbilityType.Aprodox)
                        {
                            Destroy(abilityItem.gameObject);
                        }

                    }
                }
                    break;
            case EnemyType.BloodSucker:


                foreach (Collider2D col in nearbyCells) 
                {
                    int uses = 0;
                    int randomIndexToSteal;
                    ItemData itemSteal = null;
                    if (col.name == "Player1")
                    {
                        if (GameManager.Instance.PlayerInventory1.data.Count != 0)
                        {
                            bool ComfortItem;
                            if (itemSteal == null)
                            {
                                randomIndexToSteal = Random.Range(0, GameManager.Instance.PlayerInventory1.data.Count);
                                itemSteal = GameManager.Instance.PlayerInventory1.data[randomIndexToSteal];
                            }
                            if (itemSteal.itemName == "Poison")
                            {
                                ComfortItem = true ? GameManager.Instance.player1PoisonHave > 0 : false;
                            }
                            else
                            {
                                ComfortItem = true ? GameManager.Instance.ItemStatsPlayer1[itemSteal.itemName] > 0 : false;
                            }


                            while (!ComfortItem)
                            {
                                uses++;
                                randomIndexToSteal = Random.Range(0, GameManager.Instance.PlayerInventory1.data.Count);
                                Debug.LogError($"{randomIndexToSteal}");
                                itemSteal = GameManager.Instance.PlayerInventory1.data[randomIndexToSteal];
                                if (uses >= GameManager.Instance.PlayerInventory1.data.Count)
                                {
                                    CellType newPortal1 = GameManager.Instance.GetRandomCellNotWall();
                                    newPortal1.currentType = EffectType.BloodPortal;
                                    stageOfEnemy = EnemyStage.Escaping;
                                    return;
                                }
                            }

                            AudioManager.Instance.OnBloodSuckerAttack();
                            itemsInThisEnemy.Add(itemSteal);
                            GameManager.Instance.StealItemsFromPlayer("Player1", itemSteal, itemsToStealBloodSucker);
                            AddItemsInThisEnemyTo();
                            CellType newPortal = GameManager.Instance.GetRandomCellNotWall();
                            newPortal.currentType = EffectType.BloodPortal;
                            stageOfEnemy = EnemyStage.Escaping;

                        }

                    }
                    else if (col.name == "Player2")
                    {
                        if (GameManager.Instance.PlayerInventory2.data.Count != 0)
                        {
                            bool ComfortItem;
                            if (itemSteal == null)
                            {
                                randomIndexToSteal = Random.Range(0, GameManager.Instance.PlayerInventory2.data.Count);
                                itemSteal = GameManager.Instance.PlayerInventory2.data[randomIndexToSteal];
                            }

                            if (itemSteal.itemName == "Poison")
                            {
                                ComfortItem = true ? GameManager.Instance.player2PoisonHave > 0 : false;
                            }
                            else
                            {
                                ComfortItem = true ? GameManager.Instance.ItemStatsPlayer2[itemSteal.itemName] > 0 : false;
                            }
                            while (!ComfortItem)
                            {
                                uses++;
                                randomIndexToSteal = Random.Range(0, GameManager.Instance.PlayerInventory2.data.Count);
                                Debug.LogError($"{randomIndexToSteal}");
                                itemSteal = GameManager.Instance.PlayerInventory2.data[randomIndexToSteal];
                                if (uses >= GameManager.Instance.PlayerInventory1.data.Count)
                                {
                                    return;
                                }
                            }
                            itemsInThisEnemy.Add(itemSteal);
                            GameManager.Instance.StealItemsFromPlayer("Player2", itemSteal, itemsToStealBloodSucker);
                            AddItemsInThisEnemyTo();
                            CellType newPortal = GameManager.Instance.GetRandomCellNotWall();
                            newPortal.currentType = EffectType.BloodPortal;
                            stageOfEnemy = EnemyStage.Escaping;

                        }
                    }
                }

                break;
        }
    }

    private void AddItemsInThisEnemyTo()
    {
        switch (currentType)
        {
            case EnemyType.BloodSucker:
                List<Transform> allChildrens = new List<Transform>();
                foreach(Transform child in transform)
                {
                    allChildrens.Add(child);
                }
                for (int i = 0; i < allChildrens.Count; i++)
                {
                    SpriteRenderer spriteRenderer = allChildrens[i].GetComponent<SpriteRenderer>();
                    if (itemsInThisEnemy.Count == 0)
                    {
                        spriteRenderer.enabled = false;
                    }
                    else
                    {
                        spriteRenderer.enabled = true;
                        spriteRenderer.sprite = itemsInThisEnemy[i].icon;
                    }

                }
                break;
        }
    }

    private void BasicItemsToEnemy()
    {
        List<Transform> allChildrens = new List<Transform>();
        foreach (Transform child in transform)
        {
            allChildrens.Add(child);
        }
        for (int i = 0; i < allChildrens.Count; i++)
        {
            SpriteRenderer spriteRenderer = allChildrens[i].GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }

    private void EnemyCheckCellUnder()//начало появления урона, логика его получения
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                Collider2D[] nearbyCells1 = Physics2D.OverlapCircleAll(transform.position, damageRadius);
                foreach (Collider2D col in nearbyCells1)
                {
                    CellType cellType = col.GetComponent<CellType>();
                    switch (cellType.currentType)
                    {
                        case EffectType.FireCell:
                            TakeDamage(3);
                            break;
                    }
                }
                break;
            case EnemyType.BloodSucker:
                Collider2D[] nearbyCells2 = Physics2D.OverlapCircleAll(transform.position, damageRadius);
                foreach (Collider2D col in nearbyCells2)
                {
                    CellType cellType = col.GetComponent<CellType>();
                    switch (cellType.currentType)
                    {
                        case EffectType.BloodPortal:
                            cellType.ChangeType(EffectType.None);
                            DestroyThisEnemy();
                            break;
                    }
                }
                break;
        }
        
        
    }
    private void CheckTargetInRadiusAttack()
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                Collider2D[] nearbyCells = Physics2D.OverlapCircleAll(transform.position, visionRadius);
                foreach (Collider2D col in nearbyCells)
                {
                    if (col.name == PlayerTarget)
                    {
                        Attack(PlayerTarget);
                    }
                }
                break;
        }
    }

    private void TryToEscapingToEscapingPos()//движение, Побег
    {
        if(stageOfEnemy == EnemyStage.Escaping)
        {
            switch (currentType)
            {
                case EnemyType.BloodSucker:
                    FindGameObjectToEscapeNearest();
                    Vector3 direction = (EscapePosition - transform.position).normalized;
                    Vector3 TargetPos = transform.position + direction * moveDistance;
                    currentTween = transform.DOMove(TargetPos, 0.3f).OnComplete(() => { EnemyCheckCellUnder(); });
                    break;
            }
        }
    }

    private void Attack(string PlayerTarget)
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                stunsEnemy = 2;
                GameManager.Instance.StunAllEditings(2, PlayerTarget);
                break;
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        switch (currentType)
        {
            case EnemyType.Cactus:
                GameManager.Instance.OnSwitchTurn -= EnemyMakeMove;
                GameManager.Instance.OnSwitchTurn -= CheckTargetInRadiusAttack;
                GameManager.Instance.OnSwitchTurn -= CheckCollision;
                break;
            case EnemyType.BloodSucker:
                GameManager.Instance.OnSwitchTurn -= TargetInMeeleRadius;
                GameManager.Instance.OnSwitchTurn -= EnemyMakeMove;
                GameManager.Instance.OnSwitchTurn -= TryToEscapingToEscapingPos;
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "Player1" || collision.collider.name == "Player2" && currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }

    public void DestroyThisEnemy()
    {
        Destroy(gameObject);
    }

    private void FindGameObjectToEscapeNearest()//Найти близжайший объект для побега
    {
        CellType nearest = null;
        float minDistance = float.MaxValue;
        switch (currentType)
        {
            case EnemyType.BloodSucker:
                cellsToEscape = GameManager.Instance.GetAllCellsWithType(EffectType.BloodPortal);
                foreach (CellType cell in cellsToEscape)
                {
                    float distance = Vector2.Distance(transform.position, cell.transform.position);
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        nearest = cell;
                    }
                }
                if (nearest != null)
                {
                    EscapePosition = nearest.transform.position;
                }
                break;
        }
    }
}

public enum EnemyType
{
    None,
    Cactus,
    BloodSucker
}

public enum EnemyStage
{
    Standart,
    Escaping
}
