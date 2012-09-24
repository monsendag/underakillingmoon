using System;
using System.Collections.Generic;

public class Attribute
{
	public string Label;
	// list of possible values
	public List<Value> Values = new List<Value>();

	public Attribute(String label)
	{
		Label = label;
	}

	public override string ToString()
	{
		return Label;
	}

	public void AddValue(Value value)
	{
		Values.Add(value);
	}

	public Value GetValue(string value)
	{
		foreach (var val in Values) {
			if (val.ToString().Equals(value)) {

				//Console.WriteLine("value exist " + value + " " + val);
				return val;
			}
		}
		Value newVal = Value.Parse(value);
		AddValue(newVal);
		return newVal;
	}
}

