using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLineTarget : MonoBehaviour
{
    public List<GameObject> EnemyTargetList = new List<GameObject>();

    void Start()
    {
        foreach(Transform target in this.transform)
        {
            EnemyTargetList.Add(target.gameObject);
        }
    }
}
