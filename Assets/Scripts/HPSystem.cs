using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HPSystem : MonoBehaviour
{
    public float hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (this.gameObject.tag)
        {
            case "Target":
                if (collision.gameObject.CompareTag("Mallet"))
                {
                    float afterHp = hp - 1;
                    DOTween.To(() => hp, num => hp = num, afterHp, 0.1f);
                    if (afterHp <= 0)
                    {
                        PhaseManager phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
                        if (phase.phaseNow < phase.phaseMax)
                        {
                            phase.PinballClear();
                        }
                        else
                        {
                            phase.StageClear();
                        }
                        this.gameObject.SetActive(false);
                    }
                }
                break;
            case "Enemy":
                if (collision.gameObject.CompareTag("Mallet")|| collision.gameObject.CompareTag("AtkItem"))
                {
                    float afterHp = hp - 1;
                    DOTween.To(() => hp, num => hp = num, afterHp, 0.1f);
                    if (afterHp <= 0)
                    {
                        EnemyHoleManager enemyManager = GameObject.Find("EnemyHoleManager").GetComponent<EnemyHoleManager>();
                        enemyManager.existEnemyList.Remove(this.gameObject);
                        this.gameObject.SetActive(false);
                    }
                }
                else if (collision.gameObject.CompareTag("DeadLine"))
                {
                    EnemyHoleManager enemyManager = GameObject.Find("EnemyHoleManager").GetComponent<EnemyHoleManager>();
                    enemyManager.existEnemyList.Remove(this.gameObject);
                    this.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

    }
}
