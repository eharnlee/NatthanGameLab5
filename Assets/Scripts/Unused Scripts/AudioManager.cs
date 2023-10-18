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
    // private bool isRestartButtonPressed = true;

    void Awake()
    {
        // subscribe to events

        // SuperMarioManager.instance.gamePause.AddListener(GamePause);
        // SuperMarioManager.instance.gameResume.AddListener(GameResume);
        // SuperMarioManager.instance.levelRestart.AddListener(LevelRestart);
        // SuperMarioManager.instance.marioDeath.AddListener(MarioDeath);
        // SuperMarioManager.instance.gameOver.AddListener(GameOver);
    }

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

    // void OnSceneLoad()
    // {
    //     backgroundMusicAudio = this.transform.Find("BackgroundMusicAudio").gameObject.GetComponent<AudioSource>();
    //     pauseAudio = this.transform.Find("PauseAudio").gameObject.GetComponent<AudioSource>();
    //     marioDeathAudio = this.transform.Find("MarioDeathAudio").gameObject.GetComponent<AudioSource>();
    //     gameOverAudio = this.transform.Find("GameOverAudio").gameObject.GetComponent<AudioSource>();

    //     audioSources = new List<AudioSource>
    //     {
    //         backgroundMusicAudio,
    //         pauseAudio,
    //         gameOverAudio
    //     };

    //     backgroundMusicAudio.Play();
    // }

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
        // isRestartButtonPressed = true;

        StopAllAudio();
        backgroundMusicAudio.Play();
    }

    public void OnMarioDeath()
    {
        marioDeathAudio.Play();
    }

    public void OnGameOver()
    {
        // isRestartButtonPressed = false;
        backgroundMusicAudio.Stop();

        gameOverAudio.Play();

        // StartCoroutine(GameOverCoroutine());
    }

    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

    // IEnumerator GameOverCoroutine()
    // {
    //     yield return new WaitForSecondsRealtime(2.5f);

    //     // in the case where coroutine continues when game has already been restarted,
    //     // do not play the game over audio
    //     if (!isRestartButtonPressed)
    //     {
    //         gameOverAudio.Play();
    //     }
    // }
}
