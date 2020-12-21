using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invokeCreate : MonoBehaviour
{
    GameManager gameManager;
    public GameObject gameObject;
    public float initTime;
    public float appearTime;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(InvokeObject());
    }

    private IEnumerator InvokeObject()
    {
        yield return new WaitForSeconds(initTime);
        while (true)
        {
            if (!gameManager.game_stop_flg)
                Instantiate(gameObject, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(appearTime);
        }
    }

}
