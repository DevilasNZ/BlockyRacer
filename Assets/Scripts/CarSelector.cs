using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script manages the picking of a car on the car selector screen
public class CarSelector : MonoBehaviour
{
    GameState gameState;

    public GameObject[] carsInScene;
    public GameObject[] driveCarsInScene;
    int carShown = 0;

    //-------------------------------------------------------------------------
    //note that this stuff should probably be put into its own object class. That way it would be easier to assign multiple colours etc.
    //the cars could also just be stored in GameState for this script to get on Start()
    bool[] carOwned = { true, true, false, false, false, false, false, false, false, false, false, false };
    string[] carName = { "Compact", "Hatchback", "Jeep", "Van", "Taxi", "Bus", "Truck", "Police", "Fire", "Limo", "RaceCar", "Tank" };
    int[] carPrice = { 0, 20, 50, 50, 80, 100, 100, 120, 120, 120, 150, 250 };
    //-------------------------------------------------------------------------

    [Header("UI Elements")]
    public GameObject carNameTextObject;
    public GameObject carPriceTextObject;
    Text carNameText;
    Text carPriceText;

    public GameObject purchaseButton;
    public GameObject purchasePlaceholder;//shown if current car is unaffordable
    public GameObject selectButton;
    public GameObject selectedPlaceholder;//shown on the currently selected car

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameObject.FindGameObjectsWithTag("PlayerScripts")[0].GetComponent<GameState>();

        carShown = gameState.selectedCar;
        carsInScene[carShown].SetActive(true);

        carNameText = carNameTextObject.GetComponent<Text>();
        carPriceText = carPriceTextObject.GetComponent<Text>();

        updateSelectorUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-------------------------------------------------------------------------
    //select a car
    public void selectCar()
    {
        //hide the old car
        driveCarsInScene[gameState.selectedCar].SetActive(false);

        //show the new one
        gameState.selectedCar = carShown;
        driveCarsInScene[gameState.selectedCar].SetActive(true);

        updateSelectorUI();
    }

    //-------------------------------------------------------------------------
    //purchase a car TODO: show some kind of sound and visual response to purchase
    public void purchaseCar()
    {
        carOwned[carShown] = true;
        gameState.carOwned[carShown] = true;

        gameState.gemBank -= carPrice[carShown];
        gameState.gemsSpent += carPrice[carShown];

        selectCar();
    }

    //-------------------------------------------------------------------------
    //click left and right to select a car
    public void nextCar()
    {
        //if the last car is currently selected, cycle around to the first
        if(carShown == carsInScene.Length - 1)
        {
            //hide the current car
            carsInScene[carShown].SetActive(false);

            //show the new car
            carShown = 0;
            carsInScene[carShown].SetActive(true);
        }
        else
        {
            //hide the current car
            carsInScene[carShown].SetActive(false);

            //show the new car
            carShown ++;
            carsInScene[carShown].SetActive(true);
        }

        updateSelectorUI();
    }

    public void previousCar()
    {
        //if the first car is currently selected, cycle around to the last
        if (carShown == 0)
        {
            //hide the current car
            carsInScene[carShown].SetActive(false);

            //show the new car
            carShown = carsInScene.Length - 1;
            carsInScene[carShown].SetActive(true);
        }
        else
        {
            //hide the current car
            carsInScene[carShown].SetActive(false);

            //show the new car
            carShown--;
            carsInScene[carShown].SetActive(true);
        }

        updateSelectorUI();
    }

    //-------------------------------------------------------------------------
    //update the UI text when a car is changed
    void updateSelectorUI()
    {
        //update the labels
        carNameText.text = carName[carShown];
        carPriceText.text = carPrice[carShown].ToString();
        carPriceTextObject.SetActive(!carOwned[carShown]);

        //update the button at the bottom
        if (carShown == gameState.selectedCar)//if the car is selected
        {
            purchaseButton.SetActive(false);
            purchasePlaceholder.SetActive(false);
            selectButton.SetActive(false);
            selectedPlaceholder.SetActive(true);
        }
        else if (carOwned[carShown])//if the car is owned but not selected
        {
            purchaseButton.SetActive(false);
            purchasePlaceholder.SetActive(false);
            selectButton.SetActive(true);
            selectedPlaceholder.SetActive(false);
        }
        else//if the car is not owned
        {
            //if the car is affordable
            if(gameState.gemBank >= carPrice[carShown])
            {
                purchaseButton.SetActive(true);
                purchasePlaceholder.SetActive(false);
                selectButton.SetActive(false);
                selectedPlaceholder.SetActive(false);
            }
            else//if the car is unaffordable
            {
                purchaseButton.SetActive(false);
                purchasePlaceholder.SetActive(true);
                selectButton.SetActive(false);
                selectedPlaceholder.SetActive(false);
            }
        }
    }
}
