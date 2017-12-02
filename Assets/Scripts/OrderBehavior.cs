using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehavior : MonoBehaviour
{
	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };

	public Rigidbody2D orderRB;
	private BoxCollider2D orderCol;

	public PizzaBehavior pizza;
	private TextMesh toppings;

	private bool isSliding;
	private bool[] orderedToppings;

	// Use this for initialization
	void Start ()
	{
		orderRB = GetComponent<Rigidbody2D>();
		orderCol = GetComponent<BoxCollider2D>();

		isSliding = false;

		toppings = transform.GetChild(0).gameObject.GetComponent<TextMesh>();
		toppings.text = "";

		orderedToppings = GameObject.Find("Game Manager").GetComponent<GameBehavior>().orderedToppings;

		if(!orderedToppings[(int)TOPPINGS.SAUCE])
		{
			toppings.text += "- Sauce\n";
		}
		if(!orderedToppings[(int)TOPPINGS.CHEESE])
		{
			toppings.text += "- Cheese\n";
		}

		bool hasToppings = false;

		if(orderedToppings[(int)TOPPINGS.PEPPERONI])
		{
			toppings.text += "'Roni\n";
			hasToppings = true;
		}
		if(orderedToppings[(int)TOPPINGS.ANCHOVIES])
		{
			toppings.text += "'Chovies\n";
			hasToppings = true;
		}
		if(orderedToppings[(int)TOPPINGS.MUSHROOMS])
		{
			toppings.text += "Mush.\n";
			hasToppings = true;
		}
		if(orderedToppings[(int)TOPPINGS.PEPPERS])
		{
			toppings.text += "Pep.\n";
			hasToppings = true;
		}
		if(orderedToppings[(int)TOPPINGS.ONIONS])
		{
			toppings.text += "On.\n";
			hasToppings = true;
		}

		if(!hasToppings)
		{
			toppings.text = "Plain\n";
			if(!orderedToppings[(int)TOPPINGS.SAUCE])
			{
				toppings.text += "- Sauce\n";
			}
			if(!orderedToppings[(int)TOPPINGS.CHEESE])
			{
				toppings.text += "- Cheese\n";
			}
		}

		transform.position = new Vector3(4.3f, -2.5f, 0.0f);
		transform.rotation = Quaternion.identity;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Camera.main.WorldToViewportPoint(new Vector2(0.0f , transform.position.y + 2.56f)).y < 0.0f)
		{
			Destroy(gameObject);
		}
	}
}
