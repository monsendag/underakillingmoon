using UnityEngine;
using System.Collections;

public class TargetHealth : IValue
{
	Agent _agent;
	uint _threshold;
	
	TargetHealth(Agent agent, uint threshold){
		_agent = agent;
		_threshold = threshold;
	}
	
	public int Decide(Agent agent){

		Agent target = AttackPair.GetTargetOrNull(_agent);
        if (target == null)
        {
            return 5;
        }
        else return (int) Mathf.Clamp(target.Health, 0, 100) / 20;
	}
}
