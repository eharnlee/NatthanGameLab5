using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SuperMarioManager : MonoBehaviour
{
    // Scriptable Objects
    public SimpleGameEvent levelRestart;
    public SimpleGameEvent gameOver;
    public GameVariables gameVariables;
    public IntVariable lives;
    public IntVariable score;

    // Mario
    private GameObject marioBody;
    public static Vector3 marioPosition;

    // states
    private bool isGamePaused;
    private bool isLevelRestarted;
    private bool isGameOver;

    // // Audio
    // public AudioMixer audioMixer;
    // private AudioMixerSnapshot audioMixerDefaultSnapshot;
    // private float specialEventsPitch = 0.95f;

    void Start()
    {
        // Set game's framerate to be 30 FPS
        Application.targetFrameRate = 30;

        marioBody = GameObject.Find("Mario");

        StartCoroutine(WaitForLoadingScreenCoroutine());

        isGamePaused = false;
        isLevelRestarted = false;
        isGameOver = false;

        // audioMixerDefaultSnapshot = audioMixer.FindSnapshot("Default");
        // audioMixerDefaultSnapshot.TransitionTo(0.1f);
    }

    // wait for HUDManager's loading screen coroutine to finish before letting the game start running
    IEnumerator WaitForLoadingScreenCoroutine()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2f);

        // if game is not paused and not game over, let the game start running
        if (!isGamePaused && !isGameOver)
        {
            Time.timeScale = 1f;
        }
        // else if (the game is paused but) the restart button has been pressed, let the game start running
        else if (isLevelRestarted)
        {
            isLevelRestarted = false;
            Time.timeScale = 1f;
        }
        // else the game should remain paused
    }

    // new game
    public void OnGameStart()
    {
        isGameOver = false;
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        score.SetValue(0);
        lives.SetValue(gameVariables.maxLives);

        yield return new WaitForSecondsRealtime(0.3f);

        SceneManager.LoadScene("World 1-1", LoadSceneMode.Single);
    }

    public void OnGamePause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void OnGameResume()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void OnLevelRestart()
    {
        isLevelRestarted = true;
        StartCoroutine(WaitForLoadingScreenCoroutine());
    }

    public void OnMarioDeath()
    {
        StartCoroutine(MarioDeathCoroutine());
    }

    IEnumerator MarioDeathCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);

        if (lives.Value < 1)
        {
            GameOver();
        }
        else
        {
            levelRestart.Raise(null);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0.0f;

        gameOver.Raise(null);

        score.Value = 0;
    }

    // public void IncreaseAudioMixerSpecialEventsPitch()
    // {
    //     specialEventsPitch += 0.025f;
    //     audioMixer.SetFloat("SpecialEventsPitch", specialEventsPitch);
    // }

    // public void ResetAudioMixerSpecialEventsPitch()
    // {
    //     specialEventsPitch = 0.975f;
    //     audioMixer.SetFloat("SpecialEventsPitch", 1f);
    // }
}