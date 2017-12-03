using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevel : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(Navigate);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void Navigate()
	{
		SceneManager.LoadScene("Gameplay");

		SceneManager.sceneLoaded += GameObject.Find("Game Manager").GetComponent<GameBehavior>().init;
	}
}
