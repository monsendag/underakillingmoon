/**
 * Provides a way of transferring info about a agents current motion 
 * properties.
 **/
using UnityEngine;
using System.Collections;

public class KinematicInfo
{
    public Vector2 Velocity = Vector2.zero;
    public Vector2 Position = Vector2.zero;

    // Scalar, anticlockwise representing facing direction in radians.
    public float Orientation = 0.0f;
    // Scalar, radians per second that orientation is changing
    public float AngularVelocity = 0.0f;
}
