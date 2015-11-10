using System;
using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(AudioSource))]
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
	private AudioSource audSrc;
	private float timeTillEmit;
	private float baseSpeed;


	public void Setup(float strength, Vector2 dir)
	{
		Strength = strength;
		Dir = dir;
		
		audSrc.Play();
	}

	protected override void Awake()
	{
		base.Awake();

		audSrc = GetComponent<AudioSource>();
		parts = GetComponent<ParticleSystem>();
		timeTillEmit = Consts.BurstInterval;
		baseSpeed = parts.startSpeed;

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
			audSrc.volume = Mathf.Lerp(0.2f, 1.0f, Mathf.Clamp01(Strength / 30.0f));
			audSrc.Play();
		}

		//Move along the surface of the terrain.
		Vector3 newPos = MyTr.position;
		newPos += (Dir * Time.deltaTime * Speed).Full3D(0.0f);
		newPos.y = Terrain.activeTerrain.SampleHeight(newPos);
		MyTr.position = newPos;

		parts.startSpeed = Strength * Consts.ParticleSpeedGrowth;
	}
}