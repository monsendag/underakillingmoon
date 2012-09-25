using UnityEngine;
using System.Collections;

public class TargetHealth : IDecision
{
	Agent _agent;
	uint _threshold;
	
	TargetHealth(Agent agent, uint threshold){
		_agent = agent;
		_threshold = threshold;
	}
	
	public bool Decide(){
		return AttackPair.GetTargetOrNull(_agent).Health < _threshold;
	}
}
