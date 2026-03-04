using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Rendering;

public class CellEnemyType : MonoBehaviour
{
    public EnemyTypeCell currentType;
    public Vector2 VectorAttack;
    public int Health;
    public bool HaveHealth;
    public int AttackVar;
    public List<GameObject> GameObjectAroundThisCell = new List<GameObject>();
    public List<EffectType> CellTypeCanActive = new List<EffectType>();
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
                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position,VectorAttack, 0);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.gameObject == gameObject) continue;

                    GameObject obj = collider.gameObject;
                    bool hasCellType = obj.GetComponent<CellType>() != null;
                    

                    if (collider.name == "Player1"|| collider.name == "Player2" || hasCellType)
                    {
                        if (hasCellType)
                        {
                            if (CellTypeCanActive.Contains(obj.GetComponent<CellType>().currentType))
                            {
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
                            GameManager.Instance.TakeDamage("Player1", AttackVar);
                        }
                        if (collider.name == "Player2")
                        {
                            GameManager.Instance.TakeDamage("Player2", AttackVar);
                        }
                        if (!GameObjectAroundThisCell.Contains(obj))
                            GameObjectAroundThisCell.Add(obj);
                    }
                }
                break;
        }
    }

    public void DestroyEnemy()
    {
        CellType thisCell = GetComponent<CellType>();
        thisCell.ChangeType(EffectType.None);
        Destroy(GetComponent<CellEnemyType>());
        GameManager.Instance.OnSwitchTurn -= CheckAttackCan;
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, VectorAttack);
    }
}
public enum EnemyTypeCell
{
    Tentacle
}
