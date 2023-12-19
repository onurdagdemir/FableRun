using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public struct CharacterInfo
{
    public bool isUnlocked;
    public int price;
}

public class LockCharManager : MonoBehaviour
{
    public static LockCharManager Instance;

    public CharacterData[] CharacterDatas;
    public GameObject[] CharacterLocks;

    private int selectedCharacterIndex = -1;
    private CharacterInfo selectedCharacterInfo;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //ilk karakterin kilidini oyun ba�lang�c�nda a�ar
        if (!PlayerPrefs.HasKey("CharacterData_Koala_isUnlocked"))
        {
            PlayerPrefs.SetInt("CharacterData_Koala_isUnlocked", 1);
        }
        UpdateCharacterStatus(true);
    }

    void UpdateCharacterStatus(bool _isLoad)
    {
        for (int i = 0; i < CharacterDatas.Length; i++)
        {
            if (i < CharacterLocks.Length && CharacterDatas[i] != null)
            {
                if (_isLoad)
                {
                    CharacterDatas[i].LoadData();
                }
                GameObject characterPrefab = CharacterLocks[i];
                bool isUnlocked = CharacterDatas[i].isUnlocked;

                // Karakter kilidi a��ksa prefab'� aktif yap, kapal�ysa deaktif yap
                characterPrefab.SetActive(!isUnlocked);

                // E�er �u an se�ili karakterse, se�ili karakterin bilgilerini g�ncelle
                if (i == selectedCharacterIndex)
                {
                    selectedCharacterInfo.isUnlocked = isUnlocked;
                    selectedCharacterInfo.price = CharacterDatas[i].unlockCost;
                    // Di�er karakter �zelliklerini de saklayabilirsiniz
                }
            }
        }
    }

    // Alt�n ile karakter a�ma fonksiyonu
    public void UnlockCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            CharacterData characterData = CharacterDatas[characterIndex];

            if (!characterData.isUnlocked)
            {
                // Karakter kilidini a� ve stat�y� g�ncelle
                selectedCharacterIndex = characterIndex;
                characterData.isUnlocked = true;
                characterData.SaveData();

                // G�ncellenmi� karakter durumlar�na g�re hiyerar�ideki karakter prefab'lar�n� g�ncelle
                UpdateCharacterStatus(false);
            }
            else
            {
                // Karakter zaten a��k ise bir �ey yapma
                Debug.Log("Karakter zaten a��k.");
            }
        }
    }

    public CharacterInfo GetCharacterInfo(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            // Se�ili karakterin bilgilerini al
            CharacterInfo characterInfo;
            characterInfo.isUnlocked = CharacterDatas[characterIndex].isUnlocked;
            characterInfo.price = CharacterDatas[characterIndex].unlockCost;
            // Di�er karakter �zelliklerini de alabilirsiniz

            return characterInfo;
        }
        else
        {
            // Ge�ersiz bir karakter indeksi verildi�inde bo� bir bilgi d�nd�r
            return new CharacterInfo();
        }
    }

    //Test i�in
    public void LockCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            CharacterData characterData = CharacterDatas[characterIndex];

            if (characterData.isUnlocked)
            {
                // Karakter kilidini a� ve stat�y� g�ncelle
                selectedCharacterIndex = characterIndex;
                characterData.isUnlocked = false;
                selectedCharacterInfo.price = characterData.unlockCost;

                // G�ncellenmi� karakter durumlar�na g�re hiyerar�ideki karakter prefab'lar�n� g�ncelle
                UpdateCharacterStatus(false);
            }
            else
            {
                // Karakter zaten a��k ise bir �ey yapma
                Debug.Log("Karakter zaten a��k.");
            }
        }
    }
}
