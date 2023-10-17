using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPowerUp : BasePowerUp
{
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

        SuperMarioManager.instance.IncreaseScore(1);
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }

    // public override void GameRestart()
    // {
    //     if (spawned)
    //     {
    //         spawned = false;
    //         consumed = false;
    //         moveRight = -1;
    //         animator.SetTrigger("reset");
    //     }
    // }
}