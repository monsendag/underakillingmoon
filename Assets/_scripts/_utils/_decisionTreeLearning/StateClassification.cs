using System;

public class StateClassification : Classification
{
	Type state;

	public StateClassification(Type state)
	{
		this.state = state;
	}

	public override string ToString()
	{
		return state.ToString();
	}
}