using System;
using System.Linq;
using System.Collections.Generic;


public class Attribute
{
	static Dictionary<string, Attribute> attrs = new Dictionary<string, Attribute>();

	public string Label;
	// list of possible values
	public List<Value> Values = new List<Value>();

	Attribute(String label)
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

	public static Attribute Get(String name)
	{
		if (!attrs.ContainsKey(name)) {
			attrs [name] = new Attribute(name);
		}

		return attrs [name];
	}

	public double Importance(List<Example> examples)
	{
		//return First(examples);
		//return Infogain(examples);
		return Random(examples);
	}

	/// <summary>
	/// Returns a random im
	/// </summary>
	/// <returns>
	/// The random.
	/// </returns>
	/// <param name='examples'>
	/// Examples.
	/// </param>
	double Random(List<Example> examples)
	{
		return new Random().NextDouble();
	}
	
	
	/*
	we're looking at how much information an attribute gains.
	Entropy is a measure of the uncertainty of a random variable; 
	acquisition of information corresponds to a reduction in entropy
	So we're basically looking for the attribute with the lowest amount of entropy. 
	*/
	double Infogain(List<Example> examples)
	{
		double Px;
		int count;
		double totalEntropy = 0;
		
		Console.WriteLine(ToString());
		
		// the number of distinct classifications
		double numClassifications = examples.Select(e => e.Classification).Distinct().Count();
		
		
		// for each value, figure out how many distinct classifications it creates
		foreach (var value in Values) {
			// the number distinct classifications given with the current value 
			count = examples.Where(ex => ex [this] == value)
				.Select(e => e.Classification).Distinct().Count();
			
			// the probability of the value 
			Px = count / numClassifications;
			
			// add the entropy of the current values probability
			totalEntropy += Entropy(Px, Values.Count());
		}
		
		double remainder = Remainder(examples);
		//The information gain from the attribute test on A is the expected reduction in entropy
		double infogain = totalEntropy - remainder;
		
		//Console.WriteLine("Total # classifications: " + numClassifications);
		//Console.WriteLine("Total entropy: " + totalEntropy);
		//Console.WriteLine("Remainder: " + remainder);
		//Console.WriteLine("InfoGain: " + infogain);
		return infogain;
	}
	
	// the entropy of a n-ary random variable p that is true with probability q
	double Entropy(double p, int n)
	{
		
		Console.WriteLine("p n: {0} {1} ", p, n);

		if (p == 0 || n < 2) {
			return 0;
		}
		
		var entropy = -(p * Math.Log(p, n) + (1 - p) * Math.Log(1 - p, n)); 

		
		return entropy;
	}
	
	double Remainder(List<Example> examples)
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
		foreach (var value in Values) {
			//subset
			List<Example> cis = examples.Where(ex => ex [this] == value).ToList(); 
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

