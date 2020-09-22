using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficManager : MonoBehaviour
{
    GameState gameState;

    public GameObject[] commonTrafficVehicles;
    public GameObject[] uncommonTrafficVehicles;
    public GameObject[] rareTrafficVehicles;
    public int trafficVolume = 10;
    public int trafficSide = 0;//0=left, 1=right
    public float onSideTrafficMinSpeed = 80;
    public float onSideTrafficMaxSpeed = 90;
    public float offSideTrafficMinSpeed = 140;
    public float offSideTrafficMaxSpeed = 150;

    public float timeBetweenSpawns = 0.5f;
    float respawnTimer;

    GameObject[] trafficObjects;
    int[] trafficDirections;
    float[] trafficSpeeds;

    bool trafficStopped = false;
    float trafficStoppedTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        //generate all the cars
        trafficObjects = new GameObject[trafficVolume];
        trafficDirections = new int[trafficVolume];
        trafficSpeeds = new float[trafficVolume];

        for (int i = 0; i < trafficVolume; i++)
        {
            trafficObjects[i] = Instantiate(commonTrafficVehicles[(int)Random.value * commonTrafficVehicles.Length], transform);

            if (Random.value > 0.5) trafficDirections[i] = 0;
            else trafficDirections[i] = 1;

            //the speed of traffic should be different depending on which side of the road it is on.
            if (trafficSide == 0 && trafficDirections[i] == 0 || trafficSide == 1 && trafficDirections[i] == 1)
            {
                trafficSpeeds[i] = Random.Range(onSideTrafficMinSpeed, onSideTrafficMaxSpeed);
            }
            else
            {
                trafficSpeeds[i] = Random.Range(offSideTrafficMinSpeed, offSideTrafficMaxSpeed);
            }

            //set the initial positions of the traffic.
            if (trafficDirections[i] == 0)
            {
                trafficObjects[i].transform.position = new Vector3(-6, 0.55f, Random.Range(50, 900));
                trafficObjects[i].transform.Rotate(0, 90f, 0);
            }
            else if (trafficDirections[i] == 1)
            {
                trafficObjects[i].transform.position = new Vector3(6, 0.55f, Random.Range(50, 900));
                trafficObjects[i].transform.Rotate(0, -90f, 0);
            }
        }

    }

    //-------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (gameState.paused) return;
        //move the cars
        for (int i = 0; i < trafficVolume; i++)
        {
            trafficObjects[i].transform.position -= new Vector3(0, 0, trafficSpeeds[i]) * Time.deltaTime;

            //respawn any cars as needed.
            if (trafficObjects[i].transform.position.z < -20 && respawnTimer < 0 && !trafficStopped)
            {
                respawnTimer = timeBetweenSpawns;
                respawnCar(i);
            }
        }

        //run the respawn timer.
        respawnTimer -= Time.deltaTime;

        //run the traffic stopped timer
        if (trafficStopped)
        {
            trafficStoppedTimer -= Time.deltaTime;

            if (trafficStoppedTimer < 0) trafficStopped = false;
        }
    }

    //-------------------------------------------------------------------------
    //spawn a car
    void respawnCar(int carIndex)
    {
        //-------------------------------------------------------------------------
        //change the vehicle
        int vehicleChoice;
        if (Random.value < 0.2)
        {
            if (Random.value < 0.2)
            {
                //rare
                vehicleChoice = (int)Random.Range(0, rareTrafficVehicles.Length);
                GameObject.Destroy(trafficObjects[carIndex]);
                trafficObjects[carIndex] = Instantiate(rareTrafficVehicles[vehicleChoice], transform);
            }
            else
            {
                //uncommon
                vehicleChoice = (int)Random.Range(0, uncommonTrafficVehicles.Length);
                GameObject.Destroy(trafficObjects[carIndex]);
                trafficObjects[carIndex] = Instantiate(uncommonTrafficVehicles[vehicleChoice], transform);
            }
        }else
        {
            //common
            vehicleChoice = (int)Random.Range(0, commonTrafficVehicles.Length);
            GameObject.Destroy(trafficObjects[carIndex]);
            trafficObjects[carIndex] = Instantiate(commonTrafficVehicles[vehicleChoice], transform);
        }
        //-------------------------------------------------------------------------

        if (Random.value > 0.5) trafficDirections[carIndex] = 0;
        else trafficDirections[carIndex] = 1;

        //set the speed and rotation depending on the side the vehicle is on
        if (trafficSide == 0 && trafficDirections[carIndex] == 0 || trafficSide == 1 && trafficDirections[carIndex] == 1)
        {
            //same side traffic
            trafficSpeeds[carIndex] = Random.Range(onSideTrafficMinSpeed, onSideTrafficMaxSpeed);
            trafficObjects[carIndex].transform.Rotate(0, 90f, 0);
        }
        else
        {
            //other side traffic
            trafficSpeeds[carIndex] = Random.Range(offSideTrafficMinSpeed, offSideTrafficMaxSpeed);
            trafficObjects[carIndex].transform.Rotate(0, 270f, 0);
        }

        //set the lane TODO: update this in future to accomodate for all roads. it will need to get the lane positions of the road the car is spawning on. note that there may be issues with the speed of the car on the road tile; need to ensure the car stays of the same road tile to avoid issues.
        float lanePos;
        if (trafficDirections[carIndex] == 0)
        {
            if (Random.value > 0.5) lanePos = -6f;
            else lanePos = -17.8f;
            trafficObjects[carIndex].transform.position = new Vector3(lanePos, 1.27f, 800);
        }
        else if (trafficDirections[carIndex] == 1)
        {
            if (Random.value > 0.5) lanePos = 6f;
            else lanePos = 17.8f;
            trafficObjects[carIndex].transform.position = new Vector3(lanePos, 1.27f, 800);
        }
    }

    //-------------------------------------------------------------------------
    //change the traffic direction
    public void switchTrafficDirection()
    {
        if (trafficSide == 0) trafficSide = 1;
        else trafficSide = 0;
    }

    //-------------------------------------------------------------------------
    //stop the traffic for a set amount of time.
    public void stopTraffic(float time)
    {
        trafficStopped = true;
        trafficStoppedTimer = time;
    }

    //-------------------------------------------------------------------------
    //getters and setters
    public void setTimeBetweenSpawns(float newTime) { timeBetweenSpawns = newTime; }

}
