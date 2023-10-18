using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundMusicAudio;
    private AudioSource pauseAudio;
    private AudioSource marioDeathAudio;
    private AudioSource gameOverAudio;
    private List<AudioSource> audioSources;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusicAudio = this.transform.Find("BackgroundMusicAudio").gameObject.GetComponent<AudioSource>();
        pauseAudio = this.transform.Find("PauseAudio").gameObject.GetComponent<AudioSource>();
        marioDeathAudio = this.transform.Find("MarioDeathAudio").gameObject.GetComponent<AudioSource>();
        gameOverAudio = this.transform.Find("GameOverAudio").gameObject.GetComponent<AudioSource>();

        audioSources = new List<AudioSource>
        {
            backgroundMusicAudio,
            pauseAudio,
            gameOverAudio
        };

        backgroundMusicAudio.Play();
    }

    public void OnGamePause()
    {
        backgroundMusicAudio.Pause();
        pauseAudio.PlayOneShot(pauseAudio.clip);
    }

    public void OnGameResume()
    {
        backgroundMusicAudio.UnPause();
    }

    public void OnLevelRestart()
    {
        StopAllAudio();
        backgroundMusicAudio.Play();
    }

    public void OnMarioDeath()
    {
        marioDeathAudio.Play();
    }

    public void OnGameOver()
    {
        backgroundMusicAudio.Stop();
        gameOverAudio.Play();
    }

    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }
}
