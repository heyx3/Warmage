using System;
using System.Collections;
using UnityEngine;


public class LightningController : Attacker
{
	private static SpellConstants.LightningData Consts { get { return SpellConstants.Instance.Lightning; } }


	public Faction EnemyFaction;
	private int enemyIndex = 0;
	private Minion enemy { get { return EnemyFaction.Minions[enemyIndex]; } }

	private float elapsed, timeTillNext;

	private LightningController nextEnemyLightning = null;


	/// <summary>
	/// Prevents this lightning and all connected ones from wearing out.
	/// </summary>
	public void Reset()
	{
		elapsed = 0.0f;

		if (nextEnemyLightning != null)
			nextEnemyLightning.Reset();
	}

	void Start()
	{
		StartCoroutine(DamageEnemyCoroutine());
		elapsed = 0.0f;
		timeTillNext = Consts.JumpTime;

		MyTr.position = enemy.MyTr.position;
	}

	void Update()
	{
		elapsed += Time.deltaTime;

		//If enemies died, destroy all lightning after this one.
		if (EnemyFaction.Minions.Count <= enemyIndex)
		{
			DestroyAllFromHere();
		}
		//Otherwise, see if this lightning is done being applied.
		else if (elapsed > Consts.DisappearTime)
		{
			Destroy(gameObject);
		}
		//Otherwise, see if it's time to create the next lightning in the chain.
		else if (nextEnemyLightning == null && EnemyFaction.Minions.Count > enemyIndex + 1)
		{
			timeTillNext -= Time.deltaTime;
			if (timeTillNext <= 0.0f)
			{
				nextEnemyLightning = Instantiate(Consts.Prefab).GetComponent<LightningController>();
				nextEnemyLightning.EnemyFaction = EnemyFaction;
				nextEnemyLightning.enemyIndex = enemyIndex;
			}
		}
	}
	private void DestroyAllFromHere()
	{
		if (nextEnemyLightning != null)
			nextEnemyLightning.DestroyAllFromHere();
		Destroy(gameObject);
	}
	private IEnumerator DamageEnemyCoroutine()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(Consts.MinDamageInterval,
																 Consts.MaxDamageInterval));

		if (enemyIndex < EnemyFaction.Minions.Count)
		{
			MyTr.position = enemy.MyTr.position;
			yield return null;

			Attack(Consts.Force);

			yield return StartCoroutine(DamageEnemyCoroutine());
		}
	}
}