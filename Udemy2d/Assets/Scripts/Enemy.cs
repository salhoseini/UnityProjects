using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int target = 0;
	public Transform exitPiont;
	public Transform[] wayPoints;
	public float navigationUpdate;
	public float enemySpeed;

	private Transform enemy;
	private float navigationTime;

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (wayPoints != null) {
			navigationTime += Time.deltaTime;
			if (navigationTime > navigationUpdate) {
				if (target < wayPoints.Length) {
					enemy.position = Vector2.MoveTowards (enemy.position, wayPoints [target].position, navigationTime * enemySpeed);
				} else {
					enemy.position = Vector2.MoveTowards (enemy.position, exitPiont.position, navigationTime * enemySpeed);
				}
				navigationTime = 0;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "checkpoint") {
			target += 1;
		} else if (other.tag == "Finish") {
			Destroy (gameObject);
			GameManager.getInstance ().destroyEnemyOnScreen ();
		}
	}
}
