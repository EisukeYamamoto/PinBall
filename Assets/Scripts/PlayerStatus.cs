using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool _preparingToCatch;  //キャッチする準備
    public bool _canCatchMallet;    //キャッチできる状態
    public bool _catching;          //キャッチしている状態
    public Vector2 player2Mallet;   //プレイヤーとマレットの距離

    public List<GameObject> catchingMallet = new List<GameObject>();

    PlayerMotion p_motion;
    SpriteRenderer spriteRenderer;
    CircleCollider2D collider2D;
    GameManager gameManager;
    PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        p_motion = GetComponent<PlayerMotion>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        _canCatchMallet = false;
        _catching = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_preparingToCatch || _catching || p_motion._afterShot)
        {
            collider2D.isTrigger = true;
            ChangeTransparency(0.5f);
        }
        else
        {
            collider2D.isTrigger = false;
            ChangeTransparency(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mallet"))
        {
            phaseManager.touchNow += 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_preparingToCatch || _canCatchMallet)
        {
            if (collision.gameObject.tag == "Mallet")
            {
                _canCatchMallet = true;
                
                if (!catchingMallet.Contains(collision.gameObject))
                {
                    collision.gameObject.GetComponent<MalletMotion>().play2Target = collision.gameObject.transform.position - this.transform.position;
                    catchingMallet.Add(collision.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Mallet")
        {
            _canCatchMallet = false;
            if (catchingMallet.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<MalletMotion>().play2Target = Vector2.zero;
                catchingMallet.Remove(collision.gameObject);
            }
        }

    }

    public void Catch()
    {
        if (_canCatchMallet && !gameManager.game_stop_flg)
        {
            _catching = true;
            _canCatchMallet = false;
        }
    }

    private void ChangeTransparency(float alpha)
    {
        Material material = spriteRenderer.material;
        material.color = new Color(1, 1, 1, alpha);
    }

}
