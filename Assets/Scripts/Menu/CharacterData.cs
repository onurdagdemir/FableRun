using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public int unlockCost;
    public bool isUnlocked;

    // PlayerPrefs anahtarýný oluþtur
    private string PlayerPrefsKey => "CharacterData_" + characterName;

    // Karakter açýk durumu güncelle
    public void SaveData()
    {
        PlayerPrefs.SetInt(PlayerPrefsKey + "_isUnlocked", isUnlocked ? 1 : 0);
    }

    // Karakter açýk kontrolü
    public void LoadData()
    {
        isUnlocked = PlayerPrefs.GetInt(PlayerPrefsKey + "_isUnlocked", 0) == 1;
    }
}
