using System;
using System.Collections.Generic;

public class Attribute
{
	public string Label;
	public List<object> Values = new List<object>();
	public object Value;

	public Attribute(String label)
	{
		Label = label;
	}
}

