using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameVariables gameVariables;
    public SimpleGameEvent marioDeath;

    // game variables
    private float speed;
    private float maxSpeed;
    private float upSpeed;
    private float deathImpulse;
    private float stompImpulse;
    private Vector3 marioStartingPosition;

    private bool moving = false;
    private bool jumpedState = false;
    private bool onGroundState = true;

    private GameObject marioBody;
    private Rigidbody2D marioBodyRigidBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private Animator marioAnimator;

    private AudioSource marioJumpAudio;
    private AudioSource smallMarioPowerUpAudio;

    [System.NonSerialized]
    public bool alive = true;

    private Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    ActionManager actionManager;



    // Start is called before the first frame update
    void Start()
    {
        speed = gameVariables.speed;
        maxSpeed = gameVariables.maxSpeed;
        upSpeed = gameVariables.upSpeed;
        deathImpulse = gameVariables.deathImpulse;
        stompImpulse = gameVariables.stompImpulse;
        marioStartingPosition = gameVariables.marioStartingPosition;

        actionManager = this.GetComponent<ActionManager>();
        actionManager.jump.AddListener(Jump);
        actionManager.jumpHold.AddListener(JumpHold);
        actionManager.moveCheck.AddListener(MoveCheck);

        marioBody = this.transform.Find("MarioBody").gameObject;
        marioSprite = marioBody.GetComponent<SpriteRenderer>();
        marioBodyRigidBody = this.GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator = marioBody.GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        marioJumpAudio = this.transform.Find("MarioJumpAudio").GetComponent<AudioSource>();
        smallMarioPowerUpAudio = this.transform.Find("SmallMarioPowerUpAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBodyRigidBody.velocity.x));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // if above goomba, stomp on goomba
            if (marioBodyRigidBody.transform.position.y > other.gameObject.transform.position.y + 0.1)
            {
                // alive
                other.gameObject.GetComponent<EnemyController>().stomped();

                marioBodyRigidBody.velocity = new Vector2(marioBodyRigidBody.velocity.x, 0);
                marioBodyRigidBody.AddForce(Vector2.up * stompImpulse, ForceMode2D.Impulse);
            }
            // else if not above goomba, die
            else
            {
                MarioDeath();
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        SuperMarioManager.marioPosition = marioBodyRigidBody.position;

        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBodyRigidBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBodyRigidBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }
    void PlayJumpSound()
    {
        // play jump sound
        marioJumpAudio.PlayOneShot(marioJumpAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBodyRigidBody.velocity = Vector2.zero;
        marioBodyRigidBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);

        // check if it doesn't go beyond maxSpeed
        if (marioBodyRigidBody.velocity.magnitude < maxSpeed)
            marioBodyRigidBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBodyRigidBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            PlayJumpSound();

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBodyRigidBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void MarioDeath()
    {
        marioAnimator.Play("Small Mario Die");
        PlayDeathImpulse();
        alive = false;

        // set gameover scene
        marioDeath.Raise(null);
    }

    public void OnLevelRestart()
    {
        alive = true;

        // reset position
        marioBodyRigidBody.transform.position = marioStartingPosition;

        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.Play("Small Mario Idle");

        // reset camera position
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        gameCamera.position = new Vector3(0, 6.5f, -10);
    }

    public void OnApplyPowerUpToPlayer(IPowerUp powerUp)
    {
        SmallMarioPowerUp();
    }

    public void SmallMarioPowerUp()
    {
        smallMarioPowerUpAudio.PlayOneShot(smallMarioPowerUpAudio.clip);
        marioAnimator.Play("Small Mario Power Up");
    }
}
