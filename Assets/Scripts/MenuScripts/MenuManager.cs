using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    private SpriteRenderer selectedCharactersSprite;
    private static SpriteRenderer spriteToSave;
    //private SpriteRenderer mySpriteRenderer = null;
    private void Start()
    {
        //Debug.Log("Menu Manager Script Start");
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        

    }
    public void startGame(Button _activeButton)
    {
        if (_activeButton != null)
        {
            UIManager _UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            selectedCharactersSprite = _activeButton.gameObject.GetComponent<SpriteRenderer>();
            _UIManager.setPlayerSprite(selectedCharactersSprite);
            _UIManager.getCharacterButtons();
            SceneManager.UnloadSceneAsync(2);
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        } 
    }
    

}
