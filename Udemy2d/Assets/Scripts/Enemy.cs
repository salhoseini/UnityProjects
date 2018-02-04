using System.Collections.Generic;
using System.Collections;
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
    [SerializeField] private int attackPower;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRadius;

    private Accessory targetSpike;
    private float attackCounter;
    private bool isDead = false;
    private bool isAttacking = false;
    private bool readyToAttack = false;

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

    void FixedUpdate()
    {
        if(readyToAttack)
        {
            attack(targetSpike);
        }
    }

    // Update is called once per frame
    void Update () {
		if (wayPoints != null && !isDead && !isAttacking) {
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

        attackCounter -= Time.deltaTime;
        targetSpike = getTargetsInRange();
        if (targetSpike != null && !targetSpike.IsDestroyed)
        {
            if(Vector2.Distance(transform.localPosition, targetSpike.transform.localPosition) >= attackRadius)
            {
                targetSpike = null;
                readyToAttack = false;
            }
            if (attackCounter <= 0)
            {
                readyToAttack = true;
                attackCounter = attackDelay;
            }
            else
            {
                readyToAttack = false;
            }
        } else
        {
            isAttacking = false;
            readyToAttack = false;
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
            if(newProjectile != null)
            {
                enemyHit(newProjectile.AttackStrength);
                Destroy(other.gameObject);
            }
        }
    }

    public Accessory getTargetsInRange()
    {
        Accessory result = null;
        foreach (Accessory spike in AccessoryManager.getInstance().Accessories)
        {
            if(!spike.IsDestroyed)
            {
                if (Vector2.Distance(transform.localPosition, spike.transform.localPosition) <= attackRadius)
                {
                    result = spike;
                    break;
                }
            }
            
        }
        return result;
        
    }

    public void attack(Accessory accessory)
    {
        if(accessory != null)
        {
            readyToAttack = false;
            isAttacking = true;
            enemyAnim.Play("attacking");
            accessory.accessoryHit(attackPower);
            if (accessory.IsDestroyed)
            {
                isAttacking = false;
            }
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
        isAttacking = false;
        GameManager.getInstance().TotalKilled += 1;
        Destroy(enemyCollider);
        GameManager.getInstance().isWaveOVer();
        GameManager.getInstance().AudioSource.PlayOneShot(SoundManager.getInstance().Death);
        isDead = true;
        GameManager.getInstance().addMoney(awardAmount);
    }
}
