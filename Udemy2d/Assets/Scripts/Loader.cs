using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Use this for initialization
	public GameManager gameManager;

	void Awake() {
		gameManager = GameManager.getInstance ();
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
