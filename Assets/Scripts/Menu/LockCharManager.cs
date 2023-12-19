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
        //ilk karakterin kilidini oyun baþlangýcýnda açar
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

                // Karakter kilidi açýksa prefab'ý aktif yap, kapalýysa deaktif yap
                characterPrefab.SetActive(!isUnlocked);

                // Eðer þu an seçili karakterse, seçili karakterin bilgilerini güncelle
                if (i == selectedCharacterIndex)
                {
                    selectedCharacterInfo.isUnlocked = isUnlocked;
                    selectedCharacterInfo.price = CharacterDatas[i].unlockCost;
                    // Diðer karakter özelliklerini de saklayabilirsiniz
                }
            }
        }
    }

    // Altýn ile karakter açma fonksiyonu
    public void UnlockCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            CharacterData characterData = CharacterDatas[characterIndex];

            if (!characterData.isUnlocked)
            {
                // Karakter kilidini aç ve statüyü güncelle
                selectedCharacterIndex = characterIndex;
                characterData.isUnlocked = true;
                characterData.SaveData();

                // Güncellenmiþ karakter durumlarýna göre hiyerarþideki karakter prefab'larýný güncelle
                UpdateCharacterStatus(false);
            }
            else
            {
                // Karakter zaten açýk ise bir þey yapma
                Debug.Log("Karakter zaten açýk.");
            }
        }
    }

    public CharacterInfo GetCharacterInfo(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            // Seçili karakterin bilgilerini al
            CharacterInfo characterInfo;
            characterInfo.isUnlocked = CharacterDatas[characterIndex].isUnlocked;
            characterInfo.price = CharacterDatas[characterIndex].unlockCost;
            // Diðer karakter özelliklerini de alabilirsiniz

            return characterInfo;
        }
        else
        {
            // Geçersiz bir karakter indeksi verildiðinde boþ bir bilgi döndür
            return new CharacterInfo();
        }
    }

    //Test için
    public void LockCharacter(int characterIndex)
    {
        if (characterIndex >= 0 && characterIndex < CharacterDatas.Length)
        {
            CharacterData characterData = CharacterDatas[characterIndex];

            if (characterData.isUnlocked)
            {
                // Karakter kilidini aç ve statüyü güncelle
                selectedCharacterIndex = characterIndex;
                characterData.isUnlocked = false;
                selectedCharacterInfo.price = characterData.unlockCost;

                // Güncellenmiþ karakter durumlarýna göre hiyerarþideki karakter prefab'larýný güncelle
                UpdateCharacterStatus(false);
            }
            else
            {
                // Karakter zaten açýk ise bir þey yapma
                Debug.Log("Karakter zaten açýk.");
            }
        }
    }
}
