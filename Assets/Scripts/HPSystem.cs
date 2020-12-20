using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HPSystem : MonoBehaviour
{
    public float hp;
    PhaseManager phase;
    EnemySystem enemySystem;
    EnemyHoleManager enemyManager;
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        switch (this.gameObject.tag)
        {
            case "Target":
                phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
                break;
            case "Enemy":
                enemySystem = GetComponent<EnemySystem>();
                enemyManager = GameObject.Find("EnemyHoleManager").GetComponent<EnemyHoleManager>();
                scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
                break;
            default:
                break;
        }
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
                if (collision.gameObject.CompareTag("Mallet"))
                {
                    float afterHp = hp - 1;
                    DOTween.To(() => hp, num => hp = num, afterHp, 0.1f);
                    if (afterHp <= 0)
                    {
                        enemyManager.existEnemyList.Remove(this.gameObject);
                        scoreManager.score += enemySystem.score;
                        this.gameObject.SetActive(false);
                    }
                }
                else if (collision.gameObject.CompareTag("DeadLine"))
                {
                    enemyManager.existEnemyList.Remove(this.gameObject);
                    this.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }

    }
}
