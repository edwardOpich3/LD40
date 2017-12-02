using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(ExitGame);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void ExitGame()
	{
		Application.Quit();
	}
}
