/**
 * Provides a way of transferring info about a agents current motion 
 * properties.
 **/
using UnityEngine;
using System.Collections;

public class KinematicInfo
{
    public Vector2 Velocity = Vector3.zero;
    public Vector2 Position = Vector3.zero;

    // Unit Vector representing facing direction.
    public float Orientation = 0.0f;
    // Scalar, runs it anticlockwise direction.
    public float AngularVelocity = 0.0f;
}
