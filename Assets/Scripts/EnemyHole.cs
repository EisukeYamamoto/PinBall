using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHole : MonoBehaviour
{
    public float initTime;
    public float appearTime;
    public List<GameObject> enemyList = new List<GameObject>();
    EnemyHoleManager enemyManager;
    int targetNum;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        enemyManager = transform.parent.GetComponent<EnemyHoleManager>();
        //InvokeRepeating(nameof(EnemyGanarate), initTime, appearTime);
        StartCoroutine(Repeat());
    }

    void EnemyGanarate()
    {
        int enemyNum = (int)Random.Range(0, enemyList.Count);
        GameObject Enemy = Instantiate(enemyList[enemyNum]);
        Enemy.name = enemyList[enemyNum].name;
        Enemy.transform.position = this.transform.position;
        EnemySystem system = Enemy.GetComponent<EnemySystem>();
        system.initPos = this.transform.position;

        switch (this.gameObject.name)
        {
            case "EnemyHoleLeftMiddle":
                targetNum = (int)Random.Range(4, enemyManager.enemyTargetList.EnemyTargetList.Count);
                break;
            case "EnemyHoleLeftAbove":
                targetNum = (int)Random.Range(3, enemyManager.enemyTargetList.EnemyTargetList.Count);
                break;
            case "EnemyHoleRightMiddle":
                targetNum = (int)Random.Range(0, 5);
                break;
            case "EnemyHoleRightAbove":
                targetNum = (int)Random.Range(0, 6);
                break;
            default:
                targetNum = (int)Random.Range(0, enemyManager.enemyTargetList.EnemyTargetList.Count);
                break;
        }
           
        system.targetPos = enemyManager.enemyTargetList.EnemyTargetList[targetNum].transform.position;
        
        enemyManager.existEnemyList.Add(Enemy);
        
    }

    IEnumerator Repeat()
    {
        yield return new WaitForSeconds(initTime);
        while (true)
        {
            if (!gameManager.game_stop_flg)
                EnemyGanarate();

            yield return new WaitForSeconds(appearTime);
        }
    }
}
