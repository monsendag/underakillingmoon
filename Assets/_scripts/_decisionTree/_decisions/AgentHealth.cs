using UnityEngine;
using System.Collections;

public class AgentHealth : IValue
{
    public static float MaxHealth = 100.0f;
    public static int OutputNumber = 5;
	
	public int Decide(Agent agent){
        float health = Mathf.Clamp(agent.Health, 0, MaxHealth);
        return (int) Mathf.Round((OutputNumber - 1) * agent.Health / MaxHealth);
	}

    public string GetPrettyTypeName()
    {
        return "MyHealth";
    }
}
