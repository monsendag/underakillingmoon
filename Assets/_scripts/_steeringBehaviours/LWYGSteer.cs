/**
 * Look where you are going steer.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// LWYG look where you're going.
/// The LWYGSteer behaviour creates the effect of a wandering unit.
/// </summary>
public class LWYGSteer : AlignSteer
{
    public LWYGSteer() {}

    override public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {

        if (info.Velocity.magnitude == 0)
        {
            return new SteeringOutput();
        }

        Target.Orientation = MotionUtils.SetOrientationFromVector(info.Velocity);

        return base.CalculateAcceleration(agent, info);
    }
}
