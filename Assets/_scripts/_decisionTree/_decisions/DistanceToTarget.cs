using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToTarget : IValue
{
	public int Decide(Agent agent){
		Agent target = AttackPair.GetTargetOrNull(agent);
        if (target == null) { return 5; }
		float dist = Vector2.Distance(agent.KinematicInfo.Position, target.KinematicInfo.Position);
        dist = Mathf.Clamp(dist, 0.0f, 100.0f);
        dist /= 20;
        return (int) dist;
	}
}
