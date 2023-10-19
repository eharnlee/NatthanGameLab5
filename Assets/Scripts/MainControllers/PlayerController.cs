using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
// using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public SimpleGameEvent marioDeath;
    public BoolVariable marioFaceRight;

    public GameVariables gameVariables;

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

    private Rigidbody2D thisRigidBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private Animator marioAnimator;

    private AudioSource marioJumpAudio;
    private AudioSource marioPowerUpAudio;

    [System.NonSerialized]
    public bool alive = true;

    private Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    ActionManager actionManager;

    private MarioStateController marioStateController;


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

        marioSprite = this.GetComponent<SpriteRenderer>();
        thisRigidBody = this.GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator = this.GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        marioJumpAudio = this.transform.Find("MarioJumpAudio").GetComponent<AudioSource>();
        marioPowerUpAudio = this.transform.Find("MarioPowerUpAudio").GetComponent<AudioSource>();

        marioStateController = this.GetComponent<MarioStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(thisRigidBody.velocity.x));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // if above goomba, stomp on goomba
            if (thisRigidBody.transform.position.y > other.gameObject.transform.position.y + 0.2)
            {
                // alive
                other.gameObject.GetComponent<EnemyController>().stomped();

                thisRigidBody.velocity = new Vector2(thisRigidBody.velocity.x, 0);
                thisRigidBody.AddForce(Vector2.up * stompImpulse, ForceMode2D.Impulse);
            }
            // else if not above goomba, die
            else
            {
                DamageMario();
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
        SuperMarioManager.marioPosition = thisRigidBody.position;

        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            updateMarioShouldFaceRight(false);
            marioSprite.flipX = true;

            if (thisRigidBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            updateMarioShouldFaceRight(true);
            marioSprite.flipX = false;

            if (thisRigidBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    private void updateMarioShouldFaceRight(bool value)
    {
        faceRightState = value;
        marioFaceRight.SetValue(faceRightState);
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);

        // check if it doesn't go beyond maxSpeed
        if (thisRigidBody.velocity.magnitude < maxSpeed)
            thisRigidBody.AddForce(movement * speed);
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
            thisRigidBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            marioJumpAudio.PlayOneShot(marioJumpAudio.clip);

            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            thisRigidBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void MarioDeath()
    {
        alive = false;

        marioAnimator.Play("Small Mario Die");
        MarioDeathImpulse();

        marioDeath.Raise(null);
    }

    void MarioDeathImpulse()
    {
        thisRigidBody.velocity = Vector2.zero;
        thisRigidBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    public void OnLevelRestart()
    {
        alive = true;
        jumpedState = false;
        onGroundState = true;

        // reset position
        thisRigidBody.transform.position = marioStartingPosition;

        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetBool("onGround", onGroundState);
        marioAnimator.Play("LevelRestart");

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
        marioPowerUpAudio.PlayOneShot(marioPowerUpAudio.clip);
        marioAnimator.Play("Small Mario Power Up");
    }

    public void DamageMario()
    {
        // GameOverAnimationStart(); // last time Mario dies right away

        // pass this to StateController to see if Mario should start game over
        // since both state StateController and MarioStateController are on the same gameobject, it's ok to cross-refer between scripts
        if (marioStateController.currentState.name == "SmallMario")
        {
            MarioDeath();
        }

        GetComponent<MarioStateController>().SetPowerup(PowerUpType.Damage);
    }
}
