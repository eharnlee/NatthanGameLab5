using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerUp : BasePowerUp
{
    private AudioSource magicMushroomAppearAudio;
    private Vector3 magicMushroomStartingPosition;
    private Vector2 velocity;
    private bool canMove;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.MagicMushroom;
        canMove = false;

        animator = this.transform.Find("MagicMushroomBody").gameObject.GetComponent<Animator>();

        magicMushroomAppearAudio = this.transform.Find("MagicMushroomAppearAudio").GetComponent<AudioSource>();

        magicMushroomStartingPosition = this.transform.position;

        // for the mushroom to remain stationary while BoxCollider2D is inactive
        powerUpRigidBody.bodyType = RigidbodyType2D.Static;
        powerUpCollider.enabled = false;

        velocity = new Vector2(3.0f, 0f);
    }

    void Update()
    {
        if (canMove)
        {
            MoveMushroom();
        }
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
        magicMushroomAppearAudio.PlayOneShot(magicMushroomAppearAudio.clip);

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
        canMove = false;
        consumed = true;

        // TODO: do something with the object
        // SuperMarioManager.instance.SmallMarioPowerUp();

        // then "destroy" power up
        // for the mushroom to remain stationary while BoxCollider2D is inactive
        animator.Play("LevelRestart");
        powerUpRigidBody.bodyType = RigidbodyType2D.Static;
        powerUpCollider.enabled = false;
        this.transform.position = magicMushroomStartingPosition;
    }

    IEnumerator WaitToMoveAfterSpawn()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (spawned && !consumed)
        {
            canMove = true;
            powerUpRigidBody.bodyType = RigidbodyType2D.Dynamic;
            powerUpCollider.enabled = true;
        }
    }

    public void MoveMushroom()
    {
        powerUpRigidBody.MovePosition(powerUpRigidBody.position + velocity * moveRight * Time.fixedDeltaTime);
    }

    public override void LevelRestart()
    {
        animator.Play("LevelRestart");

        if (spawned)
        {
            spawned = false;
            consumed = false;
            canMove = false;
            moveRight = -1;

            powerUpRigidBody.bodyType = RigidbodyType2D.Static;
            powerUpCollider.enabled = false;
            this.transform.position = magicMushroomStartingPosition;
        }
    }
}