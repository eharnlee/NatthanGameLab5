using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator;
    public AudioSource stompAudio;
    public IntGameEvent scoreIncrease;

    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private Vector3 startPosition;
    private bool isAlive;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        startPosition = transform.position;
        ComputeVelocity();

        isAlive = true;
    }

    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - startPosition.x) < maxOffset)
        {// move goomba
            // ComputeVelocity();
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    public void OnLevelRestart()
    {
        isAlive = true;

        this.gameObject.SetActive(true);
        this.GetComponent<Collider2D>().enabled = true;

        enemyAnimator.Play("Goomba Idle");

        transform.position = startPosition;
        moveRight = -1;
        ComputeVelocity();
    }

    public void stomped()
    {
        StartCoroutine(StompedCoroutine());
    }

    IEnumerator StompedCoroutine()
    {
        isAlive = false;

        this.GetComponent<Collider2D>().enabled = false;

        playStompedSound();
        enemyAnimator.SetTrigger("stomped");

        // SuperMarioManager.instance.IncreaseScore(1);
        scoreIncrease.Raise(5);

        yield return new WaitForSecondsRealtime(0.3f);

        // if the restart button has been pressed, then goomba should be alive and remain active
        if (!isAlive)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void playStompedSound()
    {
        stompAudio.PlayOneShot(stompAudio.clip);
    }
}