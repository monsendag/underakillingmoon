/**
 * Provides an interface for creating steering behaviours.
/**
 * Provides an interface for creating steering behaviours.
 */
using UnityEngine;
using System.Collections;

public interface ISteeringBehaviour
{
    /// <summary>
    /// Calculates the steering behaviours
    /// </summary>
    /// <param name="agent">
    ///    The agent object. Don't modify this value.
    /// </param>
    /// <param name="info">
    ///     Information about the current position and acceleration of the
    ///     agent.
    /// </param>
    /// <returns></returns> 
    SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info);
}
