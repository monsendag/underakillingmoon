using UnityEngine;
using System.Collections;
using System.Linq;

public class NearbyEnemies<T> : IDecision
{
	Agent _agent;
	float _radius;
	uint _threshold;
	
	NearbyEnemies(Agent agent, float radius, uint threshold){
		_agent = agent;
		_radius = radius;
		_threshold = threshold;
	}
	
	public bool Decide(){
		return (uint)_agent.GetAgentsInArea(_radius).Where (a => a.GetType() == typeof(T)).Count() < _threshold;
	}
}
