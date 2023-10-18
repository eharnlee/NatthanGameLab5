using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBumpSound : MonoBehaviour
{
    public AudioSource brickBlockBounceAudio;
    void playBumpSound()
    {
        brickBlockBounceAudio.PlayOneShot(brickBlockBounceAudio.clip);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
