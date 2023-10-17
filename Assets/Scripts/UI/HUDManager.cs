using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private Vector3[] highScoreTextPosition = {
        new Vector3(-645, 600, 0),
        new Vector3(0, 50, 0)
        };
    private Vector3[] scoreTextPosition = {
        new Vector3(-645, 525, 0),
        new Vector3(0, -25, 0)
        };
    private Vector3[] pauseButtonPosition = {
        new Vector3(1000, 590, 0)
    };
    private Vector3[] restartButtonPosition = {
        new Vector3(1120, 590, 0),
    };

    public IntVariable score;
    public IntVariable lives;

    private GameObject loadingScreen;
    private GameObject gamePausedPanel;
    private GameObject gameOverPanel;
    private GameObject scoreText;
    private GameObject highScoreText;
    private GameObject livesText;
    private GameObject pauseButton;
    private GameObject restartButton;

    void Awake()
    {
        // subscribe to events
        SuperMarioManager.instance.loadScene.AddListener(OnSceneLoad);
        SuperMarioManager.instance.gameOver.AddListener(GameOver);
        SuperMarioManager.instance.livesChange.AddListener(SetLives);
        SuperMarioManager.instance.scoreChange.AddListener(SetScore);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNewGame()
    {
        SuperMarioManager.instance.StartNewGame();
    }

    public void OnSceneLoad()
    {
        loadingScreen = this.transform.Find("LoadingScreen").gameObject;

        gamePausedPanel = this.transform.Find("GamePausedPanel").gameObject;
        gameOverPanel = this.transform.Find("GameOverPanel").gameObject;

        scoreText = this.transform.Find("ScoreText").gameObject;
        highScoreText = this.transform.Find("HighScoreText").gameObject;
        livesText = this.transform.Find("LivesText").gameObject;

        pauseButton = this.transform.Find("PauseButton").gameObject;
        restartButton = this.transform.Find("RestartButton").gameObject;

        // hide gameover panel
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

        SetScore();
        SetLives();

        StartCoroutine(LoadingScreenCoroutine());
    }

    IEnumerator LoadingScreenCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        for (float alpha = 1f; alpha > 0f; alpha -= 0.1f)
        {
            loadingScreen.GetComponent<CanvasGroup>().alpha = alpha;

            yield return new WaitForSecondsRealtime(0.1f);
        }

        loadingScreen.SetActive(false);
    }


    public void LevelRestart()
    {
        SuperMarioManager.instance.LevelRestart();
    }

    public void GamePause()
    {
        gamePausedPanel.SetActive(true);
        pauseButton.SetActive(false);
        SuperMarioManager.instance.GamePause();
    }

    public void GameResume()
    {
        gamePausedPanel.SetActive(false);
        pauseButton.SetActive(true);
        SuperMarioManager.instance.GameResume();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        scoreText.transform.localPosition = scoreTextPosition[1];
        scoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        highScoreText.transform.localPosition = highScoreTextPosition[1];
        highScoreText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        livesText.SetActive(false);

        pauseButton.SetActive(false);
        restartButton.SetActive(false);

        // set highscore
        // highscoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + score.previousHighestValue.ToString("D6");
        // // show
        // highscoreText.SetActive(true);
    }

    public void MainMenu()
    {
        SuperMarioManager.instance.LoadScene("Main Menu");
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