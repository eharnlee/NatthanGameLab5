using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPowerUpController : MonoBehaviour, IPowerUpController
{
    public GameObject questionBlock;
    private Animator blockAnimator;
    private AudioSource blockBumpAudio;

    public GameObject powerUpObject;
    public BasePowerUp powerUp;
    public Animator powerUpAnimator;

    void Start()
    {
        blockAnimator = questionBlock.GetComponent<Animator>();
        blockBumpAudio = questionBlock.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerUp.hasSpawned)
        {


            powerUp.SpawnPowerup();
        }
        else if (other.gameObject.tag == "Player")
        {
            blockBumpAudio.PlayOneShot(blockBumpAudio.clip);
        }
    }

    public void SpawnPowerup()
    {
        // show disabled sprite
        blockAnimator.SetTrigger("spawned");
        // spawn the powerup
        powerUpAnimator.SetTrigger("spawned");
    }

    // used by animator
    public void Disable()
    {
        // this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        // transform.localPosition = new Vector3(0, 0, 0);
    }
}