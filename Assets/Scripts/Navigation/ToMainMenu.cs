using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToMainMenu : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(Back);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void Back()
	{
		SceneManager.LoadScene("Main Menu");
	}
}
