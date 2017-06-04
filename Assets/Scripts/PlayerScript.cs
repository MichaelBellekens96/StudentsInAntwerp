using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : PlayerController
{
    public bool hasBeenReset = false;
    public int Money;
    public bool bankRupt;
    private UIManager _UIManager;
    private void Start()
    {
        Money = 5000;
        _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        Debug.Log(_UIManager.gameObject.name);
        Image myChar;
        if (gameObject.name == "Player")
        {
            myChar = _UIManager.returnMyChar();

            _UIManager.setPlayerName(myChar.sprite.name);
            _UIManager.setPlayerMoney(Money);

            addPlayerToList(myChar.sprite, gameObject, myChar.sprite.name);
        }
        else if (gameObject.name == "Bot1")
        {
            myChar = _UIManager.returnBot1Char();

            _UIManager.setBot1Name(myChar.sprite.name);
            _UIManager.setBot1Money(Money);

            addPlayerToList(myChar.sprite, gameObject, myChar.sprite.name);
        }
        else if (gameObject.name == "Bot2")
        {
            myChar = _UIManager.returnBot2Char();

            _UIManager.setBot2Name(myChar.sprite.name);
            _UIManager.setBot2Money(Money);

            addPlayerToList(myChar.sprite, gameObject, myChar.sprite.name);
        }
        
    }
    public void executePayment(int _amount)
    {
        Money -= _amount;

        if (gameObject.name == "Player")
        {
            _UIManager.setPlayerMoney(Money);
        }
        else if (gameObject.name == "Bot1")
        {
            _UIManager.setBot1Money(Money);
        }
        else if (gameObject.name == "Bot2")
        {
            _UIManager.setBot2Money(Money);
        }
    }
    public void recieveMoney(int _amount)
    {
        Money += _amount;

        if (gameObject.name == "Player")
        {
            _UIManager.setPlayerMoney(Money);
        }
        else if (gameObject.name == "Bot1")
        {
            _UIManager.setBot1Money(Money);
        }
        else if (gameObject.name == "Bot2")
        {
            _UIManager.setBot2Money(Money);
        }
    }
}
