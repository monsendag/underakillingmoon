using UnityEngine;
using System.Collections;

public class SaveCamper : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Camper")
		{
			++GameManager.campersSaved;	
			var agent = col.GetComponent<Agent>();
			if(AttackPair.IsTarget(agent))AttackPair.RemoveByTarget(agent);
			GameObject.Destroy(col.gameObject);
		}
	}
}
