using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayersScript : PlayerController {
    private List<GameObject> playerObjects = new List<GameObject>();
    // Use this for initialization
    public void createPlayerObjectList(GameObject _player)
    {
        playerObjects.Add(_player);
        showPlayerObjects(playerObjects);
    }


    public void showPlayerObjects(List<GameObject> _playerList)
    {
        
        for (int i = 0; i < _playerList.Count; i++)
        {
            Debug.Log(_playerList[i].name);
            if (_playerList[i].name == "Player")
            {
                Debug.Log("ShowPlayerObject Loop");
                //Debug.Log(test.name + ", " + _playerList[i] + ", " + _playerList[i].name);
            }
            
            //Player newPlayer = new Player(returnSprite(), _playerList[i], _playerList[i].name);
        }
    }
    private void Start()
    {
        Debug.Log("CreatePlayerScript Start");
    }



}
