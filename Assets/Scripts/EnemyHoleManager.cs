using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyHoleSetting
{
    public string phaseNumber;
    [Header("ホールリスト")]
    public List<HoleInfomation> holeInfomation = new List<HoleInfomation>();
    [Header("出現する敵の種類")]
    public List<GameObject> enemyList = new List<GameObject>();
}

[System.Serializable]
public class HoleInfomation
{
    [Header("敵の巣窟の番号:0~6")]
    public int holeNumber;
    [Header("ゲーム開始から敵が出現し始めるまでの時間")]
    public float initTime;
    [Header("その穴から出てくる敵の出現頻度(繰り返す時間)")]
    public float appearTime;
}


public class EnemyHoleManager : MonoBehaviour
{
    public List<EnemyHoleSetting> enemyManageList = new List<EnemyHoleSetting>();
    PhaseManager phase;
    List<GameObject> enemyHolesList = new List<GameObject>();
    public List<GameObject> existEnemyList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        foreach (Transform Hole in this.gameObject.transform)
        {
            enemyHolesList.Add(Hole.gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyHoleSets()
    {
        EnemyHoleReset();
        for(int i = 0; i < enemyManageList[phase.phaseNow-1].holeInfomation.Count; i++)
        {
            int holenumber = enemyManageList[phase.phaseNow-1].holeInfomation[i].holeNumber;
            enemyHolesList[holenumber].SetActive(true);
            EnemyHole enemyHole = enemyHolesList[holenumber].GetComponent<EnemyHole>();
            enemyHole.appearTime = enemyManageList[phase.phaseNow-1].holeInfomation[i].appearTime;
            enemyHole.initTime = enemyManageList[phase.phaseNow - 1].holeInfomation[i].initTime;
            enemyHole.enemyList = enemyManageList[phase.phaseNow-1].enemyList;
        }    
    }

    public void EnemyHoleReset()
    {
        foreach (GameObject Hole in enemyHolesList)
        {
            Hole.SetActive(false);
        }
    }

    public void EnemyClear()
    {
        if (existEnemyList.Count > 0)
        {
            foreach(GameObject Enemy in existEnemyList)
            {
                Enemy.SetActive(false);
            }
            existEnemyList.Clear();
        }
    }

}
