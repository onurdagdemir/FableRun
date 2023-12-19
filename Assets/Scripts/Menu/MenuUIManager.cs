using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuUIManager : MonoBehaviour
{
    public static MenuUIManager Instance;

    public TextMeshProUGUI BestTxt;
    public TextMeshProUGUI CoinTxt;

    private const string scoreKey = "BestScore";
    private const string coinKey = "CoinAmount";

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
        if (PlayerPrefs.HasKey(scoreKey))
        {
            BestTxt.text = SecurePlayerPrefs.GetEncryptedInt(scoreKey).ToString();
        }
        else
        {
            BestTxt.text = "";
        }

        if (PlayerPrefs.HasKey(coinKey))
        {
            CoinTxt.text = SecurePlayerPrefs.GetEncryptedInt(coinKey).ToString();
        }
        else
        {
            CoinTxt.text = "0";
        }
    }

    public void UpdateCoin()
    {
        CoinTxt.text = SecurePlayerPrefs.GetEncryptedInt(coinKey).ToString();
    }

}
