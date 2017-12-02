using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };
	private char[] toppings;	// How many of what topping are on the pizza? The size should equal the max number of toppings

	private Rigidbody2D pizzaRB;

	// Use this for initialization
	void Start ()
	{
		toppings = new char[8];

		pizzaRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(Input.KeyCode.LeftArrow))
		{
			pizzaRB.AddForce(Vector2(10.0f, 0.0f));
		}
	}
}
