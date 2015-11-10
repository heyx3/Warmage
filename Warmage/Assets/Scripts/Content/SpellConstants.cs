using System;
using System.Collections.Generic;
using UnityEngine;


public class SpellConstants : Singleton<SpellConstants>
{
	[NonSerialized]
	public List<Spell> Spells;


	//TODO: Spells are below:
	/*
	 * Lightning (WIP, see below): Point with the index finger and move your hand forward. Shoots a bolt of lightning that arcs between minions of the same faction.
	 * Earthquake: Smack the ground with a closed fist. One large explosion on the surface of the terrain.
	 * Level 9: Flip the bird. Not sure what effect it should have yet. Something big.
	 * Magnifying Glass: Make a ring with your index finger and thumb, like an "OK" sign. Emits a beam out of that ring.
	 * Spidersilk: Make the spiderman finger sign. Spits out a web-like substance that knocks enemies down for a while.
	 * Force push: Open your fingers and push forward. Knocks enemies out of the way.
	 * Lift: Open your fingers and swipe upward. Only lifts enemies.
	 */


	[Serializable]
	public class GroundPoundData
	{
		public float RechargeTime = 2.0f;
		public float Lifetime = 3.0f;

		public float BurstInterval = 0.25f;

		public float MinForce = 1.0f,
					 ForceGrowth = 1.0f;
		public float MinSpeed = 1.0f,
					 SpeedGrowth = 0.05f;
		public float ParticleSpeedGrowth = 0.1f;

		public int ParticlesPerBurst = 100;

		public float MaxDistFromTerrain = 0.5f;

		public GameObject Prefab = null;
	}
	public GroundPoundData GroundPound = new GroundPoundData();

	[Serializable]
	public class LightningData
	{
		public float ActivationDistanceLerp = 0.3f;

		public float Force = 2.0f;
		public float MinDamageInterval = 0.5f,
					 MaxDamageInterval = 1.5f;

		public float JumpTime = 1.0f,
					 DisappearTime = 2.0f;

		public GameObject Prefab = null;
		public AudioClip AudioLoop = null;
	}
	public LightningData Lightning = new LightningData();


	[NonSerialized]
	public bool UsedGroundPoundYet = false,
				UsedLightningYet = false;

	public GameObject DiscoveryImage_GroundPound = null,
					  DiscoveryImage_Lightning = null;


	public Transform DiscoveryImageMarker = null;


	public void CreateDiscoveryImage(GameObject prefab)
	{
		Transform tr = Instantiate(prefab).transform;
		tr.parent = DiscoveryImageMarker;
		tr.localPosition = Vector3.zero;
		tr.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
	}


	private void AssertExists(object o, string varName)
	{
		UnityEngine.Assertions.Assert.IsNotNull(o, "SpellConstants field '" + varName + "' is not set!");
	}
	protected override void Awake()
	{
		base.Awake();

		Spells = new List<Spell>()
		{
			new Spell_GroundPound(),
			new Spell_Lightning(),
		};

		AssertExists(GroundPound.Prefab, "GroundPound.Prefab");
		AssertExists(Lightning.Prefab, "Lightning.Prefab");
		AssertExists(Lightning.AudioLoop, "Lightning.AudioLoop");
		
		AssertExists(DiscoveryImage_GroundPound, "DiscoveryImage_GroundPound");
		AssertExists(DiscoveryImage_Lightning, "DiscoveryImage_Lightning");
	}
}