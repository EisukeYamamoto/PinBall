using System.Collections;
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

    [Header("アイテムリスト")]
    public List<GameObject> itemSeries = new List<GameObject>();
    public List<GameObject> itemIconSeries = new List<GameObject>();

    [Header("パネルリスト")]
    public List<GameObject> panelSeries = new List<GameObject>();
    public List<GameObject> panelIconSeries = new List<GameObject>();

    public List<GameObject> itemList = new List<GameObject>();
    private bool _bumperChack = false;

    public List<GameObject> panelList = new List<GameObject>();
    private bool _panelChack = false;

    private Vector2 hidePos;
    private Vector2 appearPos;


    // Start is called before the first frame update
    void Start()
    {
        PMFind();
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        holders_item = ItemHolders.GetComponent<ItemHoldersManager>();
        holders_area = AreaHolders.GetComponent<ItemHoldersManager>();
        _panelBottunPushed = true;
        _bumperChack = false;
        appearPos = new Vector3(-315, -20, 0);
        hidePos = new Vector3(-800, -20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ItemCheck();
        PanelCheck();
    }

    void ItemCheck()
    {
        // 仮
        if (m_motion.bumpNum > 0 && m_motion.bumpNum % 3 == 0 && !_bumperChack &&
            itemList.Count < holders_item.itemHolderList.Count)
        {
            GameObject bumperIconClone = Instantiate(itemIconSeries[0]);
            itemList.Insert(0, bumperIconClone);
            _bumperChack = true;
        }
        else if(m_motion.bumpNum > 0 && m_motion.bumpNum % 3 != 0)
        {
            _bumperChack = false;
        }

        if (itemList.Count >= 1)
        {
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                var drops = itemList[i].gameObject.GetComponent<DragAndDrop>();
                if (!drops._draging)
                {
                    itemList[i].gameObject.transform.position =
                    holders_item.itemHolderList[i].gameObject.transform.position;     
                }
                if (drops._installaction)
                {
                    switch (itemList[i].gameObject.tag)
                    {
                        case "Bumper":
                            GameObject bumperClone = Instantiate(itemSeries[0], itemList[i].gameObject.transform.position, Quaternion.identity);
                            break;
                        default:
                            break;
                    }
                    
                    itemList[i].gameObject.SetActive(false);
                    itemList.RemoveAt(i);
                }
            }
        }
    }

    void PanelCheck()
    {
        // 仮
        if (m_motion.panelNum > 0 && m_motion.panelNum % 5 == 0 && !_panelChack &&
            panelList.Count < holders_area.itemHolderList.Count)
        {
            int randomValue = Random.Range(0, panelIconSeries.Count);
            GameObject panelIconClone = Instantiate(panelIconSeries[randomValue]);
            panelIconClone.name = panelIconSeries[randomValue].name;
            panelList.Insert(0, panelIconClone);
            _panelChack = true;
        }
        else if (m_motion.panelNum > 0 && m_motion.panelNum % 5 != 0)
        {
            _panelChack = false;
        }

        if (panelList.Count >= 1)
        {
            for (int i = panelList.Count - 1; i >= 0; i--)
            {
                var drops = panelList[i].gameObject.GetComponent<DragAndDrop>();
                if (!drops._draging)
                {
                    panelList[i].gameObject.transform.position =
                    holders_area.itemHolderList[i].gameObject.transform.position;
                }
                if (drops._installaction)
                {
                    switch (panelList[i].gameObject.name)
                    {
                        case "PanelIcon01":
                            GameObject panelClone01 = Instantiate(panelSeries[0]);
                            panelClone01.transform.position = panelList[i].gameObject.transform.position;
                            break;
                        case "PanelIcon02":
                            GameObject panelClone02 = Instantiate(panelSeries[1]);
                            panelClone02.transform.position = panelList[i].gameObject.transform.position;
                            break;
                        case "PanelIcon03":
                            GameObject panelClone03 = Instantiate(panelSeries[2]);
                            panelClone03.transform.position = panelList[i].gameObject.transform.position;
                            break;
                        case "PanelIcon04":
                            GameObject panelClone04 = Instantiate(panelSeries[3]);
                            panelClone04.transform.position = panelList[i].gameObject.transform.position;
                            break;
                        default:
                            break;
                    }

                    panelList[i].gameObject.SetActive(false);

                    panelList.RemoveAt(i);
                }
            }
        }
    }

    public void PMFind()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Mallet = GameObject.FindGameObjectWithTag("Mallet");
        m_motion = Mallet.GetComponent<MalletMotion>();
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


}
