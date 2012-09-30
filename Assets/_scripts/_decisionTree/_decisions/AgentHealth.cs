using UnityEngine;
using System.Collections;

public class AgentHealth : IValue
{		
	public int Decide(Agent agent)
    {
        int health = Mathf.Clamp((int) agent.Health, 0, 100);
        health = (int) (agent.Health / 20);
        return health;
	}

    public string GetPrettyTypeName()
    {
        return "MyHealth";
    }
}
