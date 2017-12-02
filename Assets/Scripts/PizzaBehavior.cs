using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaBehavior : MonoBehaviour
{
	public GameObject newPizza;

	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };
	public bool[] toppings;	// How many of what topping are on the pizza? The size should equal the max number of toppings

	private bool isSliding;		// Is the pizza currently in motion?

	private Rigidbody2D pizzaRB;
	private CircleCollider2D pizzaCol;
	private SpriteRenderer[] pizzaSprites;

	// Use this for initialization
	void Start ()
	{
		isSliding = false;
		toppings = new bool[8];

		pizzaRB = GetComponent<Rigidbody2D>();
		pizzaCol = GetComponent<CircleCollider2D>();
		pizzaSprites = GetComponents<SpriteRenderer>();
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

		if(Input.GetKeyUp(KeyCode.UpArrow) && !isSliding)
		{
			toppings[(int)TOPPINGS.CRUST] = true;
			pizzaSprites[(int)TOPPINGS.CRUST].enabled = true;
		}

		if(isSliding)
		{
			pizzaRB.AddForce((-pizzaRB.velocity * pizzaRB.mass) / (Time.deltaTime * 50.0f));
		}

		// You sent the order
		if(Camera.main.WorldToViewportPoint(new Vector2(transform.position.x - pizzaCol.radius, 0.0f)).x > 1.0f)
		{
			GameObject myObject = Instantiate(newPizza, new Vector3(0.0f, -2.5f, 0.0f), Quaternion.identity);
			myObject.name = "Pizza";

			Destroy(gameObject);
		}

		// You recycled the pizza
		else if (Camera.main.WorldToViewportPoint(new Vector2(transform.position.x + pizzaCol.radius, 0.0f)).x < 0.0f)
		{
			GameObject myObject = Instantiate(newPizza, new Vector3(0.0f, -2.5f, 0.0f), Quaternion.identity);
			Destroy(gameObject);

			myObject.name = "Pizza";
			PizzaBehavior test = myObject.GetComponent<PizzaBehavior>();
			bool[] test2 = test.toppings;
		}
	}
}
