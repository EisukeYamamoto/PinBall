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
    GameManager gameManager;
    public ParticleSystem Particle;
    public ParticleSystem Particle2;
    public ParticleSystem Particle3;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        switch (this.gameObject.tag)
        {
            case "Target":
                phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
                break;
            case "Enemy":
                phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
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
        if (!gameManager.game_stop_flg)
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
                    if (collision.gameObject.CompareTag("Mallet") || collision.gameObject.CompareTag("AtkItem"))
                    {
                        float afterHp = hp - 1;
                        DOTween.To(() => hp, num => hp = num, afterHp, 0.1f);
                        if (afterHp <= 0)
                        {
                            enemyManager.existEnemyList.Remove(this.gameObject);
                            scoreManager.score += enemySystem.score;
                            this.gameObject.SetActive(false);
                            Instantiate(Particle, transform.position, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(Particle3, transform.position, Quaternion.identity);
                        }
                    }
                    else if (collision.gameObject.CompareTag("DeadLine"))
                    {
                        enemyManager.existEnemyList.Remove(this.gameObject);
                        this.gameObject.SetActive(false);
                        phase.breakCountNow -= 1;
                        gameManager.audioSource.PlayOneShot(gameManager.receiveDamage_se);
                        Instantiate(Particle2, transform.position, Quaternion.identity);
                        if (phase.breakCountNow <= 0)
                        {
                            gameManager.GameOver();
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
