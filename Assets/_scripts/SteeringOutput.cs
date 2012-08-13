/**
 * SteeringOutput is used to returns the linear and rotation accelerations
 * output by a steering behaviour.
 **/
using UnityEngine;
using System.Collections;

public struct SteeringOutput
{
	public Vector2 Linear; // Linear acceleration.
	public float Angular; // Angular Rotation in radians.
}