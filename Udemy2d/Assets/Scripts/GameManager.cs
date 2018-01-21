using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameStatus
{
    next, play, gameover, win , inGame
}

public class GameManager : MonoBehaviour {

    [SerializeField] private int totalWaves;
    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWaveLabel;
    [SerializeField] private Text playButtonLabel;
    [SerializeField] private Button playButton;
    [SerializeField] private Text enemiesEscapedLabel;

    private int waveNumber = 1;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private int maxWaves = 50;
    private AudioSource audioSource;

    private GameStatus currentState = GameStatus.play;

    public static GameManager instance;

	public GameObject spawnPoint;
	public GameObject[] enemies;
    [SerializeField] public int totalEnemies = 3;
	public int enemiesPerSpawn;
	const float spawnDelay = 0.8f;
	[SerializeField] private int enemiesInMap;
    public List<Enemy> EnemyList = new List<Enemy>();

	private GameManager(){
	}

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = totalMoney.ToString();
        }
    }

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }

    public GameStatus CurrentState
    {
        get
        {
            return currentState;
        }
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
        playButton.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        showMenu();
	}

    private void Update()
    {
        handleEscape();
    }

    IEnumerator spawn() {

		if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies) {
			for(int i = 0 ; i < enemiesPerSpawn ; i++) {
				if (EnemyList.Count <= totalEnemies) {
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
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

	private int getEnemies() {
		Random r = new Random();
        //int result = Random.Range (0, enemiesInMap);
        int result = 1;
        return result;
	}

    public void addMoney(int value)
    {

        TotalMoney += value;
    }

    public void subtractMoney(int value)
    {
        TotalMoney -= value;
    }

    public bool isWaveOVer()
    {
        enemiesEscapedLabel.text = "Escaped : " + totalEscaped + "/10";
        setCurrentGameState();
        if(currentState.Equals(GameStatus.next) || currentState.Equals(GameStatus.win))
        {
            showMenu();
            return true;
        }
        return false;
    }

    public void setCurrentGameState()
    {
        if(totalEscaped >= 10)
        {
            currentState = GameStatus.gameover;
            AudioSource.PlayOneShot(SoundManager.getInstance().GameOver);
        } else if(waveNumber >= maxWaves)
        {
            currentState = GameStatus.win;
        }
        else if (totalKilled + totalEscaped >= totalEnemies)
        {
            currentState = GameStatus.next;
        }
        else if (totalKilled + totalEscaped == 0 && waveNumber == 1)
        {
            currentState = GameStatus.play;
        } else
        {
            currentState = GameStatus.inGame;
        }
    }

    public void playButtonPressed()
    {
        switch(currentState)
        {
            case GameStatus.next:
                waveNumber++;
                totalEnemies += waveNumber;
                break;
            default:
                // reset everything including labels, towers on screen, total values
                totalEnemies = 3;
                TotalEscaped = 0;
                totalMoney = 10;
                totalMoneyLabel.text = TotalMoney.ToString();
                enemiesEscapedLabel.text = "Escaped : " + totalEscaped + "/10";
                TowerManager.getInstance().renameBuildSiteTag();
                TowerManager.getInstance().destroyAllTowers();
                audioSource.PlayOneShot(SoundManager.getInstance().NewGame);
                break;
        }
        destroyAllEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLabel.text = "Wave " + waveNumber;
        StartCoroutine(spawn());
        playButton.gameObject.SetActive(false);
    }

    private void showMenu()
    {
        switch(currentState)
        {
            case GameStatus.gameover:
                playButtonLabel.text = "Play Again!";
                break;
            case GameStatus.play:
                playButtonLabel.text = "Play";
                break;
            case GameStatus.next:
                playButtonLabel.text = "Next Wave";
                break;
            case GameStatus.win:
                playButtonLabel.text = "Next Wave";
                break;
        }
        playButton.gameObject.SetActive(true);
    }

    public void handleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.getInstance().disableDragSprite();
            TowerManager.getInstance().selectedTowerBtn = null;
        }
    }
}
