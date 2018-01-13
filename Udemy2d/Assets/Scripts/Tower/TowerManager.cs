using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TowerManager : MonoBehaviour
{

    public static TowerManager instance;

    private TowerController selectedTowerBtn;

    private SpriteRenderer spriteRenderer;

    public static TowerManager getInstance()
    {
        if (instance == null)
        {
            instance = new TowerManager();
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

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit.collider.tag == "buildSite")
            {
                hit.collider.tag = "buildSiteFull";
                placeTower(hit);
            }
        }

        if(spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void placeTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && selectedTowerBtn != null)
        {
            GameObject newTower = Instantiate(selectedTowerBtn.TowerObject);
            newTower.transform.position = hit.transform.position;
            disableDragSprite();
        }
    }

    public void selectedTower(TowerController tower)
    {
        selectedTowerBtn = tower;
        Debug.Log("tower Selected " + selectedTowerBtn.TowerObject);
        enableDragSprite(selectedTowerBtn.DragSprite);
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }
}
