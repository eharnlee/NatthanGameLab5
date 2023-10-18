using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartGameButtonController : MonoBehaviour, IInteractiveButton
{
    public SimpleGameEvent gameStart;

    public void OnButtonClick()
    {
        gameStart.Raise(null);
    }
}
