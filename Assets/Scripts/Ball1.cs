using UnityEngine;
using System.Collections;

public class Ball1 : MonoBehaviour {

	// public variables
	public float moveSpeed = 30f;
	public float jumpSpeed = 15f;
	public float angle = 80f;
	public float killTime = 4f;

	// private references
	private Rigidbody2D rb2d;
	
	// booleans
	private bool jumpButtonState = true;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//rb2d.gravityScale = 0;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

		Simulate();
		Destroy(gameObject, killTime);
	}
	
	void FixedUpdate()
	{
		Vector3 horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed;
		
		float velocityY = transform.InverseTransformDirection(rb2d.velocity).y;
		Vector3 verticalVelocity = transform.up * velocityY;

		// apply vertical velocity
		if (jumpButtonState)
		{
			jumpButtonState = false;
			verticalVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * jumpSpeed;
		}

		rb2d.velocity = horizontalVelocity + verticalVelocity;
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			jumpButtonState = true;
			jumpSpeed = jumpSpeed * 0.5f;
		}
	}

	void Simulate()
	{
		// create (and destroy) an empty gameobject
		GameObject simuation = new GameObject("Simulation 1");
		simuation.transform.position = transform.position;
		Destroy(simuation, killTime);

		// populate an array with gameobjects containing a sprite
		GameObject[] points = new GameObject[150];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
			points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = simuation.transform;
		}

		float time = 0f;
		float gravity = 9.81f;

		for (int i = 0; i < points.Length; i++)
		{
			time = i * 0.02f;
			float xPos = moveSpeed * time * Mathf.Cos(angle * Mathf.Deg2Rad);
			float yPos = jumpSpeed * time * Mathf.Sin(angle * Mathf.Deg2Rad) - 0.5f * gravity * time * time;
			Vector3 horzPos = points[i].transform.right * xPos;
			Vector3 vertPos = points[i].transform.up * yPos;
			points[i].transform.position = simuation.transform.position + horzPos + vertPos;
		}
	}
	
}
