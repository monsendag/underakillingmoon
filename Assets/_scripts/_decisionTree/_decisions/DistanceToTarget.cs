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
