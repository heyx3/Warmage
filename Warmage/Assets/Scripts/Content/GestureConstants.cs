using System;
using UnityEngine;


/// <summary>
/// Contains all math constants for gesture recognition.
/// </summary>
public class GestureConstants : Singleton<GestureConstants>
{
	public float FingerTouchMaxDist = 0.1f;
	public float FingerPointAtPalmMinDot = 0.5f;

	public float SwipeGestureDuration = 0.2f;
	public float SwipeVelocityThreshold = 100.0f;

	public float SwipeWait = 0.5f;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}