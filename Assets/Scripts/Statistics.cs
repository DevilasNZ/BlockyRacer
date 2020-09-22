using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    GameState gameState;

    public GameObject gamesPlayedObject;

    //gems
    public GameObject gemsLastGameObject;
    public GameObject gemsBestObject;
    public GameObject averageGemsObject;
    public GameObject gemsTotalObject;
    public GameObject gemsSpentObject;

    //distance
    public GameObject distanceLastGameObject;
    public GameObject distanceBestObject;
    public GameObject averageDistanceObject;
    public GameObject distanceTotalObject;


    //other
    public GameObject lowTrafficGamesObject;
    public GameObject mediumTrafficGamesObject;
    public GameObject highTrafficGamesObject;

    public GameObject favouriteCarObject;

    Text gamesPlayedText;

    //gems
    Text gemsLastGameText;
    Text gemsBestText;
    Text averageGemsText;
    Text gemsTotalText;
    Text gemsSpentText;

    //distance
    Text distanceLastGameText;
    Text distanceBestText;
    Text averageDistanceText;
    Text distanceTotalText;


    //other
    Text lowTrafficGamesText;
    Text mediumTrafficGamesText;
    Text highTrafficGamesText;

    Text favouriteCarText;

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        gamesPlayedText = gamesPlayedObject.GetComponent<Text>();

        //gems
        gemsLastGameText = gemsLastGameObject.GetComponent<Text>();
        gemsBestText = gemsBestObject.GetComponent<Text>();
        averageGemsText = averageGemsObject.GetComponent<Text>();
        gemsTotalText = gemsTotalObject.GetComponent<Text>();
        gemsSpentText = gemsSpentObject.GetComponent<Text>();

        //distance
        distanceLastGameText = distanceLastGameObject.GetComponent<Text>();
        distanceBestText = distanceBestObject.GetComponent<Text>();
        averageDistanceText = averageDistanceObject.GetComponent<Text>();
        distanceTotalText = distanceTotalObject.GetComponent<Text>();


        //other
        lowTrafficGamesText = lowTrafficGamesObject.GetComponent<Text>();
        mediumTrafficGamesText = mediumTrafficGamesObject.GetComponent<Text>();
        highTrafficGamesText = highTrafficGamesObject.GetComponent<Text>();

        favouriteCarText = favouriteCarObject.GetComponent<Text>();

        setValues();
    }

    //set the text values
    public void setValues()
    {
        //check if Start() has run before executing this.
        if (favouriteCarText != favouriteCarObject.GetComponent<Text>()) return;

        gamesPlayedText.text = gameState.gamesPlayed.ToString();

        //gems
        gemsLastGameText.text = gameState.gemsLastGame.ToString();
        gemsBestText.text = gameState.gemsBest.ToString();
        averageGemsText.text = gameState.averageGems.ToString();
        gemsTotalText.text = gameState.gemsTotal.ToString();
        gemsSpentText.text = gameState.gemsSpent.ToString();

        //distance
        distanceLastGameText.text = string.Format("{0:#,###0}m", gameState.distanceLastGame);
        distanceBestText.text = string.Format("{0:#,###0}m", gameState.distanceBest);
        averageDistanceText.text = string.Format("{0:#,###0}m", gameState.averageDistance);
        distanceTotalText.text = string.Format("{0:#,###0}m", gameState.distanceTotal);


        //other
        lowTrafficGamesText.text = gameState.gamesInTrafficLevels[0].ToString();
        mediumTrafficGamesText.text = gameState.gamesInTrafficLevels[1].ToString();
        highTrafficGamesText.text = gameState.gamesInTrafficLevels[2].ToString();

        favouriteCarText.text = gameState.favouriteCar;
    }
}
