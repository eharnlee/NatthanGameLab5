using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    public int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    public Vector3 startPosition;
    public Animator enemyAnimator;
    public AudioSource stompAudio;
    public GameObject enemyPrefab;

    private bool isDead;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        startPosition = transform.position;
        ComputeVelocity();

        isDead = false;
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - startPosition.x) < maxOffset)
        {// move goomba
            ComputeVelocity();
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

    public void GameRestart()
    {
        if (isDead)
        {
            // reset must be done before "SetActive(true)" because we only want to trigger "reset" for
            // enemies who are dead but still active. If "SetActive(true)" first then trigger "reset", then
            // the enemies who were inactive will be set active, then given an extra "reset", preventing their
            // death animation
            enemyAnimator.SetTrigger("reset");
        }

        this.gameObject.SetActive(true);

        isDead = false;

        transform.localPosition = startPosition;
        moveRight = -1;
    }

    public void stomped()
    {
        isDead = true;
        SuperMarioManager.instance.IncreaseScore(1);
        enemyAnimator.SetTrigger("stomped");
        playStompedSound();
        StartCoroutine(WaitForDeath());
    }

    public void playStompedSound()
    {
        stompAudio.PlayOneShot(stompAudio.clip);
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        if (isDead)
        {
            this.gameObject.SetActive(false);
        }
    }
}