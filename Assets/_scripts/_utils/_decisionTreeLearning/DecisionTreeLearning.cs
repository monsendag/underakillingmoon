using System;
using System.Collections.Generic;
using System.Linq;

public class DecisionTreeLearning
{

	//private IList<Attribute> attributes = new IList<Attribute>();

	public DecisionTreeLearning()
	{
		//attributes.Add(new Attribute());


	}


	public Tree decisionTreeLearning(Example[] examples, Attribute[] attributes, Attribute[] parentExamples)
	{
		/*
		
		var classifications = []; 
		// store all classifications in an array
		examples.forEach(function (ex) {
			classifications.push(ex.classification);
		});

		// no examples
		if (examples.length == 0) { 
			return pluralityValue(parentExamples);
		}

		// all examples has the same classification
		else if (classifications.distinct().length == 1) { 
			return new Tree(''+classifications[0]);
		}

		// no attributes
		else if (attributes.length == 0) { 
			return pluralityValue(examples);
		}
		else {
			// find most important attribute
			var important = argmax(importance, attributes, examples); 

			// instantiate a Tree object
			var tree = new Tree(important.label);  
			var exs, branchAttrs = attributes.clone();
			important.values.forEach(function (value) {

				exs = [];

				// copy all examples which has the value
				examples.forEach(function(ex) { 
					if(ex.values[important.id] == value) exs.push(ex);
				});

				// remove attribute from remaining
				branchAttrs.remove(important); 
				tree.addBranch(decisionTreeLearning(exs, branchAttrs, examples));
			});
			return tree;
		}
*/

		return null;
	}

	public Tree pluralityValue(Example[] examples)
	{
		return null;
	}


	/**
	 * Returns a Number defining the importance of an attribute given a set of examples
	 * @param A
	 * @param examples
	 * @return Number

	function importance(A, examples) {
		if(window.DTL.importancefunc == 'random') {
			return importanceRandom(A, examples);
		}
		return importanceInfoGain(A, examples);
	}

	/**
	 * @param A
	 * @param examples
	 */
	double importanceRandom(Attribute[] attributes, Example[] examples)
	{
		//	return new Random();
		return 0;
	}

	/**
	 * @param A
	 * @param examples
	 */
	double importanceInfoGain(Attribute[] attributes, Example[] examples)
	{

		return 0;
		/*

		function B(q) { return -(q * log2(q) + (1 - q) * log2(1 - q)); }

		function numClassifications(examples, match) {
			var count = 0;
			examples.forEach(function (ex) {
				if (ex.classification == match) count++;
			});
			return count;
		}

		var allOnes = numClassifications(examples, 1);
		var allTwos = numClassifications(examples, 2);

		function Remainder(A) {
			var sum = 0;
			A.values.forEach(function(v) {
				var exs = [];
				examples.forEach(function (ex) {
					if (ex.values[A.id] == v) exs.push(ex);
				});

				if (exs.length == 0) return;

				var pk = numClassifications(exs, 1);
				var nk = numClassifications(exs, 2);

				sum += ((pk + nk) / (allOnes + allTwos)) * B(pk / (pk + nk));
			});
			return sum;
		}
		return (B(allOnes / (allOnes + allTwos)) - Remainder(A));
	*/
	}


}

