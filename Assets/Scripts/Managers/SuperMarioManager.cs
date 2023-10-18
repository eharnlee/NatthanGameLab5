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
    private bool isGamePaused;
    private bool isGameRestarted;

    // UnityEvents
    // public UnityEvent loadScene;
    // public UnityEvent scoreChange;
    // public UnityEvent livesChange;
    // public UnityEvent levelRestart;
    // // public UnityEvent gameRestart;
    // public UnityEvent gamePause;
    // public UnityEvent gameResume;
    // public UnityEvent marioDeath;
    // public UnityEvent gameOver;

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
        isGameRestarted = false;

        // audioMixerDefaultSnapshot = audioMixer.FindSnapshot("Default");
        // audioMixerDefaultSnapshot.TransitionTo(0.1f);
    }

    // wait for HUDManager's loading screen coroutine to finish before letting the game start running
    IEnumerator WaitForLoadingScreenCoroutine()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2f);

        // if game is not paused, let the game start running
        if (!isGamePaused)
        {
            Time.timeScale = 1f;
        }
        // else if (the game is paused but) the restart button has been pressed, let the game start running
        else if (isGameRestarted)
        {
            isGameRestarted = false;
            Time.timeScale = 1f;
        }
        // else the game should remain paused
    }

    // new game
    public void OnGameStart()
    {
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
        isGameRestarted = true;
        StartCoroutine(WaitForLoadingScreenCoroutine());

        // score.SetValue(score.currentLevelInitialScore);
        // SceneManager.LoadScene(gameVariables.currentLevel, LoadSceneMode.Single);
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
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0.0f;

        gameOver.Raise(null);

        score.Value = 0;
    }

    public void SmallMarioPowerUp()
    {
        marioBody.GetComponent<PlayerController>().SmallMarioPowerUp();
    }

    // public void LoadScene(string nextSceneName)
    // {
    //     StartCoroutine(LoadSceneCoroutine(nextSceneName));
    // }

    // IEnumerator LoadSceneCoroutine(string nextSceneName)
    // {
    //     Time.timeScale = 0.0f;

    //     if (nextSceneName == "Main Menu")
    //     {
    //         gameVariables.currentLevel = "World 1-1";
    //     }
    //     else
    //     {
    //         gameVariables.currentLevel = nextSceneName;
    //     }

    //     SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

    //     score.currentLevelInitialScore = score.Value;

    //     yield return new WaitForSecondsRealtime(0.5f);

    //     // loadScene.Invoke();

    //     marioBody = GameObject.Find("Mario");

    //     yield return new WaitForSecondsRealtime(1.5f);

    //     Time.timeScale = 1.0f;
    // }



    // public void GameRestart()
    // {
    //     score.SetValue(0);
    //     score.currentLevelInitialScore = 0;
    //     lives.SetValue(gameVariables.maxLives);


    // }

    // IEnumerator LevelRestartCoroutine()
    // {

    //     Time.timeScale = 0.0f;
    //     SceneManager.LoadSceneAsync(gameVariables.currentLevel, LoadSceneMode.Single);

    //     yield return new WaitForSecondsRealtime(0.5f);

    //     loadScene.Invoke();
    //     gameRestart.Invoke();

    //     marioBody = GameObject.Find("Mario");

    //     Time.timeScale = 1.0f;
    //     // ResetAudioMixerSpecialEventsPitch();
    // }   

    // public void IncreaseScore(int increment)
    // {
    //     // score += increment;
    //     // IncreaseAudioMixerSpecialEventsPitch();

    //     score.ApplyChange(increment);
    //     SetScore();
    // }

    // public void ResetHighScore()
    // {
    //     GameObject eventSystem = GameObject.Find("EventSystem");
    //     eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

    //     score.ResetHighestValue();
    //     SetScore();
    // }

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

    // public void SetScore()
    // {
    //     // scoreChange.Invoke();
    // }

    // public void SetLives()
    // {
    //     // livesChange.Invoke();
    // }
}