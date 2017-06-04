using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotsMind : MonoBehaviour {

    public House houseController;
    public GameManager gameMan;
    private PlayerScript Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void BotDecisionMaking(GameObject _currentPosition)
    {
        int freePropertiesInCurrentStreet;
        int currentStreetLength;
        bool willBuy = false;

        if (houseController.checkIfBuyable(_currentPosition))
        {
            //Debug.Log(_currentPosition.name + " , " + _currentPosition.tag);
            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GET STREET LENGTH AND FREE PROPERTIES IN STREET~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
            if (_currentPosition.tag == "Property")
            {
                //Debug.Log("You landed on a property");
                freePropertiesInCurrentStreet = houseController.returnNumberOfFreePropertiesInStreet(_currentPosition);
                currentStreetLength = houseController.returnLengthOfCurrentPropertyStreet(_currentPosition);
                willBuy = houseController.CheckIfPropertyStreetIsFreeOrYours(_currentPosition);
            }
            else
            {
                //Debug.Log("You landed on an EXTRA property");
                freePropertiesInCurrentStreet = houseController.returnNumberOfFreeExtraPropertiesInStreet(_currentPosition);
                currentStreetLength = houseController.returnLengthOfCurrentExtraPropertyStreet(_currentPosition);
                willBuy = houseController.CheckIfExtraPropertyStreetIsFreeOrYours(_currentPosition);
            }
            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~BOT BUYS PROPERTY ON SPECIFIC CONDITIONS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

            if (freePropertiesInCurrentStreet == currentStreetLength)
            {
                houseController.buyProperty();
            }
            if ((freePropertiesInCurrentStreet == 1))
            {
                houseController.buyProperty();
            }
            else if (willBuy)
            {
                houseController.buyProperty();
            }

            /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        }

    } //Bot decides if he'll buy the (Extra)Property
    public void BotBuildingDecisions()
    {
        Player = gameMan.activePlayer.GetComponent<PlayerScript>();
        if (houseController.DoYouOwnAStreet())
        {
            while (Player.Money > 1000)
            {
                houseController.setHouse(houseController.ReturnBestBuildableProperty());
            }
        }



    }

    public void CheckBotsStreets()
    {
        //houseController.ReturnPropertiesInMostExpensiveStreet();
        
    }
}
