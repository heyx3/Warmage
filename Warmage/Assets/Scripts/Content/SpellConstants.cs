using System;
using System.Collections.Generic;
using UnityEngine;


public class SpellConstants : Singleton<SpellConstants>
{
	[NonSerialized]
	public List<Spell> Spells;


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


	[NonSerialized]
	public bool UsedGroundPoundYet = false;

	public GameObject DiscoveryImage_GroundPound = null;


	public Transform DiscoveryImageMarker = null;


	public void CreateDiscoveryImage(GameObject prefab)
	{
		Transform tr = Instantiate(prefab).transform;
		tr.parent = DiscoveryImageMarker;
		tr.localPosition = Vector3.zero;
		tr.localRotation = Quaternion.AngleAxis(0.0f, Vector3.up);
	}


	private void AssertExists(GameObject go, string varName)
	{
		UnityEngine.Assertions.Assert.IsNotNull(go, "SpellConstants field '" + varName + "' is not set!");
	}
	protected override void Awake()
	{
		base.Awake();

		Spells = new List<Spell>()
		{
			new Spell_GroundPound(),
		};

		AssertExists(GroundPound.Prefab, "GroundPound.Prefab");
		AssertExists(DiscoveryImage_GroundPound, "DiscoveryImage_GroundPound");
	}
}