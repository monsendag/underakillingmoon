using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class NearbyAgents<T> : IProperty
{
	public static float Radius = 5.0f;
	public static int OutputNumber = 5;
	
	public int Get(Agent agent)
	{
		int agentCount = (int)agent.GetAgentsInArea(Radius).Where(a => a.GetType() == typeof(T)).Count();
		agentCount = Mathf.Clamp(agentCount, 0, OutputNumber - 1);
		return agentCount;
	}

	public string GetPrettyTypeName()
	{
		return "Num" + typeof(T).ToString();
	}
}
