using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    void Awake()
    {
        // subscribe to Game Restart event
        // SuperMarioManager.instance.levelRestart.AddListener(LevelRestart);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelRestart()
    {
        // foreach (Transform child in transform)
        // {
        //     child.GetComponent<EnemyController>().OnLevelRestart();
        // }
    }
}