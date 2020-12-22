using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    [System.Serializable]  //　範囲制限
    public class Bounds
    {
        public float xMin, xMax, yMin, yMax;
    }

    [SerializeField] Bounds bounds;

    PlayerStatus p_status;

    //GameObject Mallet;
    //MalletMotion m_motion;

    public float shotTimeLimit = 1f;
    private float shotTimeNow = 0f;
    public bool _afterShot;

    GameManager gameManager;
    PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        //Mallet = GameObject.FindGameObjectWithTag("Mallet");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        p_status = GetComponent<PlayerStatus>();
        //m_motion = Mallet.GetComponent<MalletMotion>();
        _afterShot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.game_stop_flg)
        {
            // マウス位置をスクリーン座標からワールド座標へ
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 範囲制限
            targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
            targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

            transform.position = targetPos;

            if (_afterShot)
            {
                shotTimeNow += Time.deltaTime;
                if (shotTimeNow >= shotTimeLimit)
                {
                    _afterShot = false;
                    shotTimeNow = 0f;
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                if (p_status._catching)
                {
                    Shot();
                }
                else
                {
                    p_status._preparingToCatch = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                p_status._preparingToCatch = false;
                p_status.Catch();
            }
        }
        
    }

    void Shot()
    {
        if(p_status.catchingMallet.Count > 0)
        {
            foreach(GameObject mallet in p_status.catchingMallet)
            {
                mallet.transform.rotation = Quaternion.identity;
                MalletMotion m_motion = mallet.GetComponent<MalletMotion>();
                m_motion.rigidbody2D.AddForce(new Vector2(0, 1f) * m_motion.initSpeed * 1.5f);
                m_motion.play2Target = Vector2.zero;
                _afterShot = true;
            }
            p_status.catchingMallet.Clear();
            
            p_status._catching = false;
            phaseManager.touchNow += 1;
        }    
    }
}
