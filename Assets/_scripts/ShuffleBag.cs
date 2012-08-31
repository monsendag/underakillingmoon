using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShuffleBag<T> {
	public List<T> Bag{
		get;
		private set;
	}
	
	public ShuffleBag(){
		Bag = new List<T>();
	}
	
	public void GenerateShuffleBag(List<T> items){
		Bag.Clear();
		
		if(items == null || items.Count == 0)
			return;
		
		for(int i = 0; i < items.Count; ++i){
			for(int j = i + 1; j >= 0; --j){
				Bag.Add(items[i]);	
			}
		}
		
		ShuffleItems();
	}
	
	public T PopShuffleBagItem(){
		if(Bag.Count == 0)
			return default(T);
		
		T item = Bag[Bag.Count - 1];
		Bag.RemoveAt(Bag.Count - 1);
		
		return item;
	}
	
	public void Copy(ShuffleBag<T> original){
		Bag.Clear();
		T[] temp = new T[original.Bag.Count];
		original.Bag.CopyTo(temp);
		Bag.AddRange(temp);
	}
	
	//Fisher–Yates shuffle
	void ShuffleItems(){
		T temp;
		
		for(int i = Bag.Count - 1; i >= 0; --i){
			int j = UnityEngine.Random.Range(0, Bag.Count);
			temp = Bag[i];
			Bag[i] = Bag[j];
			Bag[j] = temp;	
		}
	}
}
