using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MoveRepeat : MonoBehaviour
{
    EnemySystem system;
    private float distance;
    Vector2 nowPos;
    float seata;
    GameManager gameManager;
    private bool _start = false;

    [Header("初回目的地")]
    public Vector2 targetPos;
    [Header("何秒かけていくか")]
    public float time = 5.0f;

    Tweener tweener;

    Transform myTrans;
    [Header("2回目以降のループ移動")]
    [SerializeField] Vector2[] positions;
    [SerializeField] float[] durations;

    int step;
    float time1;
    int positionCount;

    bool move;

    void Start()
    {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _start = false;
        system = this.GetComponent<EnemySystem>();
        StartCoroutine(InitSetting());
        tweener = transform.DOMove(targetPos, time).OnComplete(() => MoveTrrger(true));
    }
    void Awake()
    {
        if (positions.Length != durations.Length)
            Debug.LogError("positionsとdurationsの要素数が異なっています");

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        step = 0;
        time1 = 0f;
        positionCount = positions.Length;
        move = false;

    }

    void Update()
    {
        if (!gameManager.game_stop_flg)
        {
            if (move)
            {
                if (positionCount == 0) return;

                time1 += Time.deltaTime / durations[step % positionCount];

                myTrans.localPosition = Vector3.Lerp(
                    positions[step % positionCount],
                    positions[(step + 1) % positionCount],
                    time1
                );

                if (time1 >= 1f)
                {
                    myTrans.localPosition = positions[(step + 1) % positionCount];
                    step++;
                    time1 = 0f;
                }

            }
            else tweener.Play();
        }
        else
        {
            if (!move) tweener.Pause();
        }
    }

    void MoveTrrger(bool a)
    {
        move = a;
        myTrans = transform;
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