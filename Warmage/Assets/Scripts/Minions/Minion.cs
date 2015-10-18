using System;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Minion : Attackable
{
	private static MinionConstants Consts { get { return MinionConstants.Instance; } }


	public GameObject PunchParticlesPrefab;

	public Attacker HandAttackSource;
	[NonSerialized]
	public Faction Owner;

	[NonSerialized]
	public Vector2 WalkTargetPos;

	private Animator contr;

	
	public Transform MyTr { get; private set; }
	public Rigidbody MyRgd { get; private set; }
	

	public bool IsFlailing { get { return contr.GetCurrentAnimatorStateInfo(0).IsName("Flail"); } }
	public bool IsWalking { get { return contr.GetCurrentAnimatorStateInfo(0).IsName("Walk"); } }
	public bool IsAttacking { get { return contr.GetCurrentAnimatorStateInfo(0).IsName("Attack"); } }
	public bool IsIdle { get { return contr.GetCurrentAnimatorStateInfo(0).IsName("Idle"); } }


	void Awake()
	{
		contr = GetComponent<Animator>();
		MyTr = transform;
		MyRgd = GetComponent<Rigidbody>();

		if (PunchParticlesPrefab == null)
		{
			Debug.LogError("Punch Particles Prefab is null for a minion!");
		}
	}
	void Start()
	{
		if (Owner == null)
		{
			Debug.LogWarning("This minion has no owner: \"" + gameObject.name + "\"");
		}
		Owner.Minions.Add(this);
	}
	void OnDestroy()
	{
		UnityEngine.Assertions.Assert.IsTrue(Owner.Minions.Contains(this));
		Owner.Minions.Remove(this);
	}

	void Update()
	{
		Vector3 myPos = MyTr.position;
		Vector2 myPos2 = myPos.Horz();

		//Rotate to face the target position.
		MyTr.LookAt(Vector3.Lerp(myPos + (MyTr.forward * 10.0f),
									(WalkTargetPos + new Vector2(0.00001f, 0.0f)).Full3D(myPos.y),
									Consts.TurnLerpRate));

		//Only do normal logic if not in the middle of attacking somebody or falling down.
		if (!IsAttacking && !IsFlailing)
		{
			bool closeEnough = (myPos2.DistanceSqr(WalkTargetPos) <= Consts.MaxDistSqrFromTarget);
			if (IsWalking)
			{
				//If near enough to the target pos, stop walking.
				if (closeEnough)
				{
					StopWalking();
				}
				else
				{
					MyRgd.position += MyTr.forward * (Time.deltaTime * Consts.WalkSpeed);
				}
			}
			else
			{
				if (!closeEnough)
				{
					StartWalking();
				}
			}
		}
	}


	//Called by the Animator when this minion just threw a punch.
	public void MakeAttack()
	{
		HandAttackSource.Attack(Consts.PunchForce);
	}

	public override void OnAttacked(Attacker attacker, float force)
	{
		if (!IsFlailing)
			FallOver();

		MyRgd.AddForceAtPosition(attacker.MyTr.forward * force, attacker.MyTr.position,
								 ForceMode.Impulse);

		Instantiate<GameObject>(PunchParticlesPrefab).transform.position = attacker.MyTr.position;
	}


	public void StartWalking()
	{
		contr.SetTrigger("Start Walking");
	}
	public void StopWalking()
	{
		contr.SetTrigger("Stop Walking");
	}
	public void StartAttacking()
	{
		contr.SetTrigger("Start Attacking");
	}
	public void StopAttacking()
	{
		contr.SetTrigger("Stop Attacking");
	}
	public void FallOver()
	{
		contr.SetTrigger("Fall Over");
	}
	public void GetBackUp()
	{
		contr.SetTrigger("Get Back Up");
	}
}