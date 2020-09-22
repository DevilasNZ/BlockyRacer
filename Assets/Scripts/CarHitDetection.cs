using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//basic code to detect if the player hit a car.
public class CarHitDetection : MonoBehaviour
{
    GameState gameState;

    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();
    }

    //when the car hits something, check if it's the player
    void OnTriggerEnter(Collider collider)
    {
        if (!gameState.controlsLocked && collider.gameObject.CompareTag("Player")) gameState.changeGameState(3);
    }
}
