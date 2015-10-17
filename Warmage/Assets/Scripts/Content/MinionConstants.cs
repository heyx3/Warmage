using System;
using System.Collections.Generic;
using UnityEngine;


public class MinionConstants : Singleton<MinionConstants>
{
	public float WalkSpeed = 3.0f;

	public float PunchForce = 1.0f;

	public float TurnLerpRate = 0.15f;
	public float MaxDistSqrFromTarget = 1.0f;
}