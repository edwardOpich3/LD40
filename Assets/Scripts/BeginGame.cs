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
		SceneManager.LoadScene("Gameplay");
	}
}
