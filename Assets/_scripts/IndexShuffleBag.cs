using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IndexShuffleBag{
	public List<int> Bag{
		get;
		private set;
	}
	
	public IndexShuffleBag(){
		Bag = new List<int>();
	}
	
	public virtual void GenerateShuffleBag(int size){
		Bag.Clear();
		
		if(size == 0)
			return;
		
		for(int i = 1; i < size; ++i){
			for(int j = size; j >= 0; --j){
				Bag.Add(i);	
			}
		}
		
		ShuffleItems();
	}
	
	public int PopShuffleBagItem(){
		if(Bag.Count == 0)
			return 0;
		
		int item = Bag[Bag.Count - 1];
		Bag.RemoveAt(Bag.Count - 1);
		
		return item;
	}
	
	public void Copy(IndexShuffleBag original){
		Bag.Clear();
		int[] temp = new int[original.Bag.Count];
		original.Bag.CopyTo(temp);
		Bag.AddRange(temp);
	}
	
	//Fisher–Yates shuffle
	void ShuffleItems(){
//		int temp;
		for(int i = Bag.Count - 1; i >= 0; --i){
			int j = UnityEngine.Random.Range(0, Bag.Count);
			/*temp = Bag[i];
			Bag[i] = Bag[j];
			Bag[j] = temp;*/
			Bag[i] = Bag[i] + Bag[j];
			Bag[j] = Bag[i] - Bag[j];
			Bag[i] = Bag[i] - Bag[j];
		}
	}
}
