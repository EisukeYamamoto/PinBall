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

    PhaseManager phase;
    ItemManager item;
    private float distanceLimit;

    // Start is called before the first frame update
    void Start()
    {
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        item = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        distanceLimit = item.distanceLimit;
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
            itemSpaceClone.name = itemSpace.name;
            itemSpaceClone.transform.parent = this.transform;
            itemSpaceList.Add(itemSpaceClone);
            itemSpaceClone.transform.localScale *= shrink;
            itemSpaceClone.transform.localPosition = afterPos;
        }
    }

    void Update()
    {
        if(this.gameObject.tag != "PanelIcon")
        {
            foreach (GameObject Space in itemSpaceList)
            {
                float distance = Vector2.Distance(Space.transform.position, phase.targetPos[phase.phaseNow - 1]);
                if (Space.transform.childCount > 0 || distance < distanceLimit)
                {
                    Space.GetComponent<CircleCollider2D>().enabled = false;
                }
                else
                {
                    Space.GetComponent<CircleCollider2D>().enabled = true;
                }

            }
        }
        
    }
}
