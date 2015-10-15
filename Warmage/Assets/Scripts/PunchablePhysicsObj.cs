using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PunchablePhysicsObj : Attackable
{
	public float ForceMultiplier = 1.0f;

	private Rigidbody rigd;


	void Awake()
	{
		rigd = GetComponent<Rigidbody>();
	}


	public override void OnAttacked(Attacker attacker, float force)
	{
		Vector3 awayFromHitter = (rigd.position - attacker.MyTr.position).normalized;
		rigd.velocity += force * ForceMultiplier * awayFromHitter;
	}
}