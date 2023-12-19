using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private int _coinAmount = 0;
    private int _pointAmount = 0;

    public Action<int> OnCoinAmountChanged;
    public Action<bool> OnPlayerDead;
    public Action<bool> OnFirstDead;
    public Action<bool> OnSecondChance;
    public Action<bool> OnPlayerPaused;
    public Action<bool> OnGameStart;

    public AudioClip[] coinSounds;
    public AudioClip crashSound;
    private AudioSource audioSource;

    private GameObject player;

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
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();    
    }

    public void EndStartAnm()
    {
        OnGameStart?.Invoke(true);
    }

    public float GetPlayerPositionX()
    {
        return player.transform.position.x;
    }

    public void AddCoin(int amount)
    {
        _coinAmount += amount;
        OnCoinAmountChanged?.Invoke(_coinAmount);
        PlaySound("Coin");
    }

    public void RemoveCoin(int amount)
    {
        _coinAmount -= amount;
        OnCoinAmountChanged?.Invoke(_coinAmount);
    }

    public void ResetCoin()
    {
        _coinAmount = 0;
    }

    public int GetCoinAmount()
    {
        return _coinAmount;
    }

    public void SetPointAmount(int amount)
    {
        _pointAmount = amount;
    }

    public int GetPointAmount()
    {
        return _pointAmount;
    }

    public void ResetPoint()
    {
        _pointAmount = 0;
    }

    public void SetPlayerDead(bool _isDead)
    {
        GameManager.Instance.SetCoin(_coinAmount);
        OnPlayerDead?.Invoke(_isDead);
        PlaySound("Crash");
    }

    public void DeadBeforeSecondChance()
    {
        OnFirstDead?.Invoke(true);
        PlaySound("Crash");
    }

    public void SecondChance()
    {
        OnSecondChance?.Invoke(true);
    }

    public void SetPlayerPaused(bool _isPaused)
    {
        OnPlayerPaused?.Invoke(_isPaused);
    }

    void PlaySound(string type)
    {
        if (type == "Coin")
        {
            AudioClip nextSound = coinSounds[UnityEngine.Random.Range(0, coinSounds.Length)];
            audioSource.clip = nextSound;
            audioSource.Play();
        }
        else if (type == "Crash")
        {
            audioSource.clip = crashSound;
            audioSource.Play();
        }

    }
}
