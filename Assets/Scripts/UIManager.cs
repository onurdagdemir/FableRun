using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using CandyCoded.HapticFeedback;

public class UIManager : MonoBehaviour
{
    //Player Prefs karakter anahtarý
    private const string SelectionKey = "PlayerSelection";
    private string charSelection;

    public GameObject MenuScreen;
    public GameObject GameOverScreen;
    public GameObject SecondChanceScreen;
    public GameObject TimeControlButton;
    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI PointText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestText;

    private int _pointAmount = 0;
    private int _coinAmount = 0;
    private bool _isPlayerDead = false;
    private bool _isStarted = false;

    private void Start()
    {
        charSelection = PlayerPrefs.GetString(SelectionKey);
        PlayerManager.Instance.OnCoinAmountChanged += UpdateCoinText;
        PlayerManager.Instance.OnPlayerDead += PlayerDead;
        PlayerManager.Instance.OnFirstDead += PlayerFirstDead;
        PlayerManager.Instance.OnGameStart += EndStartAnm;
        CoinText.text = PlayerManager.Instance.GetCoinAmount().ToString();
        if (charSelection == "Porky")
        {
            TimeControlButton.SetActive(true);
        }
        else
        {
            TimeControlButton.SetActive(false);
        }
        MenuScreen.SetActive(false);
        GameOverScreen.SetActive(false);
    }

    private void EndStartAnm(bool _isEnd)
    {
        _isStarted = _isEnd;
    }

    private void FixedUpdate()
    {
        if (!_isPlayerDead && _isStarted)
        {
            if(charSelection == "Fish")
            {
                _pointAmount += 2;
            }
            else
            {
                _pointAmount++;
            }

            PointText.text = _pointAmount.ToString().PadLeft(6, '0');
        }

    }

    private void UpdateCoinText(int newCoinAmount)
    {
        _coinAmount = newCoinAmount;
        CoinText.text = _coinAmount.ToString();
    }

    public void OpenMenuScreen()
    {
        HapticFeedback.MediumFeedback();
        MenuScreen.SetActive(true);
        PlayerManager.Instance.SetPlayerPaused(true);
        Time.timeScale = 0f;
    }

    public void CloseMenuScreen()
    {
        HapticFeedback.MediumFeedback();
        MenuScreen.SetActive(false);
        PlayerManager.Instance.SetPlayerPaused(false);
        Time.timeScale = 1.0f;
    }

    public void GoToMenu()
    {
        HapticFeedback.MediumFeedback();
        Time.timeScale = 1.0f;
        StartCoroutine(GoMenu());
    }

    private IEnumerator GoMenu()
    {
        HapticFeedback.MediumFeedback();
        DOTween.KillAll();
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("Menu");
    }

    public void Restart()
    {
        HapticFeedback.MediumFeedback();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }

    private void PlayerFirstDead(bool _isDead)
    {
        _isPlayerDead = _isDead;
        SecondChanceScreen.SetActive(_isDead);
    }

    public void ResumeSecondChance()
    {
        HapticFeedback.MediumFeedback();
        SecondChanceScreen.SetActive(false);
        PlayerManager.Instance.SecondChance();
        _isPlayerDead = false;
    }

    private void PlayerDead(bool _isDead)
    {
        _isPlayerDead = _isDead;
        PlayerManager.Instance.SetPointAmount(_pointAmount);
        ScoreText.text = _pointAmount.ToString();
        GameManager.Instance.SetScore(_pointAmount);

        BestText.text = GameManager.Instance.GetScore().ToString();

        GameOverScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnCoinAmountChanged -= UpdateCoinText;
        PlayerManager.Instance.OnPlayerDead -= PlayerDead;
        PlayerManager.Instance.OnFirstDead -= PlayerFirstDead;
    }
}
