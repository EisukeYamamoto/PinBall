using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletInit : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    Vector2 down;

    public Vector2 initPos;
    public float initSpeed = 200f;  // 最初のスピード

    public float waitTimeLimit = 3f;  // 失敗したときのロスタイム
    private float waitTimeNow = 0f;

    public bool _failure = false;    //　失敗したときのフラグ

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        down = new Vector2(0, -1);
        MalletStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (_failure)
        {
            waitTimeNow += Time.deltaTime;
            if(waitTimeNow >= waitTimeLimit)
            {
                MalletStart();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadLine"))
        {
            _failure = true;    
            MalletReset();
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
