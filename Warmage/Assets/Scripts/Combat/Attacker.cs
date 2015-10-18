using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// A source of attacks that hits in a radius.
/// Collider component must be a trigger!
/// </summary>
[RequireComponent(typeof(Collider))]
public class Attacker : MonoBehaviour
{
	public Factions Faction = Factions.Player;

	public Transform MyTr { get; private set; }
	

	private List<Attackable> inRange = new List<Attackable>();


	protected virtual void Awake()
	{
		MyTr = transform;
		if (!GetComponent<Collider>().isTrigger)
		{
			Debug.LogError("Collider for attacker '" + gameObject.name + "' isn't a trigger");
		}
	}
	protected virtual void OnTriggerEnter(Collider other)
	{
		Attackable attck = other.GetComponent<Attackable>();
		if (attck != null && !inRange.Contains(attck))
		{
			inRange.Add(attck);
		}
	}
	protected virtual void OnTriggerExit(Collider other)
	{
		for (int i = 0; i < inRange.Count; ++i)
		{
			if (inRange[i].gameObject == other.gameObject)
			{
				inRange.RemoveAt(i);
				break;
			}
		}
	}


	public virtual void Attack(float force)
	{
		foreach (Attackable attck in inRange)
			if (attck.Faction != Faction)
				attck.OnAttacked(this, force);
	}
}