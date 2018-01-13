using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int target = 0;
	public Transform exitPiont;
	public Transform[] wayPoints;
	public float navigationUpdate;
	public float enemySpeed;
    [SerializeField] private int health;
    private bool isDead = false;

	private Transform enemy;
    private Animator enemyAnim;
	private float navigationTime;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

	// Use this for initialization
	void Start () {
		enemy = GetComponent<Transform> ();
        enemyAnim = GetComponent<Animator>();
        GameManager.getInstance().registerEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (wayPoints != null && !isDead) {
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
			GameManager.getInstance().unregisterEnemy(this);
		} else if(other.tag == "projectile")
        {
            Projectile newProjectile = other.gameObject.GetComponent<Projectile>();
            enemyHit(newProjectile.AttackStrength);
            Destroy(other.gameObject);
        }
	}

    public void enemyHit(int hitPoints)
    {
        health = health - hitPoints;
        if(health <=0)
        {
            die();
            enemyAnim.SetTrigger("didDie");
            // call death animation
            //Destroy(this);
        } else
        {
            enemyAnim.Play("hurt");
            //call hurt animation
        }
    }

    private void die()
    {
        isDead = true;
    }
}
