using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public GameObject Dragon;
    public GameObject Ground;
    private AudioSource dragonSound;

    private void Start()
    {
        dragonSound = GetComponent<AudioSource>();
        StartCoroutine(DragonSound());
        PlayerManager.Instance.OnGameStart += EndStartAnm;
    }

    IEnumerator DragonSound()
    {
        yield return new WaitForSeconds(0.8f);
        dragonSound.Play();
    }

    public void EndStartAnm(bool _isEnd)
    {
        Destroy(Dragon);
        Destroy(Ground);
    }

}
