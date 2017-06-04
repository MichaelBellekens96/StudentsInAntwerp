using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    private Button[] buttonArray;
    private Image myChar, bot1Char, bot2Char;
    private Text myName, bot1Name, bot2Name;
    private Text myMoney, bot1Money, bot2Money;
    private Text bonusMoney;
    // Use this for initialization
    void Start () {
        myChar = GameObject.Find("PlayerIMG").GetComponentInChildren<Image>();
        bot1Char = GameObject.Find("Bot1IMG").GetComponentInChildren<Image>();
        bot2Char = GameObject.Find("Bot2IMG").GetComponentInChildren<Image>();

        myName = GameObject.Find("PlayerName").GetComponentInChildren<Text>();
        bot1Name = GameObject.Find("Bot1Name").GetComponentInChildren<Text>();
        bot2Name = GameObject.Find("Bot2Name").GetComponentInChildren<Text>();

        myMoney = GameObject.Find("PlayerMoney").GetComponentInChildren<Text>();
        bot1Money = GameObject.Find("Bot1Money").GetComponentInChildren<Text>();
        bot2Money = GameObject.Find("Bot2Money").GetComponentInChildren<Text>();

        bonusMoney = GameObject.Find("BonusAmount").GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void setPlayerSprite(SpriteRenderer _img)
    {
        myChar.sprite = _img.sprite;
        //Debug.Log("Sprite for Player");
    }
    public void setBot1Sprite(SpriteRenderer _img)
    {
        bot1Char.sprite = _img.sprite;
        //Debug.Log("Sprite for bot 1");
    }
    public void setBot2Sprite(SpriteRenderer _img)
    {
        bot2Char.sprite = _img.sprite;
        //Debug.Log("Sprite for bot 2");
    }
    public void setRandomSprites(Button[] _buttons)
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].gameObject.GetComponent<SpriteRenderer>().sprite == myChar.sprite)
            {
                Debug.Log("ZELDE SPRITE GEVONDE");
            }
            else
            {
                if (bot1Char.sprite == null)
                {
                    setBot1Sprite(_buttons[i].gameObject.GetComponent<SpriteRenderer>());
                    
                }
                else
                {
                    setBot2Sprite(_buttons[i].gameObject.GetComponent<SpriteRenderer>());
                    
                    break;
                }
                
            }
        }
    }
    public void getCharacterButtons()
    {
        buttonArray = GameObject.Find("ElementHolder").GetComponentsInChildren<Button>();
        setRandomSprites(buttonArray);
    }
    public Image returnMyChar()
    {
        return myChar;
    }
    public Image returnBot1Char()
    {
        return bot1Char;
    }
    public Image returnBot2Char()
    {
        return bot2Char;
    }
    public void setPlayerMoney(int _money)
    {
        myMoney.text = _money.ToString();
    }
    public void setBot1Money(int _money)
    {
        bot1Money.text = _money.ToString();
    }
    public void setBot2Money(int _money)
    {
        bot2Money.text = _money.ToString();
    }
    public void setPlayerName(string _name)
    {
        myName.text = _name.ToString();
    }
    public void setBot1Name(string _name)
    {
        bot1Name.text = _name.ToString();
    }
    public void setBot2Name(string _name)
    {
        bot2Name.text = _name.ToString();
    }
    public void setBonusMoney(int _money)
    {
        bonusMoney.text = "Bonus: " + _money.ToString();
    }
}
