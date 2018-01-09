using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject spawnPoint;
	public GameObject[] enemies;
	public int maxEnemiesOnScreen;
	public int totalEnemies;
	public int enemiesPerSpawn;
	const float spawnDelay = 0.8f;
	[SerializeField] private int enemiesInMap;

	private int enemiesOnScreen = 0;

	private GameManager(){
	}

	public static GameManager getInstance(){
		if (instance == null) {
			instance = new GameManager ();
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
		enemiesInMap--;
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (spawn ());
	}

	IEnumerator spawn() {

		if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies) {
			for(int i = 0 ; i < enemiesPerSpawn ; i++) {
				if (enemiesOnScreen <= maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate (enemies [getEnemies()]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
					enemiesOnScreen++;
				}

			}
			yield return new WaitForSeconds (spawnDelay);
			StartCoroutine (spawn ());
		}
	}

	public void destroyEnemyOnScreen() {
		if (enemiesOnScreen > 0) {
			enemiesOnScreen -= 1;
		}

	}

	private int getEnemies() {
		Random r = new Random();
		int result = Random.Range (0, enemiesInMap);
		return result;
	}
}
