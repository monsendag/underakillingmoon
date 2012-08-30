/**
 * Implements basic ObstacleAvoidance algorithm.
 **/
using UnityEngine;
using System.Collections;

/// <summary>
/// Tries to avoid irregularly shaped game geometry.
/// </summary>
public class ObstacleAvoidSteer : SeekSteer
{
    public float MinWallDistance = 0.5f;
    public float LookAhead = 4.0f;

    public ObstacleAvoidSteer() : base()
    {
    }

    override public SteeringOutput CalculateAcceleration(Agent agent)
    {
        Vector3 point1 = agent.transform.position;
        Vector2 aheadPoint = agent.KinematicInfo.Velocity.normalized * LookAhead;
        Vector3 direction =  new Vector3(aheadPoint.x,0.0f,aheadPoint.y);

        var hits = Physics.SphereCastAll(point1,MinWallDistance,direction, LookAhead);
        RaycastHit nearestHit;
        bool foundHit = false;
        foreach (var hit in hits)
        {
            if (hit.collider == agent.collider) { continue; }
            nearestHit = hit;
            foundHit = true;
            break;
        }

        if (!foundHit) {  return new SteeringOutput(); }

        Vector2 hitPosition = new Vector2(nearestHit.point.x, nearestHit.point.z);
        Vector2 normal = new Vector2(nearestHit.normal.x, nearestHit.normal.z);


        base.Target.Position =  new Vector2(agent.transform.position.x, agent.transform.position.z)
            + normal * MinWallDistance * 10.0f;

        return base.CalculateAcceleration(agent);
    }
}
