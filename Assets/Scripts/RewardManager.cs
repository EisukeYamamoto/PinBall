using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public bool _selecting = true;

    ItemManager itemManager;
    public GameObject PanelHolder;
    ItemHoldersManager panelHolders;
    public GameObject ItemHolder;
    ItemHoldersManager itemHolders;

    // Start is called before the first frame update
    void Start()
    {
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        panelHolders = PanelHolder.GetComponent<ItemHoldersManager>();
        itemHolders = ItemHolder.GetComponent<ItemHoldersManager>();
        _selecting = true;

    }

    // Update is called once per frame
    void Update()
    {
        RewardManage();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit2D)
            {
                Debug.Log(hit2D.transform.gameObject);
                if (itemManager.rewardList.Contains(hit2D.transform.gameObject))
                {
                    RewardCheck(hit2D.transform.gameObject);
                }
            }   
        }       
    }

    private void RewardManage()
    {
        for (int i = 0; i < itemManager.rewardPanelNum; i++)
        {
            itemManager.rewardList[i].gameObject.transform.position =
                panelHolders.itemHolderList[i].gameObject.transform.position;
            itemManager.rewardList[i].transform.SetParent(this.transform);
        }
        for (int i = 0; i < itemManager.rewardItemNum; i++)
        {
            itemManager.rewardList[i + itemManager.rewardPanelNum].gameObject.transform.position =
                itemHolders.itemHolderList[i].gameObject.transform.position;
            itemManager.rewardList[i + itemManager.rewardPanelNum].transform.SetParent(this.transform);
        }
    }

    public void SelectFinish()
    {
        _selecting = false;
    }

    private void RewardCheck(GameObject Reward)
    {
        float expand = 1.3f;
        if (itemManager.selectList.Contains(Reward))
        {
            itemManager.selectList.Remove(Reward);
            Reward.transform.localScale /= expand;
        }
        else
        {
            if (itemManager.selectList.Count < itemManager.canSelectNum)
            {
                itemManager.selectList.Add(Reward);
                Reward.transform.localScale *= expand;
            }
        }
    }
}