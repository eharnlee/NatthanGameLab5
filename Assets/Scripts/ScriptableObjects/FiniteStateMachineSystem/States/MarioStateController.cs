using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioStateController : StateController
{
    public GameVariables gameVariables;
    public PowerUpType currentPowerupType = PowerUpType.Default;
    public MarioState shouldBeNextState = MarioState.Default;
    private SpriteRenderer spriteRenderer;

    public override void Start()
    {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart()
    {
        // clear powerup
        currentPowerupType = PowerUpType.Default;
        // set the start state
        TransitionToState(startState);
    }

    public void SetPowerup(PowerUpType i)
    {
        currentPowerupType = i;
    }

    public void SetRendererToFlicker()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(BlinkSpriteRenderer());
    }
    private IEnumerator BlinkSpriteRenderer()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (string.Equals(currentState.name, "InvincibleSmallMario", StringComparison.OrdinalIgnoreCase))
        {
            // Toggle the visibility of the sprite renderer
            spriteRenderer.enabled = !spriteRenderer.enabled;

            // Wait for the specified blink interval
            yield return new WaitForSeconds(gameVariables.flickerInterval);
        }

        spriteRenderer.enabled = true;
    }

    public void Fire()
    {
        this.currentState.DoEventTriggeredActions(this, ActionType.Attack);
    }
}