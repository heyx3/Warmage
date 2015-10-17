using System;
using UnityEngine;


public class FactionConstants : Singleton<FactionConstants>
{
	/// <summary>
	/// Once a faction gets within this distance from its target faction,
	/// all minions in that faction target specific enemies.
	/// </summary>
	public float AttackDist = 20.0f;
}