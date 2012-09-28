using UnityEngine;
using System.Collections;
using System.Linq;

public class NearbyAgents<T> : IValue
{
    public float Radius = 5.0f;
	
	public int Decide(Agent agent)
    {
		int agentCount = (int) agent.GetAgentsInArea(Radius).Where (a => a.GetType() == typeof(T)).Count();
        agentCount = Mathf.Clamp(agentCount, 0, 5);
        return agentCount;
	}
}
