using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Defines a type of spell to be cast.
/// Each instance will "belong" to a specific hand and will only recieve events from that hand.
/// </summary>
public abstract class Spell
{
	protected static SpellConstants Consts { get { return SpellConstants.Instance; } }


	/// <summary>
	/// Gets whether this spell is ready to be used by the given hand.
	/// </summary>
	public abstract bool CanUseSpell(GestureController controller);
	/// <summary>
	/// Starts a casting of this spell by the given hand.
	/// </summary>
	public abstract void CastSpell(GestureController controller);

	/// <summary>
	/// Called every frame.
	/// </summary>
	public virtual void Update() { }

	/// <summary>
	/// Makes a copy of this spell definition.
	/// </summary>
	public abstract Spell MakeCopy();
}