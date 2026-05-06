using UnityEngine;
using DG.Tweening;

public class AbilityButton : MonoBehaviour
{
    public bool Changed = false;
    public int MoggedZone;
    public string PlayerName;
    public CanvasGroup thisCanvasGroup;
    private CharacterData CharacterData;
    private GameObject mousePrephabGameObject;
    private Sprite spriteMouse;
    public AbilityData thisAbility;
    private CharacterManager characterManager;
    private MovementOnTheMouseManager movementOnTheMouseManager;
    public int cooldown;
    public bool mouseUpdate;

    private void Start()
    {
        mouseUpdate = false;
        thisCanvasGroup = GetComponent<CanvasGroup>();
        mousePrephabGameObject = GameObject.Find("MousePrephab");
        characterManager = GameObject.Find("GameManager").GetComponent<CharacterManager>();
        movementOnTheMouseManager = GameObject.Find("GameManager").GetComponent<MovementOnTheMouseManager>();
        NonVisible();
    }


    public void AbilityInitialize(CharacterData characterData)
    {
        CharacterData = characterData;
        thisAbility = characterData.abilityForCharacter;
        if(thisAbility != null)
        {
            Visible();  
        }
    }



    public void AbilityUse()
    {
        switch(thisAbility.nameAbility)
        {
                //сделать курсор предметом
            case "JammingSignal":
                spriteMouse = Resources.Load<Sprite>("sprites/Characters/CharacterAbilityItems/Aprodox");
                SpriteRenderer spriteRenderer = mousePrephabGameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = spriteMouse;
                mouseUpdate = true;
                break;
        }
    }

    public void Update() // Ability
    {
        if(thisAbility == null) return;
        switch (thisAbility.nameAbility)
        {
            case "JammingSignal":
                TraceNewItem();
                break;
        }

    }

    private void TraceNewItem()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mouseUpdate = false;
            mousePrephabGameObject.transform.position = new Vector3(-14, -35, 0);
        }
        if (mouseUpdate)
        {

            Cursor.visible = false;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0;
            Vector3 WorldPos = Camera.main.ScreenToWorldPoint(mousePosition);
            WorldPos.z = 0;
            mousePrephabGameObject.transform.position = WorldPos;

            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = true;
                GameObject player = GameObject.Find(PlayerName);
                Vector3 TargetPos = WorldPos;
                GameObject item = Instantiate(mousePrephabGameObject);
                AbilityItem abilityItem = item.AddComponent<AbilityItem>();
                item.transform.position = player.transform.position;
                item.transform.DOMove(TargetPos, 0.5f);
                mouseUpdate = false;
                mousePrephabGameObject.transform.position = new Vector3(-14, -35, 0);
                cooldown = thisAbility.cooldown;
                if (cooldown > 0)
                {
                    GameManager.Instance.OnSwitchTurn += WaitCooldown;
                    NonAs50Visible();
                }

                if (Changed)
                {
                    abilityItem.Change(new Vector2(MoggedZone, MoggedZone));
                }
            }
            //при нажатии изменить клетку и не заканчивать ход. точнее только функции.
        }
    }

    private void WaitCooldown()
    {
        cooldown--;
        if(cooldown > 0)
        {
            NonAs50Visible();
        }
        else
        {
            GameManager.Instance.OnSwitchTurn -= WaitCooldown;
            Visible();
        }
    }

    bool CooldownCan()
    {
        if (cooldown == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void NonVisible()
    {
        thisCanvasGroup.alpha = 0;
        thisCanvasGroup.interactable = false;
        thisCanvasGroup.blocksRaycasts = false;
    }

    private void NonAs50Visible()
    {
        thisCanvasGroup.alpha = 0.5f;
        thisCanvasGroup.interactable = false;
        thisCanvasGroup.blocksRaycasts = false;
    }

    private void Visible()
    {
        thisCanvasGroup.alpha = 1;
        thisCanvasGroup.interactable = true;
        thisCanvasGroup.blocksRaycasts = true;
    }
}
