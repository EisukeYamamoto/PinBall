using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HPSystem : MonoBehaviour
{
    public float hp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mallet"))
        {
            float afterHp = hp - 1;
            DOTween.To(() => hp, num => hp = num, afterHp, 0.1f);
            //hp -= 1;
            if(afterHp <= 0)
            {
                if(this.gameObject.name == "Target")
                {
                    PhaseManager phase = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
                    if (phase.phaseNow < phase.phaseMax)
                    {
                        phase.PinballClear();
                    }
                    else
                    {
                        phase.StageClear();
                    }
                }
                this.gameObject.SetActive(false);
            }
        }
    }
}
