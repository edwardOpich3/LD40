using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{
	public GameObject pizzaPF;		// Pizza Prefab
	public GameObject orderPF;		// Order Prefab

	// Here there be level things! Adjust accordingly!
	public int level;						// Current level; used to determine how long you have to make a pizza, or which toppings are available
	public int maxLevel;					// The last level; if level passes above this, the game is over!
	public bool[][] levelToppings;			// Which toppings are available on which level?
	public int[] levelCustomers;			// How many customers are there each level?
	public int[] levelScores;				// What's the minimum score needed to pass each level?
	public float[] levelLengths;			// What's the length in seconds of each level?
	public float[] levelPizzaTime;			// How long in seconds can one take making a pizza per level?

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
		// Here's where you define the toppings that are available for each level!
		levelToppings = new bool[][] { new bool[8], new bool[8], new bool[8], new bool[8] };
		levelToppings[0] = new bool[8] { false, false, false, true, false, false, false, false };
		levelToppings[1] = new bool[8] { false, false, false, true, false, false, true, true };
		levelToppings[2] = new bool[8] { false, false, false, true, true, true, true, true };
		levelToppings[3] = new bool[8] { false, true, true, true, true, true, true, true };

		DontDestroyOnLoad(gameObject);

		level = 0;

		orderedToppings = new bool[8];
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
				// Level over. Subtract appropriate points for the remaining customers.
				timeLeft = 0.0f;
				score -= (levelCustomers[level] - pizzasServed) * 10;

				level++;
				if(level <= maxLevel)
				{
					SceneManager.LoadScene("Results");
				}
				else
				{
					SceneManager.LoadScene("WinGame");
				}
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
						if((int)orderTime > levelPizzaTime[level])
						{
							mistakeText.text += "Too Slow!";
						}
						else if ((int)orderTime > (2.0f * levelPizzaTime[level]) / 3.0f)
						{
							mistakeText.text += "Good!";
						}
						else if ((int)orderTime > levelPizzaTime[level] / 3.0f)
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
					if(pizzasServed >= levelCustomers[level])
					{
						// Level complete! Add a remaining time bonus!
						level++;
						score += (int)timeLeft;
						if(level <= maxLevel)
						{
							SceneManager.LoadScene("Results");
						}
						else
						{
							SceneManager.LoadScene("WinGame");
						}
					}

					Destroy(curPizza);
					orderTime = 0.0f;

					for(uint i = 1; i < orderedToppings.Length; i++)
					{
						orderedToppings[i] = (Random.Range(0, 2) == 1) && levelToppings[level][i];
						if(i < 3)
						{
							orderedToppings[i] = !orderedToppings[i];
						}
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

	public void init(Scene scene, LoadSceneMode mode)
	{
		gameStarted = false;

		for(uint i = 1; i < orderedToppings.Length; i++)
		{
			orderedToppings[i] = (Random.Range(0, 2) == 1) && levelToppings[level][i];
			if(i < 3)
			{
				orderedToppings[i] = !orderedToppings[i];
			}
		}
		orderedToppings[0] = true;

		score = 0;
		orderTime = 0.0f;
		timeLeft = levelLengths[level];
		pizzasServed = 0;

		uiText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		mistakeText = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();

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

		SceneManager.sceneLoaded -= init;
	}
}
