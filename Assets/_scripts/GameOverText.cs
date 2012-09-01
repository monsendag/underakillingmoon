using UnityEngine;
using System.Collections;

public class GameOverText : MonoBehaviour {	
	// Use this for initialization
	void Start () { 
		GUIText textComp = GetComponent<GUIText>();
		textComp.text = string.Format("Campers Saved: {0}\nCampers Lost: {1}", 
			GameManager.campersSaved,
			GameManager.campersLost);
	}
}
