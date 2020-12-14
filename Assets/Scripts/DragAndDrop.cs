using UnityEngine;
using System.Collections;

public class DragAndDrop: MonoBehaviour
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

    PhaseManager phase;

    void Start()
    {
        phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
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
        if (phase._stageEditPhase)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
            this.transform.position = currentPosition;
            _draging = true;
        }   
    }

    void OnMouseUp()
    {
        if (phase._stageEditPhase)
        {
            if (_inItemSpace)
            {
                this.transform.position = spacePos;
                _installaction = true;
            }
            else
            {
                this.transform.position = prevPos;
                _draging = false;
            }
        }   
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (phase._stageEditPhase)
        {
            if (this.gameObject.CompareTag("PanelIcon"))
            {
                if (_fieldChack(collision))
                {
                    _inItemSpace = true;
                    //Debug.Log(collision.gameObject.name);
                    spacePos = collision.gameObject.transform.position;
                    if (collision.gameObject.CompareTag("Panel"))
                    {
                        alreadyEditObject = collision.gameObject;
                        //Debug.Log(alreadyEditObject.name);
                    }
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("ItemSpace"))
                {
                    _inItemSpace = true;
                    spacePos = collision.gameObject.transform.position;
                    itemSpace = collision.gameObject;
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
                if(_fieldChack(collision))
                {
                    _inItemSpace = false;
                    //alreadyEditObject = null;
                    //spacePos = collision.gameObject.transform.position;
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("ItemSpace"))
                {
                    _inItemSpace = false;
                    //spacePos = collision.gameObject.transform.position;
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
            case "Panel":
                return true;
            default:
                return false;
        }
    }
}