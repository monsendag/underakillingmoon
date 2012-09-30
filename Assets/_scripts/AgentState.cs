using System;

public class AgentState
{

	public Agent agent {
		set;
		get;
	}

	public AgentState()
	{

	}

	public AgentState(Agent agent)
	{
		this.agent = agent;
	}

	/// <summary>
	/// Update the specified state. 
	/// </summary>
	/// <param name='state'>
	/// State.
	/// </param>
	public virtual void Update(out Type state)
	{
		state = null;
	}

    /// <summary>
    /// Get a pretty version of the type name, appropriate for printing.
    /// </summary>
    /// <returns> A pretty version of the type name.</returns>
    public virtual string GetPrettyTypeName()
    {
        return this.GetType().ToString();
    }
}

