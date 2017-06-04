using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Move Player;
    public GameManager gameMan;

    private Transform cameraPosition;
    private Vector3 originPos;
    private Vector3 newCamPos;
    public bool delayStarted = true;
    public bool coroutineStarted = false;
    public bool zoomedIn = false;
	// Use this for initialization
	void Start () {
        cameraPosition = GetComponent<Transform>();
        originPos = cameraPosition.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameMan.activePlayer.GetComponent<Move>().mayMove)
        {
            //cameraPosition.position = Player.transform.position + new Vector3(0, 2, 0);
            newCamPos = gameMan.activePlayer.GetComponent<Move>().transform.position + new Vector3(0, 2, 0);
            //cameraPosition.position = Vector3.Lerp(transform.position, newCamPos, Time.deltaTime*2);
            cameraPosition.position = Vector3.MoveTowards(transform.position, newCamPos, Time.deltaTime * 15);
            zoomedIn = true;
        }
        else if(gameMan.activePlayer.GetComponent<Move>().movingToPrison)
        {
            //cameraPosition.position = Player.transform.position + new Vector3(0, 2, 0);
            newCamPos = gameMan.activePlayer.GetComponent<Move>().transform.position + new Vector3(0, 2, 0);
            //cameraPosition.position = Vector3.Lerp(transform.position, newCamPos, Time.deltaTime*2);
            cameraPosition.position = Vector3.MoveTowards(transform.position, newCamPos, Time.deltaTime * 15);
            zoomedIn = true;
        }
        else if (gameMan.activePlayer.GetComponent<Move>().movingToCardLocation)
        {
            //cameraPosition.position = Player.transform.position + new Vector3(0, 2, 0);
            newCamPos = gameMan.activePlayer.GetComponent<Move>().transform.position + new Vector3(0, 2, 0);
            //cameraPosition.position = Vector3.Lerp(transform.position, newCamPos, Time.deltaTime*2);
            cameraPosition.position = Vector3.MoveTowards(transform.position, newCamPos, Time.deltaTime * 15);
            zoomedIn = true;
        }
        else
        {
            if (delayStarted == true && coroutineStarted == false && zoomedIn == true)
            {
                coroutineStarted = true;
                StartCoroutine(ZoomOut());
            }
            if (transform.position != originPos && delayStarted == false)
            {
                cameraPosition.position = Vector3.MoveTowards(transform.position, originPos, Time.deltaTime * 15);
                //Debug.Log("Start coroutine...");
            }
            if (transform.position == originPos && coroutineStarted == true)
            {
                coroutineStarted = false;
                zoomedIn = false;
                delayStarted = true;
            }
        }
	}
    IEnumerator ZoomOut()
    {
        yield return new WaitForSeconds(2f);
        delayStarted = false;
    }
}
