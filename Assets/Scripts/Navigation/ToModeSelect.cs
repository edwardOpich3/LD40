using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ToModeSelect : MonoBehaviour
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
		GetComponent<AudioSource>().Play();

		SceneManager.LoadScene("ModeSelect");
	}
}
