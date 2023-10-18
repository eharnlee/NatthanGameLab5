using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ClearPowerUp")]
public class ClearPowerUpAction : Action
{
    public override void Act(StateController controller)
    {
        MarioStateController m = (MarioStateController)controller;
        m.currentPowerupType = PowerUpType.Default;
    }
}