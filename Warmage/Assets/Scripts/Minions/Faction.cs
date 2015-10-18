using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// An army of Minion objects.
/// This component destroys itself once the number of minions in its army reaches 0.
/// </summary>
public class Faction : MonoBehaviour
{
	public static Dictionary<Factions, List<Faction>> AllFactions = new Dictionary<Factions, List<Faction>>()
	{
		{ Factions.Allies, new List<Faction>() },
		{ Factions.Enemies, new List<Faction>() },
	};
	public static IEnumerator<Faction> GetAllEnemies(Factions myFaction)
	{
		var val = AllFactions
					.Where(kvp => (kvp.Key != myFaction))
					.Select(kvp => kvp.Value)
					.Combine<Faction, List<Faction>>();
		val.MoveNext();
		return val;
	}


	private static FactionConstants Consts { get { return FactionConstants.Instance; } }


	public Factions Alignment
	{
		get { return alignment; }
		set
		{
			AllFactions[alignment].Remove(this);
			alignment = value;
			AllFactions[alignment].Add(this);
		}
	}
	private Factions alignment = Factions.Allies;
	
	public Faction Target;

	[NonSerialized]
	public List<Minion> Minions = new List<Minion>();
	private Vector3[] minionPoses;


	/// <summary>
	/// The average of all minion positions.
	/// Note that this isn't updated every frame for performance reasons.
	/// </summary>
	public Vector2 AveragePos { get; private set; }


	void Awake()
	{
		AllFactions[Alignment].Add(this);
	}
	void Start()
	{
		minionPoses = new Vector3[Minions.Count];
		StartCoroutine(UpdateLogicCoroutine());
	}

	void OnDestroy()
	{
		AllFactions[Alignment].Remove(this);
	}

	private System.Collections.IEnumerator UpdateLogicCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 1.0f));


			//Get each minion's position and compute the average.
			if (minionPoses.Length != Minions.Count)
			{
				minionPoses = new Vector3[Minions.Count];
			}
			AveragePos = Vector2.zero;
			for (int i = 0; i < Minions.Count; ++i)
			{
				minionPoses[i] = Minions[i].MyTr.position;
				AveragePos += minionPoses[i].Horz();
			}
			AveragePos /= (float)Minions.Count;


			//If no faction is currently targeted, pick a new enemy arbitrarily.
			if (Target == null)
			{
				Target = GetAllEnemies(Alignment).Current;
			}


			//If no faction is available to target, go idle.
			if (Target == null)
			{
				for (int i = 0; i < Minions.Count; ++i)
					Minions[i].WalkTargetPos = minionPoses[i].Horz();
			}
			//Otherwise, make every minion in this faction seek towards the target faction.
			else
			{
				//If this faction is close enough, seek directly towards minions in the target faction.
				if (Target.Minions.Count > 0 && Target.minionPoses.Length > 0 &&
					AveragePos.DistanceSqr(Target.AveragePos) < Consts.AttackDist * Consts.AttackDist)
				{
					for (int i = 0; i < Minions.Count; ++i)
					{
						int enemyI = i % Target.Minions.Count;
						Vector3 enemyPos = Target.minionPoses[enemyI];
						
						Minions[i].WalkTargetPos = enemyPos.Horz();
						
						//If close enough, attack.
						if (minionPoses[i].DistanceSqr(enemyPos) < Consts.MinionAttackDist * Consts.MinionAttackDist)
						{
							if (!Minions[i].IsAttacking)
								Minions[i].StartAttacking();
						}
						else
						{
							if (Minions[i].IsAttacking)
								Minions[i].StopAttacking();
						}
					}
				}
				//Otherwise, just move towards the enemy faction while preserving formation.
				else
				{
					for (int i = 0; i < Minions.Count; ++i)
					{
						Vector2 delta = minionPoses[i].Horz() - AveragePos;
						Minions[i].WalkTargetPos = Target.AveragePos + delta;
					}
				}
			}
		}
	}
	void Update()
	{
		if (Minions.Count == 0)
		{
			Destroy(this);
			return;
		}
	}
}