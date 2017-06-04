using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : ButtonControlls
{
    
    // Use this for initialization
    public void clickedButton(Button _clickedButton)
    {
        showCorrectInfo(_clickedButton);   
    }
    public void playGame()
    {
        startGame(getSelectedCharacter());
    }
    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
    
}
