using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Creates a Faction in a spherical area around this spawner's position, then destroys itself.
/// Assumes the prefab for creating minions has a capsule collider for the physics.
/// </summary>
public class FactionSpawner : MonoBehaviour
{
	public Factions Alignment;
	public GameObject MinionPrefab;

	public float Radius = 10.0f;
	public int MinMinions = 10,
			   MaxMinions = 15;

	public bool DestroyWholeObject = false;

	
	void OnDrawGizmos()
	{
		if (Terrain.activeTerrain != null)
		{
			Vector2 posHorz = transform.position.Horz();
			Vector3 posFull = posHorz.Full3D(Terrain.activeTerrain.SampleHeight(posHorz.Full3D(99999.0f)));

			Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.25f);
			Gizmos.DrawSphere(posFull, Radius);
		}
		else
		{
			Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
			Gizmos.DrawSphere(transform.position, Radius);
		}
	}

	void Start()
	{
		UnityEngine.Assertions.Assert.IsNotNull(MinionPrefab, "MinionPrefab is null");

		//Create the faction object.
		Faction f = new GameObject("Faction: " + Alignment).AddComponent<Faction>();
		f.Alignment = Alignment;

		//Get the capsule collider minions use so that they can be spawned away from solid geometry.
		CapsuleCollider cc = MinionPrefab.GetComponent<CapsuleCollider>();
		UnityEngine.Assertions.Assert.IsNotNull(cc, "Minion prefab '" + MinionPrefab.name +
													 "' doesn't have a capsule collider");

		Transform myTr = transform;

		int nMinions = UnityEngine.Random.Range(MinMinions, MaxMinions + 1);
		for (int i = 0; i < nMinions; ++i)
		{
			//Choose a random position.
			const int nTries = 999;
			int tryN = 0;
			Vector3 myPos = myTr.position;
			Vector3 pos = Vector3.zero;
			bool hitSomething = true;
			while (hitSomething && tryN < nTries)
			{
				tryN += 1;
				pos = GetRandomPosOnTerrain(myPos);
				pos.y += cc.radius + 0.1f;

				hitSomething = Physics.CheckCapsule(pos, new Vector3(pos.x, pos.y + cc.height, pos.z),
													cc.radius, Physics.DefaultRaycastLayers,
													QueryTriggerInteraction.Ignore);
			}

			UnityEngine.Assertions.Assert.IsTrue(tryN < nTries,
												 "Couldn't find a usable space for a minion");

			GameObject mo = Instantiate(MinionPrefab);
			Transform mTr = mo.transform;
			mTr.position = pos - cc.center;
			Minion mm = mo.GetComponent<Minion>();
			mm.Faction = Alignment;
			mm.Owner = f;
		}

		if (DestroyWholeObject)
		{
			Destroy(gameObject);
		}
		else
		{
			Destroy(this);
		}
	}
	private Vector3 GetRandomPosOnTerrain(Vector3 basePos)
	{
		//Create a random 2D position.
		float angle = UnityEngine.Random.Range(0.0f, Mathf.PI * 2.0f),
			  dist = UnityEngine.Random.value * Radius;
		Vector2 offset = new Vector2(dist, 0.0f).Rotate(angle);
		Vector3 offset3 = offset.Full3D(Terrain.activeTerrain.SampleHeight(offset.Full3D(9999999.0f)));
		
		//Get the correct height for it.
		Vector3 offsetPos = basePos + offset.Full3D(0.0f);
		return new Vector3(offsetPos.x, Terrain.activeTerrain.SampleHeight(offsetPos), offsetPos.z);
	}
}