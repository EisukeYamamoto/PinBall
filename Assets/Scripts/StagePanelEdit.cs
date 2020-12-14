using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanelEdit : MonoBehaviour
{
    public GameObject itemSpace;
    [Header("アイテム穴の個数及びそれぞれの座標 : x: -3 ~ 3, y: -1 ~ 1")]
    public Vector2[] vectors;
    private float shrink = 0.67f;
    private float expand = 0.67f;
    private List<GameObject> itemSpaceList = new List<GameObject>();

    private Vector2 parent;
    private Vector2 afterPos;

    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "PanelIcon")
        {
            shrink = 0.67f;
            expand = 1f;
            
        }
        else
        {
            shrink = 1f;
            expand = 0.67f;
        }

        this.transform.localScale *= shrink;
        
        parent = this.transform.localPosition;
        foreach(Vector2 vec in vectors)
        {
            afterPos = vec / expand; // * shrink * shrink;
            GameObject itemSpaceClone = Instantiate(itemSpace);
            itemSpaceClone.transform.parent = this.transform;
            itemSpaceList.Add(itemSpaceClone);
            itemSpaceClone.transform.localScale *= shrink;
            itemSpaceClone.transform.localPosition = afterPos;
        }
    }

    void Update()
    {

    }
}
