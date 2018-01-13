using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    [SerializeField] private float attackRadius;
    [SerializeField] private float attackDelay;
    [SerializeField] private Projectile projectile;
    private Enemy targetEnemy = null;
    private float attackCounter;
    private bool readyToAttack = false;

    public void Start()
    {
        
    }

    public void Update()
    {
        attackCounter -= Time.deltaTime;
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            targetEnemy = getNearestEnemy() == null ? null : getNearestEnemy();
        }
        else
        {
            if (attackCounter <= 0)
            {
                readyToAttack = true;
                attackCounter = attackDelay;
            }
            else
            {
                readyToAttack = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) >= attackRadius)
            {
                targetEnemy = null; // call off the attack
            }
        }
    }

    public void FixedUpdate()
    {
        if(readyToAttack)
        {
            attack();
        }
    }

    public void attack()
    {
        readyToAttack = false;
        if(targetEnemy != null || !targetEnemy.IsDead)
        {
            Projectile newProjectile = Instantiate(projectile) as Projectile;
            newProjectile.transform.localPosition = transform.localPosition;
            StartCoroutine(shootProjectile(newProjectile));
        }
    }

    IEnumerator shootProjectile(Projectile projectile) 
    {
        while(getDistanceToTarget(targetEnemy) > 0.2f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);
            yield return null;
        }
        if(projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }

    private float getDistanceToTarget(Enemy enemy)
    {
        if(enemy == null)
        {
            enemy = getNearestEnemy();
            if(enemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    }

    public List<Enemy> getEnemiesInRange()
    {
        List<Enemy> result = new List<Enemy>();
        foreach(Enemy enemy in GameManager.getInstance().EnemyList)
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
            {
                result.Add(enemy);
            }
        }
        return result;
    }

    public Enemy getNearestEnemy()
    {
        Enemy nearest = null;
        float smallestDistance = float.PositiveInfinity;
        foreach(Enemy enemy in getEnemiesInRange())
        {
            if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= smallestDistance)
            {
                nearest = enemy;
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
            }
        }
        return nearest;
    }

}
