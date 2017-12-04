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
	public float[] levelPizzaTime;			// How long in seconds can one take making a pizza per level?

	public bool[] orderedToppings;	// How many of what topping NEED to be on the pizza?

	private int score;
	private int minScore;			// Survival mode only. What's the score below which dipping will cause a game over?
	private float orderTime;		// Time since the order was given, in seconds
	private int pizzasServed;
	public Text uiText;
	public Text mistakeText;
	public Text customersText;

	private float timer;			// How much time since the game started? Used for survival.
	private float pizzaTime;		// Time for the order. In seconds. Used for survival.

	public int gameMode;				// Game mode; 0 = beginner, 1 = survival

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
			orderTime -= Time.deltaTime;
			timer += Time.deltaTime;
			if(timer >= 30.0f && gameMode == 1)
			{
				timer = 0.0f;
				level++;
				minScore += 100;

				if(level > 3)
				{
					level = 3;
					pizzaTime -= 1.0f;
					minScore += 50;
					if(pizzaTime < 3.0f)
					{
						pizzaTime = 3.0f;
						minScore += 100;
					}
				}
				else
				{
					pizzaTime = levelPizzaTime[level];
				}
			}

			if(gameMode == 1 && minScore > score)
			{
				SceneManager.LoadScene("WinGame");
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
								if(gameMode == 0)
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
								if(gameMode == 0)
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
						if(gameMode == 0)
						{
							score += (int)levelPizzaTime[level];
						}
						else
						{
							score += 20;
						}

						if((int)orderTime <= 0.0f)
						{
							mistakeText.text += "Too Slow!";
							if(gameMode == 1)
							{
								score -= 20;
							}
						}

						else if(gameMode == 0)
						{
							if ((int)orderTime < levelPizzaTime[level] / 3.0f)
							{
								mistakeText.text += "Good!";
							}
							else if ((int)orderTime < (2.0f * levelPizzaTime[level]) / 3.0f)
							{
								mistakeText.text += "Great!";
							}
							else
							{
								mistakeText.text += "Incredible!";
							}
						}
						else
						{
							if ((int)orderTime < pizzaTime / 3.0f)
							{
								mistakeText.text += "Good!";
							}
							else if ((int)orderTime < (2.0f * pizzaTime) / 3.0f)
							{
								mistakeText.text += "Great!";
							}
							else
							{
								mistakeText.text += "Incredible!";
							}
						}
					}

					score -= (int)(levelPizzaTime[level] - orderTime);

					pizzasServed++;
					if(pizzasServed >= levelCustomers[level] && gameMode == 0)
					{
						// Level complete! Add a remaining time bonus!
						level++;
						if(level <= maxLevel && score >= levelScores[level - 1])
						{
							SceneManager.LoadScene("Results");
						}
						else
						{
							SceneManager.LoadScene("WinGame");
						}

						return;
					}
						
					Destroy(curPizza);

					if(gameMode == 0)
					{
						orderTime = levelPizzaTime[level];
					}
					else
					{
						orderTime = pizzaTime;
					}

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
				
			if(gameMode == 0)
			{
				uiText.text = "Score: " + score + " / " + levelScores[level] + "\n";
			}
			else
			{
				uiText.text = "Score: " + score + " / " + minScore + "\n";
			}

			if(orderTime > 0.0f)
			{
				uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";
			}
			else
			{
				uiText.text += "Order Time: 0:00\n";
			}

			if(gameMode == 0)
			{
				customersText.text = "Customers:\n\n" + (levelCustomers[level] - pizzasServed);
			}
			else
			{
				customersText.text = "Customers:\n\n" + float.PositiveInfinity;
			}
		}

		else if (currentScene.name == "Results" || currentScene.name == "WinGame")
		{
			uiText = GameObject.Find("Stats").GetComponent<Text>();
			uiText.text = "Final Score:\n";

			if(gameMode == 0)
			{
				uiText.text += score + " / " + levelScores[level - 1] +  "\n";
			}
			else
			{
				uiText.text += score + "\n";
			}

			if(gameMode == 0)
			{
				if(score < levelScores[level - 1])
				{
					GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>().text = "Sorry, your score is too low to proceed.\n";
				}
				else
				{
					if(currentScene.name == "Results")
					{
						GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>().text = "Ready for the next level?";
					}
					else
					{
						GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>().text = "Congratulations!\nYou win!\nNow try Survival!";
					}
				}
			}
		}

		else if (currentScene.name == "Cutscene")
		{
			if(Input.GetMouseButtonDown(0))
			{
				SceneManager.LoadScene("Gameplay");
				SceneManager.sceneLoaded += init;
			}

			if(!uiText)
			{
				uiText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
			}

			if(uiText.text == "")
			{
				if(level == 0)
				{
					uiText.text = "Hey, welcome to the shop! Business around here is pretty slow, so you don't have too much to worry about.\n" +
						"Just make the pizzas correctly, that's all I, and the customers, ask.\n" +
						"We don't have many toppings here, just pepperoni, so it shouldn't be too hard to mess up anyway!\n" +
						"Good luck out there!\n\n";

					uiText.text += "(Press Left mouse button to continue.)";
				}
				else if(level == 1)
				{
					uiText.text = "Good job making those pizzas! Not bad at all for your first time on the job.\n" +
					"We're getting some more toppings tomorrow though due to popular demand! Onions and Peppers.\n" +
					"Hopefully that won't be too tough for you to juggle. Oh, and try not to mix up orders asking " +
						"for pepperoni with orders asking for peppers!\n\n";

					uiText.text += "(Press Left mouse button to continue.)";
				}
				else if (level == 2)
				{
					uiText.text = "You've been great so far! Business is picking up quite a bit around here.\n" +
						"Tomorrow we're getting even more toppings! Anchovies and Mushrooms.\n" +
						"Also, with the extra customers, you might have to work a bit quicker to make sure they're all satisifed!\n" +
						"You can handle that, right?\n\n";

					uiText.text += "(Press Left mouse button to continue.)";
				}
				else if (level == 3)
				{
					uiText.text = "Our shop's on the web now with some remote ordering software! Isn't that great?\n" +
						"That means even more customers! The ordering software comes with complete customizability of pizzas.\n" +
						"I tested, and if someone wanted to, they could even order a pizza with no cheese or sauce!\n" +
						"Not that they'd want to of course. Anyway, good luck today!\n\n";

					uiText.text += "(Press Left mouse button to continue.)";
				}
			}
		}
	}

	public void init(Scene scene, LoadSceneMode mode)
	{
		gameStarted = false;

		timer = 0.0f;

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
		orderTime = levelPizzaTime[level];
		pizzaTime = levelPizzaTime[level];
		pizzasServed = 0;

		if(gameMode == 1)
		{
			minScore = 0;
		}

		uiText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		mistakeText = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
		customersText = GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

		if(gameMode == 0)
		{
			uiText.text = "Score: " + score + " / " + levelScores[level] + "\n";
		}
		else
		{
			uiText.text = "Score: " + score + " / " + minScore + "\n";
		}

		uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";

		mistakeText.text = "Press Up Arrow to start!";

		if(gameMode == 0)
		{
			customersText.text = "Customers:\n\n" + levelCustomers[level];
		}
		else
		{
			customersText.text = "Customers:\n\n" + float.PositiveInfinity;
		}

		if(gameMode == 0)
		{
			if(level < 1)
			{
				GameObject.Find("Level 2").SetActive(false);
			}
			if (level < 2)
			{
				GameObject.Find("Level 3").SetActive(false);
			}
		}

		curPizza = Instantiate(pizzaPF);
		curPizza.name = "Pizza";
		curPizzaBehavior = curPizza.GetComponent<PizzaBehavior>();

		curOrder = Instantiate(orderPF);
		curOrder.name = "Order Sheet";

		curOrder.GetComponent<OrderBehavior>().pizza = curPizza.GetComponent<PizzaBehavior>();

		SceneManager.sceneLoaded -= init;
	}
}
