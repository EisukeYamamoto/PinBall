using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SelectManager : MonoBehaviour
{
    public List<string> stageList;
    public GameObject StagePanel;
    public GameObject ArrowButton;
    public TextMeshProUGUI StageName;
    Button[] button;
    GameManager gameManager;
    private bool _moving = false;

    private int interval = 9;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int i = 0;
        foreach(string StageName in stageList)
        {
            GameObject StagePanelClone = Instantiate(StagePanel);
            StagePanelClone.name = StageName;
            StagePanelClone.transform.position = this.transform.position + new Vector3(interval * i, 0, 0);
            StagePanelClone.transform.parent = this.transform;
            i += 1;
        }
        // ボタンを取得
        button = ArrowButton.GetComponentsInChildren<Button>();

        // ボタンにイベント設定
        button[0].onClick.AddListener(Right);
        button[1].onClick.AddListener(Left);

    }

    // Update is called once per frame
    void Update()
    {
        StageName.text = stageList[gameManager.stageSelectNum - 1];
    }

    void Right()
    {
        if (gameManager.stageSelectNum < stageList.Count && !_moving)
        {
            gameManager.stageSelectNum += 1;
            Vector2 panelPos = this.transform.position;
            panelPos.x -= interval;
            Tween currentPlayTween = transform.DOMove(panelPos, 0.2f);
            currentPlayTween.SetEase(Ease.Linear);
            currentPlayTween.OnStart(() => _moving = true);
            currentPlayTween.OnComplete(() => _moving = false);
            currentPlayTween.Play();
            //this.transform.position = panelPos;
        }
    }

    void Left()
    {
        if (gameManager.stageSelectNum > 1 && !_moving)
        {
            gameManager.stageSelectNum -= 1;
            Vector2 panelPos = this.transform.position;
            panelPos.x += interval;
            Tween currentPlayTween = transform.DOMove(panelPos, 0.2f);
            currentPlayTween.SetEase(Ease.Linear);
            currentPlayTween.OnStart(() => _moving = true);
            currentPlayTween.OnComplete(() => _moving = false);
            currentPlayTween.Play();
            //this.transform.position = panelPos;
        }
    }
}
