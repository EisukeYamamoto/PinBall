using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhaseManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject Mallet;
    private GameObject playerClone;
    private GameObject malletClone;

    public bool _pinballPhase;
    public bool _stageEditPhase;

    public TextMeshProUGUI message = default;
    public float waitTime = 1.0f;

    public TextMeshProUGUI phaseText = default;

    // Start is called before the first frame update
    void Awake()
    {
        _pinballPhase = false;
        _stageEditPhase = false;
        StartCoroutine(ReadyGo());
    }

    // Update is called once per frame
    void Update()
    {
        // 仮のクリア条件：Cキーを押す
        if (Input.GetKeyDown(KeyCode.C))
        {
            PinballClear();
        }

        PhaseText();
    }

    public void PinballtoStageEdit()
    {
        _pinballPhase = false;
        _stageEditPhase = true;

        Destroy(playerClone);
        Destroy(malletClone);
    }

    public void StageEdittoPinball()
    {

    }

    public void PinballClear()
    {
        StartCoroutine(ToStageEdit());
    }

    private void PhaseText()
    {
        if(_pinballPhase && !_stageEditPhase)
        {
            phaseText.text = "Pinball Phase";
        }
        else if (!_pinballPhase && _stageEditPhase)
        {
            phaseText.text = "StageEdit Phase";
        }
        else
        {
            phaseText.text = "";
        }
    }


    // ゲーム開始処理
    IEnumerator ReadyGo()
    {
        //yield return new WaitForEndOfFrame();
        playerClone = Instantiate(Player, new Vector2(0, -3f), Quaternion.identity) as GameObject;
        malletClone = Instantiate(Mallet, new Vector2(0, 1f), Quaternion.identity) as GameObject;

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
    }

    // 戦闘終了処理
    IEnumerator ToStageEdit()
    {
        yield return new WaitForEndOfFrame();

        message.text = "Great!!";
        _pinballPhase = false;

        yield return new WaitForSeconds(2.0f);

        message.text = "";

        PinballtoStageEdit();
    }
}
