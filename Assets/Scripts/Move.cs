using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Move : MonoBehaviour {

    public bool mayMove = false;

    public GameManager gameMan;
    public GameObject[] waypoints;
    public GameObject Prison;
   // private GameObject currentWP;
    public PlayerScript Player;
    public DiceScript Dice;
    public CardScript Card;
    public int index = 0;
    public int count;
    public GameObject dest;
    public bool startedMoving = false;
    public int timesMoved = 0;
    public bool atDest = false;
    public bool inPrison = false;
    public int remainingTurnsInPrison;
    public bool movingToPrison = false;
    public bool payedPrison = false;
    public BotsMind botsMind;
    public bool movingToCardLocation = false;
    public AudioSource rollingDices;

    public GameObject waypointDestination;
    private int DicesCount;
    private  UIManager _UIManager;
	// Use this for initialization
	void Start () {
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        _UIManager.setBonusMoney(0);
        this.transform.position = waypoints[0].transform.position;
        Player = GetComponent<PlayerScript>();
        //agent.SetDestination(dest.transform.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (mayMove)
        {
            MovePlayer(count);
        }
        if (movingToPrison)
        {
            MoveToPrison();
        }
        if (movingToCardLocation)
        {
            MoveToSpecificlocation(waypointDestination);
        }
	}

    public void MovePlayer(int count)
    {
        atDest = false;
        if (transform.position == waypoints[index].transform.position && startedMoving == false)
        {
            index++;
            if (index == waypoints.Length)
            {
                index = 0;
                Player.recieveMoney(200);
                Player.Money += 200;
                Debug.Log("You received €200");
                //Debug.Log("Over start gegaan.");
            }
            startedMoving = true;
        }

        if (transform.position == waypoints[index].transform.position)
        {
            timesMoved++;
            if (timesMoved == count)
            {
                //Debug.Log("Player at destination");
                mayMove = false;
                timesMoved = 0;
                startedMoving = false;
                atDest = true;
                gameMan.playerAtDest = true;
                //Debug.Log("Destination is: " + waypoints[index].tag);
                if (gameMan.activePlayer.name == "Player")
                {
                    if (waypoints[index].tag != "Pay" &&  waypoints[index].tag != "Bonus" && waypoints[index].tag != "Prison" && waypoints[index].tag != "CommunityChestCard" && waypoints[index].tag != "ChanceCard" && waypoints[index].tag != "PrisionPosition")   
                    {
                        gameMan.buyButton.gameObject.SetActive(true);
                    }
                    
                }
                
            }
            else
            {
                index++;
                if (index == waypoints.Length)
                {
                    index = 0;
                    //Debug.Log("Over andere start gegaan.");
                    Player.recieveMoney(200);
                    Debug.Log("You received €200");
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, 1.5f * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.tag == "RealPrison")
        {
            inPrison = true;
        }*/
        StartCoroutine(PutInPrison(other));
        StartCoroutine(getCommunityChestCard(other));
        StartCoroutine(getChanceCard(other));
        StartCoroutine(payFee(other));
        StartCoroutine(receiveBonus(other));
        StartCoroutine(checkPropertyForBot(other));
    }
    
    IEnumerator receiveBonus(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (col.gameObject.tag == "Bonus" && inPrison == false && atDest == true)
        {
            Debug.Log("Position: " + col.gameObject.name);
            Debug.Log("Received from bonus: " +gameMan.bonusMoney);
            Player.recieveMoney(gameMan.bonusMoney);
            gameMan.bonusMoney = 0;
            _UIManager.setBonusMoney(0);
        }
    }

    IEnumerator PutInPrison(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (col.gameObject.tag == "Prison" && inPrison == false && atDest == true)
        {
            movingToPrison = true;
            remainingTurnsInPrison = 3;
            inPrison = true;
        }
    }

    IEnumerator payFee(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (col.gameObject.tag == "Pay" && inPrison == false && atDest == true)
        {
            Debug.Log("Position: " + col.gameObject.name);
            Player.executePayment(200);
            gameMan.bonusMoney += 200;
            _UIManager.setBonusMoney(gameMan.bonusMoney);
            Debug.Log("pay 200 to bonus");       
        }
    }

    IEnumerator getCommunityChestCard(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (col.gameObject.tag == "CommunityChestCard" && inPrison == false && atDest == true && movingToCardLocation == false)
        {
            Debug.Log("Position: " + col.gameObject.name);
            Card.pullRandomCommunityChestCard(gameObject/*this is the current player*/);
        }
    }

    IEnumerator getChanceCard(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (col.gameObject.tag == "ChanceCard" && inPrison == false && atDest == true && movingToCardLocation == false)
        {
            Debug.Log("Position: " + col.gameObject.name);
            Card.pullRandomChanceCard(gameObject/*this is the current player*/);
        }
    }

    public IEnumerator CheckPrison()
    {
        Debug.Log("CheckPrison is gestart");
        if (remainingTurnsInPrison == 0)
        {
            Debug.Log("Speler moet geen beurten meer in de prison ziten");
            atDest = false;
            inPrison = false;
            //yield return false;
        }
        else
        {
            // Player laten gooien
            Debug.Log("Player gooit om uit prison te kunnen");
            gameMan.throwButton.gameObject.SetActive(true);
            gameMan.payedPrison.gameObject.SetActive(true);

            if (gameObject.name != "Player")
            {
                Debug.Log("Bot zit nog in prison");
                if (gameMan.activePlayer.GetComponent<PlayerScript>().Money > 4000)
                {
                    Debug.Log("Bot heeft meer dan €4000 en heft zich uit prison gekocht");
                    payedPrison = true;
                }
            }
            else
            {
                while (!gameMan.playerHasThrown && !payedPrison)
                {
                    yield return null;
                }
            }

            if (payedPrison == true)
            {
                Player.executePayment(1000);
                inPrison = false;
            }
            else
            {
                gameMan.playerHasThrown = false;
                StartCoroutine(Dice.RandomDices());
                rollingDices.Play();
                yield return new WaitForSeconds(1.1f);
                // wachten tot er gegooit is           

                // Wanneer dubbel inPrison false else true

                if (Dice.num1 == Dice.num2)
                {
                    Debug.Log("Player heeft dubbel gegooit en mag uit de prison");
                    inPrison = false;
                    // UI => Je mag uit de gevangenis!
                }
                else
                {
                    Debug.Log("Player heeft niet dubbel gegooit en moet in prison blijven");
                    inPrison = true;
                    // UI => Je moet nog in de gevangenis blijven.
                }
            }
        }
        yield return new WaitForSeconds(2f);
        gameMan.prisonChecked = true;
    }

    public void MoveToPrison()
    {
        if (transform.position == waypoints[index].transform.position)
        {
            index++;
            if (transform.position == Prison.transform.position)
            {
                movingToPrison = false;
                index--;
            }
            
            if (index == waypoints.Length)
            {
                index = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, 1.5f * Time.deltaTime);
    }

    IEnumerator checkPropertyForBot(Collider col)
    {
        yield return new WaitForSeconds(1f);
        if (inPrison == false && atDest == true && gameObject.name != "Player")
        {
            if (col.gameObject.tag == "Property" || col.gameObject.tag == "Train" || col.gameObject.tag == "BuyableServices")
            {
                Debug.Log("Position from bot: " + col.gameObject.name);
                botsMind.BotDecisionMaking(col.gameObject);
            }
        }
    }

    public void MoveToSpecificlocation(GameObject _SpeciLocation)
    {
        if (transform.position == waypoints[index].transform.position)
        {
            index++;
            if (transform.position == _SpeciLocation.transform.position)
            {
                movingToCardLocation = false;
                index--;
            }

            if (index == waypoints.Length)
            {
                index = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, 1.5f * Time.deltaTime);
    }
}