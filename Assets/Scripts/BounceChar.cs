using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceChar : MonoBehaviour
{
    private Vector2 vec, nowVelocity;
    private Rigidbody2D rb;
    private Vector3 old_pos; //弾の過去座標
    public float speed;
    void Start()
    {
        old_pos = GetComponent<Transform>().position;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        nowVelocity = rb.velocity;
        //進行方向によって向きを変える
        vec = (old_pos - this.transform.position).normalized;
        this.transform.rotation = Quaternion.FromToRotation(Vector3.down, vec);

        old_pos = transform.position; //過去座標を更新
        //毎秒減速
        rb.velocity *= 0.99f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            float playerVec = coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
            if (playerVec > rb.velocity.magnitude)
                rb.velocity = coll.gameObject.GetComponent<Rigidbody2D>().velocity;
        }

        //speed = rb.velocity.magnitude;
        //rb.AddForce((transform.up) * speed * 0.5f, ForceMode2D.Impulse); //プレイヤーのRigidbodyに対してInputにspeedを掛けた値で更新し移動
        if (coll.gameObject.tag == "Wall"|| coll.gameObject.tag == "Player")
        {
            Vector2 refrectVec = Vector2.Reflect(this.nowVelocity, coll.contacts[0].normal);
            this.rb.velocity = refrectVec;
        }


    }

}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MalletMotion : MonoBehaviour
//{
//    GameObject Player;
//    PlayerStatus p_status;

//    PhaseManager phase;
//    GameManager gameManager;
//    ScoreManager scoreManager;

//    public Rigidbody2D rigidbody2D;
//    Vector2 down;

//    public Vector2 initPos;
//    public float initSpeed = 200f;  // 最初のスピード

//    public float waitTimeLimit = 3f;  // 失敗したときのロスタイム
//    private float waitTimeNow = 0f;

//    public bool _failure = false;    //　失敗したときのフラグ

//    private bool _start = false;

//    private bool _pause = false;


//    public float UpSpeed, DownSpeed;
//    public float MaxSpeed, MinSpeed;

//    // Start is called before the first frame update
//    void Start()
//    {
//        Player = GameObject.FindGameObjectWithTag("Player");
//        p_status = Player.GetComponent<PlayerStatus>();
//        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
//        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
//        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
//        rigidbody2D = GetComponent<Rigidbody2D>();
//        down = new Vector2(0, -1);
//        //MalletStart();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (!gameManager.game_stop_flg)
//        {
//            if (_pause)
//            {
//                _pause = false;
//                gameObject.GetComponent<Rigidbody2D>().Resume(this.gameObject);
//            }

//            if (!_start && phase._pinballPhase && !phase._stageEditPhase)
//            {
//                MalletStart();
//                _start = true;
//            }

//            if (_failure)
//            {
//                waitTimeNow += Time.deltaTime;
//                if (waitTimeNow >= waitTimeLimit)
//                {
//                    MalletStart();
//                }
//            }

//            if (p_status._catching)
//            {
//                rigidbody2D.velocity = Vector2.zero;
//                Vector2 p_pos = Player.transform.position;
//                this.transform.position = p_pos + p_status.player2Mallet;
//            }

//            if (rigidbody2D.velocity.magnitude > MinSpeed)
//            {
//                rigidbody2D.velocity *= DownSpeed;
//                Debug.Log("速度低下中");
//            }
//        }
//        else
//        {
//            if (!_pause)
//            {
//                _pause = true;
//                gameObject.GetComponent<Rigidbody2D>().Pause(this.gameObject);
//            }
//        }
//        Debug.Log(rigidbody2D.velocity.magnitude);
//    }

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (!gameManager.game_stop_flg)
//        {
//            if (collision.gameObject.CompareTag("DeadLine"))
//            {
//                if (phase._pinballPhase)
//                {
//                    _failure = true;
//                    MalletReset();
//                }
//            }
//            // 仮
//            if (collision.gameObject.CompareTag("Wall"))
//            {
//                if (phase._pinballPhase)
//                {
//                    scoreManager.score += 100;
//                }
//            }
//            if (collision.gameObject.CompareTag("Bumper"))
//            {
//                if (rigidbody2D.velocity.magnitude < MaxSpeed)
//                    rigidbody2D.velocity *= UpSpeed;
//            }
//            //衝突時プレイヤーの速度をパックにのせる
//            if (collision.gameObject.CompareTag("Player"))
//            {
//                float playerVec = collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
//                //if (playerVec > rigidbody2D.velocity.magnitude)
//                //    rigidbody2D.velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
//            }
//        }
//    }



//    // マレットの開始処理
//    private void MalletStart()
//    {
//        _failure = false;
//        waitTimeNow = 0f;
//        rigidbody2D.AddForce(down * initSpeed);
//    }

//    // マレットの動きリセット
//    private void MalletReset()
//    {
//        waitTimeNow = 0f;
//        rigidbody2D.velocity = Vector2.zero;
//        this.transform.position = initPos;
//        this.transform.rotation = Quaternion.identity;
//    }
//}


/*
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

    public Vector2 initPos;
    public float initSpeed = 200f;  // 最初のスピード

    public float waitTimeLimit = 3f;  // 失敗したときのロスタイム
    private float waitTimeNow = 0f;

    public bool _failure = false;    //　失敗したときのフラグ

    private bool _start = false;

    private bool _pause = false;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        p_status = Player.GetComponent<PlayerStatus>();
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        down = new Vector2(0, -1);
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
        }
        else
        {
            if (!_pause)
            {
                _pause = true;
                gameObject.GetComponent<Rigidbody2D>().Pause(this.gameObject);
            }
        }

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
                if (phase._pinballPhase)
                {
                    scoreManager.score += 100;
                }
            }
        }
    }

    // マレットの開始処理
    private void MalletStart()
    {
        _failure = false;
        waitTimeNow = 0f;
        rigidbody2D.AddForce(down * initSpeed);
    }

    // マレットの動きリセット
    private void MalletReset()
    {
        waitTimeNow = 0f;
        rigidbody2D.velocity = Vector2.zero;
        this.transform.position = initPos;
        this.transform.rotation = Quaternion.identity;
    }
}
*/