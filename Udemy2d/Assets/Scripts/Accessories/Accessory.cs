using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Accessory : MonoBehaviour {

    [SerializeField] private int health;

    private float currentHealthInBar;
    private int maxHealthInBar;

    private Image healthBar;
    private Transform accessory;
    private bool isDestroyed = false;
    private Collider2D accessoryCollider;
    private Collider2D site;

    public bool IsDestroyed
    {
        get
        {
            return isDestroyed;
        }

        set
        {
            isDestroyed = value;
        }
    }

    public Collider2D AccessoryCollider
    {
        get
        {
            return accessoryCollider;
        }

        set
        {
            accessoryCollider = value;
        }
    }

    public Collider2D Site
    {
        get
        {
            return site;
        }

        set
        {
            site = value;
        }
    }

    // Use this for initialization
    void Start () {
        accessory = GetComponent<Transform>();
        healthBar = accessory.Find("CanvasEnemy").Find("healthBG").Find("health").GetComponent<Image>();
        currentHealthInBar = 100;
        maxHealthInBar = 100;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void accessoryHit(int hitPoint)
    {
        currentHealthInBar = currentHealthInBar - (hitPoint * maxHealthInBar) / health;
        healthBar.fillAmount = currentHealthInBar / maxHealthInBar;
        health = health - hitPoint;
        // play tower hit audio
        if (health <= 0 || currentHealthInBar <= 0)
        {
            AccessoryManager.getInstance().destroyAccessory(this);
        }
    }
}
