using System.Collections.Generic;
using System.Linq;
using System.Net;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class CellEnemyType : MonoBehaviour
{
    public bool canBoom;
    public int level;
    public Collider2D[] colliders;
    public EnemyTypeCell currentType;
    public Vector2 VectorAttack;
    public int Health;
    public bool HaveHealth;
    public int AttackVar;
    private string DontAttackPlayer;
    public List<GameObject> GameObjectAroundThisCell = new List<GameObject>();
    public List<EffectType> CellTypeCanActive = new List<EffectType>();
    public CellType currentCellType;
    private GameObject _bulletPrephab;

    private void Start()
    {
        currentCellType = GetComponent<CellType>();
        _bulletPrephab = GameObject.Find("BulletPrephab");
        canBoom = true;
    }

    public void StartInitialize(EffectType effectFromCell)
    {
        switch (effectFromCell)
        {
            case EffectType.Tentacle:
                Animator animator = GetComponent<Animator>();
                if(animator != null)
                {
                    animator.runtimeAnimatorController = null;
                }
                else
                {
                    animator = gameObject.AddComponent<Animator>();
                    animator.runtimeAnimatorController = null;

                }

                currentType = EnemyTypeCell.Tentacle;
                HaveHealth = false;
                AttackVar = 1;
                VectorAttack = new Vector2(5.2f ,5.2f);
                CellTypeCanActive.Add(EffectType.Wall);
                CellTypeCanActive.Add(EffectType.MushroomMines);
                CellTypeCanActive.Add(EffectType.NoneWithNoneEffectedMushrooms);
                CellTypeCanActive.Add(EffectType.GuavaBoom);
                CellTypeCanActive.Add(EffectType.InfectionCell);
                CheckAttackCan();
                GameManager.Instance.OnSwitchTurn += CheckAttackCan;
                break;
            case EffectType.Turret:

                currentType = EnemyTypeCell.Turret;
                HaveHealth = true;
                AttackVar = 1;
                VectorAttack = new Vector2(5.2f, 1);
                if(GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
                {
                    DontAttackPlayer = "Player1";
                }
                else 
                {
                    DontAttackPlayer = "Player2";
                }


                    CheckAttackCan();
                GameManager.Instance.OnSwitchTurn += CheckAttackCan;
                break;
        }
    }

    public string dontAttackPlayer
    {
        get
        {
            return DontAttackPlayer;
        }
        private set
        {
            DontAttackPlayer = value;
        }
    }

    public void CheckAttackCan()
    {
        switch (currentType)
        {
            case EnemyTypeCell.Tentacle:
                colliders = Physics2D.OverlapBoxAll(transform.position,VectorAttack, 0);

                foreach (Collider2D collider in colliders)
                {
                    bool hasAbility = collider.GetComponent<AbilityItem>() != null;
                    if (hasAbility)
                    {
                        Destroy(collider.gameObject);
                    }
                    if (collider.gameObject == gameObject) continue;

                    GameObject obj = collider.gameObject;
                    bool hasCellType = obj.GetComponent<CellType>() != null;
                    

                    if (collider.name == "Player1"|| collider.name == "Player2" || hasCellType)
                    {
                        if (hasCellType)
                        {
                            if (CellTypeCanActive.Contains(obj.GetComponent<CellType>().currentType))
                            {
                                

                                if (obj.GetComponent<CellType>().currentType != EffectType.NoneWithNoneEffectedMushrooms)
                                {
                                    AudioManager.Instance.OnCactusAttack();
                                }
                                if (obj.GetComponent<CellType>().currentType == EffectType.InfectionCell)
                                {
                                    DestroyEnemy(EffectType.InfectionCell);
                                    return;
                                }

                                CellType selectedCell = obj.GetComponent<CellType>();
                                if (obj.GetComponent<CellType>().currentType == EffectType.MushroomMines)
                                {
                                    CellType thisCell = GetComponent<CellType>();
                                    thisCell.ChangeType(EffectType.None);
                                    DestroyEnemy();
                                }
                                
                                selectedCell.ChangeType(EffectType.None);
                                BoxCollider2D boxCollider2DSelected = selectedCell.gameObject.GetComponent<BoxCollider2D>();
                                boxCollider2DSelected.isTrigger = true;
                                Rigidbody2D rigidbody2DSelected = selectedCell.gameObject.GetComponent<Rigidbody2D>();
                                Destroy(rigidbody2DSelected);
                            }
                        }

                        if(collider.name == "Player1")
                        {
                            AudioManager.Instance.OnCactusAttack();
                            GameManager.Instance.TakeDamage("Player1", AttackVar);
                        }
                        if (collider.name == "Player2")
                        {
                            AudioManager.Instance.OnCactusAttack();
                            GameManager.Instance.TakeDamage("Player2", AttackVar);
                        }

                        if (!GameObjectAroundThisCell.Contains(obj))
                            GameObjectAroundThisCell.Add(obj);
                    }
                }
                break;
            case EnemyTypeCell.Turret:
                colliders = Physics2D.OverlapBoxAll(transform.position, VectorAttack, 0);
                List<Collider2D> colidersGet = new List<Collider2D>();
                foreach (Collider2D collider in colliders)
                {
                    string AttackPlayer = "";
                    colidersGet.Clear();
                    if (collider.gameObject == gameObject) continue;
                    if(collider.name == "Player1" && DontAttackPlayer != collider.name)
                    {
                        AttackPlayer = "Player1";
                        colidersGet.Add(collider);
                        GameManager.Instance.TakeDamage("Player1", AttackVar);
                    }
                    if (collider.name == "Player2" && DontAttackPlayer != collider.name)
                    {
                        colidersGet.Add(collider);
                        AttackPlayer = "Player2";
                        GameManager.Instance.TakeDamage("Player2", AttackVar);
                    }

                    if(colidersGet.Count != 0)
                    {
                        Debug.LogError(AttackPlayer);
                        GameObject newBullet = Instantiate(_bulletPrephab, transform.position, Quaternion.identity);
                        BulletAI newBulletAI = newBullet.AddComponent<BulletAI>();
                        newBullet.transform.DOMove(GameObject.Find(AttackPlayer).transform.position, .5f);
                        newBulletAI.nameCollision = AttackPlayer;
                        AudioManager.Instance.OnTurretAttack();
                        return;
                    }

                }
                    break;
        }
    }

    public void DestroyEnemy(EffectType newEffectType = EffectType.None)
    {
        CellType thisCell = GetComponent<CellType>();
        thisCell.ChangeType(newEffectType);
        Destroy(this);
    }
    public void OnDrawGizmos()
    {
        if(currentType != EnemyTypeCell.None)
        Gizmos.DrawWireCube(transform.position, VectorAttack);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSwitchTurn -= CheckAttackCan;
    }
    public void ChangeLevel(int level, string Activator)
    {
        switch (currentType)
        {
            case EnemyTypeCell.Tentacle:
                this.level += level;
                GameObject newEnemy = Instantiate(GameObject.Find("EnemyPrephab"), transform.position, Quaternion.identity);
                EnemyAI enemyAI = newEnemy.AddComponent<EnemyAI>();
                enemyAI.CurrentType = EnemyType.Cactus;
                enemyAI.StartInitialize();
                if (Activator == "Player1")
                {
                    enemyAI.SetPlayerTarget("Player2");
                }
                if (Activator == "Player2")
                {
                    enemyAI.SetPlayerTarget("Player1");
                }
                DestroyEnemy();
                break;
            case EnemyTypeCell.Turret:
                this.level = level;
                switch (this.level)
                {
                    case 2:
                        VectorAttack = new Vector2(10.2f, 1);
                        break;
                    case 3:
                        canBoom = false;
                        VectorAttack = new Vector2(10.2f, 1);
                        break;
                }
                break;
        }
        
        
    }
}
public enum EnemyTypeCell
{
    None,
    Tentacle,
    Turret
}
