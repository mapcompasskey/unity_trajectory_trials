using UnityEngine;
using System.Collections;

public class Ball5 : MonoBehaviour {
	
	// public variables
	public float moveSpeed = 5f;
	public float jumpSpeed = 15f;
	public float killTime = 4f;
	
	// private references
	private Rigidbody2D rb2d;
	
	// floats
	private float timeElapsed = 0f;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.gravityScale = 0;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		
		Simulate();
		Destroy(gameObject, killTime);
	}
	
	void FixedUpdate()
	{
		float gravity = -9.81f;
		timeElapsed += Time.deltaTime;

		Vector3 horizontalVelocity = transform.right * moveSpeed;
		Vector3 verticalVelocity = transform.up * (jumpSpeed + (gravity * timeElapsed));
		transform.position += (horizontalVelocity + verticalVelocity) * Time.deltaTime;
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			timeElapsed = 0;
			jumpSpeed = jumpSpeed * 0.5f;
		}
	}
	
	void Simulate()
	{
		// create (and destroy) an empty gameobject
		GameObject simuation = new GameObject("Simulation 5");
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
		
		float time = 0.02f;
		float gravity = -9.81f;
		Vector3 position = simuation.transform.position;
		Vector3 horzVelocity = Vector3.zero;
		Vector3 vertVelocity = Vector3.zero;

		/*
		for (int i = 0; i < points.Length; i++)
		{
			points[i].transform.position = position;

			points[i].transform.position += (horzVelocity + vertVelocity) * time;
			horzVelocity = points[i].transform.right * moveSpeed;
			vertVelocity = points[i].transform.up * (jumpSpeed + (gravity * time * i));
			
			position = points[i].transform.position;
		}
		*/

		float timePassed = 0f;

		for (int i = 0; i < points.Length; i++)
		{
			timePassed += time;
			points[i].transform.position = position;

			horzVelocity = points[i].transform.right * moveSpeed;
			vertVelocity = points[i].transform.up * (jumpSpeed + (gravity * timePassed));
			points[i].transform.position += (horzVelocity + vertVelocity) * time;

			position = points[i].transform.position;
		}
	}
	
}
