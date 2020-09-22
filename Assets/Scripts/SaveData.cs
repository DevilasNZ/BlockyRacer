using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int gemCount;

    public int selectedCar;

    //settings
    public int difficultySetting;

    //statistics
    public int gamesPlayed;

    //gems
    public int gemsLastGame;
    public int gemsBest;
    public int gemsTotal;
    public int gemsSpent;
    public int averageGems;

    //distance
    public int distanceLastGame;
    public int distanceBest;
    public int distanceTotal;
    public int averageDistance;

    //other
    public int[] gamesInTrafficLevels;
    public int[] carPlayTimes;//used to determine favourite car

    public SaveData(int gemCount, int selectedCar, int difficultySetting, int gamesPlayed, int gemsLastGame, int gemsBest, int gemsTotal, int gemsSpent, int averageGems, int distanceLastGame, int distanceBest, int distanceTotal, int averageDistance, int[] gamesInTrafficLevels, int[] carPlayTimes)
    {
        this.gemCount = gemCount;

        this.selectedCar = selectedCar;

        this.difficultySetting = difficultySetting;

        //statistics
        this.gamesPlayed = gamesPlayed;

        //gems
        this.gemsLastGame = gemsLastGame;
        this.gemsBest = gemsBest;
        this.gemsTotal = gemsTotal;
        this.gemsSpent = gemsSpent;
        this.averageGems = averageGems;

        //distance
        this.distanceLastGame = distanceLastGame;
        this.distanceBest = distanceBest;
        this.distanceTotal = distanceTotal;
        this.averageDistance = averageDistance;

        //other
        this.gamesInTrafficLevels = gamesInTrafficLevels;
        this.carPlayTimes = carPlayTimes;
    }
}
