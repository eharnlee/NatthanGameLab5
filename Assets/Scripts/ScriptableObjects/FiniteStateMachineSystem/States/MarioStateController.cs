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
    private AudioSource marioPowerUpAudio;


    public override void Start()
    {
        base.Start();
        GameRestart(); // clear powerup in the beginning, go to start state
        marioPowerUpAudio = this.transform.Find("MarioPowerUpAudio").GetComponent<AudioSource>();
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
        if (i != PowerUpType.Damage)
        {
            marioPowerUpAudio.PlayOneShot(marioPowerUpAudio.clip);
        }
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