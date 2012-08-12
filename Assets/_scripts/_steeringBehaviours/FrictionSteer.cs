﻿/**
 * Implements basic frictionalgorithm
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Applies a force to slow down the current velocity or angular velocity..
/// </summary>
public class FrictionSteer : ISteeringBehaviour
{
    // This variable represents the rate of inertia
    public float VelocityFrictionPercentage = 0.1f;
    public float AngularVelocityFrictionPercentage = 0.1f;

    public FrictionSteer() { }

    public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {
        SteeringOutput output = new SteeringOutput();
        output.Linear = info.Velocity * (-VelocityFrictionPercentage);
        output.Angular = info.AngularVelocity * (-VelocityFrictionPercentage);

        return output;
    }
}