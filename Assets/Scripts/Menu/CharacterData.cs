using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int unlockCost;
    public bool isUnlocked;

    // PlayerPrefs anahtar�n� olu�tur
    private string PlayerPrefsKey => "CharacterData_" + characterName;

    // Karakter a��k durumu g�ncelle
    public void SaveData()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey + "_isUnlocked", isUnlocked ? 1 : 0);
    }

    // Karakter a��k kontrol�
    public void LoadData()
    {
        isUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey + "_isUnlocked", 0) == 1;
    }
}
