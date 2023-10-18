using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RestartButtonController : MonoBehaviour, IInteractiveButton
{
    public SimpleGameEvent levelRestart;

    public void OnButtonClick()
    {
        levelRestart.Raise(null);
    }
}
