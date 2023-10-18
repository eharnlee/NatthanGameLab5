using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuHUDManager : MonoBehaviour
{
    public IntVariable score;
    public GameVariables gameVariables;

    private GameObject loadingScreen;
    private GameObject highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        loadingScreen = this.transform.Find("LoadingScreen").gameObject;
        highScoreText = this.transform.Find("HighScoreText").gameObject;

        score.SetValue(0);
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
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

        score.ResetHighestValue();
        SetScore();
    }

    // public void OnSceneLoad()
    // {
    //     loadingScreen = this.transform.Find("LoadingScreen").gameObject;
    //     highScoreText = this.transform.Find("HighScoreText").gameObject;


    //     SetScore();
    //     StartCoroutine(LoadingScreenCoroutine());
    // }
}
