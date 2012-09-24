using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DSLTest
{
	public DSLTest()
	{
		var examples = Example.ParseResource(@"Werewolf_TrainingExamples.csv");


		var attr = examples.First().GetAttributes().Skip(1).First();

		Console.WriteLine(attr);
		Console.WriteLine(string.Join(", ", attr.Values));


		DecisionTree tree = DecisionTree.Create(examples);

		Console.Write(tree.ToTGF());
	}

	public static void Main(string[] args)
	{
		new DSLTest();	
	}
}

