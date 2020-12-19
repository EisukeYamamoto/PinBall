using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotion_Zako : MonoBehaviour
{
    EnemySystem system;
    private float distance;
    Vector2 nowPos;
    float seata;
    [SerializeField]
    private float speed = 0.1f;
    private bool _start = false;

    // Start is called before the first frame update
    void Start()
    {
        _start = false;
        system = this.GetComponent<EnemySystem>();
        StartCoroutine(InitSetting());
    }

    // Update is called once per frame
    void Update()
    {
        if (_start)
        {
            distance -= speed;
            nowPos.x = distance * Mathf.Cos(seata) + system.targetPos.x;
            nowPos.y = distance * Mathf.Sin(seata) + system.targetPos.y;
            this.transform.position = nowPos;
        }  
    }

    IEnumerator InitSetting()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        this.transform.localPosition = system.initPos;
        distance = Vector2.Distance(system.targetPos, system.initPos);
        seata = Mathf.Atan2((system.initPos.y - system.targetPos.y), (system.initPos.x - system.targetPos.x));
        _start = true;
    }
}
