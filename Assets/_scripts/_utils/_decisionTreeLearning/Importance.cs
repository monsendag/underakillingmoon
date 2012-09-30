using System;
using System.Linq;
using System.Collections.Generic;

public static class Importance
{
	/// <summary>
	/// Returns a random im
	/// </summary>
	/// <returns>
	/// The random.
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	public static double Random(Attribute attribute, List<Example> examples)
	{
		return new Random().NextDouble();
	}


	/*
	we're looking at how much information an attribute gains.
	Entropy is a measure of the uncertainty of a random variable; 
	acquisition of information corresponds to a reduction in entropy
	So we're basically looking for the attribute with the lowest amount of entropy. 
	*/
	public static double Infogain(Attribute attribute, List<Example> examples)
	{
		double Px;
		int count;
		double valueEntropy;
		double totalEntropy = 0;

		Console.WriteLine(attribute.ToString());

		// the number of distinct classifications
		double numClassifications = examples.Select(e => e.Classification).Distinct().Count();

		
		Console.WriteLine("Total # classifications: " + numClassifications);

		// for each value, figure out how many distinct classifications it creates
		foreach (var value in attribute.Values) {
			// the number distinct classifications given with the current value 
			count = examples.Where(ex => ex [attribute] == value)
				.Select(e => e.Classification).Distinct().Count();

			// the probability of the value 
			Px = count / numClassifications;

			// add the entropy of the current values probability
			totalEntropy += Entropy(Px, attribute.Values.Count());
		}

		//The information gain from the attribute test on A is the expected reduction in entropy
		double remainder = Remainder(attribute, examples);
		double infogain = totalEntropy - remainder;
		Console.WriteLine(totalEntropy);
		Console.WriteLine(remainder);
		Console.WriteLine(infogain);
		return infogain;
	}

	// the entropy of a n-ary random variable p that is true with probability q
	static double Entropy(double p, int n)
	{
		if (p < 2 || n < 2) {
			return 0;
		}


		var entropy = -(p * Math.Log(p, n) + (1 - p) * Math.Log(1 - p, n)); 

		Console.WriteLine("p n E: {0} {1} {2}", p, n, entropy);

		return entropy;
	}

	static double Remainder(Attribute attribute, List<Example> examples)
	{
		/*
		 * the sum from k = 1 to d 
		 * d = # of distinct values dividing the training set into subsets E1 .. Ed
		 * each subset Ek has examples a various different classifications. pk, nk...
		 * 
		 * 
		SUM( (( #Pk + #Nk + #koko ...) / total # of samples )) * Entropy(pk / #Pk+#Nk+#koko) 

		*/
		
		double sum = 0;
		double entropy;
		foreach (var value in attribute.Values) {
			//subset
			List<Example> cis = examples.Where(ex => ex [attribute] == value).ToList(); 
			entropy = 0;
			
			foreach (var classification in cis.Select(ex => ex.Classification)) {
				//TODO FIX
				//entropy += Entropy(cis [attribute].Where());
			}
			sum += cis.Count() / examples.Count() * entropy;
		}
		return sum;
	}

}
