using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public CharacterData currentCharacterData;
    public string playerButton;
    public void MoveNewLayer()
    {
        CharacterManager characterManager = GameObject.Find("GameManager").GetComponent<CharacterManager>();
        if (playerButton == "player1")
        {
            characterManager.CharacterDataPlayer1 = currentCharacterData;
        }
        else
        {
            characterManager.CharacterDataPlayer2 = currentCharacterData;
        }
        characterManager.PlayerSellected();
    }
}
