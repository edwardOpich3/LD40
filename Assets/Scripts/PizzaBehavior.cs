using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
	public GameObject newPizza;
	public GameObject newOrder;

	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };

	public GameObject[] toppings;	// How many of what topping are on the pizza? The size should equal the max number of toppings
	public bool[] orderedToppings;	// How many of what topping NEED to be on the pizza?

	public bool isSliding;		// Is the pizza currently in motion?

	private Rigidbody2D pizzaRB;
	private CircleCollider2D pizzaCol;

	// Use this for initialization
	void Start ()
	{
		isSliding = false;
		toppings = new GameObject[8];
		orderedToppings = new bool[8];

		for(uint i = 0; i < toppings.Length; i++)
		{
			toppings[i] = transform.GetChild((int)i).gameObject;
			toppings[i].SetActive(false);

			orderedToppings[i] = Random.Range(0, 2) == 1;
		}

		pizzaRB = GetComponent<Rigidbody2D>();
		pizzaCol = GetComponent<CircleCollider2D>();

		transform.position = new Vector3(0.0f, -2.5f, 0.0f);
		transform.rotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.RightArrow) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			isSliding = true;

			pizzaRB.AddForce(new Vector2(1000.0f, 0.0f));
			pizzaRB.AddTorque(-50.0f);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			isSliding = true;

			pizzaRB.AddForce(new Vector2(-1000.0f, 0.0f));
			pizzaRB.AddTorque(50.0f);
		}

		// Crust
		if(Input.GetKeyDown(KeyCode.UpArrow) && !isSliding)
		{
			toppings[(int)TOPPINGS.CRUST].SetActive(true);
		}

		// Sauce
		if(Input.GetKeyDown(KeyCode.Q) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.SAUCE].SetActive(true);
		}

		// Cheese
		if(Input.GetKeyDown(KeyCode.W) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.CHEESE].SetActive(true);
		}

		// Pepperoni
		if(Input.GetKeyDown(KeyCode.E) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.PEPPERONI].SetActive(true);
		}

		// Anchovies
		if(Input.GetKeyDown(KeyCode.R) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.ANCHOVIES].SetActive(true);
		}

		// Mushrooms
		if(Input.GetKeyDown(KeyCode.T) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.MUSHROOMS].SetActive(true);
		}

		// Peppers
		if(Input.GetKeyDown(KeyCode.Y) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.PEPPERS].SetActive(true);
		}

		// Onions
		if(Input.GetKeyDown(KeyCode.U) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.ONIONS].SetActive(true);
		}

		if(isSliding)
		{
			pizzaRB.AddForce((-pizzaRB.velocity * pizzaRB.mass) / (Time.deltaTime * 50.0f));
		}

		// You sent the order
		if(Camera.main.WorldToViewportPoint(new Vector2(transform.position.x - pizzaCol.radius, 0.0f)).x > 1.0f)
		{
			GameObject myObject = Instantiate(newPizza);
			myObject.name = "Pizza";

			Destroy(gameObject);
		}

		// You recycled the pizza
		else if (Camera.main.WorldToViewportPoint(new Vector2(transform.position.x + pizzaCol.radius, 0.0f)).x < 0.0f)
		{
			GameObject myObject = Instantiate(newPizza);
			myObject.name = "Pizza";

			Destroy(gameObject);
		}
	}
}
