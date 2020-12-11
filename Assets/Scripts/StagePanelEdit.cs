using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelEdit : MonoBehaviour
{
    public GameObject itemSpace;
    [Header("アイテム穴の個数及びそれぞれの座標 : x: -3 ~ 3, y: -1 ~ 1")]
    public Vector2[] vectors;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 parent = this.transform.position;
        foreach(Vector2 vec in vectors)
        {
            Instantiate(itemSpace, parent + vec, Quaternion.identity);
        }
    }
}
