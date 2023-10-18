using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using Unity.VisualScripting;

public class MainMenuMarioController : MonoBehaviour
{
    private float upSpeed = 15;
    private bool onGroundState = true;
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    private GameObject marioBody;
    private Rigidbody2D marioBodyRigidBody;
    private Animator marioAnimator;
    private AudioSource marioJumpAudio;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        marioBody = this.transform.Find("MarioBody").gameObject;
        marioBodyRigidBody = this.GetComponent<Rigidbody2D>();

        marioAnimator = marioBody.GetComponent<Animator>();
        marioAnimator.SetBool("onGround", true);

        marioJumpAudio = this.transform.Find("MarioJumpAudio").GetComponent<AudioSource>();
    }

    void OnSceneLoad()
    {
        // marioBody = this.transform.Find("MarioBody").gameObject;
        // marioBodyRigidBody = this.GetComponent<Rigidbody2D>();

        // marioAnimator = marioBody.GetComponent<Animator>();
        // marioAnimator.SetBool("onGround", true);

        // marioJumpAudio = this.transform.Find("MarioJumpAudio").GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", true);
        }
    }

    public void OnJumpAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Debug.Log("Main Menu Jump was started");
        }
        else if (context.performed)
        {
            // Debug.Log("Main Menu Jump was performed");

            // jump
            if (onGroundState)
            {
                onGroundState = false;
                marioBodyRigidBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
                PlayJumpSound();

                // update animator state
                marioAnimator.SetBool("onGround", false);
            }

        }
        else if (context.canceled)
        {
            // Debug.Log("Jump was cancelled");
        }
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioJumpAudio.PlayOneShot(marioJumpAudio.clip);
    }
}
