using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public bool _preparingToCatch;  //キャッチする準備
    public bool _canCatchMallet;    //キャッチできる状態
    public bool _catching;          //キャッチしている状態

    CircleCollider2D collider2D;

    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
        _canCatchMallet = false;
        _catching = false;
    }

    // Update is called once per frame
    void Update()
    {
        collider2D.isTrigger = _preparingToCatch;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_preparingToCatch || _canCatchMallet)
        {
            if (collision.gameObject.tag == "Mallet")
            {
                _canCatchMallet = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Mallet")
        {
            _canCatchMallet = false;
        }

    }

    public void Catch()
    {
        if (_canCatchMallet)
        {
            _catching = true;
            _canCatchMallet = false;
        }
    }

}
