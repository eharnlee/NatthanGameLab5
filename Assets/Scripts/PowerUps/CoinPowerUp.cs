using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPowerUp : BasePowerUp
{
    public IntGameEvent increaseScore;

    private AudioSource coinAudio;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.Coin;

        coinAudio = this.gameObject.GetComponent<AudioSource>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;

        animator.SetTrigger("spawned");
        coinAudio.PlayOneShot(coinAudio.clip);

        increaseScore.Raise(1);
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }

    // public override void LevelRestart()
    // {
    //     animator.Play("LevelRestart");

    //     if (spawned)
    //     {
    //         spawned = false;
    //         consumed = false;
    //         moveRight = -1;
    //     }
    // }
}