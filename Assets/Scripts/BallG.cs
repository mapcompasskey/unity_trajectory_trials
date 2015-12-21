using UnityEngine;
using System.Collections;

public class BallG : MonoBehaviour {
	
	// public variables
	public float moveSpeed = 5f;
	public float jumpSpeed = 15f;
	public float killTime = 4f;
	public Vector3 origin = Vector3.zero;
	
	// private references
	private Rigidbody2D rb2d;
	
	// booleans
	private bool jumpButtonState = true;

	// floats
	private float timeElapsed = 0f;
	Quaternion rotation = Quaternion.identity;
	
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.drag = 0f;
		rb2d.angularDrag = 0f;
		rb2d.gravityScale = 0f;
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

		Simulate();
		Destroy(gameObject, killTime);
	}

	void GetRotation()
	{
		Vector2 targetDir = (transform.position - origin).normalized;
		float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
		rotation = Quaternion.Euler(0, 0, degrees);
	}
	
	void FixedUpdate()
	{
		GetRotation();

		Vector3 horizontalVelocity = transform.right * moveSpeed;

		float gravity = -9.81f;
		timeElapsed += Time.fixedDeltaTime;

		Vector3 verticalVelocity = transform.up * (jumpSpeed + (gravity * timeElapsed));

		if (jumpButtonState)
		{
			jumpButtonState = false;
			verticalVelocity = (rotation * transform.up) * jumpSpeed;
		}

		rb2d.velocity = rotation * (horizontalVelocity + verticalVelocity);
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Ground")
		{
			jumpButtonState = true;
			timeElapsed = 0f;
			jumpSpeed = jumpSpeed * 0.5f;
		}
	}
	
	void Simulate()
	{
		// create (and destroy) an empty gameobject
		GameObject simuation = new GameObject("Simulation G");
		simuation.transform.position = transform.position;
		Destroy(simuation, killTime);
		
		// populate an array with gameobjects containing a sprite
		GameObject[] points = new GameObject[600];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
			points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = simuation.transform;
		}
		
		float time = 0.005f;
		float gravity = -9.81f;
		Quaternion rotation = Quaternion.identity;
		Vector3 position = simuation.transform.position;
		Vector3 horzVelocity = Vector3.zero;
		Vector3 vertVelocity = Vector3.zero;
		
		float timePassed = 0f;
		
		for (int i = 0; i < points.Length; i++)
		{
			/*
			timePassed += time;
			points[i].transform.position = position;
			
			Vector2 targetDir = (position - origin).normalized;
			float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			points[i].transform.rotation = Quaternion.AngleAxis(degrees, points[i].transform.forward);
			
			horzVelocity = points[i].transform.right * moveSpeed2;
			vertVelocity = points[i].transform.up * (jumpSpeed2 + (gravity * timePassed));
			points[i].transform.position += (horzVelocity + vertVelocity) * time;
			
			position = points[i].transform.position;
			*/

			timePassed += time;
			points[i].transform.position = position;

			Vector2 targetDir = (position - origin).normalized;
			float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			rotation = Quaternion.Euler(0, 0, degrees);

			horzVelocity = points[i].transform.right * moveSpeed;
			vertVelocity = points[i].transform.up * (jumpSpeed + (gravity * timePassed));

			points[i].transform.position += rotation * (horzVelocity + vertVelocity) * time;

			position = points[i].transform.position;
		}
		
	}
	
}
