using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBlockEmpty : MonoBehaviour
{
    public Animator blockAnimator;
    public GameObject brickBlock;
    private AudioSource blockBumpAudio;

    // Start is called before the first frame update
    void Start()
    {
        blockBumpAudio = brickBlock.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            blockAnimator.SetTrigger("hit");
            blockBumpAudio.PlayOneShot(blockBumpAudio.clip);
        }
    }
}
