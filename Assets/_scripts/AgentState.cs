using System;

public class AgentState
{
	
	private Agent _agent;

	public Agent agent {
		get { return _agent;}
	}

	public AgentState()
	{

	}

	public AgentState(Agent agent)
	{
		_agent = agent;
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

}

