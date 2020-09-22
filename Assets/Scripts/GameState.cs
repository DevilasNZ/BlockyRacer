using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;


//Controls various important components of the game; the pause/play state, the score etc

public class GameState : MonoBehaviour
{
    public bool paused = false;
    public bool controlsLocked = true;
    public int gameState = 0;//0=main menu, 1=settings, 2=stats, 3=crashed, 4=gem count, 5=car selector (ABLE TO ADD MORE GAME STATES), 10 = in game

    [Header("Game Menus")]
    public GameObject menuCamera;
    public GameObject chaseCamera;
    public GameObject topValuesUI;
    public GameObject mainMenu;
    public GameObject gameUI;
    public GameObject settingsMenu;
    public GameObject statsMenu;
    public GameObject crashedMenu;
    public GameObject gemMenu;
    public GameObject carSelector;

    [Header("Game Start Variables")]
    public GameObject playerCar;
    public GameObject trafficManager;
    TrafficManager trafficManagerScript;

    bool gameStarting = false;
    public float startCarMoveSpeed = 100;
    public float musicVolumeIncreaseSpeed = 0.2f;
    AudioSource gameStartMusicSource;

    [Header("Game Variables")]
    public int carSpeedInMetersPerSecond = 50;
    public GameObject DistanceTextObject;
    Text DistanceText;
    float distanceTravelled = 0;

    [Header("Game Settings")]
    public int gameDifficulty = 0;//0=easy, 1=medium, 2=hard
    public float gemSpawnRate = 0.2f;
    float audioLevel = 1;//volume multiplier for game sounds.

    public float gemBank = 0;
    float gemCountThisGame = 0;
    Text topMenuGemCountText;

    [Header("Car Selector Variables")]
    public GameObject[] cars;
    public int selectedCar = 0;
    public GameObject[] carsInScene;
    //note that this stuff should probably be put into its own object class. That way it would be easier to assign multiple colours etc.
    public bool[] carOwned = { true, false, false, false, false, false, false, false, false, false, false, false };
    string[] carName = { "Compact", "Hatchback", "Jeep", "Van", "Taxi", "Bus", "Truck", "Police", "Fire", "Limo", "RaceCar", "Tank" };
    static int[] carPrice = { 0, 20, 50, 50, 80, 100, 100, 120, 120, 120, 150, 250 };

    [Header("Gem Counter Variables")]
    public GameObject difficultySlider;

    [Header("Gem Counter Variables")]
    public GameObject GemsCounterTextObject;
    Text GemCounterText;
    public float endGameGemTransitionSpeed = 10f;
    bool transitioningGems = false;
    public GameObject gemFlowParticles;

    //statistics
    public int gamesPlayed = 0;

    //gems
    public int gemsLastGame = 0;
    public int gemsBest = 0;
    public int averageGems = 0;
    public int gemsTotal = 0;
    public int gemsSpent = 0;

    //distance
    public int distanceLastGame = 0;
    public int distanceBest = 0;
    public int averageDistance = 0;
    public int distanceTotal = 0;


    //other
    public int[] gamesInTrafficLevels = { 0, 0, 0 };
    int[] carPlayTimes = { 0,0,0,0,0,0,0,0,0,0,0,0 };//used to determine favourite car
    public string favouriteCar = "compact";

    // Start is called before the first frame update
    void Start()
    {
        trafficManagerScript = trafficManager.GetComponent<TrafficManager>();

        topMenuGemCountText = topValuesUI.transform.GetChild(0).gameObject.GetComponent<Text>();
        GemCounterText = GemsCounterTextObject.GetComponent<Text>();
        DistanceText = DistanceTextObject.GetComponent<Text>();

        gameStartMusicSource = gameUI.GetComponent<AudioSource>();

        if (loadGame() != 0) Debug.Log("error loading save");
        topMenuGemCountText.text = gemBank.ToString();

        //set the setting UI values up
        difficultySlider.GetComponent<Slider>().value = gameDifficulty;
    }

    //-------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        //manage the game start
        if (gameStarting)
        {
            //slowly increase game music volume
            gameStartMusicSource.volume += musicVolumeIncreaseSpeed * Time.deltaTime;
            gameStartMusicSource.volume *= audioLevel;

            //bring in the car, then disable the Menu Camera
            playerCar.transform.position += new Vector3(0, 0, startCarMoveSpeed) * Time.deltaTime;

            //check if it reached the target
            if(playerCar.transform.position.z > 0)
            {
                //switch the cameras and unlock controls
                playerCar.transform.position = new Vector3(playerCar.transform.position.x, playerCar.transform.position.y, 0);
                chaseCamera.SetActive(true);
                menuCamera.SetActive(false);
                controlsLocked = false;

                gameStarting = false;
            }
        }

