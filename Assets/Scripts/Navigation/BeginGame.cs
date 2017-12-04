using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginGame : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(ToGame);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void ToGame()
	{
		if(gameObject.name == "Beginner")
		{
			GameObject.Find("Game Manager").GetComponent<GameBehavior>().gameMode = 0;
			SceneManager.LoadScene("Cutscene");
		}

		else if (gameObject.name == "Survival")
		{
			GameObject.Find("Game Manager").GetComponent<GameBehavior>().gameMode = 1;
			SceneManager.LoadScene("Gameplay");

			SceneManager.sceneLoaded += GameObject.Find("Game Manager").GetComponent<GameBehavior>().init;
		}
	}
}
