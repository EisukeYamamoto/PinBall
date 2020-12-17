using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FadeManager : MonoBehaviour
{

    [System.NonSerialized]
    public bool fadeIn = false;
    [System.NonSerialized]
    public bool fadeOut = false;
    [System.NonSerialized]
    public bool fadeReset = false;

    [SerializeField]
    Image panelImage = default;
    [SerializeField]
    float fadeSpeed = 2.0f;

    float red, green, blue, alpha;

    //最初の処理
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //元の色を取得
        red = panelImage.color.r;
        green = panelImage.color.g;
        blue = panelImage.color.b;
        alpha = panelImage.color.a;
    }

    //毎フレームの処理
    void Update()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        else if (fadeOut)
        {
            FadeOut();
        }
        else if (fadeReset)
        {
            Destroy(gameObject);
        }
    }

    //フェードイン
    void FadeIn()
    {
        alpha += fadeSpeed;

        SetAlpha();

        if (alpha >= 1)
        {
            fadeIn = false;
        }
    }

    //フェードアウト
    void FadeOut()
    {
        alpha -= fadeSpeed;

        SetAlpha();

        if (alpha <= 0)
        {
            fadeOut = false;

            Destroy(gameObject);
        }
    }

    //透明度を変更
    void SetAlpha()
    {
        panelImage.color = new Color(red, green, blue, alpha);
    }
}

