using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour {
	
	public GameObject ball;
	public float launchTime = 2f;

	private bool canLaunch = true;
	private float launchDelayTimer = 0f;
	
	void Update()
	{
		if (canLaunch)
		{
			canLaunch = false;
			GameObject.Instantiate(ball, transform.position, transform.rotation);
		}
		else
		{
			launchDelayTimer += Time.deltaTime;
			if (launchDelayTimer >= launchTime)
			{
				canLaunch = true;
				launchDelayTimer = 0;
			}
		}
	}
	
}
