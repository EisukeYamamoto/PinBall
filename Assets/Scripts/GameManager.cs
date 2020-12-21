using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [System.NonSerialized]
    public int currentStageNum = 0; //現在のステージ番号（0始まり）
    public int stageSelectNum = 1;

    public List<string> stageList;
    public GameObject fadeCanvasPrefab;
    public GameObject gameOverCanvasPrefab;
    public GameObject resultCanvasPrefab;
    public GameObject resultCanvasPrefab_Final;
    public GameObject PauseCanvasPrefab;
    public GameObject ConfilmCanvasPrefab_StageSelect;
    public GameObject ConfilmCanvasPrefab_Pause_Restart;
    public GameObject ConfilmCanvasPrefab_Pause_Back;
    public GameObject ConfilmCanvasPrefab_NextStage;

    [SerializeField]
    float fadeWaitTime = 1.0f; //フェード時の待ち時間

    GameObject fadeCanvasClone;
    FadeManager fadeCanvas;
    GameObject gameOverCanvasClone;
    GameObject resultCanvasClone;
    GameObject PauseCanvasClone;
    GameObject Confilm;
    //GameObject gameStartCanvas;
    // Target target_image;

    // SE
    //AudioSource audioSource;
    //public AudioClip start_se;
    //public AudioClip positive_se;
    //public AudioClip negative_se;
    //public AudioClip pause_se;

    Button Pause_button;
    Button[] buttons;
    //
    public bool game_stop_flg = false;
    public bool pause_flg;

    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        //シーンを切り替えてもこのゲームオブジェクトを削除しないようにする
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadComponents();

        //デリゲートの登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //シーンのロード時に実行（最初は実行されない）
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //改めて取得
        LoadComponents();
    }

    //コンポーネントの取得
    void LoadComponents()
    {
        //タイトル画面じゃないなら取得
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartScene":
                currentStageNum = 0;
                GameStart();
                break;
            case "StageSelect":
                currentStageNum = 1;
                game_stop_flg = false;
                SelectStart();
                break;
            case "Stage1":
                currentStageNum = 2;
                stageSelectNum = 1;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage2":
                currentStageNum = 3;
                stageSelectNum = 2;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage3":
                currentStageNum = 4;
                stageSelectNum = 3;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage4":
                currentStageNum = 5;
                stageSelectNum = 4;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage5":
                currentStageNum = 6;
                stageSelectNum = 5;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage6":
                currentStageNum = 7;
                stageSelectNum = 6;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage7":
                currentStageNum = 8;
                stageSelectNum = 7;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            case "Stage8":
                currentStageNum = 9;
                stageSelectNum = 8;
                game_stop_flg = false;
                pause_flg = true;
                Pause_button = GameObject.Find("PauseButton").GetComponent<Button>();
                Pause_button.onClick.AddListener(Pause);
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //次のステージに進む処理
    public void NextStage()
    {
        // SE
        //audioSource.PlayOneShot(start_se);
        stageSelectNum += 1;

        //コルーチンを実行
        StartCoroutine(WaitForLoadScene(currentStageNum + 1));
    }

    //次のステージに進む処理
    public void GotoSelectStage()
    {
        // SE
        //audioSource.PlayOneShot(start_se);

        //コルーチンを実行
        StartCoroutine(WaitForLoadScene(currentStageNum + 1));
    }

    //任意のステージに移動する処理
    public void MoveToStage(int stageNum)
    {
        //コルーチンを実行
        StartCoroutine(WaitForLoadScene(stageNum));
    }

    //シーンの読み込みと待機を行うコルーチン
    IEnumerator WaitForLoadScene(int stageNum)
    {
        //フェードオブジェクトを生成
        fadeCanvasClone = Instantiate(fadeCanvasPrefab);

        //コンポーネントを取得
        fadeCanvas = fadeCanvasClone.GetComponent<FadeManager>();

        //フェードインさせる
        fadeCanvas.fadeIn = true;

        yield return new WaitForSeconds(fadeWaitTime);

        //シーンを非同期で読込し、読み込まれるまで待機する
        yield return SceneManager.LoadSceneAsync(stageNum);

        //フェードアウトさせる
        fadeCanvas.fadeOut = true;
        // fadeCanvas.fadeReset = true;
    }

    //ゲームオーバー処理
    public void GameOver()
    {
        //キャラやカメラの移動を停止させる
        // character.enabled = false;
        game_stop_flg = true;
        pause_flg = false;

        //ゲームオーバー画面表示
        gameOverCanvasClone = Instantiate(gameOverCanvasPrefab);

        // ボタンを取得
        buttons = gameOverCanvasClone.GetComponentsInChildren<Button>();

        // ボタンにイベント設定
        buttons[0].onClick.AddListener(Retry);
        buttons[1].onClick.AddListener(Return);

    }

    public void GameClear()
    {
        game_stop_flg = true;
        pause_flg = false;

        if (stageSelectNum < stageList.Count)
        {
            resultCanvasClone = Instantiate(resultCanvasPrefab);
            resultCanvasClone.name = resultCanvasPrefab.name;
        }
        else
        {
            resultCanvasClone = Instantiate(resultCanvasPrefab_Final);
            resultCanvasClone.name = resultCanvasPrefab_Final.name;
        }
        
    }

    public void Result()
    {
        //キャラやカメラの移動を停止させる
        game_stop_flg = true;
        pause_flg = false;

        //ゲームオーバー画面表示
        resultCanvasClone = Instantiate(resultCanvasPrefab);
        //
        // ボタンを取得
        buttons = resultCanvasClone.GetComponentsInChildren<Button>();

        // ボタンにイベント設定
        buttons[0].onClick.AddListener(Retry_Result);
        buttons[1].onClick.AddListener(Return_Result);

    }

    //ポーズ処理
    public void Pause()
    {
        if (pause_flg)
        {
            //キャラやカメラの移動を停止させる
            game_stop_flg = true;
            pause_flg = false;

            //ゲームオーバー画面表示
            PauseCanvasClone = Instantiate(PauseCanvasPrefab);

            // SE
            //audioSource.PlayOneShot(pause_se);

            //ボタンを取得
            buttons = PauseCanvasClone.GetComponentsInChildren<Button>();

            //ボタンにイベント設定
            buttons[0].onClick.AddListener(Retry_Pause);
            buttons[1].onClick.AddListener(ConfilmPauseRestart);
            buttons[2].onClick.AddListener(ConfilmPauseBack);
        }

    }

    //ゲームスタート処理
    public void GameStart()
    {
        GameObject gameStartCanvas = GameObject.Find("StartCanvas");
        //ボタンを取得
        buttons = gameStartCanvas.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(GotoSelectStage);
        buttons[1].onClick.AddListener(ExitGame);

    }

    //ゲームセレクト処理
    public void SelectStart()
    {
        GameObject gameSelectCanvas = GameObject.Find("SelectCanvas");
        //ボタンを取得
        buttons = gameSelectCanvas.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(ConfilmStageSelect);
        buttons[1].onClick.AddListener(Return);
    }

    public void ConfilmStageSelect()
    {
        game_stop_flg = true;
        Confilm = Instantiate(ConfilmCanvasPrefab_StageSelect);

        Button[] buttons = Confilm.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(GoToStage);
        buttons[1].onClick.AddListener(SelectNo);
    }

    public void ConfilmPauseRestart()
    {
        game_stop_flg = true;
        Confilm = Instantiate(ConfilmCanvasPrefab_Pause_Restart);

        Button[] buttons = Confilm.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(Restart);
        buttons[1].onClick.AddListener(SelectNo_2);
    }

    public void ConfilmPauseBack()
    {
        game_stop_flg = true;
        Confilm = Instantiate(ConfilmCanvasPrefab_Pause_Back);

        Button[] buttons = Confilm.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(Return_Pause);
        buttons[1].onClick.AddListener(SelectNo_2);
    }

    public void ConfilmNextStage()
    {
        game_stop_flg = true;
        Confilm = Instantiate(ConfilmCanvasPrefab_NextStage);

        Button[] buttons = Confilm.GetComponentsInChildren<Button>();

        //ボタンにイベント設定
        buttons[0].onClick.AddListener(NextStage);
        buttons[1].onClick.AddListener(SelectNo_2);
    }

    public void GoToStage()
    {
        Destroy(Confilm);
        MoveToStage(currentStageNum + stageSelectNum);
    }

    public void SelectNo()
    {
        Destroy(Confilm);
        game_stop_flg = false;
    }

    public void SelectNo_2()
    {
        Destroy(Confilm);
    }

    //リトライ
    public void Retry()
    {
        Destroy(gameOverCanvasClone);

        // SE
        //audioSource.PlayOneShot(positive_se);

        MoveToStage(currentStageNum);
    }

    public void Retry_Result()
    {
        Destroy(resultCanvasClone);

        // SE
        //audioSource.PlayOneShot(positive_se);

        MoveToStage(currentStageNum);
    }

    //最初のシーンに戻る
    public void Return()
    {
        Destroy(gameOverCanvasClone);

        // SE
        //audioSource.PlayOneShot(negative_se);

        MoveToStage(0);
    }

    public void Return_Result()
    {
        Destroy(resultCanvasClone);

        // SE
        //audioSource.PlayOneShot(negative_se);

        currentStageNum = 0;

        MoveToStage(currentStageNum);
    }

    // リスタート
    public void Restart()
    {
        Destroy(PauseCanvasClone);
        Destroy(Confilm);
        // SE
        //audioSource.PlayOneShot(positive_se);
        MoveToStage(currentStageNum);
    }

    //リトライ
    public void Retry_Pause()
    {
        Destroy(PauseCanvasClone);
        // SE
        //audioSource.PlayOneShot(positive_se);
        game_stop_flg = false;
        pause_flg = true;
    }

    //最初のシーンに戻る
    public void Return_Pause()
    {
        Destroy(PauseCanvasClone);
        Destroy(Confilm);
        // SE
        //audioSource.PlayOneShot(negative_se);

        MoveToStage(1);
    }

    //ゲーム終了
    public void ExitGame()
    {
        // SE
        //audioSource.PlayOneShot(negative_se);
        Application.Quit();
    }
}

