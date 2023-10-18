using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BackToMainMenuButtonController : MonoBehaviour, IInteractiveButton
{
    public void OnButtonClick()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}