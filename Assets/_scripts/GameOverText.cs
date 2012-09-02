using UnityEngine;
using System.Collections;

public class GameOverText : MonoBehaviour {	
//	public GUIStyle style;
	
	// Use this for initialization
	void Start () { 
		GUIText textComp = GetComponent<GUIText>();
        Debug.Log(GameManager.campersLost + " " + GameManager.campersSaved);
        textComp.text = "Campers Saved: " + GameManager.campersSaved +
            "\n Campers Lost: " + GameManager.campersLost;
	}
}
