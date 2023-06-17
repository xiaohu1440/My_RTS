using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TowerFog : MonoBehaviour
{
    public static TowerFog instance;

    public TowerAttribute towerAttribute;
    public GameObject[] players;
    private bool isPaused = true;
    private float timeElapsed = 0f;
    private int CricleFog=3;
    public Tilemap fogOfWar;
    public float timeFog=1f;//每隔多少秒刷新一次
    [SerializeField]
    private bool isPlayerIn = true;
    [SerializeField]
    private bool isEnemyIn = false;
    private int beAttack;
    public Image HpImage;
    public Image TimeImage;
    private float timeStart=0f;
    private float timePercent=0f;
    private int enemyCount = 0;
    private bool isDealingDamage = false;
    private bool isTimePause=false;
    public bool isRevert=false;

    public GameObject Revert;

    public GameObject defeated;
    public GameObject victory;

    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        //获取场上所有标签为Player的物体给到players数组
        players = GameObject.FindGameObjectsWithTag("Player");
        towerAttribute = GetComponent<TowerAttribute>();
        //StartCoroutine("DealDamagerTowerTime");
    }
    private void OnTriggerStay2D(Collider2D other)
     {
        //Debug.Log("Player is in the trigger");
        if (other.gameObject.tag == "Player")
        
        {
            isPlayerIn = true;
            //Debug.Log("Player is in the trigger");
            
            isPaused = false;
            
        }
        if (other.gameObject.tag == "Enemy"&& !isDealingDamage)

        {
            enemyCount++;
            StartCoroutine(DealDamagerTowerTime(enemyCount));
            isDealingDamage = true;
            beAttack=other.gameObject.GetComponent<EnemyAttribute>().enemyData.enemyATK;
            isEnemyIn = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerIn = false;
            isPaused = true;
        }
        if (other.gameObject.tag == "Enemy"&& isDealingDamage)
        {
            enemyCount--;
            StopCoroutine(DealDamagerTowerTime(enemyCount));
            isDealingDamage = false;
            beAttack=0;
            isEnemyIn = false;
            

        }
    }

    private void Update()
    {
        if (!isPaused)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= timeFog)
            {
                CricleFog++;
                timeElapsed = 0f;
                
            }
            
        }
        UpdateFog();
        float hpPercent = towerAttribute.towerHP / towerAttribute.towerData.towerHP;
        HpImage.fillAmount = hpPercent;
        if(isPlayerIn&&!isEnemyIn)
        {
            timeStart+=Time.deltaTime;
            timePercent=timeStart/towerAttribute.towerData.towerTime;
        }
        TimeImage.fillAmount=timePercent;

        if(timePercent>=1f)
        {
            victory.SetActive(true);
            Time.timeScale = 0;
        }
        if(hpPercent<=0f)
        {
            defeated.SetActive(true);
            Time.timeScale = 0;
        }
        if(timePercent>=0.5f&&isTimePause==false)
        {
            isTimePause=true;
            Revert.SetActive(true);
            Time.timeScale = 0;
        }
        
    }
    private void UpdateFog()
    {
        Vector3Int currentTower=fogOfWar.WorldToCell(transform.position);
        for(int i=-CricleFog;i<=CricleFog;i++)
        {
            for(int j=-CricleFog;j<=CricleFog;j++)
            {
                fogOfWar.SetTile(currentTower+new Vector3Int(i,j,0) ,null);
            }
        }
    }

    public void Recover()
    {
        Time.timeScale = 1;
        isRevert=true;
        //players上的每一个物体都执行一次RecoverFog函数
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerAttribute>().playerHP =player.GetComponent<PlayerAttribute>().playerData.playerHP;
        }
    }

    private IEnumerator DealDamagerTowerTime(int enemyCount)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 等待1秒

            if (!isPlayerIn&&isEnemyIn)
            {
                towerAttribute.towerHP -= beAttack*enemyCount;
            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
