using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using UnityEngine.UI;
using System.Security.Cryptography;

public class HUDManager : MonoBehaviour
{
    public IntVariable score;
    public IntVariable lives;

    private Vector3[] highScoreTextPosition = {
        new Vector3(-645, 600, 0),
        new Vector3(0, -75, 0)
        };
    private Vector3[] scoreTextPosition = {
        new Vector3(-645, 525, 0),
        new Vector3(0, -150, 0)
        };
    private Vector3[] pauseButtonPosition = {
        new Vector3(1000, 590, 0)
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(1120, 590, 0),
    };

    private GameObject loadingScreen;
    private GameObject gamePausedPanel;
    private GameObject gameOverPanel;
    private GameObject scoreText;
    private GameObject highScoreText;
    private GameObject livesText;
    private GameObject pauseButton;
    private GameObject restartButton;
    private bool isPaused = true;

    // Start is called before the first frame update
    void Start()
    {
        // find game objects
        loadingScreen = this.transform.Find("LoadingScreen").gameObject;

        gamePausedPanel = this.transform.Find("GamePausedPanel").gameObject;
        gameOverPanel = this.transform.Find("GameOverPanel").gameObject;

        scoreText = this.transform.Find("ScoreText").gameObject;
        highScoreText = this.transform.Find("HighScoreText").gameObject;
        livesText = this.transform.Find("LivesText").gameObject;

        pauseButton = this.transform.Find("PauseButton").gameObject;
        restartButton = this.transform.Find("RestartButton").gameObject;

        // after finding objects, start loading screen coroutine
        StartCoroutine(LoadingScreenCoroutine());

        // arrange, display and hide UI elements while loading screen coroutine is ongoing
        gamePausedPanel.SetActive(false);
        gameOverPanel.SetActive(false);

        scoreText.transform.localPosition = scoreTextPosition[0];
        scoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        highScoreText.transform.localPosition = highScoreTextPosition[0];
        highScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Left;

        livesText.SetActive(true);

        pauseButton.SetActive(true);
        pauseButton.transform.localPosition = pauseButtonPosition[0];

        restartButton.SetActive(true);
        restartButton.transform.localPosition = restartButtonPosition[0];

        // set the value for the current level's initial score
        score.currentLevelInitialScore = score.Value;

        SetScore();
        SetLives();

        isPaused = false;
    }

    IEnumerator LoadingScreenCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        for (float alpha = 1f; alpha > 0f; alpha -= 0.1f)
        {
            loadingScreen.GetComponent<CanvasGroup>().alpha = alpha;

            yield return new WaitForSecondsRealtime(0.1f);
        }

        loadingScreen.SetActive(false);
    }

    public void OnGamePause()
    {
        gamePausedPanel.SetActive(true);
        pauseButton.SetActive(false);

        isPaused = true;
    }

    public void OnGameResume()
    {
        gamePausedPanel.SetActive(false);
        pauseButton.SetActive(true);

        isPaused = false;
    }

    public void OnLevelRestart()
    {
        loadingScreen.SetActive(true);
        loadingScreen.GetComponent<CanvasGroup>().alpha = 1f;

        if (isPaused)
        {
            OnGameResume();
        }

        score.SetValue(score.currentLevelInitialScore);
        SetScore();
        SetLives();

        StartCoroutine(LoadingScreenCoroutine());
    }

    public void OnMarioDeath()
    {
        lives.ApplyChange(-1);
        SetLives();
    }

    public void OnGameOver()
    {
        gameOverPanel.SetActive(true);

        scoreText.transform.localPosition = scoreTextPosition[1];
        scoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        highScoreText.transform.localPosition = highScoreTextPosition[1];
        highScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        livesText.SetActive(false);

        pauseButton.SetActive(false);
        restartButton.SetActive(false);
    }

    public void OnScoreIncrease(int increment)
    {
        score.ApplyChange(increment);
        SetScore();
    }

    public void SetScore()
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.Value.ToString("D6");
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + score.previousHighestValue.ToString("D6");
    }

    public void SetLives()
    {
        livesText.GetComponent<TextMeshProUGUI>().text = "Lives: " + lives.Value.ToString();
    }
}