using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class CellEnemyType : MonoBehaviour
{
    public int level;
    public Collider2D[] colliders;
    public EnemyTypeCell currentType;
    public Vector2 VectorAttack;
    public int Health;
    public bool HaveHealth;
    public int AttackVar;
    public List<GameObject> GameObjectAroundThisCell = new List<GameObject>();
    public List<EffectType> CellTypeCanActive = new List<EffectType>();
    private CellType currentCellType;

    private void Start()
    {
        currentCellType = GetComponent<CellType>();
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
                                if (obj.GetComponent<CellType>().currentType == EffectType.InfectionCell)
                                {
                                    DestroyEnemy(EffectType.InfectionCell);
                                    return;
                                }

                                if (obj.GetComponent<CellType>().currentType != EffectType.NoneWithNoneEffectedMushrooms)
                                {
                                    AudioManager.Instance.OnCactusAttack();
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
        }
    }

    public void DestroyEnemy(EffectType newEffectType = EffectType.None)
    {
        CellType thisCell = GetComponent<CellType>();
        thisCell.ChangeType(newEffectType);
        Destroy(GetComponent<CellEnemyType>());
        GameManager.Instance.OnSwitchTurn -= CheckAttackCan;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, VectorAttack);
    }

    public void ChangeLevel(int level, string Activator)
    {
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
        
    }
}
public enum EnemyTypeCell
{
    Tentacle
}