        //-------------------------------------------------------------------------
        //update the counters when in the game.
        if (gameState == 10)
        {
            topMenuGemCountText.text = gemCountThisGame.ToString();
            distanceTravelled += carSpeedInMetersPerSecond * Time.deltaTime;
            DistanceText.text = string.Format("{0:#,###0}m", distanceTravelled);
        }

        //-------------------------------------------------------------------------
        //animate the gems moving into the total gem count at the end of the game.
        if (transitioningGems)
        {
            float gemsTransitioning = endGameGemTransitionSpeed * Time.deltaTime;

            //make sure too many gems arent added to the total count
            if (gemCountThisGame < gemsTransitioning)
            {
                gemBank+= gemCountThisGame;
                gemCountThisGame = 0;
            }
            else
            {
                gemBank+= gemsTransitioning;
                gemCountThisGame -= gemsTransitioning;
            }

            //if there are no more gems to move over, stop playing this animation.
            if(gemCountThisGame < 1)
            {
                transitioningGems = false;
                topValuesUI.SetActive(false);
                gemFlowParticles.GetComponent<ParticleSystem>().Stop();

                //pop the text
                GemsCounterTextObject.GetComponent<TextPop>().pop();
            }

            //update the text components
            GemCounterText.text = Mathf.Round(gemBank).ToString();
            topMenuGemCountText.text =  Mathf.Round(gemCountThisGame).ToString();
        }
    }

    //-------------------------------------------------------------------------
    //change the game state

    public void changeGameState(int newState)
    {
        gameState = newState;

        switch (newState)
        {
            case 0://the main menu
                paused = false;
                controlsLocked = true;
                //reset the car position
                playerCar.transform.position = new Vector3(0, 0, -50);

                menuCamera.SetActive(true);
                chaseCamera.SetActive(false);
                topValuesUI.SetActive(true);
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
                statsMenu.SetActive(false);
                crashedMenu.SetActive(false);
                gemMenu.SetActive(false);
                carSelector.SetActive(false);

                topMenuGemCountText.text = Mathf.Round(gemBank).ToString();

                saveGame();//TODO: the game is not saving all of the gems gained; is the gemCounter not giving all of the gems to the total before saving?

                break;

            case 1://the settings menu
                paused = true;
                controlsLocked = true;

                menuCamera.SetActive(true);
                chaseCamera.SetActive(false);
                topValuesUI.SetActive(false);
                settingsMenu.SetActive(true);
                statsMenu.SetActive(false);
                crashedMenu.SetActive(false);
                gemMenu.SetActive(false);

                break;

            case 2://the stats menu
                paused = true;
                controlsLocked = true;

                menuCamera.SetActive(true);
                chaseCamera.SetActive(false);
                topValuesUI.SetActive(false);
                settingsMenu.SetActive(false);
                statsMenu.SetActive(true);
                crashedMenu.SetActive(false);
                gemMenu.SetActive(false);

                topMenuGemCountText.text = gemBank.ToString();

                break;

            case 3://run when player crashes
                paused = true;
                controlsLocked = true;

                menuCamera.SetActive(false);
                chaseCamera.SetActive(true);
                topValuesUI.SetActive(true);
                mainMenu.SetActive(false);
                gameUI.SetActive(false);
                settingsMenu.SetActive(false);
                statsMenu.SetActive(false);
                crashedMenu.SetActive(true);
                gemMenu.SetActive(false);

                break;

            case 4://the gem counter menu
                paused = true;
                controlsLocked = true;

                menuCamera.SetActive(false);
                chaseCamera.SetActive(true);
                topValuesUI.SetActive(true);
                mainMenu.SetActive(false);
                settingsMenu.SetActive(false);
                statsMenu.SetActive(false);
                crashedMenu.SetActive(false);
                gemMenu.SetActive(true);

                gemCount();

                break;

            case 5://car selector
                paused = true;
                controlsLocked = true;

                menuCamera.SetActive(false);
                mainMenu.SetActive(false);
                carSelector.SetActive(true);

                break;

            case 10://the game itself
                paused = false;
                controlsLocked = true;

                menuCamera.SetActive(true);
                chaseCamera.SetActive(false);
                topValuesUI.SetActive(true);//the game should use this same UI to count how many gems have been collected the current game.
                mainMenu.SetActive(false);
                gameUI.SetActive(true);
                settingsMenu.SetActive(false);
                statsMenu.SetActive(false);
                crashedMenu.SetActive(false);
                gemMenu.SetActive(false);

                startGame();

                break;
        }
    }

    //-------------------------------------------------------------------------
    //change the game difficulty
    public void changeDifficulty()//TODO: change the gem spawn rate based on the game difficulty. make the default difficulty medium, have this show when the settings are opened.
    {
        int newDifficulty = (int) difficultySlider.GetComponent<Slider>().value;
        Debug.Log("changed difficulty: " + newDifficulty);

        gameDifficulty = newDifficulty;
        switch (newDifficulty)
        {
            case 0://easy
                trafficManagerScript.setTimeBetweenSpawns(0.5f);
                gemSpawnRate = 0.2f;

                break;

            case 1://medium
                trafficManagerScript.setTimeBetweenSpawns(0.35f);
                gemSpawnRate = 0.4f;

                break;

            case 2://hard
                trafficManagerScript.setTimeBetweenSpawns(0.25f);
                gemSpawnRate = 0.6f;

                break;
        }
    }

    //-------------------------------------------------------------------------
    //Start the game
    void startGame()
    {
        //clear the traffic
        trafficManagerScript.stopTraffic(5f);

        //start the game music off at 0 volume, and slowly increase it
        gameStartMusicSource.volume = 0;

        gameStarting = true;
    }

    //-------------------------------------------------------------------------
    //add a gem to the current game total
    public void addGem() { gemCountThisGame += 1; }

    //-------------------------------------------------------------------------
    //run the end game menu
    void gemCount()
    {
        calculateStatistics();

        //set the top menu gem count to the total value before this game.
        topMenuGemCountText.text = gemBank.ToString();

        //set the value in the center of the screen to the number of gems gained this game.
        GemCounterText.text = gemCountThisGame.ToString();
        GemsCounterTextObject.SetActive(true);

        //start the animation for adding the gems to the total gem count.
        transitioningGems = true;
        gemFlowParticles.GetComponent<ParticleSystem>().Play();
    }

    //-------------------------------------------------------------------------
    //run when the player continues from the gem count menu.
    public void exitGemCount()
    {
        transitioningGems = false;
        gemBank+= gemCountThisGame +1;
        gemCountThisGame = 0;

        distanceTravelled = 0;

        changeGameState(0);
    }

    //-------------------------------------------------------------------------
    //save the game state out to a file.
    void saveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/blockyRacerSave.brs";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData save = new SaveData((int)gemBank, selectedCar, gameDifficulty, gamesPlayed, gemsLastGame, gemsBest, gemsTotal, gemsSpent, averageGems, distanceLastGame, distanceBest, distanceTotal, averageDistance, gamesInTrafficLevels, carPlayTimes);

        formatter.Serialize(stream, save);
        stream.Close();
    }

    int loadGame()
    {
        string path = Application.persistentDataPath + "/blockyRacerSave.brs";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData save = formatter.Deserialize(stream) as SaveData;

            //load in the selected car
            selectedCar = save.selectedCar;
            carsInScene[selectedCar].SetActive(true);

            gemBank= (float)save.gemCount;

            gameDifficulty = save.difficultySetting;

            //statistics
            gamesPlayed = save.gamesPlayed;

            //gems
            gemsLastGame = save.gemsLastGame;
            gemsBest = save.gemsBest;
            averageGems = save.averageGems;
            gemsTotal = save.gemsTotal;
            gemsSpent = save.gemsSpent;

            //distance
            distanceLastGame = save.distanceLastGame;
            distanceBest = save.distanceBest;
            averageDistance = save.averageDistance;
            distanceTotal = save.distanceTotal;


            //other
            gamesInTrafficLevels = save.gamesInTrafficLevels;
            carPlayTimes = save.carPlayTimes;

            return 0;
        }

        //if the path doesnt exist, set defaults
        //load in the selected car
        selectedCar = 0;
        carsInScene[selectedCar].SetActive(true);

        gemBank = 0;

        gameDifficulty = 1;

        //statistics
        gamesPlayed = 0;

        //gems
        gemsLastGame = 0;
        gemsBest = 0;
        averageGems = 0;
        gemsTotal = 0;
        gemsSpent = 0;

        //distance
        distanceLastGame = 0;
        distanceBest = 0;
        averageDistance = 0;
        distanceTotal = 0;

        //return 1 if there's an error
        return 1;
    }

    //-------------------------------------------------------------------------
    //calculate game statistics
    void calculateStatistics()
    {
        gamesPlayed ++;

        gemsLastGame = (int)gemCountThisGame;
        if(gemsLastGame > gemsBest) gemsBest = gemsLastGame;
        gemsTotal = (int)gemBank + gemsSpent;
        averageGems = gemsTotal / gamesPlayed;

        distanceLastGame = (int)distanceTravelled;
        if (distanceLastGame > distanceBest) distanceBest = distanceLastGame;
        distanceTotal += distanceLastGame;
        averageDistance = distanceTotal / gamesPlayed;

        gamesInTrafficLevels[gameDifficulty] ++;
        carPlayTimes[selectedCar] ++;
        favouriteCar = carName[carPlayTimes.ToList().IndexOf(carPlayTimes.Max())];
    }
}
