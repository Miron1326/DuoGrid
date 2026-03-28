using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CellType : MonoBehaviour
{
    public CellEffectFromSacrifice cellEffectFromSacrifice;
    public CellClass currentClass;
    public bool ActiveCell;
    public int TurnsHave;
    public StilisticType CurrentStilisticType;
    public int Activates;
    public int MovesComplete;
    public string playerAddModifire;
    private int TimerForItemsCell;
    private const int DamageForExpl = 2;
    public EffectType currentType;
    public Modificator modificatorHave;
    public bool CanPlaceInYourSt;
    public bool CanPlaceToThisCellInSt = true;
    public bool CanPlaceToThisCell = true;
    public bool DisableAllColor = false;
    private int _damage = 1;
    private Color normalColor = Color.gray;
    private Color AntidoteColor = Color.springGreen;
    private Color TentacleColor = Color.aliceBlue;
    private Animator animatorForThisCell;
//speed
    private SpriteRenderer SpriteRenderer;

    private void Start()
    {
        CurrentStilisticType  = GameObject.Find("StilisticManager").GetComponent<StilisticManager>().Currenttype;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        modificatorHave = Modificator.Standart;
        UpdateVisual();
    }

    public void PlayerChangeModifire(string player)
    {
        playerAddModifire = player;
    }

    private void ModifireActivate()
    {
        if (playerAddModifire == "Player1")
        {
            if (GameManager.Instance.TurnPlayer1 == GameManager.Instance.TurnPlayer2)
            {
                MovesComplete++;
            }
        }
        if (playerAddModifire == "Player2")
        {
            if (GameManager.Instance.TurnPlayer1 > GameManager.Instance.TurnPlayer2)
            {
                MovesComplete++;
            }
        }
        if (playerAddModifire == " ")
        {
            return;
        }

        switch (currentType) // постановка изменений за счет модификаторов и типов
        {
            case EffectType.Tentacle:
                if (modificatorHave == Modificator.InCapsule)
                {
                    if (MovesComplete == 6) //прошло 3 хода
                    {
                        CellEnemyType cellEnemyType = GetComponent<CellEnemyType>();
                        cellEnemyType.DestroyEnemy();
                        GameObject newEnemy = Instantiate(GameObject.Find("EnemyPrephab"), transform.position, Quaternion.identity);
                        EnemyAI enemyAI = newEnemy.AddComponent<EnemyAI>();
                        enemyAI.currentType = EnemyType.Cactus;
                        enemyAI.StartInitialize();
                        if (playerAddModifire == "Player1")
                        {
                            enemyAI.SetPlayerTarget("Player1");
                        }
                        if (playerAddModifire == "Player2")
                        {
                            enemyAI.SetPlayerTarget("Player2");
                        }

                    }
                }
                break;
        }
    }

    public bool disableAllColor
    {
        get => DisableAllColor;
        set
        {
            if (DisableAllColor != value)
            {
                Debug.Log($"{gameObject.name}: DisableAllColor изменяется с {DisableAllColor} на {value}");
                DisableAllColor = value;
            }
        }
    }


    public void UpdateVisual()
    {

        if (DisableAllColor)
        {
            Debug.Log($"{gameObject.name}: DisableAllColor = true, пропускаем смену цвета");
            return;
        }
        switch (CurrentStilisticType)
        {
            case StilisticType.Standart:
                switch (currentType)
                {
                    case EffectType.None:

                        if (modificatorHave == Modificator.Standart)
                        {
                            SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCell1");
                        }
                        else
                        {
                            UpdateModificatorsVisual();
                        }
                        break;

                    case EffectType.FireCell: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/FireCell"); break;
                    case EffectType.GuavaBoom: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCell1WithGuavaBoom"); break;
                    case EffectType.Finish: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/finish"); break;
                    case EffectType.Damage: SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/needlemoss"); SpriteRenderer.color = normalColor; break;
                    case EffectType.Poison: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/PoisonCell"); break;
                    case EffectType.Antidote: SpriteRenderer.color = AntidoteColor; break;
                    case EffectType.Wall: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/vine"); currentType = EffectType.Wall; break;
                    case EffectType.MushroomMines: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/mushroomMines"); break;
                    case EffectType.NoneWithNoneEffectedMushrooms: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellWithMushroom"); animatorForThisCell = SpriteRenderer.gameObject.AddComponent<Animator>(); break;
                    case EffectType.Tentacle:

                        if (modificatorHave == Modificator.Standart)
                        {
                            SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/CactusNone");
                        }
                        else
                        {
                            UpdateModificatorsVisual();
                        }
                        break;

                    case EffectType.Medkit:
                            SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/medkitInNoneCell");
                            Animator sourceAnimator = GameObject.Find("animateSprite").GetComponent<Animator>();
                            RuntimeAnimatorController runtimeAnimatorController = sourceAnimator.runtimeAnimatorController;
                            if (runtimeAnimatorController == null)
                            {
                                Debug.LogError("У источника нет Animator Controller!");
                                return;
                            }
                            Animator targetAnimator = SpriteRenderer.gameObject.GetComponent<Animator>();
                            if (targetAnimator == null)
                            {
                                targetAnimator = SpriteRenderer.gameObject.AddComponent<Animator>();
                            }
                            if (targetAnimator.runtimeAnimatorController != runtimeAnimatorController)
                            {
                                targetAnimator.runtimeAnimatorController = runtimeAnimatorController;
                                animatorForThisCell = targetAnimator;
                            }

                        

                        break;

                    case EffectType.Sacrifice: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/sacrificeCell"); break;
                    case EffectType.BloodMoonPortal: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/bloodPortalCell"); break;
                }
                break;


            case StilisticType.Desert:
                switch (currentType)
                {
                    case EffectType.None:

                        if (modificatorHave == Modificator.Standart)
                        {
                            SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/NoneCellDesert");
                        }
                        else
                        {
                            UpdateModificatorsVisual();
                        }
                        break;

                    case EffectType.FireCell: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/FireCell"); break;
                    case EffectType.GuavaBoom: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCell1WithGuavaBoom"); break;
                    case EffectType.Finish: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/FinishDesert"); break;
                    case EffectType.Damage: SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/Desert/DamageCellDesert"); SpriteRenderer.color = normalColor; break;
                    case EffectType.Poison: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/PoisonCell"); break;
                    case EffectType.Antidote: SpriteRenderer.color = AntidoteColor; break;
                    case EffectType.Wall: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/vine"); currentType = EffectType.Wall; break;
                    case EffectType.MushroomMines: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/mushroomMines"); break;
                    case EffectType.NoneWithNoneEffectedMushrooms: SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/NoneCellWithMushroom"); animatorForThisCell = SpriteRenderer.gameObject.AddComponent<Animator>(); break;
                    case EffectType.Tentacle:

                        if (modificatorHave == Modificator.Standart)
                        {
                            SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/CactusNone");
                        }
                        else
                        {
                            UpdateModificatorsVisual();
                        }
                        break;
                }
                break;
        }
        

    }
    public void ActivateStartAnimation()
    {
        switch (currentType)
        {
            case EffectType.NoneWithNoneEffectedMushrooms:
                Animator sourceAnimator = GameObject.Find("startCellSpriteTo").GetComponent<Animator>();

                // ПРАВИЛЬНО: получаем контроллер ИЗ Animator
                RuntimeAnimatorController runtimeAnimatorController = sourceAnimator.runtimeAnimatorController;

                // Проверяем, что контроллер существует
                if (runtimeAnimatorController == null)
                {
                    Debug.LogError("У источника нет Animator Controller!");
                    return;
                }

                // Получаем или добавляем Animator на целевом объекте
                Animator targetAnimator = SpriteRenderer.gameObject.GetComponent<Animator>();
                if (targetAnimator == null)
                {
                    targetAnimator = SpriteRenderer.gameObject.AddComponent<Animator>();
                }

                // Устанавливаем контроллер
                targetAnimator.runtimeAnimatorController = runtimeAnimatorController;
                Debug.Log($"Контроллер '{runtimeAnimatorController.name}' установлен");

                targetAnimator.Play("startNoneWithMashroom");
                break;
        }
        
    }
    
    public void ActivateEffect(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("ActivateEffect: player is null!");
            return;
        }

        switch (currentType)
        {
            case EffectType.FireCell:
            case EffectType.Damage:
                if (!ActiveCell) return;
                GameManager.Instance.TakeDamage(player.name, _damage);
                break;

            case EffectType.Finish:
                Debug.Log(player.name + " коснулся финиша!");

                // Находим PlayerController
                GameObject GameObjectGameManager = GameObject.Find("GameManager");
                PlayerController playerController = GameObjectGameManager.GetComponent<PlayerController>();

                if (playerController == null)
                {
                    Debug.LogError("PlayerController не найден!");
                }
                else
                {
                    Debug.Log("PlayerController найден, вызываю TeleportPlayerToStart...");

                    // Сохраняем текущую позицию до телепортации
                    Vector3 beforePos = player.transform.position;
                    Debug.Log($"Позиция до телепортации: {beforePos}");

                    // Телепортируем игрока в начало
                    playerController.TeleportPlayerToStart(player.name);

                    // Проверяем позицию после телепортации
                    Vector3 afterPos = player.transform.position;
                    Debug.Log($"Позиция после телепортации: {afterPos}");

                    if (beforePos == afterPos)
                    {
                        Debug.LogError("Телепортация НЕ произошла! Позиция не изменилась.");
                    }
                }

                // Вызываем победу
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.PlayerToWin(player.name);
                }
                else
                {
                    Debug.LogError("GameManager.Instance is null!");
                }
                break;

            case EffectType.Poison:
                Debug.Log(player.name + " отравлён");
                GameManager.Instance.PlayerPoisoned(player.name);
                break;

            case EffectType.Antidote:
                Debug.Log(player.name + " использовал антидот");
                GameManager.Instance.AntidoteCellUse(player.name);
                break;

            case EffectType.MushroomMines:
                if (!ActiveCell) return;
                Debug.Log(1);
                Debug.Log(player.name + " получил урон");
                GameManager.Instance.TakeDamage("Explosure " + player.name, DamageForExpl);
                Debug.Log(2);
                EffectManager effectManager = GameObject.Find("GameManager").GetComponent<EffectManager>();
                Debug.Log(3);
                effectManager.PlayerExplosion(player.transform.position);
                GameManager.Instance.PlayerStun(player.name, 1);
                Debug.Log(4);
                currentType = EffectType.None;
                UpdateVisual();
                break;

            case EffectType.Medkit:
                if (!ActiveCell) return;
                Debug.Log(player.name + " использовал аптечку");
                if(animatorForThisCell == null){
                    animatorForThisCell = gameObject.AddComponent<Animator>();
                }
                if (Activates > 0)
                {
                    GameManager.Instance.HealPlayer(player.name, 1);
                }
                switch (Activates)
                {
                    case 4:
                        Activates--;
                        animatorForThisCell.SetBool("Three", true);
                        break;
                    case 3:
                        Activates--;
                        animatorForThisCell.SetBool("Two", true);
                        break;
                    case 2:
                        Activates--;
                        animatorForThisCell.SetBool("One", true);
                        break;
                    case 1:
                        Activates--;
                        animatorForThisCell.SetBool("NoActivates", true);
                        break;
                        
                }

                break;

            case EffectType.Sacrifice:
                if (!ActiveCell) return;
                GameManager.Instance.TakeDamage(player.name, _damage);
                cellEffectFromSacrifice.AfterNeededAction(player.name);
                ChangeType(EffectType.None);
                BloodMagesticManager bloodMagesticManager = GameObject.Find("BloodManager").GetComponent<BloodMagesticManager>();
                bloodMagesticManager.ChanceforMage += 100;
                break;
        }
    }

  
    public void AddModificator(Modificator modificatorToAdd)
    {
        modificatorHave = modificatorToAdd;
    }

    public void UpdateModificatorsVisual()
    {
        if (modificatorHave == Modificator.InCapsule)
        {
            switch (currentType)
            {
                case EffectType.None:
                    SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/CapsuleCell");
                    break;
                case EffectType.Tentacle:
                    SpriteRenderer.color = normalColor; SpriteRenderer.sprite = Resources.Load<Sprite>("sprites/CellsType/Modifires/CactusNoneWithCapsule");
                    break;
            }
        }
    }

    public void ChangeType(EffectType newtype)
    {
        if (GameManager.Instance.cactusLikeCell.Contains(newtype))
        {
            currentClass = CellClass.cactusLike;
        }
        else
        {
            currentClass = CellClass.None;
        }
        if (GameManager.Instance.NewCellsWithMoreFunctional.Contains(newtype))
        {
            if(gameObject.GetComponent<CellEffectFromSacrifice>() == false)
            {
                cellEffectFromSacrifice = gameObject.AddComponent<CellEffectFromSacrifice>();
                cellEffectFromSacrifice.effectType = newtype;
            }

        }
        else
        {
            if (gameObject.GetComponent<CellEffectFromSacrifice>() == true)
            {
                Destroy(gameObject.GetComponent<CellAddToInventory>());
            }
        }
        ActiveCell = true;
        GameManager.Instance.OnSwitchTurn -= TurnUp;
        GameManager.Instance.OnSwitchTurn += TurnUp;
        TurnsHave = 0;
        if (newtype == EffectType.Medkit)
        {
            animatorForThisCell = gameObject.AddComponent<Animator>();
            Activates = 4;
        }
        if (newtype == EffectType.GuavaBoom)
        {
            gameObject.AddComponent<RealizeMechanicsCellItems>();
            GameManager.Instance.TimerStart(gameObject);
        }
        if(GameManager.Instance.TypeCellEnemy.Contains(newtype))
        {
            CellEnemyType cellEnemyType = gameObject.AddComponent<CellEnemyType>();
            cellEnemyType.StartInitialize(newtype);
        }

        if (TryGetComponent<Animator>(out Animator animator))
        {
            animatorForThisCell = animator;
        }
        if (animatorForThisCell != null)
        {
            animatorForThisCell.Play("New State");
        }
        
        modificatorHave = Modificator.Standart;
        GameManager.Instance.OnSwitchTurn += ModifireActivate;
        currentType = newtype;
        DisableAllColor = false;
        UpdateVisual();
        ActivateStartAnimation();
        gameObject.AddComponent<DisastersCheckGameObjectsAround>();
        Debug.Log("клетка изменена на " + newtype);
    }

    public void Restart()
    {
        UpdateVisual();
    }

    public void TurnUp()
    {
        TurnsHave++;
        if (currentType == EffectType.FireCell && TurnsHave == 7)
        {
            ChangeType(EffectType.None);
        }
    }
}
public enum EffectType
{
    None,
    NoneWithNoneEffectedMushrooms,
    Damage,
    Finish,
    Poison,
    Antidote,
    Wall,
    MushroomMines,
    Tentacle,
    GuavaBoom,
    FireCell,
    Capsule,
    Medkit,
    Sacrifice,
    BloodMoonPortal,
    BloodCell
}
public enum Modificator
{
    Standart,
    InCapsule,
    InVarTechnichion
}

public enum Variate
{
    Cell,
    EnemyCell,
    Enemy,
    Item
}

public enum CellClass
{
    cactusLike,
    None
}
