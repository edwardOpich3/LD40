using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
	public GameObject pizzaPF;		// Pizza Prefab
	public GameObject orderPF;		// Order Prefab

	public bool[] orderedToppings;	// How many of what topping NEED to be on the pizza?

	private int score;
	private float orderTime;		// Time since the order was given, in seconds
	private float timeLeft;			// Time left in the day, in seconds
	private int pizzasServed;
	public Text uiText;
	public Text mistakeText;

	bool gameStarted;

	private GameObject curPizza;
	private PizzaBehavior curPizzaBehavior;

	private GameObject curOrder;

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(gameObject);

		gameStarted = false;

		orderedToppings = new bool[8];
		for(uint i = 1; i < orderedToppings.Length; i++)
		{
			orderedToppings[i] = Random.Range(0, 2) == 1;
		}
		orderedToppings[0] = true;

		score = 0;
		orderTime = 0.0f;
		timeLeft = 1.0f * 60.0f;
		pizzasServed = 0;

		uiText.text = "Score: " + score + "\n";
		uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";
		uiText.text += "Time Left: " + string.Format("{0}:{1:00}", (int)timeLeft / 60, (int)timeLeft % 60) + "\n";

		mistakeText.text = "Press Up Arrow to start!";

		curPizza = Instantiate(pizzaPF);
		curPizza.name = "Pizza";
		curPizzaBehavior = curPizza.GetComponent<PizzaBehavior>();

		curOrder = Instantiate(orderPF);
		curOrder.name = "Order Sheet";

		curOrder.GetComponent<OrderBehavior>().pizza = curPizza.GetComponent<PizzaBehavior>();
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			mistakeText.text = "";
			gameStarted = true;
		}

		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene.name == "Gameplay" && gameStarted)
		{
			orderTime += Time.deltaTime;
			timeLeft -= Time.deltaTime;
			if(timeLeft < 0.0f)
			{
				// Game over condition here!
				timeLeft = 0.0f;
				SceneManager.LoadScene("Results");
			}

			if(curPizza)
			{
				// You sent the order
				if(Camera.main.WorldToViewportPoint(new Vector2(curPizza.transform.position.x - 1.28f, 0.0f)).x > 1.0f)
				{
					mistakeText.text = "";

					bool messedUp = false;
					bool forgotToppings = false;
					bool wrongToppings = false;
					for(int i = 0; i < 8; i++)
					{
						if(curPizzaBehavior.toppings[i].activeInHierarchy != orderedToppings[i])
						{
							messedUp = true;

							// Put something on there that doesn't belong
							if(curPizzaBehavior.toppings[i].activeInHierarchy && !orderedToppings[i])
							{
								score -= 2;
								if(!wrongToppings)
								{
									mistakeText.text += "Wrong Toppings!\n";
									wrongToppings = true;
								}
							}

							// Forgot something
							else
							{
								score -= 1;
								if(!forgotToppings)
								{
									mistakeText.text += "Forgot Toppings!\n";
									forgotToppings = true;
								}
							}
						}
					}
					if(!messedUp)
					{
						score += 15;
						if((int)orderTime > 15)
						{
							mistakeText.text += "Too Slow!";
						}
						else if ((int)orderTime > 10)
						{
							mistakeText.text += "Good!";
						}
						else if ((int)orderTime > 5)
						{
							mistakeText.text += "Great!";
						}
						else
						{
							mistakeText.text += "Incredible!";
						}
					}

					score -= (int)(orderTime);

					pizzasServed++;

					Destroy(curPizza);
					orderTime = 0.0f;

					for(uint i = 1; i < orderedToppings.Length; i++)
					{
						orderedToppings[i] = Random.Range(0, 2) == 1;
					}
					orderedToppings[0] = true;

					curOrder.transform.Translate(0.0f, 0.0f, -5.0f);
					curOrder.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -1000.0f));

					curPizza = Instantiate(pizzaPF);
					curPizza.name = "Pizza";
					curPizzaBehavior = curPizza.GetComponent<PizzaBehavior>();

					curOrder = Instantiate(orderPF);
					curOrder.name = "Order Sheet";

					curOrder.GetComponent<OrderBehavior>().pizza = curPizza.GetComponent<PizzaBehavior>();
				}

				// You recycled the pizza
				else if (Camera.main.WorldToViewportPoint(new Vector2(curPizza.transform.position.x + 1.28f, 0.0f)).x < 0.0f)
				{
					Destroy(curPizza);

					curPizza = Instantiate(pizzaPF);
					curPizza.name = "Pizza";
					curPizzaBehavior = curPizza.GetComponent<PizzaBehavior>();

					curOrder.GetComponent<OrderBehavior>().pizza = curPizza.GetComponent<PizzaBehavior>();
				}
			}

			uiText.text = "Score: " + score + "\n";
			uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";
			uiText.text += "Time Left: " + string.Format("{0}:{1:00}", (int)timeLeft / 60, (int)timeLeft % 60) + "\n";
		}

		else if (currentScene.name == "Results")
		{
			uiText = GameObject.Find("Stats").GetComponent<Text>();
			uiText.text = "Final Score:\n";
			uiText.text += score + "\n";
			uiText.text += "Pizzas Served:\n";
			uiText.text += pizzasServed + "\n";
		}
	}
}
