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
	public KinematicInfo LocalTarget = new KinematicInfo();

	public FaceSteer()
	{
	}

	override public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
	{
		//float rotation = Target.Orientation - info.Orientation;
		SteeringOutput steering = new SteeringOutput();
		Vector2 direction = (LocalTarget.Position - info.Position);
		if (direction.magnitude == 0.0f) {
			return steering;
		}
		base.Target = LocalTarget;
		base.Target.Orientation = MotionUtils.SetOrientationFromVector(direction.normalized);

		return base.CalculateAcceleration(agent, info);
	}
}
