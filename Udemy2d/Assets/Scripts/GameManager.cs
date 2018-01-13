using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject spawnPoint;
	public GameObject[] enemies;
	public int maxEnemiesOnScreen;
	public int totalEnemies;
	public int enemiesPerSpawn;
	const float spawnDelay = 0.8f;
	[SerializeField] private int enemiesInMap;
    public List<Enemy> EnemyList = new List<Enemy>();

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

		if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
			for(int i = 0 ; i < enemiesPerSpawn ; i++) {
				if (EnemyList.Count <= maxEnemiesOnScreen) {
					GameObject newEnemy = Instantiate (enemies [getEnemies()]) as GameObject;
					newEnemy.transform.position = spawnPoint.transform.position;
				}

			}
			yield return new WaitForSeconds (spawnDelay);
			StartCoroutine (spawn ());
		}
	}

    public void registerEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void unregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void destroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            unregisterEnemy(enemy);
        }
        EnemyList.Clear();
    }

	private int getEnemies() {
		Random r = new Random();
		int result = Random.Range (0, enemiesInMap);
		return result;
	}
}
