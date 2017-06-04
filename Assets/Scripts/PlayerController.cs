using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public class Player
    {
        public string name;
        public Sprite characterIMG;
        public GameObject playerObject;
        public Player(Sprite _characterIMG, GameObject _playerObject, string _name)
        {
            characterIMG = _characterIMG;
            playerObject = _playerObject;
            name = _name;
        }
    }
    
    private List<Player> playerList = new List<Player>();


    private void Start()
    {

    }
    public void addPlayerToList(Sprite _img, GameObject _playerObject, string _name)
    {
        playerList.Add(new Player(_img, _playerObject, _name));
        //Debug.Log(_playerObject.name);
    }

    
    public House manager;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {

        

        if (other.gameObject.tag != "Player")
        {
            //Debug.Log(other.gameObject.name);
            
            /*if (gameObject.name == "Player")
            {
                manager.lastPlayerPosition = other.gameObject;

            }*/
            /*else if (gameObject.name == "Bot1")
            {
                manager.lastBot1Position = other.gameObject;

            }
            else if (gameObject.name == "Bot")
            {
                manager.lastBot2Position = other.gameObject;

            }*/
            manager.currentPositionObject = other.gameObject;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        
        
    }
}
