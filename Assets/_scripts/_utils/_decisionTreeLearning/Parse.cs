using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

public class Parse
{
	public static void Main()
	{
		var reader = new StreamReader(File.OpenRead(@"../Werewolf_TrainingExamples.csv"));
		
		String attributeLine = reader.ReadLine();
		var attributes = new List<Attribute>(); 
		foreach (String attributeName in attributeLine.Split(',')) {
			attributes.Add(new Attribute(attributeName));
		}

		var examples = new List<Example>();
		while (!reader.EndOfStream) {
			String line = reader.ReadLine();
			var values = line.Split(',');
			var example = new Example();
			Attribute[] attributeArr = attributes.ToArray();
			for (var i=0; i<attributeArr.Length; i++) {
				example.Add(attributeArr [i], values [i]);
			}
			examples.Add(example);
		}
	}
}


