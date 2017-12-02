using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBehavior : MonoBehaviour
{
	private enum TOPPINGS { CRUST, SAUCE, CHEESE, PEPPERONI, ANCHOVIES, MUSHROOMS, PEPPERS, ONIONS };

	public Rigidbody2D orderRB;

	public PizzaBehavior pizza;
	private TextMesh toppings;

	private bool isSliding;

	// Use this for initialization
	void Start ()
	{
		orderRB = GetComponent<Rigidbody2D>();

		isSliding = false;

		pizza = GameObject.Find("Pizza").GetComponent<PizzaBehavior>();
		toppings = transform.GetChild(0).gameObject.GetComponent<TextMesh>();
		toppings.text = "";

		if(!pizza.orderedToppings[(int)TOPPINGS.SAUCE])
		{
			toppings.text += "- Sauce\n";
		}
		if(!pizza.orderedToppings[(int)TOPPINGS.CHEESE])
		{
			toppings.text += "- Cheese\n";
		}
		if(pizza.orderedToppings[(int)TOPPINGS.PEPPERONI])
		{
			toppings.text += "'Roni\n";
		}
		if(pizza.orderedToppings[(int)TOPPINGS.ANCHOVIES])
		{
			toppings.text += "'Chovies\n";
		}
		if(pizza.orderedToppings[(int)TOPPINGS.MUSHROOMS])
		{
			toppings.text += "Mush.\n";
		}
		if(pizza.orderedToppings[(int)TOPPINGS.PEPPERS])
		{
			toppings.text += "Pep.\n";
		}
		if(pizza.orderedToppings[(int)TOPPINGS.ONIONS])
		{
			toppings.text += "On.\n";
		}

		transform.position = new Vector3(4.3f, -2.5f, 0.0f);
		transform.rotation = Quaternion.identity;
	}

	// Update is called once per frame
	void Update ()
	{
		if(pizza.isSliding && !isSliding)
		{
			isSliding = true;

			orderRB.AddForce(new Vector2(0.0f, -1000.0f));
		}
	}
}
