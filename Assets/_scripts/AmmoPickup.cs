using UnityEngine;
using System.Collections;

public class AmmoPickup : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player"){
			col.SendMessage("TopUpAmmo");
			Destroy(gameObject);
		}
	}
}
