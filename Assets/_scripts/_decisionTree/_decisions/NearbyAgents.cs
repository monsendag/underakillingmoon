using UnityEngine;
using System.Collections;
using System.Linq;

public class NearbyAgents<T> : IValue
{
    public static float Radius = 5.0f;
    public static int OutputNumber = 5;
	
	NearbyAgents()
    {
	}
	
	public int Decide(Agent agent)
    {
		int agentCount = (int) agent.GetAgentsInArea(Radius).Where (a => a.GetType() == typeof(T)).Count();
        agentCount = Mathf.Clamp(agentCount, 0, OutputNumber - 1);
        return agentCount;
	}
}
