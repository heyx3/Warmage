using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls input for a Leap Motion hand.
/// </summary>
[RequireComponent(typeof(HandModel))]
public class GestureController : MonoBehaviour
{
	public static List<GestureController> AllControllers = new List<GestureController>();

	private static GestureConstants Consts { get { return GestureConstants.Instance; } }


	public HandModel Hand { get; private set; }
	public Transform EyeCenter { get; private set; }

	/// <summary>
	/// The kinematics tracker for the palm.
	/// </summary>
	public KinematicsTracker PalmTracker { get; private set; }
	/// <summary>
	/// The kinematics tracker for each finger (thumb-pinky, in that order).
	/// </summary>
	public KinematicsTracker[] FingerTrackers { get; private set; }

	/// <summary>
	/// Whether each finger (thumb-pinky, in that order) are each pointing outwards.
	/// </summary>
	public bool[] IsFingerPointing { get; private set; }
	/// <summary>
	/// Whether the index finger is touching the thumb, making a circular shape.
	/// </summary>
	public bool IsIndexTouchingThumb { get; private set; }


	/// <summary>
	/// Positive value means it's swiping right; negative means it's swiping left.
	/// A value of 0 means the hand isn't moving fast enough to consider it a swipe.
	/// </summary>
	public float SwipeSidewaysStrength { get; private set; }
	/// <summary>
	/// Positive value means it's swiping up; negative meaans it's swiping down.
	/// A value of 0 means the hand isn't moving fast enough to consider it a swipe.
	/// </summary>
	public float SwipeVerticalStrength { get; private set; }
	/// <summary>
	/// Positive value means it's swiping forward; negative means it's swiping backwards.
	/// A value of 0 means the hand isn't moving fast enough to consider it a swipe.
	/// </summary>
	public float SwipeForwardStrength { get; private set; }


	//Swipe gestures will not happen until a certain amount of time has elapsed.
	private float elapsed = 0.0f;


	void Start()
	{
		Hand = GetComponent<HandModel>();
		EyeCenter = transform.parent.parent.parent;

		PalmTracker = AddTrackerTo(Hand.palm.gameObject);

		FingerTrackers = new KinematicsTracker[5];
		IsFingerPointing = new bool[5];
		for (int i = 0; i < 5; ++i)
		{
			FingerTrackers[i] = AddTrackerTo(Hand, i);
			IsFingerPointing[i] = false;
		}
	}
	private KinematicsTracker AddTrackerTo(HandModel model, int fingerIndex)
	{
		Transform[] fingerBones = model.fingers[fingerIndex].bones;
		return AddTrackerTo(fingerBones[fingerBones.Length - 1].gameObject);
	}
	private KinematicsTracker AddTrackerTo(GameObject g)
	{
		KinematicsTracker kt = g.AddComponent<KinematicsTracker>();
		return kt;
	}

	private Vector3[] fingerPoses = new Vector3[5],
					  fingerForwards = new Vector3[5],
					  fingersToPalm = new Vector3[5];
	void Update()
	{
		Vector3 palmPos = GetWorldPos(PalmTracker);

		//Calculate finger information.

		for (int i = 0; i < 5; ++i)
		{
			fingerPoses[i] = GetWorldPos(FingerTrackers[i]);
			fingerForwards[i] = GetWorldForward(FingerTrackers[i]);
			fingersToPalm[i] = (palmPos - fingerPoses[i]);

			IsFingerPointing[i] = (-Vector3.Dot(fingersToPalm[i], fingerForwards[i]) >
								   Consts.FingerPointAtPalmMinDot);
		}

		float maxDistSqr = Consts.FingerTouchMaxDist * Consts.FingerTouchMaxDist;
		IsIndexTouchingThumb = (fingerPoses[0].DistanceSqr(fingerPoses[1]) <= maxDistSqr);


		//Calculate palm sweeping information if we've waited long enough since this hand was created.
		if (elapsed < Consts.SwipeWait)
		{
			elapsed += Time.deltaTime;
		}
		else
		{
			Vector3 palmVel = PalmTracker.GetAverageVelocity(Consts.SwipeGestureDuration);
			Vector3 camRight = EyeCenter.right,
					camUp = EyeCenter.up,
					camForward = EyeCenter.forward;

			float sidewaysSpeed = Vector3.Dot(palmVel, camRight),
				  upwardsSpeed = Vector3.Dot(palmVel, camUp),
				  forwardSpeed = Vector3.Dot(palmVel, camForward);
			if (Mathf.Abs(sidewaysSpeed) < Consts.SwipeVelocityThreshold)
			{
				SwipeSidewaysStrength = 0.0f;
			}
			else
			{
				SwipeSidewaysStrength = sidewaysSpeed;
				elapsed = 0.0f;
			}
			if (Mathf.Abs(upwardsSpeed) < Consts.SwipeVelocityThreshold)
			{
				SwipeVerticalStrength = 0.0f;
			}
			else
			{
				SwipeVerticalStrength = upwardsSpeed;
				elapsed = 0.0f;
			}
			if (Mathf.Abs(forwardSpeed) < Consts.SwipeVelocityThreshold)
			{
				SwipeForwardStrength = 0.0f;
			}
			else
			{
				SwipeForwardStrength = forwardSpeed;
				elapsed = 0.0f;
			}
		}
	}
	private Vector3 GetWorldPos(KinematicsTracker tracker)
	{
		return tracker.PositionLogs[tracker.GetLogIndex(0)];
	}
	private Vector3 GetWorldForward(KinematicsTracker tracker)
	{
		return tracker.ForwardLogs[tracker.GetLogIndex(0)];
	}
}