using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResultManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public TextMeshProUGUI limitText;
    public TextMeshProUGUI touchText;
    public TextMeshProUGUI touchScoreText;
    private int touchScore = 0;
    public TextMeshProUGUI finalText;
    private int finalScore = 0;
    public List<GameObject> RankList = new List<GameObject>();

    public GameObject AgainButton;
    public GameObject NextButton;
    public GameObject BackButton;

    GameManager gameManager;
    ScoreManager scoreManager;
    PhaseManager phaseManager;
    Button[] buttons;
    public int touchScore_public;
    private float f_score;
    public int finalScore_public;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        f_score = ((float)phaseManager.touchLimit / (float)phaseManager.touchNow) * 100;
        touchScore_public = (int)f_score;
        finalScore_public = scoreManager.score * touchScore_public;
        StartCoroutine(Result());
        //DOTween.To(() => score, num => score = num, scoreManager.score, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        touchScoreText.text = touchScore.ToString();
        scoreText.text = score.ToString();
        finalText.text = finalScore.ToString();
        //if (score == scoreManager.score && !_finish)
        //{
        //    _finish = true;
        //    StartCoroutine(AppearButton());
        //}
    }

    IEnumerator Result()
    {
        yield return new WaitForEndOfFrame();

        limitText.text = phaseManager.touchLimit.ToString();

        yield return new WaitForSeconds(1.0f);

        touchText.text = phaseManager.touchNow.ToString();

        yield return new WaitForSeconds(1.0f);

        DOTween.To(() => touchScore, num => touchScore = num, (int)touchScore_public, 1.0f);

        yield return new WaitUntil(() => touchScore == touchScore_public);

        yield return new WaitForSeconds(1.0f);

        DOTween.To(() => score, num => score = num, scoreManager.score, 1.0f);

        yield return new WaitUntil(() => score == scoreManager.score);

        yield return new WaitForSeconds(1.0f);

        DOTween.To(() => finalScore, num => finalScore = num, finalScore_public, 2.0f);

        yield return new WaitUntil(() => finalScore == finalScore_public);

        yield return new WaitForSeconds(1.5f);

        RankSet();

        yield return new WaitForSeconds(1.0f);

        if (this.gameObject.name == "ResultPanel")
        {
            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(1.0f);

            AgainButton.SetActive(true);
            NextButton.SetActive(true);
            BackButton.SetActive(true);

            buttons = GetComponentsInChildren<Button>();

            //ボタンにイベント設定
            buttons[0].onClick.AddListener(gameManager.ConfilmPauseRestart);
            buttons[1].onClick.AddListener(gameManager.ConfilmPauseBack);
            buttons[2].onClick.AddListener(gameManager.ConfilmNextStage);
        }
        else
        {
            yield return new WaitForEndOfFrame();

            yield return new WaitForSeconds(1.0f);

            AgainButton.SetActive(true);
            //NextButton.SetActive(true);
            BackButton.SetActive(true);

            buttons = GetComponentsInChildren<Button>();

            //ボタンにイベント設定
            buttons[0].onClick.AddListener(gameManager.ConfilmPauseRestart);
            buttons[1].onClick.AddListener(gameManager.ConfilmPauseBack);
            //buttons[2].onClick.AddListener(gameManager.ConfilmNextStage);
        }
    }

    private void RankSet()
    {
        GameObject Rank;
        if (finalScore_public >= phaseManager.rankList[0])
        {
            Rank = RankList[0];
        }
        else if(finalScore_public < phaseManager.rankList[0] && finalScore_public >= phaseManager.rankList[1])
        {
            Rank = RankList[1];
        }
        else if (finalScore_public < phaseManager.rankList[1] && finalScore_public >= phaseManager.rankList[2])
        {
            Rank = RankList[2];
        }
        else if (finalScore_public < phaseManager.rankList[2] && finalScore_public >= phaseManager.rankList[3])
        {
            Rank = RankList[3];
        }
        else
        {
            Rank = RankList[4];
        }
        Rank.SetActive(true);
        Rank.transform.DOScale(new Vector2(1f, 1f), 0.5f).SetEase(Ease.OutBack);
    }
}
