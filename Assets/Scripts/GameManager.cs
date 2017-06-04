using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int bonusMoney = 0;

    public GameObject[] Players;
    public GameObject activePlayer;
    public House housePlayers;
    public DiceScript Dice;
    public CameraController Cam;
    public BotsMind botsMind;


    public int curPlayer = 0;
    public bool playerAtDest = false;
    public bool playerHasThrown = false;
    public bool AIhasThrown = false;
    public bool endedTurn = false;
    public bool prisonChecked = false;
    public bool checkingWinner = false;

    public Button buyButton;
    public Button buildButton;
    public Button throwButton;
    public Button endTurn;
    public Button payedPrison;
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public AudioSource rollingDices;
    // Use this for initialization
    void Start() {
        pausePanel.SetActive(false);
        payedPrison.gameObject.SetActive(false);
        buildButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);
        StartCoroutine(GameLoop());
        
    }

    IEnumerator GameLoop() // Gameloop overloopt alles wat een speler in 1 beurt doet
    {
        activePlayer = Players[curPlayer];  // Stelt huidige speler in
        housePlayers.activePlayer = activePlayer;
        endTurn.gameObject.SetActive(false);
        //Debug.Log("active player set to: " + activePlayer.name);
        if (activePlayer.name == "Player")
        {
            endedTurn = false;
            //Debug.Log("It's the normal player");
            if (activePlayer.GetComponent<Move>().inPrison == true)
            {
                //Debug.Log("Player still in prison");

                prisonChecked = false;
                activePlayer.GetComponent<Move>().remainingTurnsInPrison--;
                StartCoroutine(activePlayer.GetComponent<Move>().CheckPrison());
                while (!prisonChecked)
                {
                    yield return null;
                }

                if (activePlayer.GetComponent<Move>().inPrison == true)
                {
                    curPlayer++;
                    //Debug.Log("Ending turn");
                    StartGameLoop();
                }
                else
                {
                    //Debug.Log("Player NOT in prison");
                    StartGameLoop();
                }
            }
            else
            {
                //Debug.Log("Player NOT in prison");
                throwButton.gameObject.SetActive(true);
                // Debug.Log("Wachten tot speler gooit");
                while (!playerHasThrown) // Zolang de speler niet gegooid heeft zal de while volgende frame opnieuw beginnen
                {
                    yield return null;
                }
                rollingDices.Play();
                //Debug.Log("Dobbelstenen worden gegooid");
                Dice.ThrowDices();
                playerHasThrown = false;
            }

        }
        else // Wanneer het een bot is automatisch gooien
        {
            if (activePlayer.GetComponent<Move>().inPrison == true)
            {
                Debug.Log("Bot still in prison");
                prisonChecked = false;
                activePlayer.GetComponent<Move>().remainingTurnsInPrison--;
                StartCoroutine(activePlayer.GetComponent<Move>().CheckPrison());
                while (!prisonChecked)
                {
                    yield return null;
                }

                if (activePlayer.GetComponent<Move>().inPrison == true)
                {
                    curPlayer++;
                    Debug.Log("Ending turn of bot");
                    StartGameLoop();
                }
                else
                {
                    Debug.Log("Bot NOT in prison");
                    StartGameLoop();
                }
            }
            else
            {
                //Debug.Log("Het is een bot");
                buyButton.gameObject.SetActive(false);
                throwButton.gameObject.SetActive(false);
                //Debug.Log("Dobbelstenen worden gegooid");
                Dice.ThrowDices();
                rollingDices.Play();
                endedTurn = true;
            }
        }
        //Debug.Log("Wachten tot de speler op zijn positie is...");
        while (!playerAtDest) // Zolang de speler niet op zijn bestemming is code hier tegenhouden
        {
            yield return null;
        }
        //Debug.Log("Speler op zijn positie");
        housePlayers.checkCurrentProperty();
        botsMind.BotBuildingDecisions();
        playerAtDest = false;
        //Debug.Log("3 seconden wachten op camera...");
        yield return new WaitForSeconds(3f); // Wacht 3 seconden zodat de camera terug op zijn plek staat
        //Debug.Log("Wachten tot de speler zijn beurt eindigd");
        if (activePlayer.name == "Player")
        {
            endTurn.gameObject.SetActive(true);
        }
        while (!endedTurn)
        {
            yield return null;
        }
        endTurn.gameObject.SetActive(false);
        CheckWinner();
        while (checkingWinner)
        {
            yield return null;
        }
        //Debug.Log("Beurt voorbij, volgende index wordt ingesteld");
        curPlayer++; // Volgende index instellen
        
        if (curPlayer == Players.Length)
        {
            //Debug.Log("Speler index naar 0 gereset");
            curPlayer = 0; // Wanneer het de laatste speler zijn beurt was terug naar de eerste speler gaan
        }
        if (Players[curPlayer].GetComponent<PlayerScript>().bankRupt == true)
        {
            curPlayer++;
        }
        //Debug.Log("Gameloop eindigen");
        StartGameLoop(); // De GameLoop opnieuw starten voor de volgende speler

    }

    public void StartGameLoop()
    {
        StopCoroutine(GameLoop());
        StartCoroutine(GameLoop());
    }

    public void EndTurn()
    {
        endedTurn = true;
    }

    public void PlayerHasPayed()
    {
        activePlayer.GetComponent<Move>().payedPrison = true;
    }

    public void controllBuyButton()
    {
        if (activePlayer.GetComponent<Move>().atDest == true)
        {
            if (activePlayer.GetComponent<Move>().dest.tag == "Property")
            {
                if (housePlayers.getProperty(activePlayer.GetComponent<Move>().dest.name).currentOwner == null)
                {
                    buyButton.gameObject.SetActive(true);
                }
                else
                {
                    buyButton.gameObject.SetActive(false);
                }
            }
            else
            {
                if (housePlayers.getExtraProperty(activePlayer.GetComponent<Move>().dest.name).currentOwner == null)
                {
                    buyButton.gameObject.SetActive(true);
                }
                else
                {
                    buyButton.gameObject.SetActive(false);
                }
            }
        }
    }

    public void PauseGame()
    {
        
        if (Time.timeScale == 0)
        {

            Time.timeScale = 1;
            pausePanel.SetActive(false);
            activateButtons();
        }
        else
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            deactivateButtons();
        }    
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        activateButtons();
    }
    public void LoadStartScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void activateButtons()
    {
        buyButton.interactable = true;
        buildButton.interactable = true;
        throwButton.interactable = true;
        endTurn.interactable = true;
        payedPrison.interactable = true;
    }
    public void deactivateButtons()
    {
        buyButton.interactable = false;
        buildButton.interactable = false;
        throwButton.interactable = false;
        endTurn.interactable = false;
        payedPrison.interactable = false;
    }

    public void CheckWinner()
    {
        checkingWinner = true;
        for (int i = 0; i < Players.Length; i++)
        {
            if (Players[i].GetComponent<PlayerScript>().Money <= 0)
            {
                Players[i].GetComponent<PlayerScript>().bankRupt = true;
                Debug.Log("Set to bankrupt state" + Players[i].name);
                housePlayers.resetPlayersOwnedProperties(Players[i]);

                if (Players[i].name == "Player")
                {
                    gameOverPanel.SetActive(true);
                    //gameOverPanel.GetComponent<Text>().text = "Game Over!";
                    gameOverPanel.GetComponentInChildren<Text>().text = "Game Over!";
                    Time.timeScale = 0;
                }
            }
        }
        if (Players[1].GetComponent<PlayerScript>().bankRupt == true && Players[2].GetComponent<PlayerScript>().bankRupt == true)
        {
            gameOverPanel.SetActive(true);
            //gameOverPanel.GetComponent<Text>().text = "U heeft gewonnen!!";
            gameOverPanel.GetComponentInChildren<Text>().text = "U heeft gewonnen!!";
            Time.timeScale = 0;
        }
        checkingWinner = false;
    }
}
