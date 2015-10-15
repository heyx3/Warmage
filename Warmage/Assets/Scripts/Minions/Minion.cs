using System;
using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Minion : Attackable
{
	private static MinionConstants Consts { get { return MinionConstants.Instance; } }


	public bool ToggleWalking = false,
				ToggleAttacking = false,
				ToggleFallOver = false;

	public Attacker HandAttackSource;

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
	}

	void Update()
	{
		if (!contr.IsInTransition(0))
		{
			AnimatorStateInfo info = contr.GetCurrentAnimatorStateInfo(0);
			bool isFlailing = IsFlailing;

			if (!isFlailing && ToggleWalking)
			{
				ToggleWalking = false;
				if (IsWalking)
					StopWalking();
				else
					StartWalking();
			}
			else if (!isFlailing && ToggleAttacking)
			{
				ToggleAttacking = false;
				if (IsAttacking)
					StopAttacking();
				else
					StartAttacking();
			}
			else if (ToggleFallOver)
			{
				ToggleFallOver = false;
				if (isFlailing)
				{
					GetBackUp();
				}
				else
				{
					FallOver();
					MyRgd.AddTorque(new Vector3(100000000.0f, 0.0f, 0.0f), ForceMode.VelocityChange);
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