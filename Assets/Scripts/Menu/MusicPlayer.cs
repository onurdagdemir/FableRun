using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClips; // Müzik dosyalarý
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Ýlk müziði baþlat
        PlayMusicBasedOnScene();
    }

    void PlayMusicBasedOnScene()
    {
        // Sahne ismini al ve buna göre müzik çal
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Menu":
                PlayMusic(0);
                break;
            case "Game":
                PlayMusic(1);
                break;
                // Diðer sahneleri ekleyebilirsiniz.
        }
    }

    void Update()
    {
        // Eðer müzik tamamlandýysa, bir sonraki müziði çal
        if (!audioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    void PlayNextMusic()
    {
        // Bir sonraki müziði seç ve çal
        AudioClip nextMusic = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.clip = nextMusic;
        audioSource.Play();
    }

    public void PlayMusic(int which)
    {
        // Bir sonraki müziði seç ve çal
        AudioClip nextMusic = musicClips[which];
        audioSource.clip = nextMusic;
        audioSource.Play();
    }

}
