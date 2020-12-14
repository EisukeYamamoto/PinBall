using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHoldersManager : MonoBehaviour
{
    public List<GameObject> itemHolderList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.transform)
        {
            itemHolderList.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
