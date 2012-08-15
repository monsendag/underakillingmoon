﻿/**
 * Collision Avoidance steer
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Try to keep agents from drifting too far apart by moving them towards the 
/// center of the group.
/// </summary>
class CohesionSteer : ISteeringBehaviour
{

    public float LookAhead = 9.0f;
    public float Radius = 1.0f;
    public float MaxAcceleration = 12.0f;

    public CohesionSteer() { }

    virtual public SteeringOutput CalculateAcceleration(GameObject agent, KinematicInfo info)
    {

        SteeringOutput output = new SteeringOutput();
        output.Angular = 0.0f;
        output.Linear = Vector2.zero;

        var agentList = agent.GetComponent<Agent>().GetAgentsInArea(LookAhead);

        Vector2 average = new Vector2();
        Vector2 velocityAverage = new Vector2();
        int num = 0;

        // Get the average pack location
        foreach (var a in agentList)
        {
            // Check whether we are facing the agent.
            if (a == agent) { continue; }

            num++;
            average += a.KinematicInfo.Position;
            velocityAverage += a.KinematicInfo.Velocity;
        }

        if (num == 0) { return output; }


        average /= num;
        velocityAverage /= num;

        Vector2 positionDif = average - info.Position;
        Vector2 velocityDif = velocityAverage - info.Velocity;
        if (velocityDif.magnitude > MaxAcceleration)
        {
            velocityDif = velocityDif.normalized * MaxAcceleration;
        }

        float strength = positionDif.magnitude / 2 * LookAhead;

        output.Linear += (positionDif.normalized * strength) * MaxAcceleration;
        output.Linear += velocityDif / 2.0f;
        return output;

    }

}