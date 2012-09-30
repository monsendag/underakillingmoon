using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToTarget : IValue
{
    public static float MaxDistance = 15.0f;
    public static int OutputNumber = 5;
	
	public int Decide(Agent agent){
		Agent target = AttackPair.GetTargetOrNull(agent);
        
        if (target == null) { return OutputNumber - 1; }

        // Test the visability of the target.
        Vector3 direction = (target.transform.position - agent.transform.position).normalized;
        var hits = Physics.RaycastAll(agent.transform.position, direction, Config.DefaultWerewolfVisionRange);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != agent.gameObject && hit.collider.gameObject != target.gameObject)
            {
                return (OutputNumber - 1);
            }
        }
        

		float dist = Vector2.Distance(agent.KinematicInfo.Position, target.KinematicInfo.Position);
        dist = Mathf.Clamp(dist, 0.0f, MaxDistance);
        dist = (OutputNumber - 1) * dist / MaxDistance;
        return (int) dist;
	}
    
    public string GetPrettyTypeName()
    {
        return "TargetDistance";
    }

}
