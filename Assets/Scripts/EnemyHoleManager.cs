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
    public int holeNumber;
    public float appearTime;
}


public class EnemyHoleManager : MonoBehaviour
{
    public List<EnemyHoleSetting> enemyManageList = new List<EnemyHoleSetting>();
    PhaseManager phase;
    List<GameObject> enemyHolesList = new List<GameObject>();
    
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

}
