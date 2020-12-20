using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class EnemyMotion_Stop : MonoBehaviour
{
    EnemySystem system;
    private float distance;
    Vector2 nowPos;
    float seata;
    GameManager gameManager;
    [SerializeField]
    private float speed = 0.1f;
    private bool _start = false;

    [Header("目的地")]
    public Vector2 targetPos;
    [Header("何秒かけていくか")]
    public float time = 5.0f;

    Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _start = false;
        system = this.GetComponent<EnemySystem>();
        StartCoroutine(InitSetting());
        tweener = transform.DOMove(targetPos, time);
    }

    // Update is called once per frame
    void Update()
    {

        if (_start)
        {
            //float sin = Mathf.Sin(Time.time);
            //this.transform.position = new Vector2(0, sin);
            if (!gameManager.game_stop_flg)
            {
                tweener.Play();
            }
            else
            {
                tweener.Pause();
            }
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
