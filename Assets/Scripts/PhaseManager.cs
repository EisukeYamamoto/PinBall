using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    public int phaseMax;
    public int phaseNow;
    [Header("ターゲットリスト")]
    public Vector2[] targetPos;
    public List<GameObject> TargetList;
    public List<GameObject> TargetIconList;
    public Vector2 targetIconPos ;
    GameObject targetClone;
    GameObject targetIcon;
    HPSystem targetHP;
    private float hpMax;
    private float hpNow;
    public Slider slider;
    [Header("プレイヤー/マレット")]
    public GameObject Player;
    public GameObject Mallet;
    private GameObject playerClone;
    private GameObject malletClone;

    public GameObject PinballButton;

    public bool _pinballPhase;
    public bool _stageEditPhase;

    public TextMeshProUGUI message;
    public float waitTime = 1.0f;

    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI phaseMaxText;
    public TextMeshProUGUI phaseNowText;

    public GameObject ItemManager;
    ItemManager itemManager;

    public GameObject EnemyManager;
    EnemyHoleManager enemyManager;

    public GameObject RewardPlate;

    private bool _ready;
    private bool _clear;

    GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        phaseNow = 1;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _pinballPhase = false;
        _stageEditPhase = false;
        itemManager = ItemManager.GetComponent<ItemManager>();
        enemyManager = EnemyManager.GetComponent<EnemyHoleManager>();
        _ready = false;
        //PinballStart();
        _stageEditPhase = true;
        _pinballPhase = false;
        AppearTarget(0);
    }

    // Update is called once per frame
    void Update()
    {
        // 仮のクリア条件：Cキーを押す
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (_pinballPhase)
            {
                if (phaseNow < phaseMax)
                {
                    PinballClear();
                }
                else
                {
                    StageClear();
                }
            }        
        }
        PhaseText();
        if (targetClone != null)
        {
            hpNow = targetHP.hp;
            slider.value = hpNow / hpMax;
        }

        
    }

    public void PinballtoStageEdit()
    {
        _pinballPhase = false;
        
        Destroy(playerClone);
        Destroy(malletClone);

        StartCoroutine(RewardSelect());
    }

    public void StageEdittoPinball()
    {
        _pinballPhase = true;
        _stageEditPhase = false;
    }

    public void PinballClear()
    {
        StartCoroutine(ToStageEdit());
    }

    public void StageClear()
    {
        StartCoroutine(StageComplete());
    }

    public void PinballStart()
    {
        if (!_ready)
        {
            StartCoroutine(ReadyGo());
        }      
    }

    private void PhaseText()
    {
        if(_pinballPhase && !_stageEditPhase)
        {
            phaseText.text = "Pinball Phase";
            PinballButton.SetActive(false);
        }
        else if (!_pinballPhase && _stageEditPhase)
        {
            phaseText.text = "StageEdit Phase";
            PinballButton.SetActive(true);
        }
        else
        {
            phaseText.text = "";
            PinballButton.SetActive(false);
        }
        phaseMaxText.text = phaseMax.ToString();
        phaseNowText.text = phaseNow.ToString();
    }

    public void AppearTarget(int targetnum)
    {
        targetClone = Instantiate(TargetList[targetnum]);
        targetClone.name = TargetList[targetnum].name;
        if(targetIcon != null)
        {
            Destroy(targetIcon);
        }
        targetIcon = Instantiate(TargetIconList[targetnum]);
        targetIcon.name = TargetIconList[targetnum].name;
        targetIcon.transform.position = targetIconPos;
        targetHP = targetClone.GetComponent<HPSystem>();
        hpMax = targetHP.hp;
        hpNow = hpMax;
        slider.value = 1;
        targetClone.transform.position = targetPos[phaseNow - 1];
    }


    // ゲーム開始処理
    IEnumerator ReadyGo()
    {
        //yield return new WaitForEndOfFrame();
        _ready = true;
        itemManager.GroundColliderSwitchAll(true);
        itemManager.PanelColliderSwitch(true);
        playerClone = Instantiate(Player, new Vector2(0, -3f), Quaternion.identity) as GameObject;
        malletClone = Instantiate(Mallet, new Vector2(0, 1f), Quaternion.identity) as GameObject;
        enemyManager.EnemyHoleSets();

        itemManager.PMFind();

        yield return new WaitForSeconds(1.0f);

        message.text = "Ready?";

        yield return new WaitForSeconds(waitTime);

        message.text = "GO!!";

        _pinballPhase = true;
        _stageEditPhase = !_pinballPhase;

        ////プレイヤーを移動可能にさせる
        //gameManager.game_stop_flg = false;
        //gameManager.pause_flg = true;


        yield return new WaitForSeconds(waitTime);

        message.text = "";
        _ready = false;
    }

    // 戦闘終了処理
    IEnumerator ToStageEdit()
    {
        yield return new WaitForEndOfFrame();

        message.text = "Great!!";
        _pinballPhase = false;
        enemyManager.EnemyHoleReset();
        enemyManager.EnemyClear();

        yield return new WaitForSeconds(2.0f);

        message.text = "";

        PinballtoStageEdit();
    }

    IEnumerator StageComplete()
    {
        yield return new WaitForEndOfFrame();

        message.text = "Complete!!!";
        _pinballPhase = false;
        enemyManager.EnemyHoleReset();
        enemyManager.EnemyClear();

        yield return new WaitForSeconds(2.0f);

        message.text = "";

        gameManager.GameClear();
    }

    // 報酬選択処理
    IEnumerator RewardSelect()
    {
        yield return new WaitForEndOfFrame();
        itemManager.RewardListGenarate();

        itemManager.GroundColliderSwitchAll(false);
        itemManager.PanelColliderSwitch(false);
        GameObject RewardPlateClone = Instantiate(RewardPlate);
        RewardPlateClone.name = RewardPlate.name;
        RewardManager rewardManager = RewardPlateClone.GetComponent<RewardManager>();

        yield return new WaitUntil(() => rewardManager._selecting == false);
        itemManager.AddReward();
        RewardPlateClone.SetActive(false);
        itemManager.GroundColliderSwitchAll(true);
        itemManager.ExistPanelChack(false);
        _stageEditPhase = true;
        phaseNow += 1;
        AppearTarget(0);
    }
}
