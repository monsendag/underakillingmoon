using UnityEngine;
using System.Collections;

public class TargetHealth : IValue
{	
    public static int OutputNumber = 5;
    public static float MaxHealth = 100;
	
	public int Decide(Agent agent)
    {
		Agent target = AttackPair.GetTargetOrNull(agent);
        if (target == null)
        {
            return OutputNumber - 1;
        }
        float health =  Mathf.Clamp(target.Health, 0, MaxHealth);
        return (int) Mathf.Round( (OutputNumber - 1) * health / MaxHealth);
	}

    public string GetPrettyTypeName()
    {
        return "TargetHealth";
    }
}
