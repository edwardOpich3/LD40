using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };
	private char[] toppings;	// How many of what topping are on the pizza? The size should equal the max number of toppings

	private bool isSliding;		// Is the pizza currently in motion?

	private Rigidbody2D pizzaRB;
	private CircleCollider2D pizzaCol;

	// Use this for initialization
	void Start ()
	{
		isSliding = false;
		toppings = new char[8];

		pizzaRB = GetComponent<Rigidbody2D>();
		pizzaCol = GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.RightArrow) && !isSliding)
		{
			isSliding = true;

			pizzaRB.AddForce(new Vector2(1000.0f, 0.0f));
			pizzaRB.AddTorque(-50.0f);
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow) && !isSliding)
		{
			isSliding = true;

			pizzaRB.AddForce(new Vector2(-1000.0f, 0.0f));
			pizzaRB.AddTorque(50.0f);
		}

		if(isSliding)
		{
			pizzaRB.AddForce((-pizzaRB.velocity * pizzaRB.mass) / (Time.deltaTime * 50.0f));
		}

		// You sent the order
		if(Camera.main.WorldToViewportPoint(new Vector2(transform.position.x - pizzaCol.radius, 0.0f)).x > 1.0f)
		{
			Destroy(gameObject);
		}

		// You recycled the pizza
		else if (Camera.main.WorldToViewportPoint(new Vector2(transform.position.x + pizzaCol.radius, 0.0f)).x < 0.0f)
		{
			Destroy(gameObject);
		}
	}
}
