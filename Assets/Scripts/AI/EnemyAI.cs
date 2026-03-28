using DG.Tweening;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int EnemyLevel;
    public EnemyType currentType;
    private SpriteRenderer spriteRenderer;
    private float visionRadius = 2;
    private int stunsEnemy;
    private float damageRadius = 1;
    private float colissionRadius = 1;
    private float moveDistance = 0.525f;
    public int health = 3;
    private string PlayerTarget;
    private Tween currentTween;
    public void StartInitialize() // начальные действия
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (currentType)
        {
            case EnemyType.Cactus:
                EnemyLevel = 2;
                spriteRenderer.sprite = Resources.Load<Sprite>("sprites/Enemys/CactusEnemyBase");
                GameManager.Instance.OnSwitchTurn += EnemyMakeMove;
                GameManager.Instance.OnSwitchTurn += CheckTargetInRadiusAttack;
                GameManager.Instance.OnSwitchTurn += CheckCollision;
                break;
        }

    }
    public void SetPlayerTarget(string Target)
    {
        PlayerTarget = Target;
    }

    private void EnemyMakeMove() //движение
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                GameObject playerGameObject = GameObject.Find(PlayerTarget);
                Vector3 PlayerPos = playerGameObject.transform.position;
                Vector3 direction = (PlayerPos - transform.position).normalized;
                Vector3 TargetPos = transform.position + direction * moveDistance;

                if (currentTween != null && currentTween.IsActive())
                {
                    currentTween.Kill();
                }
                if(stunsEnemy == 0)
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

    private void CheckCollision()
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                Collider2D[] nearbyCells = Physics2D.OverlapCircleAll(transform.position, colissionRadius);
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
        }
    }
    private void EnemyCheckCellUnder()//начало появления урона, логика его получения
    {
        switch (currentType)
        {
            case EnemyType.Cactus:
                Collider2D[] nearbyCells = Physics2D.OverlapCircleAll(transform.position, damageRadius);
                foreach (Collider2D col in nearbyCells)
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
            switch (currentType)
            {
                case EnemyType.Cactus:
                    GameManager.Instance.OnSwitchTurn -= EnemyMakeMove;
                    GameManager.Instance.OnSwitchTurn -= CheckTargetInRadiusAttack;
                    GameManager.Instance.OnSwitchTurn -= CheckCollision;
                    break;
            }

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
}

public enum EnemyType
{
    Cactus
}
