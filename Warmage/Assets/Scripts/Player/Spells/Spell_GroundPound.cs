using System;
using System.Linq;
using UnityEngine;


/// <summary>
/// Triggered by swiping downwards with an open palm.
/// </summary>
public class Spell_GroundPound : RechargableSpell
{
	public Spell_GroundPound()
		: base(Consts.GroundPound.RechargeTime)
	{

	}


	public override bool CanUseSpell(GestureController controller)
	{
		return base.CanUseSpell(controller) &&
			   controller.IsFingerPointing.All(b => b) &&
			   controller.SwipeVerticalStrength < 0.0f;
	}

	public override void CastSpell(GestureController controller)
	{
		base.CastSpell(controller);

		Vector3 dir = controller.FingerTrackers[2].MyTransform.up;

		Transform gpTr = GameObject.Instantiate<GameObject>(Consts.GroundPound.Prefab).transform;
		
		//Position the ground-pound.
		Vector3 startPos = controller.PalmTracker.MyTransform.position;
		startPos.y = Terrain.activeTerrain.SampleHeight(startPos);
		gpTr.position = startPos;

		//Set the ground-pound's strength and direction.
		UnityEngine.Assertions.Assert.IsTrue(controller.SwipeVerticalStrength < 0.0f);
		GroundPoundController gpC = gpTr.gameObject.GetComponent<GroundPoundController>();
		gpC.Setup(-controller.SwipeVerticalStrength - GestureConstants.Instance.SwipeVelocityThreshold,
				  dir.Horz().normalized);

		//Update discovery image.
		if (!Consts.UsedGroundPoundYet)
		{
			Consts.UsedGroundPoundYet = true;
			Consts.CreateDiscoveryImage(Consts.DiscoveryImage_GroundPound);
		}
	}

	public override Spell MakeCopy()
	{
		return new Spell_GroundPound();
	}
}