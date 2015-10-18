using System;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class GroundPoundController : Attacker
{
	private static SpellConstants.GroundPoundData Consts { get { return SpellConstants.Instance.GroundPound; } }


	/// <summary>
	/// A value starting from 0 indicating how powerful this particular ground pound was.
	/// </summary>
	[NonSerialized]
	public float Strength = 0.0f;
	/// <summary>
	/// The horizontal direction this ground-pound travels in.
	/// </summary>
	[NonSerialized]
	public Vector2 Dir = Vector2.zero;

	
	public float Speed
	{
		get
		{
			return Consts.MinSpeed + (Strength * Consts.SpeedGrowth);
		}
	}
	public float Force
	{
		get
		{
			return Consts.MinForce + (Strength * Consts.ForceGrowth);
		}
	}


	private ParticleSystem parts;
	private float timeTillEmit;


	public void Setup(float strength, Vector2 dir)
	{
		Strength = strength;
		Dir = dir;
	}

	protected override void Awake()
	{
		base.Awake();

		parts = GetComponent<ParticleSystem>();
		timeTillEmit = Consts.BurstInterval;

		gameObject.AddComponent<KillAfterTime>().TimeTillDeath = Consts.Lifetime;
	}
	void Update()
	{
		//Update timing.
		timeTillEmit -= Time.deltaTime;
		if (timeTillEmit <= 0.0f)
		{
			timeTillEmit += Consts.BurstInterval;
			parts.Emit(Consts.ParticlesPerBurst);
			Attack(Force);
		}

		//Move along the surface of the terrain.
		Vector3 newPos = MyTr.position;
		newPos += (Dir * Time.deltaTime * Speed).Full3D(0.0f);
		newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
		MyTr.position = newPos;
	}
}