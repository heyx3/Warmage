using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Spell_Lightning : Spell
{
	private static bool IsCasting(GestureController contr)
	{
		return contr.IsFingerPointing[1] &&
			   !contr.IsFingerPointing[3] &&
			   (contr.PalmTrackerPos.y / contr.MaxPalmForward) >= Consts.Lightning.ActivationDistanceLerp;
	}


	private LightningController lightningObject = null;
	private GestureController contr = null;


	public override bool CanUseSpell(GestureController controller)
	{
		return lightningObject == null && IsCasting(controller);
	}
	public override void CastSpell(GestureController controller)
	{
		//Find the enemy faction closest to where the player was pointing.
		float closestDist = float.MaxValue;
		Faction closestF = null;
		Vector3 fingerPos = controller.FingerTrackers[1].MyTransform.position,
				palmPos = controller.PalmTracker.MyTransform.position,
				terrPos = CastApproximateRayOnTerrain(fingerPos, (fingerPos - palmPos).normalized);
		Vector2 terrPosHorz = terrPos.Horz();
		foreach (Faction f in Faction.AllFactions[Factions.Enemies])
		{
			float tempDist = terrPosHorz.DistanceSqr(f.AveragePos);
			if (tempDist < closestDist)
			{
				closestDist = tempDist;
				closestF = f;
			}
		}
		if (closestF == null)
			return;

		//Create a lightning object there.
		lightningObject = GameObject.Instantiate(Consts.Lightning.Prefab).GetComponent<LightningController>();
		lightningObject.EnemyFaction = closestF;

		//Start looping lighting audio.
		controller.AudioSrc.clip = Consts.Lightning.AudioLoop;
		controller.AudioSrc.volume = 1.0f;
		controller.AudioSrc.loop = true;
		controller.AudioSrc.Play();


		//Update discovery image.
		if (!Consts.UsedLightningYet)
		{
			Consts.UsedLightningYet = true;
			Consts.CreateDiscoveryImage(Consts.DiscoveryImage_Lightning);
		}

		contr = controller;
	}
	private Vector3 CastApproximateRayOnTerrain(Vector3 rayPos, Vector3 rayDir)
	{
		Terrain terr = Terrain.activeTerrain;

		if (rayDir.y >= 0.0f)
		{
			return rayPos.Horz().Full3D(terr.SampleHeight(rayPos));
		}

		while (rayPos.y > terr.SampleHeight(rayPos))
		{
			rayPos += rayDir;
		}

		return rayPos - (rayDir * 0.5f);
	}

	public override void Update()
	{
		base.Update();

		if (lightningObject != null && !IsCasting(contr))
		{
			lightningObject.Reset();
			contr.AudioSrc.Stop();
		}
	}

	public override Spell MakeCopy()
	{
		return new Spell_Lightning();
	}
}