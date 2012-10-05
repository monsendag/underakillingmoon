using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class DSLTest
{
	public DSLTest()
	{
		var examples = Example.ParseResource(@"Werewolf_TrainingExamples.csv");
		var attributes = examples.First().GetAttributes();



		foreach (var a in attributes) {
			//if (a.ToString().Equals("RecentGunfire")) {
			Console.WriteLine("" + a + " - Importance: " + a.Importance(examples));
			//}
		}


		var tree = DecisionTree.Create(attributes, examples);

		tree.SaveTGFonDesktop();

	}

	public static void Main(string[] args)
	{
		new DSLTest();	
	}
}

