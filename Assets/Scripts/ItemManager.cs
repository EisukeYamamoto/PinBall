using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    GameObject Player;
    GameObject Mallet;

    public GameObject itemHolders;
    ItemHoldersManager itemHoldersManager;

    MalletMotion m_motion;

    PhaseManager phase;

    public GameObject bumperIcon;
    public GameObject bumper;

    public List<GameObject> itemList = new List<GameObject>();
    private bool _bumperChack = false;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Mallet = GameObject.FindGameObjectWithTag("Mallet");
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        m_motion = Mallet.GetComponent<MalletMotion>();
        itemHoldersManager = itemHolders.GetComponent<ItemHoldersManager>();
        _bumperChack = false;
    }

    // Update is called once per frame
    void Update()
    {
        ItemCheck();
    }

    void ItemCheck()
    {
        if (m_motion.bumpNum > 0 && m_motion.bumpNum % 3 == 0 && !_bumperChack &&
            itemList.Count < itemHoldersManager.itemHolderList.Count)
        {
            GameObject bumperIconClone = Instantiate(bumperIcon);
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
                    itemHoldersManager.itemHolderList[i].gameObject.transform.position;     
                }
                if (drops._installaction)
                {
                    GameObject bumperClone = Instantiate(bumper, itemList[i].gameObject.transform.position, Quaternion.identity);
                    itemList[i].gameObject.SetActive(false);
                    itemList.RemoveAt(i);
                }
            }
        }
    }
}
