using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {
    private enum cardTypes
    {
        Pay,
        Receive,
        Move,
        Bonus,
    }

    private class Card
    {
        public string Name;
        public string Description;
        public cardTypes cardType;
        public Card() { }
    }

    private class PayCard:Card
    {
        public int amountToPay;
        public PayCard(string _name, string _description, int _amountToPay)
        {
            Name = _name;
            Description = _description;
            amountToPay = _amountToPay;
            cardType = cardTypes.Pay;
        }
    }
    private class ReceiveCard: Card
    {
        public int amountToReceive;
        public ReceiveCard(string _name, string _description, int _amountToReceive)
        {
            Name = _name;
            Description = _description;
            cardType = cardTypes.Receive;
            amountToReceive = _amountToReceive;
        }
    }
    private class MoveCard: Card
    {
        public GameObject waypointToMoveTo;
        public MoveCard(string _name, string _description, GameObject _waypointToMoveTo)
        {
            Name = _name;
            Description = _description;
            cardType = cardTypes.Move;
            waypointToMoveTo = _waypointToMoveTo;
        }
    }
    private class BonusCard: Card
    {
        public string Bonus;
        public BonusCard(string _name, string _destription, string _bonus)
        {
            Name = _name;
            Description = _destription;
            cardType = cardTypes.Bonus;
            Bonus = _bonus;
        }
    }

    public GameObject CardPanel;
    public Text cardTitel, cardText, priceText;

    private List<PayCard> ChancePayCardList = new List<PayCard>();
    private List<ReceiveCard> ChanceReceiveCardList = new List<ReceiveCard>();
    private List<MoveCard> ChanceMoveCardList = new List<MoveCard>();
    private List<BonusCard> ChanceBonusCardList = new List<BonusCard>();

    private List<PayCard> CCPayCardList = new List<PayCard>();
    private List<ReceiveCard> CCReceiveCardList = new List<ReceiveCard>();
    private List<MoveCard> CCMoveCardList = new List<MoveCard>();

    public GameManager gameMan;
    public House houseMan;
    private UIManager _UIManager;

    public GameObject CCPosition1, CCStartPosition;
    public GameObject ChancePosition1, ChanceStartPosition, ChancePosition2, ChancePosition3, ChancePosition4;

    private void Start()
    {
        initiateChanceCards();
        initiateCommunityChestCards();
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    public void initiateChanceCards()
    {
        ChancePayCardList.Add(new PayCard("Kans", "Betaal schoolgeld", 150));
        ChancePayCardList.Add(new PayCard("Kans", "Boete voor te snel rijden", 15));
        ChancePayCardList.Add(new PayCard("Kans", "Opgebracht wegens dronkenschap", 20));
        

        ChanceReceiveCardList.Add(new ReceiveCard("Kans", "U won een wedstrijd", 150));
        ChanceReceiveCardList.Add(new ReceiveCard("Kans", "De bank heeft een fout gemaakt en is u geld schuldig", 50));
        ChanceReceiveCardList.Add(new ReceiveCard("Kans", "U heeft een kruiswoordpuzzel gewonnen", 100));

        ChanceMoveCardList.Add(new MoveCard("Kans", "Ga verder naar " + ChancePosition1.name, ChancePosition1));
        ChanceMoveCardList.Add(new MoveCard("Kans", "Ga verder naar " + ChanceStartPosition.name, ChanceStartPosition));
        ChanceMoveCardList.Add(new MoveCard("Kans", "Ga verder naar " + ChancePosition2.name, ChancePosition2));
        ChanceMoveCardList.Add(new MoveCard("Kans", "Ga verder naar " + ChancePosition3.name, ChancePosition3));
        ChanceMoveCardList.Add(new MoveCard("Kans", "Ga verder naar " + ChancePosition4.name, ChancePosition4));
    }
    public void initiateCommunityChestCards()
    {
        CCPayCardList.Add(new PayCard("Algemeen fonds", "Betaal het hospitaal", 100));
        CCPayCardList.Add(new PayCard("Algemeen fonds", "Betaal uw verzekeringspremie", 50));
        CCPayCardList.Add(new PayCard("Algemeen fonds", "Betaal uw doktersrekening", 50));

        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds", "U trekt geld terug van de verzekering", 100));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","Terugbetaling inkomstenbelasting",20));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","Een vergissing van de bank in uw voordeel",200));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","U heeft de tweede prijs in een schoonheidswedstrijd gewonnen",10));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","U ontvangt rente van uw aandelen",25));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","U verkocht enkele van uw aandelen",50));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","U erft geld", 100));
        CCReceiveCardList.Add(new ReceiveCard("Algemeen fonds","U bent jarig en ontvangt een cadeau van je ouders",50));

        CCMoveCardList.Add(new MoveCard("Algemeen fonds", "Ga verder naar " + CCPosition1.name, CCPosition1));
        CCMoveCardList.Add(new MoveCard("Algemeen fonds", "Ga verder naar " + CCStartPosition.name, CCStartPosition));
    }
    public void pullRandomChanceCard(GameObject _curPlayer)
    {

        int decideCardType = Random.Range(0, 3);
        int pullRandomCard;
        switch (decideCardType)
        {
            case 0:  //pull a pay chance card
                ChancePayCardList.Add(new PayCard("Kans", "Herstel uw huizen & hotels", (houseMan.checkHousesOnProperty() * 25) + (houseMan.checkHotelsOnProperty() * 100)));
                ChancePayCardList.Add(new PayCard("Kans", "U wordt aangeslagen voor straatgeld", (houseMan.checkHousesOnProperty() * 40) + (houseMan.checkHotelsOnProperty() * 115)));

                pullRandomCard = Random.Range(0, ChancePayCardList.Count);

                Debug.Log(_curPlayer + " Pull a Chance Pay card");
                Debug.Log("Your card is:\n" + ChancePayCardList[pullRandomCard].Description + "\n you pay: " + ChancePayCardList[pullRandomCard].amountToPay);

                _curPlayer.GetComponent<PlayerScript>().executePayment(ChancePayCardList[pullRandomCard].amountToPay);

                gameMan.bonusMoney += ChancePayCardList[pullRandomCard].amountToPay;

                _UIManager.setBonusMoney(gameMan.bonusMoney);
                setCardText(ChancePayCardList[pullRandomCard].Name, ChancePayCardList[pullRandomCard].Description, ChancePayCardList[pullRandomCard].amountToPay);

                ChancePayCardList.Remove(ChancePayCardList[ChancePayCardList.Count-1]);
                ChancePayCardList.Remove(ChancePayCardList[ChancePayCardList.Count-1]);

                break;
            case 1: //pull a receive chance card
                pullRandomCard = Random.Range(0, ChanceReceiveCardList.Count);

                Debug.Log(_curPlayer + " Pull a Chance Receive card");
                Debug.Log("Your card is:\n" + ChanceReceiveCardList[pullRandomCard].Description + "\n you receive: " + ChanceReceiveCardList[pullRandomCard].amountToReceive);

                _curPlayer.GetComponent<PlayerScript>().recieveMoney(ChanceReceiveCardList[pullRandomCard].amountToReceive);
                setCardText(ChanceReceiveCardList[pullRandomCard].Name, ChanceReceiveCardList[pullRandomCard].Description, ChanceReceiveCardList[pullRandomCard].amountToReceive);
                break;
            case 2: //pull a move chance card
                pullRandomCard = Random.Range(0, ChanceMoveCardList.Count);
                Debug.Log("Move to " + ChanceMoveCardList[pullRandomCard].waypointToMoveTo.name);

                //_curPlayer.GetComponent<Move>().MoveToSpecificLocation(ChanceMoveCardList[pullRandomCard].waypointToMoveTo);
                setCardText(ChanceMoveCardList[pullRandomCard].Name, ChanceMoveCardList[pullRandomCard].Description);
                _curPlayer.GetComponent<Move>().waypointDestination = ChanceMoveCardList[pullRandomCard].waypointToMoveTo;
                _curPlayer.GetComponent<Move>().movingToCardLocation = true;
                _curPlayer.GetComponent<Move>().atDest = false;
                break;
        }
        
    }
    public void pullRandomCommunityChestCard(GameObject _curPlayer)
    {
        int decideCardType = Random.Range(0, 3);
        int pullRandomCard;
        switch (decideCardType)
        {
            case 0:
                pullRandomCard = Random.Range(0, CCPayCardList.Count);
                Debug.Log(_curPlayer + " Pull a CommunityChest Pay card");
                Debug.Log("Your card is:\n" + CCPayCardList[pullRandomCard].Description + "\n you pay: " + CCPayCardList[pullRandomCard].amountToPay);

                _curPlayer.GetComponent<PlayerScript>().executePayment(CCPayCardList[pullRandomCard].amountToPay);

                gameMan.bonusMoney += CCPayCardList[pullRandomCard].amountToPay;
                setCardText(CCPayCardList[pullRandomCard].Name, CCPayCardList[pullRandomCard].Description, CCPayCardList[pullRandomCard].amountToPay);
                _UIManager.setBonusMoney(gameMan.bonusMoney);
                break;
            case 1:
                pullRandomCard = Random.Range(0, CCReceiveCardList.Count);

                Debug.Log(_curPlayer + " Pull a CommunityChest Receive card");
                Debug.Log("Your card is:\n" + CCReceiveCardList[pullRandomCard].Description + "\n you receive: " + CCReceiveCardList[pullRandomCard].amountToReceive);

                setCardText(CCReceiveCardList[pullRandomCard].Name, CCReceiveCardList[pullRandomCard].Description, CCReceiveCardList[pullRandomCard].amountToReceive);
                _curPlayer.GetComponent<PlayerScript>().recieveMoney(CCReceiveCardList[pullRandomCard].amountToReceive);
                break;
            case 2://pull a cc move card
                pullRandomCard = Random.Range(0, CCMoveCardList.Count);
                Debug.Log("Move to " + CCMoveCardList[pullRandomCard].waypointToMoveTo.name);
                setCardText(CCMoveCardList[pullRandomCard].Name, CCMoveCardList[pullRandomCard].Description);
                _curPlayer.GetComponent<Move>().waypointDestination = CCMoveCardList[pullRandomCard].waypointToMoveTo;
                _curPlayer.GetComponent<Move>().movingToCardLocation = true;
                _curPlayer.GetComponent<Move>().atDest = false;
                break;
        }
    }
    public void setCardText(string _cardTitel, string _cardText)
    {
        CardPanel.SetActive(true);
        priceText.gameObject.SetActive(false);
        cardTitel.text = _cardTitel;
        cardText.text = _cardText;
    }
    public void setCardText(string _cardTitel, string _cardText, int _price)
    {
        CardPanel.SetActive(true);
        priceText.gameObject.SetActive(true);
        priceText.text = "€" + _price.ToString();
        cardTitel.text = _cardTitel;
        cardText.text = _cardText;
    }
    public void HideCardPanel()
    {
        CardPanel.SetActive(false);
    }

}
