using System;
using UnityEngine;


/// <summary>
/// A type of spell that must wait a certain amount of time between each activation.
/// </summary>
public abstract class RechargableSpell : Spell
{
	public float TimeTillUsable = 0.0f,
				 RechargeTime;


	public RechargableSpell(float rechargeTime) { RechargeTime = rechargeTime; }


	public override bool CanUseSpell(GestureController controller)
	{
		return (TimeTillUsable <= 0.0f);
	}
	public override void CastSpell(GestureController controller)
	{
		TimeTillUsable = RechargeTime;
	}

	public override void Update()
	{
		base.Update();
		TimeTillUsable -= Time.deltaTime;
	}
}