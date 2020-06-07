using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum GameStatus
    {
        Play,Die
    }

    public int health = 3;
    public int money=0;
    private GameStatus gameStatus;
    [SerializeField] private List<GameObject> spawners;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private List<GameObject> EnemyPrefabs; // 0 is chiken , 1 is yello , 2 is blue , 3 is owl, 4 is eagle
    [SerializeField] private Text scoreText;
    [SerializeField] private Text waveText;
    [SerializeField] public Text healthText;
    [SerializeField] private Text gameNameText;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject restartBtn;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject shopPanel;
    int score = 0;
    private int thisWave=1;
    public float secondsToSpawn = 2f;
    public int birdCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameStatus = GameStatus.Die;
        waveText.text = "Wave: " + thisWave;
        healthText.text = "Health: " + health;
       // StartCoroutine("WaveManager");
        //  StartCoroutine("SummonMonsters");
        //StartCoroutine("SummonMonstersByWave");
        //StartCoroutine("CheckWaveEnding");

    }

    // Update is called once per frame
    void Update()
    {
        if(health<=0 && gameStatus==GameStatus.Play)
        {
            gameStatus = GameStatus.Die;
            // StopCoroutine("SummonMonsters");
            // StopCoroutine("SummonMonstersByWave");
            StopCoroutine("CheckWaveEnding");
             GameObject[] enemies= GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);

            money += score / 10+1;
            restartBtn.SetActive(true);
            menuBtn.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<EnemyMovement>().currentEnemyHealth--;
                    if(hit.collider.gameObject.GetComponent<EnemyMovement>().currentEnemyHealth==0)
                    {
                        score += hit.collider.gameObject.GetComponent<EnemyMovement>().EnemyHealth;
                        birdCount--;
                        Destroy(hit.collider.gameObject);
                       
                    }
                   
                }
                
                scoreText.text = "Score: " + score ;
            }

        }

    }
    
   IEnumerator SummonMonsters()
    {
        int randomSpawn = Random.Range(0, 4);

        GameObject enemy = GameObject.Instantiate(EnemyPrefab);
        enemy.transform.position = new Vector3(spawners[randomSpawn].transform.position.x, spawners[randomSpawn].transform.position.y, 0);
       
        yield return new WaitForSeconds(secondsToSpawn);
        if(secondsToSpawn>=0.02)
        secondsToSpawn -= 0.01f;
        StartCoroutine("SummonMonsters");
    }

    IEnumerator SummonMonstersByWave()
    {
        int randomSpawn;
        int randomEnemyNum;
        GameObject enemy;

        for (int i=0;i<thisWave;i++)
        {
            randomSpawn = Random.Range(0, 4);
            randomEnemyNum = Random.Range(0, 5);
            enemy = GameObject.Instantiate(EnemyPrefabs[randomEnemyNum]);
            enemy.transform.position = new Vector3(spawners[randomSpawn].transform.position.x, spawners[randomSpawn].transform.position.y, 0);
        }

       
       
        yield return new WaitForSeconds(secondsToSpawn);
        if (secondsToSpawn >= 0.02)
            secondsToSpawn -= 0.01f;
       
    }

    IEnumerator CheckWaveEnding()
    {

        if(birdCount==0)
        {
            thisWave++;
            waveText.text = "Wave: " + thisWave;
            StartCoroutine("WaveManager"); 
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine("CheckWaveEnding");
    }

    IEnumerator WaveManager()
    {
        StopCoroutine("CheckWaveEnding");

        GameObject enemy;
        int enemiesToSpawn = thisWave;
        birdCount = 0;
        int lastSpawnLoc = 5; // 5 not exist so the first is always false

        if(enemiesToSpawn%10==0) // boss eagle
        {
            for(int i =1; i<=enemiesToSpawn/10;i++)
            {
                birdCount++;
                int randomSpawn = Random.Range(0, 4);
                if(lastSpawnLoc==randomSpawn)
                {
                    if (randomSpawn == 0)
                        randomSpawn = 1;
                    else
                        randomSpawn--;
                }
                lastSpawnLoc = randomSpawn;
                enemy = GameObject.Instantiate(EnemyPrefabs[4]);
                enemy.transform.position = new Vector3(spawners[randomSpawn].transform.position.x, spawners[randomSpawn].transform.position.y, 0);
                yield return new WaitForSeconds(1f);
            }
        }
        else if(enemiesToSpawn%5==0) // mini boss owl
        {
            for (int i = 1; i <= enemiesToSpawn / 5; i++)
            {
                birdCount++;
                int randomSpawn = Random.Range(0, 4);
                if (lastSpawnLoc == randomSpawn)
                {
                    if (randomSpawn == 0)
                        randomSpawn = 1;
                    else
                        randomSpawn--;
                }
                lastSpawnLoc = randomSpawn;
                enemy = GameObject.Instantiate(EnemyPrefabs[3]);
                enemy.transform.position = new Vector3(spawners[randomSpawn].transform.position.x, spawners[randomSpawn].transform.position.y, 0);
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            for (int i = 1; i <= enemiesToSpawn ; i++)
            {
                birdCount++;
                int randomSpawn = Random.Range(0, 4);
                if (lastSpawnLoc == randomSpawn)
                {
                    if (randomSpawn == 0)
                        randomSpawn = 1;
                    else
                        randomSpawn--;
                }
                lastSpawnLoc = randomSpawn;

                if (i%3==0)
                     enemy = GameObject.Instantiate(EnemyPrefabs[2]); // blue bird
                else if(i%2==0)
                    enemy = GameObject.Instantiate(EnemyPrefabs[1]); // yellow bird
                else
                    enemy = GameObject.Instantiate(EnemyPrefabs[0]); // chicken
                enemy.transform.position = new Vector3(spawners[randomSpawn].transform.position.x, spawners[randomSpawn].transform.position.y, 0);
                yield return  new WaitForSeconds(1f);
            }
        }
        StartCoroutine("CheckWaveEnding");
    }

    public void StartBtn()
    {
        birdCount = 0;
        thisWave = 1;
        health = 3;
        score = 0;
        scoreText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        gameNameText.gameObject.SetActive(false);
        scoreText.text = "Score: " + score;
        waveText.text = "Wave: " + thisWave;
        healthText.text = "Health: " + health;
        gameStatus = GameStatus.Play;
        StartCoroutine("WaveManager");
        panel.SetActive(false);
        menuBtn.SetActive(false);
        restartBtn.SetActive(false);
    }
    public void ExitBtn()
    {
        Application.Quit();
    }
    public void MenuBtn()
    {
        scoreText.gameObject.SetActive(false);
        waveText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        gameNameText.gameObject.SetActive(true);
        panel.SetActive(true);
        restartBtn.SetActive(false);
        menuBtn.SetActive(false);
       

    }

    public void RestartBtn()
    {
        menuBtn.SetActive(false);
        restartBtn.SetActive(false);
        StartBtn();
    }
    public void ShopBtn()
    {
        panel.SetActive(false);
        shopPanel.SetActive(true);
        moneyText.text = "Money: " + money;
    }
    public void backFromShopBtn()
    {
        shopPanel.SetActive(false);
        panel.SetActive(true);
    }
}
