using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuHUDManager : MonoBehaviour
{
    private GameObject loadingScreen;
    private GameObject highScoreText;
    public IntVariable score;

    void Awake()
    {
        SuperMarioManager.instance.loadScene.AddListener(OnSceneLoad);
        SuperMarioManager.instance.scoreChange.AddListener(SetScore);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartNewGameButton()
    {
        SuperMarioManager.instance.StartNewGame();
    }
    public void OnSceneLoad()
    {
        loadingScreen = this.transform.Find("LoadingScreen").gameObject;
        highScoreText = this.transform.Find("HighScoreText").gameObject;


        SetScore();
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



    public void SetScore()
    {
        highScoreText.GetComponent<TextMeshProUGUI>().text = "High Score: " + score.previousHighestValue.ToString("D6");
    }

    public void ResetHighScore()
    {
        SuperMarioManager.instance.ResetHighScore();
    }
}
