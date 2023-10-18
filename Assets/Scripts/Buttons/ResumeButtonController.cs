using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResumeButtonController : MonoBehaviour, IInteractiveButton
{
    public SimpleGameEvent gameResume;

    public void OnButtonClick()
    {
        gameResume.Raise(null);
    }
}
