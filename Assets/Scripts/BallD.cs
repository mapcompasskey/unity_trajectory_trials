using UnityEngine;
using System.Collections;

public class BallD : MonoBehaviour {
	
	// public variables
	public float moveSpeed = 5f;
	public float jumpSpeed = 15f;
	public float killTime = 4f;
	public Vector3 origin = Vector3.zero;
	
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
	
	Vector3 ApplyGravity()
	{
		float gravity = -9.81f;

		Vector2 targetDir = (transform.position - origin).normalized;
		float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.AngleAxis(degrees, transform.forward);

		return transform.up * (gravity * Time.fixedDeltaTime);
	}
	
	void FixedUpdate()
	{
		Vector3 horizontalVelocity = transform.right * moveSpeed;
		
		float velocityY = transform.InverseTransformDirection(rb2d.velocity).y;
		Vector3 verticalVelocity = transform.up * velocityY;
		
		if (jumpButtonState)
		{
			jumpButtonState = false;
			verticalVelocity = transform.up * jumpSpeed;
		}
		
		verticalVelocity += ApplyGravity();
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
		GameObject simuation = new GameObject("Simulation D");
		simuation.transform.position = transform.position;
		Destroy(simuation, killTime);
		
		// populate an array with gameobjects containing a sprite
		GameObject[] points = new GameObject[100];
		for (int i = 0; i < points.Length; i++)
		{
			points[i] = new GameObject("dot");
			points[i].AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("dot");
			points[i].transform.localScale = new Vector3(0.1f, 0.1f, 0f);
			points[i].transform.parent = simuation.transform;
		}
		
		float time = 0f;
		float gravity = 9.81f;
		Quaternion rotation = Quaternion.identity;
		Vector3 position = simuation.transform.position;
		
		for (int i = 0; i < points.Length; i++)
		{
			points[i].transform.position = position;
			points[i].transform.rotation = rotation;

			Vector2 targetDir = (position - origin).normalized;
			float degrees = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90;
			points[i].transform.rotation = Quaternion.AngleAxis(degrees, points[i].transform.forward);

			time = i * 0.02f;
			float xPos = moveSpeed * time;
			float yPos = jumpSpeed * time - 0.5f * gravity * time * time;
			Vector3 horzPos = points[i].transform.right * xPos;
			Vector3 vertPos = points[i].transform.up * yPos;
			points[i].transform.position = simuation.transform.position + horzPos + vertPos;

			position = points[i].transform.position;
			rotation = points[i].transform.rotation;
		}
	}
	
}
