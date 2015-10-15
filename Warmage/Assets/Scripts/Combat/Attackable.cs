using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Any object that can be attacked.
/// </summary>
[RequireComponent(typeof(Collider))]
public abstract class Attackable : MonoBehaviour
{
	public Factions Faction;

	public abstract void OnAttacked(Attacker attacker, float force);
}