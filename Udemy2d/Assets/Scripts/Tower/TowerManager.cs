using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TowerManager : MonoBehaviour {

	public static TowerManager instance;

	private TowerController selectedTowerBtn;

	public static TowerManager getInstance(){
		if (instance == null) {
			instance = new TowerManager ();
		}
		return instance;
	}

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if(instance != this) {
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
			if (hit.collider.tag == "buildSite") {
				placeTower (hit);
			}
		}
	}

	public void placeTower(RaycastHit2D hit) {
		if (!EventSystem.current.IsPointerOverGameObject () && selectedTowerBtn != null) {
			GameObject newTower = Instantiate (selectedTowerBtn.TowerObject);
			newTower.transform.position = hit.transform.position;
		}
	}

	public void selectedTower(TowerController tower) {
		selectedTowerBtn = tower;
		Debug.Log("tower Selected " + selectedTowerBtn.TowerObject);
	}
}
