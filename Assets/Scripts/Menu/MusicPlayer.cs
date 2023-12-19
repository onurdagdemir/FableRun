using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] musicClips; // M�zik dosyalar�
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // �lk m�zi�i ba�lat
        PlayMusicBasedOnScene();
    }

    void PlayMusicBasedOnScene()
    {
        // Sahne ismini al ve buna g�re m�zik �al
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Menu":
                PlayMusic(0);
                break;
            case "Game":
                PlayMusic(1);
                break;
                // Di�er sahneleri ekleyebilirsiniz.
        }
    }

    void Update()
    {
        // E�er m�zik tamamland�ysa, bir sonraki m�zi�i �al
        if (!audioSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    void PlayNextMusic()
    {
        // Bir sonraki m�zi�i se� ve �al
        AudioClip nextMusic = musicClips[Random.Range(0, musicClips.Length)];
        audioSource.clip = nextMusic;
        audioSource.Play();
    }

    public void PlayMusic(int which)
    {
        // Bir sonraki m�zi�i se� ve �al
        AudioClip nextMusic = musicClips[which];
        audioSource.clip = nextMusic;
        audioSource.Play();
    }

}
