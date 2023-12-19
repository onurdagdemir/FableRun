using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private const string scoreKey = "BestScore";
    private const string coinKey = "CoinAmount";

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetScore(int newScore)
    {
        int _bestScore = 0;

        if (PlayerPrefs.HasKey(scoreKey))
        {
            _bestScore = SecurePlayerPrefs.GetEncryptedInt(scoreKey);
        }
        if(_bestScore < newScore)
        {
            SecurePlayerPrefs.SetEncryptedInt(scoreKey, newScore);
        }
    }

    public int GetScore()
    {
        if (PlayerPrefs.HasKey(scoreKey))
        {
            return SecurePlayerPrefs.GetEncryptedInt(scoreKey);
        }
        else 
        { 
            return 0; 
        }

    }

    public void SetCoin(int amount)
    {
        int _coinAmount = 0;

        if (PlayerPrefs.HasKey(coinKey))
        {
            _coinAmount = SecurePlayerPrefs.GetEncryptedInt(coinKey);
        }

        _coinAmount += amount;

        SecurePlayerPrefs.SetEncryptedInt(coinKey, _coinAmount);
    }

    public void SpendCoin(int amount)
    {
        int _coinAmount = 0;

        if (PlayerPrefs.HasKey(coinKey))
        {
            _coinAmount = SecurePlayerPrefs.GetEncryptedInt(coinKey);
        }

        _coinAmount -= amount;

        SecurePlayerPrefs.SetEncryptedInt(coinKey, _coinAmount);
    }

    public int GetCoin()
    {
        if (PlayerPrefs.HasKey(coinKey))
        {
            return SecurePlayerPrefs.GetEncryptedInt(coinKey);
        }
        else
        {
            return 0;
        }
    }

}
