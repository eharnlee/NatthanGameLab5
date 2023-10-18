using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPowerUpController : MonoBehaviour, IPowerUpController
{
    public GameObject block;
    private Animator blockAnimator;
    private AudioSource blockBumpAudio;
    public BasePowerUp powerUp;

    void Start()
    {
        blockAnimator = block.GetComponent<Animator>();
        blockBumpAudio = block.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if there is a power up
        if (powerUp != null)
        {
            if (other.gameObject.tag == "Player" && !powerUp.hasSpawned)
            {
                SpawnPowerup();
            }
            else if (other.gameObject.tag == "Player")
            {
                blockBumpAudio.PlayOneShot(blockBumpAudio.clip);
            }
        }
        // else if it is an empty brick
        else if (other.gameObject.tag == "Player")
        {
            blockAnimator.SetTrigger("hit");
            blockBumpAudio.PlayOneShot(blockBumpAudio.clip);
        }
    }

    public void SpawnPowerup()
    {
        // show disabled sprite
        blockAnimator.SetTrigger("spawned");

        // spawn the powerup
        powerUp.SpawnPowerup();
    }

    public void OnLevelRestart()
    {
        blockAnimator.Play("LevelRestart");

        if (powerUp != null)
        {
            if (powerUp.hasSpawned)
            {
                powerUp.LevelRestart();
            }
        }
    }

    // used by animator
    public void Disable()
    {

    }
}