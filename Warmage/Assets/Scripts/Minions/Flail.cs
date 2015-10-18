using UnityEngine;
using System.Collections;


public class Flail : StateMachineBehaviour
{
	public static readonly float FlailTime = 2.0f;


	private Minion mn;
	private float elapsed = 0.0f;


	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);

		mn = animator.GetComponent<Minion>();
		mn.MyRgd.constraints = RigidbodyConstraints.None;
		elapsed = 0.0f;
	}
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);

		mn.MyRgd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);

		elapsed += Time.deltaTime;
		if (elapsed >= FlailTime)
		{
			//Wait until this minion is on or near the ground.
			Vector3 pos = mn.MyRgd.position;
			if (pos.y < Terrain.activeTerrain.SampleHeight(pos) + 0.1f)
			{
				animator.SetTrigger("Get Back Up");
			}
		}
	}
}