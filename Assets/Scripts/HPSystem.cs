using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSystem : MonoBehaviour
{
    public int hp;

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
            hp -= 1;
            if(hp <= 0)
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
