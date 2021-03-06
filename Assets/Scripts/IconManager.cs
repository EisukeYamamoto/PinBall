﻿using UnityEngine;
using System.Collections;

public class IconManager: MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private Vector2 prevPos;
    private Vector2 spacePos;
    private bool _inItemSpace;
    public bool _installaction;
    public bool _draging;

    public GameObject alreadyEditObject;
    public GameObject itemSpace;
    public int groundNum;

    PhaseManager phase;
    ItemManager itemManager;
    GameManager gameManager;

    void Start()
    {
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prevPos = this.transform.position;
        _inItemSpace = false;
        _installaction = false;
        _draging = false;
    }

    void OnMouseDown()
    {
        if (phase._stageEditPhase)
        {
            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    void OnMouseDrag()
    {
        if (!gameManager.game_stop_flg)
        {
            if (phase._stageEditPhase)
            {
                Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
                this.transform.position = currentPosition;
                _draging = true;
                if (this.transform.gameObject.tag == "PanelIcon")
                {
                    itemManager.PanelColliderSwitch(true);
                }
                else
                {
                    itemManager.PanelColliderSwitch(false);
                }



            }
        }

         
    }

    void OnMouseUp()
    {
        if (phase._stageEditPhase)
        {
            if (this.gameObject.CompareTag("PanelIcon"))
            {
                if (_inItemSpace)
                {
                    this.transform.position = spacePos;
                    _installaction = true;
                    StartCoroutine(MouseUpLimit());

                }
                else
                {
                    float distance = Vector2.Distance(this.transform.position, phase.targetPos[phase.phaseNow - 1]);
                    if (distance < itemManager.distanceLimit)
                    {
                        itemManager.audioSource.PlayOneShot(itemManager.boo_se);
                        StartCoroutine(itemManager.Alert());
                    }

                    this.transform.position = prevPos;
                    _draging = false;
                    _inItemSpace = false;
                }
            }
            else
            {
                if (_inItemSpace && (itemSpace != null || alreadyEditObject != null))
                {
                    this.transform.position = spacePos;
                    _installaction = true;
                    StartCoroutine(MouseUpLimit());

                }
                else
                {
                    float distance = Vector2.Distance(this.transform.position, phase.targetPos[phase.phaseNow - 1]);
                    if (distance < itemManager.distanceLimit)
                    {
                        itemManager.audioSource.PlayOneShot(itemManager.boo_se);
                        StartCoroutine(itemManager.Alert());
                    }

                    this.transform.position = prevPos;
                    _draging = false;
                    _inItemSpace = false;
                }
            }

            
        }   
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (phase._stageEditPhase)
        {
            //Debug.Log(this.gameObject.tag);
            if (this.gameObject.CompareTag("PanelIcon"))
            {
                if (_fieldChack(collision))
                {
                    _inItemSpace = true;
                    spacePos = collision.gameObject.transform.position;
                    groundNum = itemManager.groundList.IndexOf(collision.gameObject);
                }
                else if (collision.gameObject.CompareTag("Panel"))
                {
                    _inItemSpace = true;
                    spacePos = collision.gameObject.transform.position;
                    alreadyEditObject = collision.gameObject;
                    groundNum = 100;
                    //Debug.Log(alreadyEditObject.name);
                }
            }
            else if (this.gameObject.CompareTag("ItemIcon"))
            {
                if (collision.gameObject.CompareTag("ItemSpace"))
                {
                    _inItemSpace = true;
                    spacePos = collision.gameObject.transform.position;
                    itemSpace = collision.gameObject;
                }
                else if (_itemChack(collision))
                {
                    _inItemSpace = true;
                    spacePos = collision.gameObject.transform.position;
                    alreadyEditObject = collision.gameObject;
                    Debug.Log(alreadyEditObject);
                }
            }
            else
            {
                Debug.Log(collision.gameObject.name);
                if (_draging)
                {
                    if (collision.gameObject.CompareTag("ItemSpace"))
                    {
                        _inItemSpace = true;
                        spacePos = collision.gameObject.transform.position;
                        itemSpace = collision.gameObject;
                    }
                    else if (_itemChack(collision))
                    {
                        _inItemSpace = true;
                        spacePos = collision.gameObject.transform.position;
                        alreadyEditObject = collision.gameObject;
                        //Debug.Log(alreadyEditObject);
                    }
                }
                
            }   
        } 
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (phase._stageEditPhase)
        {
            if (this.gameObject.CompareTag("PanelIcon"))
            {
                if(_fieldChack(collision) || collision.gameObject.CompareTag("Panel"))
                {
                    _inItemSpace = false;
                    alreadyEditObject = null;
                    groundNum = 100;
                    //spacePos = collision.gameObject.transform.position;
                }
            }
            else if (this.gameObject.CompareTag("ItemIcon"))
            {
                if (collision.gameObject.CompareTag("ItemSpace"))
                {
                    _inItemSpace = false;
                    alreadyEditObject = null;
                }
                else if (_itemChack(collision))
                {
                    _inItemSpace = false;
                    alreadyEditObject = null;
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("ItemSpace"))
                {
                    _inItemSpace = false;
                    alreadyEditObject = null;
                }
                else if (_itemChack(collision))
                {
                    _inItemSpace = false;
                    alreadyEditObject = null;
                }
            }
        }        
    }

    private bool _fieldChack(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Grass":
            case "Volcano":
            case "Snow":
                return true;
            default:
                return false;
        }
    }

    private bool _itemChack(Collider2D collision)
    {
        bool hit = false;
        foreach (GameObject item in itemManager.itemSeries)
        {
            if(collision.gameObject.name == item.name)
            {               
                hit = true;
                break;
            }
        }
        return hit;
    }

    IEnumerator MouseUpLimit()
    {
        yield return new WaitForEndOfFrame();

        yield return new WaitForEndOfFrame();

        itemManager.PanelColliderSwitch(false);
        _installaction = false;
    }

}