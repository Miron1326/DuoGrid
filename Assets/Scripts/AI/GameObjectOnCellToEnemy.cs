using System;
using UnityEngine;

public class GameObjectOnCellToEnemy : MonoBehaviour
{
    [SerializeField] private EffectType effectParent;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite bloodCapsuleSprite;
    [SerializeField] private int _aliwe;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Change(Sprite sprite, EffectType newEffectType)
    {
        effectParent = newEffectType;
        bloodCapsuleSprite = sprite;
        spriteRenderer.sprite = bloodCapsuleSprite;
        GameManager.Instance.OnSwitchTurn += OnChangeTurn;
    }

    private void OnChangeTurn()
    {
        switch (effectParent)
        {
            case EffectType.BloodCapsule:
                _aliwe++;
                AfterChangeTurn();
                break;
        }
    }

    private void AfterChangeTurn()
    {
        int Count = GameManager.Instance.EffectToCountAliwe[effectParent];
        if (_aliwe >= Count)
        {
            switch (effectParent)
            {
                case EffectType.BloodCapsule:
                    InstantiateNewObjectToThisPos(true, EnemyType.BloodSucker, true);
                    break;
            }
        }
    }

    private void InstantiateNewObjectToThisPos(bool newEnemy = false, EnemyType newEnemyType = EnemyType.None, bool DeleteAfterUse = false)
    {
        if (newEnemy)
        {
            if(newEnemyType != EnemyType.None)
            {
                GameObject newEnemyGameObject = Instantiate(GameObject.Find("EnemyPrephab"), transform.position, Quaternion.identity);
                EnemyAI enemyAI = newEnemyGameObject.AddComponent<EnemyAI>();
                enemyAI.CurrentType = newEnemyType;
                enemyAI.StartInitialize();
                if (DeleteAfterUse)
                {
                    DestroyThisObject();
                }
            }
        }
    }

    private void DestroyThisObject()
    {
        Destroy(gameObject);
        GameManager.Instance.OnSwitchTurn -= OnChangeTurn;
    }
}
