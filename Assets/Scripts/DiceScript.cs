using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DiceScript : MonoBehaviour {

    public Transform[] Dices;
    [SerializeField]
    private List<Transform> NumDice1;
    [SerializeField]
    private List<Transform> NumDice2;
    [SerializeField]
    private GameManager gameManager;

    public bool GenerateDices = false;
    public bool HideAllDices = false;
    public int totalDices;
    public int num1;
    public int num2;

    public Move Player;
    //public Move Player;
    // Use this for initialization
    void Start () {

        foreach (Transform trans in Dices[0])
        {
            NumDice1.Add(trans);
            trans.gameObject.SetActive(false);
        }
        foreach (Transform trans in Dices[1])
        {
            NumDice2.Add(trans);
            trans.gameObject.SetActive(false);
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        /*if (GenerateDices)
        {
            ThrowDices();
        }*/
        if (HideAllDices)
        {
            HideDices();
        }
	}

    public void ThrowDices()
    {
        Player = gameManager.activePlayer.GetComponent<Move>();
        GenerateDices = false;
        foreach (var tile in NumDice1)
        {
            tile.gameObject.SetActive(false);
        }
        foreach (var tile in NumDice2)
        {
            tile.gameObject.SetActive(false);
        }

        //Debug.Log("Je hebt "+ num1 +" en "+ num2 +" geworpen.");
        StartCoroutine(MovePlayerAfterDelay());
        //Player.mayMove = true;
        //Player.count = totalDices;
        //Debug.Log("Dices count " + CountDices(num1, num2));
    }

    IEnumerator MovePlayerAfterDelay()
    {
        StartCoroutine(RandomDices());
        yield return new WaitForSeconds(2f);
        totalDices = CountDices(num1, num2);
        Player.mayMove = true;
        Player.count = totalDices;
    }

    public IEnumerator RandomDices()
    {
        for (int i = 0; i < 10; i++)
        {
            NumDice1[num1].gameObject.SetActive(false);
            NumDice2[num2].gameObject.SetActive(false);

            num1 = RandomNum();
            num2 = RandomNum();

            NumDice1[num1].gameObject.SetActive(true);
            NumDice2[num2].gameObject.SetActive(true);
            //Debug.Log(i + " keer gegooit");
            yield return new WaitForSeconds(0.1f);
        }
    }

    int RandomNum()
    {
        int value1 = Random.Range(0, 6);
        return value1;
    }

    int CountDices(int a, int b)
    {
        int c = (a + 1) + (b + 1);
        return c;
    }

    void HideDices()
    {
        HideAllDices = false;
        foreach (var tile in NumDice1)
        {
            tile.gameObject.SetActive(false);
        }
        foreach (var tile in NumDice2)
        {
            tile.gameObject.SetActive(false);
        }
    }

    public void playerHasThrown()
    {
        gameManager.playerHasThrown = true;
    } 
}
