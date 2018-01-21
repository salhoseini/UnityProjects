using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    [SerializeField] private AudioClip arrow;
    [SerializeField] private AudioClip fireBall;
    [SerializeField] private AudioClip rock;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip death;
    [SerializeField] private AudioClip level;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip newGame;
    [SerializeField] private AudioClip towerBuilt;

    private SoundManager ()
    {

    }

    public static SoundManager getInstance()
    {
        if(instance == null)
        {
            instance = new SoundManager();
        }
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioClip Arrow
    {
        get
        {
            return arrow;
        }
    }
    public AudioClip FireBall
    {
        get
        {
            return fireBall;
        }
    }
    public AudioClip Rock
    {
        get
        {
            return rock;
        }
    }
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
    public AudioClip Death
    {
        get
        {
            return death;
        }
    }
    public AudioClip Level
    {
        get
        {
            return level;
        }
    }
    public AudioClip GameOver
    {
        get
        {
            return gameOver;
        }
    }
    public AudioClip NewGame
    {
        get
        {
            return newGame;
        }

    }
    public AudioClip TowerBuilt
    {
        get
        {
            return towerBuilt;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
