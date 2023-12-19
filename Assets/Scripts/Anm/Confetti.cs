using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class Confetti : MonoBehaviour
{
    public GameObject ConfettiPrefab;
    private AudioSource ConfettiAudioSource;

    private void Start()
    {
        ConfettiAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HapticFeedback.MediumFeedback();
            Instantiate(ConfettiPrefab, new Vector3(0, 6.5f, transform.position.z + 30f), Quaternion.Euler(0f, 0f, 0f), transform);
            ConfettiAudioSource.Play();
        }
    }
}
