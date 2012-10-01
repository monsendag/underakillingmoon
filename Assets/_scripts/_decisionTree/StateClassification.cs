using System;

public class StateClassification : Classification
{
	Type state;

    public Type State
    {
        get
        {
            return state;
        }
    }

	public StateClassification(Type state)
	{
		this.state = state;
	} 

	public override string ToString()
	{
		return state.ToString();
	}
}