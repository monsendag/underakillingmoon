using System;
using System.Text.RegularExpressions;

public class Value
{
	object Val;

	public Value(object val)
	{
		Val = val;
	}

    public bool Equals(object other)
    {
        return Val == other;
    }

	public override string ToString()
	{
		return Val.ToString();
	}

	public static Value Parse(string value)
	{
		string text = "One car red car blue car";
		string pat = @"(\w+)\s+(car)";
		
		//	if(r.Match(value).Success) {
		//		return 
		//	};
		return new Value(value);

	}

}

