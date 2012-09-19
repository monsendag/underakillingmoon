using System;
using System.Collections.Generic;

public class Example : Dictionary<Attribute, object>
{
	public object Classification;
	public List<object> Values;

	public Example()
	{
		/*
		 * Attributes:
		 * Wolf health 		Camper health 		Distance to camper		Distance to player		 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * */


	}

	public List<Attribute> GetAttributes()
	{
		return new List<Attribute>(Keys);
	}
}

