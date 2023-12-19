using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;
using CandyCoded.HapticFeedback;

public class MenuManager : MonoBehaviour
{
    public GameObject Chars;
    public TextMeshProUGUI CharNameTxt;
    public TextMeshProUGUI CharSkillTxt;
    public TextMeshProUGUI CharPriceTxt;
    public GameObject CharCoinImage;

    public GameObject PlayButton;
    public GameObject BuyButton;
    public GameObject BuyMenu;
    public GameObject FaultMenu;

    public TextMeshProUGUI BuyMenuCharNameTxt;
    public TextMeshProUGUI BuyAmountTxt;
    public TextMeshProUGUI ForBuyAmountTxt;

    private string _charSkill;
    private bool _isTouched = false;

    public AudioClip CoinSound;
    private AudioSource audioSource;

    enum SelectionChar { Koala, Fox, Bear, Badger, Porky, Fish };
    SelectionChar _selectionChar = SelectionChar.Koala;
    SelectionChar[] allValues;

    //Player Prefs karakter anahtarý
    private const string SelectionKey = "PlayerSelection";

    int _currentSelection = 0;
    int _charPos = 0;
    int _charPosOffset = -4;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        audioSource = GetComponent<AudioSource>();
        allValues = (SelectionChar[])Enum.GetValues(typeof(SelectionChar));
        LoadPlayerSelection();
        CharStatus();
        BuyMenu.SetActive(false);
        FaultMenu.SetActive(false);
    }


    private void SavePlayerSelection()
    {
        // Seçilen karakteri PlayerPrefs'e kaydet
        PlayerPrefs.SetString(SelectionKey, _selectionChar.ToString());
    }

    private void LoadPlayerSelection()
    {
        // PlayerPrefs'ten kaydedilmiþ seçimi al, yoksa varsayýlaný kullan
        if (PlayerPrefs.HasKey(SelectionKey))
        {
            string savedSelection = PlayerPrefs.GetString(SelectionKey);
            _selectionChar = (SelectionChar)System.Enum.Parse(typeof(SelectionChar), savedSelection);
        }

        SelectChar();
        _currentSelection = (int) _selectionChar;
        Chars.transform.position = new Vector3(_charPos, 0, 0);
        CharNameTxt.text = _selectionChar.ToString();
        CharSkillTxt.text = _charSkill;
    }

    private void SelectChar()
    {
        switch (_selectionChar)
        {
            case SelectionChar.Koala:
                _charPos = (int) SelectionChar.Koala * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("KoalaSkill");
                break;
            case SelectionChar.Fox:
                _charPos = (int)SelectionChar.Fox * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("FoxSkill");
                break;
            case SelectionChar.Bear:
                _charPos = (int)SelectionChar.Bear * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("BearSkill");
                break;
            case SelectionChar.Badger:
                _charPos = (int)SelectionChar.Badger * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("BadgerSkill");
                break;
            case SelectionChar.Porky:
                _charPos = (int)SelectionChar.Porky * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("PorkySkill");
                break;
            case SelectionChar.Fish:
                _charPos = (int)SelectionChar.Fish * _charPosOffset;
                _charSkill = Lean.Localization.LeanLocalization.GetTranslationText("FishSkill");
                break;

        }
    }

    public void MoveChars(int direction)
    {
        HapticFeedback.MediumFeedback();
        _currentSelection += direction;
        _currentSelection = (_currentSelection + allValues.Length) % allValues.Length;
        _selectionChar = allValues[_currentSelection];
        SelectChar();
        StartCoroutine(MoveTo(_charPos, 0.5f));
    }

    public IEnumerator MoveTo(int position, float duration)
    {
        ////.SetEase(Ease.Linear)
        Chars.transform.DOMoveX(position, duration);

        yield return new WaitForSeconds(duration / 2);

        CharNameTxt.text = _selectionChar.ToString();
        CharSkillTxt.text = _charSkill;

        CharStatus();
    }

    public void CharStatus()
    {
        if (!LockCharManager.Instance.GetCharacterInfo(_currentSelection).isUnlocked)
        {
            CharPriceTxt.text = LockCharManager.Instance.GetCharacterInfo(_currentSelection).price.ToString();
            CharCoinImage.SetActive(true);
            PlayButton.SetActive(false);
            BuyButton.SetActive(true);
        }
        else
        {
            CharPriceTxt.text = "";
            CharCoinImage.SetActive(false);
            PlayButton.SetActive(true);
            BuyButton.SetActive(false);
        }
    }

    public void LoadGame()
    {
        HapticFeedback.HeavyFeedback();
        SavePlayerSelection();
        SceneManager.LoadScene("Game");
    }

    public void CanBuy()
    {
        HapticFeedback.HeavyFeedback();
        int currentCoin;
        int spendCoin;
        currentCoin = GameManager.Instance.GetCoin();
        spendCoin = LockCharManager.Instance.GetCharacterInfo(_currentSelection).price;
        if (currentCoin < spendCoin)
        {
            ForBuyAmountTxt.text = (spendCoin- currentCoin).ToString();
            BuyMenuCharNameTxt.text = _selectionChar.ToString();
            FaultMenu.SetActive(true);
        }
        else
        {
            BuyAmountTxt.text = spendCoin.ToString();
            BuyMenu.SetActive(true);
        }
    }

    public void Buy()
    {
        HapticFeedback.HeavyFeedback();
        int currentCoin;
        int spendCoin;
        currentCoin = GameManager.Instance.GetCoin();
        spendCoin = LockCharManager.Instance.GetCharacterInfo(_currentSelection).price;
        if (currentCoin < spendCoin)
        {
            BuyMenu.SetActive(false);
            FaultMenu.SetActive(false);
        }
        else
        {
            GameManager.Instance.SpendCoin(spendCoin);
            LockCharManager.Instance.UnlockCharacter(_currentSelection);
            MenuUIManager.Instance.UpdateCoin();
            CharStatus();
            audioSource.clip = CoinSound;
            audioSource.Play();
            BuyMenu.SetActive(false);
            FaultMenu.SetActive(false);
        }
    }

    public void CloseMenu()
    {
        HapticFeedback.HeavyFeedback();
        BuyMenu.SetActive(false);
        FaultMenu.SetActive(false);
    }

    public void TestAddCoin()
    {
        GameManager.Instance.SetCoin(500);
        MenuUIManager.Instance.UpdateCoin();
    }

    public void TestRemoveCoin()
    {
        GameManager.Instance.SpendCoin(500);
        MenuUIManager.Instance.UpdateCoin();
    }

    public void Reset()
    {
        LockCharManager.Instance.LockCharacter(_currentSelection);
        CharStatus();
    }

    void Update()
    {
        if (Input.touchCount > 0 && !_isTouched)
        {
            Touch finger = Input.GetTouch(0);

            if (finger.deltaPosition.x > 25 && finger.deltaPosition.y < 20)
            {
                MoveChars(-1);
                _isTouched = true;
                StartCoroutine(ResetTouch());
            }
            else if (finger.deltaPosition.x < -25 && finger.deltaPosition.y < 20)
            {
                MoveChars(1);
                _isTouched = true;
                StartCoroutine(ResetTouch());
            }
        }
    }

    IEnumerator ResetTouch()
    {
        yield return new WaitForSeconds(0.1f);
        _isTouched = false;
    }
}
