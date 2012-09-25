using UnityEngine;
using System.Collections;

public class AgentHealth : IDecision
{
	Agent _agent;
	uint _threshold;
	
	AgentHealth(Agent agent, uint threshold){
		_agent = agent;
		_threshold = threshold;
	}
	
	public bool Decide(){
		return _agent.Health < _threshold;
	}
}
