using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlowerPowerUp : BasePowerUp
{
    private AudioSource fireFlowerAppearAudio;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.FireFlower;

        animator = this.transform.Find("FireFlowerBody").gameObject.GetComponent<Animator>();

        fireFlowerAppearAudio = this.transform.Find("FireFlowerAppearAudio").GetComponent<AudioSource>();

        // for Mario to not be able to interact with fire flower before it spawns
        powerUpCollider.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            Consume();
            ApplyPowerup(col.gameObject.GetComponent<PlayerController>());
        }
        else if (col.gameObject.layer == 7) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                moveRight *= -1;
            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;

        animator.SetTrigger("spawned");
        fireFlowerAppearAudio.PlayOneShot(fireFlowerAppearAudio.clip);

        StartCoroutine(WaitToMoveAfterSpawn());
    }


    public override void ApplyPowerup(MonoBehaviour i)
    {
        // base.ApplyPowerup(i);

        // try
        MarioStateController mario;
        bool result = i.TryGetComponent<MarioStateController>(out mario);
        if (result)
        {
            mario.SetPowerup(this.powerUpType);
        }
    }
    public void Consume()
    {
        consumed = true;

        // TODO: do something with the object
        // SuperMarioManager.instance.SmallMarioPowerUp();

        // then "destroy" power up
        // for the mushroom to remain stationary while BoxCollider2D is inactive
        animator.Play("LevelRestart");
        powerUpCollider.enabled = false;
    }

    IEnumerator WaitToMoveAfterSpawn()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (spawned && !consumed)
        {
            powerUpCollider.enabled = true;
        }
    }

    public override void LevelRestart()
    {
        animator.Play("LevelRestart");

        if (spawned)
        {
            spawned = false;
            consumed = false;
            moveRight = -1;

            powerUpCollider.enabled = false;
        }
    }
}