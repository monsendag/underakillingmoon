using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DSLTest
{
	public DSLTest()
	{
		var examples = Example.ParseResource(@"test.csv");
		var attributes = examples.First().GetAttributes();

		foreach (var a in attributes) {
			Console.WriteLine("" + a + ": " + a.Importance(examples));
		}

		var tree = DecisionTree.Create(attributes, examples);

		tree.SaveTGFonDesktop();

	}

	public static void Main(string[] args)
	{
		new DSLTest();	
	}
}

