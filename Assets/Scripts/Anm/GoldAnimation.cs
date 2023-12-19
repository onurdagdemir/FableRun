using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldAnimation : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    private int startGold = 0; //animasyon baþlangýç deðeri
    private int targetGold;
    public float animationDuration = 1.5f; //coin miktarýna göre deðiþiyor

    private float elapsedTime = 0f;

    private bool _isFinished = false;
     
    private void Start()
    {
        PlayerManager.Instance.OnPlayerDead += Finish;
    }

    private void Finish(bool _isDead)
    {
        targetGold = PlayerManager.Instance.GetCoinAmount();

        if(targetGold <= 5)
        {
            animationDuration = 0.5f;
        }
        else if(targetGold <= 20)
        {
            animationDuration = 1f;
        }
        else
        {
            animationDuration = 1.5f;
        }
        _isFinished = true;
        PlayerManager.Instance.ResetCoin();
    }

    private void Update()
    {
        if (_isFinished)
        {
            if (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;

                float t = elapsedTime / animationDuration;
                float easedT = Mathf.SmoothStep(0f, 1f, t);

                int currentGold = (int)Mathf.Lerp(startGold, targetGold, easedT);
                goldText.text = currentGold.ToString();
            }
            else
            {
                goldText.text = targetGold.ToString();
            }
        }
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerDead -= Finish;
    }
}
