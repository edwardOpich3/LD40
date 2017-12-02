using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToHowToPlay : MonoBehaviour
{
	public Button myButton;

	// Use this for initialization
	void Start ()
	{
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(HowToPlay);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void HowToPlay()
	{
		SceneManager.LoadScene("How to Play");
	}
}
