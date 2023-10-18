using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    public SimpleGameEvent gamePause;

    public void OnButtonClick()
    {
        gamePause.Raise(null);
    }
}
