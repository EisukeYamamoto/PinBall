using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletMotion : MonoBehaviour
{
    GameObject Player;
    PlayerStatus p_status;

    PhaseManager phase;
    GameManager gameManager;
    ScoreManager scoreManager;

    public Rigidbody2D rigidbody2D;
    Vector2 down;

    CircleCollider2D collider;

    public Vector2 initPos;
    public float initSpeed = 200f;  // 最初のスピード

    public float waitTimeLimit = 3f;  // 失敗したときのロスタイム
    private float waitTimeNow = 0f;

    public bool _failure = false;    //　失敗したときのフラグ

    private bool _start = false;

    private bool _pause = false;

    private bool _waiting = false;

    public float UpSpeed, DownSpeed;
    public float MaxSpeed, MinSpeed;

    public float speedLimit;  // 最低速度

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        p_status = Player.GetComponent<PlayerStatus>();
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        down = new Vector2(0, -1);
        _waiting = true;
        //MalletStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.game_stop_flg)
        {
            if (_pause)
            {
                _pause = false;
                gameObject.GetComponent<Rigidbody2D>().Resume(this.gameObject);
            }

            if (!_start && phase._pinballPhase && !phase._stageEditPhase)
            {
                MalletStart();
                _start = true;
            }

            if (_failure)
            {
                waitTimeNow += Time.deltaTime;
                if (waitTimeNow >= waitTimeLimit)
                {
                    MalletStart();
                }
            }

            if (p_status._catching)
            {
                rigidbody2D.velocity = Vector2.zero;
                Vector2 p_pos = Player.transform.position;
                this.transform.position = p_pos + p_status.player2Mallet;
            }

            //速度低下
            if (rigidbody2D.velocity.magnitude > MinSpeed)
                rigidbody2D.velocity *= DownSpeed;


            //止まったときの処理
            //最低速度に達すると、初期位置に再配置する
            if (rigidbody2D.velocity.magnitude <= speedLimit &&
                rigidbody2D.velocity.magnitude > 0 &&
                !p_status._catching && !_waiting && _start)
            {
                if (phase._pinballPhase)
                {
                    _failure = true;
                    MalletReset();
                }
            }

            //// 速度低下テスト用
            //if (rigidbody2D.velocity.magnitude > 0 &&
            //    !p_status._catching && !_waiting && _start)
            //{
            //    rigidbody2D.velocity *= DownSpeed;
            //}


        }
        else
        {
            if (!_pause)
            {
                _pause = true;
                gameObject.GetComponent<Rigidbody2D>().Pause(this.gameObject);
            }
        }
        //Debug.Log(rigidbody2D.velocity.magnitude.ToString("f0"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameManager.game_stop_flg)
        {
            if (collision.gameObject.CompareTag("DeadLine"))
            {
                if (phase._pinballPhase)
                {
                    _failure = true;
                    MalletReset();
                }
            }
            // 仮
            if (collision.gameObject.CompareTag("Wall"))
            {
                
            }
            if (collision.gameObject.CompareTag("Bumper"))
            {
                if (phase._pinballPhase)
                {
                    scoreManager.score += 100;
                    //速度上昇
                    if (GetComponent<Rigidbody2D>().velocity.magnitude < MaxSpeed)
                        rigidbody2D.velocity *= UpSpeed;
                }   
            }
        }
    }



    // マレットの開始処理
    private void MalletStart()
    {
        _failure = false;
        waitTimeNow = 0f;
        collider.enabled = true;
        rigidbody2D.AddForce(down * initSpeed);
        _waiting = false;
    }

    // マレットの動きリセット
    private void MalletReset()
    {
        waitTimeNow = 0f;
        rigidbody2D.velocity = Vector2.zero;
        collider.enabled = false;
        _waiting = true;
        this.transform.position = initPos;
        this.transform.rotation = Quaternion.identity;
    }
}