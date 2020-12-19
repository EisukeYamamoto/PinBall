using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHole : MonoBehaviour
{
    public float appearTime;
    public List<GameObject> enemyList = new List<GameObject>();

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
        
    }

    void EnemyGanarate()
    {
        int enemyNum = (int)Random.Range(0, enemyList.Count);
        GameObject Enemy = Instantiate(enemyList[enemyNum]);
        Enemy.name = enemyList[enemyNum].name;
        Enemy.transform.position = this.transform.position;
    }
}
