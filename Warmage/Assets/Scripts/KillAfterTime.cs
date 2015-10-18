using System;
using UnityEngine;


public class KillAfterTime : MonoBehaviour
{
	public float TimeTillDeath = 5.0f;
	private float elapsed = 0.0f;

	void Update()
	{
		elapsed += Time.deltaTime;
		if (elapsed >= TimeTillDeath)
		{
			Destroy(gameObject);
		}
	}
}