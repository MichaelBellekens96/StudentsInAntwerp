using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : GameManager {
    public Text PropName, PropDescription, PropPrice, PropPays;
    public Image PropIMG;
    public List<Sprite> PropertyImages = new List<Sprite>();
    private List<string> PropertyDescriptions = new List<string>();
    Property selectedHouse;

    private float houseMarge = 0f;
    public GameObject currentPositionObject = null;

    public Object housePrefab;
    public Object hotelPrefab;

    public List<string> propertyNames;
    

    private string[] streetNames = { "Schanshoeveweg", "Eikenstraat", "Molenstraat" ,"Rumstestraat", "Reetse Straat", "Steenweg", "Olmenlaan", "PP Rubensstraat", "Trains", "Services"};
    private List<int>streetLength = new List<int>{2, 3, 3, 3, 3, 3, 3, 2,4,2 };
    private List<int> costOfHouseArray = new List<int> { 50, 50, 50, 50, 50, 100, 100, 100, 100, 100, 100, 150, 150, 150, 150, 150, 150, 200, 200, 200, 200, 200 };
    private List<int> costToBuy = new List<int> { 60,60,100,100,120,140,140,160,180,180,200,220,220,240,260,260,280,300,300,320,350,400, 200, 200, 200, 200,150, 150};


    private List<int> priceArray = new List<int> { 2,4,6,6,8,10,10,12,14,14,16,18,18,20,22,22,24,26,26,28,35,50,100,100,100,100,100,100};
    private List<int> price1House = new List<int> { 10,20,30,30,40,50,50,60,70,70,80,90,90,100,110,110,120,130,130,150,175,200};
    private List<int> price2Houses = new List<int> {30,60,90,90,100,150,150,180,200,200,220,250,250,300,330,330,360,390,390,450,500,600 };
    private List<int> price3Houses = new List<int> {90,180,270,270,300,450,450,500,550,550,600,700,700,750,800,800,850,900,900,1000,1100,1400 };
    private List<int> price4Houses = new List<int> {160,320,400,400,450,625,625,700,750,750,800,875,875,925,975,975,1025,1100,1100,1200,1300,1700 };
    private List<int> priceHotel = new List<int> {250,450,550,550,600,750,750,900,950,950,1000,1050,1050,1100,1150,1150,1200,1275,1275,1400,1500,2000 };

    public GameObject[] BoardToCollide;

    List<Property> propertyArray = new List<Property>();
    List<Street> streetArray = new List<Street>();
    List<extraProperty> extraPropertyList = new List<extraProperty>();

    List<GameboardBox> buyalbePropertyiesOnBoard = new List<GameboardBox>();
    List<GameboardBox> actionBoxesOnBoard = new List<GameboardBox>();
    List<GameboardBox> trainsOnBoard = new List<GameboardBox>();
    List<GameboardBox> buyableServicesOnBoard = new List<GameboardBox>();
    List<GameboardBox> cardsOnBoard = new List<GameboardBox>();
    List<GameboardBox> boxesToPayOn = new List<GameboardBox>();

    public class GameboardBox
    {
        public string boxName;
        public int numberOnGameboard;
        public string typeOfBox;
        public GameObject positionOnBoard;
        public GameboardBox(string _boxName,int _numberOnBoard, string _typeOfBox, GameObject _positionOnBoard)
        {
            boxName = _boxName;
            numberOnGameboard = _numberOnBoard;
            typeOfBox = _typeOfBox;
            positionOnBoard = _positionOnBoard;
        }
    }
    public class Street
    {
        public string streetName;
        public bool fullStreet;
        public GameObject streetOwner;
        public int streetLength;

        public Street() { }
        public Street(string _streetName, bool _fullStreet, GameObject _streetOwner, int _streetLength)
        {
            streetName = _streetName;
            fullStreet = _fullStreet;
            streetOwner = _streetOwner;
            streetLength = _streetLength;
        }
    }
    public class extraProperty: Street
    {
        public string name;
        public int cost;
        public int priceToPay;
        public GameObject currentOwner;
        public GameObject positionOnBoardToCollide;
        public extraProperty(string _name, int _price, GameObject _currentOwner, GameObject _positionOnBoard, int _priceToPay, string _partOfStreet)
        {
            name = _name;
            cost = _price;
            currentOwner = _currentOwner;
            positionOnBoardToCollide = _positionOnBoard;
            priceToPay = _priceToPay;
            streetName = _partOfStreet;
            fullStreet = false;
        }

    }
    public class Property : Street
    {
        public string name;
        public int price;
        public GameObject currentOwner;
        public GameObject positionOnBoardToCollide;
        public Sprite PropertyImage;
        public string PropertyDescription;

        public int costOf1House;

        public int priceToPay;
        public int priceToPay1House;
        public int priceToPay2House;
        public int priceToPay3House;
        public int priceToPay4House;
        public int priceToPayHotel;

        public byte buildHouses;
        public bool fullBuild;

        public Property(string _name, int _price, GameObject _currentOwner, string _partOfStreet, int _priceToPay, int _priceToPay1House, int _priceToPay2House, int _priceToPay3House, int _priceToPay4House, int _priceToPayHotel, int _costOf1House, GameObject _positionOnBoardToCollide, Sprite _propertyImage, string _propertyDescription)
        {
            streetName = _partOfStreet;

            name = _name;
            price = _price;
            currentOwner = _currentOwner;
            PropertyDescription = _propertyDescription;
            PropertyImage = _propertyImage;
            priceToPay = _priceToPay;

            priceToPay1House = _priceToPay1House;
            priceToPay2House = _priceToPay2House;
            priceToPay3House = _priceToPay3House;
            priceToPay4House = _priceToPay4House;
            priceToPayHotel = _priceToPayHotel;

            positionOnBoardToCollide = _positionOnBoardToCollide;

            costOf1House = _costOf1House;
            fullBuild = false;

            buildHouses = 0;
        }
    }

    void Start()
    {
        fillDescriptionList();
        initiateGameboardList();
        initiateStreetList();
        initiatePropertyArray();
        //cheatPlayerOwnsEverything();
        //showAllBuyableBoxes();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Property")
                {
                    selectedHouse = getProperty(hit.collider.name);
                    Debug.Log("Selected property: " + selectedHouse.name + " part of street " + selectedHouse.streetName);
                            if (selectedHouse.currentOwner == base.activePlayer)
                            {
                                buildButton.gameObject.SetActive(true);
                            }
                            if (selectedHouse.currentOwner != activePlayer)
                            {
                                Debug.Log("You don't own this building");
                                buildButton.gameObject.SetActive(false);
                            }                   
                 }
            }
        }
    }
    public void buyProperty()
    {
        if (checkIfBuyable(currentPositionObject))
        {
            if (currentPositionObject.tag == "Property")
            {
                Property propertyToBuy = getProperty(currentPositionObject.name);
                if (activePlayer.GetComponent<PlayerScript>().Money >= propertyToBuy.price && propertyToBuy.currentOwner == null)
                {
                    propertyToBuy.currentOwner = activePlayer;
                    activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuy.price);
                    checkStreet(propertyToBuy);

                    //SetPropertyPanel(propertyToBuy.name, /*propertyToBuy.description*/"Omschrijving", "Zonder koten: €" + propertyToBuy.priceToPay + "\n1Kot: €" + propertyToBuy.priceToPay1House + "\n2Koten: €" + propertyToBuy.priceToPay2House + "\n3Koten: €" + propertyToBuy.priceToPay3House + "\n4Koten: €" + propertyToBuy.priceToPay4House + "\nGigakot: €" + propertyToBuy.priceToPayHotel,propertyToBuy.price,/*propertyToByt.img*/null);

                    Debug.Log(activePlayer.name + " Bought " + propertyToBuy.name);
                    Debug.Log("Money left: " + activePlayer.GetComponent<PlayerScript>().Money);
                }
                else if (propertyToBuy.currentOwner != null)
                {
                    Debug.Log("Someone else owns this property");

                }
                else if (activePlayer.GetComponent<PlayerScript>().Money < propertyToBuy.price)
                {
                    Debug.Log("Not enough money");
                }
            }
            else
            {
                extraProperty propertyToBuy = getExtraProperty(currentPositionObject.name);
                if (activePlayer.GetComponent<PlayerScript>().Money >= propertyToBuy.cost && propertyToBuy.currentOwner == null)
                {
                    propertyToBuy.currentOwner = activePlayer;
                    activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuy.cost);
                    checkStreet(propertyToBuy);
                    //SetPropertyPanel(propertyToBuy.name, /*propertyToBuy.description*/"Omschrijving","Vaste inkomst van: € " + propertyToBuy.priceToPay, propertyToBuy.cost,/*propertyToByt.img*/null);
                    Debug.Log(activePlayer.name + " Bought " + propertyToBuy.name);
                }
                else if (propertyToBuy.currentOwner != null)
                {
                    Debug.Log("Someone else owns this property");
                }
                else if (activePlayer.GetComponent<PlayerScript>().Money < propertyToBuy.cost)
                {
                    Debug.Log("Not enough money");
                }
            }
        }
        else
        {
            Debug.Log("You cant buy this place, it is a " + currentPositionObject.tag);
        }       
    } //buy the (Extra)Property you're on
    public void setHouse()
    {
        Property propertyToBuildOn = selectedHouse;
        GameObject house, hotel, gameObjectOfSelectedProperty;
        gameObjectOfSelectedProperty = propertyToBuildOn.positionOnBoardToCollide;

        if (checkStreet(propertyToBuildOn))
        {
            if (propertyToBuildOn.buildHouses == 4 && propertyToBuildOn.fullBuild == false && (activePlayer.GetComponent<PlayerScript>().Money-propertyToBuildOn.costOf1House) >=0)
            {
                activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuildOn.costOf1House);
                foreach (Transform child in gameObjectOfSelectedProperty.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                Debug.Log("Het collision item hiervan is "+gameObjectOfSelectedProperty.name);
                hotel = Instantiate(hotelPrefab, new Vector3(gameObjectOfSelectedProperty.transform.position.x, gameObjectOfSelectedProperty.transform.position.y, gameObjectOfSelectedProperty.transform.position.z + 0.45f), gameObjectOfSelectedProperty.transform.rotation) as GameObject;
                hotel.transform.parent = gameObjectOfSelectedProperty.transform;
                propertyToBuildOn.buildHouses += 1;
                propertyToBuildOn.fullBuild = true;
                Debug.Log("€ " + activePlayer.GetComponent<PlayerScript>().Money);
            }

            else if (propertyToBuildOn.buildHouses < 4 && propertyToBuildOn.fullBuild == false && (activePlayer.GetComponent<PlayerScript>().Money - propertyToBuildOn.costOf1House) >= 0)
            {
                Debug.Log("Het collision item heeft een rotatie van " + gameObjectOfSelectedProperty.transform.rotation);
                propertyToBuildOn.buildHouses += 1;
                activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuildOn.costOf1House);
                house = Instantiate(housePrefab, new Vector3(gameObjectOfSelectedProperty.transform.position.x, gameObjectOfSelectedProperty.transform.position.y, gameObjectOfSelectedProperty.transform.position.z + 0.45f), gameObjectOfSelectedProperty.transform.rotation) as GameObject;
                house.transform.parent = gameObjectOfSelectedProperty.transform;
                houseMarge += 0.2f;
                Debug.Log("€ " + activePlayer.GetComponent<PlayerScript>().Money);
            }
            else if (activePlayer.GetComponent<PlayerScript>().Money - propertyToBuildOn.costOf1House < 0 && propertyToBuildOn.fullBuild == false)
            {
                Debug.Log("You don't have enough money");
            }
            else if (propertyToBuildOn.fullBuild)
            {
                Debug.Log("You already own a hotel on this property");
            }
        }
        else
        {
            Debug.Log("You don't own the full street");
        }            
    } //place a house if you're the owner of the street
    public void setHouse(Property _propertyToBuildOn)
    {
        Property propertyToBuildOn = _propertyToBuildOn;
        GameObject house, hotel, gameObjectOfSelectedProperty;
        gameObjectOfSelectedProperty = propertyToBuildOn.positionOnBoardToCollide;

        if (checkStreet(propertyToBuildOn))
        {
            if (propertyToBuildOn.buildHouses == 4 && propertyToBuildOn.fullBuild == false && (activePlayer.GetComponent<PlayerScript>().Money - propertyToBuildOn.costOf1House) >= 0)
            {
                activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuildOn.costOf1House);
                foreach (Transform child in gameObjectOfSelectedProperty.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                hotel = Instantiate(hotelPrefab, new Vector3(gameObjectOfSelectedProperty.transform.position.x, gameObjectOfSelectedProperty.transform.position.y, gameObjectOfSelectedProperty.transform.position.z + 0.45f), gameObjectOfSelectedProperty.transform.rotation) as GameObject;
                hotel.transform.parent = gameObjectOfSelectedProperty.transform;
                propertyToBuildOn.buildHouses += 1;
                propertyToBuildOn.fullBuild = true;
                Debug.Log("€ " + activePlayer.GetComponent<PlayerScript>().Money);
            }

            else if (propertyToBuildOn.buildHouses < 4 && propertyToBuildOn.fullBuild == false && (activePlayer.GetComponent<PlayerScript>().Money - propertyToBuildOn.costOf1House) >= 0)
            {
                propertyToBuildOn.buildHouses += 1;
                activePlayer.GetComponent<PlayerScript>().executePayment(propertyToBuildOn.costOf1House);
                house = Instantiate(housePrefab, new Vector3(gameObjectOfSelectedProperty.transform.position.x, gameObjectOfSelectedProperty.transform.position.y, gameObjectOfSelectedProperty.transform.position.z + 0.45f), gameObjectOfSelectedProperty.transform.rotation) as GameObject;
                house.transform.parent = gameObjectOfSelectedProperty.transform;
                houseMarge += 0.2f;
                Debug.Log("€ " + activePlayer.GetComponent<PlayerScript>().Money);
            }
            else if (activePlayer.GetComponent<PlayerScript>().Money - propertyToBuildOn.costOf1House < 0 && propertyToBuildOn.fullBuild == false)
            {
                Debug.Log("You don't have enough money");
            }
            else if (propertyToBuildOn.fullBuild)
            {
                Debug.Log("You already own a hotel on this property");
            }
        }
        else
        {
            Debug.Log("You don't own the full street");
        }
    } //place a house if you're the owner of the street

    public Property getProperty(string _selectedProperty)
    {
        Property activeProperty = null;
        for (int i = 0; i < propertyArray.Count; i++)
        {
            //Debug.Log(propertyArray[i].name + " , " + _selectedProperty);
            if (propertyArray[i].name == _selectedProperty)
            {
                
                //Debug.Log(propertyArray[i].name + " , " + propertyArray[i].streetName);
                activeProperty = propertyArray[i];
            }
        }
        //Debug.Log(activeProperty.name + " , " + activeProperty.streetName);
        return activeProperty;
    }//returns the Property item of a string
    public extraProperty getExtraProperty(string _selectedProperty)
    {
        extraProperty activeProperty = null;
        for (int i = 0; i < extraPropertyList.Count; i++)
        {
            if (extraPropertyList[i].name == _selectedProperty)
            {
                activeProperty = extraPropertyList[i];
            }
        }
        //Debug.Log(activeProperty.name + " , " + activeProperty.streetName);
        return activeProperty;
    }//returns the ExtraProperty item of a string

    public bool checkIfBuyable(GameObject _position)
    {
        if (_position.tag == "Property")
        {
            return true;
        }
        else if (_position.tag == "Train" || _position.tag == "BuyableServices")
        {
            return true;
        }
        return false;
    }// check if your current position is a buyable item
    public bool checkStreet(Property _selectedProperty)
    {
        bool fullStreet = true;
        Street activeStreet=null;
       // _boughtProperty.streetName;
        for (int i = 0; i < streetArray.Count; i++)
        {
            if (_selectedProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }

        for (int i = 0; i < propertyArray.Count; i++)
        {
            if (activeStreet.streetName == propertyArray[i].streetName)
            {
                //Debug.Log(propertyArray[i].name + " is part of street " + activeStreet.streetName);
                if (propertyArray[i].currentOwner != activePlayer)
                {
                    //Debug.Log("You don't own " + propertyArray[i].name);
                    fullStreet = false;
                }
            }
        }

        if (fullStreet)
        {
            activeStreet.streetOwner = activePlayer;
            Debug.Log("Congratz, you own the " + activeStreet.streetName);
            activeStreet.fullStreet = true;
        }

        return fullStreet;
    } //returns a bool (true if the current player owns the full street, false if not), this function works for the Properties
    public bool checkStreet(extraProperty _selectedProperty)
    {
        bool fullStreet = true;
        Street activeStreet = null;
        // _boughtProperty.streetName;
        for (int i = 0; i < streetArray.Count; i++)
        {
            if (_selectedProperty.streetName == streetArray[i].streetName)
            {

                activeStreet = streetArray[i];
                //Debug.Log("Active street is " + activeStreet.streetName);
            }
        }

        for (int i = 0; i < extraPropertyList.Count; i++)
        {
            if (activeStreet.streetName == extraPropertyList[i].streetName)
            {
                //Debug.Log(extraPropertyList[i].name + " is part of street " + activeStreet.streetName);
                if (extraPropertyList[i].currentOwner != activePlayer)
                {
                    //Debug.Log("You don't own " + extraPropertyList[i].name);
                    fullStreet = false;
                }
            }
        }

        if (fullStreet)
        {
            activeStreet.streetOwner = activePlayer;
            Debug.Log("Congratz, you own the " + activeStreet.streetName);
            activeStreet.fullStreet = true;
        }

        return fullStreet;
    } //returns a bool (true if the current player owns the full street, false if not), this function works for the ExtraProperties

    public void fillDescriptionList()
    {
        PropertyDescriptions.Add("De Sint-Salvatorabdij (ook gekend als de Pieter Potabdij) was een abdij in Antwerpen waarvan de monniken behoorden tot de orde der cisterciënzers. De abdij werd opgericht op gronden die eigendom waren van Pieter Pot, een zakenman uit Dordrecht, die zich in het begin van de 15e eeuw in Antwerpen had gevestigd. De abdij werd opgeheven tijdens de Franse periode, in 1797 openbaar verkocht en omgevormd tot woonhuizen. Enkel de kapel is bewaard gebleven. De Grote en Kleine Pieter Potstraat zijn genoemd naar de financierder/stichter van deze abdij.");
        PropertyDescriptions.Add("De Sint-Salvatorabdij (ook gekend als de Pieter Potabdij) was een abdij in Antwerpen waarvan de monniken behoorden tot de orde der cisterciënzers. De abdij werd opgericht op gronden die eigendom waren van Pieter Pot, een zakenman uit Dordrecht, die zich in het begin van de 15e eeuw in Antwerpen had gevestigd. De abdij werd opgeheven tijdens de Franse periode, in 1797 openbaar verkocht en omgevormd tot woonhuizen. Enkel de kapel is bewaard gebleven. De Grote en Kleine Pieter Potstraat zijn genoemd naar de financierder/stichter van deze abdij.");
        PropertyDescriptions.Add("Of het echt een sport is kan over gediscussieerd worden, maar ontspanning is het zeker wel. Bij Antwerp Bowling nemen ze het ontspanningselement heel serieus. Naast de bowlingbanen is er een bar en een restaurant en bieden ze partyconcepten aan, zodat je na het bowlen kan aanschuiven aan een steengrill.");
        PropertyDescriptions.Add("Behalve het gebruikelijke recept: fitness, groepslessen en een saun, biedt Wezenberg Fit ook een trilplaattraining. Op de trilplaat staan en de prikkels in ontvangst nemen is het enigste dat je hoeft te doen. En om de natuur nog een handje te helpen, kun je terecht in de shop voor allerlei energie- en eiwitrijke supplementen terecht. Slank en/of gespeird worden is nog nooit zo makkelijk geweest!");
        PropertyDescriptions.Add("Een oud spoorwegterrein maakte plaats voor glooiend groen en lange wandelpaden. Een skatepark, speelfonteinen en een groot café met terras krijg je er gratis bij. De aanleg van het baanbrekende park is één stap in de algemene herwaardering van het Noord. Met de inplanting van een nieuwe hogeschoolcampus en sportfaciliteiten wordt het park en omstreken steedds meer een plek om in de gaten te houden.");
        PropertyDescriptions.Add("Je was doen, wordt voortaan een pretje. Hier kan je brunchen of apretieven, terwijl je je draaiende wasgoed in het oog kan houden. Zo gaat niemand aan de haal met je vuile onderbroeken. Een professioneel woordje uitleg van de medewerkers helpt t evoorkomen dat je was vier maten kleiner uit de machine komt.");
        PropertyDescriptions.Add("Comics Station Antwerp is het nieuwe indoor pretpark rond populaire Belgische stripfiguren. Suske en Wiske, Jommeke, De Kiekeboes, De Smurfen, Lucky Luke en Urbanus heten je hier welkom! Wat Comics Station Antwerp nog zo uniek maakt? Het is wereldwijd het eerste park dat gevestigd is in een internationaal treinstation. En bovendien vind je er de hoogste indoor glijbaan ter wereld, van maar liefst 22.5 m hoog! Eén ding staat al vast: Comics Station Antwerp garandeert een unieke belevenis voor het hele gezin: ouders, grootouders, kinderen, tieners … Stap binnen in onze 6 themazones, met verschillende leuke attracties.Je vindt er niet alleen superleuke mechanische attracties, maar ook tal van interactieve elementen en doe - activiteiten.Zo komen de stripverhalen écht tot leven!");
        PropertyDescriptions.Add("Deze speciaalzaak met de Spaanse naam voor smaak doet het al vermoeden. Enkel kwaliteitsvolle etenswaren die de smaakpapillen prikkelen vliegen hier over de toonbank. Gusta is de eerste gluten- en lactosevrije winkel in Antwerpen. Zit je met een voedseltolerantie? Geen probleem het gamma bestaat uit gluten-, suiker-, ei-, gist-, en lactosevrije producten, zoals allergenenvrij brood en koffiekoeken (op zaterdag), maar ook groenten en fruit.");
        PropertyDescriptions.Add("Onderaan het Centraal Station, aan de metro- en fietsingang, ligt de Fietshaven. Daar kan je voor 60 euro een citroengele fiets met zeven versnellingen huren, die jet het hele academiejaar mag bijhouden. Daarin zitten twee sloten en een verplichte onderhoudsbeurt inbegrepen. Je kan ook fietsen voor kortere periodes huren. Neem eerst contact op om je fiets te reserveren en spring er dan binnen.");
        PropertyDescriptions.Add("Vanaf dit academiejaar kan je terecht in GATE15; het nieuwe centrum voor én door de Antwerpse student. We adviseren je bij de serieuze én minder serieuze zaken waar je mee te maken krijgt en dat doen we van bij je aankomst tot je vertrek. Op zoek naar een vergaderzaal voor je club of een zaal voor een feestje, iets checken op internet in onze computerruimte, jouw stem laten horen op de voting wall, even bijpraten met een drankje tijdens je springuren, cultuurcheque kopen of een vraag stellen aan het Studentenoverleg of ons redactieteam ontmoeten... dat en meer.");
        PropertyDescriptions.Add("Na 16 jaar ontwortelt Scheld’apen zich aan de kaaien en groeit het jeugdcultuurcentrum uit tot een artistiek, permanent experiment aan de Ankerrui. Logischerwijs handelt Het Bos op duurzame wijze: zelf gebrouwen bosbier, eerlijke koffie en wereldkeukense filmdiners. Onder het bladerdak schuilen expo’s, muziekoptredens, workshops en een botanische tuininstallatie. Kunst staat hier in volle bloei, maar ook kleine organisaties en verenigingen mogen het huis gebruiken als co-workingkweekvijver. Wees welkom in Het Bos!");
        PropertyDescriptions.Add("In DIVA ontdek je hoe verrassend, divers en actueel het Briljantwerps verhaal is! DIVA is de unieke versmelting tussen het vroegere Diamant - en Zilvermuseum.Net als een diamant is ze mysterieus en veelzijdig.Ze verbindt Antwerpen opnieuw met alle facetten van zijn briljant verleden. In december 2017 fonkelt dit nieuwe museum en belevingscentrum aan de Suikerrui te Antwerpen.Ondertussen kan je haar ontmoeten in de paviljoenen aan het MAS, bij allerlei activiteiten en via haar website.");
        PropertyDescriptions.Add("Het motto luidt ‘Jij kookt, wij inspireren!’. De winkel is eigenlijk een kookboek waar je kunt binnenstappen. De winkel is namelijk ingedeeld in per gerecht in plaats van per soort ingrediënt. Een gloednieuw concept dat voor het eerst gelanceerd werd hier in België. Als klant kies je welk gerecht je wilt eten, waarna je alle benodigde ingrediënten kunt terugvinden op één plek in de shop. Deze opstelling bespaart je veel kostbare tijd.");
        PropertyDescriptions.Add("Eén van de bekenste partylocaties in Antwerpen. Op donderdag stromen de studenten binnen in grote hoeveelheden. De toegang is dan gratis. Op vrijdag betaal je iets meer, maar dan staan er hippe concepten zoals Peoples, House on Fire!, Sound Architecture, Tape Tape, King Kong, Feestgedruis en Lucid op het programma.");
        PropertyDescriptions.Add("Soms lijkt het wel alsof je in een sprookje bent beland. Je loopt tussen de mooiste standbeelden in de prachtige natuur van het Middelheimpark. Vaak zijn er eigenzinnige exposities of installaties van nationale en internationale kunstenaars. Maar ook de vaste collectie is een must voor alle kunstminnende studenten. Je kan er heerlijk schetsen of gewoon een gezellige wandeling maken. Je hoeft hier immers geen inkom te betalen. Een leuke tip is het museumcafé van uitbater Peter Reel.");
        PropertyDescriptions.Add("De stadsschouwburg is de verzamelplaats bij uitstek voor musicals, dansspektakels, ballet en cabaret. De schouwburg heeft vaak een meer commercieel aanbod, maar soms kan je er terecht voor zeer intieme dansvoorstellingen. De prijs voor een ticket is niet echt democratisch, maar als je snel bent, kan je meestal een ticket tussen de 15 en 20 euro bemachtigen.");
        PropertyDescriptions.Add("Cinema Rex was een van de grootste cinema’s in Antwerpen tot het in 1944 door een bomaanslag vernield werd en er 567 doden vielen. Bijna 50 jaar later, in 1995 opende hier de UGC. Als je de ingang aan de Keyserlei neemt, kan een alerte wandelaar nog een gedenkplaat terugvinden. Een historisch verantwoorde filmavond dus!");
        PropertyDescriptions.Add("Knal in het centrum van Antwerpen, maar wel meer ‘plaats’ dan ‘groen’. Omringd door cafés en restaurants, met een standbeeld van Rubens in het midden, en de kathedraal op de achtergrond. Vroeger werden de rijke mensen in de kathedraal begravenen de arme op het ‘groene kerkhof’. Toen er niet meer begraven mocht worden, werd er een laag beton over het kerkhof gestort. Et voilà, de groenplaats. Elk seizoen brengt hier andere evenementen, marktjes of spektakels.");
        PropertyDescriptions.Add("De portugese gemeenschap in Antwerpen streek jaren geleden neer aan dit plein, getuige de talloze barrios en de volkstoeloop als hun nationale ploeg weer eens een belangrijke voetbalmatch speelt. Op woensdag en vrijdag nemen marktkramers het plein over, en kun je er het meest exotische eten en drinken ontdekken.");
        PropertyDescriptions.Add("De Ossenmarkt, de Stadswaag en het plein aan de academie zijn doordrongen van de studentesfeer. Je vindt er het ene studenten café na het andere. Geen wonder dat deze pleinen jongeren aantrekken: of het nu is om er te feesten, dan wel om rustig met een pint in de hand eens bij te praten. Let wel, feestneuzen, rustigere studenten en de buurtbewoners leven er dicht op elkaar. Hou er dus rekening met ieders nachtrust en laat geen afval rondslingeren.");
        PropertyDescriptions.Add("De Antwerpse cultuurtempel bij uitstek verdeelt haar programma min of meer in vier blokken: dans, theater, muziek en architectuur. Deze internationale kunstencampus programmeert niet enkel cultuur uit binnen- en buitenstad, maar huist ook de studenten van het conservatorium. In het Grand Café deSingel kan je, naast een hoop lekkers, genieten van het surrealistisch zicht op de Antwerpse ring.");
        PropertyDescriptions.Add("Het MAS of Museum aan de Stroom is een museum in Antwerpen, dat in mei 2011 opende. Het museum is gelegen in de oude haven op het Eilandje. Het grondoppervlakte van het museumgebouw bedraagt 1.350 m², op een totale oppervlakte van 14.500 m². De totale hoogte van het torengebouw bedraagt 62 meter en is daarmee een nieuw oriëntatiepunt in de stad. Het panoramazicht en de spiraalvormige boulevard langs de glaspartijen van het gebouw zijn tot 's avonds laat gratis toegankelijk en vormen zo een nieuwe toeristische trekpleister.");
    }
    public void initiateGameboardList()
    {
        for (int i = 0; i < BoardToCollide.Length; i++)
        {
            if (BoardToCollide[i].tag == "Property")
            {
                buyalbePropertyiesOnBoard.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
            else if (BoardToCollide[i].tag == "Train")
            {
                trainsOnBoard.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
            else if (BoardToCollide[i].tag == "BuyableServices")
            {
                buyableServicesOnBoard.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
            else if (BoardToCollide[i].tag == "ChanceCard" || BoardToCollide[i].tag == "CommunityChestCard")
            {
                cardsOnBoard.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
            else if (BoardToCollide[i].tag == "Pay")
            {
                boxesToPayOn.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
            else
            {
                actionBoxesOnBoard.Add(new GameboardBox(BoardToCollide[i].name, i, BoardToCollide[i].tag, BoardToCollide[i]));
            }
        }
        //Debug.Log(cardsOnBoard.Count + " cards on board");
        
    } //creates lists of the waypoints
    public void initiateStreetList()
    {
        for (int i = 0; i < streetNames.Length; i++)//create streets
        {
            streetArray.Add(new Street(streetNames[i], false, null, streetLength[i])); //you have to create an array for variable streetlengths
        }
    } //creates a list of all the streets
    public void initiatePropertyArray()
    {
        for (int i = 0; i < streetArray.Count-2; i++) //create a list of all the properties
        {
            for (int b = 0; b < streetArray[i].streetLength; b++)
            {
                propertyArray.Add(new Property(buyalbePropertyiesOnBoard[0].boxName, costToBuy[0], null, streetArray[i].streetName, priceArray[0], price1House[0], price2Houses[0], price3Houses[0], price4Houses[0], priceHotel[0], costOfHouseArray[0], buyalbePropertyiesOnBoard[0].positionOnBoard, PropertyImages[0],PropertyDescriptions[0]));
                //Debug.Log(buyalbePropertyiesOnBoard[0].boxName + " , " + streetArray[i].streetName);
                deleteFirstInEveryList();
                
            }
            
        }
        for (int i = 0; i < trainsOnBoard.Count; i++)
        {

            extraPropertyList.Add(new extraProperty(trainsOnBoard[i].boxName, costToBuy[0], null, trainsOnBoard[i].positionOnBoard, priceArray[0],streetArray[streetArray.Count-2].streetName));
            priceArray.Remove(priceArray[0]);
            costToBuy.Remove(costToBuy[0]);
        }
        for (int i = 0; i < buyableServicesOnBoard.Count; i++)
        {
            extraPropertyList.Add(new extraProperty(buyableServicesOnBoard[i].boxName, costToBuy[0], null, buyableServicesOnBoard[i].positionOnBoard, priceArray[0], streetArray[streetArray.Count-1].streetName));
            //Debug.Log("initialize service: " + buyableServicesOnBoard[i].boxName + " in " + streetArray[streetArray.Count-1].streetName);
            priceArray.Remove(priceArray[0]);
            costToBuy.Remove(costToBuy[0]);
        }
        /*Debug.Log(propertyArray.Count + " Buyable properties on board");
        Debug.Log(extraPropertyList.Count + " Buyable extras on board");
        Debug.Log(cardsOnBoard.Count + " cards on board");
        Debug.Log(actionBoxesOnBoard.Count + " action boxes on board");*/
    } //creates the Property & ExtraProperty lists
    public void deleteFirstInEveryList()
    {
        PropertyDescriptions.Remove(PropertyDescriptions[0]);
        PropertyImages.Remove(PropertyImages[0]);
        buyalbePropertyiesOnBoard.Remove(buyalbePropertyiesOnBoard[0]);
        costToBuy.Remove(costToBuy[0]);
        priceArray.Remove(priceArray[0]);
        price1House.Remove(price1House[0]);
        price2Houses.Remove(price2Houses[0]);
        price3Houses.Remove(price3Houses[0]);
        price4Houses.Remove(price4Houses[0]);
        priceHotel.Remove(priceHotel[0]);
        costOfHouseArray.Remove(costOfHouseArray[0]);
    } //deletes first item in the lists that aren't needed
    public void showAllBuyableBoxes()
    {
        for (int i = 0; i < propertyArray.Count; i++)
        {
            Debug.Log(propertyArray[i].name + " , " + propertyArray[i].streetName);
        }
        for (int i = 0; i < extraPropertyList.Count; i++)
        {
            Debug.Log(extraPropertyList[i].name + " , " + extraPropertyList[i].streetName);
        }
    } //controll funtion to show all the buyable boxes

    public void checkIfOwned()
    {
        if (checkIfBuyable(currentPositionObject))
        {
            if (currentPositionObject.tag == "Property")
            {
                Property propertyToCheck = getProperty(currentPositionObject.name);

                if (propertyToCheck.currentOwner != null)//to insert current player)  
                {
                    //Debug.Log("active player: " + activePlayer.name + " owner: " + propertyToCheck.currentOwner.name);
                    if (propertyToCheck.currentOwner != activePlayer)
                    {
                        Debug.Log(activePlayer.name + " has payed " + /*propertyToCheck.priceToPay*/ ReturnPriceToPayOnProperty(currentPositionObject) + " to " + propertyToCheck.currentOwner);
                        activePlayer.GetComponent<PlayerScript>().executePayment(ReturnPriceToPayOnProperty(currentPositionObject));
                        propertyToCheck.currentOwner.GetComponent<PlayerScript>().recieveMoney(ReturnPriceToPayOnProperty(currentPositionObject));
                    }
                    else
                    {
                        //Debug.Log("Current player BEZIT DIT GEBOUW");
                    }
                }
                else
                {
                    //Debug.Log("No owner");
                }
            }
            else
            {
                extraProperty propertyToCheck = getExtraProperty(currentPositionObject.name);

                if (propertyToCheck.currentOwner != null)//to insert current player)  
                {
                    Debug.Log("active player: " + activePlayer.name + " owner: " + propertyToCheck.currentOwner.name);
                    if (propertyToCheck.currentOwner != activePlayer)
                    {
                        Debug.Log(activePlayer.name + " has payed " + propertyToCheck.priceToPay + " to " + propertyToCheck.currentOwner);
                        activePlayer.GetComponent<PlayerScript>().executePayment(propertyToCheck.priceToPay);
                        propertyToCheck.currentOwner.GetComponent<PlayerScript>().recieveMoney(propertyToCheck.priceToPay);
                    }
                    else
                    {
                        Debug.Log("Current player BEZIT DIT GEBOUW");
                    }
                }
                else
                {
                    Debug.Log("No owner");
                }
            }
             
        }        
    } //checks if your current position is owned by someone and pays the fee to the owner
    public void checkCurrentProperty()
    {
        if (checkIfBuyable(currentPositionObject))
        {
            checkIfOwned();
        }
    } //checks if position is buyable and then checks if its owned

    public int checkHousesOnProperty()
    {
        int numberOfHouses = 0;
        for (int i = 0; i < propertyArray.Count; i++)
        {
            if (propertyArray[i].currentOwner == activePlayer)
            {
                if (propertyArray[i].buildHouses > 0 && propertyArray[i].buildHouses != 4)
                {
                    numberOfHouses += propertyArray[i].buildHouses;
                }
            }
        }
        //Debug.Log("U bezit in totaal: " + numberOfHouses + " huisjes");
        return numberOfHouses;
    } //returns number of houses owned by one player
    public int checkHotelsOnProperty()
    {
        int numberOfHotels = 0;
        for (int i = 0; i < propertyArray.Count; i++)
        {
            if (propertyArray[i].currentOwner == activePlayer)
            {
                if (propertyArray[i].buildHouses == 4)
                {
                    numberOfHotels += 1;
                }
            }
        }
        //Debug.Log("U bezit in totaal: " + numberOfHotels + " hotelletjes");
        return numberOfHotels;     
    } //returns number of hotels owned by one player

    public int returnNumberOfFreePropertiesInStreet(GameObject _currentPosition)
    {
        Property currentProperty = getProperty(_currentPosition.name);
        Street activeStreet = null;
        int freePropertiesInStreet = 0;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }

        for (int i = 0; i < propertyArray.Count; i++)
        {
            if (propertyArray[i].streetName == activeStreet.streetName)
            {
                if (propertyArray[i].currentOwner == null)
                {
                    freePropertiesInStreet += 1;
                }
            }
        }
        //Debug.Log("Free properties in street: " + freePropertiesInStreet);
        return freePropertiesInStreet;
    } //returns number of free properties in a street (Works for Properties)
    public int returnLengthOfCurrentPropertyStreet(GameObject _currentPosition)
    {
        Property currentProperty = getProperty(_currentPosition.name);
        Street activeStreet = null;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }
        //Debug.Log("StreetLenght: " + activeStreet.streetLength);
        return activeStreet.streetLength;
    } //returns the length of a street (works for properties)


    public int returnNumberOfFreeExtraPropertiesInStreet(GameObject _currentPosition)
    {
        extraProperty currentProperty = getExtraProperty(_currentPosition.name);
        Street activeStreet = null;
        int freePropertiesInStreet = 0;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }

        for (int i = 0; i < extraPropertyList.Count; i++)
        {
            if (extraPropertyList[i].streetName == activeStreet.streetName)
            {
                if (extraPropertyList[i].currentOwner == null)
                {
                    freePropertiesInStreet += 1;
                }
            }
        }
        Debug.Log("Free properties in street: " + freePropertiesInStreet);
        return freePropertiesInStreet;
    }//returns number of free properties in a street (Works for ExtraProperties)
    public int returnLengthOfCurrentExtraPropertyStreet(GameObject _currentPosition)
    {
        extraProperty currentProperty = getExtraProperty(_currentPosition.name);
        Street activeStreet = null;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }
        //Debug.Log("StreetLenght: " + activeStreet.streetLength);
        return activeStreet.streetLength;
    }//returns the length of a street (works for ExtraProperties)

    public bool CheckIfPropertyStreetIsFreeOrYours(GameObject _currentPosition)
    {
        //Debug.Log("Properties " + streetArray.Count);
        Property currentProperty = getProperty(_currentPosition.name);
        Street activeStreet = null;
        List<bool> NotOwnedOrYours = new List<bool>();
        bool willBuy = true;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }

        for (int i = 0; i < propertyArray.Count; i++)
        {
            if (propertyArray[i].streetName == activeStreet.streetName)
            {
                if (propertyArray[i].currentOwner == null || propertyArray[i].currentOwner == activePlayer)
                {
                    //Debug.Log(activePlayer.name + " would buy " + propertyArray[i].name);
                    NotOwnedOrYours.Add(true);
                }
                else
                {
                    //Debug.Log(activePlayer.name + " would NOT buy " + propertyArray[i].name);
                    NotOwnedOrYours.Add(false);
                }
            }
        }

        for (int i = 0; i < NotOwnedOrYours.Count; i++)
        {
            
            if (NotOwnedOrYours[i] == false)
            {
                willBuy = false;
            }
        }
        if (willBuy == false)
        {
            Debug.Log("Won't buy");
        }
        return willBuy;
    }//checks if the other properties of the street you're on are owned by someone (works for Properties)
    public bool CheckIfExtraPropertyStreetIsFreeOrYours(GameObject _currentPosition)
    {
        //Debug.Log("Extra properties " +streetArray.Count);
        extraProperty currentProperty = getExtraProperty(_currentPosition.name);
        Street activeStreet = null;
        List<bool> NotOwnedOrYours = new List<bool>();
        bool willBuy = true;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (currentProperty.streetName == streetArray[i].streetName)
            {
                activeStreet = streetArray[i];
            }
        }

        for (int i = 0; i < extraPropertyList.Count; i++)
        {
            if (extraPropertyList[i].streetName == activeStreet.streetName)
            {
                if (extraPropertyList[i].currentOwner == null || extraPropertyList[i].currentOwner == activePlayer)
                {
                    //Debug.Log(activePlayer.name + " would buy " + extraPropertyList[i].name);
                    NotOwnedOrYours.Add(true);
                }
                else
                {
                    //Debug.Log(activePlayer.name + " would NOT buy " + extraPropertyList[i].name);
                    NotOwnedOrYours.Add(false);
                }
            }
        }

        for (int i = 0; i < NotOwnedOrYours.Count; i++)
        {
            if (NotOwnedOrYours[i] == false)
            {
                Debug.Log("Won't buy");
                willBuy = false;
            }
        }
        return willBuy;
    }//checks if the other properties of the street you're on are owned by someone (works for ExtraProperties)

    public int ReturnNumberOfHousesOnCurrentPropery(GameObject _currentPosition)
    {
        Property activeProperty = getProperty(_currentPosition.name);

        return activeProperty.buildHouses;
    }//returns the number of houses on a property (5=hotel)
    public int ReturnPriceToPayOnProperty(GameObject _currentPosition)
    {
        int priceToPay = 0;
        Property activeProperty = getProperty(_currentPosition.name);

        switch (ReturnNumberOfHousesOnCurrentPropery(_currentPosition))
        {
            case 0:
                priceToPay = activeProperty.priceToPay;
                break;
            case 1:
                priceToPay = activeProperty.priceToPay1House;
                break;
            case 2:
                priceToPay = activeProperty.priceToPay2House;
                break;
            case 3:
                priceToPay = activeProperty.priceToPay3House;
                break;
            case 4:
                priceToPay = activeProperty.priceToPay4House;
                break;
            case 5:
                priceToPay = activeProperty.priceToPayHotel;
                break;
        }
        return priceToPay;
    } //returns the price to pay from current property (cost without house < with house,...)

    public Property ReturnMostExpensiveBuildableProperty()
    {
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GET ALL STREETS~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        List<Street> ownedStreetsList = new List<Street>();
        for (int i = 0; i < streetArray.Count; i++)
        {
            if (streetArray[i].streetOwner == activePlayer)
            {
                if (streetArray[i].streetName != "Services" && streetArray[i].streetName != "Trains")
                {
                    ownedStreetsList.Add(streetArray[i]);
                }
            }
        }
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~GET PROPERTIES IN STREET~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        List<Property> myMostExpensiveProperties = new List<Property>();
        for (int i = 0; i < propertyArray.Count; i++)
            {
                if (propertyArray[i].streetName == ownedStreetsList[ownedStreetsList.Count - 1].streetName)
                {
                    myMostExpensiveProperties.Add(propertyArray[i]);
                }
            }
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~CHECK IF MOST EXPENSIVE PROPERTY IS FULLBUILD~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        Property myMostExpensiveProperty=null;
        for (int i = myMostExpensiveProperties.Count - 1; i >= 0; i--)
        {
            if (myMostExpensiveProperties[i].fullBuild == false)
            {
                myMostExpensiveProperty = myMostExpensiveProperties[i];
            }
            else
            {
                Debug.Log(myMostExpensiveProperties[i].name + " is full build");
            }
            if (i == 0)
            {

            }
            
        }
        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        return myMostExpensiveProperty;
    }
    public Property ReturnBestBuildableProperty()
    {
        List<Street> ownedStreetsList = new List<Street>();
        //get all streets
        for (int i = 0; i < streetArray.Count; i++)
        {
            if (streetArray[i].streetOwner == activePlayer)
            {
                if (streetArray[i].streetName != "Services" && streetArray[i].streetName != "Trains")
                {
                    ownedStreetsList.Add(streetArray[i]);
                }
            }
        }

        List<Property> PropertiesInOwnedStreets = new List<Property>();
        for (int i = 0; i < ownedStreetsList.Count; i++)
        {
            for (int b = 0; b < propertyArray.Count; b++)
            {
                if (propertyArray[b].streetName == ownedStreetsList[i].streetName)
                {
                    PropertiesInOwnedStreets.Add(propertyArray[b]);
                    
                }
            }
        }
        for (int i = PropertiesInOwnedStreets.Count-1; i >= 0; i--)
        {
            if (PropertiesInOwnedStreets[i].fullBuild == false)
            {
                Debug.Log("Will build on " + PropertiesInOwnedStreets[i].name);
                return PropertiesInOwnedStreets[i];
            }
        }
        return null;
    }

    public bool DoYouOwnAnything()
    {
        bool OwnAnything = false;

        for (int i = 0; i < streetArray.Count; i++)
        {
            if (streetArray[i].streetOwner == activePlayer)
            {
                if (streetArray[i].streetName != "Services" && streetArray[i].streetName != "Trains")
                {
                    OwnAnything = true;
                }

                //Debug.Log(activePlayer.name + " owns the " + streetArray[i].streetName);
            }
        }
        return OwnAnything;
    }
    public bool DoYouOwnAStreet()
    {
        bool DoYouOwnAStreet = false;
        for (int i = 0; i < streetArray.Count; i++)
        {
            if (streetArray[i].streetOwner == activePlayer)
            {
                if (streetArray[i].streetName != "Services" && streetArray[i].streetName != "Trains")
                {
                    DoYouOwnAStreet = true;
                }

                //Debug.Log(activePlayer.name + " owns the " + streetArray[i].streetName);
            }
        }
        return DoYouOwnAStreet;
    }

    public void SetPropertyPanel(string _name, string _description, string _payments, int _price, Sprite _PropIMG)
    {
        PropName.text = _name;
        PropDescription.text = _description;
        PropPays.text = _payments;
        PropPrice.text = "Aankoopprijs: €" + _price;
        PropIMG.sprite = _PropIMG;
    }
    public void getPropertyPanelValues()
    {
        if (checkIfBuyable(currentPositionObject))
        {
            if (currentPositionObject.tag == "Property")
            {
                Property propertyToBuy = getProperty(currentPositionObject.name);
                SetPropertyPanel(propertyToBuy.name,propertyToBuy.PropertyDescription, "Inkomsten\nZonder koten: €" + propertyToBuy.priceToPay + "\n1 Kot: €" + propertyToBuy.priceToPay1House + "\n2 Koten: €" + propertyToBuy.priceToPay2House + "\n3 Koten: €" + propertyToBuy.priceToPay3House + "\n4 Koten: €" + propertyToBuy.priceToPay4House + "\nGigakot: €" + propertyToBuy.priceToPayHotel, propertyToBuy.price, propertyToBuy.PropertyImage);
                PropIMG.enabled = true;
                PropDescription.enabled = true;
            }
            else
            {
                extraProperty propertyToBuy = getExtraProperty(currentPositionObject.name);
                SetPropertyPanel(propertyToBuy.name, /*propertyToBuy.description*/"", "Inkomsten\nVaste inkomst van: € " + propertyToBuy.priceToPay, propertyToBuy.cost,/*propertyToByt.img*/null);
                PropIMG.enabled = false;
                PropDescription.enabled = false;
            }
        }
        else
        {
            Debug.Log("You cant buy this place, it is a " + currentPositionObject.tag);
        }
    }

    public void cheatPlayerOwnsEverything()
    {
        for (int i = 0; i < propertyArray.Count; i++)
        {
            propertyArray[i].currentOwner = activePlayer;
        }
        for (int i = 0; i < streetArray.Count; i++)
        {
            streetArray[i].streetOwner = activePlayer;
        }
        Debug.Log(activePlayer.name + " owns everything now.");
    } //extra code that makes the current player the owner of every buildable property
    public void resetPlayersOwnedProperties(GameObject player)
    {

        if (player.GetComponent<PlayerScript>().hasBeenReset == false)
        {
            Debug.Log("Reset properties from bankrupt: " + player.name);
            player.GetComponent<PlayerScript>().hasBeenReset = true;
            for (int i = 0; i < propertyArray.Count; i++)
            {
                if (propertyArray[i].currentOwner == player)
                {
                    if (propertyArray[i].buildHouses > 0)
                    {
                        foreach (Transform child in propertyArray[i].positionOnBoardToCollide.transform)
                        {
                            GameObject.Destroy(child.gameObject);
                        }
                    }
                    propertyArray[i].currentOwner = null;
                    propertyArray[i].fullBuild = false;
                    propertyArray[i].fullStreet = false;
                    propertyArray[i].streetOwner = null;
                    propertyArray[i].buildHouses = 0;

                }
            }
        }        
    }
}

