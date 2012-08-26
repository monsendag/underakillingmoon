/**
 * Implements basic face steer algorithm.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// FaceSteer steers towards a target.
/// </summary>
public class FaceSteer : AlignSteer
{
	public KinematicInfo LocalTarget;

	public FaceSteer()
	{
		LocalTarget = new KinematicInfo();
	}

	public FaceSteer(KinematicInfo kinematicInfo)
	{
		LocalTarget = kinematicInfo;
	}

	public FaceSteer(Agent agent)
	{
		LocalTarget = agent.KinematicInfo;
	}

	override public SteeringOutput CalculateAcceleration(Agent agent)
	{
		KinematicInfo info = agent.KinematicInfo;

		//float rotation = Target.Orientation - info.Orientation;
		SteeringOutput steering = new SteeringOutput();
		Vector2 direction = (LocalTarget.Position - info.Position);
		if (direction.magnitude == 0.0f) {
			return steering;
		}
		base.Target = LocalTarget;
		base.Target.Orientation = MotionUtils.SetOrientationFromVector(direction.normalized);

		return base.CalculateAcceleration(agent);
	}
}
