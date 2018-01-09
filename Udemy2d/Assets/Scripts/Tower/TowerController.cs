using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {

	[SerializeField] private GameObject towerObject;

	public GameObject TowerObject {
		get{ 
			return towerObject;
		}
	}
}
