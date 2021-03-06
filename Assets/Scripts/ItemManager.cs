﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameObject Player;
    GameObject Mallet;

    public GameObject ItemHolders;
    ItemHoldersManager holders_item;

    public GameObject AreaHolders;
    ItemHoldersManager holders_area;

    public bool _panelBottunPushed;  // true:パネルボタン/false:アイテムボタン

    MalletMotion m_motion;

    PhaseManager phase;

    [Header("このステージの地面")]
    public List<GameObject> groundList = new List<GameObject>();

    [Header("アイテムの種類")]
    public List<GameObject> itemSeries = new List<GameObject>();
    public List<GameObject> itemIconSeries = new List<GameObject>();

    [Header("パネルの種類")]
    public List<GameObject> panelSeries = new List<GameObject>();
    public List<GameObject> panelIconSeries = new List<GameObject>();

    [Header("現在ステージに存在しているアイテム/パネル")]
    public List<GameObject> existItem = new List<GameObject>();
    public List<GameObject> existPanel = new List<GameObject>();

    [Header("現在取得しているアイテム/パネル")]
    public List<GameObject> itemList = new List<GameObject>();
    public List<GameObject> panelList = new List<GameObject>();

    [Header("報酬で得たアイテム/パネル")]
    public int rewardItemNum;
    public int rewardPanelNum;
    public int canSelectNum;
    public List<GameObject> rewardList = new List<GameObject>();
    public List<GameObject> selectList = new List<GameObject>();

    [Header("ターゲットとの最小距離(これより近いとリストに戻す)")]
    public float distanceLimit;
    public GameObject alertPanel;
    public AudioSource audioSource;
    public AudioClip boo_se;
    private Vector2 hidePos;

    [Header("出現するリストの位置")]
    public Vector2 appearPos;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        //PMFind();
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        holders_item = ItemHolders.GetComponent<ItemHoldersManager>();
        holders_area = AreaHolders.GetComponent<ItemHoldersManager>();
        audioSource = GetComponent<AudioSource>();
        _panelBottunPushed = true;
        //appearPos = new Vector3(-315, -20, 0);
        hidePos = new Vector3(-800, -20, 0);
        GameObject ItemIconClone = Instantiate(itemIconSeries[0]);
        ItemIconClone.name = itemIconSeries[0].name;
        itemList.Insert(0, ItemIconClone);
        GameObject PanelIconClone = Instantiate(panelIconSeries[0]);
        PanelIconClone.name = panelIconSeries[0].name;
        panelList.Insert(0, PanelIconClone);
    }

    // Update is called once per frame
    void Update()
    {
        ItemCheck();
        PanelCheck();
        ExistItemCheck();
    }



    void ItemCheck()
    {
        if (itemList.Count >= 1)
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                var icon = itemList[i].gameObject.GetComponent<IconManager>();
                if (!icon._draging)
                {
                    itemList[i].gameObject.transform.position =
                    holders_item.itemHolderList[i].gameObject.transform.position;     
                }
                if (icon._installaction)
                {
                    GameObject itemClone = FindFromSeriesWithIcon(itemList[i], itemIconSeries, itemSeries);
                    existItem.Add(itemClone);
                    itemClone.transform.position = itemList[i].transform.position;
                    if(icon.alreadyEditObject != null)
                    {
                        GameObject ItemIconClone = FindFromSeriesWithIcon(icon.alreadyEditObject, itemSeries, itemIconSeries);
                        existItem.Remove(icon.alreadyEditObject.gameObject);
                        itemClone.transform.parent = icon.alreadyEditObject.transform.parent;
                        Destroy(icon.alreadyEditObject);
                        itemList[i].gameObject.SetActive(false);
                        itemList.RemoveAt(i);
                        itemList.Insert(0, ItemIconClone);
                    }
                    else
                    {
                        itemClone.transform.parent = icon.itemSpace.transform;

                        itemList[i].gameObject.SetActive(false);
                        itemList.RemoveAt(i);
                    }   
                }
            }
        }
    }

    void PanelCheck()
    {
        if (panelList.Count >= 1)
        {
            for (int i = panelList.Count - 1; i >= 0; i--)
            {
                var icon = panelList[i].gameObject.GetComponent<IconManager>();
                if (!icon._draging)
                {
                    panelList[i].gameObject.transform.position =
                    holders_area.itemHolderList[i].gameObject.transform.position;
                }
                if (icon._installaction)
                {
                    if (icon.groundNum < groundList.Count)
                    {
                        GroundColliderSwitch(icon.groundNum, false);
                    }
                    GameObject PanelClone = FindFromSeriesWithIcon(panelList[i], panelIconSeries, panelSeries);
                    existPanel.Add(PanelClone);
                    PanelClone.transform.position = panelList[i].gameObject.transform.position;

                    if (icon.alreadyEditObject != null)
                    {
                        GameObject PanelIconClone = FindFromSeriesWithIcon(icon.alreadyEditObject, panelSeries, panelIconSeries);
                        ItemReset(icon.alreadyEditObject);
                        existPanel.Remove(icon.alreadyEditObject.gameObject);
                        icon.alreadyEditObject.SetActive(false);
                        panelList[i].gameObject.SetActive(false);
                        panelList.RemoveAt(i);
                        panelList.Insert(0, PanelIconClone);
                        
                    }
                    else
                    {
                        panelList[i].gameObject.SetActive(false);
                        panelList.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void ExistItemCheck()
    {
        if (existItem.Count > 0)
        {
            Vector2 targetPos = phase.targetPos[phase.phaseNow - 1];
            ItemTriggerSwitch(phase._stageEditPhase);
            for (int i = existItem.Count - 1; i >= 0; i--)
            {
                GameObject Item = existItem[i];
                var icon = Item.GetComponent<IconManager>();
                float distance = Vector2.Distance(Item.transform.position, targetPos);
                if (!icon._draging)
                {
                    Item.transform.localPosition = new Vector2(0, 0);
                    if(distance < distanceLimit)
                    {
                        GameObject ItemIconClone = FindFromSeriesWithIcon(Item, itemSeries, itemIconSeries);
                        existItem.Remove(Item);
                        Destroy(Item);
                        audioSource.PlayOneShot(boo_se);
                        ListAddChack(itemList, holders_item.itemHolderList);
                        itemList.Insert(0, ItemIconClone);
                        StartCoroutine(Alert());
                    }
                }
                if (icon._installaction)
                {
                    Debug.Log(icon.alreadyEditObject);
                    if (icon.alreadyEditObject != null)
                    {
                        GameObject ItemIconClone = FindFromSeriesWithIcon(icon.alreadyEditObject, itemSeries, itemIconSeries);
                        existItem.Remove(icon.alreadyEditObject.gameObject);
                        Debug.Log(icon.gameObject.transform.parent);
                        Item.transform.parent = icon.alreadyEditObject.transform.parent;
                        Destroy(icon.alreadyEditObject);
                        ListAddChack(itemList, holders_item.itemHolderList);
                        itemList.Insert(0, ItemIconClone);
                    }
                    else if (icon.itemSpace != null)
                    {
                        Item.transform.parent = icon.itemSpace.transform;
                        
                    }
                    else
                    {
                        icon._installaction = false;
                    }
                }
                //if (distance < distanceLimit && !icon._draging)
                //{
                //    GameObject ItemIconClone = FindFromSeriesWithIcon(Item, itemSeries, itemIconSeries);
                //    existItem.Remove(Item);
                //    Destroy(Item);
                //    audioSource.PlayOneShot(boo_se);
                //    ListAddChack(itemList, holders_item.itemHolderList);
                //    itemList.Insert(0, ItemIconClone);
                //    StartCoroutine(Alert());
                //}
            }
        }
    }

    public void PMFind()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
        //Mallet = GameObject.FindGameObjectWithTag("Mallet");
        //m_motion = Mallet.GetComponent<MalletMotion>();
    }

    public void RewardListGenarate()
    {
        for (int i = 0 ; i < rewardPanelNum ; i++)
        {
            int randomValue = Random.Range(0, panelIconSeries.Count);
            GameObject panelIconClone = Instantiate(panelIconSeries[randomValue]);
            panelIconClone.name = panelIconSeries[randomValue].name;
            rewardList.Add(panelIconClone);
        }
        for (int i = 0; i < rewardItemNum; i++)
        {
            int randomValue = Random.Range(0, itemIconSeries.Count);
            GameObject itemIconClone = Instantiate(itemIconSeries[randomValue]);
            itemIconClone.name = itemIconSeries[randomValue].name;
            rewardList.Add(itemIconClone);
        }
    }

    public void AddReward()
    {
        foreach(GameObject Reward in selectList)
        {
            if(Reward.gameObject.tag == "PanelIcon")
            {
                ListAddChack(panelList, holders_area.itemHolderList);
                panelList.Insert(0, FindFromSeries(Reward, panelIconSeries));
            }
            else
            {
                ListAddChack(itemList, holders_item.itemHolderList);
                itemList.Insert(0, FindFromSeries(Reward, itemIconSeries));
            }
        }
        selectList.Clear();
        rewardList.Clear();
    }

    public void PanelActive()
    {
        if (phase._stageEditPhase)
        {
            AreaHolders.transform.localPosition = appearPos;
            ItemHolders.transform.localPosition = hidePos;
            _panelBottunPushed = true;
        }
    }

    public void ItemActive()
    {
        if (phase._stageEditPhase)
        {
            AreaHolders.transform.localPosition = hidePos;
            ItemHolders.transform.localPosition = appearPos;
            _panelBottunPushed = false;
        }
    }

    private void ItemReset(GameObject already)
    {
        //Debug.Log(already.transform.childCount);
        foreach(Transform itemSpace in already.transform)
        {
            if (itemSpace.childCount > 0)
            {
                foreach (Transform item in itemSpace)
                {
                    GameObject itemIconClone = FindFromSeriesWithIcon(item.gameObject, itemSeries, itemIconSeries);
                    existItem.Remove(item.gameObject);
                    ListAddChack(itemList, holders_item.itemHolderList);
                    itemList.Insert(0, itemIconClone);
                }
            }   
        }
    }

    public void PanelColliderSwitch(bool _flag)
    {
        foreach (GameObject Panel in existPanel)
        {
            Panel.GetComponent<BoxCollider2D>().enabled = _flag;
        }
    }

    public void ItemTriggerSwitch(bool _flag)
    {
        foreach (GameObject Item in existItem)
        {
            if(Item.name == "Slingshot")
            {
                Item.GetComponent<EdgeCollider2D>().isTrigger = _flag;
            }
            else if(Item.name == "Spinner")
            {
                Item.GetComponent<CircleCollider2D>().isTrigger = _flag;
                Item.GetComponentInChildren<EdgeCollider2D>().enabled　= !_flag;
            }
            else
            {
                Item.GetComponent<CircleCollider2D>().isTrigger = _flag;
            }
            
        }
    }


    public void GroundColliderSwitchAll(bool _flag)
    {
        foreach(GameObject Ground in groundList)
        {
            Ground.GetComponent<BoxCollider2D>().enabled = _flag;
        }
    }

    public void GroundColliderSwitch(int num, bool _flag)
    {
        groundList[num].GetComponent<BoxCollider2D>().enabled = _flag;
    }

    private GameObject FindFromSeries(GameObject A, List<GameObject> Series)
    {
        GameObject Target = null;
        foreach(GameObject B in Series)
        {
            if (A.gameObject.name == B.gameObject.name)
            {
                Target = Instantiate(B);
                Target.name = B.name;
                break;
            }
        }
        return Target;
    }

    private GameObject FindFromSeriesWithIcon(GameObject A, List<GameObject> Icon, List<GameObject> Series)
    {
        GameObject Target = null;
        for(int i = 0; i < Icon.Count; i++)
        {
            if (A.gameObject.name == Icon[i].gameObject.name)
            {
                Target = Instantiate(Series[i]);
                Target.name = Series[i].name;
                break;
            }
        }
        return Target;
    }

    public void ExistPanelChack(bool _flag)
    {
   
        foreach(GameObject ExistPanel in existPanel)
        {
            foreach(GameObject Ground in groundList)
            {
                if(ExistPanel.transform.position == Ground.transform.position)
                {
                    Ground.GetComponent<BoxCollider2D>().enabled = _flag;
                    break;
                }
            }

            //foreach(Transform ItemHole in ExistPanel.transform)
            //{
            //    if(ItemHole.gameObject.transform.childCount > 0)
            //    {
            //        ItemHole.gameObject.GetComponent<CircleCollider2D>().enabled = false;

            //    }
            //    else
            //    {
            //        ItemHole.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            //    }
            //}
        }
    }

    private void ListAddChack(List<GameObject> list, List<GameObject> holdlist)
    {
        if(list.Count >= holdlist.Count)
        {
            list[list.Count - 1].SetActive(false);
            list.RemoveAt(list.Count - 1);
        }
    }

    public IEnumerator Alert()
    {
        GameObject alert = Instantiate(alertPanel);

        yield return new WaitForSeconds(2.0f);

        Destroy(alert);
    }
    
}
