using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHole : MonoBehaviour
{
    public float initTime;
    public float appearTime;
    public List<GameObject> enemyList = new List<GameObject>();
    EnemyHoleManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        enemyManager = transform.parent.GetComponent<EnemyHoleManager>();
        InvokeRepeating("EnemyGanarate", initTime, appearTime);
    }

    void EnemyGanarate()
    {
        int enemyNum = (int)Random.Range(0, enemyList.Count);
        GameObject Enemy = Instantiate(enemyList[enemyNum]);
        Enemy.name = enemyList[enemyNum].name;
        enemyManager.existEnemyList.Add(Enemy);
        Enemy.transform.position = this.transform.position;
    }
}
