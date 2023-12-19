using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBornAnm : MonoBehaviour
{
    private GameObject _bornAnimation;
    private AudioSource _healSound;
    // Start is called before the first frame update
    void Start()
    {
        _bornAnimation = transform.Find("Area_star").gameObject;
        _healSound = GetComponent<AudioSource>();
        _bornAnimation.SetActive(false);
        PlayerManager.Instance.OnSecondChance += SecondChance;
    }

    private void SecondChance(bool isOk)
    {
        _bornAnimation.SetActive(isOk);
        _healSound.Play();
        StartCoroutine(HideBornAnimation());
    }

    IEnumerator HideBornAnimation()
    {
        yield return new WaitForSeconds(2);
        _bornAnimation.SetActive(false);
    }


}
