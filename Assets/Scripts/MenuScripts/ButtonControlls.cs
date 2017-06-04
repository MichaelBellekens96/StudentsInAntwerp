using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControlls : MenuManager {

	// Use this for initialization
	private void Start () {
        //Debug.Log("Button Controlls Script Start");
        hideAllDescriptions();
        hideAllBackgroundBlurs();
    }
    public void hideAllDescriptions()
    {
        GameObject parentObject = GameObject.Find("ElementHolder");
        Text[] allChildren = parentObject.GetComponentsInChildren<Text>();
        for (int b = 0; b < allChildren.Length; b++)
        {
            if (allChildren[b].name == "Description")
            {
                allChildren[b].enabled = false;
            }
        }
    }
    public Button getSelectedCharacter()
    {
        GameObject parentObject = GameObject.Find("ElementHolder");
        Text[] allChildren = parentObject.GetComponentsInChildren<Text>();
        Button activeButton = null;
        for (int b = 0; b < allChildren.Length; b++)
        {
            if (allChildren[b].name == "Description")
            {
                if (allChildren[b].enabled)
                {
                    activeButton = allChildren[b].GetComponentInParent<Button>();
                }
            }
        }
        return activeButton;
    }
    public void showCorrectInfo(Button _activeButton)
    {
        hideAllDescriptions();
        hideAllBackgroundBlurs();
        Text[] allChildren = null;
        Image[] allIMGChildren = null;

        allChildren = _activeButton.GetComponentsInChildren<Text>();
        allIMGChildren = _activeButton.GetComponentsInChildren<Image>();
        //Debug.Log(_activeButton.name);
        for (int b = 0; b < allChildren.Length; b++)
        {
            if (allChildren[b].name == "Description")
            {

                allChildren[b].enabled = true;
            }
        }
        for (int i = 0; i < allIMGChildren.Length; i++)
        {
            if (allIMGChildren[i].name == "DB")
            {

                allIMGChildren[i].enabled = true;
            }
        }
    }
    public void hideAllBackgroundBlurs()
    {
        GameObject parentObject = GameObject.Find("ElementHolder");
        Image[] allChildren = parentObject.GetComponentsInChildren<Image>();
        for (int i = 0; i < allChildren.Length; i++)
        {
            if (allChildren[i].name == "DB")
            {
                allChildren[i].enabled = false;
            }
        }
    }
}
