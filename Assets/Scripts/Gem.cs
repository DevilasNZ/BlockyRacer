using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        //just delete the gem if it didn't spawn
        if (Random.value > gameState.gemSpawnRate) Destroy(this.gameObject);
    }

    //when the gem hits something, check if it's the player
    void OnTriggerEnter(Collider collider)
    {
        if (!gameState.controlsLocked && collider.gameObject.CompareTag("Player"))
        {
            gameState.addGem();
            Destroy(this.gameObject);
        }
    }
}
