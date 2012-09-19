using UnityEngine;
using System.Collections;
using System.Linq;

public class DistanceToTarget : IDecision
{
	Agent _agent;
	float  _threshold;
	
	DistanceToTarget(Agent agent, uint threshold){
		_agent = agent;
		_threshold = threshold;
	}
	
	public bool Decide(){
		Agent target = AttackPair.GetTargetOrNull(_agent);
		return Vector2.Distance(_agent.KinematicInfo.Position, target.KinematicInfo.Position) < _threshold;
	}
}
