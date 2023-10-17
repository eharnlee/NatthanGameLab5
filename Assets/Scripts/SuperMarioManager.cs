using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SuperMarioManager : Singleton<SuperMarioManager>
{
    // UnityEvents
    public UnityEvent loadScene;
    public UnityEvent scoreChange;
    public UnityEvent livesChange;
    public UnityEvent levelRestart;
    public UnityEvent gameRestart;
    public UnityEvent gamePause;
    public UnityEvent gameResume;
    public UnityEvent marioDeath;
    public UnityEvent gameOver;

    // Mario
    private GameObject marioBody;
    public static Vector3 marioPosition;

    // // Audio
    // public AudioMixer audioMixer;
    // private AudioMixerSnapshot audioMixerDefaultSnapshot;
    // private float specialEventsPitch = 0.95f;

    // Scriptable Objects
    public GameVariables gameVariables;
    public IntVariable lives;
    public IntVariable score;

    GameObject eventSystem;

    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        loadScene.Invoke();

        marioBody = GameObject.Find("Mario");

        Time.timeScale = 1.0f;

        loadScene.Invoke();

        // audioMixerDefaultSnapshot = audioMixer.FindSnapshot("Default");
        // audioMixerDefaultSnapshot.TransitionTo(0.1f);

        // subscribe to scene manager scene change
        // SceneManager.activeSceneChanged += LoadScene;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene(string nextSceneName)
    {
        StartCoroutine(LoadSceneCoroutine(nextSceneName));
    }

    IEnumerator LoadSceneCoroutine(string nextSceneName)
    {
        Time.timeScale = 0.0f;

        if (nextSceneName == "Main Menu")
        {
            gameVariables.currentLevel = "World 1-1";
        }
        else
        {
            gameVariables.currentLevel = nextSceneName;
        }

        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);

        score.currentLevelInitialScore = score.Value;

        yield return new WaitForSecondsRealtime(0.5f);

        loadScene.Invoke();

        marioBody = GameObject.Find("Mario");

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1.0f;
    }

    public void StartNewGame()
    {
        StartCoroutine(StartNewGameCoroutine());
    }

    IEnumerator StartNewGameCoroutine()
    {
        score.SetValue(0);
        lives.SetValue(gameVariables.maxLives);

        yield return new WaitForSecondsRealtime(0.3f);

        LoadScene("World 1-1");
    }

    public void LevelRestart()
    {
        score.SetValue(score.currentLevelInitialScore);
        LoadScene(gameVariables.currentLevel);
        // StartCoroutine(LevelRestartCoroutine());
    }

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

    public void GamePause()
    {
        gamePause.Invoke();
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        gameResume.Invoke();
        Time.timeScale = 1f;
    }

    public void MarioDeath()
    {
        StartCoroutine(MarioDeathCoroutine());
    }

    IEnumerator MarioDeathCoroutine()
    {
        marioDeath.Invoke();

        lives.ApplyChange(-1);
        SetLives();

        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(3f);

        if (lives.Value < 1)
        {
            GameOver();
        }
        else
        {
            LevelRestart();
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
        gameOver.Invoke();
        score.Value = 0;
    }

    public void IncreaseScore(int increment)
    {
        // score += increment;
        // IncreaseAudioMixerSpecialEventsPitch();

        score.ApplyChange(increment);
        SetScore();
    }

    public void ResetHighScore()
    {
        eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        score.ResetHighestValue();
        SetScore();
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

    public void SmallMarioPowerUp()
    {
        marioBody.GetComponent<PlayerMovement>().SmallMarioPowerUp();
    }

    public void SetScore()
    {
        scoreChange.Invoke();
    }

    public void SetLives()
    {
        livesChange.Invoke();
    }
}