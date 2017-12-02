using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
	public GameObject newPizza;
	public GameObject newOrder;

	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };

	public GameObject[] toppings;	// How many of what topping are on the pizza? The size should equal the max number of toppings

	public bool isSliding;		// Is the pizza currently in motion?

	public Rigidbody2D pizzaRB;
	private CircleCollider2D pizzaCol;

	// Use this for initialization
	void Start ()
	{
		isSliding = false;
		toppings = new GameObject[8];
		for(uint i = 0; i < toppings.Length; i++)
		{
			toppings[i] = transform.GetChild((int)i).gameObject;
			toppings[i].SetActive(false);
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
		if(Input.GetKeyDown(KeyCode.Alpha1) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.SAUCE].SetActive(true);
		}

		// Cheese
		if(Input.GetKeyDown(KeyCode.Alpha2) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.CHEESE].SetActive(true);
		}

		// Pepperoni
		if(Input.GetKeyDown(KeyCode.Alpha3) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.PEPPERONI].SetActive(true);
		}

		// Anchovies
		if(Input.GetKeyDown(KeyCode.Alpha4) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.ANCHOVIES].SetActive(true);
		}

		// Mushrooms
		if(Input.GetKeyDown(KeyCode.Alpha5) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.MUSHROOMS].SetActive(true);
		}

		// Peppers
		if(Input.GetKeyDown(KeyCode.Alpha6) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.PEPPERS].SetActive(true);
		}

		// Onions
		if(Input.GetKeyDown(KeyCode.Alpha7) && !isSliding && toppings[(int)TOPPINGS.CRUST].activeInHierarchy)
		{
			toppings[(int)TOPPINGS.ONIONS].SetActive(true);
		}

		if(isSliding)
		{
			pizzaRB.AddForce((-pizzaRB.velocity * pizzaRB.mass) / (Time.deltaTime * 50.0f));
		}
	}
}
