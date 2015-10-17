using UnityEngine;
using System.Collections;

public class GetBackUp : StateMachineBehaviour
{
	private Minion mn;
	private Vector3 lookAtTarget;
	private Vector3 lastUp;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);

		//Disable physics so the character can get up.
		mn = animator.GetComponent<Minion>();
		mn.MyRgd.constraints = RigidbodyConstraints.None;
		mn.MyRgd.isKinematic = true;

		//Figure out the position to look towards.
		Vector3 fwd = mn.MyTr.forward;
		Vector2 fwd2 = new Vector2(fwd.x, fwd.z);
		if (fwd2 == Vector2.zero)
		{
			fwd2 = new Vector2(1.0f, 0.0f);
		}
		fwd2.Normalize();
		lookAtTarget = mn.MyTr.position + (10.0f * new Vector3(fwd2.x, 0.0f, fwd2.y));

		lastUp = mn.MyTr.up;
	}
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);

		mn.MyTr.LookAt(lookAtTarget, Vector3.Lerp(lastUp, new Vector3(0.0f, 1.0f, 0.0f), 0.01f).normalized);
		lastUp = mn.MyTr.up;

		if (lastUp.y > 0.9f)
		{
			animator.SetTrigger("Done Getting Up");
		}
	}
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);

		//Reset the physics.
		mn.MyTr.LookAt(lookAtTarget, new Vector3(0.0f, 1.0f, 0.0f));
		mn.MyRgd.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		mn.MyRgd.isKinematic = false;
	}

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}