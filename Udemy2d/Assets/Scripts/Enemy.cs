using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public int target = 0;
	public Transform exitPiont;
	public Transform[] wayPoints;
	public float navigationUpdate;
	public float enemySpeed;
    [SerializeField] private int health;
    [SerializeField] private int awardAmount;
    private bool isDead = false;

	private Transform enemy;
    private Animator enemyAnim;
    private Collider2D enemyCollider;
	private float navigationTime;
    private float currentHealthInBar;
    private int maxHealthInBar;

    private Image healthBar;

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
        enemyCollider = GetComponent<Collider2D>();
        enemyAnim = GetComponent<Animator>();

        healthBar = enemy.Find("CanvasEnemy").Find("healthBG").Find("health").GetComponent<Image>();
        currentHealthInBar = 100;
        maxHealthInBar = 100;

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
            GameManager.getInstance().TotalEscaped += 1;
            GameManager.getInstance().RoundEscaped += 1;
            GameManager.getInstance().isWaveOVer();
        } else if(other.tag == "projectile")
        {
            Projectile newProjectile = other.gameObject.GetComponent<Projectile>();
            enemyHit(newProjectile.AttackStrength);
            Destroy(other.gameObject);
        }
	}

    public void enemyHit(int hitPoints)
    {
        currentHealthInBar = currentHealthInBar - (hitPoints * maxHealthInBar) / health;
        healthBar.fillAmount = currentHealthInBar / maxHealthInBar;
        health = health - hitPoints;
        GameManager.getInstance().AudioSource.PlayOneShot(SoundManager.getInstance().Hit);
        if(health <=0 || currentHealthInBar <= 0)
        {
            die();
            enemyAnim.SetTrigger("didDie");
        } else
        {
            enemyAnim.Play("hurt");
        }
    }

    private void die()
    {
        GameManager.getInstance().TotalKilled += 1;
        Destroy(enemyCollider);
        GameManager.getInstance().isWaveOVer();
        GameManager.getInstance().AudioSource.PlayOneShot(SoundManager.getInstance().Death);
        isDead = true;
        GameManager.getInstance().addMoney(awardAmount);
    }
}
