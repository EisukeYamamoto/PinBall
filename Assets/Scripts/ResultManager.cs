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
    public GameObject AgainButton;
    public GameObject NextButton;
    public GameObject BackButton;

    GameManager gameManager;
    ScoreManager scoreManager;
    Button[] buttons;
    private bool _finish = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        DOTween.To(() => score, num => score = num, scoreManager.score, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        if (score == scoreManager.score && !_finish)
        {
            _finish = true;
            StartCoroutine(AppearButton());
        }
    }

    IEnumerator AppearButton()
    {
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
}
