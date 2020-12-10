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

    public GameObject Mallet;
    MalletMotion m_motion;

    // Start is called before the first frame update
    void Start()
    {
        p_status = GetComponent<PlayerStatus>();
        m_motion = Mallet.GetComponent<MalletMotion>();
    }

    // Update is called once per frame
    void Update()
    {
        // マウス位置をスクリーン座標からワールド座標へ
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 範囲制限
        targetPos.x = Mathf.Clamp(targetPos.x, bounds.xMin, bounds.xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, bounds.yMin, bounds.yMax);

        transform.position = targetPos;

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

    void Shot()
    {
        m_motion.rigidbody2D.AddForce(new Vector2(0, 1f) * m_motion.initSpeed * 1.5f);
        p_status._catching = false;
    }
}
