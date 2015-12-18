using UnityEngine;
using System.Collections;

public class Ball2 : MonoBehaviour {
	
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
		rb2d.gravityScale = 0;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

		Simulate();
		Destroy(gameObject, killTime);
	}
	
	void ApplyGravity()
	{
		float gravity = -9.81f;
		rb2d.AddForce(transform.up * gravity);
	}
	
	void FixedUpdate()
	{
		ApplyGravity();

		Vector3 horizontalVelocity = transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed;
		
		float velocityY = transform.InverseTransformDirection(rb2d.velocity).y;
		Vector3 verticalVelocity = transform.up * velocityY;

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
		GameObject simuation = new GameObject("Simulation 2");
		simuation.transform.position = transform.position;
		Destroy(simuation, killTime);
		
		// populate an array with gameobjects containing a sprite
		GameObject[] points = new GameObject[35];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
			points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = simuation.transform;
		}
		
		float time = 0.09f;
		float gravity = -9.81f;
		Vector3 position = simuation.transform.position;
		Vector3 horzVelocity = Vector3.zero;
		Vector3 vertVelocity = transform.up * Mathf.Sin(angle * Mathf.Deg2Rad) * jumpSpeed;
		
		for (int i = 0; i < points.Length; i++)
		{
			points[i].transform.position = position;
			
			horzVelocity = points[i].transform.right * Mathf.Cos(angle * Mathf.Deg2Rad) * moveSpeed;
			vertVelocity = points[i].transform.up * (jumpSpeed + (gravity * time * i));
			points[i].transform.position += (horzVelocity + vertVelocity) * time;
			
			position = points[i].transform.position;
		}
		
	}

}
