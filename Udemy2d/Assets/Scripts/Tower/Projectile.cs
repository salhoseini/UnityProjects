using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    rock,
    fire,
    arrow
};

public class Projectile : MonoBehaviour {

    [SerializeField] private int attackStrength;
    [SerializeField] private ProjectileType proType;

    public int AttackStrength
    {
        get { return attackStrength; }
    }

    public ProjectileType ProType
    {
        get { return ProType;  }
    }
}
