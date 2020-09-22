using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    GameState gameState;

    public RoadChunk[] roadChunks;
    public int tileCount;
    public int tileSize;
    public float moveSpeed;
    public float despawnZ = -100;

    RoadChunk[] gameTiles;
    GameObject[] gameTileObjects;

    public float[] currentLanes;//used for player controls to determine where the car can go

    //-------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        gameTiles = new RoadChunk[tileCount];
        gameTileObjects = new GameObject[tileCount];
        //spawn all the game tiles.
        for(int i = 0; i < tileCount; i++)
        {
            gameTiles[i] = roadChunks[selectNextRoad()];
            gameTileObjects[i] = Instantiate(gameTiles[i].roadPrefab, transform);
            gameTileObjects[i].transform.position = new Vector3(0,0,i * tileSize);
        };
    }

    //-------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        if (gameState.paused) return;

        //move all the game tiles
        //NOTE: this is at a constant rate for now, but later the car should determine it(crashes, road surface etc)
        for (int i = 0; i < tileCount; i++)
        {
            gameTileObjects[i].transform.position -= new Vector3(0, 0, moveSpeed * Time.deltaTime);
        }

        //check if a game tile needs to respawn.
        for (int i = 0; i < tileCount; i++)
        {
            if (gameTileObjects[i].transform.position.z < despawnZ)
            {
                respawnRoad(i);
            }
        }

        //determine the current lane that the player is in.
        for (int i = 0; i < tileCount; i++)
        {
            if(Mathf.Abs(gameTileObjects[0].transform.position.z) < tileSize / 2)
            {
                currentLanes = gameTiles[i].lanePositions;
            }
        }
    }

    //-------------------------------------------------------------------------
    //select the next road chunk to spawn.
    //TODO: in future, this should select the next road based on what roads can come after the previous road chunk.
    int selectNextRoad()
    {
        return (int)(Random.value * roadChunks.Length);
    }

    //-------------------------------------------------------------------------
    //respawn a road piece
    void respawnRoad(int roadIndex)
    {
        //replace the road with a new one. This will also respawn item drops on the road.
        GameObject.Destroy(gameTileObjects[roadIndex]);

        gameTiles[roadIndex] = roadChunks[selectNextRoad()];
        gameTileObjects[roadIndex] = Instantiate(gameTiles[roadIndex].roadPrefab, transform);

        //spawn the tile at the back.
        if (roadIndex == 0)
        {
            gameTileObjects[0].transform.position = gameTileObjects[tileCount - 1].transform.position + new Vector3(0, 0, tileSize);
        }
        else
        {
            gameTileObjects[roadIndex].transform.position = gameTileObjects[roadIndex - 1].transform.position + new Vector3(0, 0, tileSize);
        }
    }
}
