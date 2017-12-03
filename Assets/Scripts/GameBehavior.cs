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
	private float orderTime;		// Time since the order was given, in seconds
	private int pizzasServed;
	public Text uiText;
	public Text mistakeText;
	public Text customersText;

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
						score += (int)levelPizzaTime[level];
						if((int)orderTime <= 0.0f)
						{
							mistakeText.text += "Too Slow!";
						}
						else if ((int)orderTime < levelPizzaTime[level] / 3.0f)
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

					score -= (int)(levelPizzaTime[level] - orderTime);

					pizzasServed++;
					if(pizzasServed >= levelCustomers[level])
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
					orderTime = levelPizzaTime[level];

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
				
			uiText.text = "Score: " + score + " / " + levelScores[level] + "\n";

			if(orderTime > 0.0f)
			{
				uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";
			}
			else
			{
				uiText.text += "Order Time: 0:00\n";
			}

			customersText.text = "Customers:\n\n" + (levelCustomers[level] - pizzasServed);
		}

		else if (currentScene.name == "Results" || currentScene.name == "WinGame")
		{
			uiText = GameObject.Find("Stats").GetComponent<Text>();
			uiText.text = "Final Score:\n";
			uiText.text += score + " / " + levelScores[level - 1] +  "\n";

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
					GameObject.Find("Canvas").transform.GetChild(1).GetComponent<Text>().text = "Congratulations!\nYou beat arcade mode!";
				}
			}
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
		orderTime = levelPizzaTime[level];
		pizzasServed = 0;

		uiText = GameObject.Find("Canvas").transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
		mistakeText = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(0).GetComponent<Text>();
		customersText = GameObject.Find("Canvas").transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();

		uiText.text = "Score: " + score + " / " + levelScores[level] + "\n";
		uiText.text += "Order Time: " + string.Format("{0}:{1:00}", (int)orderTime / 60, (int)orderTime % 60) + "\n";

		mistakeText.text = "Press Up Arrow to start!";

		customersText.text = "Customers:\n\n" + levelCustomers[level];

		curPizza = Instantiate(pizzaPF);
		curPizza.name = "Pizza";
		curPizzaBehavior = curPizza.GetComponent<PizzaBehavior>();

		curOrder = Instantiate(orderPF);
		curOrder.name = "Order Sheet";

		curOrder.GetComponent<OrderBehavior>().pizza = curPizza.GetComponent<PizzaBehavior>();

		SceneManager.sceneLoaded -= init;
	}
}
