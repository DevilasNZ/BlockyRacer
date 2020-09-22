using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    GameState gameState;

    public float moveSpeed;
    int currentLane = 2;
    bool changingLane = false;
    int moveDirection;

    GameObject PlayerCar;

    public GameObject tileManager;
    TileManager tileManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        PlayerCar = transform.GetChild(1).gameObject;

        tileManagerScript = tileManager.GetComponent<TileManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.controlsLocked) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            moveRight();
        }

        //if the car is changing lane, keep animating it.
        if (changingLane)
        {
            //move in the appropriate direction
            if(moveDirection == -1)
            {
                PlayerCar.transform.position -= new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                PlayerCar.transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
            }

            //has the car overshot the target?, if so, correct it.
            if(moveDirection == -1 && PlayerCar.transform.position.x < tileManagerScript.currentLanes[currentLane])
            {
                PlayerCar.transform.position = new Vector3(tileManagerScript.currentLanes[currentLane], 0.55f, 0);
                changingLane = false;
            }
            else if(moveDirection == 1 && PlayerCar.transform.position.x > tileManagerScript.currentLanes[currentLane])
            {
                PlayerCar.transform.position = new Vector3(tileManagerScript.currentLanes[currentLane], 0.55f, 0);
                changingLane = false;
            }
        }
    }

    public void moveLeft()
    {
        if (gameState.controlsLocked) return;

        if (currentLane > 0)
        {
            currentLane -= 1;
            changingLane = true;
            moveDirection = -1;
        }
    }

    public void moveRight()
    {
        if (gameState.controlsLocked) return;

        if (currentLane < tileManagerScript.currentLanes.Length - 1)
        {
            currentLane += 1;
            changingLane = true;
            moveDirection = 1;
        }
    }
}
